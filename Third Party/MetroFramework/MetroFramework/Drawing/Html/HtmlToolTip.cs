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
using System.ComponentModel;
using System.Globalization;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Provides HTML rendering on the tooltips
    /// </summary>
    public class HtmlToolTip
        : ToolTip
    {
        #region Fields

        private InitialContainer container;

        #endregion

        #region Ctor

        public HtmlToolTip()
        {

            OwnerDraw = true;

            Popup += new PopupEventHandler(HtmlToolTip_Popup);
            Draw += new DrawToolTipEventHandler(HtmlToolTip_Draw);

        }

        #endregion

        void HtmlToolTip_Popup(object sender, PopupEventArgs e)
        {
            string text = this.GetToolTip(e.AssociatedControl);
            string font = string.Format(NumberFormatInfo.InvariantInfo, "font: {0}pt {1}", e.AssociatedControl.Font.Size, e.AssociatedControl.Font.FontFamily.Name);
            
            //Create fragment container
            container = new InitialContainer("<table class=htmltooltipbackground cellspacing=5 cellpadding=0 style=\"" + font + "\"><tr><td style=border:0px>" + text + "</td></tr></table>");
            container.SetBounds(new Rectangle(0, 0, 10, 10));
            container.AvoidGeometryAntialias = true;
            
            //Measure bounds of the container
            using (Graphics g = e.AssociatedControl.CreateGraphics())
            {
                container.MeasureBounds(g);
            }

            //Set the size of the tooltip
            e.ToolTipSize = Size.Round(container.MaximumSize);

        }

        void HtmlToolTip_Draw(object sender, DrawToolTipEventArgs e)
        {
            e.Graphics.Clear(Color.White);

            if (container != null)
            {
                //Draw HTML!
                container.Paint(e.Graphics);
            }

        }
    }
}
