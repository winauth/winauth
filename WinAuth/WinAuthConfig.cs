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
using System.Text;
using System.Text.RegularExpressions;

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
		/// Get a string representation of the HotKeySequence
		/// </summary>
		/// <returns>string representation</returns>
		public override string ToString()
		{
			StringBuilder sb = new StringBuilder();
			sb.Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((int)Modifiers)));
			sb.Append(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort)HotKey)));
			sb.Append("\t");
			sb.Append(WindowTitle ?? string.Empty);
			sb.Append("\t");
			sb.Append(Advanced == true ? "Y" : "N");
			if (Advanced == true)
			{
				sb.Append(AdvancedScript.Replace("\n", string.Empty));
			}

			return sb.ToString();
		}
	}

	/// <summary>
	/// Class holding configuration data for application
	/// </summary>
	public class WinAuthConfig : ICloneable
	{
		/// <summary>
		/// Get/set name of current authentication data file
		/// </summary>
		public string AuthenticatorFile { get; set; }

		/// <summary>
		/// Get/set auto refresh flag
		/// </summary>
		public bool AutoRefresh { get; set; }

		/// <summary>
		/// Get/set on top flag
		/// </summary>
		public bool AlwaysOnTop { get; set; }

		/// <summary>
		/// Get/set hide on minimize top flag
		/// </summary>
		public bool HideOnMinimize { get; set; }

		/// <summary>
		/// Get/set start with windows flag
		/// </summary>
		public bool StartWithWindows{ get; set; }

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

		/// <summary>
		/// Create a default config object
		/// </summary>
		public WinAuthConfig()
		{
			AutoRefresh = true;
			AlwaysOnTop = true;
		}

		#region ICloneable

		/// <summary>
		/// Clone return a new WinAuthConfig object
		/// </summary>
		/// <returns></returns>
		public object Clone()
		{
			return this.MemberwiseClone();
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
