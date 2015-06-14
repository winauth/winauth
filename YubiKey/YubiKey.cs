/*
 * Copyright (C) 2015	 Colin Mackie.
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
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;

using Microsoft.Win32.SafeHandles;
using System.Net;

namespace WinAuth
{
	/// <summary>
	/// Class holding the various Windows API consts and extern function
	/// </summary>
	public class YubiKey
	{
		/// <summary>
		/// Name of our native x86 YubiKey DLL
		/// </summary>
		private const string YUBI_LIBRARY_NAME_X86 = "yubikeyx86.dll";

		/// <summary>
		/// Name of our native x64 YubiKey DLL
		/// </summary>
		private const string YUBI_LIBRARY_NAME_X64 = "yubikeyx64.dll";

		private const string YUBI_DOWNLOAD_URL = "https://winauth.com/downloads/3.x/";

		//[Serializable]
		//[StructLayout(LayoutKind.Explicit)]
		public struct STATUS
		{
			public byte VersionMajor;	/* Firmware version information */
			public byte VersionMinor;
			public byte VersionBuild;
			public byte PgmSeq;		/* Programming sequence number. 0 if no valid configuration */
			public UInt16 TouchLevel;	/* Level from touch detector */
		};

		/*
				/// <summary>
				/// Structure used to pass for keyboard hooks
				/// </summary>
				public struct KeyboardHookStruct
				{
					public UInt32 vkCode;
					public UInt32 scanCode;
					public UInt32 flags;
					public UInt32 time;
					public IntPtr dwExtraInfo;
				}

				public enum KeyboardFlag : uint
				{
					ExtendedKey = 0x0001,
					KeyUp = 0x0002,
					Unicode = 0x0004,
					ScanCode = 0x0008,
				}

				[Serializable]
				[StructLayout(LayoutKind.Explicit)]
				public struct MOUSEKEYBDHARDWAREINPUT
				{
					[FieldOffset(0)]
					public MOUSEINPUT Mouse;
					[FieldOffset(0)]
					public KEYBDINPUT Keyboard;
					[FieldOffset(0)]
					public HARDWAREINPUT Hardware;
				}
				public struct INPUT
				{
					public UInt32 Type;
					public MOUSEKEYBDHARDWAREINPUT Data;
				}

				[Serializable]
				[StructLayout(LayoutKind.Sequential)]
				public struct WINDOWPLACEMENT
				{
					public int length;
					public int flags;
					public ShowWindowCommands showCmd;
					public System.Drawing.Point ptMinPosition;
					public System.Drawing.Point ptMaxPosition;
					public System.Drawing.Rectangle rcNormalPosition;
				}
		*/

		/// <summary>
		/// Native calls
		/// </summary>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern SafeFileHandle LoadLibrary(string libname);

		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary(IntPtr hModule);

		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		private static extern SafeFileHandle GetProcAddress(SafeFileHandle hModule, string lpProcName);

		private static SafeFileHandle _library;

		private static SafeFileHandle LoadLibrary()
		{
			// load the library
			if (_library == null || _library.IsInvalid)
			{
				var name = (IntPtr.Size == 4 ? YUBI_LIBRARY_NAME_X86 : YUBI_LIBRARY_NAME_X64);

				// check if we can find the library
				string path = Assembly.GetExecutingAssembly().Location;
				var fi = new FileInfo(Path.Combine(path, name));
				if (fi.Exists == false)
				{
				}

				_library = LoadLibrary(YUBI_LIBRARY_NAME);
				if (_library.IsInvalid)
				{
					int error = Marshal.GetLastWin32Error();

					throw new LibraryNotFoundException("Download YubiKey AddOn from " + YUBI_DOWNLOAD_URL);
					using (WebClient web = new WebClient())
					{
						//web.DownloadFile("
					}

					throw new NotImplementedException("Cannot load YUBIKEY library (" + error + ")");
				}
			}

			return _library;
		}

		private static TDelegate GetFunction<TDelegate>(string name) where TDelegate : class
		{
			var lib = LoadLibrary();
			SafeFileHandle p = GetProcAddress(lib, name);
			if (p.IsInvalid)
			{
				return null;
			}

			Delegate f = Marshal.GetDelegateForFunctionPointer(p.DangerousGetHandle(), typeof(TDelegate));

			object obj = f;

			return (TDelegate)obj;
		}

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate bool IsInsertedDelegate();

		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int GetStatusDelegate([MarshalAs(UnmanagedType.Struct)] out STATUS status);

		public static STATUS GetStatus()
		{
			STATUS status = new STATUS();

			GetStatusDelegate f = GetFunction<GetStatusDelegate>("GetStatus");
			int ret = f(out status);
			return status;
		}

		/// <summary>
		/// Initialise the Library and load the appropriate x86/x64 DLL
		/// </summary>
		public void Init()
		{
		}
	}

	public class LibraryNotFoundException : ApplicationException
	{
		public LibraryNotFoundException(string msg = null, Exception ex = null) : base(msg, ex) { }
	}
}
