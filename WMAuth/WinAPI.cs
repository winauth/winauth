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
using System.ComponentModel;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class to wrapper underlying Windows calls
	/// </summary>
	public class WinAPI
	{
		/// <summary>
		/// Window API constants
		/// </summary>
		public const UInt32 SHDB_SHOW = 0x0001;
		public const UInt32 SHDB_HIDE = 0x0002;
		public const int GWL_STYLE = -16;
		public const UInt32 WS_NONAVDONEBUTTON = 0x00010000;

		/// <summary>
		/// Set Done button state
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="dwState"></param>
		/// <returns></returns>
		[DllImport("aygshell.dll")]
		private static extern bool SHDoneButton(IntPtr hWnd, UInt32 dwState);

		/// <summary>
		/// Set window paramter
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="nIndex"></param>
		/// <param name="dwNewLong"></param>
		/// <returns></returns>
		[DllImport("coredll.dll")]
		public static extern UInt32 SetWindowLong(IntPtr hWnd, int nIndex, UInt32 dwNewLong);

		/// <summary>
		/// Get window parameter
		/// </summary>
		/// <param name="hWnd"></param>
		/// <param name="nIndex"></param>
		/// <returns></returns>
		[DllImport("coredll.dll")]
		public static extern UInt32 GetWindowLong(IntPtr hWnd, int nIndex);

		/// <summary>
		/// Get the unqiue device ID
		/// </summary>
		/// <param name="appdata"></param>
		/// <param name="cbApplictionData"></param>
		/// <param name="dwDeviceIDVersion"></param>
		/// <param name="deviceIDOuput"></param>
		/// <param name="pcbDeviceIDOutput"></param>
		/// <returns></returns>
		[DllImport("coredll.dll")]
		private extern static int GetDeviceUniqueID([In, Out] byte[] appdata, int cbApplictionData, int dwDeviceIDVersion, [In, Out] byte[] deviceIDOuput, out uint pcbDeviceIDOutput);

		/// <summary>
		/// Hide the Done button for a window/form
		/// </summary>
		/// <param name="hWnd">window Handle</param>
		public static void HideDoneButton(IntPtr hWnd)
		{
			SHDoneButton(hWnd, SHDB_HIDE);
		}

		/// <summary>
		/// Hide the X button for a window/form
		/// </summary>
		/// <param name="hWnd">Window Handle</param>
		public static void HideXButton(IntPtr hWnd)
		{
			UInt32 dwStyle = GetWindowLong(hWnd, GWL_STYLE);
			if ((dwStyle & WS_NONAVDONEBUTTON) == 0)
			{
				SetWindowLong(hWnd, GWL_STYLE, dwStyle | WS_NONAVDONEBUTTON);
			}
		}

		/// <summary>
		/// Get the unique device ID hashed with an application Id
		/// </summary>
		/// <param name="appId">application Id</param>
		/// <returns>unqiue device ID or null is an error</returns>
		public static string GetDeviceID(string appId)
		{
			byte[] appIdData = System.Text.Encoding.Default.GetBytes(appId);
			byte[] deviceId = new byte[20];
			uint sizeOut = (uint)deviceId.Length;
			if (GetDeviceUniqueID(appIdData, appIdData.Length, 1, deviceId, out sizeOut) != 0)
			{
				return null;
			}
			else
			{
				return BitConverter.ToString(deviceId, 0, (int)sizeOut).Replace("-", "");
			}
		}

	}
}
