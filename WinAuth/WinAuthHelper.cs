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

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class proving helper functions to save data for application
	/// </summary>
	class WinAuthHelper
	{
		/// <summary>
		/// Load the application configuration data
		/// </summary>
		/// <returns>new WinAuthConfig with configuration data</returns>
		public static WinAuthConfig LoadConfig()
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
				node = doc.DocumentElement.SelectSingleNode("AutoRefresh");
				if (node != null && bool.TryParse(node.InnerText, out boolVal) == true)
				{
					data.AutoRefresh = boolVal;
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
				node = doc.DocumentElement.SelectSingleNode("AuthenticatorFile");
				if (node != null && node.InnerText.Length != 0)
				{
					data.AuthenticatorFile = node.InnerText;
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
			node = doc.CreateElement("AutoRefresh");
			node.InnerText = data.AutoRefresh.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("CopyOnCode");
			node.InnerText = data.CopyOnCode.ToString();
			root.AppendChild(node);
			node = doc.CreateElement("HideSerial");
			node.InnerText = data.HideSerial.ToString();
			root.AppendChild(node);
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
			if (File.Exists(configFile) == false)
			{
				return null;
			}

			XmlDocument doc = new XmlDocument();
			doc.Load(configFile);

			string data = null;
			XmlNode node = doc.DocumentElement.SelectSingleNode("Data");
			if (node != null)
			{
				data = node.InnerText;
			}

			return new AuthenticatorData(data);
		}

		/// <summary>
		/// Save an authenticator's data
		/// </summary>
		/// <param name="configFile">name of file to save data</param>
		/// <param name="authenticator">Authenticator object to save</param>
		public static void SaveAuthenticator(string configFile, Authenticator authenticator)
		{
			// save config data
			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;

			// create the xml
			XmlDocument doc = new XmlDocument();
			XmlDeclaration dec = doc.CreateXmlDeclaration("1.0", null, "yes");
			doc.AppendChild(dec);
			XmlElement root = doc.CreateElement("AuthenticatorData");
			root.SetAttribute("version", version.ToString(2));
			doc.AppendChild(root);

			XmlElement node = doc.CreateElement("Data");
			node.InnerText = authenticator.Data.ToString();
			root.AppendChild(node);

			// save the xml to the config file
			XmlWriterSettings settings = new XmlWriterSettings();
			settings.Indent = true;
			using (XmlWriter xw = XmlWriter.Create(configFile, settings))
			{
				doc.Save(xw);
			}
		}
	}
}
