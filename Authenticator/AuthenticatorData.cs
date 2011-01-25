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
using System.Net;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;

#if NETCF
using OpenNETCF.Security.Cryptography;
#endif

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class holding the authentication data that can be loaded/saved to secure storage. This contains the Authenticator's
	/// Secret Key and Serial number which are used to calculate the authentication code.
	/// </summary>
	public class AuthenticatorData : ICloneable
	{
		/// <summary>
		/// Number of bytes making up the salt
		/// </summary>
		private const int SALT_LENGTH = 8;

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
		/// Type of password to use to encrpyt secret data
		/// </summary>
		public enum PasswordTypes
		{
			None = 0,
			Explicit,
			User,
			Machine
		}

		/// <summary>
		/// Type of file format for loading key
		/// </summary>
		public enum FileFormat
		{
			WinAuth, // WinAuthg xml
			Android, // Android BMA
			Midp,
		}

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
		/// Type of password used to encrypt secretdata
		/// </summary>
		public PasswordTypes PasswordType { get; set; }

		/// <summary>
		/// Password used to encrypt secretdata (if PasswordType == Explict)
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Get/set the format of the last loaded key
		/// </summary>
		public FileFormat LoadedFormat { get; set; }

		/// <summary>
		/// Get/set the data saved for the secret data value
		/// </summary>
		protected string SecretData
		{
			get
			{
				// keep compatabillity with Android and xor with the private key
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
				// keeping comatabiity with Androind and xor with a private key
				byte[] bytes = Authenticator.StringToByteArray(value);
				for (int i = bytes.Length - 1; i >= 0; i--)
				{
					bytes[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
				}
				string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40);
			}
		}

		/// <summary>
		/// Create a blank AuthenticatorData object
		/// </summary>
		public AuthenticatorData()
		{
			LoadedFormat = FileFormat.WinAuth;
		}

		/// <summary>
		/// Load an authenticator from an XmlStream - depreciated: use (Stream, FileFormat, ...)
		/// </summary>
		/// <param name="stream"></param>
		/// <param name="format"></param>
		/// <param name="password"></param>
		[Obsolete("Using XmlReader is deprecated - use Stream instead")]
		public AuthenticatorData(XmlReader xr, string password)
		{
			LoadXmlSettings(xr, password);
		}

		/// <summary>
		/// Create or import a new AuthenticatorData object loaded from saved data
		/// </summary>
		/// <param name="stream">file stream</param>
		/// <param name="format">file format</param>
		/// <param name="password">optional encryption password</param>
		public AuthenticatorData(Stream stream, FileFormat format, string password)
		{
			// is it the java midlet file?
			if (format == FileFormat.Midp)
			{
				// read the whole recordstore into an array
				byte[] data = new byte[256];
				int datapos = 0;
				byte[] buffer = new byte[data.Length];
				int read;
				while ((read = stream.Read(buffer, 0, buffer.Length)) != 0)
				{
					Array.Copy(buffer, 0, data, datapos, read);
					datapos += read;
				}

				// since file formats are vendor specific we just use regex because we know 
				// we are looking for ASCII encoded strings of certain lengths
				string encoded = Encoding.ASCII.GetString(data, 0, data.Length);
				Match match = Regex.Match(encoded, @".*((?:EU|US)-\d{4}-\d{4}-\d{4}).*([A-Fa-f0-9]{40}).*", RegexOptions.Singleline);
				if (match.Success == true)
				{
					// extract the secret key and serial				
					SecretKey = Authenticator.StringToByteArray(match.Groups[2].Value);
					Serial = match.Groups[1].Value;
					Region = Serial.Substring(0, 2);
					ServerTimeDiff = 0L; // set as zero to force Sync

					LoadedFormat = FileFormat.Midp;
				}

				return;
			}

			// handle the Android xml file
			if (format == FileFormat.Android)
			{
				using (XmlReader xr = XmlReader.Create(stream))
				{
					XmlDocument doc = new XmlDocument();
					doc.Load(xr);

					// is the Androind Mobile Authenticator file? <xml.../><map>...</map>
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
						string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length); // yes, two extra paramters, but needed for NETCF
						SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
						Serial = full.Substring(40);

						// get offset value
						long offset = 0;
						node = doc.DocumentElement.SelectSingleNode("/map/long[@name='" + BMA_OFFSET_NAME + "']");
						if (node != null && LongTryParse(node.Attributes["value"].InnerText, out offset) /* long.TryParse(node.Attributes["value"].InnerText, out offset) == true */)
						{
							ServerTimeDiff = offset;
						}

						LoadedFormat = FileFormat.Android;

						return;
					}
				}
			}

			// use our own format
			if (format == FileFormat.WinAuth)
			{
				using (XmlReader xr = XmlReader.Create(stream))
				{
					LoadXmlSettings(xr, password);
					return;
				}
			}

			// unexpected data
			throw new InvalidConfigDataException();
		}

		/// <summary>
		/// Create a new AuthenticatorData object from a root xml node
		/// </summary>
		/// <param name="rootnode">Root node holding authenticator data</param>
		/// <param name="password"></param>
		public AuthenticatorData(XmlNode rootnode, string password)
		{
			LoadXmlSettings(rootnode, password);
		}

		/// <summary>
		/// Handle reading an XML config file with optional password
		/// </summary>
		/// <param name="xr">XmlReader to read</param>
		/// <param name="password">optional encryption password</param>
		private void LoadXmlSettings(XmlReader xr, string password)
		{
			XmlDocument doc = new XmlDocument();
			doc.Load(xr);
			LoadXmlSettings(doc.DocumentElement, password);
		}

		/// <summary>
		/// Load xml authenticator data from a XmlNode
		/// </summary>
		/// <param name="rootnode">top XmlNode to load</param>
		/// <param name="password">password for decryption</param>
		private void LoadXmlSettings(XmlNode rootnode, string password)
		{
			// is the Mobile Authenticator file? <xml.../><map>...</map>
			XmlNode node = rootnode.SelectSingleNode("/map/string[@name='" + BMA_HASH_NAME + "']");
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
				string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40);

				// get offset value
				long offset = 0;
				node = rootnode.SelectSingleNode("/map/long[@name='" + BMA_OFFSET_NAME + "']");
				if (node != null && LongTryParse(node.Attributes["value"].InnerText, out offset) /* long.TryParse(node.Attributes["value"].InnerText, out offset) == true */)
				{
					ServerTimeDiff = offset;
				}

				LoadedFormat = FileFormat.Android;

				return;
			}

			// read version 0.3 config
			node = rootnode.SelectSingleNode("/AuthenticatorData[@version='0.3']/Data");
			if (node != null)
			{
				string data = node.InnerText;

				byte[] bytes = Authenticator.StringToByteArray(data);
				string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
				SecretKey = Authenticator.StringToByteArray(full.Substring(0, 40));
				Serial = full.Substring(40, 17);
				Region = full.Substring(57, 2);
				byte[] serverTimeDiff = Authenticator.StringToByteArray(full.Substring(59, 16));
				if (BitConverter.IsLittleEndian)
				{
					Array.Reverse(serverTimeDiff);
				}
				ServerTimeDiff = BitConverter.ToInt64(serverTimeDiff, 0);

				LoadedFormat = FileFormat.WinAuth;

				return;
			}

			// read version 0.4 config
			node = rootnode.SelectSingleNode("/map[@version='0.4']/string[@name='data']");
			if (node != null)
			{
				SecretData = node.InnerText;
				//
				long offset = 0;
				node = rootnode.SelectSingleNode("long[@name='servertimediff']");
				if (node != null && LongTryParse(node.InnerText, out offset) == true /* long.TryParse(node.InnerText, out offset) == true */)
				{
					ServerTimeDiff = offset;
				}
				//
				node = rootnode.SelectSingleNode("string[@name='region']");
				if (node != null)
				{
					Region = node.InnerText;
				}
				//
				LoadedFormat = FileFormat.WinAuth;
				//
				return;
			}

			// read our curent config string
			node = rootnode.SelectSingleNode("secretdata");
			if (node != null)
			{
				string data = node.InnerText;
				XmlAttribute attr = node.Attributes["encrypted"];
				if (attr != null && attr.InnerText.Length != 0)
				{
					string encryptedType = attr.InnerText;
					if (encryptedType == "u")
					{
						// we are going to decrypt with the Windows User account key
						PasswordType = PasswordTypes.User;
						byte[] cipher = Authenticator.StringToByteArray(data);
						byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
						data = Authenticator.ByteArrayToString(plain);
					}
					else if (encryptedType == "m")
					{
						// we are going to decrypt with the Windows local machine key
						PasswordType = PasswordTypes.Machine;
						byte[] cipher = Authenticator.StringToByteArray(data);
						byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
						data = Authenticator.ByteArrayToString(plain);
					}
					else if (encryptedType == "y")
					{
						// we use an explicit password to encrypt data
						if (string.IsNullOrEmpty(password) == true)
						{
							throw new EncrpytedSecretDataException();
						}
						PasswordType = PasswordTypes.Explicit;
						Password = password;
						data = Decrypt(data, password);
					}
				}
				SecretData = data;

				long offset = 0;
				node = rootnode.SelectSingleNode("servertimediff");
				if (node != null && LongTryParse(node.InnerText, out offset) == true /* long.TryParse(node.InnerText, out offset) == true */)
				{
					ServerTimeDiff = offset;
				}

				node = rootnode.SelectSingleNode("region");
				if (node != null)
				{
					Region = node.InnerText;
				}

				LoadedFormat = FileFormat.WinAuth;

				return;
			}

			// unexpected data
			throw new InvalidConfigDataException();
		}

		/// <summary>
		/// Write the data object to an XmlWriter 
		/// </summary>
		/// <param name="writer">XmlWriter to save data</param>
		public void WriteXmlString(XmlWriter writer)
		{
			//writer.WriteStartDocument(true);
			writer.WriteStartElement("authenticator");
			//writer.WriteAttributeString("version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
			//
			writer.WriteStartElement("servertimediff");
			writer.WriteString(ServerTimeDiff.ToString());
			writer.WriteEndElement();
			//
			writer.WriteStartElement("region");
			writer.WriteString(Region);
			writer.WriteEndElement();
			//
			writer.WriteStartElement("secretdata");
			string data = SecretData;
			if (PasswordType == AuthenticatorData.PasswordTypes.Explicit)
			{
				data = Encrypt(data, Password);
				writer.WriteAttributeString("encrypted", "y");
			}
			else if (PasswordType == AuthenticatorData.PasswordTypes.User)
			{
				// we encrpyt the data using the Windows User account key
				byte[] plain = Authenticator.StringToByteArray(data);
				byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
				data = Authenticator.ByteArrayToString(cipher);
				writer.WriteAttributeString("encrypted", "u");
			}
			else if (PasswordType == AuthenticatorData.PasswordTypes.Machine)
			{
				// we encrypt the data using the Local Machine account key
				byte[] plain = Authenticator.StringToByteArray(data);
				byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.LocalMachine);
				data = Authenticator.ByteArrayToString(cipher);
				writer.WriteAttributeString("encrypted", "m");
			}
			writer.WriteString(data);
			writer.WriteEndElement();
			//
			writer.WriteEndElement();
			//writer.WriteEndDocument();
		}

		/// <summary>
		/// Encrpyt the data stored in config file
		/// </summary>
		/// <param name="plain">blace data to encrypt - hex representation of byte array</param>
		/// <param name="key">key to use to encrpyt</param>
		/// <returns>hex coded encrpyted string</returns>
		public static string Encrypt(string plain, string key)
		{
			byte[] inBytes = Authenticator.StringToByteArray(plain);
			byte[] keyBytes = Encoding.Default.GetBytes(key);

			// build a new salt
			Random random = new Random((int)DateTime.Now.Ticks);
			byte[] saltedKey = new byte[SALT_LENGTH + keyBytes.Length];
			for (int i = 0; i < SALT_LENGTH; i++)
			{
				saltedKey[i] = (byte)random.Next(256);
			}
			Array.Copy(keyBytes, 0, saltedKey, SALT_LENGTH, keyBytes.Length);
			string salt = BitConverter.ToString(saltedKey, 0, SALT_LENGTH).Replace("-", string.Empty);
			// create a key from the salt and original key
			MD5 md5 = MD5.Create();
			saltedKey = md5.ComputeHash(saltedKey);

			// get our cihper
			BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
			cipher.Init(true, new KeyParameter(saltedKey));

			// encrypt data
			int osize = cipher.GetOutputSize(inBytes.Length);
			byte[] outBytes = new byte[osize];
			int olen = cipher.ProcessBytes(inBytes, 0, inBytes.Length, outBytes, 0);
			olen += cipher.DoFinal(outBytes, olen);
			if (olen < osize)
			{
				byte[] t = new byte[olen];
				Array.Copy(outBytes, 0, t, 0, olen);
				outBytes = t;
			}

			// return encoded byte->hex string
			return salt + Authenticator.ByteArrayToString(outBytes);
		}

		/// <summary>
		/// Decrypt a config string from one hex-coded stirng into another
		/// </summary>
		/// <param name="data">data string to be decrypted</param>
		/// <param name="key">decryption key</param>
		/// <returns>hex coded decrypted string</returns>
		public static string Decrypt(string data, string key)
		{
			// extract the salt from the data
			byte[] salt = Authenticator.StringToByteArray(data.Substring(0, SALT_LENGTH*2));
			byte[] keyBytes = Encoding.Default.GetBytes(key);
			byte[] saltedKey = new byte[salt.Length + keyBytes.Length];
			Array.Copy(salt, saltedKey, salt.Length);
			Array.Copy(keyBytes, 0, saltedKey, salt.Length, keyBytes.Length);
			// build out combined key
			MD5 md5 = MD5.Create();
			saltedKey = md5.ComputeHash(saltedKey);

			// extract the actual data to be decrypted
			byte[] inBytes = Authenticator.StringToByteArray(data.Substring(SALT_LENGTH*2));

			// get cipher
			BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
			cipher.Init(false, new KeyParameter(saltedKey));

			// decrypt the data
			int osize = cipher.GetOutputSize(inBytes.Length);
			byte[] outBytes = new byte[osize];
			try
			{
				int olen = cipher.ProcessBytes(inBytes, 0, inBytes.Length, outBytes, 0);
				olen += cipher.DoFinal(outBytes, olen);
				if (olen < osize)
				{
					byte[] t = new byte[olen];
					Array.Copy(outBytes, 0, t, 0, olen);
					outBytes = t;
				}
			}
			catch (Exception)
			{
				// an exception is due to bad password
				throw new BadPasswordException();
			}

			// return encoded string
			return Authenticator.ByteArrayToString(outBytes);
		}

		#region ICloneable

		/// <summary>
		/// Clone the current object
		/// </summary>
		/// <returns>return clone</returns>
		public object Clone()
		{
			// we only need to do shallow copy
			return this.MemberwiseClone();
		}

		#endregion

		/// <summary>
		/// Wrapped TryParse for compatability with NETCF35 to simulate long.TryParse
		/// </summary>
		/// <param name="s">string of value to parse</param>
		/// <param name="val">out long value</param>
		/// <returns>true if value was parsed</returns>
		private static bool LongTryParse(string s, out long val)
		{
#if NETCF
			try
			{
				val = long.Parse(s);
				return true;
			}
			catch (Exception )
			{
				val = 0;
				return false;
			}
#else
			return long.TryParse(s, out val);
#endif
		}
	}

}
