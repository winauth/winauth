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
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WinAuth
{
	/// <summary>
	/// A generic keyboard hook to find keys and send to the application. Used in WinAuth to set up a global hotkey
	/// so we can press ctrl-alt-L and have it send the code ... that's all, honest :)
	/// </summary>
	public class KeyboardHook : IDisposable
	{
		/// <summary>
		/// Native Windows calls
		/// </summary>
		[DllImport("user32.dll")]
		public static extern IntPtr SetWindowsHookEx(WinAPI.HookType code, KeyboardHookProc callback, IntPtr hInstance, int threadId);

		/// <summary>
		/// Our keyboard hook handle
		/// </summary>
		private IntPtr m_hook = IntPtr.Zero;

		/// <summary>
		/// Refernce to hook proc to stop garbage collection - stops working and you'll not know why
		/// </summary>
		private KeyboardHookProc m_hookedProc;

		/// <summary>
		/// Reference to async hook
		/// </summary>
		private KeyboardHookAsync m_hookedAsync;

		/// <summary>
		/// Normal hook deleage
		/// </summary>
		/// <param name="code"></param>
		/// <param name="wParam"></param>
		/// <param name="lParam"></param>
		/// <returns></returns>
		public delegate int KeyboardHookProc(int code, int wParam, ref WinAPI.KeyboardHookStruct lParam);

		/// <summary>
		/// Async hook delegate
		/// </summary>
		/// <param name="keyEvent"></param>
		/// <param name="e"></param>
		protected delegate void KeyboardHookAsync(int keyEvent, KeyboardHookEventArgs e);

		/// <summary>
		/// Hook event handler fired by the async trap
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public delegate void KeyboardHookEventHandler(object sender, KeyboardHookEventArgs e);

		/// <summary>
		/// User Event for picking up KeyDown
		/// </summary>
		public event KeyboardHookEventHandler KeyDown;

		/// <summary>
		/// User Event for pickup up KeyUp
		/// </summary>
		public event KeyboardHookEventHandler KeyUp;

		/// <summary>
		/// Our keys to watch for. If none in this list send nothing. We hold a key and associated Modifier
		/// </summary>
		private Dictionary<Tuple<Keys, WinAPI.KeyModifiers>, WinAuthAuthenticator> m_hookedKeys = new Dictionary<Tuple<Keys, WinAPI.KeyModifiers>, WinAuthAuthenticator>();

		/// <summary>
		/// Create out new KeyBoard hook for these keys
		/// </summary>
		/// <param name="keys"></param>
		public KeyboardHook(Dictionary<Tuple<Keys, WinAPI.KeyModifiers>, WinAuthAuthenticator> keys)
		{
			m_hookedKeys = keys;
			Hook();
		}

		/// <summary>
		/// Destructor, do cleanup
		/// </summary>
		~KeyboardHook()
		{
			Dispose();
		}

		/// <summary>
		/// Dispose and unhook
		/// </summary>
		public void Dispose()
		{
			UnHook();
		}

		/// <summary>
		/// Set up the hook into Windows
		/// </summary>
		protected void Hook()
		{
			if (m_hook == IntPtr.Zero)
			{
				// We have to store the HookProc, so that it is not garbage collected during runtime
				m_hookedProc = HookProc;

				// need instance handle to module to create a system-wide hook
				IntPtr instance = WinAPI.LoadLibrary("User32");
				m_hook = SetWindowsHookEx(WinAPI.HookType.WH_KEYBOARD_LL, m_hookedProc, instance, 0);
				m_hookedAsync = new KeyboardHookAsync(KeyCallback);
			}
		}

		/// <summary>
		/// Unhook from windows
		/// </summary>
		public void UnHook()
		{
			if (m_hook != IntPtr.Zero)
			{
				WinAPI.UnhookWindowsHookEx(m_hook);
				m_hook = IntPtr.Zero;

				// clear the forced references for GC
				m_hookedAsync = null;
				m_hookedProc = null;
			}
		}

		/// <summary>
		/// This is the main Windows hook callback.
		/// </summary>
		/// <param name="code">callback code</param>
		/// <param name="wParam">key code</param>
		/// <param name="lParam">key extra info</param>
		/// <returns>1 if event was handled</returns>
		[MethodImpl(MethodImplOptions.NoInlining)]
		public int HookProc(int code, int wParam, ref WinAPI.KeyboardHookStruct lParam)
		{
			// we only look for keyups and downs
			if (code >= 0 && (wParam == WinAPI.WM_KEYDOWN || wParam == WinAPI.WM_SYSKEYDOWN || wParam == WinAPI.WM_KEYUP || wParam == WinAPI.WM_SYSKEYUP))
			{
				// create out async handler and call it. if we needed to return "handled" chage this to ".Invoke()"
				KeyboardHookEventArgs kea = new KeyboardHookEventArgs((Keys)lParam.vkCode);
				if (KeyMatch(wParam, kea) == true)
				{
					m_hookedAsync.BeginInvoke(wParam, kea, null, null);
					return 1;
				}
			}

			return WinAPI.CallNextHookEx(m_hook, code, wParam, ref lParam);
		}

		/// <summary>
		/// Check if the key matches one of our hooked keys
		/// </summary>
		/// <param name="code">key code</param>
		/// <param name="kea">KeyboardHookEventArgs</param>
		/// <returns>true if there is a match</returns>
		private bool KeyMatch(int code, KeyboardHookEventArgs kea)
		{
			// key and modifers match?
			foreach (var e in m_hookedKeys)
			{
				if (e.Key.Item1 == kea.Key && e.Key.Item2  == kea.Modifiers)
				{
					return true;
				}
			}

			return false;
		}

		/// <summary>
		/// Entry point for our actuall Keyboard hook. Check the keys and call User event if neccessary.
		/// </summary>
		/// <param name="code">key code</param>
		/// <param name="kea">EventArgs holding modifiers</param>
		public void KeyCallback(int code, KeyboardHookEventArgs kea)
		{
			// key and modifers match?
			WinAuthAuthenticator match = null;
			foreach (var e in m_hookedKeys)
			{
				if (e.Key.Item1 == kea.Key && e.Key.Item2  == kea.Modifiers)
				{
					match = e.Value;
					break;
				}
			}
			//if (!m_hookedKeys.ContainsKey(kea.Key) || m_hookedKeys[kea.Key] != kea.Modifiers)
			if (match == null)
			{
				return;
			}
			kea.Authenticator = match;

			// call user events
			if ((code == WinAPI.WM_KEYDOWN || code == WinAPI.WM_SYSKEYDOWN) && KeyDown != null)
			{
				KeyDown(this, kea);
			}
			else if ((code == WinAPI.WM_KEYUP || code == WinAPI.WM_SYSKEYUP) && KeyUp != null)
			{
				KeyUp(this, kea);
			}
		}
	}

	/// <summary>
	/// Class to manage sending of keys to other applications.
	/// </summary>
	public class KeyboardSender
	{
		/// <summary>
		/// Hold the handle to the destination window
		/// </summary>
		private IntPtr m_hWnd;

		/// <summary>
		/// Create a new sender and get the window handle
		/// </summary>
		/// <param name="processName"></param>
		/// <param name="windowTitle"></param>
		public KeyboardSender(string windowtitle = null, string processname = null, bool useregex = false)
		{
			m_hWnd = FindWindow(windowtitle, processname, useregex);
		}

		/// <summary>
		/// Send the keys to the expected window or current window if null
		/// </summary>
		/// <param name="keys">stirng to send</param>
		public void SendKeys(WinAuthForm form, string keys, string code)
		{
			if (m_hWnd != IntPtr.Zero && m_hWnd != WinAPI.GetForegroundWindow())
			{
				WinAPI.SetForegroundWindow(m_hWnd);
				System.Threading.Thread.Sleep(50);
			}

			// wait until the control,shift,alt keys have been lifted
			while (WinAPI.GetKeyState((UInt16)WinAPI.VirtualKeyCode.VK_CONTROL) < 0 || WinAPI.GetKeyState((UInt16)WinAPI.VirtualKeyCode.VK_SHIFT) < 0 || WinAPI.GetKeyState((UInt16)WinAPI.VirtualKeyCode.VK_MENU) < 0)
			{
				Application.DoEvents();
				System.Threading.Thread.Sleep(50);
			}

			// replace any {CODE} items
			keys = Regex.Replace(keys, @"\{\s*CODE\s*\}", code, RegexOptions.Singleline);

			// clear events and stop input
			Application.DoEvents();
			bool blocked = WinAPI.BlockInput(true);
			try
			{
				// for now just split into parts and run each
				foreach (Match match in Regex.Matches(keys, @"\{.*?\}|[^\{]*", RegexOptions.Singleline))
				{
					// split into either {CMD d w} or just plain text
					if (match.Success == true)
					{
						string single = match.Value;
						if (single.Length == 0)
						{
							continue;
						}

						if (single[0] == '{')
						{
							// send command {COMMAND delay repeat}
							Match cmdMatch = Regex.Match(single.Trim(), @"\{([^\s]+)\s*(\d*)\s*(\d*)\}");
							if (cmdMatch.Success == true)
							{
								// extract the command and any optional delay and repeat
								string cmd = cmdMatch.Groups[1].Value.ToUpper();
								int delay = 0;
								if (cmdMatch.Groups[2].Success == true && cmdMatch.Groups[2].Value.Length != 0)
								{
									int.TryParse(cmdMatch.Groups[2].Value, out delay);
								}
								int repeat = 1;
								if (cmdMatch.Groups[3].Success == true && cmdMatch.Groups[3].Value.Length != 0)
								{
									int.TryParse(cmdMatch.Groups[3].Value, out repeat);
								}
								// run the command
								switch (cmd)
								{
									case "TAB":
										SendKey('\t', delay, repeat);
										break;
									case "ENTER":
										SendKey('\n', delay, repeat);
										break;
									case "WAIT":
										for (; repeat > 0; repeat--)
										{
											System.Threading.Thread.Sleep(delay);
										}
										break;
									case "COPY":
										form.Invoke(new WinAuthForm.SetClipboardDataDelegate(form.SetClipboardData), new object[] { code });
										break;
									case "PASTE":
										string clipboard = form.Invoke(new WinAuthForm.GetClipboardDataDelegate(form.GetClipboardData), new object[] { typeof(string) }) as string;
										if (string.IsNullOrEmpty(clipboard) == false)
										{
											foreach (char key in clipboard)
											{
												SendKey(key);
											}
										}
										break;
									case "EXIT":
										Application.Exit();
										break;
									default:
										//Enum.Parse(typeof(WinAPI.VirtualKeyCode), "VK_" + cmd, true);
										//SendKey('\t', delay, repeat);
										break;
								}
							}
						}
						else
						{
							// plain text - send verbatim
							//foreach (char key in single)
							//{
							//  SendKey(key);
							//}
							SendKey(single);
						}
					}
				}
			}
			finally
			{
				// resume input
				if (blocked == true)
				{
					WinAPI.BlockInput(false);
				}
				Application.DoEvents();
			}
		}

		/// <summary>
		/// Send a single key to the current window
		/// </summary>
		/// <param name="key">key to send</param>
		private void SendKey(char key)
		{
			SendKey(key, 0, 1);
		}

		/// <summary>
		/// Send a key string to the current window
		/// </summary>
		/// <param name="key">key string to send</param>
		private void SendKey(string key)
		{
			SendKey(key, 0, 1);
		}

		/// <summary>
		/// Send a key string to the current window a number of times with a delay after each key
		/// </summary>
		/// <param name="key">key string to send</param>
		/// <param name="delay">delay in millisecs after each keypress</param>
		/// <param name="repeat">number of times</param>
		private void SendKey(string key, int delay, int repeat)
		{
			// escape any special codes for SendKeys
			key = Regex.Replace(key, @"([()+\^%~{}])", "{$1}", RegexOptions.Multiline);
			for (; repeat > 0; repeat--)
			{
				System.Windows.Forms.SendKeys.SendWait(key);
				System.Threading.Thread.Sleep(delay != 0 ? delay : 50);
			}
			System.Windows.Forms.SendKeys.Flush();
		}

		/// <summary>
		/// Send a single key to the current window a number of times with a delay after each key
		/// </summary>
		/// <param name="key">key to send</param>
		/// <param name="delay">delay in millisecs after each keypress</param>
		/// <param name="repeat">number of times</param>
		private void SendKey(char key, int delay, int repeat)
		{
			SendKey(key.ToString(), delay, repeat);
		}

		/// <summary>
		/// Find a window for the give process and/or title
		/// </summary>
		/// <param name="processName">name of process</param>
		/// <param name="windowTitle">text of window title</param>
		/// <returns>Window handle if we can match the process and/or title</returns>
		private static IntPtr FindWindow(string windowTitle, string processName, bool useregex)
		{
			// get specific processes or all
			Process[] processes = (string.IsNullOrEmpty(processName) == false ? Process.GetProcessesByName(processName) : Process.GetProcesses());

			// if we have a title match it in the window titles
			if (string.IsNullOrEmpty(windowTitle) == false)
			{
				if (useregex == true)
				{
					Regex regex = new Regex(windowTitle, RegexOptions.Singleline);
					foreach (Process process in processes)
					{
						if (regex.IsMatch(process.MainWindowTitle) == true)
						{
							return process.MainWindowHandle;
						}
					}
				}
				else
				{
					string lowerWindowTitle = (windowTitle != null ? windowTitle.ToLower() : null);
					foreach (Process process in processes)
					{
						if (process.MainWindowTitle.ToLower().IndexOf(lowerWindowTitle) != -1)
						{
							return process.MainWindowHandle;
						}
					}
				}
			}
			else if (string.IsNullOrEmpty(processName) == false)
			{
				foreach (Process process in processes)
				{
					if (string.Compare(process.ProcessName, processName, true) == 0)
					{
						return process.MainWindowHandle;
					}
				}
			}
			else if (string.IsNullOrEmpty(processName) == true)
			{
				return WinAPI.GetForegroundWindow();
			}
			else if (processes.Length != 0)
			{
				return processes[0].MainWindowHandle;
			}

			// didn't find anything
			return IntPtr.Zero;
		}
	}

	/// <summary>
	/// Event args used to hold key and modifiers for keyboard hook
	/// </summary>
	public class KeyboardHookEventArgs : EventArgs
	{
		/// <summary>
		/// Currnet key
		/// </summary>
		public Keys Key;

		/// <summary>
		/// Applied authenticator
		/// </summary>
		public WinAuthAuthenticator Authenticator;

		/// <summary>
		/// Combined modifiers
		/// </summary>
		public WinAPI.KeyModifiers Modifiers;

		/// <summary>
		/// Flag for Alt key
		/// </summary>
		public bool Alt;

		/// <summary>
		/// Flag for Ctrl key
		/// </summary>
		public bool Control;

		/// <summary>
		/// Flag for Shift key
		/// </summary>
		public bool Shift;

		/// <summary>
		/// Flag for event has been handled
		/// </summary>
		public bool Handled;

		/// <summary>
		/// Create a new EventArg for the key
		/// </summary>
		/// <param name="keyCode"></param>
		public KeyboardHookEventArgs(Keys keyCode)
		{
			this.Key = (Keys)keyCode;
			try
			{
				// we have to use Windows Form to get the modifiers as won't work if no focus 
				this.Alt = (System.Windows.Forms.Control.ModifierKeys & Keys.Alt) != 0;
				this.Control = (System.Windows.Forms.Control.ModifierKeys & Keys.Control) != 0;
				this.Shift = (System.Windows.Forms.Control.ModifierKeys & Keys.Shift) != 0;

				// combine the modifiers
				Modifiers = WinAPI.KeyModifiers.None;
				if (this.Alt)
				{
					Modifiers |= WinAPI.KeyModifiers.Alt;
				}
				if (this.Control)
				{
					Modifiers |= WinAPI.KeyModifiers.Control;
				}
				if (this.Shift)
				{
					Modifiers |= WinAPI.KeyModifiers.Shift;
				}
			}
			catch (Exception) { }
		}
	}


}
