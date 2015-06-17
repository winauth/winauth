/*
 * Copyright (C) 2011 Colin Mackie.
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
using System.Linq;
using System.Net;
using System.Security.Cryptography;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;
using System.Xml.Serialization;

namespace WinAuth
{
	/// <summary>
	/// Store the current YubiKey data and seed from original decryption. This is so
	/// we can update the config file without requiring the user key press (if enabled)
	/// from the yubi. Although DP is used, pretty pointless since it is put into managed
	/// memory anyway.
	/// </summary>
	public class YubikeyData
	{
		/// <summary>
		/// Length of key
		/// </summary>
		public int Length;

		/// <summary>
		/// Random seed
		/// </summary>
		public string Seed;

		/// <summary>
		/// Protected data block
		/// </summary>
		private byte[] _data;

		/// <summary>
		/// Get/set the protected data into a managed array
		/// </summary>
		public byte[] Data
		{
			get
			{
				if (Length == 0)
				{
					return null;
				}

				byte[] b = new byte[_data.Length];
				Array.Copy(_data, b, _data.Length);
				return ProtectedData.Unprotect(b, null, DataProtectionScope.CurrentUser);
			}
			set
			{
				if (value == null)
				{
					_data = null;
					Length = 0;
				}
				else
				{
					Length = value.Length;
					byte[] b = new byte[value.Length];
					Array.Copy(value, b, value.Length);
					_data = ProtectedData.Protect(b, null, DataProtectionScope.CurrentUser);
				}
			}
		}
	}
}
