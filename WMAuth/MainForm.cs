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
		private void ShowEnroll()
		{
			// confirm if we already have authenticator
			if (Config.CurrentAuthenticator != null)
			{
				if (MessageBox.Show("You already have an Authenticator registered.\n\nMake sure it has been removed from your Battle.net account.\n\nContinue?",
							WMAuth.APPLICATION_NAME,
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				{
					return;
				}
			}

			EnrollForm enrollForm = new EnrollForm();
			DialogResult result = enrollForm.ShowDialog();
			if (result == DialogResult.OK && enrollForm.Authenticator != null)
			{
				// save this new authenticator data
				Config.CurrentAuthenticator = enrollForm.Authenticator;
				Config.SaveAuthenticator();

				ShowCode();
			}
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
			Config.LoadAuthenticator();
			//if (Config.LoadAuthenticator() == false || Config.CurrentAuthenticator == null)
			//{
			//  this.Update();
			//  Application.DoEvents();

			//  ShowEnroll();
			//  if (Config.CurrentAuthenticator == null)
			//  {
			//    this.Close();
			//  }
			//}

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
			ShowEnroll();
		}

		/// <summary>
		/// Tick for timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTimer_Tick(object sender, EventArgs e)
		{
			// if there is no authenticator, show the enroll form
			if (Config.CurrentAuthenticator == null)
			{
				mainTimer.Enabled = false;
				ShowEnroll();
				if (Config.CurrentAuthenticator == null)
				{
					this.Close();
				}
				else
				{
					mainTimer.Enabled = true;
				}
			}

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
		}

		#endregion

	}

}