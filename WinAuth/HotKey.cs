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
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace WinAuth
{
	/// <summary>
	/// A hot key sequence and command containing the key, modifier and script
	/// </summary>
	public class HotKey
	{
		public enum HotKeyActions
		{
			Inject,
			Copy,
			Advanced
		};

		/// <summary>
		/// Modifier for hotkey
		/// </summary>
		public WinAPI.KeyModifiers Modifiers;

		/// <summary>
		/// Hotkey code
		/// </summary>
		public WinAPI.VirtualKeyCode Key;

		/// <summary>
		/// Action to be perform on hotkey
		/// </summary>
		public HotKeyActions Action;

		/// <summary>
		/// Specific window title or process name
		/// </summary>
		public string Window;

		/// <summary>
		/// Copy of advanced script from authenticator data
		/// </summary>
		public string Advanced;

		/// <summary>
		/// Create a new HotKey
		/// </summary>
		public HotKey()
		{
			Action = HotKeyActions.Inject;
		}

		/// <summary>
		/// Read the saved Xml to load the hotkey
		/// </summary>
		/// <param name="reader">XmlReader</param>
    public void ReadXml(XmlReader reader)
    {
      reader.MoveToContent();

      if (reader.IsEmptyElement)
      {
        reader.Read();
        return;
      }

      reader.Read();
      while (reader.EOF == false)
      {
        if (reader.IsStartElement())
        {
          switch (reader.Name)
          {
            case "modifiers":
      				Modifiers = (WinAPI.KeyModifiers)BitConverter.ToInt32(Authenticator.StringToByteArray(reader.ReadElementContentAsString()), 0);
              break;

            case "key":
              Key = (WinAPI.VirtualKeyCode)BitConverter.ToUInt16(Authenticator.StringToByteArray(reader.ReadElementContentAsString()), 0);
              break;

						case "action":
							Action = (HotKeyActions)Enum.Parse(typeof(HotKeyActions), reader.ReadElementContentAsString(), true);
							break;

						case "window":
							Window = reader.ReadElementContentAsString();
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
		/// Write data into the XmlWriter
		/// </summary>
		/// <param name="writer">XmlWriter to write to</param>
		public void WriteXmlString(XmlWriter writer)
		{
			writer.WriteStartElement("hotkey");

			writer.WriteStartElement("modifiers");
			writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((int)Modifiers)));
			writer.WriteEndElement();
			//
			writer.WriteStartElement("key");
			writer.WriteString(Authenticator.ByteArrayToString(BitConverter.GetBytes((ushort)Key)));
			writer.WriteEndElement();
			//
			writer.WriteStartElement("action");
			writer.WriteString(Enum.GetName(typeof(HotKeyActions), this.Action));
			writer.WriteEndElement();
			//
			if (String.IsNullOrEmpty(this.Window) == false)
			{
				writer.WriteStartElement("window");
				writer.WriteString(this.Window);
				writer.WriteEndElement();
			}

			writer.WriteEndElement();
		}

	}

}
