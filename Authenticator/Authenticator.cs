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

using NUnit.Framework;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class that implements Battle.net Mobile Authenticator v1.1.0.
	/// </summary>
	public class Authenticator : ICloneable
	{
		/// <summary>
		/// Size of model string
		/// </summary>
		private const int MODEL_SIZE = 16;

		/// <summary>
		/// String of possible chars we use in our random model string
		/// </summary>
		private const string MODEL_CHARS = " ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz01234567890";

		/// <summary>
		/// Buffer size used on Http responses
		/// </summary>
		private const int RESPONSE_BUFFER_SIZE = 64;

		/// <summary>
		/// Expect size of return data from enroll
		/// </summary>
		private const int ENROLL_RESPONSE_SIZE = 45;

		/// <summary>
		/// Expected size of return data from time sync
		/// </summary>
		private const int SYNC_RESPONSE_SIZE = 8;

		/// <summary>
		/// Buffer size used on Restore call
		/// </summary>
		private const int RESTOREINIT_BUFFER_SIZE = 32;

		/// <summary>
		/// Buffer size used on Restore Validation call
		/// </summary>
		private const int RESTOREVALIDATE_BUFFER_SIZE = 20;

		/// <summary>
		/// The public key modulus used to encrypt our data
		/// </summary>
		private const string ENROLL_MODULUS =
			"955e4bd989f3917d2f15544a7e0504eb9d7bb66b6f8a2fe470e453c779200e5e" +
			"3ad2e43a02d06c4adbd8d328f1a426b83658e88bfd949b2af4eaf30054673a14" +
			"19a250fa4cc1278d12855b5b25818d162c6e6ee2ab4a350d401d78f6ddb99711" +
			"e72626b48bd8b5b0b7f3acf9ea3c9e0005fee59e19136cdb7c83f2ab8b0a2a99";

		/// <summary>
		/// Public key exponent used to encrypt our data
		/// </summary>
		private const string ENROLL_EXPONENT =
			"0101";

		/// <summary>
		/// Number of bytes making up the salt
		/// </summary>
		private const int SALT_LENGTH = 8;

		/// <summary>
		/// Name attribute of the "string" hash element in the Mobile Authenticator file
		/// </summary>
		private const string BMA_HASH_NAME = "com.blizzard.bma.AUTH_STORE.HASH";

		/// <summary>
		/// Name attribute of the "long" offset element in the Mobile Authenticator file
		/// </summary>
		private const string BMA_OFFSET_NAME = "com.blizzard.bma.AUTH_STORE.CLOCK_OFFSET";


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
			WinAuth, // WinAuth xml
			Android, // Android BMA
			Midp,
		}


		/// <summary>
		/// URLs for all mobile services
		/// </summary>
		private static string MOBILE_URL = "http://mobile-service.blizzard.com";
		private static string ENROLL_PATH = "/enrollment/enroll2.htm";
		private static string SYNC_PATH = "/enrollment/time.htm";
		private static string RESTORE_PATH = "/enrollment/initiatePaperRestore.htm";
		private static string RESTOREVALIDATE_PATH = "/enrollment/validatePaperRestore.htm";

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


		#region Authenticator data

		/// <summary>
		/// Secret key used for Authenticator
		/// </summary>
		public byte[] SecretKey { get; set; }

		/// <summary>
		/// Serial number of authenticator
		/// </summary>
		public string Serial { get; set; }

		/// <summary>
		/// Region for authenticator taken from first 2 chars of serial
		/// </summary>
		public string Region
		{
			get
			{
				return (string.IsNullOrEmpty(Serial) == false ? Serial.Substring(0, 2) : string.Empty);
			}
		}

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
		public string RestoreCode
		{
			get
			{
				return BuildRestoreCode();
			}
		}

		#endregion

		/// <summary>
		/// Create a new Authenticator object
		/// </summary>
		public Authenticator()
		{
			LoadedFormat = FileFormat.WinAuth;
		}

		/// <summary>
		/// Enroll this authenticator with the server.
		/// </summary>
		public void Enroll()
		{
			Enroll(null);
		}

		/// <summary>
		/// Enroll the authenticator with the server. We can pass an optional country code from http://en.wikipedia.org/wiki/ISO_3166-1_alpha-2
		/// but is seems the server uses GEOIP to determine the region anyway
		/// </summary>
		/// <param name="countryCode">optional 2 letter country code</param>
		public void Enroll(string countryCode)
		{
			// generate byte array of data:
			//  00 byte[20] one-time key used to decrypt data when returned;
			//  20 byte[2] region: actually a country code, e.g. US< UK, FR, KR, etc
			//  22 byte[16] model string for this device;
			//	38 END

			byte[] data = new byte[38];
			byte[] oneTimePad = CreateOneTimePad(20);
			Array.Copy(oneTimePad, data, oneTimePad.Length);
			// add country if we have one
			if (string.IsNullOrEmpty(countryCode) == false)
			{
				byte[] countrydata = Encoding.UTF8.GetBytes(countryCode);
				Array.Copy(countrydata, 0, data, 20, Math.Min(countrydata.Length, 2));
			}
			// add model name
			byte[] model = Encoding.UTF8.GetBytes(GeneralRandomModel());
			Array.Copy(model, 0, data, 22, Math.Min(model.Length, 16));

			// encrypt the data with BMA public key
			RsaEngine rsa = new RsaEngine();
			rsa.Init(true, new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(ENROLL_MODULUS, 16), new Org.BouncyCastle.Math.BigInteger(ENROLL_EXPONENT, 16)));
			byte[] encrypted = rsa.ProcessBlock(data, 0, data.Length);

			// call the enroll server
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MOBILE_URL + ENROLL_PATH);
			request.Method = "POST";
			request.ContentType = "application/octet-stream";
			request.ContentLength = encrypted.Length;
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(encrypted, 0, encrypted.Length);
			requestStream.Close();
			byte[] responseData = null;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				// OK?
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new InvalidEnrollResponseException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
				}

				// load back the buffer - should only be a byte[45]
				using (MemoryStream ms = new MemoryStream())
				{
					//using (BufferedStream bs = new BufferedStream(response.GetResponseStream()))
					using (Stream bs = response.GetResponseStream())
					{
						byte[] temp = new byte[RESPONSE_BUFFER_SIZE];
						int read;
						while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0)
						{
							ms.Write(temp, 0, read);
						}
						responseData = ms.ToArray();

						// check it is correct size
						if (responseData.Length != ENROLL_RESPONSE_SIZE)
						{
						  throw new InvalidEnrollResponseException(string.Format("Invalid response data size (expected 45 got {0})", responseData.Length));
						}
					}
				}
			}

			// return data:
			// 00-07 server time (Big Endian)
			// 08-24 is serial (17)
			// 25-44 secret key encrpyted with our pad
			// 45 END

			// extract the server time
			byte[] serverTime = new byte[8];
			Array.Copy(responseData, serverTime, 8);
			if (BitConverter.IsLittleEndian == true)
			{
				Array.Reverse(serverTime);
			}
			// get the difference between the server time and our current time
			ServerTimeDiff = BitConverter.ToInt64(serverTime, 0) - CurrentTime;

			// get the secret key
			byte[] secretKey = new byte[20];
			Array.Copy(responseData, 25, secretKey, 0, 20);
			// decrypt the initdata with a simple xor with our key
			for (int i = oneTimePad.Length-1; i >= 0; i--)
			{
				secretKey[i] ^= oneTimePad[i];
			}
			SecretKey = secretKey;

			// get the serial number
			Serial = Encoding.Default.GetString(responseData, 8, 17);
		}

		/// <summary>
		/// Synchorise this authenticator's time with server time. We update our data record with the difference from our UTC time.
		/// </summary>
		public void Sync()
		{
			// create a connection to time sync server
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MOBILE_URL + SYNC_PATH);
			request.Method = "GET";

			// get response
			byte[] responseData = null;
			using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
			{
				// OK?
				if (response.StatusCode != HttpStatusCode.OK)
				{
					throw new ApplicationException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
				}

				// load back the buffer - should only be a byte[8]
				using (MemoryStream ms = new MemoryStream())
				{
					// using (BufferedStream bs = new BufferedStream(response.GetResponseStream()))
					using (Stream bs = response.GetResponseStream())
					{
						byte[] temp = new byte[RESPONSE_BUFFER_SIZE];
						int read;
						while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0)
						{
							ms.Write(temp, 0, read);
						}
						responseData = ms.ToArray();

						// check it is correct size
						if (responseData.Length != SYNC_RESPONSE_SIZE)
						{
							throw new InvalidSyncResponseException(string.Format("Invalid response data size (expected " + SYNC_RESPONSE_SIZE + " got {0}", responseData.Length));
						}
					}
				}
			}

			// return data:
			// 00-07 server time (Big Endian)

			// extract the server time
			if (BitConverter.IsLittleEndian == true)
			{
				Array.Reverse(responseData);
			}
			// get the difference between the server time and our current time
			long serverTimeDiff = BitConverter.ToInt64(responseData, 0) - CurrentTime;

			// update the Data object
			ServerTimeDiff = serverTimeDiff;
		}

		/// <summary>
		/// Restore an authenticator from the serial number and restore code.
		/// </summary>
		/// <param name="serial">serial code, e.g. US-1234-5678-1234</param>
		/// <param name="restoreCode">restore code given on enroll, 10 chars.</param>
		public void Restore(string serial, string restoreCode)
		{
			// get the serial data
			byte[] serialBytes = Encoding.UTF8.GetBytes(serial.Replace("-", string.Empty));

			// sewnd the request to the server to get our challenge
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(MOBILE_URL + RESTORE_PATH);
			request.Method = "POST";
			request.ContentType = "application/octet-stream";
			request.ContentLength = serialBytes.Length;
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(serialBytes, 0, serialBytes.Length);
			requestStream.Close();
			byte[] challenge = null;
			try
			{
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					// OK?
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new InvalidRestoreResponseException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
					}

					// load back the buffer - should only be a byte[32]
					using (MemoryStream ms = new MemoryStream())
					{
						using (Stream bs = response.GetResponseStream())
						{
							byte[] temp = new byte[RESPONSE_BUFFER_SIZE];
							int read;
							while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0)
							{
								ms.Write(temp, 0, read);
							}
							challenge = ms.ToArray();

							// check it is correct size
							if (challenge.Length != RESTOREINIT_BUFFER_SIZE)
							{
								throw new InvalidRestoreResponseException(string.Format("Invalid response data size (expected 32 got {0})", challenge.Length));
							}
						}
					}
				}
			}
			catch (WebException we)
			{
				int code = (int)((HttpWebResponse)we.Response).StatusCode;
				if (code >= 500 && code < 600)
				{
					throw new InvalidRestoreResponseException(string.Format("No response from server ({0}). Perhaps maintainence?", code));
				}
				else
				{
					throw new InvalidRestoreResponseException(string.Format("Error communicating with server: {0} - {1}", code, ((HttpWebResponse)we.Response).StatusDescription));
				}
			}

			// only take the first 10 bytes of the restore code
			byte[] restoreCodeBytes = new byte[10];
			char[] arrayOfChar = restoreCode.ToUpper().ToCharArray();
			for (int i = 0; i < 10; i++)
			{
				restoreCodeBytes[i] = ConvertRestoreCodeCharToByte(arrayOfChar[i]);
			}

			// build the response to the challenge
			HMac hmac = new HMac(new Sha1Digest());
			hmac.Init(new KeyParameter(restoreCodeBytes));
			byte[] hashdata = new byte[serialBytes.Length + challenge.Length];
			Array.Copy(serialBytes, 0, hashdata, 0, serialBytes.Length);
			Array.Copy(challenge, 0, hashdata, serialBytes.Length, challenge.Length);
			hmac.BlockUpdate(hashdata, 0, hashdata.Length);
			byte[] hash = new byte[hmac.GetMacSize()];
			hmac.DoFinal(hash, 0);

			// create a random key
			byte[] oneTimePad = CreateOneTimePad(20);

			// concatanate the hash and key
			byte[] hashkey = new byte[hash.Length + oneTimePad.Length];
			Array.Copy(hash, 0, hashkey, 0, hash.Length);
			Array.Copy(oneTimePad, 0, hashkey, hash.Length, oneTimePad.Length);

			// encrypt the data with BMA public key
			RsaEngine rsa = new RsaEngine();
			rsa.Init(true, new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(ENROLL_MODULUS, 16), new Org.BouncyCastle.Math.BigInteger(ENROLL_EXPONENT, 16)));
			byte[] encrypted = rsa.ProcessBlock(hashkey, 0, hashkey.Length);

			// prepend the serial to the encrypted data
			byte[] postbytes = new byte[serialBytes.Length + encrypted.Length];
			Array.Copy(serialBytes, 0, postbytes, 0, serialBytes.Length);
			Array.Copy(encrypted, 0, postbytes, serialBytes.Length, encrypted.Length);

			// send the challenge response back to the server
			request = (HttpWebRequest)WebRequest.Create(MOBILE_URL + RESTOREVALIDATE_PATH);
			request.Method = "POST";
			request.ContentType = "application/octet-stream";
			request.ContentLength = postbytes.Length;
			requestStream = request.GetRequestStream();
			requestStream.Write(postbytes, 0, postbytes.Length);
			requestStream.Close();
			byte[] secretKey = null;
			try {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
					// OK?
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new InvalidRestoreResponseException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
					}

					// load back the buffer - should only be a byte[32]
					using (MemoryStream ms = new MemoryStream())
					{
						using (Stream bs = response.GetResponseStream())
						{
							byte[] temp = new byte[RESPONSE_BUFFER_SIZE];
							int read;
							while ((read = bs.Read(temp, 0, RESPONSE_BUFFER_SIZE)) != 0)
							{
								ms.Write(temp, 0, read);
							}
							secretKey = ms.ToArray();

							// check it is correct size
							if (secretKey.Length != RESTOREVALIDATE_BUFFER_SIZE)
							{
								throw new InvalidRestoreResponseException(string.Format("Invalid response data size (expected " + RESTOREVALIDATE_BUFFER_SIZE + " got {0})", secretKey.Length));
							}
						}
					}
				}
			}
			catch (WebException we)
			{
				int code = (int)((HttpWebResponse)we.Response).StatusCode;
				if (code >= 500 && code < 600)
				{
					throw new InvalidRestoreResponseException(string.Format("No response from server ({0}). Perhaps maintainence?", code));
				}
				else if (code >= 600 && code < 700)
				{
					throw new InvalidRestoreCodeException("Invalid serial number or restore code.");
				}
				else
				{
					throw new InvalidRestoreResponseException(string.Format("Error communicating with server: {0} - {1}", code, ((HttpWebResponse)we.Response).StatusDescription));
				}
			}

			// xor the returned data key with our pad to get the actual secret key
			for (int i = oneTimePad.Length - 1; i >= 0; i--)
			{
				secretKey[i] ^= oneTimePad[i];
			}

			// set the authenticator data
			SecretKey = secretKey;
			if (serial.Length == 14)
			{
				Serial = serial.Substring(0, 2).ToUpper() + "-" + serial.Substring(2, 4) + "-" + serial.Substring(6, 4) + "-" + serial.Substring(10, 4);
			}
			else
			{
				Serial = serial.ToUpper();
			}

			ServerTimeDiff = 0L;
			Sync();
		}


		#region Loading and Saving

		/// <summary>
		/// Load or import a new Authenticator from saved data
		/// </summary>
		/// <param name="stream">file stream</param>
		/// <param name="format">file format</param>
		/// <param name="password">optional encryption password</param>
		public void Load (Stream stream, FileFormat format, string password)
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
		/// Load a new Authenticator from a root xml node
		/// </summary>
		/// <param name="rootnode">Root node holding authenticator data</param>
		/// <param name="password"></param>
		public void Load (XmlNode rootnode, string password)
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
				//node = rootnode.SelectSingleNode("string[@name='region']");
				//Region = (node != null ? node.InnerText : Serial.Substring(0, 2));
				//
				LoadedFormat = FileFormat.WinAuth;

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
			writer.WriteStartElement("authenticator");
			//
			writer.WriteStartElement("servertimediff");
			writer.WriteString(ServerTimeDiff.ToString());
			writer.WriteEndElement();
			//
			writer.WriteStartElement("secretdata");
			string data = SecretData;
			if (PasswordType == PasswordTypes.Explicit)
			{
				data = Encrypt(data, Password);
				writer.WriteAttributeString("encrypted", "y");
			}
			else if (PasswordType == PasswordTypes.User)
			{
				// we encrpyt the data using the Windows User account key
				byte[] plain = Authenticator.StringToByteArray(data);
				byte[] cipher = ProtectedData.Protect(plain, null, DataProtectionScope.CurrentUser);
				data = Authenticator.ByteArrayToString(cipher);
				writer.WriteAttributeString("encrypted", "u");
			}
			else if (PasswordType == PasswordTypes.Machine)
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
		}

		#endregion

		/// <summary>
		/// Calculate the current code for the authenticator.
		/// </summary>
		/// <param name="resyncTime">flag to resync time</param>
		/// <returns>authenticator code</returns>
		private string CalculateCode(bool resyncTime)
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
			string code = (fullcode % 100000000).ToString("00000000");

			return code;
		}

		/// <summary>
		/// Calculate the restore code for an authenticator. This is taken from the last 10 bytes of a digest of the serial and secret key,
		/// which is then specially encoded to alphanumerics.
		/// </summary>
		/// <returns>restore code for authenticator (always 10 chars)</returns>
		private string BuildRestoreCode()
    {
			// return if not set
			if (string.IsNullOrEmpty(Serial) == true || SecretKey == null)
			{
				return string.Empty;
			}

			// get byte array of serial
			byte[] serialdata = Encoding.UTF8.GetBytes(Serial.ToUpper().Replace("-", string.Empty));
			byte[] secretdata = SecretKey;

			// combine serial data and secret data
			byte[] combined = new byte[serialdata.Length + secretdata.Length];
			Array.Copy(serialdata, 0, combined, 0, serialdata.Length);
			Array.Copy(secretdata, 0, combined, serialdata.Length, secretdata.Length);

			// create digest of combined data
			IDigest digest = new Sha1Digest();
			digest.BlockUpdate(combined, 0, combined.Length);
			byte[] digestdata = new byte[digest.GetDigestSize()];
			digest.DoFinal(digestdata, 0);

			// take last 10 chars of hash and convert each byte to our encoded string
			StringBuilder code = new StringBuilder();
			int startpos = digestdata.Length - 10;
			for (int i = 0; i < 10; i++)
			{
				code.Append(ConvertRestoreCodeByteToChar(digestdata[startpos + i]));
			}

			return code.ToString();
    }

/*
		/// <summary>
		/// Create an initial random block of 37 bytes for a secret key. Seed Random with the current time
		/// and perform SHA1 on random bytes taking first 37 bytes of resultant 40 byte block.
		/// </summary>
		/// <returns>random byte[37] array</returns>
		protected static byte[] CreateInitializationRandom()
		{
			byte[] hashBlock = new byte[128];

			// There is a MITM vulnerability from using the standard Random call
			// see https://docs.google.com/document/edit?id=1pf-YCgUnxR4duE8tr-xulE3rJ1Hw-Bm5aMk5tNOGU3E&hl=en
			// in http://code.google.com/p/winauth/issues/detail?id=2
			// so we switch out to use RNGCryptoServiceProvider

			//Random random = new Random((int)CurrentTime);			
			//for (int i = hashBlock.Length-1; i >= 0; i--)
			//{
			//  hashBlock[i] = (byte)random..Next(256);
			//}

			RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
			random.GetBytes(hashBlock);

			SHA1 sha1 = SHA1.Create();
			byte[] key1 = sha1.ComputeHash(hashBlock, 0, 64);
			byte[] key2 = sha1.ComputeHash(hashBlock, 64, 64);
			byte[] key = new byte[37];
			Array.Copy(key1, key, 20);
			Array.Copy(key2, 0, key, 20, 17);

			return key;
		}
*/

		/// <summary>
		/// Create a random Model string for initialization to armor the init string sent over the wire
		/// </summary>
		/// <returns>Random model string</returns>
		private static string GeneralRandomModel()
		{
			// seed a new RNG
			RNGCryptoServiceProvider randomSeedGenerator = new RNGCryptoServiceProvider();
			byte[] seedBuffer = new byte[4];
			randomSeedGenerator.GetBytes(seedBuffer);
			Random random = new Random(BitConverter.ToInt32(seedBuffer, 0));

			// create a model string with available characters
			StringBuilder model = new StringBuilder(MODEL_SIZE);
			for (int i = MODEL_SIZE; i > 0; i--)
			{
				model.Append(MODEL_CHARS[random.Next(MODEL_CHARS.Length)]);
			}

			return model.ToString();
		}

		#region Utility functions

		/// <summary>
		/// Create a one-time pad by generating a random block and then taking a hash of that block as many times as needed.
		/// </summary>
		/// <param name="length">desired pad length</param>
		/// <returns>array of bytes conatining random data</returns>
		protected internal static byte[] CreateOneTimePad(int length)
		{
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
		/// Convert a char to a byte but with appropriate mapping to exclude I,L,O and S. E.g. A=10 but J=18 not 19 (as I is missing)
		/// </summary>
		/// <param name="c">char to convert.</param>
		/// <returns>byte value of restore code char</returns>
		private static byte ConvertRestoreCodeCharToByte(char c)
		{
			if (c >= '0' && c <= '9')
			{
				return (byte)(c - '0');
			}
			else
			{
				byte index = (byte)(c + 10 - 65);
				if (c >= 'I')
				{
					index--;
				}
				if (c >= 'L')
				{
					index--;
				}
				if (c >= 'O')
				{
					index--;
				}
				if (c >= 'S')
				{
					index--;
				}

				return index;
			}
		}

		/// <summary>
		/// Convert a byte to a char but with appropriate mapping to exclude I,L,O and S.
		/// </summary>
		/// <param name="b">byte to convert.</param>
		/// <returns>char value of restore code value</returns>
		private static char ConvertRestoreCodeByteToChar(byte b)
		{
			int index = b & 0x1f;
			if (index <= 9)
			{
				return (char)(index + 48);
			}
			else
			{
				index = (index + 65) - 10;
				if (index >= 73)
				{
					index++;
				}
				if (index >= 76)
				{
					index++;
				}
				if (index >= 79)
				{
					index++;
				}
				if (index >= 83)
				{
					index++;
				}
				return (char)index;
			}
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
			byte[] salt = Authenticator.StringToByteArray(data.Substring(0, SALT_LENGTH * 2));
			byte[] keyBytes = Encoding.Default.GetBytes(key);
			byte[] saltedKey = new byte[salt.Length + keyBytes.Length];
			Array.Copy(salt, saltedKey, salt.Length);
			Array.Copy(keyBytes, 0, saltedKey, salt.Length, keyBytes.Length);
			// build out combined key
			MD5 md5 = MD5.Create();
			saltedKey = md5.ComputeHash(saltedKey);

			// extract the actual data to be decrypted
			byte[] inBytes = Authenticator.StringToByteArray(data.Substring(SALT_LENGTH * 2));

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

	}

	[TestFixture]
	class AuthenticatorTest
	{
		public class TestAuthenticator
		{
			public string Serial {get; set;}
			public byte[] SecretKey { get; set; }
			public string RestoreCode { get; set; }
			public TestAuthenticator (string serial, byte[] key, string restorecode)
			{
				Serial = serial;
				SecretKey = key;
				RestoreCode = restorecode;
			}
		}

		public TestAuthenticator[] authenticators = new TestAuthenticator[]
		{
			new TestAuthenticator("US-1006-0970-8791", new byte[] { 0x82, 0x0A, 0xB8, 0xF9, 0x66, 0x54, 0xF9, 0xBE, 0xAD, 0xB6, 0xD1, 0x27, 0x95, 0x81, 0x13, 0xD5, 0xC8, 0xAE, 0xBA, 0x36 }, "JDDU796WQ4"),
			new TestAuthenticator("US-1108-0168-1578", new byte[] { 0xFA, 0xA4, 0x62, 0xE9, 0x20, 0x60, 0xAA, 0xAF, 0xD1, 0x2D, 0x50, 0xB5, 0x65, 0xB8, 0xCD, 0x50, 0xA8, 0xDF, 0xC5, 0xB9 }, "703PQU2BVE"),
			new TestAuthenticator("US-1108-0268-3369", new byte[] { 0x6C, 0x35, 0x02, 0xA5, 0xD6, 0xA8, 0x84, 0xBB, 0xA6, 0x86, 0x73, 0xBF, 0x18, 0xB4, 0x7B, 0x90, 0xAD, 0x28, 0xB9, 0x11 }, "HZJRHZZ7UN"),
			new TestAuthenticator("US-1108-0268-3382", new byte[] { 0x73, 0x4D, 0x9A, 0x61, 0xE5, 0x17, 0x0C, 0x36, 0xEE, 0x17, 0x44, 0xE5, 0x9F, 0x5F, 0xE3, 0xE4, 0x71, 0xBE, 0x1A, 0xA1 }, "N6D1A5MV75"),
			new TestAuthenticator("EU-1108-0214-7179", new byte[] { 0x37, 0x8E, 0x81, 0x1B, 0x31, 0x1C, 0xF0, 0xF6, 0x03, 0xAF, 0x5E, 0x6A, 0xEB, 0x5B, 0x42, 0xBA, 0xB3, 0xB0, 0x78, 0x4C }, "F3R4VJ3D0R"),
			new TestAuthenticator("EU-1108-0214-7255", new byte[] { 0xB5, 0xA2, 0xA2, 0xC3, 0xC4, 0xE0, 0xCE, 0x5B, 0x83, 0x78, 0x8E, 0xE0, 0x19, 0x56, 0x74, 0x1B, 0x8B, 0x4E, 0x41, 0xEC }, "N0RHN2GY47"),
			new TestAuthenticator("EU-1108-0214-7281", new byte[] { 0xAB, 0x48, 0xA7, 0x7F, 0x4D, 0x37, 0x4D, 0xCC, 0xFB, 0x42, 0x08, 0xD4, 0xF9, 0xEC, 0xCF, 0x09, 0xEE, 0xA0, 0x58, 0x22 }, "M2TX3V3GBG")
		};

		//protected static string SERIAL = "US-1006-0970-8791";
		//protected static byte[] SECRETYKEY = new byte[] { 0x82, 0x0A, 0xB8, 0xF9, 0x66, 0x54, 0xF9, 0xBE, 0xAD, 0xB6, 0xD1, 0x27, 0x95, 0x81, 0x13, 0xD5, 0xC8, 0xAE, 0xBA, 0x36 };
		//protected static string RESTORECODE = "JDDU796WQ4";

		//protected static string SERIAL = "US-1108-0168-1578";
		//protected static byte[] SECRETYKEY = new byte[] { 0xFA, 0xA4, 0x62, 0xE9, 0x20, 0x60, 0xAA, 0xAF, 0xD1, 0x2D, 0x50, 0xB5, 0x65, 0xB8, 0xCD, 0x50, 0xA8, 0xDF, 0xC5, 0xB9 };
		//protected static string RESTORECODE = "703PQU2BVE";

		[Test]
		public void Enroll()
		{
			Authenticator auth = new Authenticator();
			auth.Enroll(null);
			Assert.IsNotNullOrEmpty(auth.Serial);
			Console.Out.WriteLine(auth.Serial + " " + Authenticator.ByteArrayToString(auth.SecretKey) + " " + auth.RestoreCode);
		}

		[Test]
		public void EnrollUK()
		{
			Authenticator auth = new Authenticator();
			auth.Enroll("GB");
			Assert.IsNotNullOrEmpty(auth.Serial);
			Console.Out.WriteLine(auth.Serial + " " + Authenticator.ByteArrayToString(auth.SecretKey) + " " + auth.RestoreCode);
		}

		[Test]
		public void Restore()
		{
			Authenticator auth = new Authenticator();
			auth.Restore(authenticators[5].Serial, authenticators[5].RestoreCode);
			Console.Out.WriteLine(auth.Serial + " " + Authenticator.ByteArrayToString(auth.SecretKey) + " " + auth.RestoreCode);
			//Assert.AreEqual(auth.SecretKey, authenticators[0].SecretKey);
		}

		[Test]
		public void CalcRestoreCode()
		{
			Authenticator auth = new Authenticator();
			auth.Serial = authenticators[4].Serial;
			auth.SecretKey = authenticators[4].SecretKey;
			string code = auth.RestoreCode;
			Assert.AreEqual(code, authenticators[4].RestoreCode);
		}

		//[Test]
		//public void RandomRestores()
		//{
		//  Random random = new Random((int)DateTime.Now.Ticks);
		//  for (int i = 0; i < 2; i++)
		//  {
		//    Authenticator auth = new Authenticator();
		//    byte[] secret = Authenticator.CreateOneTimePad(20);
		//    auth.SecretKey = secret;
		//    string serial = "US-" + random.Next(10000).ToString("0000") + "-" + random.Next(10000).ToString("0000") + "-" + random.Next(10000).ToString("0000");
		//    auth.Serial = serial;
		//    string restorecode = auth.RestoreCode;

		//    Authenticator verifyauth = new Authenticator(serial, restorecode);

		//    Assert.AreEqual(verifyauth.SecretKey, secret);

		//    Console.Out.WriteLine(serial + " " + restorecode);
		//  }
		//}

	}

}
