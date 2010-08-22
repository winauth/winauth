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
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class for main authenticator form
	/// </summary>
	public partial class MainForm : TransparentForm
	{
		#region Data Members

		/// <summary>
		/// Region string for US
		/// </summary>
		private const string REGION_US = "US";

		/// <summary>
		/// Regions string for EU
		/// </summary>
		private const string REGION_EU = "EU";

		/// <summary>
		/// Current Config object
		/// </summary>
		private WinAuthConig m_config;

		#endregion

		#region Constructors

		/// <summary>
		/// Create a new form
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Get/set current config. If none, we create new one.
		/// </summary>
		public WinAuthConig Config
		{
			get
			{
				// create config if none
				if (m_config == null)
				{
					m_config = new WinAuthConig();
				}
				return m_config;
			}
			set
			{
				m_config = value;
			}
		}

		/// <summary>
		/// Get/set auto refresh flag.
		/// </summary>
		public bool AutoRefresh
		{
			get
			{
				return Config.AutoRefresh;
			}
			set
			{
				Config.AutoRefresh = value;
				autoRefreshMenuItem.Checked = value;
				timeToLiveBar.Visible = value;

				// show code if setting
				if (value == true)
				{
					ShowCode();
				}
				// always set this as latest time
				CodeShownSince = DateTime.Now;
			}
		}

		/// <summary>
		/// Get/set the show serial flag
		/// </summary>
		public bool ShowSerial
		{
			get
			{
				return Config.ShowSerial;
			}
			set
			{
				// show serial fields and labels
				Config.ShowSerial = value;
				showSerialMenuItem.Checked = value;
				serialField.Text = (value == true && Config.CurrentAuthenticator != null ? Config.CurrentAuthenticator.Data.Serial : string.Empty);
				serialField.Visible = value;
				serialLabel.Visible = value;
			}
		}

		/// <summary>
		/// Number of seconds we have been into the current code
		/// </summary>
		protected int CodeDuration
		{
			get
			{
				return (int)(Config.CurrentAuthenticator.ServerTime % 30000L);
			}
		}

		/// <summary>
		/// Last known code interval when dispayed a code
		/// </summary>
		public long LastCodeInterval {get; set;}

		/// <summary>
		/// The time the code was last shown, so it can be turned off
		/// </summary>
		public DateTime CodeShownSince { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Show/hide the enroll choice
		/// </summary>
		/// <param name="show"></param>
		private void ShowEnroll(bool show)
		{
			// confirm if we already have authenticator
			if (show == true && Config.CurrentAuthenticator != null)
			{
				if (MessageBox.Show("You already have an Authenticator registered on this device.\n\nMake sure you have removed it from your Battle.net account.\n\nContinue?",
							WMAuth.APPLICATION_NAME,
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				{
					return;
				}
			} 
			
			// set menu and show fields
			this.Menu = (show == true ? this.enrollMenu : this.mainMenu);
			enrollIntroLabel.Visible = show;
			enrollRegionPanel.Visible = show;
			UpdateEnrollStatus(string.Empty);
		}

		/// <summary>
		/// Set the status field whilst enrolling
		/// </summary>
		/// <param name="status">status text</param>
		public void UpdateEnrollStatus(string status)
		{
			enrollLabel.Text = status;
		}

		/// <summary>
		/// Update enrolling status bar
		/// </summary>
		public void UpdateEnrollProgressBar()
		{
			// get the enroller object
			Enroller enroller = enrollProgressBar.Tag as Enroller;
			if (enroller != null)
			{
				// just update the bar with number of seconds since we started, wrap if neccessary
				DateTime started = enroller.Started;
				TimeSpan diff = DateTime.Now.Subtract(started);
				int value = (diff.TotalSeconds >= 0 ? (int)diff.TotalSeconds % (enrollProgressBar.Maximum + 1) : 0);
				enrollProgressBar.Value = value;
			}
		}

		/// <summary>
		/// Begin the enrolling
		/// </summary>
		/// <param name="region">region to enroll</param>
		private void BeginEnroll(string region)
		{
			// set labels
			enrollLabel.Text = "Enrolling...";
			enrollLabel.Visible = true;
			enrollProgressBar.Visible = true;

			// create enroller and hook events
			Enroller enroller = new Enroller(region);
			enroller.StatusChanged += new Enroller.StatusChangedHandler(enroller_StatusChanged);
			enroller.Completed += new Enroller.CompleteHandler(enroller_Completed);
			enrollProgressBar.Tag = enroller;
		}

		/// <summary>
		/// Delegate to update status since enroller is on different thread
		/// </summary>
		/// <param name="status"></param>
		public delegate void UpdateEnrollStatusCallback(string status);

		/// <summary>
		/// Enroller status changed event. On different thread so we must invoke.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void enroller_StatusChanged(object sender, StatusChangedEventArgs args)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new UpdateEnrollStatusCallback(UpdateEnrollStatus), new object[] { args.Status });
			}
			else
			{
			  UpdateEnrollStatus(args.Status);
			}
		}

		/// <summary>
		/// Delegate for EndEnroll method
		/// </summary>
		/// <param name="auth"></param>
		public delegate void EndEnrollCallback(Authenticator auth);

		/// <summary>
		/// Enroller Completed veenta handler, which is on different thread so we Invoke.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void enroller_Completed(object sender, CompletedEventArgs args)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new EndEnrollCallback(EndEnroll), new object[] { args.Authenticator });
			}
			else
			{
				EndEnroll(args.Authenticator);
			}
		}

		/// <summary>
		/// End the enrolling in the GUI
		/// </summary>
		/// <param name="auth"></param>
		public void EndEnroll(Authenticator auth)
		{
			// if we have an authenticator, set it
			if (auth != null)
			{
				UpdateEnrollStatus("Saving authenticator...");

				// save this new authenticator data
				Config.CurrentAuthenticator = auth;
				Config.SaveAuthenticator();

				ShowEnroll(false);
				ShowCode();
			}

			// clear status
			UpdateEnrollStatus(string.Empty);

			// hide enrolling fields
			enrollLabel.Text = string.Empty;
			enrollLabel.Visible = false;
			enrollProgressBar.Visible = false;
			enrollProgressBar.Tag = null;
		}

		/// <summary>
		/// Show the current authenticator code
		/// </summary>
		private void ShowCode()
		{
			// if there is one
			if (Config.CurrentAuthenticator == null)
			{
				codeField.Text = string.Empty;
				return;
			}

			// show the code
			codeField.Text = Config.CurrentAuthenticator.CalculateCode();
			LastCodeInterval = Config.CurrentAuthenticator.CodeInterval;
			// update last updatted date
			CodeShownSince = DateTime.Now;
		}

		#endregion

		#region Form Events

		/// <summary>
		/// Load the form. Default settings and read authenticator.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			// default config
			AutoRefresh = true;
			ShowSerial = false;

			// load authenticator else enroll a new one
			if (Config.LoadAuthenticator() == false || Config.CurrentAuthenticator == null)
			{
				ShowEnroll(true);
			}

			// if one exist, show code
			if (Config.CurrentAuthenticator != null)
			{
				ShowCode();
			}

			// set timer for auorefresh
			mainTimer.Enabled = true;
		}

		/// <summary>
		/// Click the Refresh toggle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoRefreshMenuItem_Click(object sender, EventArgs e)
		{
			// toggle auto refresh
			AutoRefresh = !AutoRefresh;
		}

		/// <summary>
		/// Click the show serial toggle
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showSerialMenuItem_Click(object sender, EventArgs e)
		{
			ShowSerial = !ShowSerial;
		}

		/// <summary>
		/// Exit the application
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Click the New... item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void setupMenuItem_Click(object sender, EventArgs e)
		{
			// show enroll choices
			ShowEnroll(true);
		}

		/// <summary>
		/// Click the Register option to start enrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void registerMenuItem_Click(object sender, EventArgs e)
		{
			string region = (rbRegionUS.Checked == true ? REGION_US : REGION_EU);
			UpdateEnrollStatus("Registering...");
			BeginEnroll(region);
		}

		/// <summary>
		/// Click the exit whilst enrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enrollExitMenuItem_Click(object sender, EventArgs e)
		{
			// if we already had one, we just return, else exit
			if (Config.CurrentAuthenticator == null)
			{
				this.Close();
			}
			else
			{
				ShowEnroll(false);
			}
		}

		/// <summary>
		/// Tick for timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTimer_Tick(object sender, EventArgs e)
		{
			// update code and timeout
			if (AutoRefresh == true)
			{
				if (Config.CurrentAuthenticator != null)
				{
					timeToLiveBar.Value = CodeDuration / 100; // control is scaled 0-300
					if (Config.CurrentAuthenticator.CodeInterval != LastCodeInterval)
					{
						ShowCode();
					}
				}
			}
			else
			{
				// after 10 seconds we hide the code
				if (CodeShownSince.AddSeconds(10) < DateTime.Now)
				{
					codeField.Text = string.Empty;
					CodeShownSince = DateTime.MinValue;
				}
			}

			// if we are enrolling, show progress bar
			if (enrollProgressBar.Tag != null)
			{
				UpdateEnrollProgressBar();
			}
		}

		#endregion

	}

}