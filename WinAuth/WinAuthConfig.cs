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
	/// Class holding configuration data for application
	/// </summary>
	public class WinAuthConfig : ICloneable
	{
		#region System Settings

		/// <summary>
		/// Get/set file name of config data
		/// </summary>
		public string Filename { get; set; }

		/// <summary>
		/// Get/set on top flag
		/// </summary>
		public bool AlwaysOnTop { get; set; }

		/// <summary>
		/// Get/set use tray icon top flag
		/// </summary>
		public bool UseTrayIcon { get; set; }

		/// <summary>
		/// Get/set start with windows flag
		/// </summary>
		public bool StartWithWindows { get; set; }

		#endregion

		#region Authenticator Settings

		/// <summary>
		/// Current authenticator
		/// </summary>
		public Authenticator Authenticator { get; set; }

		/// <summary>
		/// Get/set auto refresh flag
		/// </summary>
		public bool AutoRefresh { get; set; }

		/// <summary>
		/// Get/set allow copy flag
		/// </summary>
		public bool AllowCopy { get; set; }

		/// <summary>
		/// Get/set auto copy flag
		/// </summary>
		public bool CopyOnCode { get; set; }

		/// <summary>
		/// Get/set hide serial flag
		/// </summary>
		public bool HideSerial { get; set; }

		/// <summary>
		/// Any auto login hotkey
		/// </summary>
		public HoyKeySequence AutoLogin { get; set; }

		#endregion

		/// <summary>
		/// Create a default config object
		/// </summary>
		public WinAuthConfig()
		{
			AlwaysOnTop = true;
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
			clone.Authenticator = (this.Authenticator == null ? null : new Authenticator((AuthenticatorData)this.Authenticator.Data.Clone()));
			return clone;
		}

		/// <summary>
		/// Write the data as xml into an XmlWriter
		/// </summary>
		/// <param name="writer">XmlWriter to write config</param>
		public void WriteXmlString(XmlWriter writer)
		{
			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

			writer.WriteStartDocument(true);
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
			if (this.AutoLogin != null)
			{
				this.AutoLogin.WriteXmlString(writer, this.Authenticator.Data.PasswordType, this.Authenticator.Data.Password);
			}
			//
			//if (string.IsNullOrEmpty(config.AuthenticatorFile) == false)
			//{
			//  node = doc.CreateElement("AuthenticatorFile");
			//  node.InnerText = config.AuthenticatorFile.ToString();
			//  root.AppendChild(node);
			//}

			// save the authenticator to the config file
			this.Authenticator.Data.WriteXmlString(writer);

			// close WinAuth
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

		#endregion
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
