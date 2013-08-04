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
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Xml;
using System.Xml.XPath;
using System.Windows;
using System.Windows.Forms;

using Microsoft.Win32;

using WinAuth.Resources;

namespace WinAuth
{
	class WinAuthUpdater
	{
		public class WinAuthVersionInfo
		{
			public Version Version;
			public DateTime Released;
			public string Url;
			public string Changes;

			public WinAuthVersionInfo(Version version)
			{
				Version = version;
			}
		}

		private const string WINAUTHREGKEY_LATESTVERSION = @"LatestVersion";

		private const string WINAUTHREGKEY_NEXTCHECK = @"NextCheck";

		private WinAuthVersionInfo _latestVersion;

		public Version GetLatestVersion(bool refresh = false)
		{
			if (_latestVersion == null || refresh == true)
			{
				using (RegistryKey key = Registry.CurrentUser.CreateSubKey(WinAuthHelper.WINAUTHREGKEY))
				{
					if (refresh == false)
					{
						// check the prefecteched latest version
						Version latestversion;
						if (Version.TryParse(key.GetValue(WINAUTHREGKEY_LATESTVERSION, null) as string, out latestversion) == true)
						{
							return latestversion;
						}
					}

					var settings = new System.Configuration.AppSettingsReader();
					string updateUrl = settings.GetValue("UpdateCheckUrl", typeof(string)) as string;
					if (string.IsNullOrEmpty(updateUrl) == true)
					{
						updateUrl = "http://www.winauth.com/current-version.xml";
					}
					using (WebClient web = new WebClient())
					{
						XmlDocument xml = new XmlDocument();
						xml.LoadXml(web.DownloadString(updateUrl));
						var node = xml.SelectSingleNode("//version");
						Version version;
						if (node != null && Version.TryParse(node.Value, out version) == true)
						{
							key.SetValue(WINAUTHREGKEY_LATESTVERSION, version.ToString());

							WinAuthVersionInfo latestversion = new WinAuthVersionInfo(version);

							DateTime released;
							node = xml.SelectSingleNode("//released");
							if (node != null && DateTime.TryParse(node.Value, out released) == true)
							{
								latestversion.Released = released;
							}
							node = xml.SelectSingleNode("//url");
							if (node != null && string.IsNullOrEmpty(node.Value) == false)
							{
								latestversion.Url = node.Value;
							}
							node = xml.SelectSingleNode("//changes");
							if (node != null && string.IsNullOrEmpty(node.Value) == false)
							{
								latestversion.Changes = node.Value;
							}

							_latestVersion = latestversion;
						}
					}
				}
			}

			return (_latestVersion != null ? _latestVersion.Version : null);
		}

		public Version CurrentVersion
		{
			get
			{
				Version version;
				if (Version.TryParse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, out version) == false)
				{
					throw new InvalidOperationException("Cannot get Assembly version information");
				}
				return version;
			}
		}

		public bool CanUpdate
		{
			get
			{
				Version currentversion = CurrentVersion;
				Version latestversion = GetLatestVersion();
				return (latestversion != null && latestversion > currentversion);
			}
		}

		public bool UpdateCheck()
		{
			// get the next check time in registry
			using (RegistryKey key = Registry.CurrentUser.CreateSubKey(WinAuthHelper.WINAUTHREGKEY))
			{
				long nextCheck = 0;
				long.TryParse(key.GetValue(WINAUTHREGKEY_NEXTCHECK, null) as string, out nextCheck);
				if (nextCheck == 0)
				{
					// update the next check as 7 days
					key.SetValue(WINAUTHREGKEY_NEXTCHECK, DateTime.Now.AddDays(7).Ticks.ToString());
				}
				if (nextCheck > DateTime.Now.Ticks)
				{
					return false;
				}

				// use the prefecteched latest version
				Version latestversion;
				if (Version.TryParse(key.GetValue(WINAUTHREGKEY_LATESTVERSION, null) as string, out latestversion) == true && latestversion > CurrentVersion)
				{
					return true;
				}

				var latest = GetLatestVersion(true);
				if (latest != null)
				{
					key.SetValue(WINAUTHREGKEY_LATESTVERSION, latest.ToString());
				}

				// do an update check
				return CanUpdate;
			}
		}
	}
}
