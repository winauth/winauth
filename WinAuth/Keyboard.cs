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
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

using WindowsInput;

namespace WinAuth
{
	/// <summary>
	/// A generic keyboard hook to find keys and send to the application. Used in WinAuth to set up a global hotkey
	/// so we can press ctrl-alt-L and have it send the code ... that's all, honest :)
	/// </summary>
	public class KeyboardHook : IDisposable
	{
		/// <summary>
		/// Owning form for hotkey
		/// </summary>
		private Form m_form;

		/// <summary>
		/// Our keyboard hook handle
		/// </summary>
		private IntPtr m_hook = IntPtr.Zero;

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
		public event KeyboardHookEventHandler KeyPressed;

		/// <summary>
		/// Our keys to watch for. If none in this list send nothing. We hold a key and associated Modifier
		/// </summary>
		private List<WinAuthAuthenticator> m_hooked = new List<WinAuthAuthenticator>();

		/// <summary>
		/// Create out new KeyBoard hook for these keys
		/// </summary>
		/// <param name="keys"></param>
		public KeyboardHook(Form form, List<WinAuthAuthenticator> hookedauths)
		{
			m_form = form;
			m_hooked = hookedauths;
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
			for (int i=0; i<m_hooked.Count; i++)
			{
				WinAuthAuthenticator auth = m_hooked[i];
				Keys key = (Keys)auth.HotKey.Key;
				WinAPI.KeyModifiers modifier = auth.HotKey.Modifiers;

				if (WinAPI.RegisterHotKey(m_form.Handle, i + 1, modifier | WinAPI.KeyModifiers.NoRepeat, key) == false)
				{
					// the MOD_NOREPEAT flag is not support in XP or 2003 and we should get a fail, so we call again without it
					WinAPI.RegisterHotKey(m_form.Handle, i + 1, modifier, key);
				}
			}
		}

		/// <summary>
		/// Unhook from windows
		/// </summary>
		public void UnHook()
		{
			for (int i=0; i<m_hooked.Count; i++)
			{
				WinAPI.UnregisterHotKey(m_form.Handle, i+1);
			}
			m_hooked.Clear();
		}

		/// <summary>
		/// Check if a specified Hotkey is available
		/// </summary>
		/// <param name="form">owning form</param>
		/// <param name="key">hot key Key</param>
		/// <param name="modifier">hoy key Modifier</param>
		/// <returns>true if available</returns>
		public static bool IsHotkeyAvailable(Form form, Keys key, WinAPI.KeyModifiers modifier)
		{
			bool available = WinAPI.RegisterHotKey(form.Handle, 0, modifier, key);
			if (available == true)
			{
				WinAPI.UnregisterHotKey(form.Handle, 0);
			}
			return available;
		}

		/// <summary>
		/// Callback called by Form to initaite press of hotkey
		/// </summary>
		/// <param name="kea">KeyboardHookEventArgs event args</param>
		public void KeyCallback(KeyboardHookEventArgs kea)
		{
			// key and modifers match?
			WinAuthAuthenticator match = null;
			foreach (var auth in m_hooked)
			{
				if ((Keys)auth.HotKey.Key == kea.Key && auth.HotKey.Modifiers == kea.Modifiers)
				{
					match = auth;
					break;
				}
			}
			if (match == null)
			{
				return;
			}
			kea.Authenticator = match;

			// call user events
			if (KeyPressed != null)
			{
				KeyPressed(this, kea);
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
		public KeyboardSender(string window)
		{
			m_hWnd = FindWindow(window);
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
									case "BS":
										SendKey('\x08', delay, repeat);
										break;
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
										break;
								}
							}
						}
						else
						{
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
			for (; repeat > 0; repeat--)
			{
				// Issue#100: change to use InputSimulator as SendKeys does not work for internation keyboards
				InputSimulator.SimulateTextEntry(key);
				System.Threading.Thread.Sleep(delay != 0 ? delay : 50);
			}
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
		/// <param name="window">text of window title or process name</param>
		/// <returns>Window handle if we can match the process and/or title</returns>
		private static IntPtr FindWindow(string window)
		{
			// default to return current window
			if (string.IsNullOrEmpty(window) == true)
			{
				return WinAPI.GetForegroundWindow();
			}

			// build regex
			Regex reg;
			Match match = Regex.Match(window, @"/(.*)/([a-z]*)", RegexOptions.IgnoreCase);
			if (match.Success == true)
			{
				RegexOptions regoptions = RegexOptions.None;
				if (match.Groups[2].Value.Contains("i") == true)
				{
					regoptions |= RegexOptions.IgnoreCase;
				}
				reg = new Regex(match.Groups[1].Value, regoptions);
			}
			else
			{
				reg = new Regex(Regex.Escape(window), RegexOptions.IgnoreCase);
			}


			// find process matches
			List<Process> matchingProcesses = new List<Process>();
			foreach (var process in Process.GetProcesses())
			{
				if (reg.IsMatch(process.ProcessName) || reg.IsMatch(process.MainWindowTitle))
				{
					matchingProcesses.Add(process);
				}
			}

			// return first match or zero
			return (matchingProcesses.Count != 0 ? matchingProcesses[0].MainWindowHandle : IntPtr.Zero);

/*
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
*/
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
		/// Create a new KeyboardHookEventArgs for a key and modifier
		/// </summary>
		/// <param name="key"></param>
		/// <param name="modifier"></param>
		public KeyboardHookEventArgs(Keys key, WinAPI.KeyModifiers modifier)
		{
			this.Key = key;
			this.Modifiers = modifier;

			this.Alt = (modifier & WinAPI.KeyModifiers.Alt) != 0;
			this.Control = (modifier & WinAPI.KeyModifiers.Control) != 0;
			this.Shift = (modifier & WinAPI.KeyModifiers.Shift) != 0;
		}

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
