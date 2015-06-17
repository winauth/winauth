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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Reflection;
using System.Runtime.ConstrainedExecution;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Text;
using System.Security.Cryptography;

namespace WinAuth
{
	/// <summary>
	/// Class wrapping API around pinvoke YubiKey DLL
	/// </summary>
	public class YubiKey
	{
		/// <summary>
		/// USB Device Vendor and Product IDs for YubiKeys
		/// </summary>
		public const int VENDOR_ID = 0x1050;	/* Global vendor ID */
		public const int YUBIKEY_PID = 0x0010;	/* Yubikey (version 1 and 2) */
		public const int NEO_OTP_PID = 0x0110;	/* Yubikey NEO - OTP only */
		public const int NEO_OTP_CCID_PID = 0x0111;	/* Yubikey NEO - OTP and CCID */
		public const int NEO_CCID_PID = 0x0112;	/* Yubikey NEO - CCID only */
		public const int NEO_U2F_PID = 0x0113;	/* Yubikey NEO - U2F only */
		public const int NEO_OTP_U2F_PID = 0x0114;	/* Yubikey NEO - OTP and U2F */
		public const int NEO_U2F_CCID_PID = 0x0115;	/* Yubikey NEO - U2F and CCID */
		public const int NEO_OTP_U2F_CCID_PID = 0x0116;	/* Yubikey NEO - OTP, U2F and CCID */
		public const int YK4_OTP_PID = 0x0401;	/* Yubikey 4 - OTP only */
		public const int YK4_U2F_PID = 0x0402;	/* Yubikey 4 - U2F only */
		public const int YK4_OTP_U2F_PID = 0x0403;	/* Yubikey 4 - OTP and U2F */
		public const int YK4_CCID_PID = 0x0404;	/* Yubikey 4 - CCID only */
		public const int YK4_OTP_CCID_PID = 0x0405;	/* Yubikey 4 - OTP and CCID */
		public const int YK4_U2F_CCID_PID = 0x0406;	/* Yubikey 4 - U2F and CCID */
		public const int YK4_OTP_U2F_CCID_PID = 0x0407;	/* Yubikey 4 - OTP, U2F and CCID */
		public const int PLUS_U2F_OTP_PID = 0x0410;	/* Yubikey plus - OTP+U2F */

		/// <summary>
		/// Array of valid product IDs
		/// </summary>
		public static int[] DEVICE_IDS = new int[] {
			YUBIKEY_PID,
			NEO_OTP_PID,
			NEO_OTP_CCID_PID,
			NEO_CCID_PID,
			NEO_U2F_PID,
			NEO_OTP_U2F_PID,
			NEO_U2F_CCID_PID,
			NEO_OTP_U2F_CCID_PID,
			YK4_OTP_PID,
			YK4_U2F_PID,
			YK4_OTP_U2F_PID,
			YK4_CCID_PID,
			YK4_OTP_CCID_PID,
			YK4_U2F_CCID_PID,
			YK4_OTP_U2F_CCID_PID,
			PLUS_U2F_OTP_PID
		};

		/// <summary>
		/// USDB device GUID for keyboard
		/// </summary>
		public static Guid GUID_DEVINTERFACE_KEYBOARD = new Guid("884b96c3-56ef-11d1-bc8c-00a0c91405dd");

		/// <summary>
		/// Name of AppData folder
		/// </summary>
		public const string APPDATAFOLDER = "WinAuth";

		/// <summary>
		/// Name of our native x86 YubiKey DLL
		/// </summary>
		private const string YUBI_LIBRARY_NAME_X86 = "WinAuth.YubiKey.x86.dll";

		/// <summary>
		/// Name of our native x64 YubiKey DLL
		/// </summary>
		private const string YUBI_LIBRARY_NAME_X64 = "WinAuth.YubiKey.x64.dll";

		/// <summary>
		/// Download Website for YubiKey DLLs
		/// </summary>
		private const string YUBI_DOWNLOAD_WEBSITE = "https://winauth.com/";

		/// <summary>
		/// Download URL for YubiKey DLLs
		/// </summary>
		private const string YUBI_DOWNLOAD_BASEURL = "https://winauth.com/downloads/3.x/";

		/// <summary>
		/// Fix Status stuct returned from Yubkey DLLs
		/// </summary>
		public struct STATUS
		{
			public byte VersionMajor;	/* Firmware version information */
			public byte VersionMinor;
			public byte VersionBuild;
			public byte PgmSeq;		/* Programming sequence number. 0 if no valid configuration */
			public UInt16 TouchLevel;	/* Level from touch detector */
		};
		public struct INFO
		{
			public STATUS Status;
			public UInt32 Serial;
			public UInt32 Pid;
			public string Error;
		};

		/// <summary>
		/// Handle to loaded pinvoke library
		/// </summary>
		private IntPtr _library;

		/// <summary>
		/// Current loading Task
		/// </summary>
		public Task Loading { get; set; }

		/// <summary>
		/// Current YubiKey info
		/// </summary>
		public INFO Info { get; set; }

		/// <summary>
		/// Native calls
		/// </summary>
		[DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern IntPtr LoadLibrary(string libname);
		[DllImport("kernel32.dll", CharSet = CharSet.Auto)]
		[return: MarshalAs(UnmanagedType.Bool)]
		private static extern bool FreeLibrary(IntPtr hModule);
		[DllImport("kernel32.dll", CharSet = CharSet.Ansi)]
		private static extern IntPtr GetProcAddress(IntPtr hModule, string lpProcName);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int LastErrorDelegate();
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate bool IsInsertedDelegate();
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int GetInfoDelegate([MarshalAs(UnmanagedType.Struct)] out INFO status);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int SetChallengeResponseDelegate(
			[MarshalAs(UnmanagedType.U4)] int slot,
			byte[] secret,
			[MarshalAs(UnmanagedType.U4)] int keysize,
			[MarshalAs(UnmanagedType.Bool)] bool userpress,
			byte[] access_code);
		[UnmanagedFunctionPointer(CallingConvention.Cdecl)]
		delegate int ChallengeResponseDelegate(
			[MarshalAs(UnmanagedType.U4)] int slot,
			[MarshalAs(UnmanagedType.Bool)] bool may_block,
			byte[] challenge,
			[MarshalAs(UnmanagedType.U4)] int challenge_len,
			byte[] response,
			[MarshalAs(UnmanagedType.U4)] int response_len);

		/// <summary>
		/// Create the YubiKey instance
		/// </summary>
		public YubiKey()
		{
			// load library and load info
			Loading = LoadLibrary().ContinueWith((loaded) =>
			{
				INFO info = new INFO();
				if (loaded.Result != null)
				{
					info.Error = loaded.Result.Message;
					Info = info;
					return;
				}

				GetInfoDelegate f = GetFunction<GetInfoDelegate>("GetInfo");
				int ret = f(out info);
				if (ret > 1)
				{
					info.Error = string.Format("Error {0}", ret);
				}
				Info = info;
			});
		}

		/// <summary>
		/// Kludge for .net 4.0 to return immediate task result
		/// </summary>
		/// <typeparam name="TResult"></typeparam>
		/// <param name="result"></param>
		/// <returns></returns>
		public static Task<TResult> FromResult<TResult>(TResult result)
		{
			var completionSource = new TaskCompletionSource<TResult>();
			completionSource.SetResult(result);
			return completionSource.Task;
		}

		/// <summary>
		/// Load the 32 or 64 bit native YubiKey DLL depending on current platform, or download from winauth servers
		/// </summary>
		/// <param name="downloadIfNeeded">option to download DLL if required</param>
		/// <returns>Task when DLL is loaded</returns>
		private Task<Exception> LoadLibrary(bool downloadIfNeeded = true)
		{
			if (_library != IntPtr.Zero)
			{
				return FromResult<Exception>(null);
			}

			// load the library
			var name = (IntPtr.Size == 4 ? YUBI_LIBRARY_NAME_X86 : YUBI_LIBRARY_NAME_X64);

			// get the current exe path
			var exedir = Path.GetDirectoryName(new Uri(System.Reflection.Assembly.GetExecutingAssembly().CodeBase).AbsolutePath);

			// check if we can find the library in the current path, appdata and %path%
			string path = Environment.CurrentDirectory;
			FileInfo fi = new FileInfo(Path.Combine(path, name));
			if (fi.Exists == false)
			{
				// look in the exe's folder
				fi = new FileInfo(Path.Combine(exedir, name));
			}
			if (fi.Exists == false)
			{
				// look in the config folder
				path = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPDATAFOLDER);
				fi = new FileInfo(Path.Combine(path, name));
			}
			if (fi.Exists == false)
			{
				// check the path
				var paths = Environment.GetEnvironmentVariable("PATH");
				foreach (var p in paths.Split(';'))
				{
					fi = new FileInfo(Path.Combine(p, name));
					if (fi.Exists == true)
					{
						path = p;
						break;
					}
				}
			}
			// if we found it update the name to be the full name
			if (fi.Exists == true)
			{
				name = Path.Combine(path, name);
			}
			else if (Directory.Exists(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData)) == true)
			{
				// we will put in the AppData folder
				name = Path.Combine(Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), APPDATAFOLDER), name);
			}
			else
			{
				// we will put in the exe folder
				name = Path.Combine(exedir, name);
			}

			// load the library
			_library = LoadLibrary(name);
			if (_library == IntPtr.Zero)
			{
				int error = Marshal.GetLastWin32Error();
				if (downloadIfNeeded == false)
				{
					return FromResult<Exception>(new LibraryNotFoundException("Download the YubiKey AddOn from " + YUBI_DOWNLOAD_WEBSITE));
				}

				var task = Task.Factory.StartNew<Exception>(() =>
				{
					try
					{
						using (WebClient web = new WebClient())
						{
							web.DownloadFile(YUBI_DOWNLOAD_BASEURL + (IntPtr.Size == 4 ? YUBI_LIBRARY_NAME_X86 : YUBI_LIBRARY_NAME_X64), name);
							return LoadLibrary(false).Result;
						}
					}
					catch (WebException ex)
					{
						return new ApplicationException("Download the YubiKey AddOn from " + YUBI_DOWNLOAD_WEBSITE, ex);
					}
				});

				return task;
			}

			return FromResult<Exception>(null);
		}

		/// <summary>
		/// Dynamically get the function point to a native DLL entry point
		/// </summary>
		/// <typeparam name="TDelegate">delegate we require</typeparam>
		/// <param name="name">name of function</param>
		/// <returns>function delegate</returns>
		private TDelegate GetFunction<TDelegate>(string name) where TDelegate : class
		{
			IntPtr p = GetProcAddress(_library, name);
			if (p == IntPtr.Zero)
			{
				return null;
			}

			Delegate f = Marshal.GetDelegateForFunctionPointer(p, typeof(TDelegate));

			object obj = f;

			return (TDelegate)obj;
		}

		/// <summary>
		/// Get the last error from the native DLL
		/// </summary>
		/// <returns>last error code</returns>
		public int LastError()
		{
			LastErrorDelegate fe = GetFunction<LastErrorDelegate>("LastError");
			return fe();
		}

		/// <summary>
		/// Set the Challenge/Response for the Yubikey
		/// </summary>
		/// <param name="slot">slot number, only 1 or 2</param>
		/// <param name="key">20 byte key</param>
		/// <param name="keysize">size of key - currently must be 20 bytes</param>
		/// <param name="press">if we require a keypress</param>
		/// <param name="accesscode">current access code if required</param>
		public void SetChallengeResponse(int slot, byte[] key, int keysize, bool press, byte[] accesscode = null)
		{
			var loaded = LoadLibrary(false);
			if (loaded.Result != null)
			{
				throw loaded.Result;
			}

			SetChallengeResponseDelegate f = GetFunction<SetChallengeResponseDelegate>("SetChallengeResponse");
			int ret = f(slot, key, keysize, press, accesscode);
			if (ret != 0)
			{
				throw new CannotSetChallengeResponseException(string.Format("Cannot set ChallengeResponse. Error {0}:{1}.", ret, LastError()));
			}
		}

		/// <summary>
		/// Perform the Challenge/Response with the current data
		/// </summary>
		/// <param name="slot">slot number, 1 or 2</param>
		/// <param name="challenge">challenge array, up to 64 bytes</param>
		/// <param name="allowBlock">if we are allow to block (i.e. if keypress = true)</param>
		/// <param name="hash">to save returned hash</param>
		/// <returns>hash result of challengeresponse</returns>
		public byte[] ChallengeResponse(int slot, byte[] challenge, bool allowBlock = true, byte[] hash = null)
		{
			var loaded = LoadLibrary(false);
			if (loaded.Result != null)
			{
				throw loaded.Result;
			}

			byte[] maxhash = new byte[64];
			ChallengeResponseDelegate f = GetFunction<ChallengeResponseDelegate>("ChallengeResponse");
			int hashret = f(slot, allowBlock, challenge, challenge.Length, maxhash, maxhash.Length);
			if (hashret != 0) 
			{
				int lasterror = LastError();
				throw new ChallengeResponseException(string.Format("Cannot perform ChallengeResponse. Error {0}:{1}.", hashret, lasterror));
			}

			if (hash == null)
			{
				hash = new byte[20];
			}
			Array.Copy(maxhash, 0, hash, 0, hash.Length);

			return hash;
		}
	}

	/// <summary>
	/// General YubiKey exception
	/// </summary>
	public class YubKeyException : ApplicationException
	{
		public YubKeyException(string msg = null, Exception ex = null) : base(msg, ex) { }
	}

	/// <summary>
	/// Exception if the native library cannot be loaded
	/// </summary>
	public class LibraryNotFoundException : YubKeyException
	{
		public LibraryNotFoundException(string msg = null, Exception ex = null) : base(msg, ex) { }
	}

	/// <summary>
	/// Exception if the Challenge/Response cannot be set
	/// </summary>
	public class CannotSetChallengeResponseException : YubKeyException
	{
		public CannotSetChallengeResponseException(string msg = null, Exception ex = null) : base(msg, ex) { }
	}

	/// <summary>
	/// Exception if the ChallengeResponse cannot be executed
	/// </summary>
	public class ChallengeResponseException : YubKeyException
	{
		public ChallengeResponseException(string msg = null, Exception ex = null) : base(msg, ex) { }
	}
}
