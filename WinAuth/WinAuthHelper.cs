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
		private const string RUNKEY = @"Software\Microsoft\Windows\CurrentVersion\Run";

		/// <summary>
		/// Load the application configuration data
		/// </summary>
		/// <returns>new WinAuthConfig with configuration data</returns>
		public static WinAuthConfig LoadConfig(MainForm form)
		{
			WinAuthConfig data = new WinAuthConfig();

			// load config data
			string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
			string configFile = Path.Combine(configDirectory, WinAuth.DEFAULT_CONFIG_FILE_NAME);
			if (File.Exists(configFile) == true)
			{
				XmlDocument doc = new XmlDocument();
				doc.Load(configFile);

				bool boolVal = false;
				XmlNode node = doc.DocumentElement.SelectSingleNode("AlwaysOnTop");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.AlwaysOnTop = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("HideOnMinimize");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.HideOnMinimize = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("StartWithWindows");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.StartWithWindows = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("AutoRefresh");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.AutoRefresh = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("AllowCopy");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.AllowCopy = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("CopyOnCode");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.CopyOnCode = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("HideSerial");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.HideSerial = boolVal;
				}
				node = doc.DocumentElement.SelectSingleNode("AutoLogin");
				if (node != null && node.InnerText.Length != 0)
				{
					data.AutoLogin = new HoyKeySequence(node.InnerText);
				}
				node = doc.DocumentElement.SelectSingleNode("AuthenticatorFile");
				if (node != null && node.InnerText.Length != 0)
				{
					data.AuthenticatorFile = node.InnerText;
				}
			}

			// override with any commandline
			string[] args = Environment.GetCommandLineArgs();
			for (int i=1; i<args.Length; i++)
			{
				string arg = args[i];
				if (arg[0] == '-')
				{
					switch (arg)
					{
						case "-min":
							// set initial state as minimized
							form.WindowState = FormWindowState.Minimized;
							break;
						default:
							break;
					}
				}
				else
				{
					data.AuthenticatorFile = arg;
				}
			}

			return data;
		}

		/// <summary>
		/// Save the current configuration data
		/// </summary>
		/// <param name="data">current config data</param>
		public static void SaveConfig(WinAuthConfig data)
		{
			// save config data
			string configDirectory = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuth.APPLICATION_NAME);
			Directory.CreateDirectory(configDirectory);
			string configFile = Path.Combine(configDirectory, WinAuth.DEFAULT_CONFIG_FILE_NAME);

			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

			// create the xml
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, "yes");
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("WinAuth");
			root.SetAttribute("version", version.ToString(2));
			doc.AppendChild(root);

			XmlElement node = doc.CreateElement("AlwaysOnTop");
			node.InnerText = data.AlwaysOnTop.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("HideOnMinimize");
			node.InnerText = data.HideOnMinimize.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("StartWithWindows");
			node.InnerText = data.StartWithWindows.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("AutoRefresh");
			node.InnerText = data.AutoRefresh.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("AllowCopy");
			node.InnerText = data.AllowCopy.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("CopyOnCode");
			node.InnerText = data.CopyOnCode.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("HideSerial");
			node.InnerText = data.HideSerial.ToString();
			root.AppendChild(node);
			if (data.AutoLogin != null)
			{
				node = doc.CreateElement("AutoLogin");
				node.InnerText = data.AutoLogin.ToString();
				root.AppendChild(node);
			}
			if (string.IsNullOrEmpty(data.AuthenticatorFile) == false)
			{
				node = doc.CreateElement("AuthenticatorFile");
				node.InnerText = data.AuthenticatorFile.ToString();
				root.AppendChild(node);
			}

			// save the xml to the config file
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			using (XmlWriter xw = XmlWriter.Create(configFile, settings))
			{
				doc.Save(xw);
			}
		}

		/// <summary>
		/// Load an authenticator's data file
		/// </summary>
		/// <param name="configFile">file name of data file</param>
		/// <returns>new AuthenticatorData object</returns>
		public static AuthenticatorData LoadAuthenticator(string configFile)
		{
			return LoadAuthenticator(configFile, null);
		}

		/// <summary>
		/// Load an authenticator's data file
		/// </summary>
		/// <param name="configFile">file name of data file</param>
		/// <param name="password">password to use on config</param>
		/// <returns>new AuthenticatorData object</returns>
		public static AuthenticatorData LoadAuthenticator(string configFile, string password)
		{
			// no file?
			if (File.Exists(configFile) == false)
			{
				return null;
			}

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
		/// Save an authenticator's data
		/// </summary>
		/// <param name="configFile">name of file to save data</param>
		/// <param name="authenticator">Authenticator object to save</param>
		public static void SaveAuthenticator(string configFile, Authenticator authenticator)
		{
			// save the xml to the config file
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			using (XmlWriter xw = XmlWriter.Create(configFile, settings))
			{
				authenticator.Data.WriteXmlString(xw);
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
