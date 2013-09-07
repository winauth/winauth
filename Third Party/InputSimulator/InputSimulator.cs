using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;

namespace WindowsInput
{
	/// <summary>
	/// Provides a useful wrapper around the User32 SendInput and related native Windows functions.
	/// </summary>
	public static class InputSimulator
	{
		#region DllImports

		/// <summary>
		/// The SendInput function synthesizes keystrokes, mouse motions, and button clicks.
		/// </summary>
		/// <param name="numberOfInputs">Number of structures in the Inputs array.</param>
		/// <param name="inputs">Pointer to an array of INPUT structures. Each structure represents an event to be inserted into the keyboard or mouse input stream.</param>
		/// <param name="sizeOfInputStructure">Specifies the size, in bytes, of an INPUT structure. If cbSize is not the size of an INPUT structure, the function fails.</param>
		/// <returns>The function returns the number of events that it successfully inserted into the keyboard or mouse input stream. If the function returns zero, the input was already blocked by another thread. To get extended error information, call GetLastError.Microsoft Windows Vista. This function fails when it is blocked by User Interface Privilege Isolation (UIPI). Note that neither GetLastError nor the return value will indicate the failure was caused by UIPI blocking.</returns>
		/// <remarks>
		/// Microsoft Windows Vista. This function is subject to UIPI. Applications are permitted to inject input only into applications that are at an equal or lesser integrity level.
		/// The SendInput function inserts the events in the INPUT structures serially into the keyboard or mouse input stream. These events are not interspersed with other keyboard or mouse input events inserted either by the user (with the keyboard or mouse) or by calls to keybd_event, mouse_event, or other calls to SendInput.
		/// This function does not reset the keyboard's current state. Any keys that are already pressed when the function is called might interfere with the events that this function generates. To avoid this problem, check the keyboard's state with the GetAsyncKeyState function and correct as necessary.
		/// </remarks>
		[DllImport("user32.dll", SetLastError = true)]
		static extern UInt32 SendInput(UInt32 numberOfInputs, INPUT[] inputs, Int32 sizeOfInputStructure);

		/// <summary>
		/// The GetAsyncKeyState function determines whether a key is up or down at the time the function is called, and whether the key was pressed after a previous call to GetAsyncKeyState. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
		/// </summary>
		/// <param name="virtualKeyCode">Specifies one of 256 possible virtual-key codes. For more information, see Virtual Key Codes. Windows NT/2000/XP: You can use left- and right-distinguishing constants to specify certain keys. See the Remarks section for further information.</param>
		/// <returns>
		/// If the function succeeds, the return value specifies whether the key was pressed since the last call to GetAsyncKeyState, and whether the key is currently up or down. If the most significant bit is set, the key is down, and if the least significant bit is set, the key was pressed after the previous call to GetAsyncKeyState. However, you should not rely on this last behavior; for more information, see the Remarks. 
		/// 
		/// Windows NT/2000/XP: The return value is zero for the following cases: 
		/// - The current desktop is not the active desktop
		/// - The foreground thread belongs to another process and the desktop does not allow the hook or the journal record.
		/// 
		/// Windows 95/98/Me: The return value is the global asynchronous key state for each virtual key. The system does not check which thread has the keyboard focus.
		/// 
		/// Windows 95/98/Me: Windows 95 does not support the left- and right-distinguishing constants. If you call GetAsyncKeyState with these constants, the return value is zero. 
		/// </returns>
		/// <remarks>
		/// The GetAsyncKeyState function works with mouse buttons. However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to. For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling 
		/// Copy CodeGetSystemMetrics(SM_SWAPBUTTON) which returns TRUE if the mouse buttons have been swapped.
		/// 
		/// Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
		/// 
		/// You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. 
		/// 
		/// Windows NT/2000/XP: You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of those keys. 
		/// 
		/// Code Meaning 
		/// VK_LSHIFT Left-shift key. 
		/// VK_RSHIFT Right-shift key. 
		/// VK_LCONTROL Left-control key. 
		/// VK_RCONTROL Right-control key. 
		/// VK_LMENU Left-menu key. 
		/// VK_RMENU Right-menu key. 
		/// 
		/// These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
		/// </remarks>
		[DllImport("user32.dll", SetLastError = true)]
		static extern Int16 GetAsyncKeyState(UInt16 virtualKeyCode);

		/// <summary>
		/// The GetKeyState function retrieves the status of the specified virtual key. The status specifies whether the key is up, down, or toggled (on, off alternating each time the key is pressed). (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
		/// </summary>
		/// <param name="virtualKeyCode">
		/// Specifies a virtual key. If the desired virtual key is a letter or digit (A through Z, a through z, or 0 through 9), nVirtKey must be set to the ASCII value of that character. For other keys, it must be a virtual-key code. 
		/// If a non-English keyboard layout is used, virtual keys with values in the range ASCII A through Z and 0 through 9 are used to specify most of the character keys. For example, for the German keyboard layout, the virtual key of value ASCII O (0x4F) refers to the "o" key, whereas VK_OEM_1 refers to the "o with umlaut" key.
		/// </param>
		/// <returns>
		/// The return value specifies the status of the specified virtual key, as follows: 
		/// If the high-order bit is 1, the key is down; otherwise, it is up.
		/// If the low-order bit is 1, the key is toggled. A key, such as the CAPS LOCK key, is toggled if it is turned on. The key is off and untoggled if the low-order bit is 0. A toggle key's indicator light (if any) on the keyboard will be on when the key is toggled, and off when the key is untoggled.
		/// </returns>
		/// <remarks>
		/// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
		/// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
		/// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
		/// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
		/// VK_LSHIFT
		/// VK_RSHIFT
		/// VK_LCONTROL
		/// VK_RCONTROL
		/// VK_LMENU
		/// VK_RMENU
		/// 
		/// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
		/// </remarks>
		[DllImport("user32.dll", SetLastError = true)]
		static extern Int16 GetKeyState(UInt16 virtualKeyCode);

		/// <summary>
		/// The GetMessageExtraInfo function retrieves the extra message information for the current thread. Extra message information is an application- or driver-defined value associated with the current thread's message queue. 
		/// </summary>
		/// <returns></returns>
		/// <remarks>To set a thread's extra message information, use the SetMessageExtraInfo function. </remarks>
		[DllImport("user32.dll")]
		static extern IntPtr GetMessageExtraInfo();

		[DllImport("user32.dll", CharSet = CharSet.Unicode)]
		static extern short VkKeyScan(char ch);

		#endregion

		#region Methods

		/// <summary>
		/// Determines whether a key is up or down at the time the function is called by calling the GetAsyncKeyState function. (See: http://msdn.microsoft.com/en-us/library/ms646293(VS.85).aspx)
		/// </summary>
		/// <param name="keyCode">The key code.</param>
		/// <returns>
		/// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// The GetAsyncKeyState function works with mouse buttons. However, it checks on the state of the physical mouse buttons, not on the logical mouse buttons that the physical buttons are mapped to. For example, the call GetAsyncKeyState(VK_LBUTTON) always returns the state of the left physical mouse button, regardless of whether it is mapped to the left or right logical mouse button. You can determine the system's current mapping of physical mouse buttons to logical mouse buttons by calling 
		/// Copy CodeGetSystemMetrics(SM_SWAPBUTTON) which returns TRUE if the mouse buttons have been swapped.
		/// 
		/// Although the least significant bit of the return value indicates whether the key has been pressed since the last query, due to the pre-emptive multitasking nature of Windows, another application can call GetAsyncKeyState and receive the "recently pressed" bit instead of your application. The behavior of the least significant bit of the return value is retained strictly for compatibility with 16-bit Windows applications (which are non-preemptive) and should not be relied upon.
		/// 
		/// You can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the vKey parameter. This gives the state of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. 
		/// 
		/// Windows NT/2000/XP: You can use the following virtual-key code constants as values for vKey to distinguish between the left and right instances of those keys. 
		/// 
		/// Code Meaning 
		/// VK_LSHIFT Left-shift key. 
		/// VK_RSHIFT Right-shift key. 
		/// VK_LCONTROL Left-control key. 
		/// VK_RCONTROL Right-control key. 
		/// VK_LMENU Left-menu key. 
		/// VK_RMENU Right-menu key. 
		/// 
		/// These left- and right-distinguishing constants are only available when you call the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
		/// </remarks>
		public static bool IsKeyDownAsync(VirtualKeyCode keyCode)
		{
			Int16 result = GetAsyncKeyState((UInt16)keyCode);
			return (result < 0);
		}

		/// <summary>
		/// Determines whether the specified key is up or down by calling the GetKeyState function. (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
		/// </summary>
		/// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
		/// <returns>
		/// 	<c>true</c> if the key is down; otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
		/// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
		/// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
		/// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
		/// VK_LSHIFT
		/// VK_RSHIFT
		/// VK_LCONTROL
		/// VK_RCONTROL
		/// VK_LMENU
		/// VK_RMENU
		/// 
		/// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
		/// </remarks>
		public static bool IsKeyDown(VirtualKeyCode keyCode)
		{
			Int16 result = GetKeyState((UInt16)keyCode);
			return (result < 0);
		}

		/// <summary>
		/// Determines whether the toggling key is toggled on (in-effect) or not by calling the GetKeyState function.  (See: http://msdn.microsoft.com/en-us/library/ms646301(VS.85).aspx)
		/// </summary>
		/// <param name="keyCode">The <see cref="VirtualKeyCode"/> for the key.</param>
		/// <returns>
		/// 	<c>true</c> if the toggling key is toggled on (in-effect); otherwise, <c>false</c>.
		/// </returns>
		/// <remarks>
		/// The key status returned from this function changes as a thread reads key messages from its message queue. The status does not reflect the interrupt-level state associated with the hardware. Use the GetAsyncKeyState function to retrieve that information. 
		/// An application calls GetKeyState in response to a keyboard-input message. This function retrieves the state of the key when the input message was generated. 
		/// To retrieve state information for all the virtual keys, use the GetKeyboardState function. 
		/// An application can use the virtual-key code constants VK_SHIFT, VK_CONTROL, and VK_MENU as values for the nVirtKey parameter. This gives the status of the SHIFT, CTRL, or ALT keys without distinguishing between left and right. An application can also use the following virtual-key code constants as values for nVirtKey to distinguish between the left and right instances of those keys. 
		/// VK_LSHIFT
		/// VK_RSHIFT
		/// VK_LCONTROL
		/// VK_RCONTROL
		/// VK_LMENU
		/// VK_RMENU
		/// 
		/// These left- and right-distinguishing constants are available to an application only through the GetKeyboardState, SetKeyboardState, GetAsyncKeyState, GetKeyState, and MapVirtualKey functions. 
		/// </remarks>
		public static bool IsTogglingKeyInEffect(VirtualKeyCode keyCode)
		{
			Int16 result = GetKeyState((UInt16)keyCode);
			return (result & 0x01) == 0x01;
		}

		/// <summary>
		/// Calls the Win32 SendInput method to simulate a Key DOWN.
		/// </summary>
		/// <param name="keyCode">The VirtualKeyCode to press</param>
		public static void SimulateKeyDown(VirtualKeyCode keyCode)
		{
			var down = new INPUT();
			down.Type = (UInt32)InputType.KEYBOARD;
			down.Data.Keyboard = new KEYBDINPUT();
			down.Data.Keyboard.Vk = (UInt16)keyCode;
			down.Data.Keyboard.Scan = 0;
			down.Data.Keyboard.Flags = 0;
			down.Data.Keyboard.Time = 0;
			down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			INPUT[] inputList = new INPUT[1];
			inputList[0] = down;

			var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
			if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key down simulation for {0} was not successful.", keyCode));
		}

		/// <summary>
		/// Calls the Win32 SendInput method to simulate a Key UP.
		/// </summary>
		/// <param name="keyCode">The VirtualKeyCode to lift up</param>
		public static void SimulateKeyUp(VirtualKeyCode keyCode)
		{
			var up = new INPUT();
			up.Type = (UInt32)InputType.KEYBOARD;
			up.Data.Keyboard = new KEYBDINPUT();
			up.Data.Keyboard.Vk = (UInt16)keyCode;
			up.Data.Keyboard.Scan = 0;
			up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KEYUP;
			up.Data.Keyboard.Time = 0;
			up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			INPUT[] inputList = new INPUT[1];
			inputList[0] = up;

			var numberOfSuccessfulSimulatedInputs = SendInput(1, inputList, Marshal.SizeOf(typeof(INPUT)));
			if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key up simulation for {0} was not successful.", keyCode));
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a KeyDown and KeyUp message in the same input sequence in order to simulate a Key PRESS.
		/// </summary>
		/// <param name="keyCode">The VirtualKeyCode to press</param>
		public static void SimulateKeyPress(VirtualKeyCode keyCode)
		{
			var down = new INPUT();
			down.Type = (UInt32)InputType.KEYBOARD;
			down.Data.Keyboard = new KEYBDINPUT();
			down.Data.Keyboard.Vk = (UInt16)keyCode;
			down.Data.Keyboard.Scan = 0;
			down.Data.Keyboard.Flags = 0;
			down.Data.Keyboard.Time = 0;
			down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			var up = new INPUT();
			up.Type = (UInt32)InputType.KEYBOARD;
			up.Data.Keyboard = new KEYBDINPUT();
			up.Data.Keyboard.Vk = (UInt16)keyCode;
			up.Data.Keyboard.Scan = 0;
			up.Data.Keyboard.Flags = (UInt32)KeyboardFlag.KEYUP;
			up.Data.Keyboard.Time = 0;
			up.Data.Keyboard.ExtraInfo = IntPtr.Zero;

			INPUT[] inputList = new INPUT[2];
			inputList[0] = down;
			inputList[1] = up;

			var numberOfSuccessfulSimulatedInputs = SendInput(2, inputList, Marshal.SizeOf(typeof(INPUT)));
			if (numberOfSuccessfulSimulatedInputs == 0) throw new Exception(string.Format("The key press simulation for {0} was not successful.", keyCode));
		}

		/// <summary>
		/// Calls the Win32 SendInput method with a stream of KeyDown and KeyUp messages in order to simulate uninterrupted text entry via the keyboard.
		/// </summary>
		/// <param name="text">The text to be simulated.</param>
		public static void SimulateTextEntry(string text)
		{
			if (text.Length > UInt32.MaxValue / 2) throw new ArgumentException(string.Format("The text parameter is too long. It must be less than {0} characters.", UInt32.MaxValue / 2), "text");

			var chars = text.ToCharArray(); // UTF8Encoding.ASCII.GetBytes(text);
			var len = chars.Length;
			INPUT[] inputList = new INPUT[len * 2];
			for (int x = 0; x < len; x++)
			{
				UInt16 scanCode = chars[x];
				var down = new INPUT();
				var up = new INPUT();

				if (scanCode < 32)
				{
					// map the scan code to a Vk so we can deal with special control chars, e.g. Tab and Enter
					ushort vk = (ushort)VkKeyScan((char)chars[x]);
					ushort state = (ushort)((vk & (ushort)0xff00) >> 8);
					vk = (ushort)(vk & (ushort)0xff);

					down.Type = (UInt32)InputType.KEYBOARD;
					down.Data.Keyboard = new KEYBDINPUT();
					down.Data.Keyboard.Vk = vk;
					down.Data.Keyboard.Scan = 0;
					down.Data.Keyboard.Flags = (UInt32)0;
					down.Data.Keyboard.Time = 0;
					down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

					up.Type = (UInt32)InputType.KEYBOARD;
					up.Data.Keyboard = new KEYBDINPUT();
					up.Data.Keyboard.Vk = vk;
					up.Data.Keyboard.Scan = 0;
					up.Data.Keyboard.Flags = (UInt32)(KeyboardFlag.KEYUP);
					up.Data.Keyboard.Time = 0;
					up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
				}
				else
				{
					down.Type = (UInt32)InputType.KEYBOARD;
					down.Data.Keyboard = new KEYBDINPUT();
					down.Data.Keyboard.Vk = 0;
					down.Data.Keyboard.Scan = scanCode;
					down.Data.Keyboard.Flags = (UInt32)KeyboardFlag.UNICODE;
					down.Data.Keyboard.Time = 0;
					down.Data.Keyboard.ExtraInfo = IntPtr.Zero;

					up.Type = (UInt32)InputType.KEYBOARD;
					up.Data.Keyboard = new KEYBDINPUT();
					up.Data.Keyboard.Vk = 0;
					up.Data.Keyboard.Scan = scanCode;
					up.Data.Keyboard.Flags = (UInt32)(KeyboardFlag.KEYUP | KeyboardFlag.UNICODE);
					up.Data.Keyboard.Time = 0;
					up.Data.Keyboard.ExtraInfo = IntPtr.Zero;
				}

				// Handle extended keys:
				// If the scan code is preceded by a prefix byte that has the value 0xE0 (224),
				// we need to include the KEYEVENTF_EXTENDEDKEY flag in the Flags property. 
				if ((scanCode & 0xFF00) == 0xE000)
				{
					down.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
					up.Data.Keyboard.Flags |= (UInt32)KeyboardFlag.EXTENDEDKEY;
				}

				inputList[2 * x] = down;
				inputList[2 * x + 1] = up;
			}

			var numberOfSuccessfulSimulatedInputs = SendInput((UInt32)len * 2, inputList, Marshal.SizeOf(typeof(INPUT)));
		}

		/// <summary>
		/// Performs a simple modified keystroke like CTRL-C where CTRL is the modifierKey and C is the key.
		/// The flow is Modifier KEYDOWN, Key PRESS, Modifier KEYUP.
		/// </summary>
		/// <param name="modifierKeyCode">The modifier key</param>
		/// <param name="keyCode">The key to simulate</param>
		public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKeyCode, VirtualKeyCode keyCode)
		{
			SimulateKeyDown(modifierKeyCode);
			SimulateKeyPress(keyCode);
			SimulateKeyUp(modifierKeyCode);
		}

		/// <summary>
		/// Performs a modified keystroke where there are multiple modifiers and one key like CTRL-ALT-C where CTRL and ALT are the modifierKeys and C is the key.
		/// The flow is Modifiers KEYDOWN in order, Key PRESS, Modifiers KEYUP in reverse order.
		/// </summary>
		/// <param name="modifierKeyCodes">The list of modifier keys</param>
		/// <param name="keyCode">The key to simulate</param>
		public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, VirtualKeyCode keyCode)
		{
			if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => SimulateKeyDown(x));
			SimulateKeyPress(keyCode);
			if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => SimulateKeyUp(x));
		}

		/// <summary>
		/// Performs a modified keystroke where there is one modifier and multiple keys like CTRL-K-C where CTRL is the modifierKey and K and C are the keys.
		/// The flow is Modifier KEYDOWN, Keys PRESS in order, Modifier KEYUP.
		/// </summary>
		/// <param name="modifierKey">The modifier key</param>
		/// <param name="keyCodes">The list of keys to simulate</param>
		public static void SimulateModifiedKeyStroke(VirtualKeyCode modifierKey, IEnumerable<VirtualKeyCode> keyCodes)
		{
			SimulateKeyDown(modifierKey);
			if (keyCodes != null) keyCodes.ToList().ForEach(x => SimulateKeyPress(x));
			SimulateKeyUp(modifierKey);
		}

		/// <summary>
		/// Performs a modified keystroke where there are multiple modifiers and multiple keys like CTRL-ALT-K-C where CTRL and ALT are the modifierKeys and K and C are the keys.
		/// The flow is Modifiers KEYDOWN in order, Keys PRESS in order, Modifiers KEYUP in reverse order.
		/// </summary>
		/// <param name="modifierKeyCodes">The list of modifier keys</param>
		/// <param name="keyCodes">The list of keys to simulate</param>
		public static void SimulateModifiedKeyStroke(IEnumerable<VirtualKeyCode> modifierKeyCodes, IEnumerable<VirtualKeyCode> keyCodes)
		{
			if (modifierKeyCodes != null) modifierKeyCodes.ToList().ForEach(x => SimulateKeyDown(x));
			if (keyCodes != null) keyCodes.ToList().ForEach(x => SimulateKeyPress(x));
			if (modifierKeyCodes != null) modifierKeyCodes.Reverse().ToList().ForEach(x => SimulateKeyUp(x));
		}

		#endregion
	}
}