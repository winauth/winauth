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
using System.Globalization;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Threading;
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
		//public const int CODE_DISPLAY_DURATION = 10;

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
		/// Configuration data for authenticator and app settings
		/// </summary>
		private WinAuthConfig m_config;

		/// <summary>
		/// Time for next code refresh
		/// </summary>
		private DateTime m_nextRefresh = DateTime.MaxValue;

		/// <summary>
		/// Time when code was displayed
		/// </summary>
		private DateTime m_codeDisplayed = DateTime.MinValue;

		/// <summary>
		/// Hook for hotkey to send code to window
		/// </summary>
		private KeyboardHook m_hook;

		/// <summary>
		/// Flag to say we are processing sending message to other window
		/// </summary>
		private object m_sendingKeys = new object();

		/// <summary>
		/// Flag to use when we explicitly closing from menu rather than just close window
		/// </summary>
		private bool m_explictClose;

		/// <summary>
		/// Flag that the clipboard is erroring and we don't display any more error messages
		/// </summary>
		private bool m_ignoreClipboard;

		#endregion

		#region Properties

		/// <summary>
		/// Configuration settings for the application (including the Authenticator data)
		/// </summary>
		private WinAuthConfig Config
		{
			get
			{
				return m_config;
			}
			set
			{
				m_config = value;

				// when we set a new config also force any necessary form changes
				this.AlwaysOnTop = m_config.AlwaysOnTop;
				//this.AutoRefresh = m_config.AutoRefresh;
				this.UseTrayIcon = m_config.UseTrayIcon;
				this.HideSerial = m_config.HideSerial;
				this.AllowCopy = m_config.AllowCopy;
			}
		}

		/// <summary>
		/// Get/set the current Authenticator
		/// </summary>
		public Authenticator Authenticator
		{
			get
			{
				return Config.Authenticator;
			}
			set
			{
				Config.Authenticator = value;
			}
		}

/*
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
*/

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
		/// Get/set flag to use tray icon
		/// </summary>
		public bool UseTrayIcon
		{
			get
			{
				return Config.UseTrayIcon;
			}
			set
			{
				Config.UseTrayIcon = value;
				if (value == false && this.Visible == false)
				{
					BringToFront();
					Show();
					WindowState = FormWindowState.Normal;
					Activate();
				}
				notifyIcon.Visible = value;
			}
		}

		/// <summary>
		/// Get/set flag to set start with Windows
		/// </summary>
		public bool StartWithWindows
		{
			get
			{
				return Config.StartWithWindows;
			}
			set
			{
				Config.StartWithWindows = value;
				WinAuthHelper.SetStartWithWindows(value);
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
		/// Get/set flag to allow copy of code
		/// </summary>
		public bool AllowCopy
		{
			get
			{
				return Config.AllowCopy;
			}
			set
			{
				Config.AllowCopy = value;
				codeField.SecretMode = !value;
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
		/// Load a new authenticator by prompting for a file
		/// </summary>
		private void LoadAuthenticator(string configFile)
		{
			if (string.IsNullOrEmpty(configFile) == true)
			{
				// use default or the path of the current authenticator
				string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
				configFile = WinAuthHelper.DEFAULT_AUTHENTICATOR_FILE_NAME;
				if (string.IsNullOrEmpty(Config.Filename) == false)
				{
					configFile = Path.GetFileName(Config.Filename);
					configDirectory = Path.GetDirectoryName(Config.Filename);
				}

				OpenFileDialog ofd = new OpenFileDialog();
				ofd.AddExtension = true;
				ofd.CheckFileExists = true;
				ofd.DefaultExt = "xml";
				ofd.InitialDirectory = configDirectory;
				ofd.FileName = configFile;
				ofd.Filter = "WinAuth Authenticator (*.xml)|*.xml|Java BMA File (*.rs;*.rms)|*.rs;*.rms|All Files (*.*)|*.*";
				ofd.RestoreDirectory = true;
				ofd.ShowReadOnly = false;
				ofd.Title = "Load Authenticator";
				DialogResult result = ofd.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return;
				}
				configFile = ofd.FileName;
			}
			if (File.Exists(configFile) == false)
			{
				return;
			}

			// if the file is xml, could be our 1.4 config, 1.3 authenticator or android. Otherwise is an import
			WinAuthConfig config = WinAuthHelper.LoadConfig(this, configFile, null);
			Authenticator auth = (config != null && config.Authenticator != null ? config.Authenticator : null);
			if (config != null && auth != null)
			{
				// if there is no filename, we imported a different authenticator, so clone some of the current config
				if (string.IsNullOrEmpty(config.Filename) == true)
				{
					// set specific authenticator settings
					config.AllowCopy = Config.AllowCopy;
					config.AlwaysOnTop = Config.AlwaysOnTop;
					//config.AutoRefresh = Config.AutoRefresh;
					config.CopyOnCode = Config.CopyOnCode;
					config.HideSerial = Config.HideSerial;
					config.UseTrayIcon = Config.UseTrayIcon;
					
					// set the filename
					config.Filename = configFile;
				}

				// if this was an import, i.e. an .rms file, then clear authFile so we are forced to save a new name
				if (auth.LoadedFormat != Authenticator.FileFormat.WinAuth)
				{
					config.Filename = null;
				}

				// set up the Authenticator
				Config = config;
				// unhook and rehook hotkey
				HookHotkey(this.Config);
				// show the new code
				ShowCode();

				// re-save
				SaveAuthenticator(Config.Filename);

				// set title
				notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);
			}
		}

		/// <summary>
		/// Save the current authenticator into a file
		/// </summary>
		/// <param name="configFile">file to save or null to prompt</param>
		/// <returns>true if saved</returns>
		private bool SaveAuthenticator(string configFile)
		{
			if (string.IsNullOrEmpty(configFile) == true)
			{
				// use the user's directory to save files
				string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
				configFile = WinAuthHelper.DEFAULT_AUTHENTICATOR_FILE_NAME;
				if (string.IsNullOrEmpty(Config.Filename) == false)
				{
					configFile = Path.GetFileName(Config.Filename);
					configDirectory = Path.GetDirectoryName(Config.Filename);
				}
				Directory.CreateDirectory(configDirectory);

				// get the config file name
				SaveFileDialog sfd = new SaveFileDialog();
				sfd.AddExtension = true;
				sfd.CheckPathExists = true;
				sfd.DefaultExt = "xml";
				sfd.Filter = "Authenticator Data (*.xml)|*.xml";
				sfd.InitialDirectory = configDirectory;
				sfd.FileName = configFile;
				sfd.OverwritePrompt = true;
				sfd.RestoreDirectory = true;
				sfd.Title = "Save Authentication";
				DialogResult result = sfd.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return false;
				}
				configFile = sfd.FileName;

				// request a password
				RequestPasswordForm requestPasswordForm = new RequestPasswordForm();
				result = requestPasswordForm.ShowDialog(this);
				if (result != System.Windows.Forms.DialogResult.OK)
				{
					return false;
				}
				Config.Authenticator.PasswordType = requestPasswordForm.PasswordType;
				Config.Authenticator.Password = (requestPasswordForm.PasswordType == Authenticator.PasswordTypes.Explicit ? requestPasswordForm.Password : null);
			}

			// save data
			try
			{
				WinAuthHelper.SaveAuthenticator(this, configFile, Config);
				Config.Filename = configFile;
				notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);
				return true;
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,
					"There was an error writing to your authenticator file " + configFile + " (" + ex.Message + ").",
					"New Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}
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

			// initialise the new authenticator
			Authenticator authenticator = null;
			try
			{
				// get the current country code
				string countrycode = (RegionInfo.CurrentRegion != null ? RegionInfo.CurrentRegion.TwoLetterISORegionName : null);
	
				// create and enroll a new authenticator
				authenticator = new Authenticator();
				authenticator.Enroll(countrycode);
			}
			catch (Exception ex)
			{
				MessageBox.Show(this,
					"There was an error registering your authenticator:\n\n" + ex.Message + "\n\nThis may be because the Battle.net servers are unavailable. Please try again later.",
					"New Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return false;
			}

			// prompt now done
			if (MessageBox.Show(this,
				"Your authenticator has been created and registered.\n\n"
				+ "You will now be prompted to save it to a file on your computer before you can add it to your account.\n\n"
				+ "1. Choose a file to save your new authenticator.\n"
				+ "2. Select the encrpytion option.\n"
				+ "3. Add your authenticator to your Battle.net account.\n\n"
				+ "Click \"OK\" to save your authenticator.",
				"New Registred Authenticator", MessageBoxButtons.OKCancel, MessageBoxIcon.Information) != DialogResult.OK)
			{
				return false;
			}

			// remember old config
			WinAuthConfig oldconfig = Config;
			// set the new authenticator
			Config = Config.Clone() as WinAuthConfig;
			Config.Authenticator = authenticator;
			Config.AutoLogin = null; // clear autologin
			// save config data
			if (SaveAuthenticator(null) == false)
			{
				// restore authenticator
				Config = oldconfig;
				return false;
			}

			// unhook and rehook hotkey
			HookHotkey(this.Config);

			// set filename and window title
			notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);

			// prompt to backup
			InitializedForm initForm = new InitializedForm();
			initForm.Authenticator = Config.Authenticator;
			if (initForm.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
			{
				BackupData();
			}

			// show the new code
			ShowCode();

			return true;
		}

		/// <summary>
		/// Show the form to backup the data
		/// </summary>
		private void BackupData()
		{
			BackupForm backup = new BackupForm();
			//backup.CurrentAuthenticator = this.Authenticator;
			backup.CurrentAuthenticatorFile = Config.Filename;
			backup.CurrentConfig = this.Config;
			backup.ShowDialog(this);
		}

		/// <summary>
		/// Show the form to import a key
		/// </summary>
		private bool ImportKey()
		{
			// already have one?
			if (Authenticator != null)
			{
				DialogResult warning = MessageBox.Show(this, "WARNING: You already have an authenticator registered.\n\n"
					+ "You will NOT be able to access your Battle.net account if you continue and this authenticator is overwritten.\n\n"
					+ "Do you still want to import an authenticator?", "Import Authenticator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if (warning != System.Windows.Forms.DialogResult.Yes)
				{
					return false;
				}
			}

			// get the import
			ImportForm import = new ImportForm();
			if (import.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
			{
				return false;
			}

			// remember old
			WinAuthConfig oldconfig = Config;
			// set the new authenticator
			Config = Config.Clone() as WinAuthConfig;
			Config.Authenticator = import.Authenticator;
			Config.AutoLogin = null;
			// save config data
			if (SaveAuthenticator(null) == false)
			{
				// restore auth
				Config = oldconfig;
				return false;
			}

			// unhook and rehook hotkey
			HookHotkey(this.Config);

			// set filename and window title
			notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);

			// prompt to backup
			InitializedForm initForm = new InitializedForm();
			initForm.Authenticator = Config.Authenticator;
			if (initForm.ShowDialog(this) == System.Windows.Forms.DialogResult.Yes)
			{
				BackupData();
			}

			// show new code and serial
			ShowCode();

			return true;
		}

		/// <summary>
		/// Show the form to export a key
		/// </summary>
		private void ExportKey()
		{
			if (MessageBox.Show(this, "This will display your unencrypted authenticator secret key." + Environment.NewLine + Environment.NewLine +
						"Are you sure you want to continue?",
						WinAuth.APPLICATION_NAME,
						MessageBoxButtons.YesNo,
						MessageBoxIcon.Warning,
						MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
			{
				return;
			}

			// get the form
			ExportForm export = new ExportForm();
			export.Authenticator = this.Authenticator;
			if (export.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
			{
				return;
			}
		}

		/// <summary>
		/// Show the restore code. The form also verifiies it with the servers in case it is an old authenticator.
		/// </summary>
		private void ShowRestoreCode()
		{
			// already have one?
			if (Authenticator == null)
			{
				return;
			}

			ShowRestoreCodeForm restorecodeform = new ShowRestoreCodeForm();
			restorecodeform.Authenticator = this.Authenticator;
			restorecodeform.ShowDialog(this);
		}

		/// <summary>
		/// Show the form to restore the authenticator
		/// </summary>
		private bool RestoreAuthenticator()
		{
			// already have one?
			if (Authenticator != null)
			{
				DialogResult warning = MessageBox.Show(this, "WARNING: You already have an authenticator registered.\n\n"
					+ "You will NOT be able to access your Battle.net account if you continue and this authenticator is overwritten.\n\n"
					+ "Do you still want to restore an authenticator?", "Restore Authenticator", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
				if (warning != System.Windows.Forms.DialogResult.Yes)
				{
					return false;
				}
			}

			// get the restore form
			RestoreForm restore = new RestoreForm();
			if (restore.ShowDialog(this) != System.Windows.Forms.DialogResult.OK)
			{
				return false;
			}

			// remember old
			WinAuthConfig oldconfig = Config;
			// set the new authenticator
			Config = Config.Clone() as WinAuthConfig;
			Config.Authenticator = restore.Authenticator;
			Config.AutoLogin = null;
			// save config data
			if (SaveAuthenticator(null) == false)
			{
				// restore auth
				Config = oldconfig;
				return false;
			}

			// unhook and rehook hotkey
			HookHotkey(this.Config);

			// set filename and window title
			notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);

			// show new code and serial
			ShowCode();

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
				if (MessageBox.Show(this,
					"Since this seems to be your first time we will connect to Battle.net and register a new authenticator.\n\nIf you already have an authenticator, you can cancel this and right-click to either \"Load...\", \"Import...\" or \"Restore...\" your existing one.\n\nRegister a new authenticator now?",
					"Register New Authenticator", MessageBoxButtons.YesNo, MessageBoxIcon.Question) != DialogResult.Yes)
				{
					return;
				}
				// enroll it
				if (Enroll() == false)
				{
					return;
				}
			}

			// if we have authenticaor then get code
			if (Authenticator != null)
			{
				string code = Authenticator.CurrentCode;
				codeField.Text = code;

				// optionally copy the code to the clipboard, only when not minimized
				if (CopyOnCode == true && m_ignoreClipboard == false && WindowState != FormWindowState.Minimized)
				{
					bool clipRetry = false;
					do
					{
						try
						{
							Clipboard.Clear();
							Clipboard.SetDataObject(code, true);
						}
						catch (ExternalException )
						{
							// only show an error the first time
							clipRetry = (MessageBox.Show(this, "Unable to copy your code to the clipboard. Another application is probably using it.\n\nTry again?",
								WinAuth.APPLICATION_NAME,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
							if (clipRetry == false)
							{
								// dont show error again...gets annoying
								m_ignoreClipboard = true;
							}
						}
					}
					while (clipRetry == true);
				}

				serialLabel.Text = Authenticator.Serial;
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
			progressBar.Visible = (Authenticator != null /* && AutoRefresh == true */);
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
			// get any command arguments
			string configFile = null;
			string password = null;
			//
			string[] args = Environment.GetCommandLineArgs();
			for (int i = 1; i < args.Length; i++)
			{
				string arg = args[i];
				if (arg[0] == '-')
				{
					switch (arg)
					{
						case "-min":
						case "--minimize":
							// set initial state as minimized
							this.WindowState = FormWindowState.Minimized;
							break;
						case "-p":
						case "--password":
							// set explicit password to use
							i++;
							password = args[i];
							break;
						default:
							break;
					}
				}
				else
				{
					configFile = arg;
				}
			}
			// load config data
			this.Config = WinAuthHelper.LoadConfig(this, configFile, password);
			if (this.Config == null)
			{
				System.Diagnostics.Process.GetCurrentProcess().Kill();
				return;
			}

			// set title
			notifyIcon.Text = this.Text = WinAuth.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);

			Authenticator authenticator = this.Authenticator;
			serialLabel.Visible = !Config.HideSerial;
			serialLabel.Text = (authenticator != null ? authenticator.Serial : string.Empty);
			codeField.Text = (authenticator != null /* && Config.AutoRefresh == true */ ? authenticator.CurrentCode : string.Empty);
			codeField.SecretMode = !AllowCopy;
			progressBar.Value = 0;
			progressBar.Visible = (authenticator != null /* && AutoRefresh == true */);

			// hook our hotkey to send code to target window (e.g . Ctrl-Alt-C)
			HookHotkey(this.Config);

			// finally enable the timer to show code changes
			refreshTimer.Enabled = true;			
		}

		private void UnhookHotkey()
		{
			// remove the hotkey hook
			if (m_hook != null)
			{
				m_hook.UnHook();
				m_hook = null;
			}
		}

		/// <summary>
		/// Hook the hot key for the authenticator
		/// </summary>
		/// <param name="config">current config settings</param>
		private void HookHotkey(WinAuthConfig config)
		{
			// unhook any old hotkey
			UnhookHotkey();

			// hook new hotkey
			if (config != null && config.AutoLogin != null)
			{
				Dictionary<Keys, WinAPI.KeyModifiers> keys = new Dictionary<Keys, WinAPI.KeyModifiers>();
				keys.Add((Keys)config.AutoLogin.HotKey, config.AutoLogin.Modifiers);
				m_hook = new KeyboardHook(keys);
				m_hook.KeyDown += new KeyboardHook.KeyboardHookEventHandler(Hotkey_KeyDown);
			}
		}

		/// <summary>
		/// A hotkey keyboard event occured, e.g. "Ctrl-Alt-C"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Hotkey_KeyDown(object sender, KeyboardHookEventArgs e)
		{
			// avoid multiple keypresses being sent
			if (Monitor.TryEnter(m_sendingKeys) == true)
			{
				try
				{
					// get keyboard sender
					KeyboardSender keysend = new KeyboardSender(this.Config.AutoLogin.WindowTitle, this.Config.AutoLogin.ProcessName, this.Config.AutoLogin.WindowTitleRegex);

					// get the current code
					string code = Authenticator.CurrentCode;

					// get the script and execute it
					string script = (string.IsNullOrEmpty(this.Config.AutoLogin.AdvancedScript) == false ? this.Config.AutoLogin.AdvancedScript : "{CODE}{ENTER 4000}");

					// replace any {CODE} items
					script = script.Replace("{CODE}", code);

					// send the whole script
					keysend.SendKeys(script);

					// mark event as handled
					e.Handled = true;
				}
				finally
				{
					Monitor.Exit(m_sendingKeys);
				}
			}
		}

		/// <summary>
		/// Before we close save the config
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// save current config
			if (Config.Authenticator != null)
			{
				SaveAuthenticator(Config.Filename);
			}

			// keep in the tray when closing Form 
			if (UseTrayIcon == true && this.Visible == true && m_explictClose == false)
			{
				e.Cancel = true;
				notifyIcon.Visible = true;
				notifyIcon.Text = this.Text;
				Hide();
				return;
			}

			// remove the hotkey hook
			UnhookHotkey();

			// ensure the notify icon is closed
			notifyIcon.Visible = false;
		}

		/// <summary>
		/// Form event called on first show
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Shown(object sender, EventArgs e)
		{
			// if we use tray icon make sure it is set
			if (UseTrayIcon == true)
			{
				notifyIcon.Visible = true;
				notifyIcon.Text = this.Text;

				// if initially minizied, we need to hide
				if (WindowState == FormWindowState.Minimized)
				{
					Hide();
				}
			}
		}

		/// <summary>
		/// Form resize event to save authenticator when we mininize
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void MainForm_Resize(object sender, EventArgs e)
		{
			// save current config
			if (this.WindowState == FormWindowState.Minimized && Config != null && Config.Authenticator != null)
			{
				SaveAuthenticator(Config.Filename);
			}
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
			openMenuItem.Visible = (UseTrayIcon == true && this.Visible == false);
			openMenuItemSeparator.Visible = (UseTrayIcon == true && this.Visible == false);

			syncServerTimeMenuItem.Enabled = (Authenticator != null);
			copyOnCodeMenuItem.Enabled = (Authenticator != null);
			allowCopyMenuItem.Enabled = (Authenticator != null);
			//autoRefreshMenuItem.Enabled = (Authenticator != null);

			saveAsMenuItem.Enabled = (Authenticator != null);
			saveMenuItem.Enabled = (Authenticator != null);

			//autoRefreshMenuItem.Checked = (autoRefreshMenuItem.Enabled == true ? AutoRefresh : false);
			copyOnCodeMenuItem.Checked = (copyOnCodeMenuItem.Enabled == true ? CopyOnCode : false);
			allowCopyMenuItem.Checked = (allowCopyMenuItem.Enabled == true ? AllowCopy : false);
			alwaysOnTopMenuItem.Checked = (alwaysOnTopMenuItem.Enabled == true ? AlwaysOnTop : false);
			hideSerialMenuItem.Checked = (hideSerialMenuItem.Enabled == true ? HideSerial : false);
			useTrayIconMenuItem.Checked = (useTrayIconMenuItem.Enabled == true ? UseTrayIcon : false);
			startWithWindowsMenuItem.Checked = (startWithWindowsMenuItem.Enabled == true ? StartWithWindows : false);

			// check we have the separator
			if (contextMenuStrip.Items.ContainsKey("lastLoadedMenuItemSep") == false)
			{
				int exitIndex = contextMenuStrip.Items.IndexOfKey("exitMenuItem");
				ToolStripSeparator sep = new ToolStripSeparator();
				sep.Name = "lastLoadedMenuItemSep";
				contextMenuStrip.Items.Insert(exitIndex, sep);
			}
			// remove old last loaded items
			int nextindex;
			int index = 1;
			while ((nextindex = contextMenuStrip.Items.IndexOfKey("lastLoadedMenuItem" + index)) >= 0)
			{
				contextMenuStrip.Items.RemoveAt(nextindex);
				index++;
			}
			// add the last loaded after the separator
			index = 1;
			nextindex = contextMenuStrip.Items.IndexOfKey("lastLoadedMenuItemSep");
			do
			{
				string lastloaded = WinAuthHelper.GetLastFile(index);
				if (string.IsNullOrEmpty(lastloaded) == true)
				{
					break;
				}

				ToolStripMenuItem item = new ToolStripMenuItem();
				item.Name = "lastLoadedMenuItem" + index;
				item.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.D1 + (index-1))));
				item.Text = index + " " + lastloaded;
				item.Tag = lastloaded;
				item.Click += new System.EventHandler(this.lastloadedMenuItem_Click);
				contextMenuStrip.Items.Insert(nextindex, item);

				index++;
				nextindex++;
			} while (true);
		}

		/// <summary>
		/// Click the Open menu item when on icon tray
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openMenuItem_Click(object sender, EventArgs e)
		{
			// show the main form
			BringToFront();
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
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
			//if (Authenticator != null)
			//{
			//  SaveAuthenticator(Config.Filename);
			//}
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
		/// Click to create a backup authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void createBackupMenuItem_Click(object sender, EventArgs e)
		{
			BackupData();
		}

		/// <summary>
		/// Click the import menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importMenuItem_Click(object sender, EventArgs e)
		{
			ImportKey();
		}

		/// <summary>
		/// Click the Export menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exportKeyMenuItem_Click(object sender, EventArgs e)
		{
			ExportKey();
		}

		/// <summary>
		/// Click menu item to show the restore code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void showRestoreCodeMenuItem_Click(object sender, EventArgs e)
		{
			ShowRestoreCode();
		}

		/// <summary>
		/// Click the Restore menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void restoreMenuItem_Click(object sender, EventArgs e)
		{
			RestoreAuthenticator();
		}

		/// <summary>
		/// Click menu item to close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitMenuItem_Click(object sender, EventArgs e)
		{
			// we nede to make sure we aren't just hidden to tray
			m_explictClose = true;
			this.Close();
		}

		/// <summary>
		/// Click menu item to load a lastloaded authenticator file
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void lastloadedMenuItem_Click(object sender, EventArgs e)
		{
			// get the filename of the previous authenticator and load it
			string filename = ((ToolStripMenuItem)sender).Tag as string;
			if (string.IsNullOrEmpty(filename) == false)
			{
				LoadAuthenticator(filename);
			}
		}

		/// <summary>
		/// Click the AutoLogin menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoLoginMenuItem_Click(object sender, EventArgs e)
		{
			AutoLoginForm options = new AutoLoginForm();
			options.Sequence = this.Config.AutoLogin;
			if (options.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				this.Config.AutoLogin = options.Sequence;

				// remove the old hook
				UnhookHotkey();

				// install the new hook
				HookHotkey(this.Config);

				// save
				SaveAuthenticator(Config.Filename);
			}
		}

		/// <summary>
		/// Click menu item to toggle auto refresh
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoRefreshMenuItem_Click(object sender, EventArgs e)
		{
		  //AutoRefresh = !autoRefreshMenuItem.Checked;
		}

		/// <summary>
		/// Click menu item to toggle allow copy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void allowCopyMenuItem_Click(object sender, EventArgs e)
		{
			AllowCopy = !allowCopyMenuItem.Checked;
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
		/// Click menu item to toggle UseTrayIcon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void useTrayIconMenuItem_Click(object sender, EventArgs e)
		{
			UseTrayIcon = !useTrayIconMenuItem.Checked;
		}

		/// <summary>
		/// Click the item to toggle start with Windows
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void startWithWindowsMenuItem_Click(object sender, EventArgs e)
		{
			StartWithWindows = !startWithWindowsMenuItem.Checked;
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
			//if (AutoRefresh == false && CodeDisplayed != DateTime.MinValue && CodeDisplayed.AddSeconds(CODE_DISPLAY_DURATION) < now)
			//{
			//  CodeDisplayed = DateTime.MinValue;
			//  codeField.Text = string.Empty;
			//  serialLabel.Visible = false;
			//}

			if (progressBar.Visible == true)
			{
				int tillUpdate = (int)((Authenticator.ServerTime % 30000L) / 1000L);
				progressBar.Value = tillUpdate;
				if (tillUpdate == 0)
				{
					NextRefresh = now;
				}
			}
			if (/* AutoRefresh == true && */ Authenticator != null && now >= NextRefresh)
			{
				NextRefresh = now.AddSeconds(30);
				ShowCode();
			}
		}

		/// <summary>
		/// Double click the notify icon to restore the app
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			BringToFront();
			Show();
			WindowState = FormWindowState.Normal;
			Activate();

			// hide icon
			//notifyIcon.Visible = false;
		}

		#endregion

	}
}
