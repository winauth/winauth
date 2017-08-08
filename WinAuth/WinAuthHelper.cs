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
using System.Collections.Specialized;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Security.Permissions;
using System.Security;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.XPath;
using System.Web;
using System.Windows;
using System.Windows.Forms;

using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Engines;
using Org.BouncyCastle.Crypto.Macs;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Paddings;
using Org.BouncyCastle.Crypto.Digests;
using Org.BouncyCastle.Crypto.Generators;
using Org.BouncyCastle.Bcpg;
using Org.BouncyCastle.Bcpg.OpenPgp;

using ICSharpCode.SharpZipLib.Core;
using ICSharpCode.SharpZipLib.Zip;

using Microsoft.Win32;

using WinAuth.Resources;

namespace WinAuth
{
  /// <summary>
  /// Class proving helper functions to save data for application
  /// </summary>
  class WinAuthHelper
  {
    /// <summary>
    /// Registry key for application
    /// </summary>
    public const string WINAUTHREGKEY = @"Software\WinAuth3";

		/// <summary>
		/// Registry key for application
		/// </summary>
		private const string WINAUTH2REGKEY = @"Software\WinAuth";

		/// <summary>
    /// Registry data name for last loaded file
    /// </summary>
    private const string WINAUTHREGKEY_LASTFILE = @"File{0}";

		/// <summary>
		/// Registry data name for last good
		/// </summary>
		private const string WINAUTHREGKEY_BACKUP = @"Software\WinAuth3\Backup";

		/// <summary>
		/// Encrpyted config backup
		/// </summary>
		private const string WINAUTHREGKEY_CONFIGBACKUP = @"Software\WinAuth3\Backup\Config";

		/// <summary>
		/// Registry data name for errors
		/// </summary>
		private const string WINAUTHREGKEY_ERROR = @"Software\WinAuth3\Error";

    /// <summary>
    /// Registry key for starting with windows
    /// </summary>
    private const string RUNKEY = @"Software\Microsoft\Windows\CurrentVersion\Run";

    /// <summary>
    /// Name of application config file for 1.3
    /// </summary>
    public const string CONFIG_FILE_NAME_1_3 = "winauth.xml";

    /// <summary>
    /// Name of default authenticator file
    /// </summary>
    public const string DEFAULT_AUTHENTICATOR_FILE_NAME = "winauth.xml";

    /// <summary>
    /// The WinAuth PGP public key
    /// </summary>
    public const string WINAUTH_PGP_PUBLICKEY =
      @"-----BEGIN PGP PUBLIC KEY BLOCK-----
			Version: BCPG C# v1.7.4114.6375
			
			mQEMBFA8sxQBCAC5EWjbGHDgyo4e9rcwse1mWbCOyeTwGZH2malJreF2v81KwBZa
			eCAPX6cP6EWJPlMOgkJpBQOgh+AezkYEidrW4+NXCGv+Z03U1YBc7e/nYnABZrJx
			XsqWVyM3d3iLSpKsMfk2OAIAIvoCvzcdx0ljm2IXGKRHGnc0nU7hSFXh5S/sJErN
			Cgrll6lD2CPNIPuUiMSWptgO1RAjerk0rwLh1DSChicPMJZfxJWn7JD1VVQLmAon
			EJ4x0MUIbff7ZmEna4O2rF9mrCjwfANkcz8N6WFp3PrfhxArXkvOBPYF9iEigFRS
			QVt6XAF6sjGhSYxZRaRj0tE4PyajE/HfNk0DAAkBAbQbV2luQXV0aCA8d2luYXV0
			aEBnbWFpbC5jb20+iQE0BBABAgASBQJQPRWEApsPAhYBAhUCAgsDABYJEJ3DDyNp
			qwwqApsPAhYBAhUCAgsDqb8IAKJRlRu5ne2BuHrMlKW/BI6I/hpkGQ8BzmO7LatM
			YYj//XKkbQ2BSvbDNI1al5HSo1iseokIZqD07iMwsp9GvLXSOVCROK9HYJ4dHsdP
			l68KgNDWu8ZDhPRGerf4+pn1jRfXW4NdFT8W1TX3RArpdVSd5Q2tV2tZrANErBYa
			UTDodsNKwikcgk89a2NI+Lh17lFGCFdAdZ07gRwu6cOm4SqP2TjWjDreXqlE9fHd
			0dwmYeS1QlGYK3ETNS1KvVTNaKdht231jGwlxy09Rxtx1EBLqFNsc+BW5rjYEPN2
			EAlelUJsVidUjZNB1ySm9uW8xurSEXWPZxWITl+LYmgwtn0=
			=dvwu
			-----END PGP PUBLIC KEY BLOCK-----";


    /// <summary>
    /// Load the authenticator and configuration settings
    /// </summary>
    /// <param name="form">parent winform</param>
    /// <param name="configFile">name of configfile or null for auto</param>
    /// <param name="password">optional supplied password or null to prompt if necessatu</param>
    /// <returns>new WinAuthConfig settings</returns>
    public static WinAuthConfig LoadConfig(Form form, string configFile, string password = null)
    {
      WinAuthConfig config = new WinAuthConfig();
			if (string.IsNullOrEmpty(password) == false)
			{
				config.Password = password;
			}

      if (string.IsNullOrEmpty(configFile) == true)
      {
        // check for file in current directory
        configFile = Path.Combine(Environment.CurrentDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
					if (File.Exists(configFile) == false)
					{
						configFile = null;
					}
				}
      if (string.IsNullOrEmpty(configFile) == true)
      {
        // check for file in exe directory
        configFile = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), DEFAULT_AUTHENTICATOR_FILE_NAME);
					if (File.Exists(configFile) == false)
					{
						configFile = null;
					}
				}
      if (string.IsNullOrEmpty(configFile) == true)
      {
        // do we have a file specific in the registry?
        string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuthMain.APPLICATION_NAME);
        // check for default authenticator
        configFile = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
					// if no config file, just return a blank config
					if (File.Exists(configFile) == false)
					{
						return config;
					}
				}

      // if no config file when one was specified; report an error
      if (File.Exists(configFile) == false)
      {
        //MessageBox.Show(form,
        // strings.CannotFindConfigurationFile + ": " + configFile,
        //  form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
        // return config;
				throw new ApplicationException(strings.CannotFindConfigurationFile + ": " + configFile);
      }

			// check if readonly
			FileInfo fi = new FileInfo(configFile);
			if (fi.Exists && fi.IsReadOnly)
			{
				config.IsReadOnly = true;
			}

			bool changed = false;
			try
			{
				var data = File.ReadAllBytes(configFile);
				if (data.Length == 0 || data[0] == 0)
				{
					// switch to backup
					if (File.Exists(configFile + ".bak") == true)
					{
						data = File.ReadAllBytes(configFile + ".bak");
						if (data.Length != 0 && data[0] != 0)
						{
							File.WriteAllBytes(configFile, data);
						}
					}
				}

				using (FileStream fs = new FileStream(configFile, FileMode.Open, FileAccess.Read))
				{
					XmlReader reader = XmlReader.Create(fs);
					changed = config.ReadXml(reader, password);
				}

				config.Filename = configFile;

				if (config.Version < WinAuthConfig.CURRENTVERSION)
				{
					// set new created values
					foreach (WinAuthAuthenticator wa in config)
					{
						wa.Created = fi.CreationTime;
					}

					config.Upgraded = true;
				}

				if (changed == true && config.IsReadOnly == false)
				{
					SaveConfig(config);
				}
			}
			catch (EncryptedSecretDataException )
			{
				// we require a password
				throw;
			}
			catch (BadPasswordException)
			{
				// we require a password
				throw;
			}
			catch (Exception )
			{
				throw;
			}

			SaveToRegistry(config);

      return config;
    }

		/// <summary>
		/// Return any 2.x authenticator entry in the registry
		/// </summary>
		/// <returns></returns>
		public static string GetLastV2Config()
		{
			// check for a v2 last file entry
			try
			{
				using (RegistryKey key = Registry.CurrentUser.OpenSubKey(WINAUTH2REGKEY, false))
				{
					string lastfile;
					if (key != null
						&& (lastfile = key.GetValue(string.Format(CultureInfo.InvariantCulture, WINAUTHREGKEY_LASTFILE, 1), null) as string) != null
						&& File.Exists(lastfile) == true)
					{
						return lastfile;
					}
				}
			}
			catch (System.Security.SecurityException) { }

			return null;
		}

    /// <summary>
    /// Save the authenticator
    /// </summary>
    /// <param name="configFile">filename to save to</param>
    /// <param name="config">current settings to save</param>
    public static void SaveConfig(WinAuthConfig config)
    {
      // create the xml
      XmlWriterSettings settings = new XmlWriterSettings();
      settings.Indent = true;
      settings.Encoding = Encoding.UTF8;

      // Issue 41 (http://code.google.com/p/winauth/issues/detail?id=41): saving may crash leaving file corrupt, so write into memory stream first before an atomic file write
      using (MemoryStream ms = new MemoryStream())
      {
        // save config into memory
        using (XmlWriter writer = XmlWriter.Create(ms, settings))
        {
          config.WriteXmlString(writer);
        }

				// if no config file yet, use default
				if (string.IsNullOrEmpty(config.Filename) == true)
				{
					string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuthMain.APPLICATION_NAME);
					Directory.CreateDirectory(configDirectory);
					config.Filename = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
				}

				FileInfo fi = new FileInfo(config.Filename);
				if (!fi.Exists || !fi.IsReadOnly)
				{
					// write memory stream to file
					try
					{
						byte[] data = ms.ToArray();

						// getting instance of zerod files, so do some sanity checks
						if (data.Length == 0 || data[0] == 0)
						{
							throw new ApplicationException("Zero data when saving config");
						}

						var tempfile = config.Filename + ".tmp";

						File.WriteAllBytes(tempfile, data);

						// read it back
						var verify = File.ReadAllBytes(tempfile);
						if (verify.Length != data.Length || verify.SequenceEqual(data) == false)
						{
							throw new ApplicationException("Save config doesn't compare with memory: " + Convert.ToBase64String(data));
						}

						// move it to old file
						File.Delete(config.Filename + ".bak");
						File.Move(config.Filename, config.Filename + ".bak");
						File.Move(tempfile, config.Filename);
					}
					catch (UnauthorizedAccessException )
					{
						// fail silently if read only
						if (fi.IsReadOnly)
						{
							config.IsReadOnly = true;
							return;
						}

						throw;
					}
				}
      }
    }

    /// <summary>
    /// Save a PGP encrypted version of the config into the registry for recovery
		/// 
		/// Issue#133: this just compounds each time we load, and is really pointless so we are removing it
		/// but in the meantime we have to clear it out
		/// </summary>
    /// <param name="config"></param>
		private static void SaveToRegistry(WinAuthConfig config)
    {
			config.WriteSetting(WINAUTHREGKEY_CONFIGBACKUP, null);
    }

		/// <summary>
		/// Save a PGP encrypted version of an authenticator into the registry for recovery
		/// </summary>
		/// <param name="wa">WinAuthAuthenticator instance</param>
		public static void SaveToRegistry(WinAuthConfig config, WinAuthAuthenticator wa)
		{
			if (config == null || wa == null || wa.AuthenticatorData == null)
			{
				return;
			}

			using (SHA256 sha = SHA256.Create())
			{
				// get a hash based on the authenticator key
				string authkey = Convert.ToBase64String(sha.ComputeHash(Encoding.UTF8.GetBytes(wa.AuthenticatorData.SecretData)));

				// save the PGP encrypted key
				using (EncodedStringWriter sw = new EncodedStringWriter(Encoding.UTF8))
				{
					XmlWriterSettings xmlsettings = new XmlWriterSettings();
					xmlsettings.Indent = true;
					using (XmlWriter xw = XmlWriter.Create(sw, xmlsettings))
					{
						xw.WriteStartElement("WinAuth");
						xw.WriteAttributeString("version", System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString(2));
						wa.WriteXmlString(xw);
						xw.WriteEndElement();
					}

					string pgpkey = string.IsNullOrEmpty(config.PGPKey) == false ? config.PGPKey : WinAuthHelper.WINAUTH_PGP_PUBLICKEY;
					config.WriteSetting(WINAUTHREGKEY_BACKUP + "\\" + authkey, PGPEncrypt(sw.ToString(), pgpkey));
				}
			}
		}

		/// <summary>
		/// Read the encrpyted backup registry entries to be sent within the diagnostics report
		/// </summary>
		public static string ReadBackupFromRegistry(WinAuthConfig config)
		{
			StringBuilder buffer = new StringBuilder();
			foreach (string name in config.ReadSettingKeys(WINAUTHREGKEY_BACKUP))
			{
				object val = ReadRegistryValue(name);
				if (val != null)
				{
					buffer.Append(name + "=" + Convert.ToString(val)).Append(Environment.NewLine);
				}
			}

			return buffer.ToString();
		}

		/// <summary>
    /// Set up winauth so it will start with Windows by adding entry into registry
    /// </summary>
    /// <param name="enabled">enable or disable start with windows</param>
    public static void SetStartWithWindows(bool enabled)
    {
      if (enabled == true)
      {
        // get path of exe and minimize flag
				WriteRegistryValue(RUNKEY + "\\" + WinAuthMain.APPLICATION_NAME, Application.ExecutablePath + " -min");
      }
      else
      {				
        DeleteRegistryKey(RUNKEY + "\\" + WinAuthMain.APPLICATION_NAME);
      }			
    }

		/// <summary>
		/// Import a file containing authenticators in the KeyUriFormat. The file might be plain text, encrypted zip or encrypted pgp.
		/// </summary>
		/// <param name="parent">parent Form</param>
		/// <param name="file">file name to import</param>
		/// <returns>list of imported authenticators</returns>
		public static List<WinAuthAuthenticator> ImportAuthenticators(Form parent, string file)
		{
			List<WinAuthAuthenticator> authenticators = new List<WinAuthAuthenticator>();

			string password = null;
			string pgpKey = null;

			StringBuilder lines = new StringBuilder();
			bool retry;
			do
			{
				retry = false;
				lines.Length = 0;

				// open the zip file
				if (string.Compare(Path.GetExtension(file), ".zip", true) == 0)
				{
					using (var fs = new FileStream(file, FileMode.Open, FileAccess.Read))
					{
						ZipFile zip = null;
						try
						{
							zip = new ZipFile(fs);
							if (string.IsNullOrEmpty(password) == false)
							{
								zip.Password = password;
							}

							byte[] buffer = new byte[4096];
							foreach (ZipEntry entry in zip)
							{
								if (entry.IsFile == false || string.Compare(Path.GetExtension(entry.Name), ".txt", true) != 0)
								{
									continue;
								}

								// read file out
								Stream zs = zip.GetInputStream(entry);
								using (var ms = new MemoryStream())
								{
									StreamUtils.Copy(zs, ms, buffer);

									// get as string and append
									ms.Seek(0, SeekOrigin.Begin);
									using (var sr = new StreamReader(ms))
									{
										lines.Append(sr.ReadToEnd()).Append(Environment.NewLine);
									}
								}
							}
						}
						catch (ZipException ex)
						{
							if (ex.Message.IndexOf("password") != -1)
							{
								// already have a password
								if (string.IsNullOrEmpty(password) == false)
								{
									WinAuthForm.ErrorDialog(parent, strings.InvalidPassword, ex.InnerException, MessageBoxButtons.OK);
								}

								// need password
								GetPasswordForm form = new GetPasswordForm();
								if (form.ShowDialog(parent) == DialogResult.Cancel)
								{
									return null;
								}
								password = form.Password;
								retry = true;
								continue;
							}

							throw;
						}
						finally
						{
							if (zip != null)
							{
								zip.IsStreamOwner = true;
								zip.Close();
							}
						}
					}
				}
				else if (string.Compare(Path.GetExtension(file), ".pgp", true) == 0)
				{
					string encoded = File.ReadAllText(file);
					if (string.IsNullOrEmpty(pgpKey) == true)
					{
						// need password
						GetPGPKeyForm form = new GetPGPKeyForm();
						if (form.ShowDialog(parent) == DialogResult.Cancel)
						{
							return null;
						}
						pgpKey = form.PGPKey;
						password = form.Password;
						retry = true;
						continue;
					}
					try
					{
						string line = PGPDecrypt(encoded, pgpKey, password);
						lines.Append(line);
					}
					catch (Exception ex)
					{
						WinAuthForm.ErrorDialog(parent, strings.InvalidPassword, ex.InnerException, MessageBoxButtons.OK);

						pgpKey = null;
						password = null;
						retry = true;
						continue;
					}
				}
				else // read a plain text file
				{
					lines.Append(File.ReadAllText(file));
				}
			} while (retry);

			int linenumber = 0;
			try
			{
				using (var sr = new StringReader(lines.ToString()))
				{
					string line;
					while ((line = sr.ReadLine()) != null)
					{
						linenumber++;

						// ignore blank lines or comments
						line = line.Trim();
						if (line.Length == 0 || line.IndexOf("#") == 0)
						{
							continue;
						}

						// parse and validate URI
						var uri = new Uri(line);

						// we only support "otpauth"
						if (uri.Scheme != "otpauth")
						{
							throw new ApplicationException("Import only supports otpauth://");
						}
						// we only support totp (not hotp)
						if (uri.Host != "totp" && uri.Host != "hotp")
						{
							throw new ApplicationException("Import only supports otpauth://totp/ or otpauth://hotp/");
						}

						// get the label and optional issuer
						string issuer = string.Empty;
						string label = (string.IsNullOrEmpty(uri.LocalPath) == false ? uri.LocalPath.Substring(1) : string.Empty); // skip past initial /
						int p = label.IndexOf(":");
						if (p != -1)
						{
							issuer = label.Substring(0, p);
							label = label.Substring(p + 1);
						}
						// + aren't decoded
						label = label.Replace("+", " ");

						var query = HttpUtility.ParseQueryString(uri.Query);
						string secret = query["secret"];
						if (string.IsNullOrEmpty(secret) == true)
						{
							throw new ApplicationException("Authenticator does not contain secret");
						}

						string counter = query["counter"];
						if (uri.Host == "hotp" && string.IsNullOrEmpty(counter) == true)
						{
							throw new ApplicationException("HOTP authenticator should have a counter");
						}

						WinAuthAuthenticator importedAuthenticator = new WinAuthAuthenticator();
						importedAuthenticator.AutoRefresh = false;
						//
						Authenticator auth;
						if (string.Compare(issuer, "BattleNet", true) == 0)
						{
							string serial = query["serial"];
							if (string.IsNullOrEmpty(serial) == true)
							{
								throw new ApplicationException("Battle.net Authenticator does not have a serial");
							}
							serial = serial.ToUpper();
							if (Regex.IsMatch(serial, @"^[A-Z]{2}-?[\d]{4}-?[\d]{4}-?[\d]{4}$") == false)
							{
								throw new ApplicationException("Invalid serial for Battle.net Authenticator");
							}
							auth = new BattleNetAuthenticator();
							//char[] decoded = Base32.getInstance().Decode(secret).Select(c => Convert.ToChar(c)).ToArray(); // this is hex string values
							//string hex = new string(decoded);
							//((BattleNetAuthenticator)auth).SecretKey = Authenticator.StringToByteArray(hex);

							((BattleNetAuthenticator)auth).SecretKey = Base32.getInstance().Decode(secret);

							((BattleNetAuthenticator)auth).Serial = serial;

							issuer = string.Empty;
						}
						else if (string.Compare(issuer, "Steam", true) == 0)
						{
							auth = new SteamAuthenticator();
							((SteamAuthenticator)auth).SecretKey = Base32.getInstance().Decode(secret);
							((SteamAuthenticator)auth).Serial = string.Empty;
							((SteamAuthenticator)auth).DeviceId = query["deviceid"] ?? string.Empty;
							((SteamAuthenticator)auth).SteamData = query["data"] ?? string.Empty;
							issuer = string.Empty;
						}
						else if (uri.Host == "hotp")
						{
							auth = new HOTPAuthenticator();
							((HOTPAuthenticator)auth).SecretKey = Base32.getInstance().Decode(secret);
							((HOTPAuthenticator)auth).Counter = int.Parse(counter);
							
							if (string.IsNullOrEmpty(issuer) == false)
							{
								auth.Issuer = issuer;
							}
						}
						else // if (string.Compare(issuer, "Google", true) == 0)
						{
							auth = new GoogleAuthenticator();
							((GoogleAuthenticator)auth).Enroll(secret);

							if (string.Compare(issuer, "Google", true) == 0)
							{
								issuer = string.Empty;
							}
							else if (string.IsNullOrEmpty(issuer) == false)
							{
								auth.Issuer = issuer;
							}
						}

						int period = 0;
						int.TryParse(query["period"], out period);
						if (period != 0)
						{
							auth.Period = period;
						}

						int digits = 0;
						int.TryParse(query["digits"], out digits);
						if (digits != 0)
						{
							auth.CodeDigits = digits;
						}

            Authenticator.HMACTypes hmactype;
            if (Enum.TryParse<Authenticator.HMACTypes>(query["algorithm"], true, out hmactype) == true)
            {
              auth.HMACType = hmactype;
            }
						//
						if (label.Length != 0)
						{
							importedAuthenticator.Name = (issuer.Length != 0 ? issuer + " (" + label + ")" : label);
						}
						else if (issuer.Length != 0)
						{
							importedAuthenticator.Name = issuer;
						}
						else
						{
							importedAuthenticator.Name = "Imported";
						}
						//
						importedAuthenticator.AuthenticatorData = auth;

						// set the icon
						string icon = query["icon"];
						if (string.IsNullOrEmpty(icon) == false)
						{
							if (icon.StartsWith("base64:") == true)
							{
								string b64 = Convert.ToBase64String(Base32.getInstance().Decode(icon.Substring(7)));
								importedAuthenticator.Skin = "base64:" + b64;
							}
							else
							{
								importedAuthenticator.Skin = icon + "Icon.png";
							}
						}

						// sync
						importedAuthenticator.Sync();

						authenticators.Add(importedAuthenticator);
					}
				}

				return authenticators;
			}
			catch (UriFormatException ex)
			{
				throw new ImportException(string.Format(strings.ImportInvalidUri, linenumber), ex);
			}
			catch (Exception ex)
			{
				throw new ImportException(string.Format(strings.ImportError, linenumber, ex.Message), ex);
			}
		}

		public static void ExportAuthenticators(Form form, IList<WinAuthAuthenticator> authenticators, string file, string password, string pgpKey)
		{
			// create file in memory
			using (var ms = new MemoryStream())
			{
				using (var sw = new StreamWriter(ms))
				{
					List<WinAuthAuthenticator> unprotected = new List<WinAuthAuthenticator>();
					foreach (var auth in authenticators)
					{
						// unprotect if necessary
						if (auth.AuthenticatorData.RequiresPassword == true)
						{
							// request the password
							UnprotectPasswordForm getPassForm = new UnprotectPasswordForm();
							getPassForm.Authenticator = auth;
							DialogResult result = getPassForm.ShowDialog(form);
							if (result == DialogResult.OK)
							{
								unprotected.Add(auth);
							}
							else
							{
								continue;
							}
						}

						string line = auth.ToUrl();
						sw.WriteLine(line);
					}

					// reprotect
					foreach (var auth in unprotected)
					{
						auth.AuthenticatorData.Protect();
					}

					// reset and write stream out to disk or as zip
					sw.Flush();
					ms.Seek(0, SeekOrigin.Begin);

					// reset and write stream out to disk or as zip
					if (string.Compare(Path.GetExtension(file), ".zip", true) == 0)
					{
						using (var zip = new ZipOutputStream(new FileStream(file, FileMode.Create, FileAccess.Write)))
						{
							if (string.IsNullOrEmpty(password) == false)
							{
								zip.Password = password;
							}

							zip.IsStreamOwner = true;

							ZipEntry entry = new ZipEntry(ZipEntry.CleanName(Path.GetFileNameWithoutExtension(file) + ".txt"));
							entry.DateTime = DateTime.Now;
							zip.UseZip64 = UseZip64.Off;

							zip.PutNextEntry(entry);

							byte[] buffer = new byte[4096];
							StreamUtils.Copy(ms, zip, buffer);

							zip.CloseEntry();
						}
					}
					else if (string.IsNullOrEmpty(pgpKey) == false)
					{
						using (var sr = new StreamReader(ms))
						{
							var plain = sr.ReadToEnd();
							string encoded = PGPEncrypt(plain, pgpKey);

							File.WriteAllText(file, encoded);
						}
					}
					else
					{
						using (var fs = new FileStream(file, FileMode.Create, FileAccess.Write))
						{
							byte[] buffer = new byte[4096];
							StreamUtils.Copy(ms, fs, buffer);
						}
					}
				}
			}
		}

		#region HttpUtility

		/// <summary>
		/// Our own version of HtmlEncode replacing HttpUtility.HtmlEncode so we can remove the System.Web reference
		/// which isn't available in the .Net client profile
		/// </summary>
		/// <param name="text">text to be encoded</param>
		/// <returns>encoded string</returns>
		public static string HtmlEncode(string text)
		{
			if (string.IsNullOrEmpty(text) == true)
			{
				return text;
			}

			StringBuilder sb = new StringBuilder(text.Length);

			int len = text.Length;
			for (int i = 0; i < len; i++)
			{
				switch (text[i])
				{

					case '<':
						sb.Append("&lt;");
						break;
					case '>':
						sb.Append("&gt;");
						break;
					case '"':
						sb.Append("&quot;");
						break;
					case '&':
						sb.Append("&amp;");
						break;
					default:
						if (text[i] > 159)
						{
							// decimal numeric entity
							sb.Append("&#");
							sb.Append(((int)text[i]).ToString(CultureInfo.InvariantCulture));
							sb.Append(";");
						}
						else
						{
							sb.Append(text[i]);
						}
						break;
				}
			}
			
			return sb.ToString();
		}

		/// <summary>
		/// Our own version of HttpUtility.ParseQueryString so we can remove the reference to System.Web
		/// which is not available in client profile.
		/// </summary>
		/// <param name="qs">string query string</param>
		/// <returns>collection of name value pairs</returns>
		public static NameValueCollection ParseQueryString(string qs)
		{
			NameValueCollection pairs = new NameValueCollection();

			// ignore blanks and remove initial "?"
			if (string.IsNullOrEmpty(qs) == true)
			{
				return pairs;
			}
			if (qs.StartsWith("?") == true)
			{
				qs = qs.Substring(1);
			}

			// get each a=b&... key-value pair
			foreach (string p in qs.Split('&'))
			{
				string[] keypair = p.Split('=');
				string key = keypair[0];
				string v = (keypair.Length >= 2 ? keypair[1] : null);
				if (string.IsNullOrEmpty(v) == false)
				{
					// decode (without using System.Web)
					string newv;
					while ((newv = Uri.UnescapeDataString(v)) != v)
					{
						v = newv;
					}
				}
				pairs.Add(key, v);
			}

			return pairs;
		}

		#endregion

		#region Registry Function

		/// <summary>
		/// Read a value from a registry key, e.g. Software\WinAuth3\BetValue. Return defaultValue
		/// if key does not exist or there is a security exception
		/// 
		/// The key name can conjtain the explicit root, e.g. "HKEY_LOCAL_MACHINE\Software..." otherwise
		/// HKEY_CURRENT_USER is assumed.
		/// </summary>
		/// <param name="keyname">full key name</param>
		/// <param name="defaultValue">default value</param>
		/// <returns>key value or default value</returns>
		public static object ReadRegistryValue(string keyname, object defaultValue = null)
		{
			RegistryKey basekey;
			List<string> keyparts = keyname.Split('\\').ToList();
			switch (keyparts[0])
			{
				case "HKEY_CLASSES_ROOT":
					basekey = Registry.ClassesRoot;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_CONFIG":
					basekey = Registry.CurrentConfig;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_USER":
					basekey = Registry.CurrentUser;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_LOCAL_MACHINE":
					basekey = Registry.LocalMachine;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_PERFORMANCE_DATA":
					basekey = Registry.PerformanceData;
					keyparts.RemoveAt(0);
					break;
				default:
					basekey = Registry.CurrentUser;
					break;
			}
			string subkey = string.Join("\\", keyparts.Take(keyparts.Count - 1).ToArray());
			string valuekey = keyparts[keyparts.Count - 1];

			try
			{
				using (RegistryKey key = basekey.OpenSubKey(subkey))
				{
					return (key != null ? key.GetValue(valuekey, defaultValue) : defaultValue);
				}
			}
			catch (System.Security.SecurityException)
			{
				return defaultValue;
			}
		}

		/// <summary>
		/// Get the names of all the child value keys for a given parent.
		/// </summary>
		/// <param name="keyname">name of parent key</param>
		/// <returns>string array of all child value names or empty array</returns>
		public static string[] ReadRegistryKeys(string keyname)
		{
			RegistryKey basekey;
			List<string> keyparts = keyname.Split('\\').ToList();
			switch (keyparts[0])
			{
				case "HKEY_CLASSES_ROOT":
					basekey = Registry.ClassesRoot;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_CONFIG":
					basekey = Registry.CurrentConfig;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_USER":
					basekey = Registry.CurrentUser;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_LOCAL_MACHINE":
					basekey = Registry.LocalMachine;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_PERFORMANCE_DATA":
					basekey = Registry.PerformanceData;
					keyparts.RemoveAt(0);
					break;
				default:
					basekey = Registry.CurrentUser;
					break;
			}
			string subkey = string.Join("\\", keyparts.ToArray());

			try
			{
				using (RegistryKey key = basekey.OpenSubKey(subkey))
				{
					if (key == null)
					{
						return new string[0];
					}

					// get all value names
					List<string> keys = key.GetValueNames().ToList();
					for (int i = 0; i < keys.Count; i++)
					{
						keys[i] = keyname + "\\" + keys[i];
					}

					// read any subkeys
					if (key.SubKeyCount != 0)
					{
						foreach (string subkeyname in key.GetSubKeyNames())
						{
							keys.AddRange(ReadRegistryKeys(keyname + "\\" + subkeyname));
						}
					}

					return keys.ToArray();
				}
			}
			catch (System.Security.SecurityException)
			{
				return new string[0];
			}
		}

		/// <summary>
		/// Write a value into a registry key value.
		/// </summary>
		/// <param name="keyname">full name of key</param>
		/// <param name="value">value to write</param>
		public static void WriteRegistryValue(string keyname, object value)
		{
			RegistryKey basekey;
			List<string> keyparts = keyname.Split('\\').ToList();
			switch (keyparts[0])
			{
				case "HKEY_CLASSES_ROOT":
					basekey = Registry.ClassesRoot;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_CONFIG":
					basekey = Registry.CurrentConfig;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_USER":
					basekey = Registry.CurrentUser;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_LOCAL_MACHINE":
					basekey = Registry.LocalMachine;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_PERFORMANCE_DATA":
					basekey = Registry.PerformanceData;
					keyparts.RemoveAt(0);
					break;
				default:
					basekey = Registry.CurrentUser;
					break;
			}
			string subkey = string.Join("\\", keyparts.Take(keyparts.Count - 1).ToArray());
			string valuekey = keyparts[keyparts.Count - 1];

			try
			{
				using (RegistryKey key = basekey.CreateSubKey(subkey))
				{
					key.SetValue(valuekey, value);
				}
			}
			catch (System.Security.SecurityException)
			{
				return;
			}
		}

		/// <summary>
		/// Delete a registry entry value or key. If it is deleted and there are no more sibling values or subkeys,
		/// the parent is also removed.
		/// </summary>
		/// <param name="keyname"></param>
		public static void DeleteRegistryKey(string keyname)
		{
			RegistryKey basekey;
			List<string> keyparts = keyname.Split('\\').ToList();
			switch (keyparts[0])
			{
				case "HKEY_CLASSES_ROOT":
					basekey = Registry.ClassesRoot;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_CONFIG":
					basekey = Registry.CurrentConfig;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_CURRENT_USER":
					basekey = Registry.CurrentUser;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_LOCAL_MACHINE":
					basekey = Registry.LocalMachine;
					keyparts.RemoveAt(0);
					break;
				case "HKEY_PERFORMANCE_DATA":
					basekey = Registry.PerformanceData;
					keyparts.RemoveAt(0);
					break;
				default:
					basekey = Registry.CurrentUser;
					break;
			}
			string subkey = string.Join("\\", keyparts.Take(keyparts.Count - 1).ToArray());
			string valuekey = keyparts[keyparts.Count - 1];

			try
			{
				using (RegistryKey key = basekey.CreateSubKey(subkey))
				{
					if (key != null)
					{
						if (key.GetValueNames().Contains(valuekey) == true)
						{
							key.DeleteValue(valuekey, false);
						}
						if (key.GetSubKeyNames().Contains(valuekey) == true)
						{
#if NETFX_4
							key.DeleteSubKeyTree(valuekey, false);
#endif
#if NETFX_3
							key.DeleteSubKeyTree(valuekey);
#endif
						}

						// if the parent now has no values, we can remove it too
						if (key.SubKeyCount == 0 && key.ValueCount == 0)
						{
							basekey.DeleteSubKey(subkey, false);
						}
					}
				}
			}
			catch (System.Security.SecurityException)
			{
				return;
			}
		}

#endregion

#region PGP functions

		/// <summary>
    /// Build a PGP key pair
    /// </summary>
    /// <param name="bits">number of bits in key, e.g. 2048</param>
    /// <param name="identifier">key identifier, e.g. "Your Name <your@emailaddress.com>" </param>
    /// <param name="password">key password or null</param>
    /// <param name="privateKey">returned ascii private key</param>
    /// <param name="publicKey">returned ascii public key</param>
    public static void PGPGenerateKey(int bits, string identifier, string password, out string privateKey, out string publicKey)
    {
      // generate a new RSA keypair 
      RsaKeyPairGenerator gen = new RsaKeyPairGenerator();
      gen.Init(new RsaKeyGenerationParameters(BigInteger.ValueOf(0x101), new Org.BouncyCastle.Security.SecureRandom(), bits, 80));
      AsymmetricCipherKeyPair pair = gen.GenerateKeyPair();

      // create PGP subpacket
      PgpSignatureSubpacketGenerator hashedGen = new PgpSignatureSubpacketGenerator();
      hashedGen.SetKeyFlags(true, PgpKeyFlags.CanCertify | PgpKeyFlags.CanSign | PgpKeyFlags.CanEncryptCommunications | PgpKeyFlags.CanEncryptStorage);
      hashedGen.SetPreferredCompressionAlgorithms(false, new int[] { (int)CompressionAlgorithmTag.Zip });
      hashedGen.SetPreferredHashAlgorithms(false, new int[] { (int)HashAlgorithmTag.Sha1 });
      hashedGen.SetPreferredSymmetricAlgorithms(false, new int[] { (int)SymmetricKeyAlgorithmTag.Cast5 });
      PgpSignatureSubpacketVector sv = hashedGen.Generate();
      PgpSignatureSubpacketGenerator unhashedGen = new PgpSignatureSubpacketGenerator();

      // create the PGP key
      PgpSecretKey secretKey = new PgpSecretKey(
        PgpSignature.DefaultCertification,
        PublicKeyAlgorithmTag.RsaGeneral,
        pair.Public,
        pair.Private,
        DateTime.Now,
        identifier,
        SymmetricKeyAlgorithmTag.Cast5,
        (password != null ? password.ToCharArray() : null),
        hashedGen.Generate(),
        unhashedGen.Generate(),
        new Org.BouncyCastle.Security.SecureRandom());

      // extract the keys
      using (MemoryStream ms = new MemoryStream())
      {
        using (ArmoredOutputStream ars = new ArmoredOutputStream(ms))
        {
          secretKey.Encode(ars);
        }
        privateKey = Encoding.ASCII.GetString(ms.ToArray());
      }
      using (MemoryStream ms = new MemoryStream())
      {
        using (ArmoredOutputStream ars = new ArmoredOutputStream(ms))
        {
          secretKey.PublicKey.Encode(ars);
        }
        publicKey = Encoding.ASCII.GetString(ms.ToArray());
      }
    }

    /// <summary>
    /// Encrypt string using a PGP public key
    /// </summary>
    /// <param name="plain">plain text to encrypt</param>
    /// <param name="armoredPublicKey">public key in ASCII "-----BEGIN PGP PUBLIC KEY BLOCK----- .. -----END PGP PUBLIC KEY BLOCK-----" format</param>
    /// <returns>PGP message string</returns>
    public static string PGPEncrypt(string plain, string armoredPublicKey)
    {
      // encode data
      byte[] data = Encoding.UTF8.GetBytes(plain);

      // create the WinAuth public key
      PgpPublicKey publicKey = null;
      using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(armoredPublicKey)))
      {
        using (Stream dis = PgpUtilities.GetDecoderStream(ms))
        {
          PgpPublicKeyRingBundle bundle = new PgpPublicKeyRingBundle(dis);
          foreach (PgpPublicKeyRing keyring in bundle.GetKeyRings())
          {
            foreach (PgpPublicKey key in keyring.GetPublicKeys())
            {
              if (key.IsEncryptionKey == true && key.IsRevoked() == false)
              {
                publicKey = key;
                break;
              }
            }
          }
        }
      }

      // encrypt the data using PGP
      using (MemoryStream encryptedStream = new MemoryStream())
      {
        using (ArmoredOutputStream armored = new ArmoredOutputStream(encryptedStream))
        {
          PgpEncryptedDataGenerator pedg = new PgpEncryptedDataGenerator(Org.BouncyCastle.Bcpg.SymmetricKeyAlgorithmTag.Cast5, true, new Org.BouncyCastle.Security.SecureRandom());
          pedg.AddMethod(publicKey);
          using (Stream pedgStream = pedg.Open(armored, new byte[4096]))
          {
            PgpCompressedDataGenerator pcdg = new PgpCompressedDataGenerator(Org.BouncyCastle.Bcpg.CompressionAlgorithmTag.Zip);
            using (Stream pcdgStream = pcdg.Open(pedgStream))
            {
              PgpLiteralDataGenerator pldg = new PgpLiteralDataGenerator();
              using (Stream encrypter = pldg.Open(pcdgStream, PgpLiteralData.Binary, "", (long)data.Length, DateTime.Now))
              {
                encrypter.Write(data, 0, data.Length);
              }
            }
          }
        }

        return Encoding.ASCII.GetString(encryptedStream.ToArray());
      }
    }

    /// <summary>
    /// Decrypt a PGP message (i.e. "-----BEGIN PGP MESSAGE----- ... -----END PGP MESSAGE-----")
    /// using the supplied private key.
    /// </summary>
    /// <param name="armoredCipher">PGP message to decrypt</param>
    /// <param name="armoredPrivateKey">PGP private key</param>
    /// <param name="keyPassword">PGP private key password or null if none</param>
    /// <returns>decrypted plain text</returns>
    public static string PGPDecrypt(string armoredCipher, string armoredPrivateKey, string keyPassword)
    {
      // decode the private key
      Dictionary<long, PgpPrivateKey> privateKeys = new Dictionary<long, PgpPrivateKey>();
      using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(armoredPrivateKey)))
      {
        using (Stream dis = PgpUtilities.GetDecoderStream(ms))
        {
          PgpSecretKeyRingBundle bundle = new PgpSecretKeyRingBundle(dis);
          foreach (PgpSecretKeyRing keyring in bundle.GetKeyRings())
          {
            foreach (PgpSecretKey key in keyring.GetSecretKeys())
            {
							privateKeys.Add(key.KeyId, key.ExtractPrivateKey(keyPassword != null ? keyPassword.ToCharArray() : null));
            }
          }
        }
      }

      // decrypt armored block using our private key
      byte[] cipher = Encoding.ASCII.GetBytes(armoredCipher);
      using (MemoryStream decryptedStream = new MemoryStream())
      {
        using (MemoryStream inputStream = new MemoryStream(cipher))
        {
          using (ArmoredInputStream ais = new ArmoredInputStream(inputStream))
          {
            PgpObject message = new PgpObjectFactory(ais).NextPgpObject();
            if (message is PgpEncryptedDataList)
            {
              foreach (PgpPublicKeyEncryptedData pked in ((PgpEncryptedDataList)message).GetEncryptedDataObjects())
              {
								message = new PgpObjectFactory(pked.GetDataStream(privateKeys[pked.KeyId])).NextPgpObject();
              }
            }
            if (message is PgpCompressedData)
            {
              message = new PgpObjectFactory(((PgpCompressedData)message).GetDataStream()).NextPgpObject();
            }
            if (message is PgpLiteralData)
            {
              byte[] buffer = new byte[4096];
              using (Stream stream = ((PgpLiteralData)message).GetInputStream())
              {
                int read;
                while ((read = stream.Read(buffer, 0, 4096)) > 0)
                {
                  decryptedStream.Write(buffer, 0, read);
                }
              }
            }

            return Encoding.UTF8.GetString(decryptedStream.ToArray());
          }
        }
      }
		}

#endregion

	}

	/// <summary>
	/// Helper class to make a StreamWriter use an Encoding else it will default to UTF-16
	/// </summary>
	class EncodedStringWriter : StringWriter
	{
		private readonly Encoding _encoding;

		public EncodedStringWriter(Encoding encoding)
		{
			_encoding = encoding;
		}

		public override Encoding Encoding
		{
			get
			{
				return _encoding;
			}
		}


	}
}
