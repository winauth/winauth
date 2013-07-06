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
using System.Reflection;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    public static class HtmlRenderer
    {
        #region References

        /// <summary>
        /// List of assembly references
        /// </summary>
        private static List<Assembly> _references;

        /// <summary>
        /// Gets a list of Assembly references used to search for external references
        /// </summary>
        /// <remarks>
        /// This references are used when loading images and other content, when
        /// rendering a piece of HTML/CSS
        /// </remarks>
        public static List<Assembly> References
        {
            get { return _references; }
        }

        /// <summary>
        /// Adds a reference to the References list if not yet listed
        /// </summary>
        /// <param name="assembly"></param>
        internal static void AddReference(Assembly assembly)
        {
            if (!References.Contains(assembly))
            {
                References.Add(assembly);
            }
        }

        static HtmlRenderer()
        {
            //Initialize references list
            _references = new List<Assembly>();

            //Add this assembly as a reference
            References.Add(Assembly.GetExecutingAssembly());
        }

        #endregion

        #region Methods

        /// <summary>
        /// Draws the HTML on the specified point using the specified width.
        /// </summary>
        /// <param name="g">Device to draw</param>
        /// <param name="html">HTML source</param>
        /// <param name="location">Point to start drawing</param>
        /// <param name="width">Width to fit HTML drawing</param>
        public static void Render(Graphics g, string html, PointF location, float width)
        {
            Render(g, html, new RectangleF(location, new SizeF(width, 0)), false);
        }

        /// <summary>
        /// Renders the specified HTML source on the specified area clipping if specified
        /// </summary>
        /// <param name="g">Device to draw</param>
        /// <param name="html">HTML source</param>
        /// <param name="area">Area where HTML should be drawn</param>
        /// <param name="clip">If true, it will only paint on the specified area</param>
        public static void Render(Graphics g, string html, RectangleF area, bool clip)
        {
            InitialContainer container = new InitialContainer(html);
            Region prevClip = g.Clip;

            if (clip) g.SetClip(area);

            container.SetBounds(area);
            container.MeasureBounds(g);
            container.Paint(g);

            if (clip) g.SetClip(prevClip, System.Drawing.Drawing2D.CombineMode.Replace);
        }

        #endregion
    }
}
