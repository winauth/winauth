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
using System.Text;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class holding configuration data for application
	/// </summary>
	public class WinAuthConfig
	{
		/// <summary>
		/// Get/set name of current authentication data file
		/// </summary>
		public string AuthenticatorFile { get; set; }

		/// <summary>
		/// Get/set auto refresh flag
		/// </summary>
		public bool AutoRefresh { get; set; }

		/// <summary>
		/// Get/set on top flag
		/// </summary>
		public bool AlwaysOnTop { get; set; }

		/// <summary>
		/// Get/set auto copy flag
		/// </summary>
		public bool CopyOnCode { get; set; }

		/// <summary>
		/// Get/set hide serial flag
		/// </summary>
		public bool HideSerial { get; set; }

		/// <summary>
		/// Create a default config object
		/// </summary>
		public WinAuthConfig()
		{
			AutoRefresh = true;
			AlwaysOnTop = true;
		}
	}

}
