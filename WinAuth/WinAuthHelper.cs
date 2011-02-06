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
using System.Text;
using System.Xml;
using System.Windows;
using System.Windows.Forms;

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
		private const string WINAUTHREGKEY_LASTFILE = @"File1";

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
				configFile = GetLastFile();
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
					XmlDocument doc = new XmlDocument();
					doc.Load(configFile);

					// check and load older versions
					XmlNode node = doc.SelectSingleNode("WinAuth");
					XmlAttribute versionAttr;
					decimal version;
					if (node == null)
					{
						// foreign file so we import (authenticator.xml from winauth_1.3, android BMA xml or Java rs)
						AuthenticatorData data = LoadAuthenticator(form, configFile);
						if (data != null)
						{
							config.Authenticator = new Authenticator(data);
						}
						return config;
					}
					if ((versionAttr = node.Attributes["version"]) != null && decimal.TryParse(versionAttr.InnerText, out version) && version < (decimal)1.4)
					{
						// Show if BETA
						//if (new BetaForm().ShowDialog(form) != DialogResult.OK)
						//{
						//  return null;
						//}

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
						foreach (XmlNode authenticatorNode in nodes)
						{
							// load the data
							AuthenticatorData data = null;
							try
							{
								try
								{
									data = new AuthenticatorData(authenticatorNode, password);
									config.Authenticator = new Authenticator(data);
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
											data = new AuthenticatorData(authenticatorNode, passwordForm.Password);
											config.Authenticator = new Authenticator(data);
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
							}
							catch (InvalidConfigDataException)
							{
								MessageBox.Show(form, "The authenticator data in " + configFile + " is not valid", "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
							catch (Exception ex)
							{
								MessageBox.Show(form, "Unable to load authenticator from " + configFile + ": " + ex.Message, "Load Authenticator", MessageBoxButtons.OK, MessageBoxIcon.Error);
							}
						}
					}

					// get the autologin node after we have gotten the pasword
					node = doc.DocumentElement.SelectSingleNode("autologin");
					if (node != null && node.InnerText.Length != 0)
					{
						config.AutoLogin = new HoyKeySequence(node, config.Authenticator.Data.Password);
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
					AuthenticatorData data = LoadAuthenticator(form, filename);
					if (data != null)
					{
						config.Authenticator = new Authenticator(data);
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
		/// <returns>new AuthenticatorData object</returns>
		public static AuthenticatorData LoadAuthenticator(Form form, string configFile)
		{
			// load the data
			AuthenticatorData data = null;
			try
			{
				try
				{
					// import the file
					data = ImportAuthenticator(configFile, null);

					// if this was an import, i.e. an .rms file, then clear authFile so we aare forcesto save a new name
					if (data != null && data.LoadedFormat != AuthenticatorData.FileFormat.WinAuth)
					{
						configFile = null;
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
		/// <returns>new AuthenticatorData</returns>
		public static AuthenticatorData ImportAuthenticator(string configFile, string password)
		{
			using (FileStream fs = new FileStream(configFile, FileMode.Open))
			{
				// read the file
				string ext = Path.GetExtension(configFile).ToLower();
				if (ext == ".xml")
				{
					// load ours or the Android XML file
					return new AuthenticatorData(fs, AuthenticatorData.FileFormat.WinAuth, password);
				}
				else
				{
					// load the Java MIDP recordfile
					return new AuthenticatorData(fs, AuthenticatorData.FileFormat.Midp, password);
				}
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
			using (XmlWriter writer = XmlWriter.Create(configFile, settings))
			{
				config.WriteXmlString(writer);
			}

			SetLastFile(configFile); // set this as the new last opened file
		}

		/// <summary>
		/// Get the last authenticator file name from the registry
		/// </summary>
		/// <returns>name of last file or null</returns>
		public static string GetLastFile()
		{
			using (RegistryKey key = Registry.CurrentUser.OpenSubKey(WINAUTHREGKEY, false))
			{
				return (key == null ? null : key.GetValue(WINAUTHREGKEY_LASTFILE, null) as string);
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
				key.SetValue(WINAUTHREGKEY_LASTFILE, filename);
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
	}
}
