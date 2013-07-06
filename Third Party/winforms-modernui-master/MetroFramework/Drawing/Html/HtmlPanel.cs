/**
 * A Professional HTML Renderer You Will Use
 * 
 * The BSD License (BSD)
 * Copyright (c) 2011 Jose Menendez Póo, http://www.codeproject.com/Articles/32376/A-Professional-HTML-Renderer-You-Will-Use
 * 
 * Redistribution and use in source and binary forms, with or without modification, are 
 * permitted provided that the following conditions are met:
 * 
 * Redistributions of source code must retain the above copyright notice, this list of 
 * conditions and the following disclaimer.
 * 
 * Redistributions in binary form must reproduce the above copyright notice, this list of 
 * conditions and the following disclaimer in the documentation and/or other materials 
 * provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS" AND ANY 
 * EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE IMPLIED WARRANTIES 
 * OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE ARE DISCLAIMED. IN NO EVENT 
 * SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE LIABLE FOR ANY DIRECT, INDIRECT, 
 * INCIDENTAL, SPECIAL, EXEMPLARY, OR CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED 
 * TO, PROCUREMENT OF SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR 
 * BUSINESS INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN CONTRACT, 
 * STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE) ARISING IN ANY WAY OUT OF 
 * THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE POSSIBILITY OF SUCH DAMAGE.
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;
using System.Reflection;
using System.ComponentModel;
using System.Drawing.Design;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    [CLSCompliant(false)]
    public class HtmlPanel
        : ScrollableControl
    {
        #region Fields
        protected InitialContainer htmlContainer;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new HtmlPanel
        /// </summary>
        public HtmlPanel()
        {
            htmlContainer = new InitialContainer();

            SetStyle(ControlStyles.ResizeRedraw, true);
            SetStyle(ControlStyles.Opaque, true);
            //SetStyle(ControlStyles.Selectable, true);

            DoubleBuffered = true;

            BackColor = SystemColors.Window;
            AutoScroll = true;

            HtmlRenderer.AddReference(Assembly.GetCallingAssembly());
        }

        #endregion

        #region Properties

        [DesignerSerializationVisibility( DesignerSerializationVisibility.Visible)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;
            }
        }

        [DesignerSerializationVisibility(DesignerSerializationVisibility.Visible)]
        public override bool AutoScroll
        {
            get
            {
                return base.AutoScroll;
            }
            set
            {
                base.AutoScroll = value;
            }
        }

        /// <summary>
        /// Gets the Initial HtmlContainer of this HtmlPanel
        /// </summary>
        public InitialContainer HtmlContainer
        {
            get { return htmlContainer; }
        }

        /// <summary>
        /// Gets or sets the text of this panel
        /// </summary>
        [Editor("System.Windows.Forms.Design.ListControlStringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor)), 
        DesignerSerializationVisibility(DesignerSerializationVisibility.Visible), Localizable(true), Browsable(true), EditorBrowsable(EditorBrowsableState.Always)]
        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                base.Text = value;

                CreateFragment();
                MeasureBounds();
                Invalidate();
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Creates the fragment of HTML that is rendered
        /// </summary>
        protected virtual void CreateFragment()
        {
            htmlContainer = new InitialContainer(Text);
        }

        /// <summary>
        /// Measures the bounds of the container
        /// </summary>
        public virtual void MeasureBounds()
        {
            htmlContainer.SetBounds(this is HtmlLabel ? new Rectangle(0, 0, 10, 10) : ClientRectangle);

            using (Graphics g = CreateGraphics())
            {
                htmlContainer.MeasureBounds(g);
            }
            
            AutoScrollMinSize = Size.Round(htmlContainer.MaximumSize);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);

            Focus();
        }

        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            MeasureBounds();
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);

            if (!(this is  HtmlLabel)) e.Graphics.Clear(SystemColors.Window);

            
            htmlContainer.ScrollOffset = AutoScrollPosition;
            e.Graphics.TextRenderingHint = System.Drawing.Text.TextRenderingHint.ClearTypeGridFit;
            htmlContainer.Paint(e.Graphics);

        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);

            foreach (CssBox box in htmlContainer.LinkRegions.Keys)
            {
                RectangleF rect = htmlContainer.LinkRegions[box];
                if (Rectangle.Round(rect).Contains(e.X, e.Y))
                {
                    Cursor = Cursors.Hand;
                    return;
                }
            }

            Cursor = Cursors.Default;
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

            foreach (CssBox box in htmlContainer.LinkRegions.Keys)
            {
                RectangleF rect = htmlContainer.LinkRegions[box];
                if (Rectangle.Round(rect).Contains(e.X, e.Y))
                {
                    CssValue.GoLink(box.GetAttribute("href", string.Empty));
                    return;
                }
            }

        }

        #endregion
    }
}
