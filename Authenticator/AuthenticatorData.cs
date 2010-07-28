/* Copyright (C) 2010 Colin Mackie.
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
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Xml;
using System.Xml.Serialization;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class holding the authentication data that can be loaded/saved to secure storage. This contains the Authenticator's
	/// Secret Key and Serial number which are used to calculate the authentication code.
	/// </summary>
	public class AuthenticatorData
	{
		/// <summary>
		/// Name attribute of the <string> hash element in the Mobile Authenticator file
		/// </summary>
		private const string BMA_HASH_NAME = "com.blizzard.bma.AUTH_STORE.HASH";

		/// <summary>
		/// Name attribute of the <long> offset element in the Mobile Authenticator file
		/// </summary>
		private const string BMA_OFFSET_NAME = "com.blizzard.bma.AUTH_STORE.CLOCK_OFFSET";

		/// <summary>
		/// Private encrpytion key used to encrypt mobile authenticator data.
		/// </summary>
		private static byte[] MOBILE_AUTHENTICATOR_KEY = new byte[]
			{
				0x39,0x8e,0x27,0xfc,0x50,0x27,0x6a,0x65,0x60,0x65,0xb0,0xe5,0x25,0xf4,0xc0,0x6c,
				0x04,0xc6,0x10,0x75,0x28,0x6b,0x8e,0x7a,0xed,0xa5,0x9d,0xa9,0x81,0x3b,0x5d,0xd6,
				0xc8,0x0d,0x2f,0xb3,0x80,0x68,0x77,0x3f,0xa5,0x9b,0xa4,0x7c,0x17,0xca,0x6c,0x64,
				0x79,0x01,0x5c,0x1d,0x5b,0x8b,0x8f,0x6b,0x9a
			};


		/// <summary>
		/// Region for authenticator
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// Secret key used for Authenticator
		/// </summary>
		public byte[] SecretKey { get; set; }

		/// <summary>
		/// Serial number of authenticator
		/// </summary>
		public string Serial { get; set; }

		/// <summary>
		/// Time difference in milliseconds of our machine and server
		/// </summary>
		public long ServerTimeDiff { get; set; }

		/// <summary>
		/// Get/set the data saved for the secret data value
		/// </summary>
		protected string SecretData
		{
			get
			{
				string code = Authenticator.ByteArrayToString(SecretKey) + Serial;
				byte[] plain = Encoding.UTF8.GetBytes(code);
				for (int i = plain.Length - 1; i >= 0; i--)
				{
					plain[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
				}
				return Authenticator.ByteArrayToString(plain);
			}
			set
			{
				byte[] bytes = Authenticator.StringToByteArray(value);
				for (int i = bytes.Length - 1; i >= 0; i--)
				{
					bytes[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
				}
				string full = Encoding.UTF8.GetString(bytes);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40);
			}
		}

		/// <summary>
		/// Create a blank AuthenticatorData object
		/// </summary>
		public AuthenticatorData()
		{
		}

		/// <summary>
		/// Create a new AuthenticatorData object loaded from saved data
		/// </summary>
		/// <param name="data">previous saved data</param>
		public AuthenticatorData(XmlReader xr)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(xr);

			// is the Mobile Authenticator file? <xml.../><map>...</map>
			XmlNode node = doc.DocumentElement.SelectSingleNode("/map/string[@name='" + BMA_HASH_NAME + "']");
			if (node != null)
			{
				string data = node.InnerText;

				// extract the secret key and serial
				byte[] bytes = Authenticator.StringToByteArray(data);
				// decrpyt with the fixed key
				for (int i = bytes.Length - 1; i >= 0; i--)
				{
					bytes[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
				}
				// decode and set members
				string full = Encoding.UTF8.GetString(bytes);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40);

				// get offset value
				long offset = 0;
				node = doc.DocumentElement.SelectSingleNode("/map/long[@name='" + BMA_OFFSET_NAME + "']");
				if (node != null && long.TryParse(node.Attributes["value"].InnerText, out offset) == true)
				{
					ServerTimeDiff = offset;
				}

				return;
			}

			// read version 0.3 config
			node = doc.DocumentElement.SelectSingleNode("/AuthenticatorData[@version='0.3']/Data");
			if (node != null)
			{
				string data = node.InnerText;

				byte[] bytes = Authenticator.StringToByteArray(data);
				string full = Encoding.UTF8.GetString(bytes);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40, 17);
				Region = full.Substring(57, 2);
				byte[] serverTimeDiff = Authenticator.StringToByteArray(full.Substring(59, 16));
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(serverTimeDiff);
				}
				ServerTimeDiff = BitConverter.ToInt64(serverTimeDiff, 0);
				return;
			}

			// read our curent config string
			node = doc.DocumentElement.SelectSingleNode("/map/string[@name='data']");
			if (node != null)
			{
				SecretData = node.InnerText;

				long offset = 0;
				node = doc.DocumentElement.SelectSingleNode("long[@name='servertimediff']");
				if (node != null && long.TryParse(node.InnerText, out offset) == true)
				{
					ServerTimeDiff = offset;
				}

				node = doc.DocumentElement.SelectSingleNode("string[@name='region']");
				if (node != null)
				{
					Region = node.InnerText;
				}

				return;
			}

			// unexpected data
			throw new InvalidConfigDataException();
		}

		/// <summary>
		/// Write the data object to an XmlWriter 
		/// </summary>
		/// <param name="writer"></param>
		public void WriteXmlString(XmlWriter writer)
		{
			writer.WriteStartDocument(true);
			writer.WriteStartElement("map");
			writer.WriteAttributeString("version", System.Reflection.Assembly.GetAssembly(typeof(AuthenticatorData)).GetName().Version.ToString(2));
			//
			writer.WriteStartElement("long");
			writer.WriteAttributeString("name", "servertimediff");
			writer.WriteString(ServerTimeDiff.ToString());
			writer.WriteEndElement();
			//
			writer.WriteStartElement("string");
			writer.WriteAttributeString("name", "region");
			writer.WriteString(Region);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("string");
			writer.WriteAttributeString("name", "data");
			writer.WriteString(SecretData);
			writer.WriteEndElement();
			//
			writer.WriteEndElement();
			writer.WriteEndDocument();
		}

	}

}
