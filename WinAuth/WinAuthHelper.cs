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
using System.Globalization;
using System.IO;
using System.Text;
using System.Xml;
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

using Microsoft.Win32;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class proving helper functions to save data for application
	/// </summary>
	class WinAuthHelper
	{
		/// <summary>
		/// Registry key for application
		/// </summary>
		private const string WINAUTHREGKEY = @"Software\WinAuth";

		/// <summary>
		/// Registry data name for last loaded file
		/// </summary>
		private const string WINAUTHREGKEY_LASTFILE = @"File{0}";

		/// <summary>
		/// Registry data name for saved skin
		/// </summary>
		private const string WINAUTHREGKEY_SKIN = @"Skin";

		/// <summary>
		/// Registry data name for machine time difference
		/// </summary>
		private const string WINAUTHREGKEY_TIMEDIFF = @"TimeDiff";

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
		public const string DEFAULT_AUTHENTICATOR_FILE_NAME = "authenticator.xml";

		/// <summary>
		/// Number of password attempts
		/// </summary>
		private const int MAX_PASSWORD_RETRIES = 3;

		/// <summary>
		/// Number of saved authenticators
		/// </summary>
		private const int MAX_SAVED_FILES = 4;

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
		public static WinAuthConfig LoadConfig(MainForm form, string configFile, string password)
		{
			WinAuthConfig config = new WinAuthConfig();

			if (string.IsNullOrEmpty(configFile) == true)
			{
				configFile = GetLastFile(1);
				if (string.IsNullOrEmpty(configFile) == false && File.Exists(configFile) == false)
				{
					// ignore it if file does't exist
					configFile = null;
				}
			}
			if (string.IsNullOrEmpty(configFile) == true)
			{
				// do we have a file specific in the registry?
				string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
				Directory.CreateDirectory(configDirectory);
				// check the old 1.3 file name
				configFile = Path.Combine(configDirectory, CONFIG_FILE_NAME_1_3);
				if (File.Exists(configFile) == false)
				{
					// check for default authenticator
					configFile = Path.Combine(configDirectory, DEFAULT_AUTHENTICATOR_FILE_NAME);
				}
				// if no config file, just return a blank config
				if (File.Exists(configFile) == false)
				{
					return config;
				}
			}

			// if no config file when one was specified; report an error
			if (File.Exists(configFile) == false)
			{
				MessageBox.Show(form,
					"Unable to find your configuration file \"" + configFile + "\"",
					form.Text, MessageBoxButtons.OK, MessageBoxIcon.Error);
				return config;
			}

			DialogResult configloaded;
			do {
				configloaded = DialogResult.OK;

				try
				{
					XmlDocument doc = null;
					XmlNode node = null;
					try
					{
						doc = new XmlDocument();
						doc.Load(configFile);

						// check and load older versions
						node = doc.SelectSingleNode("WinAuth");
					}
					catch (XmlException )
					{
						// cause by invalid format, so we try and load other type of authenticator
					}
					if (node == null)
					{
						// foreign file so we import (authenticator.xml from winauth_1.3, android BMA xml or Java rs)
						Authenticator auth = LoadAuthenticator(form, configFile);
						if (auth != null)
						{
							config.Authenticator = auth;
						}
						SetLastFile(configFile); // set this as the new last opened file
						return config;
					}

					// Show if BETA
					//if (new BetaForm().ShowDialog(form) != DialogResult.OK)
					//{
					//  return null;
					//}

					XmlAttribute versionAttr;
					decimal version = Authenticator.DEAFULT_CONFIG_VERSION;
					if ((versionAttr = node.Attributes["version"]) != null && (version = decimal.Parse(versionAttr.InnerText, System.Globalization.CultureInfo.InvariantCulture)) < (decimal)1.4)
					{
						// old version 1.3 file
						config = LoadConfig_1_3(form, configFile);
						if (string.IsNullOrEmpty(config.Filename) == true)
						{
							config.Filename = configFile;
						}
						else if (string.Compare(configFile, config.Filename, true) != 0)
						{
							// switch over from winauth.xml to authenticator.xml and remove old winauth.xml
							File.Delete(configFile);
							configFile = config.Filename;
						}
						SaveAuthenticator(form, configFile, config);
						SetLastFile(configFile); // set this as the new last opened file
						return config;
					}

					// set the filename as itself
					config.Filename = configFile;

					bool boolVal = false;
					node = doc.DocumentElement.SelectSingleNode("alwaysontop");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.AlwaysOnTop = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("usetrayicon");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.UseTrayIcon = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("startwithwindows");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.StartWithWindows = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("autorefresh");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.AutoRefresh = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("allowcopy");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.AllowCopy = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("copyoncode");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.CopyOnCode = boolVal;
					}
					node = doc.DocumentElement.SelectSingleNode("hideserial");
					if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
					{
						config.HideSerial = boolVal;
					}

					// load the authenticator(s) - may have multiple authenticators in future version
					XmlNodeList nodes = doc.DocumentElement.SelectNodes("authenticator");
					if (nodes != null)
					{
						// get the local machine time diff
						long machineTimeDiff = GetMachineTimeDiff();

						foreach (XmlNode authenticatorNode in nodes)
						{
							// load the data
							Authenticator auth = null;
							try
							{
								try
								{
									//auth = new Authenticator();
									//auth.Load(authenticatorNode, password, version);
									//config.Authenticator = auth;
									using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(authenticatorNode.OuterXml)))
									{
										config.Authenticator = Authenticator.ReadFromStream(ms, password, version);
									}
								}
								catch (EncrpytedSecretDataException)
								{
									PasswordForm passwordForm = new PasswordForm();

									int retries = 0;
									do
									{
										passwordForm.Password = string.Empty;
										DialogResult result = passwordForm.ShowDialog(form);
										if (result != System.Windows.Forms.DialogResult.OK)
										{
											break;
										}

										try
										{
											//auth = new Authenticator();
											//auth.Load(authenticatorNode, passwordForm.Password, version);
											//config.Authenticator = auth;
											using (MemoryStream ms = new MemoryStream(Encoding.UTF8.GetBytes(authenticatorNode.OuterXml)))
											{
												config.Authenticator = Authenticator.ReadFromStream(ms, password, version);
											}
											break;
										}
										catch (BadPasswordException)
										{
											MessageBox.Show(form, "Invalid password", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
											if (retries++ >= MAX_PASSWORD_RETRIES - 1)
											{
												break;
											}
										}
									} while (true);
								}

								// adjust the time diff from the local machine
								if (auth != null && machineTimeDiff != 0)
								{
									auth.ServerTimeDiff = machineTimeDiff;
								}
							}
							catch (InvalidUserDecryptionException)
							{
								MessageBox.Show(form, "The authenticator was encrypted using a different Windows User account.", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							catch (InvalidMachineDecryptionException)
							{
								MessageBox.Show(form, "The authenticator was encrypted using a different Windows computer.", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							catch (InvalidConfigDataException)
							{
								MessageBox.Show(form, "The authenticator data in " + configFile + " is not valid", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							catch (AuthenticatorException ae)
							{
								// detect invalid encryption data
								DialogResult r = MessageBox.Show(form,
									"An unexpected error occurred whilst load your authenticator: " + ae.Message + ".\n\nPlease try again or restart WinAuth.\n\nWould you like to send an error report to help try and fix this problem?",
									WinAuth.APPLICATION_NAME,
									MessageBoxButtons.YesNo,
									MessageBoxIcon.Error);
								if (r == System.Windows.Forms.DialogResult.Yes)
								{
									ErrorReportForm errorreport = new ErrorReportForm();
									errorreport.Config = config;
									errorreport.ConfigFileContents = File.ReadAllText(configFile);
									errorreport.ErrorException = ae;
									errorreport.ShowDialog(form);
								}
							}
							catch (Exception ex)
							{
								MessageBox.Show(form, "Unable to load authenticator from " + configFile + ": " + ex.Message, "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}

					// get the autologin node after we have gotten the pasword
					node = doc.DocumentElement.SelectSingleNode("autologin");
					if (node != null && node.InnerText.Length != 0 && config.Authenticator != null)
					{
						config.AutoLogin = new HoyKeySequence(node, config.Authenticator.Password, version);
					}
				}
				catch (Exception ex)
				{
					configloaded = MessageBox.Show(form,
						"An error occured while loading your configuration file \"" + configFile + "\": " + ex.Message + "\n\nIt may be corrupted or in use by another application.",
						form.Text, MessageBoxButtons.AbortRetryIgnore, MessageBoxIcon.Error);
					if (configloaded == DialogResult.Abort)
					{
						return null;
					}
				}
			} while (configloaded == DialogResult.Retry);

			SetLastFile(configFile); // set this as the new last opened file

			return config;
		}

		/// <summary>
		/// Load the application configuration data for a 1.3 version
		/// </summary>
		/// <param name="form">parent winform</param>
		/// <param name="configFile">name of configuration file</param>
		/// <returns>new WinAuthConfig</returns>
		private static WinAuthConfig LoadConfig_1_3(MainForm form, string configFile)
		{
			WinAuthConfig config = new WinAuthConfig();
			if (File.Exists(configFile) == false)
			{
				return config;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(configFile);

			bool boolVal = false;

			// load the system config
			XmlNode node = doc.DocumentElement.SelectSingleNode("AlwaysOnTop");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.AlwaysOnTop = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("UseTrayIcon");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.UseTrayIcon = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("StartWithWindows");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.StartWithWindows = boolVal;
			}

			// load the authenticator config
			node = doc.DocumentElement.SelectSingleNode("AutoRefresh");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.AutoRefresh = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("AllowCopy");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.AllowCopy = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("CopyOnCode");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.CopyOnCode = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("HideSerial");
			if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
			{
				config.HideSerial = boolVal;
			}
			node = doc.DocumentElement.SelectSingleNode("AutoLogin");
			if (node != null && node.InnerText.Length != 0)
			{
				config.AutoLogin = new HoyKeySequence(node.InnerText);
			}
			node = doc.DocumentElement.SelectSingleNode("AuthenticatorFile");
			if (node != null && node.InnerText.Length != 0)
			{
				// load the authenticaotr.xml file
				string filename = node.InnerText;
				if (File.Exists(filename) == true)
				{
					Authenticator auth = LoadAuthenticator(form, filename);
					if (auth != null)
					{
						config.Authenticator = auth;
					}
				}
				config.Filename = filename;
			}

			return config;
		}

		/// <summary>
		/// Load an old or 3rd party authenticator file
		/// </summary>
		/// <param name="form">parent winform</param>
		/// <param name="configFile">filename to load</param>
		/// <returns>new Authenticator object</returns>
		public static Authenticator LoadAuthenticator(Form form, string configFile)
		{
			// load the data
			Authenticator data = null;
			try
			{
				try
				{
					// import the file
					data = ImportAuthenticator(configFile, null);

					// if this was an import, i.e. an .rms file, then clear authFile so we aare forcesto save a new name
					//if (data != null && data.LoadedFormat != Authenticator.FileFormat.WinAuth)
					//{
					//  configFile = null;
					//}
				}
				catch (EncrpytedSecretDataException)
				{
					PasswordForm passwordForm = new PasswordForm();

					int retries = 0;
					do
					{
						passwordForm.Password = string.Empty;
						DialogResult result = passwordForm.ShowDialog(form);
						if (result != System.Windows.Forms.DialogResult.OK)
						{
							return null;
						}

						try
						{
							data = ImportAuthenticator(configFile, passwordForm.Password);
							break;
						}
						catch (BadPasswordException)
						{
							MessageBox.Show(form, "Invalid password", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							if (retries++ >= MAX_PASSWORD_RETRIES - 1)
							{
								return null;
							}
						}
					} while (true);
				}
			}
			catch (InvalidConfigDataException)
			{
				MessageBox.Show(form, "The authenticator file " + configFile + " is not valid", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return data;
			}
			catch (Exception ex)
			{
				MessageBox.Show(form, "Unable to load authenticator file " + configFile + ": " + ex.Message, "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return data;
			}
			if (data == null)
			{
				MessageBox.Show(form, "The file does not contain valid authenticator data.", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return data;
			}

			return data;
		}

		/// <summary>
		/// Import and authenticator file of different formats
		/// </summary>
		/// <param name="configFile">filename to load</param>
		/// <param name="password">optional password</param>
		/// <returns>new Authenticator</returns>
		public static Authenticator ImportAuthenticator(string configFile, string password)
		{
			using (FileStream fs = new FileStream(configFile, FileMode.Open))
			{
				// load ours or the Android XML file
				Authenticator auth = Authenticator.ReadFromStream(fs, password);

				return auth;
			}
		}

		/// <summary>
		/// Save the authenticator
		/// </summary>
		/// <param name="form">parent winform</param>
		/// <param name="configFile">filename to save to</param>
		/// <param name="config">current settings to save</param>
		public static void SaveAuthenticator(Form form, string configFile, WinAuthConfig config)
		{
			// create the xml
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;

			// Issue 41 (http://code.google.com/p/winauth/issues/detail?id=41): saving may crash leaving file corrupt, so write into memory stream first before an atomic file write
			using (MemoryStream ms = new MemoryStream())
			{
				// save config into memory
				using (XmlWriter writer = XmlWriter.Create(ms, settings))
				{
					config.WriteXmlString(writer);
				}

				// write memory stream to file
				byte[] data = ms.ToArray();
				using (FileStream fs = new FileStream(configFile, FileMode.Create, FileAccess.Write, FileShare.None))
				{
					fs.Write(data, 0, data.Length);
				}
			}

			// use this as the last fle
			SetLastFile(configFile);
		}

		/// <summary>
		/// Get the last authenticator file name from the registry
		/// </summary>
		/// <param name="index">index of last entry to load</param>
		/// <returns>name of last file or null</returns>
		public static string GetLastFile(int index)
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(WINAUTHREGKEY, false))
			{
				return (key == null ? null : key.GetValue(string.Format(CultureInfo.InvariantCulture, WINAUTHREGKEY_LASTFILE, index), null) as string);
			}
		}

		/// <summary>
		/// Set the last file name in the registry
		/// </summary>
		/// <param name="filename">filename to save</param>
		public static void SetLastFile(string filename)
		{
			using (RegistryKey key = Registry.CurrentUser.CreateSubKey(WINAUTHREGKEY))
			{
				// read all the last files
				List<string> lastfiles = new List<string>();
				for (int index=1; index<=MAX_SAVED_FILES; index++)
				{
					string lastfile = GetLastFile(index);
					lastfiles.Add(lastfile);
				}
				// make sure current one is at the start
				if (string.IsNullOrEmpty(filename) == false)
				{
					lastfiles.Remove(filename);
					lastfiles.Insert(0, filename);
				}
				// write all the entries back to registry
				for (int index=1; index<=MAX_SAVED_FILES; index++)
				{
					string lastfile = lastfiles[index-1];
					if (string.IsNullOrEmpty(lastfile) == true)
					{
						key.DeleteValue(string.Format(CultureInfo.InvariantCulture, WINAUTHREGKEY_LASTFILE, index), false);
					}
					else
					{
						key.SetValue(string.Format(CultureInfo.InvariantCulture, WINAUTHREGKEY_LASTFILE, index), lastfile);
					}
				}
			}
		}

		/// <summary>
		/// Set up winauth so it will start with Windows by adding entry into registry
		/// </summary>
		/// <param name="enabled">enable or disable start with windows</param>
		public static void SetStartWithWindows(bool enabled)
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(RUNKEY, true))
			{
				if (enabled == true)
				{
					// get path of exe and minimize flag
					key.SetValue(WinAuth.APPLICATION_NAME, Application.ExecutablePath + " -min");
				}
				else if (key.GetValue(WinAuth.APPLICATION_NAME) != null)
				{
					key.DeleteValue(WinAuth.APPLICATION_NAME);
				}
			}
		}

		/// <summary>
		/// Get the saved skin file name from the registry
		/// </summary>
		/// <returns>name of skin file or null if none</returns>
		public static string GetSavedSkin()
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(WINAUTHREGKEY, false))
			{
				return (key == null ? null : key.GetValue(WINAUTHREGKEY_SKIN, null) as string);
			}
		}

		/// <summary>
		/// Set or remove the saved skin in the registry
		/// </summary>
		/// <param name="skin">name of skin file or null to remove</param>
		public static void SetSavedSkin(string skin)
		{
			using (RegistryKey key = Registry.CurrentUser.CreateSubKey(WINAUTHREGKEY))
			{
				if (string.IsNullOrEmpty(skin) == false)
				{
					key.SetValue(WINAUTHREGKEY_SKIN, skin);
				}
				else
				{
					key.DeleteValue(WINAUTHREGKEY_SKIN, false);
				}
			}
		}

		/// <summary>
		/// Get the difference for the time of the local machine from the servers
		/// </summary>
		/// <returns>name of skin file or null if none</returns>
		public static long GetMachineTimeDiff()
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(WINAUTHREGKEY, false))
			{
				return (key == null ? 0L : (long)key.GetValue(WINAUTHREGKEY_TIMEDIFF, 0L));
			}
		}

		/// <summary>
		/// Set the time difference in the registry
		/// </summary>
		/// <param name="skin">name of skin file or null to remove</param>
		public static void SetMachineTimeDiff(long diff)
		{
			using (RegistryKey key = Registry.CurrentUser.CreateSubKey(WINAUTHREGKEY))
			{
				key.SetValue(WINAUTHREGKEY_TIMEDIFF, diff, RegistryValueKind.QWord);
			}
		}

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
			PgpPrivateKey privateKey = null;
			using (MemoryStream ms = new MemoryStream(Encoding.ASCII.GetBytes(armoredPrivateKey)))
			{
				using (Stream dis = PgpUtilities.GetDecoderStream(ms))
				{
					PgpSecretKeyRingBundle bundle = new PgpSecretKeyRingBundle(dis);
					foreach (PgpSecretKeyRing keyring in bundle.GetKeyRings())
					{
						foreach (PgpSecretKey key in keyring.GetSecretKeys())
						{
							privateKey = key.ExtractPrivateKey(keyPassword != null ? keyPassword.ToCharArray() : null);
							break;
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
								message = new PgpObjectFactory(pked.GetDataStream(privateKey)).NextPgpObject();
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

	}
}
