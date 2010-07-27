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
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class that launches the main form
	/// </summary>
	static class WinAuth
	{
		/// <summary>
		/// Name of this application used for %USEPATH%\[name] folder
		/// </summary>
		public const string APPLICATION_NAME = "Windows Authenticator";

		/// <summary>
		/// Name of application config file
		/// </summary>
		public const string DEFAULT_CONFIG_FILE_NAME = "winauth.xml";

		/// <summary>
		/// Name of default authenticator file
		/// </summary>
		public const string DEFAULT_AUTHENTICATOR_FILE_NAME = "authenticator.xml";

		/// <summary>
		/// The main entry point for the application.
		/// </summary>
		[STAThread]
		static void Main()
		{
			Application.EnableVisualStyles();
			Application.SetCompatibleTextRenderingDefault(false);
			Application.Run(new MainForm());
		}
	}
}
