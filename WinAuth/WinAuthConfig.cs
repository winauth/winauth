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
	/// A hot key sequence and command containing the key, modifier and script
	/// </summary>
	public class HoyKeySequence
	{
		/// <summary>
		/// Modifier for hotkey
		/// </summary>
		public WinAPI.KeyModifiers Modifiers;

		/// <summary>
		/// Hotkey code
		/// </summary>
		public WinAPI.VirtualKeyCode HotKey;

		/// <summary>
		/// Flag if custom script
		/// </summary>
		public bool Advanced;

		/// <summary>
		/// Any custom script
		/// </summary>
		public string AdvancedScript;

		/// <summary>
		/// Windows title for script
		/// </summary>
		public string WindowTitle;

		/// <summary>
		/// Use regular expression for window title
		/// </summary>
		public bool WindowTitleRegex;

		/// <summary>
		/// Process name for script
		/// </summary>
		public string ProcessName;

		/// <summary>
		/// Create a new blank HotKeySequcen
		/// </summary>
		public HoyKeySequence()
		{
		}

		/// <summary>
		/// Create a new HotKeySequence from a loaded string
		/// </summary>
		/// <param name="data"></param>
		public HoyKeySequence(string data)
		{
			if (string.IsNullOrEmpty(data) == false)
			{
				Match match = Regex.Match(data, @"([0-9a-fA-F]{8})([0-9a-fA-F]{4})\t([^\t]*)\t(Y|N)(.*)", RegexOptions.Multiline);
				if (match.Success == true)
				{
					Modifiers = (WinAPI.KeyModifiers)BitConverter.ToInt32(Authenticator.StringToByteArray(match.Groups[1].Value), 0);
					HotKey = (WinAPI.VirtualKeyCode)BitConverter.ToUInt16(Authenticator.StringToByteArray(match.Groups[2].Value), 0);
					WindowTitle = match.Groups[3].Value;
					Advanced = (match.Groups[4].Value == "Y");
					if (Advanced == true)
					{
						AdvancedScript = match.Groups[5].Value;
					}
				}
			}
		}

		/// <summary>
		/// Create a new HotKeySequence from a loaded string
		/// </summary>
		/// <param name="data">XmlNode from config</param>
		public HoyKeySequence(XmlNode autoLoginNode, string password)
		{
			bool boolVal = false;
			XmlNode node = autoLoginNode.SelectSingleNode("modifiers");
			if (node != null && node.InnerText.Length != 0)
			{
				Modifiers = (WinAPI.KeyModifiers)BitConverter.ToInt32(Authenticator.StringToByteArray(node.InnerText), 0);
			}
			node = autoLoginNode.SelectSingleNode("hotkey");
			if (node != null && node.InnerText.Length != 0)
			{
				HotKey = (WinAPI.VirtualKeyCode)BitConverter.ToUInt16(Authenticator.StringToByteArray(node.InnerText), 0);
			}
			node = autoLoginNode.SelectSingleNode("windowtitle");
			if (node != null && node.InnerText.Length != 0)
			{
				WindowTitle = node.InnerText;
			}
			node = autoLoginNode.SelectSingleNode("windowtitleregex");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				WindowTitleRegex = boolVal;
			}
			node = autoLoginNode.SelectSingleNode("processname");
			if (node != null && node.InnerText.Length != 0)
			{
				ProcessName = node.InnerText;
			}
			node = autoLoginNode.SelectSingleNode("advanced");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				Advanced = boolVal;
			}
			node = autoLoginNode.SelectSingleNode("script");
			if (node != null && node.InnerText.Length != 0)
			{
				string data = node.InnerText;

				XmlAttribute attr = node.Attributes["encrypted"];
				if (attr != null && attr.InnerText.Length != 0)
				{
					switch (attr.InnerText)
					{
						case "u":
							{
								// we are going to decrypt with the Windows User account key
								byte[] cipher = Authenticator.StringToByteArray(data);
								byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
								data = Encoding.UTF8.GetString(plain, 0, plain.Length);
								break;
							}
						case "m":
							{
								// we are going to decrypt with the Windows local machine key
								byte[] cipher = Authenticator.StringToByteArray(data);
								byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
								data = Encoding.UTF8.GetString(plain, 0, plain.Length);
								break;
							}
						case "y":
							{
								// we use an explicit password to encrypt data
								if (string.IsNullOrEmpty(password) == true)
								{
									throw new EncrpytedSecretDataException();
								}
								data = AuthenticatorData.Decrypt(data, password);
								byte[] plain = Authenticator.StringToByteArray(data);
								data = Encoding.UTF8.GetString(plain, 0, plain.Length);
								break;
							}
						default:
							break;
					}
				}
				AdvancedScript = data;
			}
		}

		/// <summary>
		/// Write data into the XmlWriter
		/// </summary>
		/// <param name="writer">XmlWriter to write to</param>
		public void WriteXmlString(XmlWriter writer, AuthenticatorData.PasswordTypes passwordType, string password)
		{
			writer.WriteStartElement("autologin");

			writer.WriteStartElement("modifiers");
			writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((int)Modifiers)));
			writer.WriteEndElement();
			//
			writer.WriteStartElement("hotkey");
			writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort)HotKey)));
			writer.WriteEndElement();
			//
			writer.WriteStartElement("windowtitle");
			writer.WriteCData(WindowTitle ?? string.Empty);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("processname");
			writer.WriteCData(ProcessName ?? string.Empty);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("windowtitleregex");
			writer.WriteValue(WindowTitleRegex);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("advanced");
			writer.WriteValue(Advanced);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("script");
			string script = AdvancedScript.Replace("\n", string.Empty);
			switch (passwordType)
			{
				case AuthenticatorData.PasswordTypes.Explicit:
					{
						byte[] plain = Encoding.UTF8.GetBytes(script);
						script = Authenticator.ByteArrayToString(plain);
						script = AuthenticatorData.Encrypt(script, password);
						writer.WriteAttributeString("encrypted", "y");
						break;
					}
				case AuthenticatorData.PasswordTypes.User:
					{
						byte[] plain = Encoding.UTF8.GetBytes(script);
						byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
						script = Authenticator.ByteArrayToString(cipher);
						writer.WriteAttributeString("encrypted", "u");
						break;
					}
				case AuthenticatorData.PasswordTypes.Machine:
					{
						// we encrypt the data using the Local Machine account key
						byte[] plain = Encoding.UTF8.GetBytes(script);
						byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.LocalMachine);
						script = Authenticator.ByteArrayToString(cipher);
						writer.WriteAttributeString("encrypted", "m");
						break;
					}
				default:
					break;
			}
			writer.WriteCData(script);
			//writer.WriteCData(AdvancedScript.Replace("\n", string.Empty));
			writer.WriteEndElement();

			writer.WriteEndElement();
		}
	
		/// <summary>
		/// Get an xml string representation of the HotKeySequence
		/// </summary>
		/// <returns>string representation</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append("<modifiers>").Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((int)Modifiers))).Append("</modifiers>");
			sb.Append("<hotkey>").Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort)HotKey))).Append("</hotkey>");
			sb.Append("<windowtitle><![CDATA[").Append(WindowTitle ?? string.Empty).Append("]]></windowtitle>");
			sb.Append("<processname><![CDATA[").Append(ProcessName ?? string.Empty).Append("]]></processname>");
			sb.Append("<windowtitleregex>").Append(WindowTitleRegex.ToString()).Append("</windowtitleregex>");
			sb.Append("<advanced>").Append(Advanced.ToString()).Append("</advanced>");
			sb.Append("<script><![CDATA[").Append(AdvancedScript.Replace("\n", string.Empty)).Append("]]></script>");

			return sb.ToString();
		}
	}

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
