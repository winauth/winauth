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
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security;
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
    public static decimal CURRENTVERSION = decimal.Parse(System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2), System.Globalization.CultureInfo.InvariantCulture);

		/// <summary>
		/// Default actions for double-click or click system tray
		/// </summary>
		public enum NotifyActions
		{
			Notification = 0,
			CopyToClipboard = 1,
			HotKey = 2
		}

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
		public string Password { protected get; set; }

		/// <summary>
		/// Save Yubi key for re-saving and encrypting file
		/// </summary>
		public YubiKey Yubi { get; set; }

		/// <summary>
		/// If the config was upgraded
		/// </summary>
		public bool Upgraded { get; set; }

		/// <summary>
		/// Current encryption type
		/// </summary>
		private Authenticator.PasswordTypes _passwordType = Authenticator.PasswordTypes.None;

		/// <summary>
		/// Get/set the encryption type
		/// </summary>
		public Authenticator.PasswordTypes PasswordType
		{
			get
			{
				return _passwordType;
			}
			set
			{
				_passwordType = value;

				if ((_passwordType & Authenticator.PasswordTypes.Explicit) == 0)
				{
					this.Password = null;
				}
				if ((_passwordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) == 0)
				{
					this.Yubi = null;
				}
			}
		}


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
		/// Default action when click in system tray
		/// </summary>
		private NotifyActions _notifyAction;

		/// <summary>
		/// Flag to set start with Windows
		/// </summary>
		private bool _startWithWindows;

		/// <summary>
		/// Flag to size form based on numebr authenticators
		/// </summary>
		private bool _autoSize;

		/// <summary>
		/// Remember position
		/// </summary>
		private Point _position = Point.Empty;

		/// <summary>
		/// Width if not autosize
		/// </summary>
		private int _width;

		/// <summary>
		/// Height if not autosize
		/// </summary>
		private int _height;

		/// <summary>
		/// This config is readonly
		/// </summary>
		private bool _readOnly;

		/// <summary>
		/// MetroFormShadowType for main form
		/// </summary>
		private string _shadowType;

		/// <summary>
		/// User's own PGPKey for backups
		/// </summary>
		private string _pgpKey;

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
		/// Default action when using the system tray or double-click
		/// </summary>
		public NotifyActions NotifyAction
		{
			get
			{
				return _notifyAction;
			}
			set
			{
				_notifyAction = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs("NotifyAction"));
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
		/// Get/set the position
		/// </summary>
		public Point Position
		{
			get
			{
				return _position;
			}
			set
			{
				if (_position != value)
				{
					_position = value;
					if (OnConfigChanged != null)
					{
						OnConfigChanged(this, new ConfigChangedEventArgs("Position"));
					}
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
				if (_width != value)
				{
					_width = value;
					if (OnConfigChanged != null)
					{
						OnConfigChanged(this, new ConfigChangedEventArgs("Width"));
					}
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
				if (_height != value)
				{
					_height = value;
					if (OnConfigChanged != null)
					{
						OnConfigChanged(this, new ConfigChangedEventArgs("Height"));
					}
				}
			}
		}

		/// <summary>
		/// Get/set shadow type for main form
		/// </summary>
		public string ShadowType
		{
			get
			{
				return _shadowType;
			}
			set
			{
				_shadowType = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs("ShadowType"));
				}
			}
		}

		/// <summary>
		/// Get user's own PGP key
		/// </summary>
		public string PGPKey
		{
			get
			{
				return _pgpKey;
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
				if (value == null)
				{
					WinAuthHelper.DeleteRegistryKey(name);
				}
				else
				{
					WinAuthHelper.WriteRegistryValue(name, value);
				}
			}
		}

		/// <summary>
		/// Check if a given password is the config's password
		/// </summary>
		/// <param name="password"></param>
		/// <returns></returns>
		public bool IsPassword(string password)
		{
			return (string.Compare(password, this.Password) == 0);
		}

    #endregion

		#region IList

		/// <summary>
		/// Rewrite the index values in the WinAuthAuthenticator objects to match our order
		/// </summary>
		private void SetIndexes()
		{
			int count = this.Count;
			for (int i = 0; i < count; i++)
			{
				this[i].Index = i;
			}
		}

		/// <summary>
		/// Add a new authenticator
		/// </summary>
		/// <param name="authenticator">WinAuthAuthenticator instance</param>
		public void Add(WinAuthAuthenticator authenticator)
		{
			authenticator.OnWinAuthAuthenticatorChanged += new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			_authenticators.Add(authenticator);
			SetIndexes();
		}

		/// <summary>
		/// Remove all the authenticators
		/// </summary>
		public void Clear()
		{
			foreach (WinAuthAuthenticator authenticator in this)
			{
				authenticator.Index = 0;
				authenticator.OnWinAuthAuthenticatorChanged -= new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			}
			_authenticators.Clear();
		}

		/// <summary>
		/// Check if the config contains an authenticator
		/// </summary>
		/// <param name="authenticator"></param>
		/// <returns></returns>
		public bool Contains(WinAuthAuthenticator authenticator)
		{
			return _authenticators.Contains(authenticator);
		}

		/// <summary>
		/// Copy elements from the list to an array
		/// </summary>
		/// <param name="index"></param>
		/// <param name="array"></param>
		/// <param name="arrayIndex"></param>
		/// <param name="count"></param>
		public void CopyTo(int index, WinAuthAuthenticator[] array, int arrayIndex, int count)
		{
			_authenticators.CopyTo(index, array, arrayIndex, count);
		}

		/// <summary>
		/// Copy the list into an array
		/// </summary>
		/// <param name="array"></param>
		/// <param name="index"></param>
		public void CopyTo(WinAuthAuthenticator[] array, int index)
		{
			_authenticators.CopyTo(array, index);
		}

		/// <summary>
		/// return the count of authenticators
		/// </summary>
		public int Count
		{
			get
			{
				return _authenticators.Count;
			}
		}

		/// <summary>
		/// Get the index of an authenticator
		/// </summary>
		/// <param name="authenticator"></param>
		/// <returns></returns>
		public int IndexOf(WinAuthAuthenticator authenticator)
		{
			return _authenticators.IndexOf(authenticator);
		}

		/// <summary>
		/// Insert an authenticator at a specified position
		/// </summary>
		/// <param name="index"></param>
		/// <param name="authenticator"></param>
		public void Insert(int index, WinAuthAuthenticator authenticator)
		{
			authenticator.OnWinAuthAuthenticatorChanged += new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			_authenticators.Insert(index, authenticator);
			SetIndexes();
		}

		/// <summary>
		/// Return if this list is read only
		/// </summary>
		public bool IsReadOnly
		{
			get
			{
				return _readOnly;
			}
			set
			{
				_readOnly = value;
			}
		}

		/// <summary>
		/// Remove an authenticator
		/// </summary>
		/// <param name="authenticator"></param>
		/// <returns></returns>
		public bool Remove(WinAuthAuthenticator authenticator)
		{
			authenticator.OnWinAuthAuthenticatorChanged -= new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			bool result = _authenticators.Remove(authenticator);
			SetIndexes();
			return result;
		}

		/// <summary>
		/// Remove an authenticator from a specific position
		/// </summary>
		/// <param name="index"></param>
		public void RemoveAt(int index)
		{
			_authenticators[index].OnWinAuthAuthenticatorChanged -= new WinAuthAuthenticatorChangedHandler(this.OnWinAuthAuthenticatorChanged);
			_authenticators.RemoveAt(index);
			SetIndexes();
		}

		/// <summary>
		/// Get an enumerator for the authenticators
		/// </summary>
		/// <returns></returns>
		public IEnumerator<WinAuthAuthenticator> GetEnumerator()
		{
			return _authenticators.GetEnumerator();
		}

		/// <summary>
		/// Get an enumerator for the authenticators
		/// </summary>
		/// <returns></returns>
		IEnumerator IEnumerable.GetEnumerator()
		{
			return GetEnumerator();
		}

		/// <summary>
		/// Indexer to get an authenticator by postion
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
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

		/// <summary>
		/// Sort the authenticator by their index value
		/// </summary>
		public void Sort()
		{
			_authenticators.Sort((a,b) => a.Index.CompareTo(b.Index));
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
			AutoSize = true;
			NotifyAction = NotifyActions.Notification;
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

    public bool ReadXml(XmlReader reader, string password = null)
    {
			bool changed = false;

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
						// v2 file
            case "WinAuth":
              changed = ReadXmlInternal(reader, password);
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

			return changed;
    }

    protected bool ReadXmlInternal(XmlReader reader, string password = null)
    {
			bool changed = false;

      decimal version;
			if (decimal.TryParse(reader.GetAttribute("version"), System.Globalization.NumberStyles.Any, System.Globalization.CultureInfo.InvariantCulture, out version) == true)
      {
        Version = version;

				if (version > WinAuthConfig.CURRENTVERSION)
				{
					// ensure we don't overwrite a newer config
					throw new WinAuthInvalidNewerConfigException(string.Format(strings.ConfigIsNewer, version));
				}
			}

      string encrypted = reader.GetAttribute("encrypted");
			this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
      if (this.PasswordType != Authenticator.PasswordTypes.None)
      {
        // read the encrypted text from the node
        string data = reader.ReadElementContentAsString();
        // decrypt
				YubiKey yubi = null;
				if ((this.PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) != 0 /* && this.Yubi == null */)
				{
					yubi = YubiKey.CreateInstance();
				}
				data = Authenticator.DecryptSequence(data, this.PasswordType, password, yubi);

				using (MemoryStream ms = new MemoryStream(Authenticator.StringToByteArray(data)))
				{
          reader = XmlReader.Create(ms);
          changed = ReadXml(reader, password);
        }

				this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
				this.Password = password;
				this.Yubi = yubi;

        return changed;
      }

      reader.MoveToContent();
      if (reader.IsEmptyElement)
      {
        reader.Read();
        return changed;
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
							changed = ReadXmlInternal(reader, password) || changed;
							break;

						// 3.2 has new layout 
						case "data":
							{
								encrypted = reader.GetAttribute("encrypted");
								this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
								if (this.PasswordType != Authenticator.PasswordTypes.None)
								{
                  HashAlgorithm hasher;
									string hash = reader.GetAttribute("sha1");
                  if (string.IsNullOrEmpty(hash) == false)
                  {
                    hasher = Authenticator.SafeHasher("SHA1");
                  }
                  else
                  {
                    // old version has md5
                    hash = reader.GetAttribute("md5");
                    hasher = Authenticator.SafeHasher("MD5");
                  }
                  // read the encrypted text from the node
                  string data = reader.ReadElementContentAsString();

									hasher.ComputeHash(Authenticator.StringToByteArray(data));
#if !NETFX_3
                  hasher.Dispose();
#endif

                  // decrypt
                  YubiKey yubi = null;
									if ((this.PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) != 0 /* && this.Yubi == null */)
									{
										yubi = YubiKey.CreateInstance();
									}
									data = Authenticator.DecryptSequence(data, this.PasswordType, password, yubi);
									byte[] plain = Authenticator.StringToByteArray(data);

									using (MemoryStream ms = new MemoryStream(plain))
									{
										var datareader = XmlReader.Create(ms);
										changed = ReadXmlInternal(datareader, password) || changed;
									}

									this.PasswordType = Authenticator.DecodePasswordTypes(encrypted);
									this.Password = password;
									this.Yubi = yubi;
								}
							}
							break;
						
						case "alwaysontop":
              _alwaysOnTop = reader.ReadElementContentAsBoolean();
              break;

            case "usetrayicon":
              _useTrayIcon = reader.ReadElementContentAsBoolean();
              break;

						case "notifyaction":
							string s = reader.ReadElementContentAsString();
							if (string.IsNullOrEmpty(s) == false)
							{
								try {
									_notifyAction = (NotifyActions)Enum.Parse(typeof(NotifyActions), s, true);
								}
								catch (Exception) { }
							}
							break;

						case "startwithwindows":
              _startWithWindows = reader.ReadElementContentAsBoolean();
              break;

						case "autosize":
							_autoSize = reader.ReadElementContentAsBoolean();
							break;

						case "left":
							_position.X = reader.ReadElementContentAsInt();
							break;

						case "top":
							_position.Y = reader.ReadElementContentAsInt();
							break;

						case "width":
							_width = reader.ReadElementContentAsInt();
							break;

						case "height":
							_height = reader.ReadElementContentAsInt();
							break;

						case "shadowtype":
							_shadowType = reader.ReadElementContentAsString();
							break;

						case "pgpkey":
							_pgpKey = reader.ReadElementContentAsString();
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
              changed = wa.ReadXml(reader, password) || changed;
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

			return changed;
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
      //
      writer.WriteStartElement("alwaysontop");
      writer.WriteValue(this.AlwaysOnTop);
      writer.WriteEndElement();
      //
      writer.WriteStartElement("usetrayicon");
      writer.WriteValue(this.UseTrayIcon);
      writer.WriteEndElement();
			//
			writer.WriteStartElement("notifyaction");
			writer.WriteValue(Enum.GetName(typeof(NotifyActions), this.NotifyAction));
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
			if (this.Position.IsEmpty == false)
			{
				writer.WriteStartElement("left");
				writer.WriteValue(this.Position.X);
				writer.WriteEndElement();
				writer.WriteStartElement("top");
				writer.WriteValue(this.Position.Y);
				writer.WriteEndElement();
			}
			//
			writer.WriteStartElement("width");
			writer.WriteValue(this.Width);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("height");
			writer.WriteValue(this.Height);
			writer.WriteEndElement();
			//
			if (string.IsNullOrEmpty(this.ShadowType) == false)
			{
				writer.WriteStartElement("shadowtype");
				writer.WriteValue(this.ShadowType);
				writer.WriteEndElement();
			}
			//
			if (string.IsNullOrEmpty(this.PGPKey) == false)
			{
				writer.WriteStartElement("pgpkey");
				writer.WriteCData(this.PGPKey);
				writer.WriteEndElement();
			}

			if (PasswordType != Authenticator.PasswordTypes.None)
			{
				writer.WriteStartElement("data");

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
				if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot1) != 0)
				{
					encryptedTypes.Append("a");
				}
				if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot2) != 0)
				{
					encryptedTypes.Append("b");
				}
				writer.WriteAttributeString("encrypted", encryptedTypes.ToString());

				byte[] data;
				using (MemoryStream ms = new MemoryStream())
				{
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					settings.Encoding = Encoding.UTF8;
					using (XmlWriter encryptedwriter = XmlWriter.Create(ms, settings))
					{
						encryptedwriter.WriteStartElement("config");
						foreach (WinAuthAuthenticator wa in this)
						{
							wa.WriteXmlString(encryptedwriter);
						}
						encryptedwriter.WriteEndElement();
					}

					data = ms.ToArray();
				}

				using (var hasher = Authenticator.SafeHasher("SHA1"))
				{
					string encdata = Authenticator.EncryptSequence(Authenticator.ByteArrayToString(data), PasswordType, Password, this.Yubi);
					string enchash = Authenticator.ByteArrayToString(hasher.ComputeHash(Authenticator.StringToByteArray(encdata)));
					writer.WriteAttributeString("sha1", enchash);
					writer.WriteString(encdata);
				}
		
				writer.WriteEndElement();
			}
			else
			{
				foreach (WinAuthAuthenticator wa in this)
				{
					wa.WriteXmlString(writer);
				}
			}

			if (includeSettings == true && _settings.Count != 0)
			{
				XmlSerializerNamespaces ns = new XmlSerializerNamespaces();
				ns.Add(string.Empty, string.Empty);
				XmlSerializer serializer = new XmlSerializer(typeof(setting[]), new XmlRootAttribute() { ElementName = "settings" });
				serializer.Serialize(writer, _settings.Select(e => new setting { Key = e.Key, Value = e.Value }).ToArray(), ns);
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
	public class WinAuthInvalidNewerConfigException : ApplicationException
	{
		public WinAuthInvalidNewerConfigException(string msg)
			: base(msg)
		{
		}
	}

}
