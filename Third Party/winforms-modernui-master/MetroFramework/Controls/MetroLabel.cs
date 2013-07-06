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
using System.ComponentModel;
using System.Security;
using System.Windows.Forms;

using MetroFramework.Components;
using MetroFramework.Drawing;
using MetroFramework.Interfaces;

namespace MetroFramework.Controls
{
    #region Enums

    public enum MetroLabelMode
    {
        Default,
        Selectable
    }

    #endregion

    [Designer("MetroFramework.Design.Controls.MetroLabelDesigner, " + AssemblyRef.MetroFrameworkDesignSN)]
    [ToolboxBitmap(typeof(Label))]
    public class MetroLabel : Label, IMetroControl
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

        private DoubleBufferedTextBox baseTextBox;

        private MetroLabelSize metroLabelSize = MetroLabelSize.Medium;
        [DefaultValue(MetroLabelSize.Medium)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelSize FontSize
        {
            get { return metroLabelSize; }
            set { metroLabelSize = value; Refresh(); }
        }

        private MetroLabelWeight metroLabelWeight = MetroLabelWeight.Light;
        [DefaultValue(MetroLabelWeight.Light)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelWeight FontWeight
        {
            get { return metroLabelWeight; }
            set { metroLabelWeight = value; Refresh(); }
        }

        private MetroLabelMode labelMode = MetroLabelMode.Default;
        [DefaultValue(MetroLabelMode.Default)]
        [Category(MetroDefaults.PropertyCategory.Appearance)]
        public MetroLabelMode LabelMode
        {
            get { return labelMode; }
            set { labelMode = value; }
        }

        #endregion

        #region Constructor

        public MetroLabel()
        {
            SetStyle(ControlStyles.SupportsTransparentBackColor |
                     ControlStyles.OptimizedDoubleBuffer |
                     ControlStyles.ResizeRedraw |
                     ControlStyles.UserPaint, true);

            baseTextBox = new DoubleBufferedTextBox();
            baseTextBox.Visible = false;
            Controls.Add(baseTextBox);
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
                    backColor = MetroPaint.BackColor.Form(Theme);
                    if (Parent is MetroTile)
                    {
                        backColor = MetroPaint.GetStyleColor(Style);
                    }
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
            Color foreColor;

            if (useCustomForeColor)
            {
                foreColor = ForeColor;
            }
            else
            {
                if (!Enabled)
                {
                    if (Parent != null)
                    {
                        if (Parent is MetroTile)
                        {
                            foreColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
                        }
                        else
                        {
                            foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                    else
                    {
                        foreColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                    }
                }
                else
                {
                    if (Parent != null)
                    {
                        if (Parent is MetroTile)
                        {
                            foreColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                        }
                        else
                        {
                            if (useStyleColors)
                            {
                                foreColor = MetroPaint.GetStyleColor(Style);
                            }
                            else
                            {
                                foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                            }
                        }
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            foreColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            foreColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                }
            }

            if (LabelMode == MetroLabelMode.Selectable)
            {
                CreateBaseTextBox();
                UpdateBaseTextBox();

                if (!baseTextBox.Visible)
                {
                    TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight), ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
                }
            }
            else
            {
                DestroyBaseTextbox();
                TextRenderer.DrawText(e.Graphics, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight), ClientRectangle, foreColor, MetroPaint.GetTextFormatFlags(TextAlign));
                OnCustomPaintForeground(new MetroPaintEventArgs(Color.Empty, foreColor, e.Graphics));
            }
        }

        #endregion

        #region Overridden Methods

        public override void Refresh()
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                UpdateBaseTextBox();
            }
            
            base.Refresh();
        }

        public override Size GetPreferredSize(Size proposedSize)
        {
            Size preferredSize;
            base.GetPreferredSize(proposedSize);

            using (var g = CreateGraphics())
            {
                proposedSize = new Size(int.MaxValue, int.MaxValue);
                preferredSize = TextRenderer.MeasureText(g, Text, MetroFonts.Label(metroLabelSize, metroLabelWeight), proposedSize, MetroPaint.GetTextFormatFlags(TextAlign));
            }

            return preferredSize;
        }

        protected override void OnEnabledChanged(EventArgs e)
        {
            base.OnEnabledChanged(e);
            Invalidate();
        }

        protected override void OnResize(EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                HideBaseTextBox();
            }

            base.OnResize(e);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);

            if (LabelMode == MetroLabelMode.Selectable)
            {
                ShowBaseTextBox();
            }
        }

        #endregion

        #region Label Selection Mode

        private class DoubleBufferedTextBox : TextBox
        {
            public DoubleBufferedTextBox()
            {
                SetStyle(ControlStyles.SupportsTransparentBackColor | ControlStyles.OptimizedDoubleBuffer, true);
            }
        }

        private bool firstInitialization = true; 

        private void CreateBaseTextBox()
        {
            if (baseTextBox.Visible && !firstInitialization) return;
            if (!firstInitialization) return;

            firstInitialization = false;

            if (!DesignMode)
            {
                Form parentForm = FindForm();
                if (parentForm != null)
                {
                    parentForm.ResizeBegin += new EventHandler(parentForm_ResizeBegin);
                    parentForm.ResizeEnd += new EventHandler(parentForm_ResizeEnd);
                }
            }

            baseTextBox.BackColor = Color.Transparent;
            baseTextBox.Visible = true;
            baseTextBox.BorderStyle = BorderStyle.None;
            baseTextBox.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);
            baseTextBox.Location = new Point(1, 0);
            baseTextBox.Text = Text;
            baseTextBox.ReadOnly = true;

            baseTextBox.Size = GetPreferredSize(Size.Empty);
            baseTextBox.Multiline = true;
            
            baseTextBox.DoubleClick += BaseTextBoxOnDoubleClick;
            baseTextBox.Click += BaseTextBoxOnClick;

            Controls.Add(baseTextBox);
        }

        private void parentForm_ResizeEnd(object sender, EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                ShowBaseTextBox();
            }
        }

        private void parentForm_ResizeBegin(object sender, EventArgs e)
        {
            if (LabelMode == MetroLabelMode.Selectable)
            {
                HideBaseTextBox();
            }
        }

        private void DestroyBaseTextbox()
        {
            if (!baseTextBox.Visible) return;
            
            baseTextBox.DoubleClick -= BaseTextBoxOnDoubleClick;
            baseTextBox.Click -= BaseTextBoxOnClick;
            baseTextBox.Visible = false;
        }

        private void UpdateBaseTextBox()
        {
            if (!baseTextBox.Visible) return;

            SuspendLayout();
            baseTextBox.SuspendLayout();

            if (useCustomBackColor)
                baseTextBox.BackColor = BackColor;
            else
                baseTextBox.BackColor = MetroPaint.BackColor.Form(Theme);

            if (!Enabled)
            {
                if (Parent != null)
                {
                    if (Parent is MetroTile)
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Tile.Disabled(Theme);
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                        }
                    }
                }
                else
                {
                    if (useStyleColors)
                    {
                        baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Disabled(Theme);
                    }
                }
            }
            else
            {
                if (Parent != null)
                {
                    if (Parent is MetroTile)
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Tile.Normal(Theme);
                    }
                    else
                    {
                        if (useStyleColors)
                        {
                            baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                        }
                        else
                        {
                            baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
                        }
                    }
                }
                else
                {
                    if (useStyleColors)
                    {
                        baseTextBox.ForeColor = MetroPaint.GetStyleColor(Style);
                    }
                    else
                    {
                        baseTextBox.ForeColor = MetroPaint.ForeColor.Label.Normal(Theme);
                    }
                }
            }

            baseTextBox.Font = MetroFonts.Label(metroLabelSize, metroLabelWeight);
            baseTextBox.Text = Text;
            baseTextBox.BorderStyle = BorderStyle.None;

            Size = GetPreferredSize(Size.Empty);

            baseTextBox.ResumeLayout();
            ResumeLayout();
        }

        private void HideBaseTextBox()
        {
            baseTextBox.Visible = false;
        }

        private void ShowBaseTextBox()
        {
            baseTextBox.Visible = true;
        }

        [SecuritySafeCritical]
        private void BaseTextBoxOnClick(object sender, EventArgs eventArgs)
        {
            Native.WinCaret.HideCaret(baseTextBox.Handle);
        }

        [SecuritySafeCritical]
        private void BaseTextBoxOnDoubleClick(object sender, EventArgs eventArgs)
        {
            baseTextBox.SelectAll();
            Native.WinCaret.HideCaret(baseTextBox.Handle);
        }

        #endregion
    }
}
