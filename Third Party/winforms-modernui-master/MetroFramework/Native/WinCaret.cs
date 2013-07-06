/**
 * MetroFramework - Modern UI for WinForms
 * 
 * The MIT License (MIT)
 * Copyright (c) 2011 Sven Walter, http://github.com/viperneo
 * 
 * Permission is hereby granted, free of charge, to any person obtaining a copy of 
 * this software and associated documentation files (the "Software"), to deal in the 
 * Software without restriction, including without limitation the rights to use, copy, 
 * modify, merge, publish, distribute, sublicense, and/or sell copies of the Software, 
 * and to permit persons to whom the Software is furnished to do so, subject to the 
 * following conditions:
 * 
 * The above copyright notice and this permission notice shall be included in 
 * all copies or substantial portions of the Software.
 * 
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR IMPLIED, 
 * INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY, FITNESS FOR A 
 * PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE AUTHORS OR COPYRIGHT 
 * HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER LIABILITY, WHETHER IN AN ACTION OF 
 * CONTRACT, TORT OR OTHERWISE, ARISING FROM, OUT OF OR IN CONNECTION WITH THE SOFTWARE 
 * OR THE USE OR OTHER DEALINGS IN THE SOFTWARE.
 */
using System;
using System.Runtime.InteropServices;
using System.Security;

namespace MetroFramework.Native
{
    [SuppressUnmanagedCodeSecurity]
    internal sealed class WinCaret
    {
        [DllImport("User32.dll")]
        private static extern bool CreateCaret(IntPtr hWnd, int hBitmap, int nWidth, int nHeight);

        [DllImport("User32.dll")]
        private static extern bool SetCaretPos(int x, int y);

        [DllImport("User32.dll")]
        private static extern bool DestroyCaret();

        [DllImport("User32.dll")]
        private static extern bool ShowCaret(IntPtr hWnd);

        [DllImport("User32.dll")]
        public static extern bool HideCaret(IntPtr hWnd);

        private IntPtr controlHandle;

        public WinCaret(IntPtr ownerHandle)
        {
            controlHandle = ownerHandle;
        }

        public bool Create(int width, int height)
        {
            return CreateCaret(controlHandle, 0, width, height);
        }
        public void Hide()
        {
            HideCaret(controlHandle);
        }
        public void Show()
        {
            ShowCaret(controlHandle);
        }
        public bool SetPosition(int x, int y)
        {
            return SetCaretPos(x, y);
        }
        public void Destroy()
        {
            DestroyCaret();
        }
    }
}
