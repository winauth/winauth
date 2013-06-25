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
using System.Diagnostics;
using System.Globalization;
using System.Windows.Forms;
using System.IO;
using System.Reflection;
using System.Text;

namespace WinAuth
{
  /// <summary>
  /// Class that launches the main form
  /// </summary>
  static class WinAuthMain
  {
    /// <summary>
    /// Name of this application used for %USEPATH%\[name] folder
    /// </summary>
    public const string APPLICATION_NAME = "WinAuth";

    /// <summary>
    /// Window title for this application
    /// </summary>
    public const string APPLICATION_TITLE = "WinAuth";

    /// <summary>
    /// Winuath email address used as sender to backup emails
    /// </summary>
    public const string WINAUTHBACKUP_EMAIL = "winauth@gmail.com";

		/// <summary>
		/// Set of inbuilt icons and authenticator types
		/// </summary>
		public static Dictionary<string, string> AUTHENTICATOR_ICONS = new Dictionary<string, string>
		{
			{"Battle.Net", "BattleNetAuthenticatorIcon.png"},
			{"Guild Wars 2", "GuildWarsAuthenticatorIcon.png"},
			{"Google", "GoogleAuthenticatorIcon.png"},
			{"Microsoft", "MicrosoftAuthenticatorIcon.png"}
		};

		public static List<RegisteredAuthenticator> REGISTERED_AUTHENTICATORS = new List<RegisteredAuthenticator>
		{
			new RegisteredAuthenticator {Name="Battle.Net", AuthenticatorType=RegisteredAuthenticator.AuthenticatorTypes.BattleNet, Icon="BattleNetAuthenticatorIcon.png"},
			new RegisteredAuthenticator {Name="Guild Wars 2", AuthenticatorType=RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME, Icon="GuildWarsAuthenticatorIcon.png"},
			new RegisteredAuthenticator {Name="Google", AuthenticatorType=RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME, Icon="GoogleAuthenticatorIcon.png"},
			new RegisteredAuthenticator {Name="Microsoft", AuthenticatorType=RegisteredAuthenticator.AuthenticatorTypes.RFC6238_TIME, Icon="MicrosoftAuthenticatorIcon.png"}
		};

    /// <summary>
    /// The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
			if (!System.Diagnostics.Debugger.IsAttached)
			{
				try
				{
					main();
				}
				catch (Exception ex)
				{
					// add catch for unknown application exceptions to try and get closer to bug
					StringBuilder capture = new StringBuilder();
					//
					Exception e = ex;
					while (e != null)
					{
						capture.Append(new StackTrace(e).ToString()).Append(Environment.NewLine);
						e = e.InnerException;
					}
					//
					string dir = Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
					File.WriteAllText(Path.Combine(dir, "winauth.log"), capture.ToString());

					throw;
				}
			}
			else
			{
				main();
			}
    }

		private static void main()
		{
			// Issue #53: set a default culture
			CultureInfo ci = new CultureInfo("en-US");
			System.Threading.Thread.CurrentThread.CurrentCulture = ci;
			System.Threading.Thread.CurrentThread.CurrentUICulture = ci;

			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new WinAuthForm());
		}
  }
}
