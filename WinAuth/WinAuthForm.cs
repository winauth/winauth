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

		private string _configFile;

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
					_configFile = arg;
        }
      }

#if BETA
			if (WinAuthHelper.BetaWarning(this) == false)
			{
				this.Close();
				return;
			}
#endif

			loadConfig(password);
		}


#region Private Methods

		private void loadConfig(string password)
		{
			// load config data
			bool retry = false;
			do
			{
				try
				{
					WinAuthConfig config = WinAuthHelper.LoadConfig(this, _configFile, password);
					if (config == null)
					{
						System.Diagnostics.Process.GetCurrentProcess().Kill();
						return;
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
					if (ErrorDialog(this, strings.UnknownError + ex.Message, ex, MessageBoxButtons.RetryCancel) == System.Windows.Forms.DialogResult.Cancel)
					{
						this.Close();
						return;
					}
					retry = true;
				}
			} while (retry == true);
		}

		private void InitializeForm()
		{
			// set up list
			loadAuthenticatorList();

			// set always on top
			this.TopMost = this.Config.AlwaysOnTop;

			// size the form based on the authenticators
			if (this.Config.AutoSize == true)
			{
				setAutoSize();
			}

			// initialize UI
			LoadAddAuthenticatorTypes();
			loadOptionsMenu();
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
					Hide();
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

		void OnWinAuthAuthenticatorChanged(object sender, WinAuthAuthenticatorChangedEventArgs e)
		{
		}

		/// <summary>
		/// Save the current config
		/// </summary>
		private void SaveConfig()
		{
			WinAuthHelper.SaveConfig(this.Config);
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
			Dictionary<Tuple<Keys, WinAPI.KeyModifiers>, WinAuthAuthenticator> keys = new Dictionary<Tuple<Keys, WinAPI.KeyModifiers>, WinAuthAuthenticator>();
			foreach (var auth in Config)
			{
				if (auth.AutoLogin != null)
				{
					keys.Add(new Tuple<Keys, WinAPI.KeyModifiers>((Keys)auth.AutoLogin.HotKey, auth.AutoLogin.Modifiers), auth);
				}
			}
			if (keys.Count != 0)
			{
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
			if (Monitor.TryEnter(m_sendingKeys) == true && e.Authenticator != null)
			{
				try
				{
					// get keyboard sender
					KeyboardSender keysend = new KeyboardSender(e.Authenticator.AutoLogin.WindowTitle, e.Authenticator.AutoLogin.ProcessName, e.Authenticator.AutoLogin.WindowTitleRegex);

					// get the script and execute it
					string script = (string.IsNullOrEmpty(e.Authenticator.AutoLogin.AdvancedScript) == false ? e.Authenticator.AutoLogin.AdvancedScript : "{CODE}");

					// send the whole script
					keysend.SendKeys(this, script, e.Authenticator.AuthenticatorData.CurrentCode);

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
				int height = this.Height - authenticatorList.Height;
				height += (this.Config.Count * authenticatorList.ItemHeight);
				this.Height = height;

				//this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
				this.Resizable = false;
			}
			else
			{
				this.Resizable = true;
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
					Hide();
				}
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
				//notifyIcon.Text = this.Text;
				Hide();
				return;
			}

			// remove the hotkey hook
			UnhookHotkeys();

			// ensure the notify icon is closed
			notifyIcon.Visible = false;
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
					// save off any new authenticators into the registry for restore
					WinAuthHelper.SaveToRegistry(winauthauthenticator);

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
					SaveConfig();
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
		/// Timer tick event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void mainTimer_Tick(object sender, EventArgs e)
		{
			authenticatorList.Tick(sender, e);
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

#endregion

#region Options menu

		/// <summary>
		/// Load the menu items for the options menu
		/// </summary>
		private void loadOptionsMenu()
		{
			ToolStripMenuItem menuitem;

			this.optionsMenu.Items.Clear();

			menuitem = new ToolStripMenuItem(strings.MenuChangeProtection + "...");
			menuitem.Name = "changePasswordOptionsMenuItem";
			menuitem.Click += changePasswordOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);
			this.optionsMenu.Items.Add(new ToolStripSeparator() { Name = "changePasswordOptionsSeparatorItem" });

			menuitem = new ToolStripMenuItem(strings.MenuOpen);
			menuitem.Name = "openOptionsMenuItem";
			menuitem.Click += openOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);
			this.optionsMenu.Items.Add(new ToolStripSeparator() { Name = "openOptionsSeparatorItem" });

			menuitem = new ToolStripMenuItem(strings.MenuStartWithWindows);
			menuitem.Name = "startWithWindowsOptionsMenuItem";
			menuitem.Click += startWithWindowsOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem(strings.MenuAlwaysOnTop);
			menuitem.Name = "alwaysOnTopOptionsMenuItem";
			menuitem.Click += alwaysOnTopOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem(strings.MenuUseSystemTrayIcon);
			menuitem.Name = "useSystemTrayIconOptionsMenuItem";
			menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem(strings.MenuAutoSize);
			menuitem.Name = "autoSizeOptionsMenuItem";
			menuitem.Click += autoSizeOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			this.optionsMenu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem(strings.MenuAbout + "...");
			menuitem.Name = "aboutOptionsMenuItem";
			menuitem.Click += aboutOptionMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			this.optionsMenu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem(strings.MenuExit);
			menuitem.Name = "exitOptionsMenuItem";
			menuitem.ShortcutKeys = Keys.F4 | Keys.Alt;
			menuitem.Click += exitOptionMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);
		}

		/// <summary>
		/// Set the state of the items when opening the Options menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void optionsMenu_Opening(object sender, CancelEventArgs e)
		{
			ToolStripItem item;
			ToolStripMenuItem menuitem;

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "changePasswordOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Enabled = (this.Config != null && this.Config.Count != 0);

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);
			item = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "openOptionsSeparatorItem").FirstOrDefault();
			item.Visible = (this.Config.UseTrayIcon == true && this.Visible == false);

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "startWithWindowsOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.StartWithWindows;

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "alwaysOnTopOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.AlwaysOnTop;

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "useSystemTrayIconOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Checked = this.Config.UseTrayIcon;

			menuitem = optionsMenu.Items.Cast<ToolStripItem>().Where(t => t.Name == "autoSizeOptionsMenuItem").FirstOrDefault() as ToolStripMenuItem;
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

				SaveConfig();
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
			else if (args.PropertyName == "AutoSize")
			{
				//this.FormBorderStyle = (this.Config.AutoSize ? System.Windows.Forms.FormBorderStyle.FixedSingle : System.Windows.Forms.FormBorderStyle.Sizable);
				setAutoSize();
				this.Invalidate();
			}
			else if (args.PropertyName == "StartWithWindows")
			{
				WinAuthHelper.SetStartWithWindows(this.Config.StartWithWindows);
			}

			SaveConfig();
    }
#endregion


  }
}
