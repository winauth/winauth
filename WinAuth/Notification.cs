/*
 * Copyright (C) 2015 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

// Adapted from https://github.com/noxad/windows-toast-notifications. No license provided.
// Code originally retrieved from http://www.vbforums.com/showthread.php?t=547778 - no license information supplied

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows.Forms;

using MetroFramework.Forms;

namespace WinAuth
{
	public partial class Notification : MetroForm
	{
		/// <summary>
		/// The methods of animation available.
		/// </summary>
		public enum AnimationMethod
		{
			/// <summary>
			/// Rolls out from edge when showing and into edge when hiding
			/// </summary>
			/// <remarks>
			/// This is the default animation method and requires a direction
			/// </remarks>
			Roll = 0x0,
			/// <summary>
			/// Expands out from center when showing and collapses into center when hiding
			/// </summary>
			Center = 0x10,
			/// <summary>
			/// Slides out from edge when showing and slides into edge when hiding
			/// </summary>
			/// <remarks>
			/// Requires a direction
			/// </remarks>
			Slide = 0x40000,
			/// <summary>
			/// Fades from transaprent to opaque when showing and from opaque to transparent when hiding
			/// </summary>
			Fade = 0x80000
		}

		/// <summary>
		/// The directions in which the Roll and Slide animations can be shown
		/// </summary>
		/// <remarks>
		/// Horizontal and vertical directions can be combined to create diagonal animations
		/// </remarks>
		[Flags]
		public enum AnimationDirection
		{
			/// <summary>
			/// From left to right
			/// </summary>
			Right = 0x1,
			/// <summary>
			/// From right to left
			/// </summary>
			Left = 0x2,
			/// <summary>
			/// From top to bottom
			/// </summary>
			Down = 0x4,
			/// <summary>
			/// From bottom to top
			/// </summary>
			Up = 0x8
		}

		/// <summary>
		/// Gets the handle of the window that currently has focus.
		/// </summary>
		/// <returns>
		/// The handle of the window that currently has focus.
		/// </returns>
		[DllImport("user32")]
		internal static extern IntPtr GetForegroundWindow();

		/// <summary>
		/// Activates the specified window.
		/// </summary>
		/// <param name="hWnd">
		/// The handle of the window to be focused.
		/// </param>
		/// <returns>
		/// True if the window was focused; False otherwise.
		/// </returns>
		[DllImport("user32")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);

		/// <summary>
		/// Windows API function to animate a window.
		/// </summary>
		[DllImport("user32")]
		internal extern static bool AnimateWindow(IntPtr hWnd, int dwTime, int dwFlags);

		[DllImport("Gdi32.dll", EntryPoint = "CreateRoundRectRgn")]
		internal static extern IntPtr CreateRoundRectRgn
		(
				int nLeftRect, // x-coordinate of upper-left corner
				int nTopRect, // y-coordinate of upper-left corner
				int nRightRect, // x-coordinate of lower-right corner
				int nBottomRect, // y-coordinate of lower-right corner
				int nWidthEllipse, // width of ellipse
				int nHeightEllipse // height of ellipse
		);

		/// <summary>
		/// Animates a form when it is shown, hidden or closed
		/// </summary>
		/// <remarks>
		/// MDI child forms do not support the Fade method and only support other methods while being displayed for the first time and when closing
		/// </remarks>
		class FormAnimator
		{
			#region Constants

			/// <summary>
			/// Hide the form
			/// </summary>
			private const int AwHide = 0x10000;
			/// <summary>
			/// Activate the form
			/// </summary>
			private const int AwActivate = 0x20000;
			/// <summary>
			/// The number of milliseconds over which the animation occurs if no value is specified
			/// </summary>
			private const int DefaultDuration = 250;

			#endregion // Constants

			#region Variables

			/// <summary>
			/// The form to be animated
			/// </summary>
			private Form _form;
			/// <summary>
			/// The animation method used to show and hide the form
			/// </summary>
			private AnimationMethod _method;
			/// <summary>
			/// The direction in which to Roll or Slide the form
			/// </summary>
			private AnimationDirection _direction;
			/// <summary>
			/// The number of milliseconds over which the animation is played
			/// </summary>
			private int _duration;

			#endregion // Variables

			#region Properties

			/// <summary>
			/// Gets or sets the animation method used to show and hide the form
			/// </summary>
			/// <value>
			/// The animation method used to show and hide the form
			/// </value>
			/// <remarks>
			/// <b>Roll</b> is used by default if no method is specified
			/// </remarks>
			public AnimationMethod Method
			{
				get
				{
					return _method;
				}
				set
				{
					_method = value;
				}
			}

			/// <summary>
			/// Gets or Sets the direction in which the animation is performed
			/// </summary>
			/// <value>
			/// The direction in which the animation is performed
			/// </value>
			/// <remarks>
			/// The direction is only applicable to the <b>Roll</b> and <b>Slide</b> methods
			/// </remarks>
			public AnimationDirection Direction
			{
				get
				{
					return _direction;
				}
				set
				{
					_direction = value;
				}
			}

			/// <summary>
			/// Gets or Sets the number of milliseconds over which the animation is played
			/// </summary>
			/// <value>
			/// The number of milliseconds over which the animation is played
			/// </value>
			public int Duration
			{
				get
				{
					return _duration;
				}
				set
				{
					_duration = value;
				}
			}

			/// <summary>
			/// Gets the form to be animated
			/// </summary>
			/// <value>
			/// The form to be animated
			/// </value>
			public Form Form
			{
				get
				{
					return _form;
				}
			}

			#endregion // Properties

			#region Constructors

			/// <summary>
			/// Creates a new <b>FormAnimator</b> object for the specified form
			/// </summary>
			/// <param name="form">
			/// The form to be animated
			/// </param>
			/// <remarks>
			/// No animation will be used unless the <b>Method</b> and/or <b>Direction</b> properties are set independently. The <b>Duration</b> is set to quarter of a second by default.
			/// </remarks>
			public FormAnimator(Form form)
			{
				_form = form;

				_form.Load += Form_Load;
				_form.VisibleChanged += Form_VisibleChanged;
				_form.Closing += Form_Closing;

				_duration = DefaultDuration;
			}

			/// <summary>
			/// Creates a new <b>FormAnimator</b> object for the specified form using the specified method over the specified duration
			/// </summary>
			/// <param name="form">
			/// The form to be animated
			/// </param>
			/// <param name="method">
			/// The animation method used to show and hide the form
			/// </param>
			/// <param name="duration">
			/// The number of milliseconds over which the animation is played
			/// </param>
			/// <remarks>
			/// No animation will be used for the <b>Roll</b> or <b>Slide</b> methods unless the <b>Direction</b> property is set independently
			/// </remarks>
			public FormAnimator(Form form, AnimationMethod method, int duration) : this(form)
			{
				_method = method;
				_duration = duration;
			}

			/// <summary>
			/// Creates a new <b>FormAnimator</b> object for the specified form using the specified method in the specified direction over the specified duration
			/// </summary>
			/// <param name="form">
			/// The form to be animated
			/// </param>
			/// <param name="method">
			/// The animation method used to show and hide the form
			/// </param>
			/// <param name="direction">
			/// The direction in which to animate the form
			/// </param>
			/// <param name="duration">
			/// The number of milliseconds over which the animation is played
			/// </param>
			/// <remarks>
			/// The <i>direction</i> argument will have no effect if the <b>Center</b> or <b>Fade</b> method is
			/// specified
			/// </remarks>
			public FormAnimator(Form form, AnimationMethod method, AnimationDirection direction, int duration) : this(form, method, duration)
			{
				_direction = direction;
			}

			#endregion // Constructors

			#region Event Handlers

			/// <summary>
			/// Animates the form automatically when it is loaded
			/// </summary>
			private void Form_Load(object sender, EventArgs e)
			{
				// MDI child forms do not support transparency so do not try to use the Fade method
				if (_form.MdiParent == null || _method != AnimationMethod.Fade)
				{
					AnimateWindow(_form.Handle, _duration, AwActivate | (int)_method | (int)_direction);
				}
			}

			/// <summary>
			/// Animates the form automatically when it is shown or hidden
			/// </summary>
			private void Form_VisibleChanged(object sender, EventArgs e)
			{
				// Do not attempt to animate MDI child forms while showing or hiding as they do not behave as expected
				if (_form.MdiParent == null)
				{
					int flags = (int)_method | (int)_direction;

					if (_form.Visible)
					{
						flags = flags | AwActivate;
					}
					else
					{
						flags = flags | AwHide;
					}

					AnimateWindow(_form.Handle, _duration, flags);
				}
			}

			/// <summary>
			/// Animates the form automatically when it closes
			/// </summary>
			private void Form_Closing(object sender, CancelEventArgs e)
			{
				if (!e.Cancel)
				{
					// MDI child forms do not support transparency so do not try to use the Fade method.
					if (_form.MdiParent == null || _method != AnimationMethod.Fade)
					{
						AnimateWindow(_form.Handle, _duration, AwHide | (int)_method | (int)_direction);
					}
				}
			}

			#endregion // Event Handlers
		}

		/// <summary>
		/// List of open Notifications
		/// </summary>
		private static List<Notification> openNotifications = new List<Notification>();
		private bool _allowFocus;
		private readonly FormAnimator _animator;
		private IntPtr _currentForegroundWindow;

		/// <summary>
		/// List of buttons in Notification
		/// </summary>
		private List<NotificationButton> _buttons;

		/// <summary>
		/// Button at bottom of notification window
		/// </summary>
		public class NotificationButton
		{
			public string Text;
			public object Tag;
			public Action<Form> OnPressed;
		}

		/// <summary>
		/// Animation details
		/// </summary>
		public class NotificationAnimation
		{
			public AnimationMethod Method = AnimationMethod.Slide;
			public AnimationDirection Direction = AnimationDirection.Up;
		}

		/// <summary>
		/// Event for clicking notification area
		/// </summary>
		public EventHandler OnNotificationClicked;

		/// <summary>
		/// Create new Notification
		/// </summary>
		/// <param name="title">title text</param>
		/// <param name="body">main body, as text or HTML</param>
		/// <param name="buttons">optional list of buttons</param>
		/// <param name="animation">optional animation overrides</param>
		public Notification(string title, string body, int hideInMs = 0, List<NotificationButton> buttons = null, NotificationAnimation animation = null) : base()
		{
			InitializeComponent();

			if (animation == null)
			{
				animation = new NotificationAnimation();
			}

			lifeTimer.Interval = (hideInMs > 0 ? hideInMs : int.MaxValue);

			// set the title
			labelTitle.Text = title;

			// show or hide the buttons
			_buttons = buttons;
			if (_buttons == null || _buttons.Count == 0)
			{
				buttonPanel.Visible = false;
			}
			else
			{
				buttonPanel.Visible = true;
				this.Height += buttonPanel.Height;
				this.labelBody.Height -= buttonPanel.Height;
				this.htmlBody.Height -= buttonPanel.Height;

				if (_buttons.Count >= 1)
				{
					button1.Text = _buttons[0].Text;
					button1.Tag = _buttons[0];
				}
				if (_buttons.Count >= 2)
				{
					button2.Text = _buttons[1].Text;
					button2.Tag = _buttons[1];
					button2.Visible = true;
				}
				if (_buttons.Count >= 3)
				{
					button3.Text = _buttons[2].Text;
					button3.Tag = _buttons[2];
					button3.Visible = true;
				}
			}

			// set the body text or html
			if (body.IndexOf("<") == -1)
			{
				labelBody.Text = body;
				labelBody.Visible = true;
			}
			else
			{
				// inject browser
				TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel browser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
				browser.Dock = DockStyle.Fill;
				browser.Location = new Point(0, 0);
				browser.Size = new Size(htmlBody.Width, htmlBody.Height);
				htmlBody.Controls.Add(browser);

				browser.Text = "<!doctype html><html><head><meta charset=\"UTF-8\"><style>html,body{width:100%;height:100%;margin:0;padding:0;}body{margin:0 5px;font-size:14px;border:1px;}h1{font-size:16px;font-weight:normal;margin:0;padding:0 0 4px 0;}</style></head><body>" + body + "</body></html>";
				browser.AutoScroll = false;
				browser.IsSelectionEnabled = false;

				browser.Click += Notification_Click;

				htmlBody.Visible = true;
			}

			if (OnNotificationClicked != null)
			{
				this.labelBody.Cursor = Cursors.Hand;
				this.htmlBody.Cursor = Cursors.Hand;
			}

			_animator = new FormAnimator(this, animation.Method, animation.Direction, 500);

			//Region = Region.FromHrgn(CreateRoundRectRgn(0, 0, Width - 5, Height - 5, 5, 5));
		}

		#region Methods

		/// <summary>
		/// Displays the form
		/// </summary>
		/// <remarks>
		/// Required to allow the form to determine the current foreground window before being displayed
		/// </remarks>
		public new void Show()
		{
			// Determine the current foreground window so it can be reactivated each time this form tries to get the focus
			_currentForegroundWindow = GetForegroundWindow();

			base.Show();
		}

		#endregion // Methods

		#region Event Handlers

		private void Notification_Load(object sender, EventArgs e)
		{
			// Display the form just above the system tray.
			Location = new Point(Screen.PrimaryScreen.WorkingArea.Width - Width,
																Screen.PrimaryScreen.WorkingArea.Height - Height);

			// Move each open form upwards to make room for this one
			foreach (Notification openForm in openNotifications)
			{
				openForm.Top -= Height;
			}

			openNotifications.Add(this);
			lifeTimer.Start();
		}

		private void Notification_Activated(object sender, EventArgs e)
		{
			// Prevent the form taking focus when it is initially shown
			if (!_allowFocus)
			{
				// Activate the window that previously had focus
				SetForegroundWindow(_currentForegroundWindow);
			}
		}

		private void Notification_Shown(object sender, EventArgs e)
		{
			// Once the animation has completed the form can receive focus
			_allowFocus = true;

			// Close the form by sliding down.
			_animator.Duration = 0;
			_animator.Direction = AnimationDirection.Down;
		}

		private void Notification_FormClosed(object sender, FormClosedEventArgs e)
		{
			// Move down any open forms above this one
			foreach (Notification openForm in openNotifications)
			{
				if (openForm == this)
				{
					// Remaining forms are below this one
					break;
				}
				openForm.Top += Height;
			}

			openNotifications.Remove(this);
		}

		private void lifeTimer_Tick(object sender, EventArgs e)
		{
			Close();
		}

		private void Notification_Click(object sender, EventArgs e)
		{
			Close();

			if (OnNotificationClicked != null)
			{
				OnNotificationClicked(this, e);
			}
		}

		#endregion // Event Handlers

		private void button1_Click(object sender, EventArgs e)
		{
			var button = button1.Tag as NotificationButton;
			if (button != null && button.OnPressed != null)
			{
				button.OnPressed(this);
			}
		}

		private void button2_Click(object sender, EventArgs e)
		{
			var button = button2.Tag as NotificationButton;
			if (button != null && button.OnPressed != null)
			{
				button.OnPressed(this);
			}
		}

		private void button3_Click(object sender, EventArgs e)
		{
			var button = button3.Tag as NotificationButton;
			if (button != null && button.OnPressed != null)
			{
				button.OnPressed(this);
			}
		}


		protected override void OnPaint(PaintEventArgs e)
		{
			base.OnPaint(e);

		//	using (var pen = new Pen(Color.FromArgb(32, 32, 32), 1))
		//	{
		//		e.Graphics.DrawRectangle(pen, 0, 0, this.Width, this.Height);
		//	}
		}

		private void Notification_Paint(object sender, PaintEventArgs e)
		{
		}

		private void closeLink_Click(object sender, EventArgs e)
		{
			Close();
		}
	}
}