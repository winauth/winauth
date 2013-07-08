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
using System.ComponentModel;

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Provides HTML rendering on the text of the label
    /// </summary>
    [CLSCompliant(false)]
    public class HtmlLabel
        : HtmlPanel
    {
        #region Fields

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new HTML Label
        /// </summary>
        public HtmlLabel()
        {
            SetStyle(System.Windows.Forms.ControlStyles.Opaque, false);

            AutoScroll = false;
        }

        #endregion

        #region Properties

        [DefaultValue(true)]
        [Description("Automatically sets the size of the label by measuring the content")]
        [Browsable(true)]
        public override bool AutoSize
        {
            get
            {
                return base.AutoSize;
            }
            set
            {
                base.AutoSize = value;

                if (value)
                {
                    MeasureBounds();
                }
            }
        }

        #endregion

        #region Methods

        protected override void CreateFragment()
        {
            string text = Text;
            string font = string.Format("font: {0}pt {1}", Font.Size, Font.FontFamily.Name);

            //Create fragment container
            htmlContainer = new InitialContainer("<table border=0 cellspacing=5 cellpadding=0 style=\"" + font + "\"><tr><td>" + text + "</td></tr></table>");
            //_htmlContainer.SetBounds(new Rectangle(0, 0, 10, 10));
            
        }

        public override void MeasureBounds()
        {
            base.MeasureBounds();

            if(htmlContainer != null && AutoSize)
                Size = System.Drawing.Size.Round(htmlContainer.MaximumSize);
        }

        #endregion
    }
}
