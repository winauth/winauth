/*
	* Copyright (C) 2010 Colin Mackie.
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

using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Text;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Interface defining a control as being a transparent container
	/// </summary>
	public interface ITransparentContainer
	{
		/// <summary>
		/// Get the current background image
		/// </summary>
		Image BackgroundImage { get; }
	}

	/// <summary>
	/// Base class for all transparent controls
	/// </summary>
	public class TransparentControl : Control
	{
		/// <summary>
		/// Flag if this control has a background
		/// </summary>
		protected bool Background;

		/// <summary>
		/// Overridden backgroun paint to paint the background image 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ITransparentContainer parent = this.Parent as ITransparentContainer;
			if (parent == null)
			{
				// not a transparent container
				base.OnPaintBackground(e);
				return;
			}

			// set this as being background
			Background = true;

			// draw the image
			e.Graphics.DrawImage(parent.BackgroundImage, 0, 0, this.Bounds, GraphicsUnit.Pixel);
		}

		/// <summary>
		/// If the text has changed invalidate the control
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}

		/// <summary>
		/// Invalidate control if control is moved to another parent
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			Invalidate();
		}
	}

	/// <summary>
	/// A transparent label control
	/// </summary>
	public class TransparentLabel : TransparentControl
	{
		/// <summary>
		/// Text alignment for label
		/// </summary>
		ContentAlignment m_alignment = ContentAlignment.TopLeft;

		/// <summary>
		/// Current formatting
		/// </summary>
		StringFormat m_format = new StringFormat();

		/// <summary>
		/// Background image
		/// </summary>
		Bitmap m_backImage = null;

		public Image Image { get; set; }

		/// <summary>
		/// Create a new transparent label
		/// </summary>
		public TransparentLabel()
		{
		}

		/// <summary>
		/// Get/set the text alignment
		/// </summary>
		public ContentAlignment TextAlign
		{
			get
			{
				return m_alignment;
			}
			set
			{
				m_alignment = value;

				// set the format
				switch (value)
				{
					case ContentAlignment.TopLeft:
						m_format.Alignment = StringAlignment.Near;
						m_format.LineAlignment = StringAlignment.Near;
						break;

					case ContentAlignment.TopCenter:
						m_format.Alignment = StringAlignment.Center;
						m_format.LineAlignment = StringAlignment.Center;
						break;

					case ContentAlignment.TopRight:
						m_format.Alignment = StringAlignment.Far;
						m_format.LineAlignment = StringAlignment.Far;
						break;
				}
			}
		}

		/// <summary>
		/// Draw the label
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			// build the image if we don't have one
			if (Image != null)
			{
				using (Bitmap backImage = new Bitmap(Image))
				{
					using (Graphics g = Graphics.FromImage(backImage))
					{
						using (SolidBrush brush = new SolidBrush(ForeColor))
						{
							g.DrawString(Text, Font, brush, new Rectangle(0, 0, Width, Height), m_format);
							e.Graphics.DrawImage(backImage, 0, 0);
						}
					}
				}
			}
			else if (base.Background == false)
			{
				if (m_backImage == null)
				{
					m_backImage = new Bitmap(ClientSize.Width, ClientSize.Height);
				}
				// clear the background image
				using (Graphics g = Graphics.FromImage(m_backImage))
				{
					using (SolidBrush brush = new SolidBrush(Parent.BackColor))
					{
						g.Clear(BackColor);
						g.FillRectangle(brush, ClientRectangle);
					}
				}
			}
			else
			{
				// draw the text into the context
				using (SolidBrush brush = new SolidBrush(ForeColor))
				{
					e.Graphics.DrawString(Text, Font, brush, new Rectangle(0, 0, Width, Height), m_format);
				}
			}
		}
	}

	/// <summary>
	/// A transparent label control
	/// </summary>
	public class TransparentTextBox : RichTextBox, ISecretTextBox
	{
		/// <summary>
		/// Flag to draw ourselves
		/// </summary>
		private bool m_secretMode;

		/// <summary>
		/// Our hidden text value
		/// </summary>
		private string m_text;

		/// <summary>
		/// Number of chars to space out when in secret mode
		/// </summary>
		private int m_spaceOut;

		public TransparentTextBox() : base()
		{
			this.MouseDown += new System.Windows.Forms.MouseEventHandler(this.OnMouseDown);
			this.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUp);

			this.SetStyle(ControlStyles.Opaque, true);
			this.SetStyle(ControlStyles.OptimizedDoubleBuffer, false);

			this.ReadOnlyChanged += new EventHandler(OnReadOnlyChanged);
			if (this.ReadOnly == true)
			{
				this.TabStop = false;
				WinAPI.HideCaret(this.Handle);
			}
		}

		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams parms = base.CreateParams;
				parms.ExStyle |= 0x20;  // Turn on WS_EX_TRANSPARENT
				return parms;
			}
		}

		public HorizontalAlignment TextAlign
		{
			get
			{
				return base.SelectionAlignment;
			}
			set
			{
				base.SelectionAlignment = value;
			}
		}

		public bool SecretMode
		{
			get
			{
				return m_secretMode;
			}
			set
			{
				m_secretMode = value;
				this.Enabled = !value; // we disable so cannot select/copy 
				this.SetStyle(ControlStyles.UserPaint, value);
				Text = m_text;
				this.Invalidate(); // force it to redraw
			}
		}

		public int SpaceOut
		{
			get
			{
				return m_spaceOut;
			}
			set
			{
				m_spaceOut = value;
			}
		}

		/// <summary>
		/// Value of the textbox, but if in secret mode then will return ***** as value
		/// </summary>
		public override string Text
		{
			get
			{
				return (SecretMode == true ? m_text : base.Text);
			}
			set
			{
				m_text = value;
				base.Text = (SecretMode == true ? (string.IsNullOrEmpty(value) == false ? new string('*', value.Length) : value) : value);
				this.Invalidate();
			}
		}

		/// <summary>
		/// Overriden Paint method to draw the text based on our secret value rather than Text, which might be ****'s
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			using (Brush brush = new SolidBrush(base.ForeColor))
			{
				StringFormat sf = StringFormat.GenericTypographic;
				sf.Alignment = StringAlignment.Center;
				sf.LineAlignment = StringAlignment.Center;
				g.TextRenderingHint = System.Drawing.Text.TextRenderingHint.AntiAlias;

				string text = m_text;

				// if we have spacing, we add a space in between each set of chars
				if (m_spaceOut != 0)
				{
					StringBuilder sb = new StringBuilder();
					for (int i = 0; i < m_text.Length; i += m_spaceOut)
					{
						sb.Append(m_text.Substring(i, m_spaceOut)).Append(" ");
					}
					text = sb.ToString().Trim();
				}

				// draw the whole string
				g.DrawString(text, base.Font, brush, new RectangleF(0, 0, base.Width, base.Height), sf);
			}
		}

		protected override void OnGotFocus(EventArgs e)
		{
			if (this.ReadOnly == true)
			{
				WinAPI.HideCaret(this.Handle);
			}
		}

		protected override void OnEnter(EventArgs e)
		{
			if (this.ReadOnly == true)
			{
				WinAPI.HideCaret(this.Handle);
			}
		}

		private void OnMouseDown(object sender, System.EventArgs e)
		{
			if (this.ReadOnly == true)
			{
				WinAPI.HideCaret(this.Handle);
			}
		}

		private void OnMouseUp(object sender, System.EventArgs e)
		{
			if (this.ReadOnly == true)
			{
				WinAPI.HideCaret(this.Handle);
			}
		}

		private void OnReadOnlyChanged(object sender, EventArgs e)
		{
			if (this.ReadOnly == true)
			{
				this.TabStop = false;
				WinAPI.HideCaret(this.Handle);
			}
			else
			{
				this.TabStop = true;
			}
		}
	}

	public class TransparentPanel : TransparentControl
	{
		/// <summary>
		/// Background image
		/// </summary>
		Bitmap m_backImage = null;

		/// <summary>
		/// Create a new transparent label
		/// </summary>
		public TransparentPanel()
		{
		}

		/// <summary>
		/// Draw the panel
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Background == false)
			{
				// build the image if we don't have one
				if (m_backImage == null)
				{
					m_backImage = new Bitmap(ClientSize.Width, ClientSize.Height);
				}
				// clear the background image
				using (Graphics g = Graphics.FromImage(m_backImage))
				{
					using (SolidBrush brush = new SolidBrush(Parent.BackColor))
					{
						g.Clear(BackColor);
						g.FillRectangle(brush, ClientRectangle);
					}
				}
			}
		}
	}

	public class TransparentForm : Form, ITransparentContainer
	{
		/// <summary>
		/// Create a new form
		/// </summary>
		public TransparentForm()
		{
		}

		/// <summary>
		/// Paint the form by painting the background image
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImage(BackgroundImage, 0, 0);
		}
	}

	/// <summary>
	/// Implementation of a transparent button based on a control that paints the background behind it.
	/// </summary>
	public class TransparentButton : ButtonBase, IButtonControl
	{
		/// <summary>
		/// WinAPI constants
		/// </summary>
		const int WS_EX_TRANSPARENT = 0x20;

		/// <summary>
		/// Store DialogResult property
		/// </summary>
		private DialogResult m_dialogResult;

		/// <summary>
		/// Create a new TransparenteButon and set styles
		/// </summary>
		public TransparentButton()
		{
			SetStyle(ControlStyles.SupportsTransparentBackColor, true);
			SetStyle(ControlStyles.Opaque, true);
			SetStyle(ControlStyles.ResizeRedraw, true);
			this.BackColor = Color.Transparent;
			this.Cursor = Cursors.Hand;
		}

		/// <summary>
		/// Handle the Paint by drawing the parent's paint
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaint(PaintEventArgs pevent)
		{
			if (this.Parent != null)
			{
				System.Drawing.Drawing2D.GraphicsContainer cstate = pevent.Graphics.BeginContainer();
				pevent.Graphics.TranslateTransform(-this.Left, -this.Top);
				Rectangle clip = pevent.ClipRectangle;
				clip.Offset(this.Left, this.Top);
				PaintEventArgs pe = new PaintEventArgs(pevent.Graphics, clip);

				InvokePaintBackground(this.Parent, pe);
				InvokePaint(this.Parent, pe);
				pevent.Graphics.EndContainer(cstate);
			}
			else
			{
				base.OnPaint(pevent); // or base.OnPaint(pevent);...
			}
		}

		/// <summary>
		/// Override the background paint to not do anything as we're transparent
		/// </summary>
		/// <param name="pevent"></param>
		protected override void OnPaintBackground(PaintEventArgs pevent)
		{
			// don't call the base class
			//base.OnPaintBackground(pevent);
		}

		/// <summary>
		/// Set the creation parameters
		/// </summary>
		protected override CreateParams CreateParams
		{
			get
			{
				CreateParams cp = base.CreateParams;
				cp.ExStyle |= WS_EX_TRANSPARENT;
				return cp;
			}
		}

		#region IButtonControl

		/// <summary>
		/// Handle our  DialogResult property
		/// </summary>
		public DialogResult DialogResult
		{
			get
			{
				return m_dialogResult;
			}
			set
			{
				if (Enum.IsDefined(typeof(DialogResult), value))
				{
					m_dialogResult = value;
				}
			}
		}

		/// <summary>
		/// Implement the PerformClock method to send the click event
		/// </summary>
		public void PerformClick()
		{
			if (CanSelect == true)
			{
				this.OnClick(EventArgs.Empty);
			}
		}

		/// <summary>
		/// Implement the NotifyDefault method
		/// </summary>
		/// <param name="value"></param>
		public void NotifyDefault(bool value)
		{
			if (this.IsDefault != value)
			{
				this.IsDefault = value;
			}
		}

		#endregion
	}
}
