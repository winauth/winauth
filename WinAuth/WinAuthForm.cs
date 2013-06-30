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

namespace WinAuth
{
	public partial class WinAuthForm : MetroForm
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

    #endregion

		/// <summary>
		/// Load the main form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
    private void WinAuthForm_Load(object sender, EventArgs e)
    {
      // get any command arguments
      string configFile = null;
      string password = null;
      string skin = null;
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
            case "-s":
            case "--skin":
              // set skin name
              i++;
              skin = args[i];
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

#if BETA
			if (WinAuthHelper.BetaWarning(this) == false)
			{
				this.Close();
				return;
			}
#endif

      // load config data
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

					this.Config = config;
					this.Config.OnConfigChanged += new ConfigChangedHandler(OnConfigChanged);

					break;
				}
				catch (EncrpytedSecretDataException )
				{
					PasswordForm form = new PasswordForm();
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						password = form.Password;
					}
				}
				catch (BadPasswordException )
				{
					PasswordForm form = new PasswordForm();
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						password = form.Password;
					}
				}
			} while (true);

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
			introLabel.Visible = (this.Config.Authenticators.Count == 0);

			// set title
			notifyIcon.Visible = this.Config.UseTrayIcon;
			notifyIcon.Text = this.Text = WinAuthMain.APPLICATION_TITLE;

			// hook hotkeys
			HookHotkeys();
		}


#region Private Methods

		/// <summary>
		/// Load the authenticators into the display list
		/// </summary>
		/// <param name="added">authenticator we just added</param>
		private void loadAuthenticatorList(WinAuthAuthenticator added = null)
		{
			// set up list
			authenticatorList.Items.Clear();
			int index = 0;
			foreach (var auth in Config.Authenticators)
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
		}

		/// <summary>
		/// Save the current config
		/// </summary>
		private void SaveConfig()
		{
			WinAuthHelper.SaveConfig(this.Config);
		}

/*
    /// <summary>
    /// Load a new authenticator by prompting for a file
    /// </summary>
    private void LoadAuthenticator(string configFile = null)
    {
      if (string.IsNullOrEmpty(configFile) == true)
      {
        // use default or the path of the current authenticator
        string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuthMain.APPLICATION_NAME);
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
        ofd.Filter = "WinAuth (*.xml)|All Files (*.*)|*.*";
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
        bool save = false;

        // if there is no filename, we imported a different authenticator, so clone some of the current config
        if (string.IsNullOrEmpty(config.Filename) == true)
        {
          // set specific authenticator settings
          config.AllowCopy = Config.AllowCopy;
          config.AlwaysOnTop = Config.AlwaysOnTop;
          config.AutoRefresh = Config.AutoRefresh;
          config.CopyOnCode = Config.CopyOnCode;
          config.HideSerial = Config.HideSerial;
          config.UseTrayIcon = Config.UseTrayIcon;

          // set the filename
          config.Filename = configFile;

          save = true;
        }

        // set up the Authenticator
        Config = config;

        // unhook and rehook hotkey
        //HookHotkey(this.Config);

        // set skin
        if (string.IsNullOrEmpty(Config.CurrentSkin) == false)
        {
          //LoadSkin(Config.CurrentSkin);
        }
        // show the new code
        //ShowCode();

        // re-save
        //if (save == true)
        //{
        //  SaveAuthenticator(Config.Filename);
        //}

        // set title
        //notifyIcon.Text = this.Text = WinAuthMain.APPLICATION_TITLE + " - " + Path.GetFileNameWithoutExtension(Config.Filename);

        // hook event for config changes
        this.Config.OnConfigChanged += new ConfigChangedHandler(OnConfigChanged);
      }
    }
*/

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
				message = "An error has occurred" + (ex != null ? ": " + ex.Message : string.Empty);
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
			foreach (var auth in Config.Authenticators)
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
					clipRetry = (MessageBox.Show(this, "Unable to copy to the clipboard. Another application is probably using it.\n\nTry again?",
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
					clipRetry = (MessageBox.Show(this, "Unable to copy to the clipboard. Another application is probably using it.\n\nTry again?",
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
				height += (this.Config.Authenticators.Count * authenticatorList.ItemHeight);
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
		/// Set up the notify icon when the main form is shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_Shown(object sender, EventArgs e)
		{
			// if we use tray icon make sure it is set
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
		/// Minimize to icon when closing or unbind and close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void WinAuthForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// keep in the tray when closing Form 
			if (Config.UseTrayIcon == true && this.Visible == true && m_explictClose == false)
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
				if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.BattleNet)
				{
					// add the new authenticator
					WinAuthAuthenticator winauthauthenticator = new WinAuthAuthenticator();

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
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						this.Config.Authenticators.Add(winauthauthenticator);

						SaveConfig();

						loadAuthenticatorList(winauthauthenticator);
					}
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

					WinAuthAuthenticator winauthauthenticator = new WinAuthAuthenticator();
					winauthauthenticator.Name = name;
					winauthauthenticator.AutoRefresh = false;

					AddTrionAuthenticator form = new AddTrionAuthenticator();
					form.Authenticator = winauthauthenticator;
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						this.Config.Authenticators.Add(winauthauthenticator);

						SaveConfig();

						loadAuthenticatorList(winauthauthenticator);
					}
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.Google)
				{
					// create the Google authenticator
					AddGoogleAuthenticator form = new AddGoogleAuthenticator();
					if (form.ShowDialog(this) == System.Windows.Forms.DialogResult.OK)
					{
						// add the new authenticator
						WinAuthAuthenticator winauthauthenticator = form.Authenticator;
						if (string.IsNullOrEmpty(winauthauthenticator.Name) == true)
						{
							int existing = 0;
							string name;
							do
							{
								name = "Google" + (existing != 0 ? " (" + existing + ")" : string.Empty);
								existing++;
							} while (authenticatorList.Items.Cast<AuthenticatorListitem>().Where(a => a.Authenticator.Name == name).Count() != 0);
							winauthauthenticator.Name = name;
						}
						winauthauthenticator.AutoRefresh = false;
						this.Config.Authenticators.Add(winauthauthenticator);

						SaveConfig();

						loadAuthenticatorList(winauthauthenticator);
					}
				}
				else if (registeredauth.AuthenticatorType == RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME)
				{
					// create the RFC6238 time based authenticator
				}
				else
				{
					throw new NotImplementedException("TRAP: New authenticator not implemented for " + registeredauth.AuthenticatorType.ToString());
				}

				// reset UI
				setAutoSize();
				introLabel.Visible = (this.Config.Authenticators.Count == 0);

				// reset hotkeeys
				HookHotkeys();
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
			foreach (var auth in Config.Authenticators)
			{
				if (auth == args.Item.Authenticator)
				{
					Config.Authenticators.Remove(auth);
					break;
				}
			}

			SaveConfig();

			// update UI
			setAutoSize();
			introLabel.Visible = (this.Config.Authenticators.Count == 0);
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

			menuitem = new ToolStripMenuItem("Open");
			menuitem.Name = "openOptionsMenuItem";
			menuitem.Click += openOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);
			this.optionsMenu.Items.Add(new ToolStripSeparator() { Name = "openOptionsSeparatorItem" });

			menuitem = new ToolStripMenuItem("Start With Windows");
			menuitem.Name = "startWithWindowsOptionsMenuItem";
			menuitem.Click += startWithWindowsOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem("Always on Top");
			menuitem.Name = "alwaysOnTopOptionsMenuItem";
			menuitem.Click += alwaysOnTopOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem("Use System Tray Icon");
			menuitem.Name = "useSystemTrayIconOptionsMenuItem";
			menuitem.Click += useSystemTrayIconOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			menuitem = new ToolStripMenuItem("Auto Size");
			menuitem.Name = "autoSizeOptionsMenuItem";
			menuitem.Click += autoSizeOptionsMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			this.optionsMenu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem("About...");
			menuitem.Name = "aboutOptionsMenuItem";
			menuitem.Click += aboutOptionMenuItem_Click;
			this.optionsMenu.Items.Add(menuitem);

			this.optionsMenu.Items.Add(new ToolStripSeparator());

			menuitem = new ToolStripMenuItem("Exit");
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

			SaveConfig();
    }
#endregion

  }
}
