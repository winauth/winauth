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
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.CompilerServices;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// A generic keyboard hook to find keys and send to the application. Used in WinAuth to set up a global hotkey ... that's all, honest :)
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
		private Dictionary<Keys, WinAPI.KeyModifiers> m_hookedKeys = new Dictionary<Keys, WinAPI.KeyModifiers>();

		/// <summary>
		/// Create out new KeyBoard hook for these keys
		/// </summary>
		/// <param name="keys"></param>
		public KeyboardHook(Dictionary<Keys, WinAPI.KeyModifiers> keys)
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
				m_hookedAsync.BeginInvoke(wParam, kea, null, null);
			}

			return WinAPI.CallNextHookEx(m_hook, code, wParam, ref lParam);
		}

		/// <summary>
		/// Entry point for our actuall Keyboard hook. Check the keys and call User event if neccessary.
		/// </summary>
		/// <param name="code">key code</param>
		/// <param name="kea">EventArgs holding modifiers</param>
		public void KeyCallback(int code, KeyboardHookEventArgs kea)
		{
			// key and modifers match?
			if (!m_hookedKeys.ContainsKey(kea.Key) || m_hookedKeys[kea.Key] != kea.Modifiers)
			{
				return;
			}

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
		public KeyboardSender(string processName, string windowTitle)
		{
			m_hWnd = FindWindow(processName, windowTitle);
		}

		/// <summary>
		/// Send the keys to the expected window or current window if null
		/// </summary>
		/// <param name="keys">stirng to send</param>
		public void SendKeys(string keys)
		{
			if (m_hWnd != IntPtr.Zero)
			{
				WinAPI.SetForegroundWindow(m_hWnd);
				System.Threading.Thread.Sleep(200);
			}

			// wait until the control key has been lifted
			while (WinAPI.GetKeyState((UInt16)WinAPI.VirtualKeyCode.VK_CONTROL) < 0)
			{
				System.Threading.Thread.Sleep(200);
			}

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
								case "ENTER":
									SendKey('\n', delay, repeat);
									break;
								case "WAIT":
									for (; repeat > 0; repeat--)
									{
										System.Threading.Thread.Sleep(delay);
									}
									break;
								default:
									Enum.Parse(typeof(WinAPI.VirtualKeyCode), "VK_" + cmd, true);
									SendKey('\t', delay, repeat);
									break;
							}
						}
					}
					else
					{
						// plain text - send verbatim
						foreach (char key in single)
						{
							SendKey(key);
						}
					}
				}
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
		/// Send a single key to the current window a number of times with a delay after each key
		/// </summary>
		/// <param name="key">key to send</param>
		/// <param name="delay">delay in millisecs after each keypress</param>
		/// <param name="repeat">number of times</param>
		private void SendKey(char key, int delay, int repeat)
		{
			for (; repeat > 0; repeat--)
			{
				if (key >= 33 && key < 128)
				{
					List<WinAPI.INPUT> inputs = BuildInput(key);
					WinAPI.SendInput((UInt32)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(WinAPI.INPUT)));
					System.Threading.Thread.Sleep(delay != 0 ? delay : 50);
				}
				else if (key == 9)
				{
					List<WinAPI.INPUT> inputs = BuildInput(WinAPI.VirtualKeyCode.VK_TAB);
					WinAPI.SendInput((UInt32)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(WinAPI.INPUT)));
					System.Threading.Thread.Sleep(delay != 0 ? delay : 100);
				}
				else if (key == 10)
				{
					List<WinAPI.INPUT> inputs = BuildInput(WinAPI.VirtualKeyCode.VK_RETURN);
					WinAPI.SendInput((UInt32)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(WinAPI.INPUT)));
					System.Threading.Thread.Sleep(delay != 0 ? delay : 4000);
				}
				else if (key == 13)
				{
				}
				else if (key == 32)
				{
					List<WinAPI.INPUT> inputs = BuildInput(WinAPI.VirtualKeyCode.VK_SPACE);
					WinAPI.SendInput((UInt32)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(WinAPI.INPUT)));
					System.Threading.Thread.Sleep(delay != 0 ? delay : 50);
				}
				else
				{
					List<WinAPI.INPUT> inputs = BuildInput((WinAPI.VirtualKeyCode)key);
					WinAPI.SendInput((UInt32)inputs.Count, inputs.ToArray(), Marshal.SizeOf(typeof(WinAPI.INPUT)));
					System.Threading.Thread.Sleep(delay != 0 ? delay : 100);
				}
			}
		}

		/// <summary>
		/// Build INPUT arrays for a string to send to SentInput
		/// </summary>
		/// <param name="text"></param>
		/// <returns></returns>
		private List<WinAPI.INPUT> BuildInput(string text)
		{
			List<WinAPI.INPUT> inputs = new List<WinAPI.INPUT>(text.Length);
			foreach (char ch in text)
			{
				inputs.AddRange(BuildInput(ch));
			}

			return inputs;
		}

		/// <summary>
		/// Build INPUT arrays for a key press (KEYUP and KEYDOWN)
		/// </summary>
		/// <param name="ch">character to press</param>
		/// <returns>List of INPUTs</returns>
		private List<WinAPI.INPUT> BuildInput(char ch)
		{
			List<WinAPI.INPUT> inputs = new List<WinAPI.INPUT>();

			UInt16 scanCode = ch;

			// create two events, a key up and down
			WinAPI.INPUT down = new WinAPI.INPUT();
			down.Type = (UInt32)WinAPI.InputType.Keyboard;
			down.Data.Keyboard = new WinAPI.KEYBDINPUT();
			down.Data.Keyboard.KeyCode = 0;
			down.Data.Keyboard.Scan = scanCode;
			down.Data.Keyboard.Flags = (UInt32)WinAPI.KeyboardFlag.Unicode;
			down.Data.Keyboard.Time = 0;
			down.Data.Keyboard.ExtraInfo = IntPtr.Zero;
			//
			WinAPI.INPUT up = new WinAPI.INPUT();
			up.Type = (UInt32)WinAPI.InputType.Keyboard;
			up.Data.Keyboard = new WinAPI.KEYBDINPUT();
			up.Data.Keyboard.KeyCode = 0;
			up.Data.Keyboard.Scan = scanCode;
			up.Data.Keyboard.Flags = (UInt32)(WinAPI.KeyboardFlag.KeyUp | WinAPI.KeyboardFlag.Unicode);
			up.Data.Keyboard.Time = 0;
			up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			if ((scanCode & 0xff00) == 0xe000)
			{
				down.Data.Keyboard.Flags |= (UInt32)WinAPI.KeyboardFlag.ExtendedKey;
				up.Data.Keyboard.Flags |= (UInt32)WinAPI.KeyboardFlag.ExtendedKey;
			}

			inputs.Add(down);
			inputs.Add(up);

			return inputs;
		}

		/// <summary>
		/// Build a list of INPUTs for a key code. This is just a KeyUP and KeyDOwn, no KeyPress is generated
		/// </summary>
		/// <param name="vk">Key code</param>
		/// <returns>List of Inputs</returns>
		private List<WinAPI.INPUT> BuildInput(WinAPI.VirtualKeyCode vk)
		{
			List<WinAPI.INPUT> inputs = new List<WinAPI.INPUT>();

			UInt16 scanCode = (UInt16)vk;

			// build keydown and keyup events
			WinAPI.INPUT down = new WinAPI.INPUT();
			down.Type = (UInt32)WinAPI.InputType.Keyboard;
			down.Data.Keyboard = new WinAPI.KEYBDINPUT();
			down.Data.Keyboard.KeyCode = scanCode;
			down.Data.Keyboard.Scan = 0;
			down.Data.Keyboard.Flags = 0;
			down.Data.Keyboard.Time = 0;
			down.Data.Keyboard.ExtraInfo = IntPtr.Zero;
			//
			WinAPI.INPUT up = new WinAPI.INPUT();
			up.Type = (UInt32)WinAPI.InputType.Keyboard;
			up.Data.Keyboard = new WinAPI.KEYBDINPUT();
			up.Data.Keyboard.KeyCode = scanCode;
			up.Data.Keyboard.Scan = 0;
			up.Data.Keyboard.Flags = (UInt32)WinAPI.KeyboardFlag.KeyUp;
			up.Data.Keyboard.Time = 0;
			up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			inputs.Add(down);
			inputs.Add(up);

			return inputs;
		}

		/// <summary>
		/// Find a window for the give process and/or title
		/// </summary>
		/// <param name="processName">name of process</param>
		/// <param name="windowTitle">text of window title</param>
		/// <returns>Window handle if we can match the process and/or title</returns>
		private static IntPtr FindWindow(string processName, string windowTitle)
		{
			// get specific processes or all
			Process[] processes = (string.IsNullOrEmpty(processName) == false ? Process.GetProcessesByName(processName) : Process.GetProcesses());

			// if we have a title match it in the window titles
			if (string.IsNullOrEmpty(windowTitle) == false)
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


	/// <summary>
	/// Class holding the various Windows API consts and extern function
	/// </summary>
	public class WinAPI
	{
		/// <summary>
		/// Type of hook to set
		/// </summary>
		public enum HookType : int
		{
			WH_JOURNALRECORD = 0,
			WH_JOURNALPLAYBACK = 1,
			WH_KEYBOARD = 2,
			WH_GETMESSAGE = 3,
			WH_CALLWNDPROC = 4,
			WH_CBT = 5,
			WH_SYSMSGFILTER = 6,
			WH_MOUSE = 7,
			WH_HARDWARE = 8,
			WH_DEBUG = 9,
			WH_SHELL = 10,
			WH_FOREGROUNDIDLE = 11,
			WH_CALLWNDPROCRET = 12,
			WH_KEYBOARD_LL = 13,
			WH_MOUSE_LL = 14
		}

		public enum VirtualKeyCode : ushort
		{
			VK_LBUTTON = 0x01,
			VK_RBUTTON = 0x02,
			VK_CANCEL = 0x03,
			VK_MBUTTON = 0x04,
			VK_XBUTTON1 = 0x05,
			VK_XBUTTON2 = 0x06,
			VK_BACK = 0x08,
			VK_TAB = 0x09,
			VK_CLEAR = 0x0C,
			VK_RETURN = 0x0D,
			VK_SHIFT = 0x10,
			VK_CONTROL = 0x11,
			VK_MENU = 0x12,
			VK_PAUSE = 0x13,
			VK_CAPITAL = 0x14,
			VK_KANA = 0x15,
			VK_HANGUEL = 0x15,
			VK_HANGUL = 0x15,
			VK_JUNJA = 0x17,
			VK_FINAL = 0x18,
			VK_HANJA = 0x19,
			VK_KANJI = 0x19,
			VK_ESCAPE = 0x1B,
			VK_CONVERT = 0x1C,
			VK_NONCONVERT = 0x1D,
			VK_ACCEPT = 0x1E,
			VK_MODECHANGE = 0x1F,
			VK_SPACE = 0x20,
			VK_PRIOR = 0x21,
			VK_NEXT = 0x22,
			VK_END = 0x23,
			VK_HOME = 0x24,
			VK_LEFT = 0x25,
			VK_UP = 0x26,
			VK_RIGHT = 0x27,
			VK_DOWN = 0x28,
			VK_SELECT = 0x29,
			VK_PRINT = 0x2A,
			VK_EXECUTE = 0x2B,
			VK_SNAPSHOT = 0x2C,
			VK_INSERT = 0x2D,
			VK_DELETE = 0x2E,
			VK_HELP = 0x2F,
			//
			VK_0 = 0x30,
			VK_1 = 0x31,
			VK_2 = 0x32,
			VK_3 = 0x33,
			VK_4 = 0x34,
			VK_5 = 0x35,
			VK_6 = 0x36,
			VK_7 = 0x37,
			VK_8 = 0x38,
			VK_9 = 0x39,
			//
			VK_A = 0x41,
			VK_B = 0x42,
			VK_C = 0x43,
			VK_D = 0x44,
			VK_E = 0x45,
			VK_F = 0x46,
			VK_G = 0x47,
			VK_H = 0x48,
			VK_I = 0x49,
			VK_J = 0x4a,
			VK_K = 0x4b,
			VK_L = 0x4c,
			VK_M = 0x4d,
			VK_N = 0x4e,
			VK_O = 0x4f,
			VK_P = 0x50,
			VK_Q = 0x51,
			VK_R = 0x52,
			VK_S = 0x53,
			VK_T = 0x54,
			VK_U = 0x55,
			VK_V = 0x56,
			VK_W = 0x57,
			VK_X = 0x58,
			VK_Y = 0x59,
			VK_Z = 0x5a,
			//
			VK_NUMPAD0 = 0x60,
			VK_NUMPAD1 = 0x61,
			VK_NUMPAD2 = 0x62,
			VK_NUMPAD3 = 0x63,
			VK_NUMPAD4 = 0x64,
			VK_NUMPAD5 = 0x65,
			VK_NUMPAD6 = 0x66,
			VK_NUMPAD7 = 0x67,
			VK_NUMPAD8 = 0x68,
			VK_NUMPAD9 = 0x69,
			VK_MULTIPLY = 0x6A,
			VK_ADD = 0x6B,
			VK_SEPARATOR = 0x6C,
			VK_SUBTRACT = 0x6D,
			VK_DECIMAL = 0x6E,
			VK_DIVIDE = 0x6F,
			VK_F1 = 0x70,
			VK_F2 = 0x71,
			VK_F3 = 0x72,
			VK_F4 = 0x73,
			VK_F5 = 0x74,
			VK_F6 = 0x75,
			VK_F7 = 0x76,
			VK_F8 = 0x77,
			VK_F9 = 0x78,
			VK_F10 = 0x79,
			VK_F11 = 0x7A,
			VK_F12 = 0x7B

			//VK_F13 = 0x7C,
			//VK_F14 = 0x7D,
			//VK_F15 = 0x7E,
			//VK_F16 = 0x7F,
			//VK_F17 = 0x80,
			//VK_F18 = 0x81,
			//VK_F19 = 0x82,
			//VK_F20 = 0x83,
			//VK_F21 = 0x84,
			//VK_F22 = 0x85,

			//VK_NUMLOCK = 0x90,
			//VK_SCROLL = 0x91,
			//VK_LSHIFT = 0xA0,
			//VK_RSHIFT = 0xA1,
			//VK_LCONTROL = 0xA2,
			//VK_RCONTROL = 0xA3,
			//VK_LMENU = 0xA4,
			//VK_RMENU = 0xA5,
		}

		/// <summary>
		/// Key modifiers for setting/check alt,ctrl,etc (there is no etc key)
		/// </summary>
		[Flags()]
		public enum KeyModifiers : int
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			Windows = 8
		}

		/// <summary>
		/// Windows message constants
		/// </summary>
		public const int WM_KEYDOWN = 0x100;
		public const int WM_KEYUP = 0x101;
		public const int WM_SYSKEYDOWN = 0x104;
		public const int WM_SYSKEYUP = 0x105;

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

		public enum InputType : uint
		{
			Mouse = 0,
			Keyboard = 1,
			Hardware = 2,
		}

		public struct MOUSEINPUT
		{
			public Int32 X;
			public Int32 Y;
			public UInt32 MouseData;
			public UInt32 Flags;
			public UInt32 Time;
			public IntPtr ExtraInfo;
		}
		public struct KEYBDINPUT
		{
			public UInt16 KeyCode;
			public UInt16 Scan;
			public UInt32 Flags;
			public UInt32 Time;
			public IntPtr ExtraInfo;
		}
		public struct HARDWAREINPUT
		{
			public UInt32 Msg;
			public UInt16 ParamL;
			public UInt16 ParamH;
		}
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

		/// <summary>
		/// Native Windows calls
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern bool UnhookWindowsHookEx(IntPtr hInstance);
		[DllImport("user32.dll")]
		internal static extern int CallNextHookEx(IntPtr id, int code, int wParam, ref KeyboardHookStruct lParam);
		[DllImport("user32.dll", EntryPoint = "PostMessageA", SetLastError = true)]
		internal static extern bool PostMessage(IntPtr hWnd, uint msg, int wParam, IntPtr lParam);
		[DllImport("user32.dll", EntryPoint = "SendMessageA", SetLastError = true)]
		internal static extern IntPtr SendMessage(IntPtr hWnd, UInt32 Msg, int wParam, IntPtr lParam);
		[DllImport("user32.dll")]
		internal static extern byte VkKeyScan(char ch);
		[DllImport("user32.dll")]
		internal static extern bool RegisterHotKey(IntPtr hWnd, int id, KeyModifiers fsModifiers, Keys vkCode);
		[DllImport("user32.dll")]
		internal static extern void UnregisterHotKey(IntPtr hWnd, int id);
		[DllImport("kernel32.dll")]
		internal static extern IntPtr LoadLibrary(string lpFileName);
		[DllImport("user32.dll")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern Int16 GetKeyState(UInt16 virtualKeyCode);
	}

}
