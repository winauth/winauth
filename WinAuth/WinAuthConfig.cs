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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

#if NUNIT
using NUnit.Framework;
#endif

namespace WindowsAuthenticator
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
	public class WinAuthConfig : ICloneable
	{
		/// <summary>
		/// Event handler fired when a config property is changed
		/// </summary>
		public event ConfigChangedHandler OnConfigChanged;

		/// <summary>
		/// Current file name
		/// </summary>
		private string _filename;

		/// <summary>
		/// Current authenticator
		/// </summary>
		private Authenticator _authenticator;

		/// <summary>
		/// Current skin
		/// </summary>
		private string _currentSkin;

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
		/// Flag for auto refresh of code
		/// </summary>
		private bool _autoRefresh;

		/// <summary>
		/// Flag to allow copy of code
		/// </summary>
		private bool _allowCopy;

		/// <summary>
		/// Flag to copy new code automatically
		/// </summary>
		private bool _copyOnCode;

		/// <summary>
		/// Flag to hide serial number
		/// </summary>
		private bool _hideSerial = true;

		/// <summary>
		/// Auto login sequence
		/// </summary>
		private HoyKeySequence _autoLogin;

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
					OnConfigChanged(this, new ConfigChangedEventArgs());
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
					OnConfigChanged(this, new ConfigChangedEventArgs());
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
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Get/set the currnet skin
		/// </summary>
		public string CurrentSkin
		{
			get
			{
				if (RememberSkin == true)
				{
					_currentSkin = WinAuthHelper.GetSavedSkin();
				}
				return _currentSkin;
			}
			set
			{
				_currentSkin = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Flag to remember skin and put in registry
		/// </summary>
		public bool RememberSkin
		{
			get
			{
				return (string.IsNullOrEmpty(WinAuthHelper.GetSavedSkin()) == false);
			}
			set
			{
				if (value == true)
				{
					WinAuthHelper.SetSavedSkin(CurrentSkin);
				}
				else
				{
					WinAuthHelper.SetSavedSkin(null);
				}
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		#endregion

		#region Authenticator Settings

		/// <summary>
		/// Current authenticator
		/// </summary>
		public Authenticator Authenticator
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

		/// <summary>
		/// Get/set auto refresh flag
		/// </summary>
		public bool AutoRefresh
		{
			get
			{
				return _autoRefresh;
			}
			set
			{
				_autoRefresh = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Get/set allow copy flag
		/// </summary>
		public bool AllowCopy
		{
			get
			{
				return _allowCopy;
			}
			set
			{
				_allowCopy = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Get/set auto copy flag
		/// </summary>
		public bool CopyOnCode
		{
			get
			{
				return _copyOnCode;
			}
			set
			{
				_copyOnCode = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Get/set hide serial flag
		/// </summary>
		public bool HideSerial
		{
			get
			{
				return _hideSerial;
			}
			set
			{
				_hideSerial = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		/// <summary>
		/// Any auto login hotkey
		/// </summary>
		public HoyKeySequence AutoLogin
		{
			get
			{
				return _autoLogin;
			}
			set
			{
				_autoLogin = value;
				if (OnConfigChanged != null)
				{
					OnConfigChanged(this, new ConfigChangedEventArgs());
				}
			}
		}

		#endregion

		/// <summary>
		/// Create a default config object
		/// </summary>
		public WinAuthConfig()
		{
			AlwaysOnTop = true;
			AutoRefresh = true;
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
			clone.Authenticator = (this.Authenticator == null ? null : this.Authenticator.Clone() as Authenticator);
			return clone;
		}

		/// <summary>
		/// Write the data as xml into an XmlWriter
		/// </summary>
		/// <param name="writer">XmlWriter to write config</param>
		public void WriteXmlString(XmlWriter writer, bool includeFilename = false)
		{
			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

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
			writer.WriteStartElement("startwithwindows");
			writer.WriteValue(this.StartWithWindows);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("autorefresh");
			writer.WriteValue(this.AutoRefresh);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("allowcopy");
			writer.WriteValue(this.AllowCopy);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("copyoncode");
			writer.WriteValue(this.CopyOnCode);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("hideserial");
			writer.WriteValue(this.HideSerial);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("skin");
			writer.WriteValue(_currentSkin ?? string.Empty);
			writer.WriteEndElement();

			// save the authenticator to the config file
			if (this.Authenticator != null)
			{
				//this.Authenticator.WriteXmlString(writer);
				this.Authenticator.WriteToWriter(writer);
			}

			// save script with password and generated salt
			if (this.AutoLogin != null)
			{
				this.AutoLogin.WriteXmlString(writer, this.Authenticator.PasswordType, this.Authenticator.Password);
			}

			// close WinAuth
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

		#endregion
	}

	/// <summary>
	/// Config change event arguments
	/// </summary>
	public class ConfigChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Default constructor
		/// </summary>
		public ConfigChangedEventArgs() : base()
		{
		}
	}

#if NUNIT
	[TestFixture]
	public class WinAuthConfig_Text
	{
		public WinAuthConfig_Text()
		{
		}

		/// <summary>
		/// Test cases to load and save each combination of modifiers and Vkeys
		/// </summary>
		[Test]
		public void HoyKeySequence_Load()
		{
			// all possible modifiers
			WinAPI.KeyModifiers[] modifiers = new WinAPI.KeyModifiers[]
			{
				WinAPI.KeyModifiers.None,
				WinAPI.KeyModifiers.Alt,
				WinAPI.KeyModifiers.Control,
				WinAPI.KeyModifiers.Shift,
				WinAPI.KeyModifiers.Alt | WinAPI.KeyModifiers.Control,
				WinAPI.KeyModifiers.Alt | WinAPI.KeyModifiers.Shift,
				WinAPI.KeyModifiers.Control | WinAPI.KeyModifiers.Shift,
				WinAPI.KeyModifiers.Alt | WinAPI.KeyModifiers.Control | WinAPI.KeyModifiers.Shift
			};

			// loop through each VKey
			foreach (WinAPI.KeyModifiers modifier in modifiers)
			{
				HoyKeySequence hk1 = new HoyKeySequence(null);
				hk1.Modifiers = modifier;
				hk1.Advanced = false;

				foreach (WinAPI.VirtualKeyCode vk in Enum.GetValues(typeof(WinAPI.VirtualKeyCode)))
				{
					hk1.HotKey = vk;
					string s1 = hk1.ToString();
					HoyKeySequence hk2 = new HoyKeySequence(s1);
					string s2 = hk2.ToString();
					Console.Out.WriteLine(Convert.ToSingle(modifier) + ":" + Convert.ToString(vk));
					Assert.AreEqual(s1, s2);
				}
			}
		}
	}
#endif
}
