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
using System.Threading;
using System.Threading.Tasks;
using System.Xml;
using System.Windows.Forms;

using MetroFramework;
using MetroFramework.Forms;

using WinAuth.Resources;
using System.Security;

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
						default:
              break;
          }
        }
        else
        {
					_startupConfigFile = arg;
        }
      }

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

			// load config data
			bool retry = false;
			do
			{
				try
				{
					WinAuthConfig config = WinAuthHelper.LoadConfig(this, configFile, password);
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

					InitializeForm();
				}
				catch (EncrpytedSecretDataException)
				{
					passwordPanel.Visible = true;
				}
				catch (BadPasswordException)
				{
					passwordPanel.Visible = true;
					this.passwordErrorLabel.Text = strings.InvalidPassword;
					this.passwordErrorLabel.Tag = DateTime.Now.AddSeconds(3);
					this.passwordTimer.Enabled = true;
				}
				catch (Exception ex)
				{
					if (ErrorDialog(this, strings.UnknownError + ": " + ex.Message, ex, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						this.Close();
						return;
					}
					retry = true;
				}
			} while (retry == true);
		}

		/// <summary>
		/// Import a v2 authenticator from an existing file name
		/// </summary>
		/// <param name="authenticatorFile">name of v2 xml file</param>
		private void importAuthenticator(string authenticatorFile)
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
							this.Config.PasswordType = form.PasswordType;
							if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
							{
								this.Config.Password = form.Password;
							}
							else
							{
								this.Config.Password = null;
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
				catch (EncrpytedSecretDataException)
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

		/// <summary>
		/// Initialise the current form and UI
		/// </summary>
		private void InitializeForm()
		{
#if BETA
			string betaversion = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(3);
			string betaConfirmed = this.Config.ReadSetting(WINAUTHREGKEY_BETAWARNING, null) as string;
			if (string.Compare(betaConfirmed, betaversion) != 0)
			{
				if (new BetaForm().ShowDialog(this) != DialogResult.OK)
				{
					this.Close();
					return;
				}

				this.Config.WriteSetting(WINAUTHREGKEY_BETAWARNING, betaversion);
				SaveConfig(true);
			}
#endif

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
			loadOptionsMenu(this.notifyMenu);
			passwordPanel.Visible = false;
			commandPanel.Visible = true;
			introLabel.Visible = (this.Config.Count == 0);
			authenticatorList.Visible = (this.Config.Count != 0);

			// set title
			notifyIcon.Visible = this.Config.UseTrayIcon;
			notifyIcon.Text = this.Text = WinAuthMain.APPLICATION_TITLE;

			// hook hotkeys
			HookHotkeys();

			// save the position of the list within the form else starting as minimized breaks the size
			_listoffset = new Rectangle(authenticatorList.Left, authenticatorList.Top, (this.Width - authenticatorList.Width), (this.Height - authenticatorList.Height));

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
				if (added != null && added == auth && auth.AutoRefresh == false)
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
		/// Event for an authenticator being changed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void OnWinAuthAuthenticatorChanged(object sender, WinAuthAuthenticatorChangedEventArgs e)
		{
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
			subitem.Text = strings.MenuImportWinauth;
			subitem.Name = "importAuthenticatorMenuItem";
			subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources.WinAuthIcon.png"));
			subitem.ImageAlign = ContentAlignment.MiddleLeft;
			subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
			subitem.Click += importAuthenticatorMenu_Click;
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
				code = auth.AuthenticatorData.CurrentCode;
			}
			catch (EncrpytedSecretDataException)
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
					code = code.Insert(code.Length / 2, " ");
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

				int height = this.Height - authenticatorList.Height;
				height += (this.Config.Count * authenticatorList.ItemHeight);
				this.Height = height;

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
					importAuthenticator(_existingv2Config);
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

			// remove the hotkey hook
			UnhookHotkeys();

			// ensure the notify icon is closed
			notifyIcon.Visible = false;

			// save size if we are not autoresize
			if (this.Config != null && this.Config.AutoSize == false && (this.Config.Width != this.Width || this.Config.Height != this.Height))
			{
				this.Config.Width = this.Width;
				this.Config.Height = this.Height;
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
							this.Config.PasswordType = form.PasswordType;
							if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
							{
								this.Config.Password = form.Password;
							}
							else
							{
								this.Config.Password = null;
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
		/// Click to import an existing WinAuth 2.x authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void importAuthenticatorMenu_Click(object sender, EventArgs e)
		{
			ToolStripItem menuitem = (ToolStripItem)sender;

			OpenFileDialog ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.DefaultExt = "xml";
			ofd.FileName = "authenticator.xml";
			string lastv2file = WinAuthHelper.GetLastV2Config();
			if (string.IsNullOrEmpty(lastv2file) == false)
			{
				ofd.InitialDirectory = Path.GetDirectoryName(lastv2file);
				ofd.FileName = Path.GetFileName(lastv2file);
			}
			ofd.Filter = "WinAuth (*.xml)|*.xml|All Files (*.*)|*.*";
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
			loadOptionsMenu(this.notifyMenu);

			// update UI
			setAutoSize();

			// save the current config
			SaveConfig();
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
		/// CLick the button to enter a password
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

				// oddity with MetroFrame controls in have to set focus away and back to field to make it stick
				this.Invoke((MethodInvoker)delegate { this.passwordButton.Focus(); this.passwordField.Focus(); });
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

#endregion

#region Options menu

		/// <summary>
		/// Load the menu items for the options menu
		/// </summary>
		private void loadOptionsMenu(ContextMenuStrip menu)
		{
			ToolStripMenuItem menuitem;

			menu.Items.Clear();

			menuitem = new ToolStripMenuItem(strings.MenuOpen);
			menuitem.Name = "openOptionsMenuItem";
			menuitem.Click += openOptionsMenuItem_Click;
			menu.Items.Add(menuitem);
			menu.Items.Add(new ToolStripSeparator() { Name = "openOptionsSeparatorItem" });

			if (this.Config != null && this.Config.Count != 0)
			{
				var index = 1;
				foreach (var auth in this.Config)
				{
					menuitem = new ToolStripMenuItem(index.ToString() + ". " + auth.Name);
					menuitem.Name = "authenticatorOptionsMenuItem_" + index;
					menuitem.Tag = auth;
					menuitem.ShortcutKeyDisplayString = (auth.HotKey != null ? auth.HotKey.ToString() : null);
					menuitem.Click += authenticatorOptionsMenuItem_Click;
					menuitem.Visible = false;
					menu.Items.Add(menuitem);
					index++;
				}
				var separator = new ToolStripSeparator();
				separator.Name = "authenticatorOptionsSeparatorItem";
				separator.Visible = false;
				menu.Items.Add(separator);
			}

			menuitem = new ToolStripMenuItem(strings.MenuChangeProtection + "...");
			menuitem.Name = "changePasswordOptionsMenuItem";
			menuitem.Click += changePasswordOptionsMenuItem_Click;
			menu.Items.Add(menuitem);
			menu.Items.Add(new ToolStripSeparator() { Name = "changePasswordOptionsSeparatorItem" });

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
		/// Set the state of the items when opening the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void optionsMenu_Opening(object sender, CancelEventArgs e)
		{
			OpeningOptionsMenu(this.optionsMenu, false, e);
		}

		/// <summary>
		/// Set the state of the items when opening the notify menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void notifyMenu_Opening(object sender, CancelEventArgs e)
		{
			OpeningOptionsMenu(this.notifyMenu, true, e);
		}

		private void OpeningOptionsMenu(ContextMenuStrip menu, bool notify, CancelEventArgs e)
		{
			ToolStripItem item;
			ToolStripMenuItem menuitem;

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "changePasswordOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Enabled = (this.Config != null && this.Config.Count != 0);

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			item = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsSeparatorItem").FirstOrDefault();
			item.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);

			for (int i = 1; i <= this.Config.Count; i++)
			{
				menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "authenticatorOptionsMenuItem_" + i).FirstOrDefault() as ToolStripMenuItem;
				if (menuitem != null)
				{
					menuitem.Visible = notify;
				}
			}
			item = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "authenticatorOptionsSeparatorItem").FirstOrDefault();
			if (item != null)
			{
				item.Visible = notify;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "startWithWindowsOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				menuitem.Checked = this.Config.StartWithWindows;
			}

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "alwaysOnTopOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.AlwaysOnTop;

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.UseTrayIcon;

			menuitem = menu.Items.Cast<ToolStripItem>().Where(t => t.Name == "autoSizeOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.AutoSize;
		}

		/// <summary>
		/// Click the Change Password item of the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void changePasswordOptionsMenuItem_Click(object sender, EventArgs e)
		{
			ChangePasswordForm form = new ChangePasswordForm();
			form.PasswordType = this.Config.PasswordType;
			if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
			{
				this.Config.PasswordType = form.PasswordType;
				if ((this.Config.PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
				{
					this.Config.Password = form.Password;
				}
				else
				{
					this.Config.Password = null;
				}

				SaveConfig(true);
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
				string code = authenticatorList.GetItemCode(item);
				if (code != null)
				{
					if (auth.CopyOnCode)
					{
						auth.CopyCodeToClipboard(this, code);
					}
					code = code.Insert(code.Length / 2, " ");
					notifyIcon.ShowBalloonTip(10000, auth.Name, code, ToolTipIcon.Info);
				}
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
		/// Click the Auto Size menu item
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoSizeOptionsMenuItem_Click(object sender, EventArgs e)
		{
			this.Config.AutoSize = !this.Config.AutoSize;
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
