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
		public enum KeyModifiers : uint
		{
			None = 0,
			Alt = 1,
			Control = 2,
			Shift = 4,
			Windows = 8,
			NoRepeat = 0x4000
		}

		/// <summary>
		/// Windows message constants
		/// </summary>
		public const int WM_KEYDOWN = 0x100;
		public const int WM_KEYUP = 0x101;
		public const int WM_SYSKEYDOWN = 0x104;
		public const int WM_SYSKEYUP = 0x105;
		public const int WM_SETREDRAW = 0x0b;
		public const int WM_HOTKEY = 0x0312;
		public const int WM_USER = 0x0400;

		/// <summary>
		/// Windows constants for capturing scroll events
		/// </summary>
		public const int WM_HSCROLL = 0x114;
		public const int WM_VSCROLL = 0x115;
		public const int SB_LINELEFT = 0;
		public const int SB_LINERIGHT = 1;
		public const int SB_PAGELEFT = 2;
		public const int SB_PAGERIGHT = 3;
		public const int SB_THUMBPOSITION = 4;
		public const int SB_THUMBTRACK = 5;
		public const int SB_LEFT = 6;
		public const int SB_RIGHT = 7;
		public const int SB_ENDSCROLL = 8;
		public const int SIF_TRACKPOS = 0x10;
		public const int SIF_RANGE = 0x1;
		public const int SIF_POS = 0x4;
		public const int SIF_PAGE = 0x2;
		public const int SIF_ALL = SIF_RANGE | SIF_PAGE | SIF_POS | SIF_TRACKPOS;

		/// <summary>
		/// Returned structure for scroll info for GetScrollInfo
		/// </summary>
		public struct ScrollInfoStruct
		{
			public int cbSize;
			public int fMask;
			public int nMin;
			public int nMax;
			public int nPage;
			public int nPos;
			public int nTrackPos;
		}

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

		public enum ShowWindowCommands : int
		{
			Hide = 0,
			Normal = 1,
			Minimized = 2,
			Maximized = 3,
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

		/// <summary>
		/// Native Windows calls
		/// </summary>
		[DllImport("user32.dll")]
		internal static extern bool UnhookWindowsHookEx(IntPtr hInstance);
		[DllImport("user32.dll")]
		internal static extern IntPtr CallNextHookEx(IntPtr id, int code, int wParam, ref KeyboardHookStruct lParam);
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
		internal static extern IntPtr SetActiveWindow(IntPtr hWnd);
		[DllImport("user32.dll")]
		internal static extern bool SetForegroundWindow(IntPtr hWnd);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern IntPtr GetForegroundWindow();
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern Int16 GetKeyState(UInt16 virtualKeyCode);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern Int16 GetAsyncKeyState(UInt16 virtualKeyCode);
		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetKeyboardState(byte[] lpKeyState);
		[DllImport("User32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool BlockInput([MarshalAs(UnmanagedType.Bool)] bool fBlockIt);
		[DllImport("user32.dll")]
		internal static extern int HideCaret(IntPtr hwnd);
		[DllImport("user32.dll", SetLastError = true)]
		internal static extern int GetScrollInfo(IntPtr hWnd, int n, ref ScrollInfoStruct lpScrollInfo);

		[DllImport("user32.dll", SetLastError=true)]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool GetWindowPlacement(IntPtr hwnd, ref WINDOWPLACEMENT lpwndpl);

		[DllImport("user32.dll")]
		[return: MarshalAs(UnmanagedType.Bool)]
		internal static extern bool ShowWindow(IntPtr hWnd, ShowWindowCommands nCmdShow);

		[return: MarshalAs(UnmanagedType.Bool)]
		[DllImport("user32.dll", SetLastError = true)]
		static extern bool PostMessage(HandleRef hWnd, uint Msg, IntPtr wParam, IntPtr lParam);

		[DllImport("user32.dll")]
		internal static extern IntPtr GetOpenClipboardWindow();

		public static WINDOWPLACEMENT GetPlacement(IntPtr hwnd)
		{
			WINDOWPLACEMENT placement = new WINDOWPLACEMENT();
			placement.length = Marshal.SizeOf(placement);
			GetWindowPlacement(hwnd, ref placement);
			return placement;
		}
	}

}
