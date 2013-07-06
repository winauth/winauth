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
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Windows.Forms;
using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;
using MetroFramework.Native;

namespace MetroFramework.Controls
{
    #region Enums

    public enum MetroScrollOrientation
    {
        Horizontal,
        Vertical
    }

    #endregion

    [Designer("MetroFramework.Design.Controls.MetroScrollBarDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [DefaultEvent("Scroll")]
    [DefaultProperty("Value")]
    public class MetroScrollBar : Control, IMetroControl
    {
        #region Interface

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintBackground;
        protected virtual void OnCustomPaintBackground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintBackground != null)
            {
                CustomPaintBackground(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaint;
        protected virtual void OnCustomPaint(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaint != null)
            {
                CustomPaint(this, e);
            }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public event EventHandler<MetroPaintEventArgs> CustomPaintForeground;
        protected virtual void OnCustomPaintForeground(MetroPaintEventArgs e)
        {
            if (GetStyle(ControlStyles.UserPaint) && CustomPaintForeground != null)
            {
                CustomPaintForeground(this, e);
            }
        }

        private MetroColorStyle metroStyle = MetroColorStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroColorStyle.Default)]
        public MetroColorStyle Style
        {
            get
            {
                if (DesignMode || metroStyle != MetroColorStyle.Default)
                {
                    return metroStyle;
                }

                if (StyleManager != null && metroStyle == MetroColorStyle.Default)
                {
                    return StyleManager.Style;
                }
                if (StyleManager == null && metroStyle == MetroColorStyle.Default)
                {
                    return MetroDefaults.Style;
                }

                return metroStyle;
            }
            set { metroStyle = value; }
        }

        private MetroThemeStyle metroTheme = MetroThemeStyle.Default;
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DefaultValue(MetroThemeStyle.Default)]
        public MetroThemeStyle Theme
        {
            get
            {
                if (DesignMode || metroTheme != MetroThemeStyle.Default)
                {
                    return metroTheme;
                }

                if (StyleManager != null && metroTheme == MetroThemeStyle.Default)
                {
                    return StyleManager.Theme;
                }
                if (StyleManager == null && metroTheme == MetroThemeStyle.Default)
                {
                    return MetroDefaults.Theme;
                }

                return metroTheme;
            }
            set { metroTheme = value; }
        }

        private MetroStyleManager metroStyleManager = null;
        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public MetroStyleManager StyleManager
        {
            get { return metroStyleManager; }
            set { metroStyleManager = value; }
        }

        private bool useCustomBackColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomBackColor
        {
            get { return useCustomBackColor; }
            set { useCustomBackColor = value; }
        }

        private bool useCustomForeColor = false;
        [Browsable(false)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [Browsable(false)]
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public bool UseStyleColors
        {
            get { return useStyleColors; }
            set { useStyleColors = value; }
        }

        [Browsable(false)]
        [Category(MetroDefaults.PropertyCategory.Behaviour)]
        [DefaultValue(false)]
        public bool UseSelectable
        {
            get { return GetStyle(ControlStyles.Selectable); }
            set { SetStyle(ControlStyles.Selectable, value); }
        }

        #endregion

        #region Events

        public event ScrollEventHandler Scroll;

        private void OnScroll(ScrollEventType type, int oldValue, int newValue, ScrollOrientation orientation)
        {
            if (Scroll == null) return;

            if (orientation == ScrollOrientation.HorizontalScroll)
            {
                if (type != ScrollEventType.EndScroll && isFirstScrollEventHorizontal)
                {
                    type = ScrollEventType.First;
                }
                else if (!isFirstScrollEventHorizontal && type == ScrollEventType.EndScroll)
                {
                    isFirstScrollEventHorizontal = true;
                }
            }
            else
            {
                if (type != ScrollEventType.EndScroll && isFirstScrollEventVertical)
                {
                    type = ScrollEventType.First;
                }
                else if (!isFirstScrollEventHorizontal && type == ScrollEventType.EndScroll)
                {
                    isFirstScrollEventVertical = true;
                }
            }

            Scroll(this, new ScrollEventArgs(type, oldValue, newValue, orientation));
        }

        #endregion

        #region Fields

        private bool isFirstScrollEventVertical = true;
        private bool isFirstScrollEventHorizontal = true;

        private bool inUpdate;

        private Rectangle clickedBarRectangle;
        private Rectangle thumbRectangle;

        private bool topBarClicked;
        private bool bottomBarClicked;
        private bool thumbClicked;

        private int thumbWidth = 6;
        private int thumbHeight;

        private int thumbBottomLimitBottom;
        private int thumbBottomLimitTop;
        private int thumbTopLimit;
        private int thumbPosition;

        private int trackPosition;

        private readonly Timer progressTimer = new Timer();

        private int mouseWheelBarPartitions = 10;

        public int MouseWheelBarPartitions
        {
            get { return mouseWheelBarPartitions; }
            set
            {
                if (value > 0)
                {
                    mouseWheelBarPartitions = value;
                }
                else
                {
                    throw new ArgumentOutOfRangeException("value", "MouseWheelBarPartitions has to be greather than zero");
                }
            }
        }

        private bool isHovered;
        private bool isPressed;

        private bool useBarColor = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseBarColor
        {
            get { return useBarColor; }
            set { useBarColor = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public int ScrollbarSize
        {
            get { return Orientation == MetroScrollOrientation.Vertical ? Width : Height; }
            set 
            { 
                if (Orientation == MetroScrollOrientation.Vertical)
                    Width = value;
                else
                    Height = value; 
            }
        }

        private bool highlightOnWheel = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool HighlightOnWheel
        {
            get { return highlightOnWheel; }
            set { highlightOnWheel = value; }
        }

        private MetroScrollOrientation metroOrientation = MetroScrollOrientation.Vertical;
        private ScrollOrientation scrollOrientation = ScrollOrientation.VerticalScroll;

        public MetroScrollOrientation Orientation
        {
            get { return metroOrientation; }

            set
            {
                if (value == metroOrientation)
                {
                    return;
                }

                metroOrientation = value;

                if (value == MetroScrollOrientation.Vertical)
                {
                    scrollOrientation = ScrollOrientation.VerticalScroll;
                }
                else
                {
                    scrollOrientation = ScrollOrientation.HorizontalScroll;
                }

                Size = new Size(Height, Width);
                SetupScrollBar();
            }
        }

        private int minimum;
        private int maximum = 100;
        private int smallChange = 1;
        private int largeChange = 10;
        private int curValue;

        public int Minimum
        {
            get { return minimum; }
            set
            {
                if (minimum == value || value < 0 || value >= maximum)
                {
                    return;
                }

                minimum = value;
                if (curValue < value)
                {
                    curValue = value;
                }

                if (largeChange > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }

                SetupScrollBar();

                if (curValue < value)
                {
                    dontUpdateColor = true;
                    Value = value;
                }
                else
                {
                    ChangeThumbPosition(GetThumbPosition());
                    Refresh();
                }
            }
        }

        public int Maximum
        {
            get { return maximum; }
            set
            {
                if (value == maximum || value < 1 || value <= minimum)
                {
                    return;
                }

                maximum = value;
                if (largeChange > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }

                SetupScrollBar();

                if (curValue > value)
                {
                    dontUpdateColor = true;
                    Value = maximum;
                }
                else
                {
                    ChangeThumbPosition(GetThumbPosition());
                    Refresh();
                }
            }
        }

        [DefaultValue(1)]
        public int SmallChange
        {
            get { return smallChange; }
            set
            {
                if (value == smallChange || value < 1 || value >= largeChange)
                {
                    return;
                }

                smallChange = value;
                SetupScrollBar();
            }
        }

        [DefaultValue(5)]
        public int LargeChange
        {
            get { return largeChange; }
            set
            {
                if (value == largeChange || value < smallChange || value < 2)
                {
                    return;
                }

                if (value > (maximum - minimum))
                {
                    largeChange = maximum - minimum;
                }
                else
                {
                    largeChange = value;
                }

                SetupScrollBar();
            }
        }

        private bool dontUpdateColor = false;

        [DefaultValue(0)]
        [Browsable(false)]
        public int Value
        {
            get { return curValue; }

            set
            {
                if (curValue == value || value < minimum || value > maximum)
                {
                    return;
                }

                curValue = value;

                ChangeThumbPosition(GetThumbPosition());

                OnScroll(ScrollEventType.ThumbPosition, -1, value, scrollOrientation);

                if (!dontUpdateColor && highlightOnWheel)
                {
                    if (!isHovered)
                        isHovered = true;

                    if (autoHoverTimer == null)
                    {
                        autoHoverTimer = new Timer();
                        autoHoverTimer.Interval = 1000;
                        autoHoverTimer.Tick += new EventHandler(autoHoverTimer_Tick);
                        autoHoverTimer.Start();
                    }
                    else
                    {
                        autoHoverTimer.Stop();
                        autoHoverTimer.Start();
                    }
                }
                else
                {
                    dontUpdateColor = false;
                }

                Refresh();
            }
        }

        private void autoHoverTimer_Tick(object sender, EventArgs e)
        {
            isHovered = false;
            Invalidate();
            autoHoverTimer.Stop();
        }

        private Timer autoHoverTimer = null;

        #endregion

        #region Constructor

        public MetroScrollBar()
        {
            SetStyle(ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.Selectable |
                     ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.UserPaint, true);

            Width = 10;
            Height = 200;

            SetupScrollBar();

            progressTimer.Interval = 20;
            progressTimer.Tick += ProgressTimerTick;
        }

        public MetroScrollBar(MetroScrollOrientation orientation)
            : this()
        {
            Orientation = orientation;
        }

        public MetroScrollBar(MetroScrollOrientation orientation, int width)
            : this(orientation)
        {
            Width = width;
        }

        public bool HitTest(Point point)
        {
            return thumbRectangle.Contains(point);
        }

        #endregion

        #region Update Methods

        [SecuritySafeCritical]
        public void BeginUpdate()
        {
            WinApi.SendMessage(Handle, (int) WinApi.Messages.WM_SETREDRAW, false, 0);
            inUpdate = true;
        }

        [SecuritySafeCritical]
        public void EndUpdate()
        {
            WinApi.SendMessage(Handle, (int)WinApi.Messages.WM_SETREDRAW, true, 0);
            inUpdate = false;
            SetupScrollBar();
            Refresh();
        }

        #endregion

        #region Paint Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (!useCustomBackColor)
                {
                    if (Parent != null)
                    {
                        if (Parent is IMetroControl)
                        {
                            backColor = MetroPaint.BackColor.Form(Theme);
                        }
                        else
                        {
                            backColor = Parent.BackColor;
                        }
                    }
                    else
                    {
                        backColor = MetroPaint.BackColor.Form(Theme);
                    }
                }

                if (backColor.A == 255)
                {
                    e.Graphics.Clear(backColor);
                    return;
                }

                base.OnPaintBackground(e);

                OnCustomPaintBackground(new MetroPaintEventArgs(backColor, Color.Empty, e.Graphics));
            }
            catch
            {
                Invalidate();
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            try
            {
                if (GetStyle(ControlStyles.AllPaintingInWmPaint))
                {
                    OnPaintBackground(e);
                }

                OnCustomPaint(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
                OnPaintForeground(e);
            }
            catch
            {
                Invalidate();
            }
        }

        protected virtual void OnPaintForeground(PaintEventArgs e)
        {
            Color backColor, thumbColor, barColor;

            if (useCustomBackColor)
            {
                backColor = BackColor;
            }
            else
            {
                if (Parent != null)
                {
                    if (Parent is IMetroControl)
                    {
                        backColor = MetroPaint.BackColor.Form(Theme);
                    }
                    else
                    {
                        backColor = Parent.BackColor;
                    }
                }
                else
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                }
            }

            if (isHovered && !isPressed && Enabled)
            {
                thumbColor = MetroPaint.BackColor.ScrollBar.Thumb.Hover(Theme);
                barColor = MetroPaint.BackColor.ScrollBar.Bar.Hover(Theme);
            }
            else if (isHovered && isPressed && Enabled)
            {
                thumbColor = MetroPaint.BackColor.ScrollBar.Thumb.Press(Theme);
                barColor = MetroPaint.BackColor.ScrollBar.Bar.Press(Theme);
            }
            else if (!Enabled)
            {
                thumbColor = MetroPaint.BackColor.ScrollBar.Thumb.Disabled(Theme);
                barColor = MetroPaint.BackColor.ScrollBar.Bar.Disabled(Theme);
            }
            else
            {
                thumbColor = MetroPaint.BackColor.ScrollBar.Thumb.Normal(Theme);
                barColor = MetroPaint.BackColor.ScrollBar.Bar.Normal(Theme);
            }

            DrawScrollBar(e.Graphics, backColor, thumbColor, barColor);

            OnCustomPaintForeground(new MetroPaintEventArgs(backColor, thumbColor, e.Graphics));
        }

        private void DrawScrollBar(Graphics g, Color backColor, Color thumbColor, Color barColor)
        {
            if (useBarColor)
            {
                using (var b = new SolidBrush(barColor))
                {
                    g.FillRectangle(b, ClientRectangle);
                }
            }

            using (var b = new SolidBrush(backColor))
            {
                var thumbRect = new Rectangle(thumbRectangle.X - 1, thumbRectangle.Y - 1, thumbRectangle.Width + 2, thumbRectangle.Height + 2);
                g.FillRectangle(b, thumbRect);
            }

            using (var b = new SolidBrush(thumbColor))
            {
                g.FillRectangle(b, thumbRectangle);
            }
        }

        #endregion

        #region Focus Methods

        protected override void OnGotFocus(EventArgs e)
        {
            Invalidate();

            base.OnGotFocus(e);
        }

        protected override void OnLostFocus(EventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLostFocus(e);
        }

        protected override void OnEnter(EventArgs e)
        {
            Invalidate();

            base.OnEnter(e);
        }

        protected override void OnLeave(EventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnLeave(e);
        }

        #endregion

        #region Mouse Methods

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            int v = e.Delta / 120 * (maximum - minimum) / mouseWheelBarPartitions;

            if (Orientation == MetroScrollOrientation.Vertical)
            {
                Value -= v;
            }
            else
            {
                Value += v;
            }
        }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
            {
                isPressed = true;
                Invalidate();
            }

            base.OnMouseDown(e);

            Focus();

            if (e.Button == MouseButtons.Left) {

                var mouseLocation = e.Location;

                if (thumbRectangle.Contains(mouseLocation))
                {
                    thumbClicked = true;
                    thumbPosition = metroOrientation == MetroScrollOrientation.Vertical ? mouseLocation.Y - thumbRectangle.Y : mouseLocation.X - thumbRectangle.X;

                    Invalidate(thumbRectangle);
                }
                else
                {
                    trackPosition = metroOrientation == MetroScrollOrientation.Vertical ? mouseLocation.Y : mouseLocation.X;

                    if (trackPosition < (metroOrientation == MetroScrollOrientation.Vertical ? thumbRectangle.Y : thumbRectangle.X))
                    {
                        topBarClicked = true;
                    }
                    else
                    {
                        bottomBarClicked = true;
                    }

                    ProgressThumb(true);
                }
            }
            else if (e.Button == MouseButtons.Right)
            {
                trackPosition = metroOrientation == MetroScrollOrientation.Vertical ? e.Y : e.X;
            }
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            isPressed = false;

            base.OnMouseUp(e);

            if (e.Button == MouseButtons.Left)
            {
                if (thumbClicked)
                {
                    thumbClicked = false;
                    OnScroll(ScrollEventType.EndScroll, -1, curValue, scrollOrientation);
                }
                else if (topBarClicked)
                {
                    topBarClicked = false;
                    StopTimer();
                }
                else if (bottomBarClicked)
                {
                    bottomBarClicked = false;
                    StopTimer();
                }

                Invalidate();
            }
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            isHovered = true;
            Invalidate();

            base.OnMouseEnter(e);
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            isHovered = false;
            Invalidate();

            base.OnMouseLeave(e);

            ResetScrollStatus();
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            if (e.Button == MouseButtons.Left)
            {
                if (thumbClicked)
                {
                    int oldScrollValue = curValue;

                    int pos = metroOrientation == MetroScrollOrientation.Vertical ? e.Location.Y : e.Location.X;
                    int thumbSize = metroOrientation == MetroScrollOrientation.Vertical ? (pos / Height) / thumbHeight : (pos / Width) / thumbWidth;

                    if (pos <= (thumbTopLimit + thumbPosition))
                    {
                        ChangeThumbPosition(thumbTopLimit);
                        curValue = minimum;
                        Invalidate();
                    }
                    else if (pos >= (thumbBottomLimitTop + thumbPosition))
                    {
                        ChangeThumbPosition(thumbBottomLimitTop);
                        curValue = maximum;
                        Invalidate();
                    }
                    else
                    {
                        ChangeThumbPosition(pos - thumbPosition);

                        int pixelRange, thumbPos;

                        if (Orientation == MetroScrollOrientation.Vertical)
                        {
                            pixelRange = Height - thumbSize;
                            thumbPos = thumbRectangle.Y;
                        }
                        else
                        {
                            pixelRange = Width - thumbSize;
                            thumbPos = thumbRectangle.X;
                        }

                        float perc = 0f;

                        if (pixelRange != 0)
                        {
                            perc = (thumbPos)/(float) pixelRange;
                        }

                        curValue = Convert.ToInt32((perc * (maximum - minimum)) + minimum);
                    }

                    if (oldScrollValue != curValue)
                    {
                        OnScroll(ScrollEventType.ThumbTrack, oldScrollValue, curValue, scrollOrientation);
                        Refresh();
                    }
                }
            }
            else if (!ClientRectangle.Contains(e.Location))
            {
                ResetScrollStatus();
            }
            else if (e.Button == MouseButtons.None)
            {
                if (thumbRectangle.Contains(e.Location))
                {
                    Invalidate(thumbRectangle);
                }
                else if (ClientRectangle.Contains(e.Location))
                {
                    Invalidate();
                }
            }
        }

        #endregion

        #region Keyboard Methods

        protected override void OnKeyDown(KeyEventArgs e)
        {
            isHovered = true;
            isPressed = true;
            Invalidate();

            base.OnKeyDown(e);
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            isHovered = false;
            isPressed = false;
            Invalidate();

            base.OnKeyUp(e);
        }

        #endregion

        #region Management Methods

        protected override void SetBoundsCore(int x, int y, int width, int height, BoundsSpecified specified)
        {
            base.SetBoundsCore(x, y, width, height, specified);

            if (DesignMode)
            {
                SetupScrollBar();
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetupScrollBar();
        }

        protected override bool ProcessDialogKey(Keys keyData)
        {
            var keyUp = Keys.Up;
            var keyDown = Keys.Down;

            if (Orientation == MetroScrollOrientation.Horizontal)
            {
                keyUp = Keys.Left;
                keyDown = Keys.Right;
            }

            if (keyData == keyUp)
            {
                Value -= smallChange;

                return true;
            }

            if (keyData == keyDown)
            {
                Value += smallChange;

                return true;
            }

            if (keyData == Keys.PageUp)
            {
                Value = GetValue(false, true);

                return true;
            }

            if (keyData == Keys.PageDown)
            {
                if (curValue + largeChange > maximum)
                {
                    Value = maximum;
                }
                else
                {
                    Value += largeChange;
                }

                return true;
            }

            if (keyData == Keys.Home)
            {
                Value = minimum;

                return true;
            }

            if (keyData == Keys.End)
            {
                Value = maximum;

                return true;
            }

            return base.ProcessDialogKey(keyData);
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        private void SetupScrollBar()
        {
            if (inUpdate) return;

            if (Orientation == MetroScrollOrientation.Vertical)
            {
                thumbWidth = Width > 0 ? Width : 10;
                thumbHeight = GetThumbSize();

                clickedBarRectangle = ClientRectangle;
                clickedBarRectangle.Inflate(-1, -1);

                thumbRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, thumbWidth, thumbHeight);

                thumbPosition = thumbRectangle.Height/2;
                thumbBottomLimitBottom = ClientRectangle.Bottom;
                thumbBottomLimitTop = thumbBottomLimitBottom - thumbRectangle.Height;
                thumbTopLimit = ClientRectangle.Y;
            }
            else
            {
                thumbHeight = Height > 0 ? Height : 10;
                thumbWidth = GetThumbSize();

                clickedBarRectangle = ClientRectangle;
                clickedBarRectangle.Inflate(-1, -1);

                thumbRectangle = new Rectangle(ClientRectangle.X, ClientRectangle.Y, thumbWidth, thumbHeight);

                thumbPosition = thumbRectangle.Width/2;
                thumbBottomLimitBottom = ClientRectangle.Right;
                thumbBottomLimitTop = thumbBottomLimitBottom - thumbRectangle.Width;
                thumbTopLimit = ClientRectangle.X;
            }

            ChangeThumbPosition(GetThumbPosition());

            Refresh();
        }

        private void ResetScrollStatus()
        {
            bottomBarClicked = topBarClicked = false;

            StopTimer();
            Refresh();
        }

        private void ProgressTimerTick(object sender, EventArgs e)
        {
            ProgressThumb(true);
        }

        private int GetValue(bool smallIncrement, bool up)
        {
            int newValue;

            if (up)
            {
                newValue = curValue - (smallIncrement ? smallChange : largeChange);

                if (newValue < minimum)
                {
                    newValue = minimum;
                }
            }
            else
            {
                newValue = curValue + (smallIncrement ? smallChange : largeChange);

                if (newValue > maximum)
                {
                    newValue = maximum;
                }
            }

            return newValue;
        }

        private int GetThumbPosition()
        {
            int pixelRange;

            if (thumbHeight == 0 || thumbWidth == 0)
            {
                return 0;
            }

            int thumbSize = metroOrientation == MetroScrollOrientation.Vertical ? (thumbPosition / Height) / thumbHeight : (thumbPosition / Width) / thumbWidth;

            if (Orientation == MetroScrollOrientation.Vertical)
            {
                pixelRange = Height - thumbSize;
            }
            else
            {
                pixelRange = Width - thumbSize;
            }

            int realRange = maximum - minimum;
            float perc = 0f;

            if (realRange != 0)
            {
                perc = (curValue - (float) minimum) / realRange;
            }

            return Math.Max(thumbTopLimit, Math.Min(thumbBottomLimitTop, Convert.ToInt32((perc*pixelRange))));
        }

        private int GetThumbSize()
        {
            int trackSize =
                metroOrientation == MetroScrollOrientation.Vertical ?
                    Height : Width;

            if (maximum == 0 || largeChange == 0)
            {
                return trackSize;
            }

            float newThumbSize = (largeChange * (float) trackSize) / maximum;

            return Convert.ToInt32(Math.Min(trackSize, Math.Max(newThumbSize, 10f)));
        }

        private void EnableTimer()
        {
            if (!progressTimer.Enabled)
            {
                progressTimer.Interval = 600;
                progressTimer.Start();
            }
            else
            {
                progressTimer.Interval = 10;
            }
        }

        private void StopTimer()
        {
            progressTimer.Stop();
        }

        private void ChangeThumbPosition(int position)
        {
            if (Orientation == MetroScrollOrientation.Vertical)
            {
                thumbRectangle.Y = position;
            }
            else
            {
                thumbRectangle.X = position;
            }
        }

        private void ProgressThumb(bool enableTimer)
        {
            int scrollOldValue = curValue;
            var type = ScrollEventType.First;
            int thumbSize, thumbPos;

            if (Orientation == MetroScrollOrientation.Vertical)
            {
                thumbPos = thumbRectangle.Y;
                thumbSize = thumbRectangle.Height;
            }
            else
            {
                thumbPos = thumbRectangle.X;
                thumbSize = thumbRectangle.Width;
            }

            if ((bottomBarClicked && (thumbPos + thumbSize) < trackPosition))
            {
                type = ScrollEventType.LargeIncrement;

                curValue = GetValue(false, false);

                if (curValue == maximum)
                {
                    ChangeThumbPosition(thumbBottomLimitTop);

                    type = ScrollEventType.Last;
                }
                else
                {
                    ChangeThumbPosition(Math.Min(thumbBottomLimitTop, GetThumbPosition()));
                }
            }
            else if ((topBarClicked && thumbPos > trackPosition))
            {
                type = ScrollEventType.LargeDecrement;

                curValue = GetValue(false, true);

                if (curValue == minimum)
                {
                    ChangeThumbPosition(thumbTopLimit);

                    type = ScrollEventType.First;
                }
                else
                {
                    ChangeThumbPosition(Math.Max(thumbTopLimit, GetThumbPosition()));
                }
            }

            if (scrollOldValue != curValue)
            {
                OnScroll(type, scrollOldValue, curValue, scrollOrientation);

                Invalidate();

                if (enableTimer)
                {
                    EnableTimer();
                }
            }
        }

        #endregion
    }
}