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
using System.Drawing;
using System.Runtime.InteropServices;
using System.Security;

namespace MetroFramework.Native
{
    public enum TaskbarPosition
    {
        Unknown = -1,
        Left,
        Top,
        Right,
        Bottom,
    }

    internal class Taskbar
    {
        private const string ClassName = "Shell_TrayWnd";

        private Rectangle bounds = Rectangle.Empty;
        public Rectangle Bounds
        {
            get { return bounds; }
            private set { bounds = value; }
        }

        private TaskbarPosition position = TaskbarPosition.Unknown;
        public TaskbarPosition Position
        {
            get { return position; }
            private set { position = value; }
        }

        public Point Location
        {
            get
            {
                return this.Bounds.Location;
            }
        }

        public Size Size
        {
            get
            {
                return this.Bounds.Size;
            }
        }

        private bool alwaysOnTop = false;
        public bool AlwaysOnTop
        {
            get { return alwaysOnTop; }
            private set { alwaysOnTop = value; }
        }

        private bool autoHide = false;
        public bool AutoHide
        {
            get { return autoHide; }
            private set { autoHide = value; }
        }

        [SecuritySafeCritical]
        public Taskbar()
        {
            IntPtr taskbarHandle = WinApi.FindWindow(Taskbar.ClassName, null);

            WinApi.APPBARDATA data = new WinApi.APPBARDATA();
            data.cbSize = (uint)Marshal.SizeOf(typeof(WinApi.APPBARDATA));
            data.hWnd = taskbarHandle;
            IntPtr result = WinApi.SHAppBarMessage(WinApi.ABM.GetTaskbarPos, ref data);
            if (result == IntPtr.Zero)
                throw new InvalidOperationException();

            this.Position = (TaskbarPosition)data.uEdge;
            this.Bounds = Rectangle.FromLTRB(data.rc.Left, data.rc.Top, data.rc.Right, data.rc.Bottom);

            data.cbSize = (uint)Marshal.SizeOf(typeof(WinApi.APPBARDATA));
            result = WinApi.SHAppBarMessage(WinApi.ABM.GetState, ref data);
            int state = result.ToInt32();
            this.AlwaysOnTop = (state & WinApi.AlwaysOnTop) == WinApi.AlwaysOnTop;
            this.AutoHide = (state & WinApi.Autohide) == WinApi.Autohide;
        }

    }
}
