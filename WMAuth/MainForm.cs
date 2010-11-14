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
using System.Reflection;
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
		/// Let the user browse for the BMA datafile and we load the key from it
		/// </summary>
		private bool ImportBMA()
		{
			if (MessageBox.Show("You need to find the Battle.net Mobile Authenticator token file (e.g. \"\\\\JavaFX\\Java\\0000#Token#Record.db\")\n\nContinue?",
						WMAuth.APPLICATION_NAME,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question,
						MessageBoxDefaultButton.Button1) != DialogResult.Yes)
			{
				return false;
			}

			// browse for the Midlet file
			OpenFileDialogFTFY ofd = new OpenFileDialogFTFY();
			ofd.Filter = "*.*|All Files (*.*)";
			DialogResult result = ofd.ShowDialog();
			if (result != DialogResult.OK || string.IsNullOrEmpty(ofd.FileName) == true)
			{
				return false;
			}

			// read the file data for the key
			Authenticator auth = null;
			string filename = ofd.FileName;
			using (FileStream fs = new FileStream(filename, FileMode.Open))
			{
				try
				{
					AuthenticatorData data = new AuthenticatorData(fs, AuthenticatorData.FileFormat.Midp, null);
					auth = new Authenticator(data);
				}
				catch (InvalidConfigDataException )
				{
					MessageBox.Show("Unable to extract key information from this file.", WMAuth.APPLICATION_NAME);
					return false;
				}
				catch (Exception ex)
				{
					MessageBox.Show("Error whilst reading this file: " + ex.Message, WMAuth.APPLICATION_NAME);
					return false;
				}
			}

			// confirm if we already have authenticator
			if (Config.CurrentAuthenticator != null)
			{
				if (MessageBox.Show("You already have an Authenticator registered.\n\nMake sure it has been removed from your Battle.net account.\n\nContinue with Import?",
							WMAuth.APPLICATION_NAME,
							MessageBoxButtons.YesNo,
							MessageBoxIcon.Question,
							MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				{
					return false;
				}
			}

			// save this new authenticator data
			Config.CurrentAuthenticator = auth;
			Config.SaveAuthenticator();

			// show code
			ShowCode();

			MessageBox.Show("The Authenticator has been successfully imported.", WMAuth.APPLICATION_NAME);

			return true;
		}

		/// <summary>
		/// Let the user export the current key in an unecnrypted state
		/// </summary>
		private bool ExportBMA()
		{
			if (MessageBox.Show("Select the location to save an unencrypted copy of your authenticator.\n\nContinue?",
						WMAuth.APPLICATION_NAME,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Question,
						MessageBoxDefaultButton.Button1) != DialogResult.Yes)
			{
				return false;
			}

			// browse for the Midlet file
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.FileName = "authenticator.xml";
			sfd.Filter = "WinAuth XML File|*.xml";
			DialogResult result = sfd.ShowDialog();
			if (result != DialogResult.OK || string.IsNullOrEmpty(sfd.FileName) == true)
			{
				return false;
			}

			// write the file data for the key
			string filename = sfd.FileName;
			Directory.CreateDirectory(Path.GetDirectoryName(filename));

			// save a copy of the authenticator data
			AuthenticatorData data = (AuthenticatorData)Config.CurrentAuthenticator.Data.Clone();
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.CloseOutput = true;
			settings.Indent = true;
			using (XmlWriter xw = XmlWriter.Create(new FileStream(filename, FileMode.Create), settings))
			{
				data.PasswordType = AuthenticatorData.PasswordTypes.None;

				// save the data
				data.WriteXmlString(xw);
			}

			return true;
		}

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

			// get the new authenticator or import BMA
			EnrollForm enrollForm = null;
			do
			{
				enrollForm = new EnrollForm();
				DialogResult result = enrollForm.ShowDialog();
				if (result == DialogResult.Cancel)
				{
					break;
				}

				if (enrollForm.ImportBMA == true)
				{
					ImportBMA();
				}
				else
				{
					// save this new authenticator data
					Config.CurrentAuthenticator = enrollForm.Authenticator;
					Config.SaveAuthenticator();

					// first time we show the serial
					ShowSerial = true;
					// and the intro
					introPanel.Visible = true;
					ShowCode();
				}
			} while (Config.CurrentAuthenticator == null);
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
			// set title
			this.Text = WMAuth.APPLICATION_NAME;

			// default config
			AutoRefresh = true;
			ShowSerial = false;

			// load authenticator else enroll a new one
			Config.LoadAuthenticator();

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
		/// Click the load the Battle.net Mobile Authenticator key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void loadBmaMenuItem_Click(object sender, EventArgs e)
		{
			ImportBMA();
		}

		/// <summary>
		/// Click the export BMA menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exportBmaMenuItem_Click(object sender, EventArgs e)
		{
			ExportBMA();
		}

		/// <summary>
		/// Click the Sync menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void syncMenuItem_Click(object sender, EventArgs e)
		{
			if (Config.CurrentAuthenticator != null)
			{
				try
				{
					// sync and save
					Config.CurrentAuthenticator.Sync();
					Config.SaveAuthenticator();

					MessageBox.Show("Authenticator synced successfully.", WMAuth.APPLICATION_NAME);
				}
				catch (Exception ex)
				{
					MessageBox.Show("Cannot sync with Battle.net: " + ex.Message, WMAuth.APPLICATION_NAME);
				}
			}
		}

		/// <summary>
		/// Click the About menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutMenuItem_Click(object sender, EventArgs e)
		{
			Assembly assembly = Assembly.GetExecutingAssembly();
			MessageBox.Show(
				((AssemblyTitleAttribute)assembly.GetCustomAttributes(typeof(AssemblyTitleAttribute), false)[0]).Title + " " + assembly.GetName().Version.ToString(3) + Environment.NewLine +
				((AssemblyCopyrightAttribute)assembly.GetCustomAttributes(typeof(AssemblyCopyrightAttribute), false)[0]).Copyright + Environment.NewLine + Environment.NewLine +
				"Licensed under GNU General Public License v3 (http://www.gnu.org/licenses)" + Environment.NewLine + Environment.NewLine
				+ "http://code.google.com/p/winauth",
				WMAuth.APPLICATION_NAME);
				//MessageBoxButtons.OK, MessageBoxIcon.None, MessageBoxDefaultButton.Button1);
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