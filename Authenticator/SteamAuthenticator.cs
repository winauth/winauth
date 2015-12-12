/*
 * Copyright (C) 2015 Colin Mackie.
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
using System.Collections.Specialized;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Cache;
using System.Security.Cryptography;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Xml;
using System.Xml.Serialization;
using System.Xml.XPath;

using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

#if NUNIT
using NUnit.Framework;
#endif

#if NETCF
using OpenNETCF.Security.Cryptography;
#endif

namespace WinAuth
{
	/// <summary>
	/// Class that implements Steam's SteamGuadrd version of the RFC6238 Authenticator
	/// </summary>
	public class SteamAuthenticator : Authenticator
	{
		/// <summary>
		/// Number of characters in code
		/// </summary>
		private const int CODE_DIGITS = 5;

		/// <summary>
		/// Number of minutes to ignore syncing if network error
		/// </summary>
		private const int SYNC_ERROR_MINUTES = 60;

		/// <summary>
		/// Number of attempts to activate
		/// </summary>
		private const int ENROLL_ACTIVATE_RETRIES = 30;

		/// <summary>
		/// Incorrect activation code
		/// </summary>
		private const int INVALID_ACTIVATION_CODE = 89;

		/// <summary>
		/// Steam issuer for KeyUri
		/// </summary>
		private const string STEAM_ISSUER = "Steam";

		/// <summary>
		/// URLs for all mobile services
		/// </summary>
		private static string COMMUNITY_BASE = "https://steamcommunity.com";
		private static string WEBAPI_BASE = "https://api.steampowered.com";
		private static string SYNC_URL = "https://api.steampowered.com:443/ITwoFactorService/QueryTime/v0001";

		/// <summary>
		/// Character set for authenticator code
		/// </summary>
		private static char[] STEAMCHARS = new char[] {
				'2', '3', '4', '5', '6', '7', '8', '9', 'B', 'C',
				'D', 'F', 'G', 'H', 'J', 'K', 'M', 'N', 'P', 'Q',
				'R', 'T', 'V', 'W', 'X', 'Y'};

		/// <summary>
		/// Enrolling state
		/// </summary>
		public class EnrollState
		{
			public string Username { get; set; }
			public string Password { get; set; }
			public string CaptchaId { get; set; }
			public string CaptchaUrl { get; set; }
			public string CaptchaText { get; set; }
			public string EmailDomain { get; set; }
			public string EmailAuthText { get; set; }
			public string ActivationCode { get; set; }
			public CookieContainer Cookies { get; set; }

			public string SteamId { get; set; }
			public string OAuthToken { get; set; }

			public bool RequiresLogin { get; set; }
			public bool RequiresCaptcha { get; set; }
			public bool Requires2FA { get; set; }
			public bool RequiresEmailAuth { get; set; }
			public bool RequiresActivation { get; set; }

			public string RevocationCode { get; set; }
			public string SecretKey { get; set; }
			public bool Success { get; set; }

			public string Error { get; set; }
		}

		public class SteamSession
		{
			public string Username { get; set; }
			public string Password { get; set; }
			public string AuthCode { get; set; }
			public string CaptchaId { get; set; }
			public string CaptchaUrl { get; set; }
			public string CaptchaText { get; set; }
			public string EmailDomain { get; set; }
			public string EmailAuthText { get; set; }
			public CookieContainer Cookies { get; set; }

			public string SteamId { get; set; }
			public string OAuthToken { get; set; }

			public bool RequiresLogin { get; set; }
			public bool RequiresCaptcha { get; set; }
			public bool Requires2FA { get; set; }
			public bool RequiresEmailAuth { get; set; }

			public string Error { get; set; }

			public SteamSession()
			{
				this.Cookies = new CookieContainer();
			}

			public byte[] GetData(string url)
			{
				return Request(url, "GET");
			}

			public string GetString(string url)
			{
				byte[] data = Request(url, "GET");
				if (data == null || data.Length == 0)
				{
					return string.Empty;
				}
				else
				{
					return Encoding.UTF8.GetString(data);
				}
			}

			public byte[] Request(string url, string method, NameValueCollection data = null, NameValueCollection headers = null)
			{
				// create form-encoded data for query or body
				string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(data[key])))));
				if (string.Compare(method, "GET", true) == 0)
				{
					url += (url.IndexOf("?") == -1 ? "?" : "&") + query;
				}

				// call the server
				HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
				request.Method = method;
				request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
				request.ServicePoint.Expect100Continue = false;
				request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
				request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
				request.Referer = COMMUNITY_BASE; // + "/mobilelogin?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client";
				if (headers != null)
				{
					request.Headers.Add(headers);
				}

				request.CookieContainer = this.Cookies;

				if (string.Compare(method, "POST", true) == 0)
				{
					request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
					request.ContentLength = query.Length;

					StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
					requestStream.Write(query);
					requestStream.Close();
				}

				try
				{
					using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
					{
						// OK?
						if (response.StatusCode != HttpStatusCode.OK)
						{
							throw new InvalidRequestException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
						}

						// load the response
						using (MemoryStream ms = new MemoryStream())
						{
							byte[] buffer = new byte[4096];
							int read;
							while ((read = response.GetResponseStream().Read(buffer, 0, 4096)) > 0)
							{
								ms.Write(buffer, 0, read);
							}

							return ms.ToArray();
						}
      //      using (var responseStream = new StreamReader(response.GetResponseStream()))
						//{
						//	string responseData = responseStream.ReadToEnd();
						//	return responseData;
						//}
					}
				}
				catch (Exception ex)
				{
					throw new InvalidRequestException(ex.Message, ex);
				}
			}
		}

		#region Authenticator data

		/// <summary>
		/// Time of last Sync error
		/// </summary>
		private static DateTime _lastSyncError = DateTime.MinValue;

		/// <summary>
		/// Returned serial number of authenticator
		/// </summary>
		public string Serial { get; set; }

		/// <summary>
		/// Random device ID we created and registered
		/// </summary>
		public string DeviceId { get; set; }

		/// <summary>
		/// JSON steam data
		/// </summary>
		public string SteamData { get; set; }

		/// <summary>
		/// Revocation code
		/// </summary>
		//public string RevocationCode { get; set; }

		#endregion

		/// <summary>
		/// Expanding offsets to retry when creating first code
		/// </summary>
		private int[] ENROLL_OFFSETS = new int[] { 0, -30, 30, -60, 60, -90, 90, -120, 120};

		/// <summary>
		/// Create a new Authenticator object
		/// </summary>
		public SteamAuthenticator()
			: base(CODE_DIGITS)
		{
			Issuer = STEAM_ISSUER;
		}

		/// <summary>
		/// Get/set the combined secret data value
		/// </summary>
		public override string SecretData
		{
			get
			{
				// this is the key |  serial | deviceid
				return base.SecretData
					+ "|" + Authenticator.ByteArrayToString(Encoding.UTF8.GetBytes(Serial))
					+ "|" + Authenticator.ByteArrayToString(Encoding.UTF8.GetBytes(DeviceId))
					//+ "|" + Authenticator.ByteArrayToString(Encoding.UTF8.GetBytes(RevocationCode));
					+ "|" + Authenticator.ByteArrayToString(Encoding.UTF8.GetBytes(SteamData));
			}
			set
			{
				// extract key + serial + deviceid
				if (string.IsNullOrEmpty(value) == false)
				{
					string[] parts = value.Split('|');
					base.SecretData = value;
					Serial = (parts.Length > 1 ? Encoding.UTF8.GetString(Authenticator.StringToByteArray(parts[1])) : null);
					DeviceId = (parts.Length > 2 ? Encoding.UTF8.GetString(Authenticator.StringToByteArray(parts[2])) : null);
					//RevocationCode = (parts.Length > 3 ? Encoding.UTF8.GetString(Authenticator.StringToByteArray(parts[3])) : string.Empty);
					SteamData = (parts.Length > 3 ? Encoding.UTF8.GetString(Authenticator.StringToByteArray(parts[3])) : string.Empty);
					if (string.IsNullOrEmpty(SteamData) == false && SteamData[0] != '{')
					{
						// convert old recovation code into SteamData json
						SteamData = "{\"revocation_code\":\"" + SteamData + "\"}";
					}
				}
				else
				{
					SecretKey = null;
					Serial = null;
					DeviceId = null;
					//RevocationCode = null;
					SteamData = null;
				}
			}
		}

		/// <summary>
		/// Perform a request to the Steam WebAPI service
		/// </summary>
		/// <param name="url">API url</param>
		/// <param name="method">GET or POST</param>
		/// <param name="data">Name-data pairs</param>
		/// <param name="cookies">current cookie container</param>
		/// <returns>response body</returns>
		private string Request(string url, string method, NameValueCollection data = null, CookieContainer cookies = null, NameValueCollection headers = null)
		{
			// create form-encoded data for query or body
			string query = (data == null ? string.Empty : string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(data[key])))));
			if (string.Compare(method, "GET", true) == 0)
			{
				url += (url.IndexOf("?") == -1 ? "?" : "&") + query;
			}

			// call the server
			HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
			request.Method = method;
			request.Accept = "text/javascript, text/html, application/xml, text/xml, */*";
			request.ServicePoint.Expect100Continue = false;
			request.UserAgent = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			request.Referer = COMMUNITY_BASE; // + "/mobilelogin?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client";
			if (headers != null)
			{
				request.Headers.Add(headers);
			}

			if (cookies != null)
			{
				request.CookieContainer = cookies;
			}

			if (string.Compare(method, "POST", true) == 0)
			{
				request.ContentType = "application/x-www-form-urlencoded; charset=UTF-8";
				request.ContentLength = query.Length;

				StreamWriter requestStream = new StreamWriter(request.GetRequestStream());
				requestStream.Write(query);
				requestStream.Close();
			}

			try {
				using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
				{
#if DEBUG
					string s = response.ToString();
					LogRequest(method, url, cookies, data, response);
#endif

					// OK?
					if (response.StatusCode != HttpStatusCode.OK)
					{
						throw new InvalidRequestException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
					}

					// load the response
					using (StreamReader responseStream = new StreamReader(response.GetResponseStream()))
					{
						string responseData = responseStream.ReadToEnd();
#if DEBUG
						LogRequest(method, url, cookies, data, responseData);
#endif

						return responseData;
					}
				}
			}
			catch (Exception ex)
			{
#if DEBUG
				LogRequest(method, url, cookies, data, ex);
#endif
				throw new InvalidRequestException(ex.Message, ex);
			}
		}

		/// <summary>
		/// Enroll the authenticator with the server
		/// </summary>
		public bool Enroll(EnrollState state)
		{
			// clear error
			state.Error = null;

			try
			{
				var data = new NameValueCollection();
				var cookies = state.Cookies = state.Cookies ?? new CookieContainer();
				string response;

				if (string.IsNullOrEmpty(state.OAuthToken) == true)
				{
					// get session
					if (cookies.Count == 0)
					{
						cookies.Add(new Cookie("mobileClientVersion", "3067969+%282.1.3%29", "/", ".steamcommunity.com")); 
            cookies.Add(new Cookie("mobileClient", "android", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("steamid", "", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("steamLogin", "", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("Steam_Language", "english", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("dob", "", "/", ".steamcommunity.com"));

						NameValueCollection headers = new NameValueCollection();
						headers.Add("X-Requested-With", "com.valvesoftware.android.steam.community");

						response = Request("https://steamcommunity.com/login?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client", "GET", null, cookies, headers);
					}

					// get the user's RSA key
					data.Add("username", state.Username);
					response = Request(COMMUNITY_BASE + "/login/getrsakey", "POST", data, cookies);
					var rsaresponse = JObject.Parse(response);
					if (rsaresponse.SelectToken("success").Value<bool>() != true)
					{
						throw new InvalidEnrollResponseException("Cannot get steam information for user: " + state.Username);
					}
					//state.SteamId = rsaresponse.SelectToken("steamid").Value<string>();

					// encrypt password with RSA key
					RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
					byte[] encryptedPassword;
					using (var rsa = new RSACryptoServiceProvider())
					{
						var passwordBytes = Encoding.ASCII.GetBytes(state.Password);
						var p = rsa.ExportParameters(false);
						p.Exponent = Authenticator.StringToByteArray(rsaresponse.SelectToken("publickey_exp").Value<string>());
						p.Modulus = Authenticator.StringToByteArray(rsaresponse.SelectToken("publickey_mod").Value<string>());
						rsa.ImportParameters(p);
						encryptedPassword = rsa.Encrypt(passwordBytes, false);
					}

					// login request
					data = new NameValueCollection();
					data.Add("password", Convert.ToBase64String(encryptedPassword));
					data.Add("username", state.Username);
					data.Add("twofactorcode", "");
					data.Add("emailauth", (state.EmailAuthText != null ? state.EmailAuthText : string.Empty));
					data.Add("loginfriendlyname", "#login_emailauth_friendlyname_mobile");
					data.Add("captchagid", (state.CaptchaId != null ? state.CaptchaId : "-1"));
					data.Add("captcha_text", (state.CaptchaText != null ? state.CaptchaText : "enter above characters"));
					data.Add("emailsteamid", (state.EmailAuthText != null ? state.SteamId ?? string.Empty: string.Empty));
					data.Add("rsatimestamp", rsaresponse.SelectToken("timestamp").Value<string>());
					data.Add("remember_login", "false");
					data.Add("oauth_client_id", "DE45CD61");
					data.Add("oauth_scope", "read_profile write_profile read_client write_client");
					data.Add("donotache", new DateTime().ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString());
					response = Request(COMMUNITY_BASE + "/login/dologin/", "POST", data, cookies);
					Dictionary<string, object> loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

					if (loginresponse.ContainsKey("emailsteamid") == true)
					{
						state.SteamId = loginresponse["emailsteamid"] as string;
					}

					// require captcha
					if (loginresponse.ContainsKey("captcha_needed") == true && (bool)loginresponse["captcha_needed"] == true)
					{
						state.RequiresCaptcha = true;
						state.CaptchaId = (string)loginresponse["captcha_gid"];
						state.CaptchaUrl = COMMUNITY_BASE + "/public/captcha.php?gid=" + state.CaptchaId;
					}
					else
					{
						state.RequiresCaptcha = false;
						state.CaptchaId = null;
						state.CaptchaUrl = null;
						state.CaptchaText = null;
					}

					// require email auth
					if (loginresponse.ContainsKey("emailauth_needed") == true && (bool)loginresponse["emailauth_needed"] == true)
					{
						if (loginresponse.ContainsKey("emaildomain") == true)
						{
							var emaildomain = (string)loginresponse["emaildomain"];
							if (string.IsNullOrEmpty(emaildomain) == false)
							{
								state.EmailDomain = emaildomain;
							}
						}
						state.RequiresEmailAuth = true;
					}
					else
					{
						state.EmailDomain = null;
						state.RequiresEmailAuth = false;
					}

					// require email auth
					if (loginresponse.ContainsKey("requires_twofactor") == true && (bool)loginresponse["requires_twofactor"] == true)
					{
						state.Requires2FA = true;
					}
					else
					{
						state.Requires2FA = false;
					}

					// if we didn't login, return the result
					if (loginresponse.ContainsKey("login_complete") == false || (bool)loginresponse["login_complete"] == false || loginresponse.ContainsKey("oauth") == false)
					{
						if (loginresponse.ContainsKey("oauth") == false)
						{
							state.Error = "No OAuth token in response";
						}
						if (loginresponse.ContainsKey("message") == true)
						{
							state.Error = (string)loginresponse["message"];
						}
						return false;
					}

					// get the OAuth token - is stringified json
					string oauth = (string)loginresponse["oauth"];
					var oauthjson = JObject.Parse(oauth);
					state.OAuthToken = oauthjson.SelectToken("oauth_token").Value<string>();
					if (oauthjson.SelectToken("steamid") != null)
					{
						state.SteamId = oauthjson.SelectToken("steamid").Value<string>();
					}
				}

				// login to webapi
				data.Clear();
				data.Add("access_token", state.OAuthToken);
				response = Request(WEBAPI_BASE + "/ISteamWebUserPresenceOAuth/Logon/v0001", "POST", data);

				var sessionid = cookies.GetCookies(new Uri(COMMUNITY_BASE + "/"))["sessionid"].Value;

				if (state.RequiresActivation == false)
				{
					data.Clear();
					data.Add("op", "has_phone");
					data.Add("arg", "null");
					data.Add("sessionid", sessionid);
					response = Request(COMMUNITY_BASE + "/steamguard/phoneajax", "POST", data, cookies);
					var jsonresponse = JObject.Parse(response);
					var hasPhone = jsonresponse.SelectToken("has_phone").Value<Boolean>();
					if (hasPhone == false)
					{
						state.OAuthToken = null; // force new login
						state.RequiresLogin = true;
						state.Cookies = null;
						state.Error = "Your Steam account must have a SMS-capable phone number attached. Go into Account Details of the Steam client or Steam website and click Add a Phone Number.";
						return false;
					}

					//response = Request(COMMUNITY_BASE + "/steamguard/phone_checksms?bForTwoFactor=1&bRevoke2fOnCancel=", "GET", null, cookies);

					// add a new authenticator
					data.Clear();
					string deviceId = BuildRandomId();
					data.Add("access_token", state.OAuthToken);
					data.Add("steamid", state.SteamId);
					data.Add("authenticator_type", "1");
					data.Add("device_identifier", deviceId);
					data.Add("sms_phone_id", "1");
					response = Request(WEBAPI_BASE + "/ITwoFactorService/AddAuthenticator/v0001", "POST", data);
					var tfaresponse = JObject.Parse(response);
					if (response.IndexOf("status") == -1 && tfaresponse.SelectToken("response.status").Value<int>() == 84)
					{
						// invalid response
						state.OAuthToken = null; // force new login
						state.RequiresLogin = true;
						state.Cookies = null;
						state.Error = "Unable to send SMS. Check your phone is registered on your Steam account.";
						return false;
					}
					if (response.IndexOf("shared_secret") == -1)
					{
						// invalid response
						state.OAuthToken = null; // force new login
						state.RequiresLogin = true;
						state.Cookies = null;
						state.Error = "Invalid response from Steam: " + response;
						return false;
					}

					// save data into this authenticator
					var secret = tfaresponse.SelectToken("response.shared_secret").Value<string>();
					this.SecretKey = Convert.FromBase64String(secret);
					this.Serial = tfaresponse.SelectToken("response.serial_number").Value<string>();
					this.DeviceId = deviceId;
					state.RevocationCode = tfaresponse.SelectToken("response.revocation_code").Value<string>();

					// add the steamid into the data
					var steamdata = JObject.Parse(tfaresponse.SelectToken("response").ToString());
					if (steamdata.SelectToken("steamid") == null)
					{
						steamdata.Add("steamid", state.SteamId);
					}
					if (steamdata.SelectToken("steamguard_scheme") == null)
					{
						steamdata.Add("steamguard_scheme", "2");
					}
					this.SteamData = steamdata.ToString(Newtonsoft.Json.Formatting.None);

					// calculate server drift
					long servertime = tfaresponse.SelectToken("response.server_time").Value<long>() * 1000;
					ServerTimeDiff = servertime - CurrentTime;
					LastServerTime = DateTime.Now.Ticks;

					state.RequiresActivation = true;

					return false;
				}

				// finalize adding the authenticator
				data.Clear();
				data.Add("access_token", state.OAuthToken);
				data.Add("steamid", state.SteamId);
				data.Add("activation_code", state.ActivationCode);

				// try and authorise
				var retries = 0;
				while (state.RequiresActivation == true && retries < ENROLL_ACTIVATE_RETRIES)
				{
					data.Add("authenticator_code", this.CalculateCode(false));
					data.Add("authenticator_time", this.ServerTime.ToString());
					response = Request(WEBAPI_BASE + "/ITwoFactorService/FinalizeAddAuthenticator/v0001", "POST", data);
					var finalizeresponse = JObject.Parse(response);
					if (response.IndexOf("status") != -1 && finalizeresponse.SelectToken("response.status").Value<int>() == INVALID_ACTIVATION_CODE)
					{
						state.Error = "Invalid activation code";
						return false;
					}

					// reset our time
					if (response.IndexOf("server_time") != -1)
					{
						long servertime = finalizeresponse.SelectToken("response.server_time").Value<long>() * 1000;
						ServerTimeDiff = servertime - CurrentTime;
						LastServerTime = DateTime.Now.Ticks;
					}

					// check success
					if (finalizeresponse.SelectToken("response.success").Value<bool>() == true)
					{
						if (response.IndexOf("want_more") != -1 && finalizeresponse.SelectToken("response.want_more").Value<bool>() == true)
						{
							ServerTimeDiff += 30000L;
							retries++;
							continue;
						}
						state.RequiresActivation = false;
						break;
					}

					ServerTimeDiff += 30000L;
					retries++;
				}
				if (state.RequiresActivation == true)
				{
					state.Error = "There was a problem activating. There might be an issue with the Steam servers. Please try again later.";
					return false;
				}

				// mark and successful and return key
				state.Success = true;
				state.SecretKey = Authenticator.ByteArrayToString(this.SecretKey);

				// send confirmation email
				data.Clear();
				data.Add("access_token", state.OAuthToken);
				data.Add("steamid", state.SteamId);
				data.Add("email_type", "2");
				response = Request(WEBAPI_BASE + "/ITwoFactorService/SendEmail/v0001", "POST", data);

				return true;
			}
			catch (InvalidRequestException ex)
			{
				throw new InvalidEnrollResponseException("Error enrolling new authenticator", ex);
			}
		}

		/// <summary>
		/// Synchronise this authenticator's time with Steam.
		/// </summary>
		public override void Sync()
		{
			// check if data is protected
			if (this.SecretKey == null && this.EncryptedData != null)
			{
				throw new EncrpytedSecretDataException();
			}

			// don't retry for 5 minutes
			if (_lastSyncError >= DateTime.Now.AddMinutes(0 - SYNC_ERROR_MINUTES))
			{
				return;
			}

			try
			{
				var response = Request(SYNC_URL, "POST");
				var json = JObject.Parse(response);

				// get servertime in ms
				long servertime = json.SelectToken("response.server_time").Value<long>() * 1000;

				// get the difference between the server time and our current time
				ServerTimeDiff = servertime - CurrentTime;
				LastServerTime = DateTime.Now.Ticks;

				// clear any sync error
				_lastSyncError = DateTime.MinValue;
			}
			catch (Exception )
			{
				// don't retry for a while after error
				_lastSyncError = DateTime.Now;

				// set to zero to force reset
				//ServerTimeDiff = 0;
			}
		}

		/// <summary>
		/// Calculate the current code for the authenticator.
		/// </summary>
		/// <param name="resyncTime">flag to resync time</param>
		/// <returns>authenticator code</returns>
		protected override string CalculateCode(bool resyncTime = false, long interval = -1)
		{
			// sync time if required
			if (resyncTime == true || ServerTimeDiff == 0)
			{
				if (interval > 0)
				{
					ServerTimeDiff = (interval * 30000L) - CurrentTime;
				}
				else
				{
					Sync();
				}
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

			// build the alphanumeric code
			StringBuilder code = new StringBuilder();
			for (var i=0; i<CODE_DIGITS; i++)
			{
				code.Append(STEAMCHARS[fullcode % STEAMCHARS.Length]);
				fullcode /= (uint)STEAMCHARS.Length;
			}

			return code.ToString();
		}

		/// <summary>
		/// Load the trade confirmations
		/// </summary>
		/// <param name="state">current Steam session</param>
		/// <returns></returns>
		public string LoadTrades(SteamSession state)
		{
			// clear error
			state.Error = null;

			try
			{
				var data = new NameValueCollection();
				var cookies = state.Cookies = state.Cookies ?? new CookieContainer();
				string response;

				if (string.IsNullOrEmpty(state.OAuthToken) == true)
				{
					// get session
					if (cookies.Count == 0)
					{
						cookies.Add(new Cookie("mobileClientVersion", "3067969+%282.1.3%29", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("mobileClient", "android", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("steamid", "", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("steamLogin", "", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("Steam_Language", "english", "/", ".steamcommunity.com"));
						cookies.Add(new Cookie("dob", "", "/", ".steamcommunity.com"));

						NameValueCollection headers = new NameValueCollection();
						headers.Add("X-Requested-With", "com.valvesoftware.android.steam.community");

						response = Request("https://steamcommunity.com/login?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client", "GET", null, cookies, headers);
					}

					// get the user's RSA key
					data.Add("username", state.Username);
					response = Request(COMMUNITY_BASE + "/login/getrsakey", "POST", data, cookies);
					var rsaresponse = JObject.Parse(response);
					if (rsaresponse.SelectToken("success").Value<bool>() != true)
					{
						throw new InvalidTradesResponseException("Cannot get steam information for user: " + state.Username);
					}

					// encrypt password with RSA key
					RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
					string encryptedPassword64;
					using (var rsa = new RSACryptoServiceProvider())
					{
						var passwordBytes = Encoding.ASCII.GetBytes(state.Password);
						var p = rsa.ExportParameters(false);
						p.Exponent = Authenticator.StringToByteArray(rsaresponse.SelectToken("publickey_exp").Value<string>());
						p.Modulus = Authenticator.StringToByteArray(rsaresponse.SelectToken("publickey_mod").Value<string>());
						rsa.ImportParameters(p);
						byte[] encryptedPassword = rsa.Encrypt(passwordBytes, false);
						encryptedPassword64 = Convert.ToBase64String(encryptedPassword);
					}

					// login request
					data = new NameValueCollection();
					data.Add("password", encryptedPassword64);
					data.Add("username", state.Username);
					data.Add("twofactorcode", state.AuthCode ?? string.Empty);
					data.Add("emailauth", (state.EmailAuthText != null ? state.EmailAuthText : string.Empty));
					data.Add("loginfriendlyname", "#login_emailauth_friendlyname_mobile");
					data.Add("captchagid", (state.CaptchaId != null ? state.CaptchaId : "-1"));
					data.Add("captcha_text", (state.CaptchaText != null ? state.CaptchaText : "enter above characters"));
					data.Add("emailsteamid", (state.EmailAuthText != null ? state.SteamId ?? string.Empty : string.Empty));
					data.Add("rsatimestamp", rsaresponse.SelectToken("timestamp").Value<string>());
					data.Add("remember_login", "false");
					data.Add("oauth_client_id", "DE45CD61");
					data.Add("oauth_scope", "read_profile write_profile read_client write_client");
					data.Add("donotache", new DateTime().ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString());
					response = Request(COMMUNITY_BASE + "/login/dologin/", "POST", data, cookies);
					Dictionary<string, object> loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

					if (loginresponse.ContainsKey("emailsteamid") == true)
					{
						state.SteamId = loginresponse["emailsteamid"] as string;
					}

					// require captcha
					if (loginresponse.ContainsKey("captcha_needed") == true && (bool)loginresponse["captcha_needed"] == true)
					{
						state.RequiresCaptcha = true;
						state.CaptchaId = (string)loginresponse["captcha_gid"];
						state.CaptchaUrl = COMMUNITY_BASE + "/public/captcha.php?gid=" + state.CaptchaId;
					}
					else
					{
						state.RequiresCaptcha = false;
						state.CaptchaId = null;
						state.CaptchaUrl = null;
						state.CaptchaText = null;
					}

					// require email auth
					if (loginresponse.ContainsKey("emailauth_needed") == true && (bool)loginresponse["emailauth_needed"] == true)
					{
						if (loginresponse.ContainsKey("emaildomain") == true)
						{
							var emaildomain = (string)loginresponse["emaildomain"];
							if (string.IsNullOrEmpty(emaildomain) == false)
							{
								state.EmailDomain = emaildomain;
							}
						}
						state.RequiresEmailAuth = true;
					}
					else
					{
						state.EmailDomain = null;
						state.RequiresEmailAuth = false;
					}

					// require email auth
					if (loginresponse.ContainsKey("requires_twofactor") == true && (bool)loginresponse["requires_twofactor"] == true)
					{
						state.Requires2FA = true;
					}
					else
					{
						state.Requires2FA = false;
					}

					// if we didn't login, return the result
					if (loginresponse.ContainsKey("login_complete") == false || (bool)loginresponse["login_complete"] == false || loginresponse.ContainsKey("oauth") == false)
					{
						if (loginresponse.ContainsKey("oauth") == false)
						{
							state.Error = "No OAuth token in response";
						}
						if (loginresponse.ContainsKey("message") == true)
						{
							state.Error = (string)loginresponse["message"];
						}

						return null;
					}

					// get the OAuth token
					string oauth = (string)loginresponse["oauth"];
					var oauthjson = JObject.Parse(oauth);
					state.OAuthToken = oauthjson.SelectToken("oauth_token").Value<string>();
					if (oauthjson.SelectToken("steamid") != null)
					{
						state.SteamId = oauthjson.SelectToken("steamid").Value<string>();

						// add the steamid into the data if missing
						var steamdata = JObject.Parse(this.SteamData);
						if (steamdata.SelectToken("steamid") == null)
						{
							steamdata.Add("steamid", state.SteamId);
							this.SteamData = steamdata.ToString(Newtonsoft.Json.Formatting.None);
						}
					}
				}

				// get servertime
				//var timeresponse = Request(SYNC_URL, "POST");
				//var timejson = JObject.Parse(timeresponse);
				//long servertime = timejson.SelectToken("response.server_time").Value<long>();
				long servertime = (CurrentTime + ServerTimeDiff) / 1000L;

				var jids = JObject.Parse(this.SteamData).SelectToken("identity_secret");
				string ids = (jids != null ? jids.Value<string>() : string.Empty);

				var timehash = CreateTimeHash(servertime, "conf", ids);

				data.Clear();
				data.Add("p", this.DeviceId);
				data.Add("a", state.SteamId);
				data.Add("k", timehash);
				data.Add("t", servertime.ToString());
				data.Add("m", "android");
				data.Add("tag", "conf");

				return Request(COMMUNITY_BASE + "/mobileconf/conf", "GET", data, state.Cookies);
			}
			catch (InvalidRequestException ex)
			{
				state.Error = ex.Message;
				throw new InvalidTradesResponseException("Error trading", ex);
			}
		}

		/// <summary>
		/// Confirm or reject a specific trade confirmation
		/// </summary>
		/// <param name="state">Steam state</param>
		/// <param name="id">id of trade</param>
		/// <param name="key">key for trade</param>
		/// <param name="accept">true to accept, false to reject</param>
		/// <returns>response data</returns>
		public string ConfirmTrade(SteamSession state, string id, string key, bool accept)
		{
			// clear error
			state.Error = null;

			try
			{
				// get servertime
				long servertime = (CurrentTime + ServerTimeDiff) / 1000L;

				var jids = JObject.Parse(this.SteamData).SelectToken("identity_secret");
				string ids = (jids != null ? jids.Value<string>() : string.Empty);
				var timehash = CreateTimeHash(servertime, "conf", ids);

				var data = new NameValueCollection();
				data.Add("op", accept ? "allow" : "cancel");
				data.Add("p", this.DeviceId);
				data.Add("a", state.SteamId);
				data.Add("k", timehash);
				data.Add("t", servertime.ToString());
				data.Add("m", "android");
				data.Add("tag", "conf");
				//
				data.Add("cid", id);
				data.Add("ck", key);
				return Request(COMMUNITY_BASE + "/mobileconf/ajaxop", "GET", data, state.Cookies);
			}
			catch (InvalidRequestException ex)
			{
				state.Error = ex.Message;
				throw new InvalidTradesResponseException("Error trading", ex);
			}
		}

		private static string CreateTimeHash(long time, string tag, string secret)
		{
			byte[] b64secret = Convert.FromBase64String(secret);

			int bufferSize = 8;
			if (string.IsNullOrEmpty(tag) == false)
			{
				bufferSize += Math.Min(32, tag.Length);
			}
			byte[] buffer = new byte[bufferSize];

			byte[] timeArray = BitConverter.GetBytes(time);
			if (BitConverter.IsLittleEndian)
			{
				Array.Reverse(timeArray);
			}
			Array.Copy(timeArray, buffer, 8);
			if (string.IsNullOrEmpty(tag) == false)
			{
				Array.Copy(Encoding.UTF8.GetBytes(tag), 0, buffer, 8, bufferSize - 8);
			}

			HMACSHA1 hmac = new HMACSHA1(b64secret, true);
			byte[] hash = hmac.ComputeHash(buffer);

			return Convert.ToBase64String(hash, Base64FormattingOptions.None);
		}

		/// <summary>
		/// Create a random Device ID string for Enrolling
		/// </summary>
		/// <returns>Random string</returns>
		private static string BuildRandomId()
		{
			return "android:" + Guid.NewGuid().ToString();
		}

#if DEBUG
		/// <summary>
		/// Log an exception from a Request
		/// </summary>
		/// <param name="method">Get or POST</param>
		/// <param name="url">Request URL</param>
		/// <param name="cookies">cookie container</param>
		/// <param name="request">Request data</param>
		/// <param name="ex">Thrown exception</param>
		private static void LogRequest(string method, string url, CookieContainer cookies, NameValueCollection request, Exception ex)
		{
			LogRequest(method, url, cookies, request, ex.Message + Environment.NewLine + ex.StackTrace);
		}

		/// <summary>
		/// Log a non 200 Request response
		/// </summary>
		/// <param name="method">Get or POST</param>
		/// <param name="url">Request URL</param>
		/// <param name="cookies">cookie container</param>
		/// <param name="request">Request data</param>
		/// <param name="response">HttpWebResponse object</param>
		private static void LogRequest(string method, string url, CookieContainer cookies, NameValueCollection request, HttpWebResponse response)
		{
			LogRequest(method, url, cookies, request, response.StatusCode.ToString() + " " + response.StatusDescription);
		}

		/// <summary>
		/// Log a normal response
		/// </summary>
		/// <param name="method">Get or POST</param>
		/// <param name="url">Request URL</param>
		/// <param name="cookies">cookie container</param>
		/// <param name="request">Request data</param>
		/// <param name="response">response body</param>
		private static void LogRequest(string method, string url, CookieContainer cookies, NameValueCollection request, string response)
		{
			StringBuilder data = new StringBuilder();
			if (cookies != null)
			{
				foreach (Cookie cookie in cookies.GetCookies(new Uri(url)))
				{
					if (data.Length == 0)
					{
						data.Append("Cookies:");
					}
					else
					{
						data.Append("&");
					}
					data.Append(cookie.Name + "=" + cookie.Value);
				}
				data.Append(" ");
			}

			if (request != null)
			{
				foreach (var key in request.AllKeys)
				{
					if (data.Length == 0)
					{
						data.Append("Req:");
					}
					else
					{
						data.Append("&");
					}
					data.Append(key + "=" + request[key]);
				}
				data.Append(" ");
			}
			
			string message = string.Format(@"{0} {1} {2} {3} {4}", DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss"), method, url, data.ToString(), (response != null ? response.Replace("\n", "\\n").Replace("\r", "") : string.Empty));

			System.Diagnostics.Trace.TraceWarning(message);
		}
#endif

		/// <summary>
		/// Our custom exception for the internal Http Request
		/// </summary>
		class InvalidRequestException : ApplicationException
		{
			public InvalidRequestException(string msg = null, Exception ex = null) : base(msg, ex) { }
		}

	}


}
