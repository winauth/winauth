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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Runtime.Serialization;

namespace WinAuth
{
	/// <summary>
	/// SteamClient for logging and getting/accepting/rejecting trade confirmations
	/// </summary>
	public class SteamClient
	{
		/// <summary>
		/// URLs for all mobile services
		/// </summary>
		private const string COMMUNITY_BASE = "https://steamcommunity.com";

		/// <summary>
		/// Default mobile user agent
		/// </summary>
		private const string USERAGENT = "Mozilla/5.0 (Linux; U; Android 4.1.1; en-us; Google Nexus 4 - 4.1.1 - API 16 - 768x1280 Build/JRO03S) AppleWebKit/534.30 (KHTML, like Gecko) Version/4.0 Mobile Safari/534.30";

		/// <summary>
		/// Regular expressions for trade confirmations
		/// </summary>
		private static Regex _tradesRegex = new Regex("\"mobileconf_list_entry\"(.*?)>(.*?)\"mobileconf_list_entry_sep\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private static Regex _tradeConfidRegex = new Regex(@"data-confid\s*=\s*""([^""]+)""", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private static Regex _tradeKeyRegex = new Regex(@"data-key\s*=\s*""([^""]+)""", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private static Regex _tradePlayerRegex = new Regex("\"mobileconf_list_entry_icon\"(.*?)src=\"([^\"]+)\"", RegexOptions.Singleline | RegexOptions.IgnoreCase);
		private static Regex _tradeDetailsRegex = new Regex(@"""mobileconf_list_entry_description"".*?<div>([^<]*)</div>\s*<div>([^<]*)</div>\s*<div>([^<]*)</div>\s*</div>", RegexOptions.Singleline | RegexOptions.IgnoreCase);

		/// <summary>
		/// A class for a single confirmation
		/// </summary>
		public class Confirmation
		{
			public string Id;
			public string Key;
			public bool Offline;
			public string Image;
			public string Details;
			public string Traded;
			public string When;
		}

		/// <summary>
		/// Session state to remember logins
		/// </summary>
		public class SteamSession
		{
			/// <summary>
			/// USer's steam ID
			/// </summary>
			public string SteamId;

			/// <summary>
			/// Current cookies
			/// </summary>
			public CookieContainer Cookies;

			/// <summary>
			/// Authorization token
			/// </summary>
			public string OAuthToken;

			/// <summary>
			/// Create Session instance
			/// </summary>
			public SteamSession()
			{
				Clear();
			}

			/// <summary>
			/// Create session instance from existing json data
			/// </summary>
			/// <param name="json">json session data</param>
			public SteamSession(string json) : this()
			{
				if (string.IsNullOrEmpty(json) == false)
				{
					this.FromJson(json);
				}
			}

			/// <summary>
			/// Clear the session
			/// </summary>
			public void Clear()
			{
				this.OAuthToken = null;
				this.Cookies = new CookieContainer();
			}

			/// <summary>
			/// Get session data that can be saved and imported
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return "{\"steamid\":\"" + this.SteamId + "\","
					+ "\"cookies\":\"" + this.Cookies.GetCookieHeader(new Uri(COMMUNITY_BASE + "/")) + "\","
					+ "\"oauthtoken\":\"" + this.OAuthToken + "\"}";
			}

			/// <summary>
			/// Convert json data into session 
			/// </summary>
			/// <param name="json"></param>
			private void FromJson(string json)
			{
				var tokens = JObject.Parse(json);
				var token = tokens.SelectToken("steamid");
				if (token != null)
				{
					this.SteamId = token.Value<string>();
				}
				token = tokens.SelectToken("cookies");
				if (token != null)
				{
					this.Cookies = new CookieContainer();
					
					var match = Regex.Match(token.Value<string>(), @"([^=]+)=([^;]*);?", RegexOptions.Singleline);
					while (match.Success == true)
					{
						this.Cookies.Add(new Cookie(match.Groups[1].Value.Trim(), match.Groups[2].Value.Trim(), "/", ".steamcommunity.com"));
						match = match.NextMatch();
					}

				}
				token = tokens.SelectToken("oauthtoken");
				if (token != null)
				{
					this.OAuthToken = token.Value<string>();
				}
			}
		}

		/// <summary>
		/// Login state fields
		/// </summary>
		public bool InvalidLogin;
		public bool RequiresCaptcha;
		public string CaptchaId;
		public string CaptchaUrl;
		public bool Requires2FA;
		public bool RequiresEmailAuth;
		public string EmailDomain;
		public string Error;

		/// <summary>
		/// Current session
		/// </summary>
		public SteamSession Session;

		/// <summary>
		/// Current authenticator
		/// </summary>
		private SteamAuthenticator Authenticator;

		/// <summary>
		/// Saved Html from GetConfirmations used as template for GetDetails
		/// </summary>
		private string ConfirmationsHtml;

		/// <summary>
		/// Query string from GetConfirmations used in GetDetails
		/// </summary>
		private string ConfirmationsQuery;

		/// <summary>
		/// Create a new SteamClient
		/// </summary>
		public SteamClient(SteamAuthenticator auth, string session = null)
		{
			this.Authenticator = auth;
			this.Session = new SteamSession(session);
		}

		/// <summary>
		/// Clear the client state
		/// </summary>
		public void Clear()
		{
			this.InvalidLogin = false;
			this.RequiresCaptcha = false;
			this.CaptchaId = null;
			this.CaptchaUrl = null;
			this.RequiresEmailAuth = false;
			this.EmailDomain = null;
			this.Requires2FA = false;
			this.Error = null;

			this.Session.Clear();
		}

		/// <summary>
		/// Check if user is logged in
		/// </summary>
		/// <returns></returns>
		public bool IsLoggedIn()
		{
			return (this.Session != null && string.IsNullOrEmpty(this.Session.OAuthToken) == false);
		}

		/// <summary>
		/// Login to Steam using credentials and optional captcha
		/// </summary>
		/// <param name="username"></param>
		/// <param name="password"></param>
		/// <param name="captchaId"></param>
		/// <param name="captchaText"></param>
		/// <returns>true if successful</returns>
		public bool Login(string username, string password, string captchaId = null, string captchaText = null)
		{
			// clear error
			this.Error = null;

			var data = new NameValueCollection();
			string response;

			if (this.IsLoggedIn() == false)
			{
				// get session
				if (this.Session.Cookies.Count == 0)
				{
					this.Session.Cookies.Add(new Cookie("mobileClientVersion", "3067969+%282.1.3%29", "/", ".steamcommunity.com"));
					this.Session.Cookies.Add(new Cookie("mobileClient", "android", "/", ".steamcommunity.com"));
					this.Session.Cookies.Add(new Cookie("steamid", "", "/", ".steamcommunity.com"));
					this.Session.Cookies.Add(new Cookie("steamLogin", "", "/", ".steamcommunity.com"));
					this.Session.Cookies.Add(new Cookie("Steam_Language", "english", "/", ".steamcommunity.com"));
					this.Session.Cookies.Add(new Cookie("dob", "", "/", ".steamcommunity.com"));

					NameValueCollection headers = new NameValueCollection();
					headers.Add("X-Requested-With", "com.valvesoftware.android.steam.community");

					response = GetString("https://steamcommunity.com/login?oauth_client_id=DE45CD61&oauth_scope=read_profile%20write_profile%20read_client%20write_client", "GET", null, headers);
				}

				// get the user's RSA key
				data.Add("username", username);
				response = GetString(COMMUNITY_BASE + "/login/getrsakey", "POST", data);
				var rsaresponse = JObject.Parse(response);
				if (rsaresponse.SelectToken("success").Value<bool>() != true)
				{
					this.InvalidLogin = true;
					this.Error = "Unknown username";
					return false;
				}

				// encrypt password with RSA key
				RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
				string encryptedPassword64;
				using (var rsa = new RSACryptoServiceProvider())
				{
					var passwordBytes = Encoding.ASCII.GetBytes(password);
					var p = rsa.ExportParameters(false);
					p.Exponent = StringToByteArray(rsaresponse.SelectToken("publickey_exp").Value<string>());
					p.Modulus = StringToByteArray(rsaresponse.SelectToken("publickey_mod").Value<string>());
					rsa.ImportParameters(p);
					byte[] encryptedPassword = rsa.Encrypt(passwordBytes, false);
					encryptedPassword64 = Convert.ToBase64String(encryptedPassword);
				}

				// login request
				data = new NameValueCollection();
				data.Add("password", encryptedPassword64);
				data.Add("username", username);
				data.Add("twofactorcode", this.Authenticator.CurrentCode);
				//data.Add("emailauth", string.Empty);
				data.Add("loginfriendlyname", "#login_emailauth_friendlyname_mobile");
				data.Add("captchagid", (string.IsNullOrEmpty(captchaId) == false ? captchaId : "-1"));
				data.Add("captcha_text", (string.IsNullOrEmpty(captchaText) == false ? captchaText : "enter above characters"));
				//data.Add("emailsteamid", (string.IsNullOrEmpty(emailcode) == false ? this.SteamId ?? string.Empty : string.Empty));
				data.Add("rsatimestamp", rsaresponse.SelectToken("timestamp").Value<string>());
				data.Add("remember_login", "false");
				data.Add("oauth_client_id", "DE45CD61");
				data.Add("oauth_scope", "read_profile write_profile read_client write_client");
				data.Add("donotache", new DateTime().ToUniversalTime().Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)).TotalMilliseconds.ToString());
				response = GetString(COMMUNITY_BASE + "/login/dologin/", "POST", data);
				Dictionary<string, object> loginresponse = JsonConvert.DeserializeObject<Dictionary<string, object>>(response);

				if (loginresponse.ContainsKey("emailsteamid") == true)
				{
					this.Session.SteamId = loginresponse["emailsteamid"] as string;
				}

				this.InvalidLogin = false;
				this.RequiresCaptcha = false;
				this.CaptchaId = null;
				this.CaptchaUrl = null;
				this.RequiresEmailAuth = false;
				this.EmailDomain = null;
				this.Requires2FA = false;

				if (loginresponse.ContainsKey("login_complete") == false || (bool)loginresponse["login_complete"] == false || loginresponse.ContainsKey("oauth") == false)
				{
					this.InvalidLogin = true;

					// require captcha
					if (loginresponse.ContainsKey("captcha_needed") == true && (bool)loginresponse["captcha_needed"] == true)
					{
						this.RequiresCaptcha = true;
						this.CaptchaId = (string)loginresponse["captcha_gid"];
						this.CaptchaUrl = COMMUNITY_BASE + "/public/captcha.php?gid=" + this.CaptchaId;
					}

					// require email auth
					if (loginresponse.ContainsKey("emailauth_needed") == true && (bool)loginresponse["emailauth_needed"] == true)
					{
						if (loginresponse.ContainsKey("emaildomain") == true)
						{
							var emaildomain = (string)loginresponse["emaildomain"];
							if (string.IsNullOrEmpty(emaildomain) == false)
							{
								this.EmailDomain = emaildomain;
							}
						}
						this.RequiresEmailAuth = true;
					}

					// require email auth
					if (loginresponse.ContainsKey("requires_twofactor") == true && (bool)loginresponse["requires_twofactor"] == true)
					{
						this.Requires2FA = true;
					}

					if (loginresponse.ContainsKey("message") == true)
					{
						this.Error = (string)loginresponse["message"];
					}

					return false;
				}

				// get the OAuth token
				string oauth = (string)loginresponse["oauth"];
				var oauthjson = JObject.Parse(oauth);
				this.Session.OAuthToken = oauthjson.SelectToken("oauth_token").Value<string>();
				if (oauthjson.SelectToken("steamid") != null)
				{
					this.Session.SteamId = oauthjson.SelectToken("steamid").Value<string>();
				}
			}

			return true;
		}

		/// <summary>
		/// Get the current trade Confirmations
		/// </summary>
		/// <returns>list of Confirmation objects</returns>
		public List<Confirmation> GetConfirmations()
		{
			long servertime = (SteamAuthenticator.CurrentTime + this.Authenticator.ServerTimeDiff) / 1000L;

			var jids = JObject.Parse(this.Authenticator.SteamData).SelectToken("identity_secret");
			string ids = (jids != null ? jids.Value<string>() : string.Empty);

			var timehash = CreateTimeHash(servertime, "conf", ids);

			var data = new NameValueCollection();
			data.Add("p", this.Authenticator.DeviceId);
			data.Add("a", this.Session.SteamId);
			data.Add("k", timehash);
			data.Add("t", servertime.ToString());
			data.Add("m", "android");
			data.Add("tag", "conf");

			string html = GetString(COMMUNITY_BASE + "/mobileconf/conf", "GET", data);

			// save last html for confirmations details
			ConfirmationsHtml = html;
			ConfirmationsQuery = string.Join("&", Array.ConvertAll(data.AllKeys, key => String.Format("{0}={1}", HttpUtility.UrlEncode(key), HttpUtility.UrlEncode(data[key]))));

			List<Confirmation> trades = new List<Confirmation>();

			// extract the trades
			Match match = _tradesRegex.Match(html);
			while (match.Success)
			{
				var tradeIds = match.Groups[1].Value;

				var trade = new Confirmation();

				var innerMatch = _tradeConfidRegex.Match(tradeIds);
				if (innerMatch.Success)
				{
					trade.Id = innerMatch.Groups[1].Value;
				}
				innerMatch = _tradeKeyRegex.Match(tradeIds);
				if (innerMatch.Success)
				{
					trade.Key = innerMatch.Groups[1].Value;
				}

				var traded = match.Groups[2].Value;

				innerMatch = _tradePlayerRegex.Match(traded);
				if (innerMatch.Success)
				{
					if (innerMatch.Groups[1].Value.IndexOf("offline") != -1)
					{
						trade.Offline = true;
					}
					trade.Image = innerMatch.Groups[2].Value;
				}

				innerMatch = _tradeDetailsRegex.Match(traded);
				if (innerMatch.Success)
				{
					trade.Details = innerMatch.Groups[1].Value;
					trade.Traded = innerMatch.Groups[2].Value;
					trade.When = innerMatch.Groups[3].Value;
				}

				trades.Add(trade);

				match = match.NextMatch();
			}

			return trades;
		}

		/// <summary>
		/// Get details for an individual Confirmation
		/// </summary>
		/// <param name="trade">trade Confirmation</param>
		/// <returns>html string of details</returns>
		public string GetConfirmationDetails(Confirmation trade)
		{
			// build details URL
			string url = COMMUNITY_BASE + "/mobileconf/details/" + trade.Id + "?" + ConfirmationsQuery;

			string response = this.GetString(url);
			if (response.IndexOf("success") == -1)
			{
				throw new InvalidSteamRequestException("Invalid request from steam: " + response);
			}
			if (JObject.Parse(response).SelectToken("success").Value<bool>() == true)
			{
				string html = JObject.Parse(response).SelectToken("html").Value<string>();

				Regex detailsRegex = new Regex(@"(.*<body[^>]*>\s*<div\s+class=""[^""]+"">).*(</div>.*?</body>\s*</html>)", RegexOptions.Singleline | RegexOptions.IgnoreCase);
				var match = detailsRegex.Match(this.ConfirmationsHtml);
				if (match.Success == true)
				{
					return match.Groups[1].Value + html + match.Groups[2].Value;
				}
			}

			return "<html><head></head><body><p>Cannot load trade confirmation details</p></body></html>";
		}

		/// <summary>
		/// Confirm or reject a specific trade confirmation
		/// </summary>
		/// <param name="id">id of trade</param>
		/// <param name="key">key for trade</param>
		/// <param name="accept">true to accept, false to reject</param>
		/// <returns>true if successful</returns>
		public bool ConfirmTrade(string id, string key, bool accept)
		{
			if (string.IsNullOrEmpty(this.Session.OAuthToken) == true)
			{
				return false;
			}

			long servertime = (SteamAuthenticator.CurrentTime + this.Authenticator.ServerTimeDiff) / 1000L;

			var jids = JObject.Parse(this.Authenticator.SteamData).SelectToken("identity_secret");
			string ids = (jids != null ? jids.Value<string>() : string.Empty);
			var timehash = CreateTimeHash(servertime, "conf", ids);

			var data = new NameValueCollection();
			data.Add("op", accept ? "allow" : "cancel");
			data.Add("p", this.Authenticator.DeviceId);
			data.Add("a", this.Session.SteamId);
			data.Add("k", timehash);
			data.Add("t", servertime.ToString());
			data.Add("m", "android");
			data.Add("tag", "conf");
			//
			data.Add("cid", id);
			data.Add("ck", key);

			string response = GetString(COMMUNITY_BASE + "/mobileconf/ajaxop", "GET", data);
			if (string.IsNullOrEmpty(response) == true)
			{
				this.Error = "Blank response";
				return false;
			}

			var success = JObject.Parse(response).SelectToken("success");
			return (success != null && success.Value<bool>() == true);
		}

		/// <summary>
		/// Create the hash needed for the confirmations query string
		/// </summary>
		/// <param name="time">current time</param>
		/// <param name="tag">tag</param>
		/// <param name="secret">identity secret</param>
		/// <returns>hash string</returns>
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

		#region Web Request

		/// <summary>
		/// Get binary data web request
		/// </summary>
		/// <param name="url">url</param>
		/// <param name="method">GET or POST</param>
		/// <param name="formdata">optional form data</param>
		/// <param name="headers">optional headers</param>
		/// <returns>array of returned data</returns>
		public byte[] GetData(string url, string method = null, NameValueCollection formdata = null, NameValueCollection headers = null)
		{
			return Request(url, method ?? "GET", formdata, headers);
		}

		/// <summary>
		/// Get string from web request
		/// </summary>
		/// <param name="url">url</param>
		/// <param name="method">GET or POST</param>
		/// <param name="formdata">optional form data</param>
		/// <param name="headers">optional headers</param>
		/// <returns>string of returned data</returns>
		public string GetString(string url, string method = null, NameValueCollection formdata = null, NameValueCollection headers = null)
		{
			byte[] data = Request(url, method ?? "GET", formdata, headers);
			if (data == null || data.Length == 0)
			{
				return string.Empty;
			}
			else
			{
				return Encoding.UTF8.GetString(data);
			}
		}

		/// <summary>
		/// Make a request to Steam URL
		/// </summary>
		/// <param name="url">url</param>
		/// <param name="method">GET or POST</param>
		/// <param name="formdata">optional form data</param>
		/// <param name="headers">optional headers</param>
		/// <returns>returned data</returns>
		protected byte[] Request(string url, string method, NameValueCollection data, NameValueCollection headers)
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
			request.UserAgent = USERAGENT;
			request.AutomaticDecompression = DecompressionMethods.Deflate | DecompressionMethods.GZip;
			request.Referer = COMMUNITY_BASE;
			if (headers != null)
			{
				request.Headers.Add(headers);
			}

			request.CookieContainer = this.Session.Cookies;

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
						throw new InvalidSteamRequestException(string.Format("{0}: {1}", (int)response.StatusCode, response.StatusDescription));
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
				}
			}
			catch (Exception ex)
			{
				throw new InvalidSteamRequestException(ex.Message, ex);
			}
		}

		#endregion

		/// <summary>
		/// Convert a hex string into a byte array. E.g. "001f406a" -> byte[] {0x00, 0x1f, 0x40, 0x6a}
		/// </summary>
		/// <param name="hex">hex string to convert</param>
		/// <returns>byte[] of hex string</returns>
		private static byte[] StringToByteArray(string hex)
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
		private static string ByteArrayToString(byte[] bytes)
		{
			// Use BitConverter, but it sticks dashes in the string
			return BitConverter.ToString(bytes).Replace("-", string.Empty);
		}

		/// <summary>
		/// Our custom exception for the internal Http Request
		/// </summary>
		public class InvalidSteamRequestException : ApplicationException
		{
			public InvalidSteamRequestException(string msg = null, Exception ex = null) : base(msg, ex) { }
		}

	}


}
