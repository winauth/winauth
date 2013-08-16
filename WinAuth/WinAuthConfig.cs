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
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using WinAuth.Resources;

namespace WinAuth
{
  /// <summary>
  /// Delegate for ConfigChange event
  /// </summary>
  /// <param name="source"></param>
  /// <param name="args"></param>
  public delegate void ConfigChangedHandler(object source, ConfigChangedEventArgs args);

  /// <summary>
  /// Class holding configuration data for application
  /// </summary>
  [Serializable()]
	public class WinAuthConfig : IList<WinAuthAuthenticator>, ICloneable, IWinAuthAuthenticatorChangedListener
  {
    public static decimal CURRENTVERSION = decimal.Parse(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));

    /// <summary>
    /// Event handler fired when a config property is changed
    /// </summary>
    public event ConfigChangedHandler OnConfigChanged;

    /// <summary>
    /// Current file name
    /// </summary>
    private string _filename;

		/// <summary>
		/// Current version of this Config
		/// </summary>
    public decimal Version { get; private set; }

		/// <summary>
		/// Save password for re-saving and encrypting file
		/// </summary>
		public string Password { private get; set; }

		/// <summary>
		/// Current encryption type
		/// </summary>
		public Authenticator.PasswordTypes PasswordType = Authenticator.PasswordTypes.None;

    /// <summary>
    /// All authenticators
    /// </summary>
    private List<WinAuthAuthenticator> _authenticators = new List<WinAuthAuthenticator>();

    /// <summary>
    /// Current authenticator
    /// </summary>
    private WinAuthAuthenticator _authenticator;

    /// <summary>
    /// Flag for always on top
    /// </summary>
    private bool _alwaysOnTop;

    /// <summary>
    /// Flag to use tray icon
    /// </summary>
    private bool _useTrayIcon;

    /// <summary>
    /// Flag to set start with Windows
    /// </summary>
    private bool _startWithWindows;

		/// <summary>
		/// Flag to size form based on numebr authenticators
		/// </summary>
		private bool _autoSize;

		/// <summary>
		/// Width if not autosize
		/// </summary>
		private int _width;

		/// <summary>
		/// Height if not autosize
		/// </summary>
		private int _height;

		/// <summary>
		/// Class used to serialize the settings inside the Xml config file
		/// </summary>
		[XmlRoot(ElementName="settings")]
		public class setting
		{
			/// <summary>
			/// Name of dictionary entry
			/// </summary>
			[XmlAttribute(AttributeName="key")]
			public string Key;

			/// <summary>
			/// Value of dictionary entry
			/// </summary>
			[XmlAttribute(AttributeName = "value")]
			public string Value;
		}

		/// <summary>
		/// Inline settings for Portable mode
		/// </summary>
		private Dictionary<string, string> _settings = new Dictionary<string, string>();

		#region System Settings

    /// <summary>
    /// Get/set file name of config data
    /// </summary>
    public string Filename
    {
      get
      {
        return _filename;
      }
      set
      {
        _filename = value;
      }
    }

    /// <summary>
    /// Get/set on top flag
    /// </summary>
    public bool AlwaysOnTop
    {
      get
      {
        return _alwaysOnTop;
      }
      set
      {
        _alwaysOnTop = value;
        if (OnConfigChanged != null)
        {
          OnConfigChanged(this, new ConfigChangedEventArgs("AlwaysOnTop"));
        }
      }
    }

    /// <summary>
    /// Get/set use tray icon top flag
    /// </summary>
    public bool UseTrayIcon
    {
      get
      {
        return _useTrayIcon;
      }
      set
      {
        _useTrayIcon = value;
        if (OnConfigChanged != null)
        {
          OnConfigChanged(this, new ConfigChangedEventArgs("UseTrayIcon"));
        }
      }
    }

    /// <summary>
    /// Get/set start with windows flag
    /// </summary>
    public bool StartWithWindows
    {
      get
      {
        return _startWithWindows;
      }
      set
      {
        _startWithWindows = value;
        if (OnConfigChanged != null)
        {
          OnConfigChanged(this, new ConfigChangedEventArgs("StartWithWindows"));
        }
      }
    }

		/// <summary>
		/// Get/set start with windows flag
		/// </summary>
		public bool AutoSize
		{
			get
			{
				return _autoSize;
			}
			set
			{
				_autoSize = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs("AutoSize"));
				}
			}
		}

		/// <summary>
		/// Saved window width
		/// </summary>
		public int Width
		{
			get
			{
				return _width;
			}
			set
			{
				_width = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs("Width"));
				}
			}
		}

		/// <summary>
		/// Saved window height
		/// </summary>
		public int Height
		{
			get
			{
				return _height;
			}
			set
			{
				_height = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs("Height"));
				}
			}
		}

		/// <summary>
		/// Return if we are in portable mode, which is when the config filename is in teh same directory as the exe
		/// </summary>
		public bool IsPortable
		{
			get
			{
				return (string.IsNullOrEmpty(this.Filename) == false
					&& string.Compare(Path.GetDirectoryName(this.Filename), Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), true) == 0);
			}
		}

		/// <summary>
		/// Read a setting value.
		/// </summary>
		/// <param name="name">name of setting</param>
		/// <param name="defaultValue">default value if setting doesn't exist</param>
		/// <returns>setting value or default value</returns>
		public string ReadSetting(string name, string defaultValue = null)
		{
			if (this.IsPortable == true)
			{
				// read setting from _settings
				string value;
				if (_settings.TryGetValue(name, out value) == true)
				{
					return value;
				}
				else
				{
					return defaultValue;
				}
			}
			else
			{
				return WinAuthHelper.ReadRegistryValue(name, defaultValue) as string;
			}
		}

		/// <summary>
		/// Get all the settings keys beneath the specified key
		/// </summary>
		/// <param name="name">name of parent key</param>
		/// <returns>string array of all child (recursively) setting names. Empty is none.</returns>
		public string[] ReadSettingKeys(string name)
		{
			if (this.IsPortable == true)
			{
				List<string> keys = new List<string>();
				foreach (var entry in _settings)
				{
					if (entry.Key.StartsWith(name) == true)
					{
						keys.Add(entry.Key);
					}
				}
				return keys.ToArray();
			}
			else
			{
				return WinAuthHelper.ReadRegistryKeys(name);
			}
		}

		/// <summary>
		/// Write a setting value into the Config
		/// </summary>
		/// <param name="name">name of setting value</param>
		/// <param name="value">setting value. If null, the setting is deleted.</param>
		public void WriteSetting(string name, string value)
		{
			if (this.IsPortable == true)
			{
				if (value == null)
				{
					if (_settings.ContainsKey(name) == true)
					{
						_settings.Remove(name);
					}
				}
				else
				{
					_settings[name] = value;
				}
			}
			else
			{
				WinAuthHelper.WriteRegistryValue(name, value);
			}
		}

    #endregion

		#region IList

		public void Add(WinAuthAuthenticator authenticator)
		{
			authenticator.OnWinAuthAuthenticatorChanged += new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			_authenticators.Add(authenticator);
		}

		public void Clear()
		{
			_authenticators.Clear();
		}

		public bool Contains(WinAuthAuthenticator authenticator)
		{
			return _authenticators.Contains(authenticator);
		}

		public void CopyTo(int index, WinAuthAuthenticator[] array, int arrayIndex, int count)
		{
			_authenticators.CopyTo(index, array, arrayIndex, count);
		}

		public void CopyTo(WinAuthAuthenticator[] array, int index)
		{
			_authenticators.CopyTo(array, index);
		}

		public int Count
		{
			get
			{
				return _authenticators.Count;
			}
		}

		public int IndexOf(WinAuthAuthenticator authenticator)
		{
			return _authenticators.IndexOf(authenticator);
		}

		public void Insert(int index, WinAuthAuthenticator authenticator)
		{
			authenticator.OnWinAuthAuthenticatorChanged += new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			_authenticators.Insert(index, authenticator);
		}

		public bool IsReadOnly
		{
			get
			{
				return false;
			}
		}

		public bool Remove(WinAuthAuthenticator authenticator)
		{
			return _authenticators.Remove(authenticator);
		}

		public void RemoveAt(int index)
		{
			_authenticators.RemoveAt(index);
		}

		public IEnumerator<WinAuthAuthenticator> GetEnumerator()
		{
			return _authenticators.GetEnumerator();
		}

		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		public WinAuthAuthenticator this[int index]
		{
			get
			{
				return _authenticators[index];
			}
			set
			{
				throw new NotImplementedException();
			}
		}

		#endregion

		#region Authenticator Settings

    /// <summary>
    /// Current authenticator
    /// </summary>
    public WinAuthAuthenticator CurrentAuthenticator
    {
      get
      {
        return _authenticator;
      }
      set
      {
        _authenticator = value;
      }
    }

    #endregion

    /// <summary>
    /// Create a default config object
    /// </summary>
    public WinAuthConfig()
    {
      Version = CURRENTVERSION;
      //AlwaysOnTop = true;
			AutoSize = true;
    }

		public void OnWinAuthAuthenticatorChanged(WinAuthAuthenticator sender, WinAuthAuthenticatorChangedEventArgs e)
		{
			if (OnConfigChanged != null)
			{
				OnConfigChanged(this, new ConfigChangedEventArgs("Authenticator", sender, e));
			}
		}

    #region ICloneable

    /// <summary>
    /// Clone return a new WinAuthConfig object
    /// </summary>
    /// <returns></returns>
    public object Clone()
    {
      WinAuthConfig clone = (WinAuthConfig)this.MemberwiseClone();
      // close the internal authenticator so the data is kept separate
      clone.OnConfigChanged = null;
      clone._authenticators = new List<WinAuthAuthenticator>();
      foreach (var wa in _authenticators)
      {
        clone._authenticators.Add(wa.Clone() as WinAuthAuthenticator);
      }
      clone.CurrentAuthenticator = (this.CurrentAuthenticator != null ? clone._authenticators[this._authenticators.IndexOf(this.CurrentAuthenticator)] : null);
      return clone;
    }

    public void ReadXml(XmlReader reader, string password = null)
    {
      reader.Read();
      while (reader.EOF == false && reader.IsEmptyElement == true)
      {
        reader.Read();
      }
      reader.MoveToContent();
      while (reader.EOF == false)
      {
        if (reader.IsStartElement())
        {
          switch (reader.Name)
          {
            case "WinAuth":
              ReadXmlInternal(reader, password);
              break;

            default:
              reader.Skip();
              break;
          }
        }
        else
        {
          reader.Read();
          break;
        }
      }
    }

    protected void ReadXmlInternal(XmlReader reader, string password = null)
    {
      decimal version;
      if (decimal.TryParse(reader.GetAttribute("version"), out version) == true)
      {
        Version = version;
      }

      string encrypted = reader.GetAttribute("encrypted");
			this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
      if (this.PasswordType != Authenticator.PasswordTypes.None)
      {
        // read the encrypted text from the node
        string data = reader.ReadElementContentAsString();
        // decrypt
				data = Authenticator.DecryptSequence(data, this.PasswordType, password);

				using (MemoryStream ms = new MemoryStream(Authenticator.StringToByteArray(data)))
				{
          reader = XmlReader.Create(ms);
          ReadXml(reader, password);
        }

				this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
				this.Password = password;

        return;
      }

      reader.MoveToContent();
      if (reader.IsEmptyElement)
      {
        reader.Read();
        return;
      }

      bool defaultAutoRefresh = true;
      bool defaultAllowCopy = false;
      bool defaultCopyOnCode = false;
      bool defaultHideSerial = true;
      string defaultSkin = null;

      reader.Read();
      while (reader.EOF == false)
      {
        if (reader.IsStartElement())
        {
          switch (reader.Name)
          {
						case "config":
							ReadXmlInternal(reader, password);
							break;

            case "alwaysontop":
              _alwaysOnTop = reader.ReadElementContentAsBoolean();
              break;

            case "usetrayicon":
              _useTrayIcon = reader.ReadElementContentAsBoolean();
              break;

            case "startwithwindows":
              _startWithWindows = reader.ReadElementContentAsBoolean();
              break;

						case "autosize":
							_autoSize = reader.ReadElementContentAsBoolean();
							break;

						case "width":
							_width = reader.ReadElementContentAsInt();
							break;

						case "height":
							_height = reader.ReadElementContentAsInt();
							break;

						case "settings":
							XmlSerializer serializer = new XmlSerializer(typeof(setting[]), new XmlRootAttribute() { ElementName = "settings" });
							_settings = ((setting[])serializer.Deserialize(reader)).ToDictionary(e => e.Key, e => e.Value);
							break;

            // previous setting used as defaults for new
            case "autorefresh":
              defaultAutoRefresh = reader.ReadElementContentAsBoolean();
              break;
            case "allowcopy":
              defaultAllowCopy = reader.ReadElementContentAsBoolean();
              break;
            case "copyoncode":
              defaultCopyOnCode = reader.ReadElementContentAsBoolean();
              break;
            case "hideserial":
              defaultHideSerial = reader.ReadElementContentAsBoolean();
              break;
            case "skin":
              defaultSkin = reader.ReadElementContentAsString();
              break;

            case "WinAuthAuthenticator":
							var wa = new WinAuthAuthenticator();
              wa.ReadXml(reader, password);
              this.Add(wa);
							if (this.CurrentAuthenticator == null)
							{
								this.CurrentAuthenticator = wa;
							}
              break;

            // for old 2.x configs
            case "authenticator":
							var waold = new WinAuthAuthenticator();
              waold.AuthenticatorData = Authenticator.ReadXmlv2(reader, password);
              if (waold.AuthenticatorData is BattleNetAuthenticator)
              {
                waold.Name = "Battle.net";
              }
              else if (waold.AuthenticatorData is GuildWarsAuthenticator)
              {
                waold.Name = "GuildWars 2";
              }
              else if (waold.AuthenticatorData is GuildWarsAuthenticator)
              {
                waold.Name = "Authenticator";
              }
              this.Add(waold);
              this.CurrentAuthenticator = waold;
              waold.AutoRefresh = defaultAutoRefresh;
              waold.AllowCopy = defaultAllowCopy;
              waold.CopyOnCode = defaultCopyOnCode;
              waold.HideSerial = defaultHideSerial;
              break;

						// old 2.x auto login script
						case "autologin":
              var hks = new HoyKeySequence();
              hks.ReadXml(reader, password);
							if (hks.HotKey != 0)
							{
								if (this.CurrentAuthenticator.HotKey == null)
								{
									this.CurrentAuthenticator.HotKey = new HotKey();
								}
								HotKey hotkey = this.CurrentAuthenticator.HotKey;
								hotkey.Action = HotKey.HotKeyActions.Inject;
								hotkey.Key = hks.HotKey;
								hotkey.Modifiers = hks.Modifiers;
								if (hks.WindowTitleRegex == true && string.IsNullOrEmpty(hks.WindowTitle) == false)
								{
									hotkey.Window = "/" + Regex.Escape(hks.WindowTitle);
								}
								else if (string.IsNullOrEmpty(hks.WindowTitle) == false)
								{
									hotkey.Window = hks.WindowTitle;
								}
								else if (string.IsNullOrEmpty(hks.ProcessName) == false)
								{
									hotkey.Window = hks.ProcessName;
								}
								if (hks.Advanced == true)
								{
									hotkey.Action = HotKey.HotKeyActions.Advanced;
									hotkey.Advanced = hks.AdvancedScript;
								}
							}
              break;

            default:
              reader.Skip();
              break;
          }
        }
        else
        {
          reader.Read();
          break;
        }
      }
    }

    /// <summary>
    /// Write the data as xml into an XmlWriter
    /// </summary>
    /// <param name="writer">XmlWriter to write config</param>
    public void WriteXmlString(XmlWriter writer, bool includeFilename = false, bool includeSettings = true)
    {
      writer.WriteStartDocument(true);
      //
      if (includeFilename == true && string.IsNullOrEmpty(this.Filename) == false)
      {
        writer.WriteComment(this.Filename);
      }
      //
      writer.WriteStartElement("WinAuth");
      writer.WriteAttributeString("version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
      if (PasswordType != Authenticator.PasswordTypes.None)
      {
				writer.WriteStartElement("config");

				StringBuilder encryptedTypes = new StringBuilder();
				if ((PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
				{
					encryptedTypes.Append("y");
				}
				if ((PasswordType & Authenticator.PasswordTypes.User) != 0)
				{
					encryptedTypes.Append("u");
				}
				if ((PasswordType & Authenticator.PasswordTypes.Machine) != 0)
				{
					encryptedTypes.Append("m");
				}
				writer.WriteAttributeString("encrypted", encryptedTypes.ToString());
				
				string data;
        using (MemoryStream ms = new MemoryStream())
        {
          XmlWriterSettings settings = new XmlWriterSettings();
          settings.Indent = true;
          settings.Encoding = Encoding.UTF8;
          using (XmlWriter encryptedwriter = XmlWriter.Create(ms, settings))
          {
            Authenticator.PasswordTypes savedpasswordType = PasswordType;
            PasswordType = Authenticator.PasswordTypes.None;
            WriteXmlString(encryptedwriter, includeFilename, false);
            PasswordType = savedpasswordType;
          }
          data = Authenticator.ByteArrayToString(ms.ToArray());
        }

				data = Authenticator.EncryptSequence(data, PasswordType, Password);
				writer.WriteString(data);
				writer.WriteEndElement();

				if (includeSettings == true && _settings.Count != 0)
				{
					XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
					ns.Add(string.Empty, string.Empty);
					XmlSerializer serializer = new XmlSerializer(typeof(setting[]), new XmlRootAttribute() { ElementName = "settings" });
					serializer.Serialize(writer, _settings.Select(e => new setting { Key = e.Key, Value = e.Value }).ToArray(), ns);
				}

				writer.WriteEndElement();

        return;
      }
      //
      writer.WriteStartElement("alwaysontop");
      writer.WriteValue(this.AlwaysOnTop);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("usetrayicon");
      writer.WriteValue(this.UseTrayIcon);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("startwithwindows");
      writer.WriteValue(this.StartWithWindows);
      writer.WriteEndElement();
			//
			writer.WriteStartElement("autosize");
			writer.WriteValue(this.AutoSize);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("width");
			writer.WriteValue(this.Width);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("height");
			writer.WriteValue(this.Height);
			writer.WriteEndElement();
			//
			if (includeSettings == true && _settings.Count != 0)
			{
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add(string.Empty, string.Empty);
				XmlSerializer serializer = new XmlSerializer(typeof(setting[]), new XmlRootAttribute() { ElementName = "settings" });
				serializer.Serialize(writer, _settings.Select(e => new setting { Key = e.Key, Value = e.Value }).ToArray(), ns);
			}

      foreach (WinAuthAuthenticator wa in this)
      {
        wa.WriteXmlString(writer);
      }

      // close WinAuth
      writer.WriteEndElement();

      // end document
      writer.WriteEndDocument();
    }

    #endregion

	}

  /// <summary>
  /// Config change event arguments
  /// </summary>
  public class ConfigChangedEventArgs : EventArgs
  {
		public string PropertyName { get; private set; }

		public WinAuthAuthenticator Authenticator { get; private set; }
		public WinAuthAuthenticatorChangedEventArgs AuthenticatorChangedEventArgs { get; private set; }

    /// <summary>
    /// Default constructor
    /// </summary>
		public ConfigChangedEventArgs(string propertyName, WinAuthAuthenticator authenticator = null, WinAuthAuthenticatorChangedEventArgs acargs = null)
			: base()
		{
			PropertyName = propertyName;
			Authenticator = authenticator;
			AuthenticatorChangedEventArgs = acargs;
		}
	}

  public class WinAuthInvalidConfigException : ApplicationException
  {
    public WinAuthInvalidConfigException(string msg, Exception ex)
      : base(msg, ex)
    {
    }
  }
  public class WinAuthConfigRequirePasswordException : ApplicationException
  {
    public WinAuthConfigRequirePasswordException()
      : base()
    {
    }
  }

}
