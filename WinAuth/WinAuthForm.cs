/*
 * Copyright (C) 2013 Colin Mackie.
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
using System.Linq;
using System.Reflection;
using System.Resources;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
#if NETFX_4
using System.Threading.Tasks;
#endif
using System.Xml;
using System.Web;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;

using NLog;

using WinAuth.Resources;
using System.Security;
using System.Net;

namespace WinAuth
{
	public partial class WinAuthForm : ResourceForm
  {
#if BETA
		/// <summary>
		/// Registry data name for beta key
		/// </summary>
		private const string WINAUTHREGKEY_BETAWARNING = @"Software\WinAuth3\BetaWarning";
#endif
		
		public WinAuthForm()
    {
      InitializeComponent();
    }

#region Properties

		/// <summary>
		/// The current winauth config
		/// </summary>
    public WinAuthConfig Config { get; set; }

		/// <summary>
		/// Datetime for when we should save config
		/// </summary>
		private DateTime? _saveConfigTime;

		/// <summary>
		/// Self-updating object
		/// </summary>
		private WinAuthUpdater Updater { get; set; }

		/// <summary>
		/// Flag used to set AutoSizing based on authenticators
		/// </summary>
		public bool AutoAuthenticatorSize { get; set; }

		/// <summary>
		/// Flag used to see if we are closing manually
		/// </summary>
		private bool m_explictClose;

		/// <summary>
		/// Hook for hotkey to send code to window
		/// </summary>
		private KeyboardHook m_hook;

		/// <summary>
		/// Handle for USB notification hook
		/// </summary>
		private IntPtr m_usbHandle;

		/// <summary>
		/// Current list of USB YubiKeys
		/// </summary>
		private List<HIDDevice.HIDDeviceEntry> m_yubis;

		/// <summary>
		/// Flag to say we are processing sending message to other window
		/// </summary>
		private object m_sendingKeys = new object();

		/// <summary>
		/// Delegates for clipbaord manipulation
		/// </summary>
		/// <param name="data"></param>
		public delegate void SetClipboardDataDelegate(object data);
		public delegate object GetClipboardDataDelegate(Type format);

		/// <summary>
		/// Save the position of the list within the form for starting as minimized
		/// </summary>
		private Rectangle _listoffset;

		/// <summary>
		/// If we were passed command line arg to minimise
		/// </summary>
		private bool _initiallyMinimised;

		/// <summary>
		/// Existing v2 config file so we can prompt for import
		/// </summary>
		private string _existingv2Config;

		private string _startupConfigFile;

		/// <summary>
		/// Forwarder for mousewheel messages to list control
		/// </summary>
		private WinAPI.MessageForwarder _wheelMessageForwarder;

		/// <summary>
		/// First time only initialzation
		/// </summary>
		private bool m_initOnce;

		/// <summary>
		/// Initial form size so we can reset
		/// </summary>
		private Size m_initialSize;

		/// <summary>
		/// Locker for WM_DEVICECHANGE
		/// </summary>
		private object m_deviceArrivalMutex = new object();

#endregion

		/// <summary>
		/// Load the main form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
    private void WinAuthForm_Load(object sender, EventArgs e)
    {
      // get any command arguments
			string password = null;
			string proxy = null;
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
							_initiallyMinimised = true;
              break;
            case "-p":
            case "--password":
              // set explicit password to use
              i++;
							password = args[i];
              break;
						case "--proxy":
							// set proxy [user[:pass]@]ip[:host]
							i++;
							proxy = args[i];
							break;
						case "-l":
						case "--log":
							i++;
							FieldInfo fi = typeof(LogLevel).GetField(args[i], BindingFlags.Static | BindingFlags.IgnoreCase | BindingFlags.Public);
							if (fi == null)
							{
								WinAuthForm.ErrorDialog(this, "Invalid parameter: log value: " + args[i] + " (must be error,info,debug,trace)");
								System.Diagnostics.Process.GetCurrentProcess().Kill();
							}
							var loglevel = fi.GetValue(null) as LogLevel;
							var target = NLog.LogManager.Configuration.AllTargets.Where(t => t.Name == null).FirstOrDefault();
							if (target != null)
							{
								LogManager.Configuration.LoggingRules.Add(new NLog.Config.LoggingRule("*", loglevel, target));
							}
							break;
						default:
              break;
          }
        }
        else
        {
					_startupConfigFile = arg;
        }
      }

			// set the default web proxy
			if (string.IsNullOrEmpty(proxy) == false)
			{
				try
				{
					Uri uri = new Uri(proxy.IndexOf("://") == -1 ? "http://" + proxy : proxy);
					WebProxy webproxy = new WebProxy(uri.Host + ":" + uri.Port, true);
					if (string.IsNullOrEmpty(uri.UserInfo) == false)
					{
						string[] auth = uri.UserInfo.Split(':');
						webproxy.Credentials = new NetworkCredential(auth[0], (auth.Length > 1 ? auth[1] : string.Empty));
					}
					WebRequest.DefaultWebProxy = webproxy;
				}
				catch (UriFormatException )
				{
					ErrorDialog(this, "Invalid proxy value (" + proxy + ")" + Environment.NewLine + Environment.NewLine + "Use --proxy [user[:password]@]ip[:port], e.g. 127.0.0.1:8080 or me:mypass@10.0.0.1:8080");
					this.Close();
				}
			}

			InitializeOnce();

			loadConfig(password);
		}

#region Private Methods

		/// <summary>
		/// Load the current config into WinAuth
		/// </summary>
		/// <param name="password">optional password to decrypt config</param>
		/// <param name="configFile">optional explicit config file</param>
		private void loadConfig(string password)
		{
			string configFile = _startupConfigFile;

			loadingPanel.Visible = true;
			passwordPanel.Visible = false;
			yubiPanel.Visible = false;

#if NETFX_4
			Task.Factory.StartNew<Tuple<WinAuthConfig, Exception>>(() =>
			{
				try
				{
					// use previous config if we have one
					WinAuthConfig config = WinAuthHelper.LoadConfig(this, configFile, password);
					return new Tuple<WinAuthConfig, Exception>(config, null);
				}
				catch (Exception ex)
				{
					return new Tuple<WinAuthConfig, Exception>(null, ex);
				}
			}).ContinueWith((configTask) =>
			{
				Exception ex = configTask.Result.Item2;
				if (ex is WinAuthInvalidNewerConfigException)
				{
					MessageBox.Show(this, ex.Message, WinAuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
					System.Diagnostics.Process.GetCurrentProcess().Kill();
					return;
				}
				else if (ex is EncryptedSecretDataException)
				{
					loadingPanel.Visible = false;
					passwordPanel.Visible = true;
					yubiPanel.Visible = false;

					this.passwordButton.Focus();
					this.passwordField.Focus();

					return;
				}
				else if (ex is BadYubiKeyException)
				{
					loadingPanel.Visible = false;
					passwordPanel.Visible = false;
					yubiPanel.Visible = true;
					this.yubiLabel.Text = strings.YubikeyInsert;
					return;
				}
				else if (ex is BadPasswordException)
				{
					loadingPanel.Visible = false;
					yubiPanel.Visible = false;
					passwordPanel.Visible = true;
					this.passwordErrorLabel.Text = strings.InvalidPassword;
					this.passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
					// oddity with MetroFrame controls in have to set focus away and back to field to make it stick
					this.Invoke((MethodInvoker)delegate { this.passwordButton.Focus(); this.passwordField.Focus(); });
					this.passwordTimer.Enabled = true;
					return;
				}
				else if (ex is Exception)
				{
					if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						this.Close();
						return;
					}
					loadConfig(password);
					return;
				}

				WinAuthConfig config = configTask.Result.Item1;
				if (config == null)
				{
					System.Diagnostics.Process.GetCurrentProcess().Kill();
					return;
				}

				// check for a v2 config file if this is a new config
				if (config.Count == 0 && string.IsNullOrEmpty(config.Filename) == true)
				{
					_existingv2Config = WinAuthHelper.GetLastV2Config();
				}

				this.Config = config;
				this.Config.OnConfigChanged += new ConfigChangedHandler(OnConfigChanged);

				if (config.Upgraded == true)
				{
					SaveConfig(true);
					// display warning
					WinAuthForm.ErrorDialog(this, string.Format(strings.ConfigUpgraded, WinAuthConfig.CURRENTVERSION));
				}

				InitializeForm();
			}, TaskScheduler.FromCurrentSynchronizationContext());
#endif
#if NETFX_3
			WinAuthConfig config;
			try
			{
				// use previous config if we have one
				config = WinAuthHelper.LoadConfig(this, configFile, password);
				if (config == null)
				{
					System.Diagnostics.Process.GetCurrentProcess().Kill();
					return;
				}

				// check for a v2 config file if this is a new config
				if (config.Count == 0 && string.IsNullOrEmpty(config.Filename) == true)
				{
					_existingv2Config = WinAuthHelper.GetLastV2Config();
				}

				this.Config = config;
				this.Config.OnConfigChanged += new ConfigChangedHandler(OnConfigChanged);

				if (config.Upgraded == true)
				{
					SaveConfig(true);
					// display warning
					WinAuthForm.ErrorDialog(this, string.Format(strings.ConfigUpgraded, WinAuthConfig.CURRENTVERSION));
				}

				InitializeForm();
			}
			catch (Exception ex)
			{
				if (ex is WinAuthInvalidNewerConfigException)
				{
					MessageBox.Show(this, ex.Message, WinAuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Error);
					System.Diagnostics.Process.GetCurrentProcess().Kill();
					return;
				}
				else if (ex is EncrpytedSecretDataException)
				{
					loadingPanel.Visible = false;
					passwordPanel.Visible = true;
					yubiPanel.Visible = false;

					this.passwordButton.Focus();
					this.passwordField.Focus();

					return;
				}
				else if (ex is BadYubiKeyException)
				{
					loadingPanel.Visible = false;
					passwordPanel.Visible = false;
					yubiPanel.Visible = true;
					this.yubiLabel.Text = strings.YubikeyInsert;
					return;
				}
				else if (ex is BadPasswordException)
				{
					loadingPanel.Visible = false;
					yubiPanel.Visible = false;
					passwordPanel.Visible = true;
					this.passwordErrorLabel.Text = strings.InvalidPassword;
					this.passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
					// oddity with MetroFrame controls in have to set focus away and back to field to make it stick
					this.Invoke((MethodInvoker)delegate { this.passwordButton.Focus(); this.passwordField.Focus(); });
					this.passwordTimer.Enabled = true;
					return;
				}
				else // if (ex is Exception)
				{
					if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						this.Close();
						return;
					}
					loadConfig(password);
					return;
				}
			};
#endif

		}

		/// <summary>
		/// Import authenticators from a file
		/// 
		/// *.xml = WinAuth v2
		/// *.txt = plain text with KeyUriFormat per line (https://code.google.com/p/google-authenticator/wiki/KeyUriFormat)
		/// *.zip = encrypted zip, containing import file
		/// *.pgp = PGP encrypted, containing import file
		/// 
		/// </summary>
		/// <param name="authenticatorFile">name import file</param>
		private void importAuthenticator(string authenticatorFile)
		{
			// call legacy import for v2 xml files
			if (string.Compare(Path.GetExtension(authenticatorFile), ".xml", true) == 0)
			{
				importAuthenticatorFromV2(authenticatorFile);
				return;
			}

			List<WinAuthAuthenticator> authenticators = null;
			bool retry;
			do
			{
				retry = false;
				try
				{
					authenticators = WinAuthHelper.ImportAuthenticators(this, authenticatorFile);
				}
				catch (ImportException ex)
				{
					if (ErrorDialog(this, ex.Message, ex.InnerException, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						return;
					}
					retry = true;
				}
			} while (retry);
			if (authenticators == null)
			{
				return;
			}

			// save all the new authenticators
			foreach (var authenticator in authenticators)
			{
				//sync
				authenticator.Sync();

				// make sure there isn't a name clash
				int rename = 0;
				string importedName = authenticator.Name;
				while (this.Config.Where(a => a.Name == importedName).Count() != 0)
				{
					importedName = authenticator.Name + " " + (++rename);
				}
				authenticator.Name = importedName;

				// save off any new authenticators as a backup
				WinAuthHelper.SaveToRegistry(this.Config, authenticator);

				// first time we prompt for protection and set out main settings from imported config
				if (this.Config.Count == 0)
				{
					ChangePasswordForm form = new ChangePasswordForm();
					form.PasswordType = Authenticator.PasswordTypes.Explicit;
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						this.Config.Yubi = form.Yubikey;
						this.Config.PasswordType = form.PasswordType;
						if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 && string.IsNullOrEmpty(form.Password) == false)
						{
							this.Config.Password = form.Password;
						}
					}
				}

				// add to main list
				this.Config.Add(authenticator);
			}

			SaveConfig(true);
			loadAuthenticatorList();

			// reset UI
			setAutoSize();
			introLabel.Visible = (this.Config.Count == 0);

			// reset hotkeys
			HookHotkeys();
		}

		/// <summary>
		/// Import a v2 authenticator from an existing file name
		/// </summary>
		/// <param name="authenticatorFile">name of v2 xml file</param>
		private void importAuthenticatorFromV2(string authenticatorFile)
		{
			bool retry = false;
			string password = null;
			bool needPassword = false;
			bool invalidPassword = false;
			do
			{
				try
				{
					WinAuthConfig config = WinAuthHelper.LoadConfig(this, authenticatorFile, password);
					if (config.Count == 0)
					{
						return;
					}

					// get the actual authenticator and ensure it is synced
					WinAuthAuthenticator importedAuthenticator = config[0];
					importedAuthenticator.Sync();

					// make sure there isn't a name clash
					int rename = 0;
					string importedName = importedAuthenticator.Name;
					while (this.Config.Where(a => a.Name == importedName).Count() != 0)
					{
						importedName = importedAuthenticator.Name + " (" + (++rename) + ")";
					}
					importedAuthenticator.Name = importedName;

					// save off any new authenticators as a backup
					WinAuthHelper.SaveToRegistry(this.Config, importedAuthenticator);

					// first time we prompt for protection and set out main settings from imported config
					if (this.Config.Count == 0)
					{
						this.Config.StartWithWindows = config.StartWithWindows;
						this.Config.UseTrayIcon = config.UseTrayIcon;
						this.Config.AlwaysOnTop = config.AlwaysOnTop;

						ChangePasswordForm form = new ChangePasswordForm();
						form.PasswordType = Authenticator.PasswordTypes.Explicit;
						if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
						{
							this.Config.Yubi = form.Yubikey;
							this.Config.PasswordType = form.PasswordType;
							if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 && string.IsNullOrEmpty(form.Password) == false)
							{
								this.Config.Password = form.Password;
							}
						}
					}

					// add to main list
					this.Config.Add(importedAuthenticator);
					SaveConfig(true);
					loadAuthenticatorList(importedAuthenticator);

					// reset UI
					setAutoSize();
					introLabel.Visible = (this.Config.Count == 0);

					// reset hotkeys
					HookHotkeys();

					needPassword = false;
					retry = false;
				}
				catch (EncryptedSecretDataException)
				{
					needPassword = true;
					invalidPassword = false;
				}
				catch (BadYubiKeyException)
				{
					needPassword = true;
					invalidPassword = false;
				}
				catch (BadPasswordException)
				{
					needPassword = true;
					invalidPassword = true;
				}
				catch (Exception ex)
				{
					if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						return;
					}
					needPassword = false;
					invalidPassword = false;
					retry = true;
				}

				if (needPassword == true)
				{
					GetPasswordForm form = new GetPasswordForm();
					form.InvalidPassword = invalidPassword;
					var result = form.ShowDialog(this);
					if (result == DialogResult.Cancel)
					{
						return;
					}
					password = form.Password;
					retry = true;
				}
			} while (retry == true);
		}

		private void InitializeOnce()
		{
			if (m_initOnce == false)
			{
#if BETA
				string betaversion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
				string betaConfirmed = WinAuthHelper.ReadRegistryValue(WINAUTHREGKEY_BETAWARNING, string.Empty) as string; // this.Config.ReadSetting(WINAUTHREGKEY_BETAWARNING, null) as string;
				if (string.Compare(betaConfirmed, betaversion) != 0)
				{
					if (new BetaForm().ShowDialog(this) != DialogResult.OK)
					{
						this.Close();
						return;
					}

					WinAuthHelper.WriteRegistryValue(WINAUTHREGKEY_BETAWARNING, betaversion);
				}
#endif

				// hook into System time change event
				Microsoft.Win32.SystemEvents.TimeChanged += new EventHandler(SystemEvents_TimeChanged);

				// save the initial form size
				m_initialSize = this.Size;

				// redirect mouse wheel events
				_wheelMessageForwarder = new WinAPI.MessageForwarder(authenticatorList, WinAPI.WM_MOUSEWHEEL);

				// get the current USB devices
				m_yubis = HIDDevice.GetAllDevices(YubiKey.VENDOR_ID);

				// register for USB changes
				m_usbHandle = WinAPI.RegisterUsbDeviceNotification(this.Handle);

				m_initOnce = true;
			}
		}

		/// <summary>
		/// Initialise the current form and UI
		/// </summary>
		private void InitializeForm()
		{
			// create the updater and check for update if appropriate
			if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed == false)
			{
				Updater = new WinAuthUpdater(this.Config);

				// the very first time, we set it to update each time
				if (Updater.LastCheck == DateTime.MinValue)
				{
					Updater.SetUpdateInterval(new TimeSpan(0, 0, 0));
				}
				if (Updater.IsAutoCheck == true)
				{
					Version latest = Updater.LastKnownLatestVersion;
					if (latest != null && latest > Updater.CurrentVersion)
					{
						newVersionLink.Text = "New version " + latest + " available";
						newVersionLink.Visible = true;
					}
				}
				// spin up the autocheck thread and assign callback
				Updater.AutoCheck(NewVersionAvailable);
			}

			// set up list
			loadAuthenticatorList();

			// set always on top
			this.TopMost = this.Config.AlwaysOnTop;

			// size the form based on the authenticators
			setAutoSize();

			// initialize UI
			LoadAddAuthenticatorTypes();
			loadOptionsMenu(this.optionsMenu);
			loadNotifyMenu(this.notifyMenu);
			loadingPanel.Visible = false;
			passwordPanel.Visible = false;
			yubiPanel.Visible = false;
			commandPanel.Visible = true;
			introLabel.Visible = (this.Config.Count == 0);
			authenticatorList.Visible = (this.Config.Count != 0);
			this.addAuthenticatorButton.Visible = !this.Config.IsReadOnly;

			// set title
			notifyIcon.Visible = this.Config.UseTrayIcon;
			notifyIcon.Text = this.Text = WinAuthMain.APPLICATION_TITLE;

			// hook hotkeys
			HookHotkeys();

			// hook Steam notifications
			HookSteam();

			// save the position of the list within the form else starting as minimized breaks the size
			_listoffset = new Rectangle(authenticatorList.Left, authenticatorList.Top, (this.Width - authenticatorList.Width), (this.Height - authenticatorList.Height));

			// set the shadow type (change in config for compatibility)
			try
			{
				MetroFormShadowType shadow = (MetroFormShadowType)Enum.Parse(typeof(MetroFormShadowType), this.Config.ShadowType, true);
				this.ShadowType = shadow;
			}
			catch (Exception) { }

			// set positions
			if (this.Config.Position.IsEmpty == false)
			{
				// check we aren't out of bounds in case of multi-monitor change
				var v = SystemInformation.VirtualScreen;
				if ((this.Config.Position.X + this.Width) >= v.Left && this.Config.Position.X < v.Width && this.Config.Position.Y > v.Top)
				{
					try
					{
						this.StartPosition = FormStartPosition.Manual;
						this.Left = this.Config.Position.X;
						this.Top = this.Config.Position.Y;
					}
					catch (Exception) { }
				}

				// check we aren't below the taskbar
				int lowesty = Screen.GetWorkingArea(this).Bottom;
				var bottom = this.Top + this.Height;
				if (bottom > lowesty)
				{
					this.Top -= (bottom - lowesty);
					if (this.Top < 0)
					{
						this.Height += this.Top;
						Top = 0;
					}
				}
			}
			else if (this.Config.AutoSize == true)
			{
				this.CenterToScreen();
			}

			// if we passed "-min" flag
			if (_initiallyMinimised == true)
			{
				this.WindowState = FormWindowState.Minimized;
				this.ShowInTaskbar = true;
			}
			if (this.Config.UseTrayIcon == true)
			{
				notifyIcon.Visible = true;
				notifyIcon.Text = this.Text;

				// if initially minimized, we need to hide
				if (WindowState == FormWindowState.Minimized)
				{
					// hide this and metro owner
					Form form = this;
					do
					{
						form.Hide();
					} while ((form = form.Owner) != null);
				}
			}
		}

		/// <summary>
		/// Load the authenticators into the display list
		/// </summary>
		/// <param name="added">authenticator we just added</param>
		private void loadAuthenticatorList(WinAuthAuthenticator added = null)
		{
			// set up list
			authenticatorList.Items.Clear();

			int index = 0;
			foreach (var auth in Config)
			{
				var ali = new AuthenticatorListitem(auth, index);
				if (added != null && added == auth && auth.AutoRefresh == false && !(auth.AuthenticatorData is HOTPAuthenticator))
				{
					ali.LastUpdate = DateTime.Now;
					ali.DisplayUntil = DateTime.Now.AddSeconds(10);
				}
				authenticatorList.Items.Add(ali);
				index++;
			}

			authenticatorList.Visible = (authenticatorList.Items.Count != 0);
		}

		/// <summary>
		/// Save the current config immediately or delay it for a few seconds so we can make more changes
		/// </summary>
		private void SaveConfig(bool immediate = false)
		{
			if (immediate == true || (_saveConfigTime != null && _saveConfigTime <= DateTime.Now))
			{
				_saveConfigTime = null;
				lock (this.Config)
				{
					WinAuthHelper.SaveConfig(this.Config);
				}
			}
			else
			{
				// save it in a few seconds so we can batch up saves
				_saveConfigTime = DateTime.Now.AddSeconds(3);
			}
		}

		/// <summary>
		/// Show an error message dialog
		/// </summary>
		/// <param name="form">owning form</param>
		/// <param name="message">optional message to display</param>
		/// <param name="ex">optional exception details</param>
		/// <param name="buttons">button choice other than OK</param>
		/// <returns>DialogResult</returns>
		public static DialogResult ErrorDialog(Form form, string message = null, Exception ex = null, MessageBoxButtons buttons = MessageBoxButtons.OK)
		{
			if (message == null)
			{
				message = strings.ErrorOccured + (ex != null ? ": " + ex.Message : string.Empty);
			}
			if (ex != null && string.IsNullOrEmpty(ex.Message) == false)
			{
				message += Environment.NewLine + Environment.NewLine + ex.Message;
			}
#if DEBUG
      StringBuilder capture = new StringBuilder();
      Exception e = ex;
      while (e != null)
      {
        capture.Append(new System.Diagnostics.StackTrace(e).ToString()).Append(Environment.NewLine);
        e = e.InnerException;
      }
			message += Environment.NewLine + Environment.NewLine + capture.ToString();

			if (ex != null)
			{
				WinAuthMain.LogException(ex);
			}
#endif

			return MessageBox.Show(form, message, WinAuthMain.APPLICATION_TITLE, buttons, MessageBoxIcon.Exclamation);
		}

		/// <summary>
		/// Show a confirmation Yes/No dialog
		/// </summary>
		/// <param name="form">owning form</param>
		/// <param name="message">message to display</param>
		/// <param name="buttons">button if other than YesNo</param>
		/// <returns>DialogResult</returns>
		public static DialogResult ConfirmDialog(Form form, string message, MessageBoxButtons buttons = MessageBoxButtons.YesNo, MessageBoxIcon icon = MessageBoxIcon.Question, MessageBoxDefaultButton defaultButton = MessageBoxDefaultButton.Button1)
		{
			return MessageBox.Show(form, message, WinAuthMain.APPLICATION_TITLE, buttons, icon, defaultButton);
		}

		/// <summary>
		/// Preload the context menu with the possible set of authenticator types
		/// </summary>
		private void LoadAddAuthenticatorTypes()
		{
			addAuthenticatorMenu.Items.Clear();

			ToolStripMenuItem subitem;
			int index = 0;
			foreach (RegisteredAuthenticator auth in WinAuthMain.REGISTERED_AUTHENTICATORS)
			{
				if (auth == null)
				{
					addAuthenticatorMenu.Items.Add(new ToolStripSeparator());
					continue;
				}

				subitem = new ToolStripMenuItem();
				subitem.Text = auth.Name;
				subitem.Name = "addAuthenticatorMenuItem_" + index++;
				subitem.Tag = auth;
				if (string.IsNullOrEmpty(auth.Icon) == false)
				{
					subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources." + auth.Icon));
					subitem.ImageAlign = ContentAlignment.MiddleLeft;
					subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
				}
				subitem.Click += addAuthenticatorMenu_Click;
				addAuthenticatorMenu.Items.Add(subitem);
			}
			//
			addAuthenticatorMenu.Items.Add(new ToolStripSeparator());
			//
			subitem = new ToolStripMenuItem();
			subitem.Text = strings.MenuImportText;
			subitem.Name = "importTextMenuItem";
			subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources.TextIcon.png"));
			subitem.ImageAlign = ContentAlignment.MiddleLeft;
			subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
			subitem.Click += importTextMenu_Click;
			addAuthenticatorMenu.Items.Add(subitem);
		}

		/// <summary>
		/// Unhook the current key hook
		/// </summary>
		private void UnhookHotkeys()
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
		private void HookHotkeys()
		{
			// unhook any old hotkeys
			UnhookHotkeys();

			// hook hotkey
			List<WinAuthAuthenticator> keys = new List<WinAuthAuthenticator>();
			foreach (var auth in Config)
			{
				if (auth.HotKey != null)
				{
					keys.Add(auth);
				}
			}
			if (keys.Count != 0)
			{
				m_hook = new KeyboardHook(this, keys);
				m_hook.KeyPressed += new KeyboardHook.KeyboardHookEventHandler(Hotkey_KeyPressed);
			}
		}

#region Steam Notifications

		/// <summary>
		/// Unhook the Steam notifications
		/// </summary>
		public void UnhookSteam()
		{
			if (this.Config == null)
			{
				return;
			}

			foreach (var auth in this.Config)
			{
				if (auth.AuthenticatorData != null && auth.AuthenticatorData is SteamAuthenticator && ((SteamAuthenticator)auth.AuthenticatorData).Client != null)
				{
					var client = ((SteamAuthenticator)auth.AuthenticatorData).GetClient();
					client.ConfirmationEvent -= SteamClient_ConfirmationEvent;
					client.ConfirmationErrorEvent -= SteamClient_ConfirmationErrorEvent;
				}
			}
		}

		/// <summary>
		/// Hook the Steam authenticators for notifications
		/// </summary>
		public void HookSteam()
		{
			UnhookSteam();
			if (this.Config == null)
			{
				return;
			}

#if NETFX_4
			// do async as setting up clients can take time (Task.Factory.StartNew wait for UI so need to use new Thread(...))
			new Thread(new ThreadStart(() =>
			{
				foreach (var auth in this.Config)
				{
					if (auth.AuthenticatorData != null && auth.AuthenticatorData is SteamAuthenticator)
					{
						var client = ((SteamAuthenticator)auth.AuthenticatorData).GetClient();
						client.ConfirmationEvent += SteamClient_ConfirmationEvent;
						client.ConfirmationErrorEvent += SteamClient_ConfirmationErrorEvent;
					}
				}
			})).Start();
#endif
#if NETFX_3
			foreach (var auth in this.Config)
			{
				if (auth.AuthenticatorData != null && auth.AuthenticatorData is SteamAuthenticator)
				{
					var client = ((SteamAuthenticator)auth.AuthenticatorData).GetClient();
					client.ConfirmationEvent += SteamClient_ConfirmationEvent;
					client.ConfirmationErrorEvent += SteamClient_ConfirmationErrorEvent;
				}
			}
#endif
		}

		/// <summary>
		/// Display error message from Steam polling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="message"></param>
		/// <param name="ex"></param>
		private void SteamClient_ConfirmationErrorEvent(object sender, string message, Exception ex)
		{
			SteamClient steam = sender as SteamClient;
			var auth = this.Config.Cast<WinAuthAuthenticator>().Where(a => a.AuthenticatorData is SteamAuthenticator && ((SteamAuthenticator)a.AuthenticatorData).Serial == steam.Authenticator.Serial).FirstOrDefault();

			// show the Notification window in the correct context
			this.Invoke(new ShowNotificationCallback(ShowNotification), new object[] {
					auth,
					auth.Name,
					message,
					false,
					0
				});
			//WinAuthForm.ErrorDialog(this, message, ex);
		}

		/// <summary>
		/// Delegate for Steam notification
		/// </summary>
		/// <param name="auth">current Authenticator</param>
		/// <param name="title">title of notification</param>
		/// <param name="message">notification body</param>
		/// <param name="openOnClick">if can open on click</param>
		/// <param name="extraHeight">extra height (for errors)</param>
		public delegate void ShowNotificationCallback(WinAuthAuthenticator auth, string title, string message, bool openOnClick, int extraHeight);

		/// <summary>
		/// Display a new Notification for a Trading confirmation
		/// </summary>
		/// <param name="auth"></param>
		/// <param name="title"></param>
		/// <param name="message"></param>
		/// <param name="extraHeight"></param>
		public void ShowNotification(WinAuthAuthenticator auth, string title, string message, bool openOnClick, int extraHeight)
		{
			var notify = new Notification(title, message, 10000);
			if (extraHeight != 0)
			{
				notify.Height += extraHeight;
			}
			notify.Tag = auth;
			if (openOnClick == true)
			{
				notify.OnNotificationClicked += Notify_Click;
			}
			notify.Show();
		}

		/// <summary>
		/// The Notification window is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Notify_Click(object sender, EventArgs e)
		{
			WinAuthAuthenticator auth = ((Notification)sender).Tag as WinAuthAuthenticator;

			// ensure window is front
			BringToFront();
			Show();
			WindowState = FormWindowState.Normal;
			Activate();

			// show waiting
			Cursor.Current = Cursors.WaitCursor;

			// open the confirmations
			var item = authenticatorList.ContextMenuStrip.Items.Cast<ToolStripItem>().Where(i => i.Name == "showSteamTradesMenuItem").FirstOrDefault();
			authenticatorList.CurrentItem = authenticatorList.Items.Cast<AuthenticatorListitem>().Where(i => i.Authenticator == auth).FirstOrDefault();
			item.PerformClick();
		}

		/// <summary>
		/// Receive a new confirmation event from the SteamClient
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="confirmation"></param>
		/// <param name="action"></param>
		private void SteamClient_ConfirmationEvent(object sender, SteamClient.Confirmation confirmation, SteamClient.PollerAction action)
		{
			SteamClient steam = sender as SteamClient;

			var auth = this.Config.Cast<WinAuthAuthenticator>().Where(a => a.AuthenticatorData is SteamAuthenticator && ((SteamAuthenticator)a.AuthenticatorData).Serial == steam.Authenticator.Serial).FirstOrDefault();

			string title = null;
			string message = null;
			bool openOnClick = false;
			int extraHeight = 0;

			if (action == SteamClient.PollerAction.AutoConfirm || action == SteamClient.PollerAction.SilentAutoConfirm)
			{
				if (steam.ConfirmTrade(confirmation.Id, confirmation.Key, true) == true)
				{
					if (action != SteamClient.PollerAction.SilentAutoConfirm)
					{
						title = "Confirmed";
						message = string.Format("<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}</td></tr></table>", auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded);
					}
				}
				else
				{
					title = "Confirmation Failed";
					message = string.Format("<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}<br/>Error: {4}</td></tr></table>", auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded, steam.Error ?? "Unknown error");
					extraHeight += 20;
				}
			}
			else if (confirmation.IsNew == true) // if (action == SteamClient.PollerAction.Notify)
			{
				title = "New Confirmation";
				message = string.Format("<h1>{0}</h1><table width=250 cellspacing=0 cellpadding=0 border=0><tr valign=top><td width=40><img src=\"{1}\" /></td><td width=210>{2}<br/>{3}</td></tr></table>", auth.Name, confirmation.Image, confirmation.Details, confirmation.Traded);
				openOnClick = true;
			}

			if (title != null)
			{
				// show the Notification window in the correct context
				this.Invoke(new ShowNotificationCallback(ShowNotification), new object[] {
					auth,
					title,
					message,
					openOnClick,
					extraHeight
				});
			}
		}

#endregion

		/// <summary>
		/// General Windows Message handler
		/// </summary>
		/// <param name="m"></param>
		protected override void WndProc(ref Message m)
		{
			base.WndProc(ref m);

			// pick up the HotKey message from RegisterHotKey and call hook callback
			if (m.Msg == WinAPI.WM_HOTKEY)
			{
				Keys key = (Keys)(((int)m.LParam >> 16) & 0xffff);
				WinAPI.KeyModifiers modifier = (WinAPI.KeyModifiers)((int)m.LParam & 0xffff);

				if (m_hook != null)
				{
					m_hook.KeyCallback(new KeyboardHookEventArgs(key, modifier));
				}
			}
			else if (m.Msg == WinAPI.WM_DEVICECHANGE)
			{
				int wParam = (int)m.WParam;
				if (wParam == WinAPI.DBT_DEVICEARRIVAL)
				{
					lock (m_deviceArrivalMutex)
					{
						if (this.Config == null || (this.Config.PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) != 0 && m_yubis.Count == 0)
						{
							m_yubis = HIDDevice.GetAllDevices(YubiKey.VENDOR_ID);
							if (m_yubis.Count != 0)
							{
								this.authenticatorList.Items.Clear();
								this.Size = m_initialSize;
								this.Config = null;
								loadNotifyMenu(this.notifyMenu);
								UnhookHotkeys();
								loadConfig(string.Empty);
							}
						}
					}
				}
				else if (wParam == WinAPI.DBT_DEVICEREMOVED)
				{
					if (this.Config != null && (this.Config.PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) != 0)
					{
						// check if YubiKey still present
						m_yubis = HIDDevice.GetAllDevices(YubiKey.VENDOR_ID);
						if (m_yubis.Count == 0)
						{
							this.authenticatorList.Items.Clear();
							this.Size = m_initialSize;
							this.Config = null;
							loadNotifyMenu(this.notifyMenu);
							UnhookHotkeys();
							loadConfig(string.Empty);
						}
					}
				}
			}
			else if (m.Msg == WinAPI.WM_USER + 1)
			{
				// show the main form
				BringToFront();
				Show();
				WindowState = FormWindowState.Normal;
				Activate();
			}
		}

		/// <summary>
		/// A hotkey keyboard event occured, e.g. "Ctrl-Alt-C"
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void Hotkey_KeyPressed(object sender, KeyboardHookEventArgs e)
		{
			// avoid multiple keypresses being sent
			if (e.Authenticator != null && Monitor.TryEnter(m_sendingKeys) == true)
			{
				try
				{
					// set Tag as HotKeyLauncher so we can pull back related authenticator and check for timeout
					hotkeyTimer.Tag = new HotKeyLauncher(this, e.Authenticator);
					hotkeyTimer.Enabled = true;

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
		/// Timer tick for hotkey
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void hotkeyTimer_Tick(object sender, EventArgs e)
		{
			HotKeyLauncher data = hotkeyTimer.Tag as HotKeyLauncher;

			// check we don't wait forever
			if (data.Started.AddSeconds(5) < DateTime.Now)
			{
				hotkeyTimer.Enabled = false;
				return;
			}

			// wait until the modifiers are released
			if ((System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != 0
				|| (System.Windows.Forms.Control.ModifierKeys & Keys.Control) != 0
				|| (System.Windows.Forms.Control.ModifierKeys & Keys.Shift) != 0)
			{
				return;
			}

			// cancel the timer
			hotkeyTimer.Enabled = false;

			// invoke the handler method in the correct context
			data.Form.Invoke((MethodInvoker)delegate { HandleHotkey(data.Authenticator); });
		}

		/// <summary>
		/// Process the pressed hotkey by performing the appropriate operation
		/// </summary>
		/// <param name="auth">Authenticator</param>
		private void HandleHotkey(WinAuthAuthenticator auth)
		{
			// get the code
			string code = null;
			try
			{
				code = auth.CurrentCode;
			}
			catch (EncryptedSecretDataException)
			{
				// if the authenticator is current protected we display the password window, get the code, and reprotect it
				// with a bit of window jiggling to make sure we get focus and then put it back

				// save the current window
				var fgwindow = WinAPI.GetForegroundWindow();
				Screen screen = Screen.FromHandle(fgwindow);
				IntPtr activewindow = IntPtr.Zero;
				if (this.Visible == true)
				{
					activewindow = WinAPI.SetActiveWindow(this.Handle);
					BringToFront();
				}

				var item = authenticatorList.Items.Cast<AuthenticatorListitem>().Where(i => i.Authenticator == auth).FirstOrDefault();
				code = authenticatorList.GetItemCode(item, screen);

				// restore active window
				if (activewindow != IntPtr.Zero)
				{
					WinAPI.SetActiveWindow(activewindow);
				}
				WinAPI.SetForegroundWindow(fgwindow);
			}
			if (code != null)
			{
				// default to sending the code to the current window
				KeyboardSender keysend = new KeyboardSender(auth.HotKey.Window);
				string command = null;
				if (auth.HotKey.Action == HotKey.HotKeyActions.Notify)
				{
					if (auth.CopyOnCode)
					{
						auth.CopyCodeToClipboard(this, code);
					}
					if (code.Length > 5)
					{
						code = code.Insert(code.Length / 2, " ");
					}
					notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
				}
				if (auth.HotKey.Action == HotKey.HotKeyActions.Copy)
				{
					command = "{COPY}";
				}
				else if (auth.HotKey.Action == HotKey.HotKeyActions.Advanced)
				{
					command = auth.HotKey.Advanced;
				}
				else if (auth.HotKey.Action == HotKey.HotKeyActions.Inject)
				{
					command = "{CODE}";
				}
				if (command != null)
				{
					keysend.SendKeys(this, command, code);
				}
			}
	  }

		/// <summary>
		/// Run an action on the authenticator
		/// </summary>
		/// <param name="auth">Authenticator to use</param>
		/// <param name="action">Action to perform</param>
		private void RunAction(WinAuthAuthenticator auth, WinAuthConfig.NotifyActions action)
		{
			// get the code
			string code = null;
			try
			{
				code = auth.CurrentCode;
			}
			catch (EncryptedSecretDataException)
			{
				// if the authenticator is current protected we display the password window, get the code, and reprotect it
				// with a bit of window jiggling to make sure we get focus and then put it back

				// save the current window
				var fgwindow = WinAPI.GetForegroundWindow();
				Screen screen = Screen.FromHandle(fgwindow);
				IntPtr activewindow = IntPtr.Zero;
				if (this.Visible == true)
				{
					activewindow = WinAPI.SetActiveWindow(this.Handle);
					BringToFront();
				}

				var item = authenticatorList.Items.Cast<AuthenticatorListitem>().Where(i => i.Authenticator == auth).FirstOrDefault();
				code = authenticatorList.GetItemCode(item, screen);

				// restore active window
				if (activewindow != IntPtr.Zero)
				{
					WinAPI.SetActiveWindow(activewindow);
				}
				WinAPI.SetForegroundWindow(fgwindow);
			}
			if (code != null)
			{
				KeyboardSender keysend = new KeyboardSender(auth.HotKey != null ? auth.HotKey.Window : null);
				string command = null;

				if (action == WinAuthConfig.NotifyActions.CopyToClipboard)
				{
					command = "{COPY}";
				}
				else if (action == WinAuthConfig.NotifyActions.HotKey)
				{
					command = auth.HotKey != null ? auth.HotKey.Advanced : null;
				}
				else // if (this.Config.NotifyAction == WinAuthConfig.NotifyActions.Notification)
				{
					if (code.Length > 5)
					{
						code = code.Insert(code.Length / 2, " ");
					}
					notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
				}
				if (command != null)
				{
					keysend.SendKeys(this, command, code);
				}
			}
		}

		/// <summary>
		/// Put data into the clipboard
		/// </summary>
		/// <param name="data"></param>
		public void SetClipboardData(object data)
		{
			bool clipRetry = false;
			do
			{
				try
				{
					Clipboard.Clear();
					Clipboard.SetDataObject(data, true, 4, 250);
				}
				catch (ExternalException)
				{
					// only show an error the first time
					clipRetry = (MessageBox.Show(this, strings.ClipboardInUse,
						WinAuthMain.APPLICATION_NAME,
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
				}
			}
			while (clipRetry == true);
		}

		/// <summary>
		/// Get data from the clipboard
		/// </summary>
		/// <param name="format"></param>
		/// <returns></returns>
		public object GetClipboardData(Type format)
		{
			bool clipRetry = false;
			do
			{
				try
				{
					IDataObject clipdata = Clipboard.GetDataObject();
					return (clipdata != null ? clipdata.GetData(format) : null);
				}
				catch (ExternalException)
				{
					// only show an error the first time
					clipRetry = (MessageBox.Show(this, strings.ClipboardInUse,
						WinAuthMain.APPLICATION_NAME,
						MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes);
				}
			}
			while (clipRetry == true);

			return null;
		}

		/// <summary>
		/// Set the size of the form based on config AutoSize property
		/// </summary>
		private void setAutoSize()
		{
			if (Config.AutoSize == true)
			{
				if (this.Config.Count != 0)
				{
					this.Width = Math.Max(420, authenticatorList.Margin.Horizontal + authenticatorList.GetMaxItemWidth() + (this.Width - authenticatorList.Width));
				}
				else
				{
					this.Width = 420;
				}

				// Issue#175; take the smallest of full height or 62% screen height
				int height = this.Height - authenticatorList.Height;
				height += (this.Config.Count * authenticatorList.ItemHeight);
				this.Height = Math.Min(Screen.GetWorkingArea(this).Height * 62 / 100, height);

				this.Resizable = false;
			}
			else
			{
				this.Resizable = true;
				if (Config.Width != 0)
				{
					this.Width = Config.Width;
				}
				if (Config.Height != 0)
				{
					this.Height = Config.Height;
				}
			}
		}

		/// <summary>
		/// Can the renaming of an authenticator
		/// </summary>
		private void EndRenaming()
		{
			// set focus to form, so that the edit field will disappear if it is visble
			if (authenticatorList.IsRenaming == true)
			{
				authenticatorList.EndRenaming();
			}
		}

		/// <summary>
		/// Callback from the Updater if a newer version is available
		/// </summary>
		/// <param name="latest"></param>
		private void NewVersionAvailable(Version latest)
		{
			if (Updater != null && Updater.IsAutoCheck == true && latest != null && latest > Updater.CurrentVersion)
			{
				this.Invoke((MethodInvoker)delegate { newVersionLink.Text = "New version " + latest.ToString(3) + " available"; newVersionLink.Visible = true; });
			}
			else
			{
				this.Invoke((MethodInvoker)delegate { newVersionLink.Visible = false; });
			}
		}

		/// <summary>
		/// Show the Update form and update status if necessary
		/// </summary>
		private void ShowUpdaterForm()
		{
			UpdateCheckForm form = new UpdateCheckForm();
			form.Config = this.Config;
			form.Updater = this.Updater;
			if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				NewVersionAvailable(Updater.LastKnownLatestVersion);
				SaveConfig();
			}
		}

		/// <summary>
		/// Set up the notify icon when the main form is shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_Shown(object sender, EventArgs e)
		{
			// if we use tray icon make sure it is set
			if (this.Config != null && this.Config.UseTrayIcon == true)
			{
				notifyIcon.Visible = true;
				notifyIcon.Text = this.Text;

				// if initially minizied, we need to hide
				if (WindowState == FormWindowState.Minimized)
				{
					// hide this and metro owner
					Form form = this;
					do
					{
						form.Hide();
					} while ((form = form.Owner) != null);
				}
			}

			// prompt to import v2
			if (string.IsNullOrEmpty(_existingv2Config) == false)
			{
				DialogResult importResult = MessageBox.Show(this,
					string.Format(strings.LoadPreviousAuthenticator, _existingv2Config),
					WinAuthMain.APPLICATION_TITLE,
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Question);
				if (importResult == System.Windows.Forms.DialogResult.Yes)
				{
					importAuthenticatorFromV2(_existingv2Config);
				}
				_existingv2Config = null;
			}
		}

		/// <summary>
		/// Minimize to icon when closing or unbind and close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// keep in the tray when closing Form 
			if (Config != null && Config.UseTrayIcon == true && this.Visible == true && m_explictClose == false)
			{
				e.Cancel = true;
				notifyIcon.Visible = true;
				// hide this and metro owner
				Form form = this;
				do
				{
					form.Hide();
				} while ((form = form.Owner) != null);
				return;
			}

			// remove the Steam hook
			UnhookSteam();

			// remove the hotkey hook
			UnhookHotkeys();

			// remove USB hook
			WinAPI.UnregisterUsbDeviceNotification(m_usbHandle);

			// ensure the notify icon is closed
			notifyIcon.Visible = false;

			// save size if we are not autoresize
			if (this.Config != null && this.Config.AutoSize == false && (this.Config.Width != this.Width || this.Config.Height != this.Height))
			{
				this.Config.Width = this.Width;
				this.Config.Height = this.Height;
			}
			if (this.Config != null /* && this.Config.Position.IsEmpty == false */)
			{
				this.Config.Position = new Point(this.Left, this.Top);
			}

			// perform save if we have one pending
			if (_saveConfigTime != null)
			{
				SaveConfig(true);
			}
		}

		/// <summary>
		/// Click on a choice of new authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void addAuthenticatorMenu_Click(object sender, EventArgs e)
		{
			ToolStripItem menuitem = (ToolStripItem)sender;
			RegisteredAuthenticator registeredauth = menuitem.Tag as RegisteredAuthenticator;
			if (registeredauth != null)
			{
				// add the new authenticator
				WinAuthAuthenticator winauthauthenticator = new WinAuthAuthenticator();
				bool added = false;

				if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.BattleNet)
				{
					int existing = 0;
					string name;
					do
					{
						name = "Battle.net" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);

					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					// create the Battle.net authenticator
					AddBattleNetAuthenticator form = new AddBattleNetAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Trion)
				{
					// create the Trion authenticator
					int existing = 0;
					string name;
					do
					{
						name = "Trion" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);

					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddTrionAuthenticator form = new AddTrionAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Steam)
				{
					// create the authenticator
					int existing = 0;
					string name;
					do
					{
						name = "Steam" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);

					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddSteamAuthenticator form = new AddSteamAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Google)
				{
					// create the Google authenticator
					// add the new authenticator
					int existing = 0;
					string name;
					do
					{
						name = "Google" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);
					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddGoogleAuthenticator form = new AddGoogleAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.GuildWars)
				{
					// create the GW2 authenticator
					int existing = 0;
					string name;
					do
					{
						name = "GuildWars" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);
					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddGuildWarsAuthenticator form = new AddGuildWarsAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Microsoft)
				{
					// create the Microsoft authenticator
					int existing = 0;
					string name;
					do
					{
						name = "Microsoft" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);
					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddMicrosoftAuthenticator form = new AddMicrosoftAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME)
				{
					// create the Google authenticator
					// add the new authenticator
					int existing = 0;
					string name;
					do
					{
						name = "Authenticator" + (existing != 0 ? " (" + existing + ")" : string.Empty);
						existing++;
					} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);
					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;
					winauthauthenticator.Skin = "WinAuthIcon.png";

					AddAuthenticator form = new AddAuthenticator();
					form.Authenticator = winauthauthenticator;
					added = (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK);
				}
				else
				{
					throw new NotImplementedException(strings.AuthenticatorNotImplemented + ": " + registeredauth.AuthenticatorType.ToString());
				}

				if (added == true)
				{
					// save off any new authenticators as a backup
					WinAuthHelper.SaveToRegistry(this.Config, winauthauthenticator);

					// first time we prompt for protection
					if (this.Config.Count == 0)
					{
						ChangePasswordForm form = new ChangePasswordForm();
						form.PasswordType = Authenticator.PasswordTypes.Explicit;
						if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
						{
							this.Config.Yubi = form.Yubikey;
							this.Config.PasswordType = form.PasswordType;
							if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 && string.IsNullOrEmpty(form.Password) == false)
							{
								this.Config.Password = form.Password;
							}
						}
					}

					this.Config.Add(winauthauthenticator);
					SaveConfig(true);
					loadAuthenticatorList(winauthauthenticator);

					// reset UI
					setAutoSize();
					introLabel.Visible = (this.Config.Count == 0);

					// reset hotkeeys
					HookHotkeys();
				}
			}
		}

		/// <summary>
		/// Click to import an text file of authenticators
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void importTextMenu_Click(object sender, EventArgs e)
		{
			ToolStripItem menuitem = (ToolStripItem)sender;

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			//
			string lastv2file = WinAuthHelper.GetLastV2Config();
			if (string.IsNullOrEmpty(lastv2file) == false)
			{
				ofd.InitialDirectory = Path.GetDirectoryName(lastv2file);
				ofd.FileName = Path.GetFileName(lastv2file);
			}
			//
			ofd.Filter = "WinAuth Files (*.xml)|*.xml|Text Files (*.txt)|*.txt|Zip Files (*.zip)|*.zip|PGP Files (*.pgp)|*.pgp|All Files (*.*)|*.*";
			ofd.RestoreDirectory = true;
			ofd.Title = WinAuthMain.APPLICATION_TITLE;
			if (ofd.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				importAuthenticator(ofd.FileName);
			}
		}

		/// <summary>
		/// Timer tick event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTimer_Tick(object sender, EventArgs e)
		{
			authenticatorList.Tick(sender, e);

			// if a save is due
			if (_saveConfigTime != null && _saveConfigTime.Value <= DateTime.Now)
			{
				SaveConfig();
			}
		}

		/// <summary>
		/// Click the Add button to add an authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void addAuthenticatorButton_Click(object sender, EventArgs e)
		{
			addAuthenticatorMenu.Show(addAuthenticatorButton, addAuthenticatorButton.Width, 0);
		}

		/// <summary>
		/// Click the Options button to show menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void optionsButton_Click(object sender, EventArgs e)
		{
			optionsMenu.Show(optionsButton, optionsButton.Width - optionsMenu.Width, optionsButton.Height - 1);
		}

		/// <summary>
		/// Double click notify to re-open
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyIcon_DoubleClick(object sender, EventArgs e)
		{
			BringToFront();
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
		}

		/// <summary>
		/// Event fired when an authenticator is removed (i.e. deleted) from the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void authenticatorList_ItemRemoved(object source, AuthenticatorListItemRemovedEventArgs args)
		{
			foreach (var auth in Config)
			{
				if (auth == args.Item.Authenticator)
				{
					Config.Remove(auth);
					break;
				}
			}

			// update UI
			setAutoSize();

			// if no authenticators, show intro text and remove any encryption
			if (this.Config.Count == 0)
			{
				introLabel.Visible = true;
				authenticatorList.Visible = false;
				this.Config.PasswordType = Authenticator.PasswordTypes.None;
				this.Config.Password = null;
			}

			// save the current config
			SaveConfig();
		}

		/// <summary>
		/// Event fired when an authenticator is dragged and dropped in the listbox
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void authenticatorList_Reordered(object source, AuthenticatorListReorderedEventArgs args)
		{
			// set the new order of items in Config from that of the list
			int count = this.authenticatorList.Items.Count;
			for (int i=0; i<count; i++)
			{
				AuthenticatorListitem item = (AuthenticatorListitem)this.authenticatorList.Items[i];
				this.Config.Where(a => a == item.Authenticator).FirstOrDefault().Index = i;
			}
			// resort the config list
			this.Config.Sort();
			// update the notify menu
			loadNotifyMenu(this.notifyMenu);

			// update UI
			setAutoSize();

			// save the current config
			SaveConfig();
		}

		/// <summary>
		/// Double click an item in the list
		/// </summary>
		/// <param name="source"></param>
		/// <param name="args"></param>
		private void authenticatorList_DoubleClick(object source, AuthenticatorListDoubleClickEventArgs args)
		{
			RunAction(args.Authenticator, WinAuthConfig.NotifyActions.CopyToClipboard);
		}

		/// <summary>
		/// Click in the main form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_MouseDown(object sender, MouseEventArgs e)
		{
			EndRenaming();
		}

		/// <summary>
		/// Click in the command panel
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void commandPanel_MouseDown(object sender, MouseEventArgs e)
		{
			EndRenaming();
		}

		/// <summary>
		/// Resizing the form, we have to manually adjust the width and height of list else starting as minimized borks
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_Resize(object sender, EventArgs e)
		{
			this.SuspendLayout();
			if (_listoffset.Bottom != 0)
			{
				authenticatorList.Height = this.Height - _listoffset.Height;
				authenticatorList.Width = this.Width - _listoffset.Width;
			}
			this.ResumeLayout(true);
		}

		/// <summary>
		/// Set the config once resizing has completed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_ResizeEnd(object sender, EventArgs e)
		{
			if (this.Config != null && this.Config.AutoSize == false)
			{
				this.Config.Width = this.Width;
				this.Config.Height = this.Height;
			}
		}

		/// <summary>
		/// Click the button to enter a password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordButton_Click(object sender, EventArgs e)
		{
			if (this.passwordField.Text.Trim().Length == 0)
			{
				this.passwordErrorLabel.Text = strings.EnterPassword;
				this.passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
				this.passwordTimer.Enabled = true;
				return;
			}

			loadConfig(this.passwordField.Text);
			this.passwordField.Text = string.Empty;
		}

		/// <summary>
		/// Remove the password error message after a few seconds
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordTimer_Tick(object sender, EventArgs e)
		{
			if (this.passwordErrorLabel.Tag != null && (DateTime)this.passwordErrorLabel.Tag <= DateTime.Now)
			{
				this.passwordTimer.Enabled = false;
				this.passwordErrorLabel.Tag = null;
				this.passwordErrorLabel.Text = string.Empty;
			}
		}

		/// <summary>
		/// Catch pressing Enter in the password field
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordField_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == (char)Keys.Return)
			{
				e.Handled = true;
				passwordButton_Click(sender, null);
			}
		}

		/// <summary>
		/// Click the button to check the YubiKey again
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubiRetryButton_Click(object sender, EventArgs e)
		{
			loadConfig(this.passwordField.Text);
		}

		/// <summary>
		/// If click the new version status link
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newVersionLink_Click(object sender, EventArgs e)
		{
			if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed == false)
			{
				ShowUpdaterForm();
			}
		}

		/// <summary>
		/// System time change event. We need to resync any unprotected authenticators
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void SystemEvents_TimeChanged(object sender, EventArgs e)
		{
			Cursor cursor = Cursor.Current;
			Cursor.Current = Cursors.WaitCursor;
			foreach (var auth in this.Config)
			{
				if (auth.AuthenticatorData != null && auth.AuthenticatorData.RequiresPassword == false)
				{
					try
					{
						auth.Sync();
					}
					catch (Exception) { }
				}
			}
			Cursor.Current = cursor;
		}

#endregion

#region Options menu

		/// <summary>
		/// Load the menu items for the options menu
		/// </summary>
		private void loadOptionsMenu(ContextMenuStrip menu)
		{
			ToolStripMenuItem menuitem;

			menu.Items.Clear();

			if (this.Config == null || this.Config.IsReadOnly == false)
			{
				menuitem = new ToolStripMenuItem(strings.MenuChangeProtection + "...");
				menuitem.Name = "changePasswordOptionsMenuItem";
				menuitem.Click += changePasswordOptionsMenuItem_Click;
				menu.Items.Add(menuitem);
				menu.Items.Add(new ToolStripSeparator() { Name = "changePasswordOptionsSeparatorItem" });
			}

			if (this.Config != null && this.Config.IsPortable == false)
			{
				menuitem = new ToolStripMenuItem(strings.MenuStartWithWindows);
				menuitem.Name = "startWithWindowsOptionsMenuItem";
				menuitem.Click += startWithWindowsOptionsMenuItem_Click;
				menu.Items.Add(menuitem);
			}

			menuitem = new ToolStripMenuItem(strings.MenuAlwaysOnTop);
			menuitem.Name = "alwaysOnTopOptionsMenuItem";
			menuitem.Click += alwaysOnTopOptionsMenuItem_Click;
			menu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem(strings.MenuUseSystemTrayIcon);
			menuitem.Name = "useSystemTrayIconOptionsMenuItem";
			menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
			menu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem(strings.MenuAutoSize);
			menuitem.Name = "autoSizeOptionsMenuItem";
			menuitem.Click += autoSizeOptionsMenuItem_Click;
			menu.Items.Add(menuitem);

			menu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem(strings.MenuExport);
			menuitem.Name = "exportOptionsMenuItem";
			menuitem.Click += exportOptionsMenuItem_Click;
			menu.Items.Add(menuitem);

			menu.Items.Add(new ToolStripSeparator());

			if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed == false)
			{
				menuitem = new ToolStripMenuItem(strings.MenuUpdates + "...");
				menuitem.Name = "aboutUpdatesMenuItem";
				menuitem.Click += aboutUpdatesMenuItem_Click;
				menu.Items.Add(menuitem);

				menu.Items.Add(new ToolStripSeparator());
			}

			menuitem = new ToolStripMenuItem(strings.MenuAbout + "...");
			menuitem.Name = "aboutOptionsMenuItem";
			menuitem.Click += aboutOptionMenuItem_Click;
			menu.Items.Add(menuitem);

			menu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem(strings.MenuExit);
			menuitem.Name = "exitOptionsMenuItem";
			menuitem.ShortcutKeys = Keys.F4 | Keys.Alt;
			menuitem.Click += exitOptionMenuItem_Click;
			menu.Items.Add(menuitem);
		}

		/// <summary>
		/// Load the menu items for the notify menu
		/// </summary>
		private void loadNotifyMenu(ContextMenuStrip menu)
		{
			ToolStripMenuItem menuitem;
			ToolStripMenuItem subitem;

			menu.Items.Clear();

			menuitem = new ToolStripMenuItem(strings.MenuOpen);
			menuitem.Name = "openOptionsMenuItem";
			menuitem.Click += openOptionsMenuItem_Click;
			menu.Items.Add(menuitem);
			menu.Items.Add(new ToolStripSeparator() { Name = "openOptionsSeparatorItem" });

			if (this.Config != null && this.Config.Count != 0)
			{
				// because of window size, we only show first 30.
				// @todo change to MRU
				var index = 1;
				foreach (var auth in this.Config.Take(30))
				{
					menuitem = new ToolStripMenuItem(index.ToString() + ". " + auth.Name);
					menuitem.Name = "authenticatorOptionsMenuItem_" + index;
					menuitem.Tag = auth;
					menuitem.ShortcutKeyDisplayString = (auth.HotKey != null ? auth.HotKey.ToString() : null);
					menuitem.Click += authenticatorOptionsMenuItem_Click;
					menu.Items.Add(menuitem);
					index++;
				}
				var separator = new ToolStripSeparator();
				separator.Name = "authenticatorOptionsSeparatorItem";
				menu.Items.Add(separator);

				menuitem = new ToolStripMenuItem(strings.DefaultAction);
				menuitem.Name = "defaultActionOptionsMenuItem";
				menu.Items.Add(menuitem);
				subitem = new ToolStripMenuItem(strings.DefaultActionNotification);
				subitem.Name = "defaultActionNotificationOptionsMenuItem";
				subitem.Click += defaultActionNotificationOptionsMenuItem_Click;
				menuitem.DropDownItems.Add(subitem);
				subitem = new ToolStripMenuItem(strings.DefaultActionCopyToClipboard);
				subitem.Name = "defaultActionCopyToClipboardOptionsMenuItem";
				subitem.Click += defaultActionCopyToClipboardOptionsMenuItem_Click;
				menuitem.DropDownItems.Add(subitem);
				subitem = new ToolStripMenuItem(strings.DefaultActionHotkey);
				subitem.Name = "defaultActionHotkeyOptionsMenuItem";
				subitem.Click += defaultActionHotkeyOptionsMenuItem_Click;
				menuitem.DropDownItems.Add(subitem);
				menu.Items.Add(menuitem);

				separator = new ToolStripSeparator();
				separator.Name = "authenticatorActionOptionsSeparatorItem";
				menu.Items.Add(separator);
			}

			//if (this.Config != null)
			//{
			//	menuitem = new ToolStripMenuItem(strings.MenuUseSystemTrayIcon);
			//	menuitem.Name = "useSystemTrayIconOptionsMenuItem";
			//	menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
			//	menu.Items.Add(menuitem);

			//	menu.Items.Add(new ToolStripSeparator());
			//}

			menuitem = new ToolStripMenuItem(strings.MenuAbout + "...");
			menuitem.Name = "aboutOptionsMenuItem";
			menuitem.Click += aboutOptionMenuItem_Click;
			menu.Items.Add(menuitem);

			menu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem(strings.MenuExit);
			menuitem.Name = "exitOptionsMenuItem";
			menuitem.ShortcutKeys = Keys.F4 | Keys.Alt;
			menuitem.Click += exitOptionMenuItem_Click;
			menu.Items.Add(menuitem);
		}

		/// <summary>
		/// Set the state of the items when opening the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void optionsMenu_Opening(object sender, CancelEventArgs e)
		{
			OpeningOptionsMenu(this.optionsMenu, e);
		}

		/// <summary>
		/// Set the state of the items when opening the notify menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyMenu_Opening(object sender, CancelEventArgs e)
		{
			OpeningNotifyMenu(this.notifyMenu, e);
		}

		/// <summary>
		/// Set state of menuitems when opening the Options menu
		/// </summary>
		/// <param name="menu"></param>
		/// <param name="e"></param>
		private void OpeningOptionsMenu(ContextMenuStrip menu, CancelEventArgs e)
		{
			ToolStripItem item;
			ToolStripMenuItem menuitem;

			if (this.Config == null)
			{
				return;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "changePasswordOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Enabled = (this.Config != null && this.Config.Count != 0);
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			}
			item = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsSeparatorItem").FirstOrDefault();
			if (item != null)
			{
				item.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "startWithWindowsOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.StartWithWindows;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "alwaysOnTopOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.AlwaysOnTop;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.UseTrayIcon;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "autoSizeOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.AutoSize;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "autoSizeOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.AutoSize;
			}
		}

		/// <summary>
		/// Set state of menuitemns when opening the notify menu
		/// </summary>
		/// <param name="menu"></param>
		/// <param name="e"></param>
		private void OpeningNotifyMenu(ContextMenuStrip menu, CancelEventArgs e)
		{
			ToolStripItem item;
			ToolStripMenuItem menuitem;

			if (this.Config == null)
			{
				return;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "changePasswordOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Enabled = (this.Config != null && this.Config.Count != 0);
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			}
			item = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsSeparatorItem").FirstOrDefault();
			if (item != null)
			{
				item.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "defaultActionOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				var subitem = menuitem.DropDownItems.Cast<ToolStripItem>().Where(t => t.Name == "defaultActionNotificationOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
				subitem.Checked = (this.Config.NotifyAction == WinAuthConfig.NotifyActions.Notification);

				subitem = menuitem.DropDownItems.Cast<ToolStripItem>().Where(t => t.Name == "defaultActionCopyToClipboardOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
				subitem.Checked = (this.Config.NotifyAction == WinAuthConfig.NotifyActions.CopyToClipboard);

				subitem = menuitem.DropDownItems.Cast<ToolStripItem>().Where(t => t.Name == "defaultActionHotkeyOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
				subitem.Checked = (this.Config.NotifyAction == WinAuthConfig.NotifyActions.HotKey);
			}

			//menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			//if (menuitem != null)
			//{
			//	menuitem.Checked = this.Config.UseTrayIcon;
			//}
		}

		/// <summary>
		/// Click the Change Password item of the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void changePasswordOptionsMenuItem_Click(object sender, EventArgs e)
		{
			// confirm current password
			if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
			{
				bool invalidPassword = false;
				while (true)
				{
					GetPasswordForm checkform = new GetPasswordForm();
					checkform.InvalidPassword = invalidPassword;
					var result = checkform.ShowDialog(this);
					if (result == DialogResult.Cancel)
					{
						return;
					}
					if (this.Config.IsPassword(checkform.Password) == true)
					{
						break;
					}
					invalidPassword = true;
				}
			}

			ChangePasswordForm form = new ChangePasswordForm();
			form.PasswordType = this.Config.PasswordType;
			form.HasPassword = ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0);
			if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				bool retry;
				var retrypasswordtype = this.Config.PasswordType;
				do
				{
					retry = false;

					this.Config.PasswordType = form.PasswordType;
					if ((this.Config.PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) != 0 && form.Yubikey != null)
					{
						this.Config.Yubi = form.Yubikey;
					}
					if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0 && string.IsNullOrEmpty(form.Password) == false)
					{
						this.Config.Password = form.Password;
					}

					try
					{
						SaveConfig(true);
					}
					catch (InvalidEncryptionException)
					{
						var result = WinAuthForm.ConfirmDialog(this, "Decryption test failed. Retry?", MessageBoxButtons.YesNo);
						if (result == DialogResult.Yes)
						{
							retry = true;
							continue;
						}
						this.Config.PasswordType = retrypasswordtype;
					}
					catch (ChallengeResponseException)
					{
						var result = WinAuthForm.ConfirmDialog(this, "YubiKey Challenge/Response failed. Retry?", MessageBoxButtons.YesNo);
						if (result == DialogResult.Yes)
						{
							retry = true;
							continue;
						}
						this.Config.PasswordType = retrypasswordtype;
					}
				} while (retry);
			}
		}

		/// <summary>
		/// Click the Open item of the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void openOptionsMenuItem_Click(object sender, EventArgs e)
		{
			// show the main form
			BringToFront();
			Show();
			WindowState = FormWindowState.Normal;
			Activate();
		}

		/// <summary>
		/// Click one of the context menu authenticators
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void authenticatorOptionsMenuItem_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem menuitem = (ToolStripMenuItem)sender;
			WinAuthAuthenticator auth = menuitem.Tag as WinAuthAuthenticator;
			var item = authenticatorList.Items.Cast<AuthenticatorListitem>().Where(i => i.Authenticator == auth).FirstOrDefault();
			if (item != null)
			{
				RunAction(auth, this.Config.NotifyAction);

				//string code = authenticatorList.GetItemCode(item);
				//if (code != null)
				//{
				//	if (auth.CopyOnCode)
				//	{
				//		auth.CopyCodeToClipboard(this, code);
				//	}
				//	if (code.Length > 5)
				//	{
				//		code = code.Insert(code.Length / 2, " ");
				//	}
				//	notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
				//}
			}
		}

		/// <summary>
		/// Click the Start With Windows menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void startWithWindowsOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.StartWithWindows = !this.Config.StartWithWindows;
		}

		/// <summary>
		/// Click the Always On Top menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void alwaysOnTopOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.AlwaysOnTop = !this.Config.AlwaysOnTop;
		}

		/// <summary>
		/// Click the Use Tray Icon menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void useSystemTrayIconOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.UseTrayIcon = !this.Config.UseTrayIcon;
		}
		
		/// <summary>
		/// Click the default action options menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void defaultActionNotificationOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.NotifyAction = WinAuthConfig.NotifyActions.Notification;
		}

		/// <summary>
		/// Click the default action options menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void defaultActionCopyToClipboardOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.NotifyAction = WinAuthConfig.NotifyActions.CopyToClipboard;
		}

		/// <summary>
		/// Click the default action options menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void defaultActionHotkeyOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.NotifyAction = WinAuthConfig.NotifyActions.HotKey;
		}

		/// <summary>
		/// Click the Auto Size menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoSizeOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.AutoSize = !this.Config.AutoSize;
		}

		/// <summary>
		/// Click the Export menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exportOptionsMenuItem_Click(object sender, EventArgs e)
		{
			// confirm current password
			if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
			{
				bool invalidPassword = false;
				while (true)
				{
					GetPasswordForm checkform = new GetPasswordForm();
					checkform.InvalidPassword = invalidPassword;
					var result = checkform.ShowDialog(this);
					if (result == DialogResult.Cancel)
					{
						return;
					}
					if (this.Config.IsPassword(checkform.Password) == true)
					{
						break;
					}
					invalidPassword = true;
				}
			}

			ExportForm exportform = new ExportForm();
			if (exportform.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				WinAuthHelper.ExportAuthenticators(this, this.Config, exportform.ExportFile, exportform.Password, exportform.PGPKey);
			}
		}

		/// <summary>
		/// Click the Updates menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutUpdatesMenuItem_Click(object sender, EventArgs e)
		{
			ShowUpdaterForm();
		}

		/// <summary>
		/// Click the About menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void aboutOptionMenuItem_Click(object sender, EventArgs e)
		{
			AboutForm form = new AboutForm();
			form.Config = this.Config;
			form.ShowDialog(this);
		}

		/// <summary>
		/// Click the Exit menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void exitOptionMenuItem_Click(object sender, EventArgs e)
		{
			m_explictClose = true;
			this.Close();
		}

#endregion

#region Custom Events

    /// <summary>
    /// Event called when any property in the config is changed
    /// </summary>
    /// <param name="source"></param>
    /// <param name="args"></param>
    void OnConfigChanged(object source, ConfigChangedEventArgs args)
    {
			if (args.PropertyName == "AlwaysOnTop")
			{
				this.TopMost = this.Config.AlwaysOnTop;
			}
			else if (args.PropertyName == "UseTrayIcon")
			{
				bool useTrayIcon = Config.UseTrayIcon;
				if (useTrayIcon == false && this.Visible == false)
				{
					BringToFront();
					Show();
					WindowState = FormWindowState.Normal;
					Activate();
				}
				notifyIcon.Visible = useTrayIcon;
			}
			else if (args.PropertyName == "AutoSize" || (args.PropertyName == "Authenticator" && args.AuthenticatorChangedEventArgs.Property == "Name"))
			{
				setAutoSize();
				this.Invalidate();
			}
			else if (args.PropertyName == "StartWithWindows")
			{
				if (this.Config.IsPortable == false)
				{
					WinAuthHelper.SetStartWithWindows(this.Config.StartWithWindows);
				}
			}
			else if (args.AuthenticatorChangedEventArgs != null && args.AuthenticatorChangedEventArgs.Property == "HotKey")
			{
				// rehook hotkeys
				HookHotkeys();
			}

			// batch up saves so they can be done out of line
			SaveConfig();
    }
#endregion

		/// <summary>
		/// Inner class used to form details of hot key
		/// </summary>
		class HotKeyLauncher
		{
			/// <summary>
			/// Owning form
			/// </summary>
			public WinAuthForm Form { get; set; }

			/// <summary>
			/// Hotkey authenticator
			/// </summary>
			public WinAuthAuthenticator Authenticator { get; set; }

			/// <summary>
			/// When hotkey was pressed
			/// </summary>
			public DateTime Started { get; set; }

			/// <summary>
			/// Create a new HotKeyLauncher object
			/// </summary>
			/// <param name="form">owning Form</param>
			/// <param name="auth">Authenticator</param>
			public HotKeyLauncher(WinAuthForm form, WinAuthAuthenticator auth)
			{
				Started = DateTime.Now;
				Form = form;
				Authenticator = auth;
			}
		}



  }
}
