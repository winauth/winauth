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
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Security;
using System.Windows.Forms;

using MetroFramework.Interfaces;
using MetroFramework.Drawing;
using MetroFramework.Components;
using MetroFramework.Native;

namespace MetroFramework.Controls
{
    [ToolboxItem(false)]
    [Designer("MetroFramework.Design.Controls.MetroTabPageDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    public class MetroTabPage : TabPage, IMetroControl
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
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool UseCustomForeColor
        {
            get { return useCustomForeColor; }
            set { useCustomForeColor = value; }
        }

        private bool useStyleColors = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
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

        #region Fields

        private MetroScrollBar verticalScrollbar = new MetroScrollBar(MetroScrollOrientation.Vertical);
        private MetroScrollBar horizontalScrollbar = new MetroScrollBar(MetroScrollOrientation.Horizontal);

        private bool showHorizontalScrollbar = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool HorizontalScrollbar 
        {
            get { return showHorizontalScrollbar; }
            set { showHorizontalScrollbar = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public int HorizontalScrollbarSize
        {
            get { return horizontalScrollbar.ScrollbarSize; }
            set { horizontalScrollbar.ScrollbarSize = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool HorizontalScrollbarBarColor
        {
            get { return horizontalScrollbar.UseBarColor; }
            set { horizontalScrollbar.UseBarColor = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool HorizontalScrollbarHighlightOnWheel
        {
            get { return horizontalScrollbar.HighlightOnWheel; }
            set { horizontalScrollbar.HighlightOnWheel = value; }
        }

        private bool showVerticalScrollbar = false;
        [DefaultValue(false)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool VerticalScrollbar
        {
            get { return showVerticalScrollbar; }
            set { showVerticalScrollbar = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public int VerticalScrollbarSize
        {
            get { return verticalScrollbar.ScrollbarSize; }
            set { verticalScrollbar.ScrollbarSize = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool VerticalScrollbarBarColor
        {
            get { return verticalScrollbar.UseBarColor; }
            set { verticalScrollbar.UseBarColor = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool VerticalScrollbarHighlightOnWheel
        {
            get { return verticalScrollbar.HighlightOnWheel; }
            set { verticalScrollbar.HighlightOnWheel = value; }
        }

        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public new bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                if (value)
                {
                    showHorizontalScrollbar = true;
                    showVerticalScrollbar = true;
                }

                base.AutoScroll = value;
            }
        }

        #endregion

        #region Constructor

        public MetroTabPage()
        {
            SetStyle(ControlStyles.UserPaint |
                     ControlStyles.AllPaintingInWmPaint |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.SupportsTransparentBackColor, true);

            Controls.Add(verticalScrollbar);
            Controls.Add(horizontalScrollbar);

            verticalScrollbar.UseBarColor = true;
            horizontalScrollbar.UseBarColor = true;

            verticalScrollbar.Scroll += VerticalScrollbarScroll;
            horizontalScrollbar.Scroll += HorizontalScrollbarScroll;
        }

        #endregion

        #region Scroll Events

        private void HorizontalScrollbarScroll(object sender, ScrollEventArgs e)
        {
            AutoScrollPosition = new Point(e.NewValue, verticalScrollbar.Value);
            UpdateScrollBarPositions();
        }

        private void VerticalScrollbarScroll(object sender, ScrollEventArgs e)
        {
            AutoScrollPosition = new Point(horizontalScrollbar.Value, e.NewValue);
            UpdateScrollBarPositions();
        }

        #endregion

        #region Overridden Methods

        protected override void OnPaintBackground(PaintEventArgs e)
        {
            try
            {
                Color backColor = BackColor;

                if (!useCustomBackColor)
                {
                    backColor = MetroPaint.BackColor.Form(Theme);
                }

                if (backColor.A == 255 && BackgroundImage == null)
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
            base.OnPaint(e);

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
            if (DesignMode)
            {
                horizontalScrollbar.Visible = false;
                verticalScrollbar.Visible = false;
                return;
            }

            UpdateScrollBarPositions();

            if (HorizontalScrollbar)
            {
                horizontalScrollbar.Visible = HorizontalScroll.Visible;                
            }
            if (HorizontalScroll.Visible)
            {
                horizontalScrollbar.Minimum = HorizontalScroll.Minimum;
                horizontalScrollbar.Maximum = HorizontalScroll.Maximum;
                horizontalScrollbar.SmallChange = HorizontalScroll.SmallChange;
                horizontalScrollbar.LargeChange = HorizontalScroll.LargeChange;
            }

            if (VerticalScrollbar)
            {
                verticalScrollbar.Visible = VerticalScroll.Visible;                
            }
            if (VerticalScroll.Visible)
            {
                verticalScrollbar.Minimum = VerticalScroll.Minimum;
                verticalScrollbar.Maximum = VerticalScroll.Maximum;
                verticalScrollbar.SmallChange = VerticalScroll.SmallChange;
                verticalScrollbar.LargeChange = VerticalScroll.LargeChange;
            }

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);

            verticalScrollbar.Value = VerticalScroll.Value;
            horizontalScrollbar.Value = HorizontalScroll.Value;
        }

        [SecuritySafeCritical]
        protected override void WndProc(ref Message m)
        {
            base.WndProc(ref m);

            if (!DesignMode)
            {
                WinApi.ShowScrollBar(Handle, (int)WinApi.ScrollBar.SB_BOTH, 0);
            }
        }

        #endregion

        #region Management Methods

        private void UpdateScrollBarPositions()
        {
            if (DesignMode)
            {
                return;
            }

            if (!AutoScroll)
            {
                verticalScrollbar.Visible = false;
                horizontalScrollbar.Visible = false;
                return;
            }

            verticalScrollbar.Location = new Point(ClientRectangle.Width - verticalScrollbar.Width, ClientRectangle.Y);
            verticalScrollbar.Height = ClientRectangle.Height;

            if (!VerticalScrollbar)
            {
                verticalScrollbar.Visible = false;
            }

            horizontalScrollbar.Location = new Point(ClientRectangle.X, ClientRectangle.Height - horizontalScrollbar.Height);
            horizontalScrollbar.Width = ClientRectangle.Width;

            if (!HorizontalScrollbar)
            {
                horizontalScrollbar.Visible = false;
            }
        }

        #endregion
    }
}
