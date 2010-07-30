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
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Main form for application
	/// </summary>
	public partial class MainForm : Form
	{
		/// <summary>
		/// Number of seconds for code to display when not on autorefresh
		/// </summary>
		public const int CODE_DISPLAY_DURATION = 10;

		/// <summary>
		/// Number of password attempts
		/// </summary>
		private const int MAX_PASSWORD_RETRIES = 3;

		#region Initialization

		/// <summary>
		/// Initialize form object
		/// </summary>
		public MainForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Member Data

		/// <summary>
		/// Confiuration settings for the application
		/// </summary>
		private WinAuthConfig Config { get; set; }

		/// <summary>
		/// Current loaded Authenticator object
		/// </summary>
		private Authenticator m_authenticator;

		/// <summary>
		/// Time for next code refresh
		/// </summary>
		private DateTime m_nextRefresh = DateTime.MaxValue;

		/// <summary>
		/// Time when code was displayed
		/// </summary>
		private DateTime m_codeDisplayed = DateTime.MinValue;

		#endregion

		#region Properties

		/// <summary>
		/// Get/set the file name of the authenticator's data
		/// </summary>
		public string AuthenticatorFile
		{
			get
			{
				return Config.AuthenticatorFile;
			}
			set
			{
				Config.AuthenticatorFile = value;
			}
		}

		/// <summary>
		/// Get/set the current Authenticator
		/// </summary>
		public Authenticator Authenticator
		{
			get
			{
				return m_authenticator;
			}
			set
			{
				m_authenticator = value;
			}
		}

		/// <summary>
		/// Get/set the flag to show how much time till next code and auto-generate it
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
				if (value == true)
				{
					// refresh code in case it was hidden
					ShowCode();
				}
				else
				{
					// hide code after 10 seconds
					CodeDisplayed = DateTime.Now;
				}
				progressBar.Visible = (Authenticator != null && value == true);
			}
		}

		/// <summary>
		/// Get/set flag to be topmost window
		/// </summary>
		public bool AlwaysOnTop
		{
			get
			{
				return Config.AlwaysOnTop;
			}
			set
			{
				Config.AlwaysOnTop = value;
				this.TopMost = value;
			}
		}

		/// <summary>
		/// Get/set flag to be hide serial
		/// </summary>
		public bool HideSerial
		{
			get
			{
				return Config.HideSerial;
			}
			set
			{
				Config.HideSerial = value;
				this.serialLabel.Visible = !value;
			}
		}

		/// <summary>
		/// Get/set flag to copy code to clipboard when it is generated
		/// </summary>
		public bool CopyOnCode
		{
			get
			{
				return Config.CopyOnCode;
			}
			set
			{
				Config.CopyOnCode = value;
			}
		}

		/// <summary>
		/// Get/set next auto refresh time
		/// </summary>
		public DateTime NextRefresh
		{
			get
			{
				return m_nextRefresh;
			}
			set
			{
				m_nextRefresh = value;
			}
		}

		/// <summary>
		/// Get/set time code was last displayed
		/// </summary>
		public DateTime CodeDisplayed
		{
			get
			{
				return m_codeDisplayed;
			}
			set
			{
				m_codeDisplayed = value;
			}
		}

		#endregion

		#region Private Functions

		/// <summary>
		/// Load authenticator data from a file, or prompt for a file
		/// </summary>
		/// <param name="authFile">data file name or null</param>
		private void LoadAuthenticator(string authFile)
		{
			// if no file, prompt
			if (string.IsNullOrEmpty(authFile))
			{
				string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);

				OpenFileDialog ofd = new OpenFileDialog();
				ofd.AddExtension = true;
				ofd.CheckFileExists = true;
				ofd.DefaultExt = "xml";
				if (AuthenticatorFile != null)
				{
					ofd.InitialDirectory = Path.GetDirectoryName(AuthenticatorFile);
					ofd.FileName = Path.GetFileName(AuthenticatorFile);
				}
				else
				{
					ofd.InitialDirectory = configDirectory;
					ofd.FileName = WinAuth.DEFAULT_AUTHENTICATOR_FILE_NAME;
				}
				ofd.Filter = "Authenticator Data (*.xml)|*.xml";
				ofd.RestoreDirectory = true;
				ofd.ShowReadOnly = false;
				ofd.Title = "Load Authenticator";
				DialogResult result = ofd.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return;
				}
				authFile = ofd.FileName;
			}

			// load the data
			AuthenticatorData data = null;
			try
			{
				try
				{
					data = WinAuthHelper.LoadAuthenticator(authFile);
				}
				catch (EncrpytedSecretDataException)
				{
					PasswordForm passwordForm = new PasswordForm();

					int retries = 0;
					do
					{
						passwordForm.Password = string.Empty;
						DialogResult result = passwordForm.ShowDialog(this);
						if (result != System.Windows.Forms.DialogResult.OK)
						{
							return;
						}

						try
						{
							data = WinAuthHelper.LoadAuthenticator(authFile, passwordForm.Password);
							break;
						}
						catch (BadPasswordException)
						{
							MessageBox.Show(this, "Invalid password", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							if (retries++ >= MAX_PASSWORD_RETRIES-1)
							{
								return;
							}
						}
					} while (true);
				}
			}
			catch (InvalidConfigDataException)
			{
				MessageBox.Show(this, "The authenticator file " + authFile + " is not valid", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this, "Unable to load authenticator file " + authFile + ": " + ex.Message, "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}
			if (data == null)
			{
				MessageBox.Show(this, "The file does not contain valid authenticator data.", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// set up the Authenticator
			Authenticator = new Authenticator(data);
			AuthenticatorFile = authFile;
			ShowCode();
		}

		/// <summary>
		/// Save the current Authenticator's data into a file
		/// </summary>
		/// <param name="authFile">file name to save data or prompt if null</param>
		private void SaveAuthenticator(string authFile)
		{
			if (authFile == null)
			{
				// use the user's directory to save files
				string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
				Directory.CreateDirectory(configDirectory);

				// get the config file name
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.AddExtension = true;
				sfd.CheckPathExists = true;
				sfd.DefaultExt = "xml";
				sfd.Filter = "Authenticator Data (*.xml)|*.xml";
				if (AuthenticatorFile != null)
				{
					sfd.InitialDirectory = Path.GetDirectoryName(AuthenticatorFile);
					sfd.FileName = Path.GetFileName(AuthenticatorFile);
				}
				else
				{
					sfd.InitialDirectory = configDirectory;
					sfd.FileName = WinAuth.DEFAULT_AUTHENTICATOR_FILE_NAME;
				}
				sfd.OverwritePrompt = true;
				sfd.RestoreDirectory = true;
				sfd.Title = "Save Authentication";
				DialogResult result = sfd.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return;
				}
				authFile = sfd.FileName;

				// request a password
				RequestPasswordForm requestPasswordForm = new RequestPasswordForm();
				result = requestPasswordForm.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return;
				}
				Authenticator.Data.Password = (requestPasswordForm.UsePassword == true ? requestPasswordForm.Password : null);
			}

			// save data
			try
			{
				WinAuthHelper.SaveAuthenticator(authFile, Authenticator);
				AuthenticatorFile = authFile;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,
					"There was an error writing to your authenticator file " + authFile + " (" + ex.Message + ").",
					"New Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			// update config file
			WinAuthHelper.SaveConfig(this.Config);
		}

		/// <summary>
		/// Enroll a new Authenticator
		/// </summary>
		/// <returns></returns>
		private bool Enroll()
		{
			// already have one?
			if (Authenticator != null)
			{
				DialogResult warning = MessageBox.Show(this, "WARNING: You already have an authenticator registered.\n\n"
					+ "You will NOT be able to access your Battle.net account if you continue and this authenticator is overwritten.\n\n"
					+ "Do you still want to create a new authenticator?", "New  Authenticator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if (warning != System.Windows.Forms.DialogResult.Yes)
				{
					return false;
				}
			}

			// get the region
			EnrollForm form = new EnrollForm();
			DialogResult result = form.ShowDialog(this);
			if (result != DialogResult.OK)
			{
				return false;
			}
			string region = form.SelectedRegion;

			// use the user's directory to save files
			string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
			Directory.CreateDirectory(configDirectory);

			// get the config file name
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.CheckPathExists = true;
			sfd.DefaultExt = "xml";
			sfd.Filter = "Configuration Files (*.xml)|*.xml";
			sfd.InitialDirectory = (AuthenticatorFile != null ? Path.GetDirectoryName(AuthenticatorFile) : configDirectory);
			sfd.FileName = "authenticator.xml";
			sfd.OverwritePrompt = true;
			sfd.RestoreDirectory = true;
			sfd.Title = "Save Authentication Data";
			result = sfd.ShowDialog(this);
			if (result != System.Windows.Forms.DialogResult.OK)
			{
				return false;
			}
			string configFile = sfd.FileName;

			// initialise the new authenticator
			Authenticator authenticator = null;
			try
			{
				authenticator = new Authenticator(region);
				authenticator.Enroll();
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,
					"There was an error registering your authenticator:\n\n" + ex.Message + "\n\nThis may be because the Battle.net servers are busy. Please try again later.",
					"New Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			// save config data
			try
			{
				WinAuthHelper.SaveAuthenticator(configFile, authenticator);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,
					"There was an error writing to your authenticator file " + configFile + "\n\n" + ex.Message + ".",
					"New Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			// set the new authenticator
			Authenticator = authenticator;
			AuthenticatorFile = configFile;

			// update config file
			WinAuthHelper.SaveConfig(this.Config);

			InitializedForm initForm = new InitializedForm();
			initForm.ShowDialog(this);

			//MessageBox.Show(this,
			//  "Your Authenticator has been successfully initialized.\n\nYou will need to add the follow serial number onto your account:\n\n"
			//    + m_authenticator.Data.Serial + "\n\n"
			//    + "You should also write it down as you may be asked to provide it if you ever need to remove the authenticator.",
			//  "Initailized", MessageBoxButtons.OK, MessageBoxIcon.Information);

			return true;
		}

		/// <summary>
		/// Show the current code for the Authenticator
		/// </summary>
		private void ShowCode()
		{
			// if none, we get one?
			if (Authenticator == null)
			{
				// enroll it
				if (Enroll() == false)
				{
					return;
				}
			}

			// if we have authenticaor then get code
			if (Authenticator != null)
			{
				string code = Authenticator.CalculateCode();
				codeField.Text = code;
				if (CopyOnCode == true)
				{
					Clipboard.Clear();
					Clipboard.SetDataObject(code, true);
				}

				serialLabel.Text = Authenticator.Data.Serial;
				if (HideSerial == false)
				{
					serialLabel.Visible = true;
				}
				CodeDisplayed = DateTime.Now;
			}
			else
			{
				codeField.Text = string.Empty;
				serialLabel.Text = string.Empty;
			}

			// show progess bar's time till next refresh
			refreshTimer.Enabled = true;
			progressBar.Visible = (Authenticator != null && AutoRefresh == true);
		}

		#endregion

		#region Events

		/// <summary>
		/// Load the form and initialize the controls
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Load(object sender, EventArgs e)
		{
			// load config data
			this.Config = WinAuthHelper.LoadConfig();

			// check if current authenticator exists
			string authFile = Config.AuthenticatorFile;
			Config.AuthenticatorFile = null; // clear until we confirm we loaded
			if (string.IsNullOrEmpty(authFile) == false && File.Exists(authFile) == true)
			{
				LoadAuthenticator(authFile);
				//AuthenticatorData data = WinAuthHelper.LoadAuthenticator(authFile);
				//if (data != null)
				//{
				//  this.Authenticator = new Authenticator(data);
				//  Config.AuthenticatorFile = authFile;
				//}
			}

			Authenticator authenticator = this.Authenticator;
			serialLabel.Visible = !Config.HideSerial;
			serialLabel.Text = (authenticator != null ? authenticator.Data.Serial : string.Empty);
			codeField.Text = (authenticator != null && Config.AutoRefresh == true ? authenticator.CalculateCode() : string.Empty);
			progressBar.Value = 0;
			progressBar.Visible = (authenticator != null && AutoRefresh == true);

			refreshTimer.Enabled = true;
		}

		/// <summary>
		/// Before we close save the config
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			WinAuthHelper.SaveConfig(this.Config);
		}

		/// <summary>
		/// Click the calc button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void calcCodeButton_Click(object sender, EventArgs e)
		{
			ShowCode();
		}

		/// <summary>
		/// Initialize the members when showing the menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			syncServerTimeMenuItem.Enabled = (Authenticator != null);
			copyOnCodeMenuItem.Enabled = (Authenticator != null);
			autoRefreshMenuItem.Enabled = (Authenticator != null);

			saveAsMenuItem.Enabled = (Authenticator != null);
			saveMenuItem.Enabled = (Authenticator != null);

			autoRefreshMenuItem.Checked = (autoRefreshMenuItem.Enabled == true ? AutoRefresh : false);
			copyOnCodeMenuItem.Checked = (copyOnCodeMenuItem.Enabled == true ? CopyOnCode : false);
			alwaysOnTopMenuItem.Checked = (alwaysOnTopMenuItem.Enabled == true ? AlwaysOnTop : false);
			hideSerialMenuItem.Checked = (hideSerialMenuItem.Enabled == true ? HideSerial : false);
		}

		/// <summary>
		/// Click menu item to sync time server
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void syncServerTimeMenuItem_Click(object sender, EventArgs e)
		{
			Authenticator.Sync();
			MessageBox.Show(this, "Time synced successfully.", "Sync Time", MessageBoxButtons.OK);
		}

		/// <summary>
		/// Click menu item to add new authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void registerMenuItem_Click(object sender, EventArgs e)
		{
			if (Enroll() == true)
			{
				ShowCode();
			}
		}

		/// <summary>
		/// Click menu item to load authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void loadMenuItem_Click(object sender, EventArgs e)
		{
			// load the data
			LoadAuthenticator(null);
		}

		/// <summary>
		/// Click menu item to save authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveMenuItem_Click(object sender, EventArgs e)
		{
			if (Authenticator != null)
			{
				SaveAuthenticator(AuthenticatorFile);
			}
		}

		/// <summary>
		/// Click menu item to save as Authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void saveAsMenuItem_Click(object sender, EventArgs e)
		{
			SaveAuthenticator(null);
		}

		/// <summary>
		/// Click menu item to close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitMeuuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Click menu item to toggle auto refresh
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoRefreshMenuItem_Click(object sender, EventArgs e)
		{
			AutoRefresh = !autoRefreshMenuItem.Checked;
		}

		/// <summary>
		/// Click menu item to toggle auto copy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void copyOnCodeMenuItem_Click(object sender, EventArgs e)
		{
			CopyOnCode = !copyOnCodeMenuItem.Checked;
		}

		/// <summary>
		/// Click menu item to toggle On top
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void alwaysOnTopMenuItem_Click(object sender, EventArgs e)
		{
			AlwaysOnTop = !alwaysOnTopMenuItem.Checked;
		}

		/// <summary>
		/// Click hide serial number menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void hideSerialMenuItem_Click(object sender, EventArgs e)
		{
			HideSerial = !hideSerialMenuItem.Checked;
		}

		/// <summary>
		/// Click the About menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm about = new AboutForm();
			about.ShowDialog(this);
		}

		/// <summary>
		/// Timer tick to update auto refresh and get next code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;

			if (Authenticator == null)
			{
				refreshTimer.Enabled = false;
				ShowCode();
				return;
			}

			// hide the code if it has been visible for more than 10 seconds
			if (AutoRefresh == false && CodeDisplayed != DateTime.MinValue && CodeDisplayed.AddSeconds(CODE_DISPLAY_DURATION) < now)
			{
				CodeDisplayed = DateTime.MinValue;
				codeField.Text = string.Empty;
				serialLabel.Visible = false;
			}

			if (progressBar.Visible == true)
			{
				int tillUpdate = (int)((Authenticator.ServerTime % 30000L) / 1000L);
				progressBar.Value = tillUpdate;
				if (tillUpdate == 0)
				{
					NextRefresh = now;
				}
			}
			if (AutoRefresh == true && Authenticator != null && now >= NextRefresh)
			{
				NextRefresh = now.AddSeconds(30);
				ShowCode();
			}
		}

		#endregion
	}
}
