/*
 * Copyright (C) 2011 Colin Mackie.
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

#if NUNIT
using NUnit.Framework;
#endif

#if NETCF
using OpenNETCF.Security.Cryptography;
#endif

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class that implements base RFC 4226 an RFC 6238 authenticator
	/// </summary>
	public abstract class Authenticator : ICloneable
	{
		/// <summary>
		/// Number of bytes making up the salt
		/// </summary>
		private const int SALT_LENGTH = 8;

		/// <summary>
		/// Number of iterations in PBKDF2 key generation
		/// </summary>
		private const int PBKDF2_ITERATIONS = 2000;

		/// <summary>
		/// Size of derived PBKDF2 key
		/// </summary>
		private const int PBKDF2_KEYSIZE = 256;

		/// <summary>
		/// Default config version for loading an unknown file
		/// </summary>
		public const decimal DEAFULT_CONFIG_VERSION = (decimal)1.6;

		/// <summary>
		/// Name attribute of the "string" hash element in the Battle.net Mobile Authenticator file
		/// </summary>
		private const string BMA_HASH_NAME = "com.blizzard.bma.AUTH_STORE.HASH";

		/// <summary>
		/// Name attribute of the "long" offset element in the Battle.net Mobile Authenticator file
		/// </summary>
		private const string BMA_OFFSET_NAME = "com.blizzard.bma.AUTH_STORE.CLOCK_OFFSET";

		/// <summary>
		/// Default number of digits in code
		/// </summary>
		private const int DEFAULT_CODE_DIGITS = 6;

		/// <summary>
		/// Private encrpytion key used to encrypt Battle.net mobile authenticator data.
		/// </summary>
		private static byte[] MOBILE_AUTHENTICATOR_KEY = new byte[]
			{
				0x39,0x8e,0x27,0xfc,0x50,0x27,0x6a,0x65,0x60,0x65,0xb0,0xe5,0x25,0xf4,0xc0,0x6c,
				0x04,0xc6,0x10,0x75,0x28,0x6b,0x8e,0x7a,0xed,0xa5,0x9d,0xa9,0x81,0x3b,0x5d,0xd6,
				0xc8,0x0d,0x2f,0xb3,0x80,0x68,0x77,0x3f,0xa5,0x9b,0xa4,0x7c,0x17,0xca,0x6c,0x64,
				0x79,0x01,0x5c,0x1d,0x5b,0x8b,0x8f,0x6b,0x9a
			};

		/// <summary>
		/// Type of password to use to encrypt secret data
		/// </summary>
		public enum PasswordTypes
		{
			None = 0,
			Explicit = 1,
			User = 2,
			Machine = 4
		}

		#region Authenticator data

		/// <summary>
		/// Serial number of authenticator
		/// </summary>
		public virtual string Serial { get; set; }

		/// <summary>
		/// Secret key used for Authenticator
		/// </summary>
		public byte[] SecretKey { get; set; }

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
		/// Number of digits returned in code (default is 6)
		/// </summary>
		public int CodeDigits { get; set; }

		/// <summary>
		/// We can check if the restore code is valid and rememeber so don't have to do it again
		/// </summary>
		public bool RestoreCodeVerified { get; set; }

		/// <summary>
		/// Get/set the combined secret data value
		/// </summary>
		public virtual string SecretData
		{
			get
			{
				return Authenticator.ByteArrayToString(SecretKey);
			}
			set
			{
				if (string.IsNullOrEmpty(value) == false)
				{
					SecretKey = Authenticator.StringToByteArray(value);
				}
				else
				{
					SecretKey = null;
				}
			}
		}

		/// <summary>
		/// Get the server time since 1/1/70
		/// </summary>
		public long ServerTime
		{
			get
			{
				return CurrentTime + ServerTimeDiff;
			}
		}

		/// <summary>
		/// Calculate the code interval based on the calculated server time
		/// </summary>
		public long CodeInterval
		{
			get
			{
				// calculate the code interval; the server's time div 30,000
				return (CurrentTime + ServerTimeDiff) / 30000L;
			}
		}

		/// <summary>
		/// Get the current code for the authenticator.
		/// </summary>
		/// <returns>authenticator code</returns>
		public string CurrentCode
		{
			get
			{
				return CalculateCode(false);
			}
		}

		/// <summary>
		/// Get the restore code for an authenticator used to recover a lost authenticator along with the serial number.
		/// </summary>
		/// <returns>restore code (10 chars)</returns>
		public virtual string RestoreCode
		{
			get
			{
				return BuildRestoreCode();
			}
		}

		#endregion

		/// <summary>
		/// Static initializer
		/// </summary>
		static Authenticator()
		{
			// Issue#71: remove the default .net expect header, which can cause issues (http://stackoverflow.com/questions/566437/)
			System.Net.ServicePointManager.Expect100Continue = false;
		}

		/// <summary>
		/// Create a new Authenticator object
		/// </summary>
		public Authenticator()
		{
			CodeDigits = DEFAULT_CODE_DIGITS;
		}

		/// <summary>
		/// Create a new Authenticator object
		/// </summary>
		public Authenticator(int codeDigits)
		{
			CodeDigits = codeDigits;
		}

		/// <summary>
		/// Calculate the current code for the authenticator.
		/// </summary>
		/// <param name="resyncTime">flag to resync time</param>
		/// <returns>authenticator code</returns>
		protected virtual string CalculateCode(bool resyncTime)
		{
			// sync time if required
			if (resyncTime == true || ServerTimeDiff == 0)
			{
				Sync();
			}

			HMac hmac = new HMac(new Sha1Digest());
			hmac.Init(new KeyParameter(SecretKey));

			byte[] codeIntervalArray = BitConverter.GetBytes(CodeInterval);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(codeIntervalArray);
			}
			hmac.BlockUpdate(codeIntervalArray, 0, codeIntervalArray.Length);

			byte[] mac = new byte[hmac.GetMacSize()];
			hmac.DoFinal(mac, 0);

			// the last 4 bits of the mac say where the code starts (e.g. if last 4 bit are 1100, we start at byte 12)
			int start = mac[19] & 0x0f;

			// extract those 4 bytes
			byte[] bytes = new byte[4];
			Array.Copy(mac, start, bytes, 0, 4);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(bytes);
			}
			uint fullcode = BitConverter.ToUInt32(bytes, 0) & 0x7fffffff;

			// we use the last 8 digits of this code in radix 10
			uint codemask = (uint)Math.Pow(10, CodeDigits);
			string format = new string('0', CodeDigits);
			string code = (fullcode % codemask).ToString(format);

			return code;
		}

		/// <summary>
		/// Synchorise this authenticator's time with server time. We update our data record with the difference from our UTC time.
		/// </summary>
		public abstract void Sync();

		/// <summary>
		/// Restore an authenticator from the serial and code (only used in Battle.net)
		/// </summary>
		/// <param name="serial">serial code, e.g. US-1234-5678-1234</param>
		/// <param name="restoreCode">restore code given on enroll, 10 chars.</param>
		public virtual void Restore(string serial, string restoreCode)
		{
			throw new NotImplementedException();
		}

		/// <summary>
		/// Calculate the Battle.net restore code for an authenticator.
		/// </summary>
		/// <returns>restore code for authenticator</returns>
		protected virtual string BuildRestoreCode()
    {
			throw new NotImplementedException();
    }

		#region Load / Save

		/// <summary>
		/// Load an authenticator from a Stream with an explicit password
		/// </summary>
		/// <param name="stream">Stream to read</param>
		/// <param name="password">explicit password if requried</param>
		/// <returns>loaded Authenticator</returns>
		public static Authenticator ReadFromStream(Stream stream, string password)
		{
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			return ReadFromStream(stream, password, decimal.Parse(version.ToString(2)));
		}

		/// <summary>
		/// Load an authenticator from a Stream with an explicit password for this current version
		/// </summary>
		/// <param name="stream">Stream to read</param>
		/// <param name="password">explicit password if requried</param>
		/// <param name="version">expected version of authenticator</param>
		/// <returns>loaded Authenticator</returns>
		public static Authenticator ReadFromStream(Stream stream, string password, decimal version)
		{
			using (XmlReader xr = XmlReader.Create(stream))
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(xr);
				XmlNode rootnode = doc.DocumentElement;
				XmlNode node;

				Authenticator authenticator = null;

				// get the type is we have it, else use the default
				XmlAttribute authenticatorType = rootnode.Attributes["type"];
				if (authenticatorType != null)
				{
					Type type = System.Reflection.Assembly.GetExecutingAssembly().GetType(authenticatorType.Value, false, true);
					authenticator = Activator.CreateInstance(type) as Authenticator;
				}
				if (authenticator == null)
				{
					authenticator = new BattleNetAuthenticator();
				}

				// is the Mobile Authenticator file? <xml.../><map>...</map>
				node = rootnode.SelectSingleNode("/map/string[@name='" + BMA_HASH_NAME + "']");
				if (node != null)
				{
					string data = node.InnerText;

					// extract the secret key and serial
					byte[] bytes = StringToByteArray(data);
					// decrpyt with the fixed key
					for (int i = bytes.Length - 1; i >= 0; i--)
					{
						bytes[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
					}
					// decode and set members
					string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length);
					authenticator.SecretData = full;

					// get offset value
					long offset = 0;
					node = rootnode.SelectSingleNode("/map/long[@name='" + BMA_OFFSET_NAME + "']");
					if (node != null && LongTryParse(node.Attributes["value"].InnerText, out offset) /* long.TryParse(node.Attributes["value"].InnerText, out offset) == true */)
					{
						authenticator.ServerTimeDiff = offset;
					}

					return authenticator;
				}

				// read <= 1.6 config
				if (version <= (decimal)1.6 && (node = rootnode.SelectSingleNode("secretdata")) != null)
				{
					// save off the processed decryptions so we can send bug report
					List<string> datas = new List<string>();
					string data = node.InnerText;
					datas.Add(data);

					XmlAttribute attr = node.Attributes["encrypted"];
					if (attr != null && attr.InnerText.Length != 0)
					{
						string encryptedType = attr.InnerText;
						if (encryptedType == "u")
						{
							// we are going to decrypt with the Windows User account key
							authenticator.PasswordType = PasswordTypes.User;
							byte[] cipher = StringToByteArray(data);
							byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
							data = ByteArrayToString(plain);
							datas.Add(data);
						}
						else if (encryptedType == "m")
						{
							// we are going to decrypt with the Windows local machine key
							authenticator.PasswordType = PasswordTypes.Machine;
							byte[] cipher = StringToByteArray(data);
							byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
							data = ByteArrayToString(plain);
							datas.Add(data);
						}
						else if (encryptedType == "y")
						{
							// we use an explicit password to encrypt data
							if (string.IsNullOrEmpty(password) == true)
							{
								throw new EncrpytedSecretDataException();
							}
							authenticator.PasswordType = PasswordTypes.Explicit;
							authenticator.Password = password;
							data = Decrypt(data, password, false);
							datas.Add(data);
						}
					}
					try
					{
						authenticator.SecretData = ConvertAndriodSecretData(data);
					}
					catch (Exception ex)
					{
						// we get a decode error if the data decrypted but isn't valid
						throw new InvalidSecretDataException(ex, password, (attr != null ? attr.InnerText : null), datas);
					}

					long offset = 0;
					node = rootnode.SelectSingleNode("servertimediff");
					if (node != null && LongTryParse(node.InnerText, out offset) == true /* long.TryParse(node.InnerText, out offset) == true */)
					{
						authenticator.ServerTimeDiff = offset;
					}

					node = rootnode.SelectSingleNode("restorecodeverified");
					if (node != null && string.Compare(node.InnerText, bool.TrueString.ToLower(), true) == 0)
					{
						authenticator.RestoreCodeVerified = true;
					}

					return authenticator;
				}

				// read current config
				if ((node = rootnode.SelectSingleNode("secretdata")) != null)
				{
					// save off the processed decryptions so we can send bug report
					string data = node.InnerText;
					XmlAttribute attr = node.Attributes["encrypted"];
					List<string> datas = new List<string>();
					datas.Add(data);
					PasswordTypes passwordType = PasswordTypes.None;
					if (attr != null && attr.InnerText.Length != 0)
					{
						char[] encTypes = attr.InnerText.ToCharArray();
						// we read the string in reverse order (the order they were encrypted)
						for (int i = encTypes.Length - 1; i >= 0; i--)
						{
							char encryptedType = encTypes[i];
							switch (encryptedType)
							{
								case 'u':
									{
										// we are going to decrypt with the Windows User account key
										try
										{
											passwordType |= PasswordTypes.User;
											byte[] cipher = StringToByteArray(data);
											byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.CurrentUser);
											data = ByteArrayToString(plain);
											datas.Add(data);
										}
										catch (System.Security.Cryptography.CryptographicException)
										{
											throw new InvalidUserDecryptionException();
										}
										break;
									}
								case 'm':
									{
										// we are going to decrypt with the Windows local machine key
										try
										{
											passwordType |= PasswordTypes.Machine;
											byte[] cipher = StringToByteArray(data);
											byte[] plain = ProtectedData.Unprotect(cipher, null, DataProtectionScope.LocalMachine);
											data = ByteArrayToString(plain);
											datas.Add(data);
										}
										catch (System.Security.Cryptography.CryptographicException)
										{
											throw new InvalidMachineDecryptionException();
										}
										break;
									}
								case 'y':
									{
										// we use an explicit password to encrypt data
										if (string.IsNullOrEmpty(password) == true)
										{
											throw new EncrpytedSecretDataException();
										}
										passwordType |= PasswordTypes.Explicit;
										authenticator.Password = password;
										data = Decrypt(data, password, true);
										datas.Add(data);
										break;
									}
								default:
									break;
							}
						}
						authenticator.PasswordType = passwordType;
					}
					try
					{
						// pre-version 2 we kept compatability with the Android file
						if (version < (decimal)2)
						{
							data = ConvertAndriodSecretData(data);
						}
						authenticator.SecretData = data;
					}
					catch (Exception ex)
					{
						throw new InvalidSecretDataException(ex, password, (attr != null ? attr.InnerText : null), datas);
					}

					long offset = 0;
					node = rootnode.SelectSingleNode("servertimediff");
					if (node != null && LongTryParse(node.InnerText, out offset) == true /* long.TryParse(node.InnerText, out offset) == true */)
					{
						authenticator.ServerTimeDiff = offset;
					}

					node = rootnode.SelectSingleNode("restorecodeverified");
					if (node != null && string.Compare(node.InnerText, bool.TrueString.ToLower(), true) == 0)
					{
						authenticator.RestoreCodeVerified = true;
					}

					return authenticator;
				}

				throw new InvalidOperationException();
			}
		}

		/// <summary>
		/// Write this authenticator into an XmlWriter
		/// </summary>
		/// <param name="writer">XmlWriter to receive authenticator</param>
		public void WriteToWriter(XmlWriter writer)
		{
			writer.WriteStartElement("authenticator");
			writer.WriteAttributeString("type", this.GetType().FullName);
			//
			writer.WriteStartElement("servertimediff");
			writer.WriteString(ServerTimeDiff.ToString());
			writer.WriteEndElement();
			//
			writer.WriteStartElement("secretdata");
			string data = SecretData;

			StringBuilder encryptionTypes = new StringBuilder();
			if ((PasswordType & PasswordTypes.Explicit) != 0)
			{
				string encrypted = Encrypt(data, Password);

				// test the encryption
				string decrypted = Decrypt(encrypted, Password, true);
				if (string.Compare(data, decrypted) != 0)
				{
					throw new InvalidEncryptionException(data, Password, encrypted, decrypted);
				}
				data = encrypted;

				encryptionTypes.Append("y");
			}
			if ((PasswordType & PasswordTypes.User) != 0)
			{
				// we encrypt the data using the Windows User account key
				byte[] plain = StringToByteArray(data);
				byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
				data = ByteArrayToString(cipher);
				encryptionTypes.Append("u");
			}
			if ((PasswordType & PasswordTypes.Machine) != 0)
			{
				// we encrypt the data using the Local Machine account key
				byte[] plain = StringToByteArray(data);
				byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.LocalMachine);
				data = ByteArrayToString(cipher);
				encryptionTypes.Append("m");
			}
			writer.WriteAttributeString("encrypted", encryptionTypes.ToString());
			writer.WriteString(data);
			writer.WriteEndElement();
			//
			if (RestoreCodeVerified == true)
			{
				writer.WriteStartElement("restorecodeverified");
				writer.WriteString(bool.TrueString.ToLower());
				writer.WriteEndElement();
			}
			//
			writer.WriteEndElement();
		}

		/// <summary>
		/// Handle conversion from our old format where we XORed the key as per BMA1.x
		/// </summary>
		/// <param name="key">SecretKey value</param>
		/// <param name="serial">Serial value</param>
		/// <returns>XORed string</returns>
		private static string ConvertAndriodSecretData(string secretData)
		{
			// we used to keep compatability with Android which was XORed with a secret key
			byte[] bytes = Authenticator.StringToByteArray(secretData);
			for (int i = bytes.Length - 1; i >= 0; i--)
			{
				bytes[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
			}
			string full = Encoding.UTF8.GetString(bytes, 0, bytes.Length); // this is hex key + ascii serial
			return full.Substring(0, 40) + Authenticator.ByteArrayToString(Encoding.UTF8.GetBytes(full.Substring(40)));
		}

		#endregion

		#region Utility functions

		/// <summary>
		/// Create a one-time pad by generating a random block and then taking a hash of that block as many times as needed.
		/// </summary>
		/// <param name="length">desired pad length</param>
		/// <returns>array of bytes conatining random data</returns>
		protected internal static byte[] CreateOneTimePad(int length)
		{
			// There is a MITM vulnerability from using the standard Random call
			// see https://docs.google.com/document/edit?id=1pf-YCgUnxR4duE8tr-xulE3rJ1Hw-Bm5aMk5tNOGU3E&hl=en
			// in http://code.google.com/p/winauth/issues/detail?id=2
			// so we switch out to use RNGCryptoServiceProvider instead of Random

			RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();

			byte[] randomblock = new byte[length];

			SHA1 sha1 = SHA1.Create();
			int i = 0;
			do
			{
				byte[] hashBlock = new byte[128];
				random.GetBytes(hashBlock);

				byte[] key = sha1.ComputeHash(hashBlock, 0, hashBlock.Length);
				if (key.Length >= randomblock.Length)
				{
					Array.Copy(key, 0, randomblock, i, randomblock.Length);
					break;
				}
				Array.Copy(key, 0, randomblock, i, key.Length);
				i += key.Length;
			} while (true);

			return randomblock;
		}

		/// <summary>
		/// Get the milliseconds since 1/1/70 (same as Java currentTimeMillis)
		/// </summary>
		public static long CurrentTime
		{
			get
			{
				return Convert.ToInt64((DateTime.UtcNow - new DateTime(1970, 1, 1)).TotalMilliseconds);
			}
		}

		/// <summary>
		/// Convert a hex string into a byte array. E.g. "001f406a" -> byte[] {0x00, 0x1f, 0x40, 0x6a}
		/// </summary>
		/// <param name="hex">hex string to convert</param>
		/// <returns>byte[] of hex string</returns>
		public static byte[] StringToByteArray(string hex)
		{
			int len = hex.Length;
			byte[] bytes = new byte[len / 2];
			for (int i = 0; i < len; i += 2)
			{
				bytes[i / 2] = Convert.ToByte(hex.Substring(i, 2), 16);
			}
			return bytes;
		}

		/// <summary>
		/// Convert a byte array into a ascii hex string, e.g. byte[]{0x00,0x1f,0x40,ox6a} -> "001f406a"
		/// </summary>
		/// <param name="bytes">byte array to convert</param>
		/// <returns>string version of byte array</returns>
		public static string ByteArrayToString(byte[] bytes)
		{
			// Use BitConverter, but it sticks dashes in the string
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}

		/// <summary>
		/// Encrypt a string with a given key
		/// </summary>
		/// <param name="plain">data to encrypt - hex representation of byte array</param>
		/// <param name="key">key to use to encrypt</param>
		/// <returns>hex coded encrypted string</returns>
		public static string Encrypt(string plain, string password)
		{
			byte[] inBytes = Authenticator.StringToByteArray(plain);
			byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

			// build a new salt
			RNGCryptoServiceProvider rg = new RNGCryptoServiceProvider();
			byte[] saltbytes = new byte[SALT_LENGTH];
			rg.GetBytes(saltbytes);
			string salt = Authenticator.ByteArrayToString(saltbytes);

			// build our PBKDF2 key
#if NETCF
			PBKDF2 kg = new PBKDF2(passwordBytes, saltbytes, PBKDF2_ITERATIONS);
#else
			Rfc2898DeriveBytes kg = new Rfc2898DeriveBytes(passwordBytes, saltbytes, PBKDF2_ITERATIONS);
#endif
			byte[] key = kg.GetBytes(PBKDF2_KEYSIZE);

			// get our cipher
			BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
			cipher.Init(true, new KeyParameter(key));

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
		/// Decrypt a hex-coded string using our MD5 or PBKDF2 generated key
		/// </summary>
		/// <param name="data">data string to be decrypted</param>
		/// <param name="key">decryption key</param>
		/// <param name="PBKDF2">flag to indicate we are using PBKDF2 to generate derived key</param>
		/// <returns>hex coded decrypted string</returns>
		public static string Decrypt(string data, string password, bool PBKDF2)
		{
			byte[] key;
			byte[] saltBytes = Authenticator.StringToByteArray(data.Substring(0, SALT_LENGTH * 2));

			if (PBKDF2 == true)
			{
				// extract the salt from the data
				byte[] passwordBytes = Encoding.UTF8.GetBytes(password);

				// build our PBKDF2 key
#if NETCF
			PBKDF2 kg = new PBKDF2(passwordBytes, saltbytes, 2000);
#else
				Rfc2898DeriveBytes kg = new Rfc2898DeriveBytes(passwordBytes, saltBytes, PBKDF2_ITERATIONS);
#endif
				key = kg.GetBytes(PBKDF2_KEYSIZE);
			}
			else
			{
				// extract the salt from the data
				byte[] passwordBytes = Encoding.Default.GetBytes(password);
				key = new byte[saltBytes.Length + passwordBytes.Length];
				Array.Copy(saltBytes, key, saltBytes.Length);
				Array.Copy(passwordBytes, 0, key, saltBytes.Length, passwordBytes.Length);
				// build out combined key
				MD5 md5 = MD5.Create();
				key = md5.ComputeHash(key);
			}

			// extract the actual data to be decrypted
			byte[] inBytes = Authenticator.StringToByteArray(data.Substring(SALT_LENGTH * 2));

			// get cipher
			BufferedBlockCipher cipher = new PaddedBufferedBlockCipher(new BlowfishEngine(), new ISO10126d2Padding());
			cipher.Init(false, new KeyParameter(key));

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

		/// <summary>
		/// Wrapped TryParse for compatability with NETCF35 to simulate long.TryParse
		/// </summary>
		/// <param name="s">string of value to parse</param>
		/// <param name="val">out long value</param>
		/// <returns>true if value was parsed</returns>
		protected internal static bool LongTryParse(string s, out long val)
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

		#endregion

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

#if NETCF
		/// <summary>
		/// Private class that implements PBKDF2 needed for older NETCF. Implemented from http://en.wikipedia.org/wiki/PBKDF2.
		/// </summary>
		private class PBKDF2
		{
			/// <summary>
			/// Our digest
			/// </summary>
			private HMac m_mac;

			/// <summary>
			/// Digest length
			/// </summary>
			private int m_hlen;

			/// <summary>
			/// Base password
			/// </summary>
			private byte[] m_password;

			/// <summary>
			/// Salt
			/// </summary>
			private byte[] m_salt;

			/// <summary>
			/// Number of iterations
			/// </summary>
			private int m_iterations;

			/// <summary>
			/// Create a new PBKDF2 object
			/// </summary>
			public PBKDF2(byte[] password, byte[] salt, int iterations)
			{
				m_password = password;
				m_salt = salt;
				m_iterations = iterations;

				m_mac = new HMac(new Sha1Digest());
				m_hlen = m_mac.GetMacSize();
			}

			/// <summary>
			/// Calculate F.
			/// F(P,S,c,i) = U1 ^ U2 ^ ... ^ Uc
			/// Where F is an xor of c iterations of chained PRF. First iteration of PRF uses master password P as PRF key and salt concatenated to i. Second and greater PRF uses P and output of previous PRF computation:
			/// </summary>
			/// <param name="P"></param>
			/// <param name="S"></param>
			/// <param name="c"></param>
			/// <param name="i"></param>
			/// <param name="DK"></param>
			/// <param name="DKoffset"></param>
			private void F(byte[] P, byte[] S, int c, byte[] i, byte[] DK, int DKoffset)
			{
				// first iteration (ses master password P as PRF key and salt concatenated to i)
				byte[] buf = new byte[m_hlen];
				ICipherParameters param = new KeyParameter(P);
				m_mac.Init(param);
				m_mac.BlockUpdate(S, 0, S.Length);
				m_mac.BlockUpdate(i, 0, i.Length);
				m_mac.DoFinal(buf, 0);
				Array.Copy(buf, 0, DK, DKoffset, buf.Length);

				// remaining iterations (uses P and output of previous PRF computation)
				for (int iter = 1; iter < c; iter++)
				{
					m_mac.Init(param);
					m_mac.BlockUpdate(buf, 0, buf.Length);
					m_mac.DoFinal(buf, 0);

					for (int j=buf.Length-1; j >= 0; j--)
					{
						DK[DKoffset + j] ^= buf[j];
					}
				}
			}

			/// <summary>
			/// Calculate a derived key of dkLen bytes long from our initial password and salt
			/// </summary>
			/// <param name="dkLen">Length of desired key to be returned</param>
			/// <returns>derived key of dkLen bytes</returns>
			public byte[] GetBytes(int dkLen)
			{
				// For each hLen-bit block Ti of derived key DK, computing is as follows:
				//  DK = T1 || T2 || ... || Tdklen/hlen
				//  Ti = F(P,S,c,i)
				int chunks = (dkLen + m_hlen - 1) / m_hlen;
				byte[] DK = new byte[chunks * m_hlen];
				byte[] idata = new byte[4];
				for (int i = 1; i <= chunks; i++)
				{
					idata[0] = (byte)((uint)i >> 24);
					idata[1] = (byte)((uint)i >> 16);
					idata[2] = (byte)((uint)i >> 8);
					idata[3] = (byte)i; 

					F(m_password, m_salt, m_iterations, idata, DK, (i-1) * m_hlen);
				}
				if (DK.Length > dkLen)
				{
					byte[] reduced = new byte[dkLen];
					Array.Copy(DK, 0, reduced, 0, dkLen);
					DK = reduced;
				}

				return DK;
			}
		}

#if NUNIT
		/// Test against standard test vectors and one of our own of the correct iterations.
		/// http://tools.ietf.org/html/draft-josefsson-pbkdf2-test-vectors-00
		[Test]
		public static void TestPBKDF2()
		{
			PBKDF2 kg;
			byte[] DK;

			byte[] tv1 = new byte[] { 0x0c, 0x60, 0xc8, 0x0f, 0x96, 0x1f, 0x0e, 0x71, 0xf3, 0xa9, 0xb5, 0x24, 0xaf, 0x60, 0x12, 0x06, 0x2f, 0xe0, 0x37, 0xa6 };
			kg = new PBKDF2(Encoding.Default.GetBytes("password"), Encoding.Default.GetBytes("salt"), 1);
			DK = kg.GetBytes(20);
			Assert.AreEqual(DK, tv1);

			byte[] tv2 = new byte[] { 0xea, 0x6c, 0x01, 0x4d, 0xc7, 0x2d, 0x6f, 0x8c, 0xcd, 0x1e, 0xd9, 0x2a, 0xce, 0x1d, 0x41, 0xf0, 0xd8, 0xde, 0x89, 0x57 };
			kg = new PBKDF2(Encoding.Default.GetBytes("password"), Encoding.Default.GetBytes("salt"), 2);
			DK = kg.GetBytes(20);
			Assert.AreEqual(DK, tv2);

			byte[] tv3 = new byte[] { 0x4b, 0x00, 0x79, 0x01, 0xb7, 0x65, 0x48, 0x9a, 0xbe, 0xad, 0x49, 0xd9, 0x26, 0xf7, 0x21, 0xd0, 0x65, 0xa4, 0x29, 0xc1 };
			kg = new PBKDF2(Encoding.Default.GetBytes("password"), Encoding.Default.GetBytes("salt"), 4096);
			DK = kg.GetBytes(20);
			Assert.AreEqual(DK, tv3);

			byte[] tv4 = new byte[] { 0x2f, 0x25, 0x5b, 0x3a, 0x95, 0x46, 0x3c, 0x76, 0x62, 0x1f, 0x06, 0x80, 0xa2, 0xb3, 0x35, 0xad, 0x90, 0x3b, 0x85, 0xde };
			kg = new PBKDF2(Encoding.Default.GetBytes("VXFr[24c=6(D8He"), Encoding.Default.GetBytes("salt"), 1000);
			DK = kg.GetBytes(20);
			Assert.AreEqual(DK, tv4);
		}
#endif

#endif

	}
}
