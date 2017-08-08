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
using System.Deployment.Application;
using System.Globalization;
using System.IO;
using System.Net;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Xml;
using System.Xml.XPath;
using System.Windows;
using System.Windows.Forms;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Class holding the latest version information
	/// </summary>
	public class WinAuthVersionInfo
	{
		/// <summary>
		/// Version number
		/// </summary>
		public Version Version;

		/// <summary>
		/// Date of release
		/// </summary>
		public DateTime Released;

		/// <summary>
		/// URL for download
		/// </summary>
		public string Url;

		/// <summary>
		/// Optional changes
		/// </summary>
		public string Changes;

		/// <summary>
		/// Create the new version instance
		/// </summary>
		/// <param name="version"></param>
		public WinAuthVersionInfo(Version version)
		{
			Version = version;
		}
	}

	/// <summary>
	/// Class to check for newer version of WinAuth
	/// </summary>
	public class WinAuthUpdater
	{
		/// <summary>
		/// Period when the poller thread will check if it needs to check for a new version
		/// </summary>
		protected const int UPDATECHECKTHREAD_SLEEP = 6 * 60 * 60 * 1000; // 6 hrs to check if we need to check

		/// <summary>
		/// Registry key value name for when we last checked for a new version
		/// </summary>
		protected const string WINAUTHREGKEY_LASTCHECK = WinAuthHelper.WINAUTHREGKEY + "\\LastUpdateCheck";

		/// <summary>
		/// Registry key value name for how often we check for a new version
		/// </summary>
		protected const string WINAUTHREGKEY_CHECKFREQUENCY = WinAuthHelper.WINAUTHREGKEY + "\\UpdateCheckFrequency";

		/// <summary>
		/// Registry key value name for the last version we found when we checked
		/// </summary>
#if BETA
		protected const string WINAUTHREGKEY_LATESTVERSION = WinAuthHelper.WINAUTHREGKEY + "\\LatestBetaVersion";
#else
		protected const string WINAUTHREGKEY_LATESTVERSION = WinAuthHelper.WINAUTHREGKEY + "\\LatestVersion";
#endif

		/// <summary>
		/// The interval for checking new versions. Null is never, Zero is each time, else a period.
		/// </summary>
		private TimeSpan? _autocheckInterval;

		/// <summary>
		/// The last known new version
		/// </summary>
		private Version _latestVersion;

		/// <summary>
		/// When we last checked for a new version
		/// </summary>
		private DateTime _lastCheck;

		/// <summary>
		/// Current Config
		/// </summary>
		protected WinAuthConfig Config { get; set; }

		/// <summary>
		/// Create the version checker instance
		/// </summary>
		public WinAuthUpdater(WinAuthConfig config)
		{
			Config = config;

			// read the update interval and last known latest version from the registry
			TimeSpan interval;
			if (TimeSpan.TryParse(Config.ReadSetting(WINAUTHREGKEY_CHECKFREQUENCY, string.Empty), out interval) == true)
			{
				_autocheckInterval = interval;
			}

			long lastCheck = 0;
			if (long.TryParse(Config.ReadSetting(WINAUTHREGKEY_LASTCHECK, null), out lastCheck) == true)
			{
				_lastCheck = new DateTime(lastCheck);
			}

			Version version;
#if NETFX_4
			if (Version.TryParse(Config.ReadSetting(WINAUTHREGKEY_LATESTVERSION, string.Empty), out version) == true)
			{
				_latestVersion = version;
			}
#endif
#if NETFX_3
			try
			{
				version = new Version(Config.ReadSetting(WINAUTHREGKEY_LATESTVERSION, string.Empty));
				_latestVersion = version;
			}
			catch (Exception) { }
#endif
		}

		#region Properties

		/// <summary>
		/// Get when the last check was done
		/// </summary>
		public DateTime LastCheck
		{
			get
			{
				return _lastCheck;
			}
		}

		/// <summary>
		/// Get the last known latest version or null
		/// </summary>
		public Version LastKnownLatestVersion
		{
			get
			{
				return _latestVersion;
			}
			protected set
			{
				_latestVersion = value;
			}
		}

		/// <summary>
		/// Get the current version
		/// </summary>
		public Version CurrentVersion
		{
			get
			{
				Version version;
#if NETFX_4
				if (Version.TryParse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, out version) == false)
				{
					throw new InvalidOperationException("Cannot get Assembly version information");
				}
#endif
#if NETFX_3
				try
				{
					version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
				}
				catch (Exception)
				{
					throw new InvalidOperationException("Cannot get Assembly version information");
				}
#endif
				return version;
			}
		}

		/// <summary>
		/// Get flag if we have autochecking enabled
		/// </summary>
		public bool IsAutoCheck
		{
			get
			{
				return (_autocheckInterval != null);
			}
		}

		/// <summary>
		/// Get the interval between checks
		/// </summary>
		public TimeSpan? UpdateInterval
		{
			get
			{
				return _autocheckInterval;
			}
		}

#endregion

		/// <summary>
		/// Start an AutoCheck thread that will periodically check for a new version and make a callback
		/// </summary>
		/// <param name="callback">Callback when a new version is found</param>
		public void AutoCheck(Action<Version> callback)
		{
			// create a thread to check for latest version
			Thread thread = new Thread(new ParameterizedThreadStart(AutoCheckPoller));
			thread.IsBackground = true;
			thread.Priority = ThreadPriority.BelowNormal;
			thread.Start(callback);
		}

		/// <summary>
		/// AutoCheck thread method to poll for new version
		/// </summary>
		/// <param name="p">our callback</param>
		protected virtual void AutoCheckPoller(object p)
		{
			Action<Version> callback = p as Action<Version>;

			do
			{
				// only if autochecking is on, and is due, and we don't already have a later version
				if (this.IsAutoCheck == true
					&& _autocheckInterval.HasValue && _lastCheck.Add(_autocheckInterval.Value) < DateTime.Now
					&& (LastKnownLatestVersion == null || LastKnownLatestVersion <= this.CurrentVersion))
				{
					// update the last check time
					_lastCheck = DateTime.Now;
					Config.WriteSetting(WINAUTHREGKEY_LASTCHECK, _lastCheck.Ticks.ToString());

					// check for latest version
					try
					{
						var latest = GetLatestVersion();
						if (latest != null && latest.Version > this.CurrentVersion)
						{
							callback(latest.Version);
						}
					}
					catch (Exception) { }
				}

				Thread.Sleep(UPDATECHECKTHREAD_SLEEP);

			} while (true);
		}

		/// <summary>
		/// Explicitly get the latest version information. Will be asynchronous if a callback is provided.
		/// </summary>
		/// <param name="callback">optional callback for async operation</param>
		/// <returns>latest WinAuthVersionInfo or null if async</returns>
		public virtual WinAuthVersionInfo GetLatestVersion(Action<WinAuthVersionInfo, bool, Exception> callback = null)
		{
			// get the update URL from the config else use the default
			string updateUrl = WinAuthMain.WINAUTH_UPDATE_URL;
			try
			{
				var settings = new System.Configuration.AppSettingsReader();
				string appvalue = settings.GetValue("UpdateCheckUrl", typeof(string)) as string;
				if (string.IsNullOrEmpty(appvalue) == false)
				{
					updateUrl = appvalue;
				}
			}
			catch (Exception) { }
			try
			{
				using (WebClient web = new WebClient())
				{
					web.Headers.Add("User-Agent", "WinAuth-" + this.CurrentVersion.ToString());
					if (callback == null)
					{
						// immediate request
						string result = web.DownloadString(updateUrl);
						WinAuthVersionInfo latestVersion = ParseGetLatestVersion(result);
						if (latestVersion != null)
						{
							// update local values
							LastKnownLatestVersion = latestVersion.Version;
							Config.WriteSetting(WINAUTHREGKEY_LATESTVERSION, latestVersion.Version.ToString(3));
						}
						return latestVersion;
					}
					else
					{
						// initiate async operation
						web.DownloadStringCompleted += new DownloadStringCompletedEventHandler(GetLatestVersionDownloadCompleted);
						web.DownloadStringAsync(new Uri(updateUrl), callback);
						return null;
					}
				}
			}
			catch (Exception )
			{
				// don't fail if we can't get latest version
				return null;
			}
		}

		/// <summary>
		/// Callback for async operation for latest version web request
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		private void GetLatestVersionDownloadCompleted(object sender, DownloadStringCompletedEventArgs args)
		{
			// no point if e have no callback
			Action<WinAuthVersionInfo, bool, Exception> callback = args.UserState as Action<WinAuthVersionInfo, bool, Exception>;
			if (callback == null)
			{
				return;
			}

			// report cancelled or error
			if (args.Cancelled == true || args.Error != null)
			{
				callback(null, args.Cancelled, args.Error);
				return;
			}

			try
			{
				// extract the latest version
				WinAuthVersionInfo latestVersion = ParseGetLatestVersion(args.Result);
				if (latestVersion != null)
				{
					// update local values
					LastKnownLatestVersion = latestVersion.Version;
					Config.WriteSetting(WINAUTHREGKEY_LATESTVERSION, latestVersion.Version.ToString(3));
				}
				// perform callback
				callback(latestVersion, false, null);
			}
			catch (Exception ex)
			{
				// report any other error
				callback(null, false, ex);
			}
		}

		/// <summary>
		/// Parse the returned xml from the website request to extract version information
		/// </summary>
		/// <param name="result">version xml information</param>
		/// <returns>new WinAuthVersionInfo object</returns>
		private WinAuthVersionInfo ParseGetLatestVersion(string result)
		{
			// load xml document and pull out nodes
			XmlDocument xml = new XmlDocument();
			xml.LoadXml(result);
			var node = xml.SelectSingleNode("//version");

			Version version = null;
#if NETFX_4
			Version.TryParse(node.InnerText, out version);
#endif
#if NETFX_3
			try
			{
				version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
			}
			catch (Exception) { }
#endif
			if (node != null && version != null)
			{
				WinAuthVersionInfo latestversion = new WinAuthVersionInfo(version);

				DateTime released;
				node = xml.SelectSingleNode("//released");
				if (node != null && DateTime.TryParse(node.InnerText, out released) == true)
				{
					latestversion.Released = released;
				}
#if NETFX_4
				node = xml.SelectSingleNode("//url");
#endif
#if NETFX_3
				node = xml.SelectSingleNode("//url35");
#endif
				if (node != null && string.IsNullOrEmpty(node.InnerText) == false)
				{
					latestversion.Url = node.InnerText;
				}
				node = xml.SelectSingleNode("//changes");
				if (node != null && string.IsNullOrEmpty(node.InnerText) == false)
				{
					latestversion.Changes = node.InnerText;
				}

				return latestversion;
			}
			else
			{
				throw new InvalidOperationException("Invalid return data");
			}
		}

		/// <summary>
		/// Set the interval for automatic update checks. Null is disabled. Zero is every time.
		/// </summary>
		/// <param name="interval">new interval or null to disable</param>
		public void SetUpdateInterval(TimeSpan? interval)
		{
			// get the next check time
			if (interval != null)
			{
				// write into regisry
				
				Config.WriteSetting(WINAUTHREGKEY_CHECKFREQUENCY, string.Format("{0}:{1:00}:{2:00}", interval.Value.TotalHours, interval.Value.Minutes, interval.Value.Seconds)); // toString("c") is Net4

				// if last update not set, set to now
				if (Config.ReadSetting(WINAUTHREGKEY_LASTCHECK) == null)
				{
					Config.WriteSetting(WINAUTHREGKEY_LASTCHECK, DateTime.Now.Ticks.ToString());
				}
			}
			else
			{
				// remove from registry
				Config.WriteSetting(WINAUTHREGKEY_CHECKFREQUENCY, null);
			}

			// update local values
			_autocheckInterval = interval;
		}
	}
}
