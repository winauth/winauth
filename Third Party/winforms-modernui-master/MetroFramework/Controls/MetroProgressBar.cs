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
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    [Designer("MetroFramework.Design.Controls.MetroProgressBarDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(ProgressBar))]
    public class MetroProgressBar : ProgressBar, IMetroControl
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
        public new MetroColorStyle Style
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

        private bool useStyleColors = true;
        [Browsable(false)]
        [DefaultValue(true)]
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

        #region Fields

        private MetroProgressBarSize metroLabelSize = MetroProgressBarSize.Medium;
        [DefaultValue(MetroProgressBarSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroProgressBarSize FontSize
        {
            get { return metroLabelSize; }
            set { metroLabelSize = value; }
        }

        private MetroProgressBarWeight metroLabelWeight = MetroProgressBarWeight.Light;
        [DefaultValue(MetroProgressBarWeight.Light)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroProgressBarWeight FontWeight
        {
            get { return metroLabelWeight; }
            set { metroLabelWeight = value; }
        }

        private ContentAlignment textAlign = ContentAlignment.MiddleRight;
        [DefaultValue(ContentAlignment.MiddleRight)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public ContentAlignment TextAlign
        {
            get { return textAlign; }
            set { textAlign = value; }
        }

        private bool hideProgressText = true;
        [DefaultValue(true)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public bool HideProgressText
        {
            get { return hideProgressText; }
            set { hideProgressText = value; }
        }

        private ProgressBarStyle progressBarStyle = ProgressBarStyle.Continuous;
        [DefaultValue(ProgressBarStyle.Continuous)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public ProgressBarStyle ProgressBarStyle
        {
            get { return progressBarStyle; }
            set { progressBarStyle = value; }
        }

        public new int Value
        {
            get { return base.Value; }
            set { if (value > Maximum) return; base.Value = value; Invalidate(); }
        }

        [Browsable(false)]
        public double ProgressTotalPercent
        {
            get { return ((1 - (double)(Maximum - Value) / Maximum) * 100); }
        }

        [Browsable(false)]
        public double ProgressTotalValue
        {
            get { return (1 - (double)(Maximum - Value) / Maximum); }
        }

        [Browsable(false)]
        public string ProgressPercentText
        {
            get { return (string.Format("{0}%", Math.Round(ProgressTotalPercent))); }
        }

        private double ProgressBarWidth
        {
            get { return (((double)Value / Maximum) * ClientRectangle.Width); }
        }

        private int ProgressBarMarqueeWidth
        {
            get { return (ClientRectangle.Width / 3); }
        }

        #endregion

        #region Constructor

        public MetroProgressBar()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);
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
                    if (!Enabled)
                    {
                        backColor = MetroPaint.BackColor.ProgressBar.Bar.Disabled(Theme);
                    }
                    else
                    {
                        backColor = MetroPaint.BackColor.ProgressBar.Bar.Normal(Theme);
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
            if (progressBarStyle == ProgressBarStyle.Continuous)
            {
                if (!DesignMode) StopTimer();

                DrawProgressContinuous(e.Graphics);
            }
            else if (progressBarStyle == ProgressBarStyle.Blocks)
            {
                if (!DesignMode) StopTimer();

                DrawProgressContinuous(e.Graphics);
            }
            else if (progressBarStyle == ProgressBarStyle.Marquee)
            {
                if (!DesignMode && Enabled) StartTimer();
                if (!Enabled) StopTimer();

                if (Value == Maximum)
                {
                    StopTimer();
                    DrawProgressContinuous(e.Graphics);
                }
                else
                {
                    DrawProgressMarquee(e.Graphics);
                }
            }

            DrawProgressText(e.Graphics);

            using (Pen p = new Pen(MetroPaint.BorderColor.ProgressBar.Normal(Theme)))
            {
                Rectangle borderRect = new Rectangle(0, 0, Width - 1, Height - 1);
                e.Graphics.DrawRectangle(p, borderRect);
            }

            OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, Color.Empty, e.Graphics));
        }

        private void DrawProgressContinuous(Graphics graphics)
        {
            graphics.FillRectangle(MetroPaint.GetStyleBrush(Style), 0, 0, (int)ProgressBarWidth, ClientRectangle.Height);
        }

        private int marqueeX = 0;

        private void DrawProgressMarquee(Graphics graphics)
        {
            graphics.FillRectangle(MetroPaint.GetStyleBrush(Style), marqueeX, 0, ProgressBarMarqueeWidth, ClientRectangle.Height);
        }

        private void DrawProgressText(Graphics graphics)
        {
            if (HideProgressText) return;

            Color foreColor;

            if (!Enabled)
            {
                foreColor = MetroPaint.ForeColor.ProgressBar.Disabled(Theme);
            }
            else
            {
                foreColor = MetroPaint.ForeColor.ProgressBar.Normal(Theme);
            }
           
            TextRenderer.DrawText(graphics, ProgressPercentText, MetroFonts.ProgressBar(metroLabelSize, metroLabelWeight), ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
        }

        #endregion

        #region Overridden Methods

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (var g = CreateGraphics())
            {
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, ProgressPercentText, MetroFonts.ProgressBar(metroLabelSize, metroLabelWeight), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
            }

            return preferredSize;
        }

        #endregion

        #region Private Methods

        private Timer marqueeTimer;
        private bool marqueeTimerEnabled = false;

        private void StartTimer()
        {
            if (marqueeTimerEnabled) return;

            if (marqueeTimer == null)
            {
                marqueeTimer = new Timer();
                marqueeTimer.Interval = 10;
                marqueeTimer.Tick += new EventHandler(marqueeTimer_Tick);
            }

            marqueeX = -ProgressBarMarqueeWidth;
    
            marqueeTimer.Stop();
            marqueeTimer.Start();

            marqueeTimerEnabled = true;

            Invalidate();
        }

        private void StopTimer()
        {
            if (marqueeTimer == null) return;

            marqueeTimer.Stop();

            Invalidate();
        }

        private void marqueeTimer_Tick(object sender, EventArgs e)
        {
            marqueeX++;

            if (marqueeX > ClientRectangle.Width)
            {
                marqueeX = -ProgressBarMarqueeWidth;
            }

            Invalidate();
        }

        #endregion
    }
}
