using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

    public WinAuthConfig Config { get; set; }

    #endregion

    private void WinAuthForm_Resize(object sender, EventArgs e)
    {

    }

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
			authenticatorList.Items.Clear();
			int index = 0;
			foreach (var auth in Config.Authenticators)
			{
				authenticatorList.Items.Add(new AuthenticatorListitem(auth, index));
				index++;
			}
    }


#region Private Methods

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

		private void mainTimer_Tick(object sender, EventArgs e)
		{
			authenticatorList.Tick(sender, e);
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
      // resave on any changes
      if (Config != null && string.IsNullOrEmpty(Config.Filename) == false)
      {
        //SaveAuthenticator(Config.Filename);
      }
    }
#endregion

  }
}
