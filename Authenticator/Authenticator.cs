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
//using System.Web;

using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Digests;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class that implements Authenticator as per http://bnetauth.freeportal.us/specification.html
	/// </summary>
	public class Authenticator
	{
		/// <summary>
		/// Default model name for authenticator when enrolling
		/// </summary>
		private const string DEFAULT_MODEL = "Motorola RAZR v3";

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
		/// URLs for enrolling by region
		/// </summary>
		private static Dictionary<string, string> ENROLL_URLS = new Dictionary<string, string>()
		{
			{"US", "http://m.us.mobileservice.blizzard.com/enrollment/enroll.htm"},
			{"EU", "http://m.eu.mobileservice.blizzard.com/enrollment/enroll.htm"}
		};

		/// <summary>
		/// URLs for server time sync
		/// </summary>
		private static Dictionary<string, string> SYNC_URLS = new Dictionary<string, string>()
		{
			{"US", "http://m.us.mobileservice.blizzard.com/enrollment/time.htm"},
			{"EU", "http://m.eu.mobileservice.blizzard.com/enrollment/time.htm"}
		};

		/// <summary>
		/// The public key modulus used to encrypt our enrollment data
		/// </summary>
		private const string ENROLL_MODULUS =
			"955e4bd989f3917d2f15544a7e0504eb9d7bb66b6f8a2fe470e453c779200e5e" +
			"3ad2e43a02d06c4adbd8d328f1a426b83658e88bfd949b2af4eaf30054673a14" +
			"19a250fa4cc1278d12855b5b25818d162c6e6ee2ab4a350d401d78f6ddb99711" +
			"e72626b48bd8b5b0b7f3acf9ea3c9e0005fee59e19136cdb7c83f2ab8b0a2a99";

		/// <summary>
		/// Public key exponent used to encrypt our enrollment data
		/// </summary>
		private const string ENROLL_EXPONENT =
			"0101";

		/// <summary>
		/// Authentication data: secret key and serial
		/// </summary>
		private AuthenticatorData m_data;

		/// <summary>
		/// Create a new Authenticator object for a given region
		/// </summary>
		/// <param name="region">region name US or EU</param>
		public Authenticator(string region)
		{
			m_data = new AuthenticatorData();
			m_data.Region = region;
		}

		/// <summary>
		/// Create a new Authenticator object for a given region
		/// </summary>
		/// <param name="data">previously saved authenticator data</param>
		public Authenticator(AuthenticatorData data)
		{
			m_data = data;
		}

		/// <summary>
		/// Enroll this authenticator with the server.
		/// Return the Secret data to be stored.
		/// </summary>
		/// <returns>secret data block to be stored</returns>
		public AuthenticatorData Enroll()
		{
			// generate byte array of data:
			//  00 byte[1] a value of 1;
			//  01 byte[37] one-time key used to decrypt data when returned;
			//  38 byte[2] region: US or EU;
			//  40 byte[16] model string for this device;

			// byte the byte array
			byte[] bytes = new byte[56];
			int index = 0;
			// add code
			byte[] code = new byte[] {1};
			code.CopyTo(bytes, index);
			index += code.Length;
			// add random key
			byte[] key = CreateInitializationRandom();
			key.CopyTo(bytes, index);
			index += key.Length;
			// add region
			byte[] regionarray = Encoding.UTF8.GetBytes(Data.Region);
			regionarray.CopyTo(bytes, index);
			index += regionarray.Length;
			// add model name
			byte[] model = Encoding.UTF8.GetBytes(DEFAULT_MODEL);
			model.CopyTo(bytes, index);
			index += model.Length;

			// encrypt out byte[56] with the ENROLL public key

			// use Legion of the Bouncy Castle RSA implementation (because we need no padding - can't get MS to work)
			RsaEngine rsa = new RsaEngine();
			rsa.Init(true, new RsaKeyParameters(false, new Org.BouncyCastle.Math.BigInteger(ENROLL_MODULUS, 16), new Org.BouncyCastle.Math.BigInteger(ENROLL_EXPONENT, 16)));
			byte[] requestData = rsa.ProcessBlock(bytes, 0, bytes.Length);

			//RSAParameters rsaparams = new RSAParameters();
			//rsaparams.Modulus = StringToByteArray(ENROLL_MODULUS);
			//rsaparams.Exponent = StringToByteArray(ENROLL_EXPONENT);
			//RSACryptoServiceProvider rsa = new RSACryptoServiceProvider();
			//rsa.ImportParameters(rsaparams);
			//byte[] requestData = rsa.Encrypt(bytes, false);
			//Array.Reverse(requestData);

			// create a connection to initialisation server
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(ENROLL_URLS[Data.Region]);
			request.Method = "POST";
			request.ContentType = "application/octet-stream";
			request.ContentLength = requestData.Length;

			// write our encrypted data
			Stream requestStream = request.GetRequestStream();
			requestStream.Write(requestData, 0, requestData.Length);
			requestStream.Close();

			// get response
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
			// 08-45 init data encrpyted with our key

			// extract the server time
			byte[] serverTime = new byte[8];
			Array.Copy(responseData, serverTime, 8);
			if (BitConverter.IsLittleEndian == true)
			{
				Array.Reverse(serverTime);
			}
			// get the difference between the server time and our current time
			long serverTimeDiff = BitConverter.ToInt64(serverTime, 0) - CurrentTime;

			// extract the init data
			byte[] initData = new byte[37];
			Array.Copy(responseData, 8, initData, 0, 37);
			// decrypt the initdata with a simple xor with our key
			for (int i = 0; i < initData.Length; i++)
			{
				initData[i] ^= key[i];
			}

			// init data:
			// 00-19 secret key
			// 20-37 serial number

			// get the secret key
			byte[] secretKey = new byte[20];
			Array.Copy(initData, secretKey, 20);
			// get the serial number
			string serial = Encoding.Default.GetString(initData, 20, 17);

			// set the data member
			AuthenticatorData data = new AuthenticatorData();
			data.Region = Data.Region;
			data.SecretKey = secretKey;
			data.Serial = serial;
			data.ServerTimeDiff = serverTimeDiff;
			m_data = data;

			return m_data;
		}

		/// <summary>
		/// Synchorise this authenticator's time with server time. We update our data record with the difference from our UTC time.
		/// </summary>
		public void Sync()
		{
			if (Data == null || string.IsNullOrEmpty(Data.Region))
			{
				return;
			}

			// create a connection to time sync server
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(SYNC_URLS[Data.Region]);
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
			Data.ServerTimeDiff = serverTimeDiff;
		}

		/// <summary>
		/// Get the current AuthenticationData object
		/// </summary>
		public AuthenticatorData Data
		{
			get
			{
				return m_data;
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
				return (CurrentTime + Data.ServerTimeDiff) / 30000L;
			}
		}

		/// <summary>
		/// Calculate the current code for the authenticator.
		/// </summary>
		/// <returns>authenticator code</returns>
		public string CalculateCode()
		{
			return CalculateCode(false);
		}

		/// <summary>
		/// Calculate the current code for the authenticator.
		/// </summary>
		/// <param name="resyncTime">flag to resync time</param>
		/// <returns>authenticator code</returns>
		public string CalculateCode(bool resyncTime)
		{
			// sync time if required
			if (resyncTime == true || this.Data.ServerTimeDiff == 0)
			{
				Sync();
			}

			HMac hmac = new HMac(new Sha1Digest());
			hmac.Init(new KeyParameter(Data.SecretKey));

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
		/// Create an initial random block of 37 bytes for a secret key. Seed Random with the current time
		/// and perform SHA1 on random bytes taking first 37 bytes of resultant 40 byte block.
		/// </summary>
		/// <returns>random byte[37] array</returns>
		protected static byte[] CreateInitializationRandom()
		{
			Random random = new Random((int)CurrentTime);

			byte[] hashBlock = new byte[128];
			for (int i = hashBlock.Length-1; i >= 0; i--)
			{
				hashBlock[i] = (byte)random.Next(256);
			}

			SHA1 sha1 = SHA1.Create();
			byte[] key1 = sha1.ComputeHash(hashBlock, 0, 64);
			byte[] key2 = sha1.ComputeHash(hashBlock, 64, 64);
			byte[] key = new byte[37];
			Array.Copy(key1, key, 20);
			Array.Copy(key2, 0, key, 20, 17);

			return key;
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

		public static string ByteArrayToString(byte[] bytes)
		{
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}

		/// <summary>
		/// Get the server time since 1/1/70
		/// </summary>
		public long ServerTime
		{
			get
			{
				return CurrentTime + Data.ServerTimeDiff;
			}
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


	}

}
