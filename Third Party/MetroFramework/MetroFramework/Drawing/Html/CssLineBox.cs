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
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Represents a line of text.
    /// </summary>
    /// <remarks>
    /// To learn more about line-boxes see CSS spec:
    /// http://www.w3.org/TR/CSS21/visuren.html
    /// </remarks>
    internal class CssLineBox
    {

        #region Fields

        private List<CssBoxWord> _words;
        private CssBox _ownerBox;
        private Dictionary<CssBox, RectangleF> _rects;
        private List<CssBox> _relatedBoxes;

        #endregion

        #region Ctors

        /// <summary>
        /// Creates a new LineBox
        /// </summary>
        public CssLineBox(CssBox ownerBox)
        {
            _rects = new Dictionary<CssBox, RectangleF>();
            _relatedBoxes = new List<CssBox>();
            _words = new List<CssBoxWord>();
            _ownerBox = ownerBox;
            _ownerBox.LineBoxes.Add(this);
        }

        #endregion

        #region Props

        

        /// <summary>
        /// Gets a list of boxes related with the linebox. 
        /// To know the words of the box inside this linebox, use the <see cref="WordsOf"/> method.
        /// </summary>
        public List<CssBox> RelatedBoxes
        {
            get { return _relatedBoxes; }
        }


        /// <summary>
        /// Gets the words inside the linebox
        /// </summary>
        public List<CssBoxWord> Words
        {
            get { return _words; }
        }

        /// <summary>
        /// Gets the owner box
        /// </summary>
        public CssBox OwnerBox
        {
            get { return _ownerBox; }
        }

        /// <summary>
        /// Gets a List of rectangles that are to be painted on this linebox
        /// </summary>
        public Dictionary<CssBox, RectangleF> Rectangles
        {
            get { return _rects; }
        }


        #endregion

        #region Methods


       
        /// <summary>
        /// Gets the maximum bottom of the words
        /// </summary>
        /// <returns></returns>
        public float GetMaxWordBottom()
        {
            float res = float.MinValue;

            foreach (CssBoxWord word in Words)
            {
                res = Math.Max(res, word.Bottom);
            }

            return res;
        }

        #endregion

        /// <summary>
        /// Lets the linebox add the word an its box to their lists if necessary.
        /// </summary>
        /// <param name="word"></param>
        internal void ReportExistanceOf(CssBoxWord word)
        {
            if (!Words.Contains(word))
            {
                Words.Add(word);
            }

            if (!RelatedBoxes.Contains(word.OwnerBox))
            {
                RelatedBoxes.Add(word.OwnerBox);
            }
        }

        /// <summary>
        /// Return the words of the specified box that live in this linebox
        /// </summary>
        /// <param name="box"></param>
        /// <returns></returns>
        internal List<CssBoxWord> WordsOf(CssBox box)
        {
            List<CssBoxWord> r = new List<CssBoxWord>();

            foreach (CssBoxWord word in Words)
                if (word.OwnerBox.Equals(box)) r.Add(word);

            return r;
        }

        /// <summary>
        /// Updates the specified rectangle of the specified box.
        /// </summary>
        /// <param name="box"></param>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="r"></param>
        /// <param name="b"></param>
        internal void UpdateRectangle(CssBox box, float x, float y, float r, float b)
        {
            float leftspacing = box.ActualBorderLeftWidth + box.ActualPaddingLeft;
            float rightspacing = box.ActualBorderRightWidth + box.ActualPaddingRight;
            float topspacing = box.ActualBorderTopWidth + box.ActualPaddingTop;
            float bottomspacing = box.ActualBorderBottomWidth + box.ActualPaddingTop;

            if ((box.FirstHostingLineBox != null && box.FirstHostingLineBox.Equals(this)) || box.IsImage) x -= leftspacing;
            if ((box.LastHostingLineBox != null && box.LastHostingLineBox.Equals(this)) || box.IsImage) r += rightspacing;

            if (!box.IsImage)
            {
                y -= topspacing;
                b += bottomspacing;
            }
            

            if (!Rectangles.ContainsKey(box))
            {
                Rectangles.Add(box, RectangleF.FromLTRB(x, y, r, b));
            }
            else
            {
                RectangleF f = Rectangles[box];
                Rectangles[box] = RectangleF.FromLTRB(
                    Math.Min(f.X, x), Math.Min(f.Y, y),
                    Math.Max(f.Right, r), Math.Max(f.Bottom, b));
            }

            if (box.ParentBox != null && box.ParentBox.Display == CssConstants.Inline)
            {
                UpdateRectangle(box.ParentBox, x, y, r, b);
            }
        }

        /// <summary>
        /// Copies the rectangles to their specified box
        /// </summary>
        internal void AssignRectanglesToBoxes()
        {
            foreach (CssBox b in Rectangles.Keys)
            {
                b.Rectangles.Add(this, Rectangles[b]);
            }
        }

        /// <summary>
        /// Draws the rectangles for debug purposes
        /// </summary>
        /// <param name="g"></param>
        internal void DrawRectangles(Graphics g)
        {
            foreach (CssBox b in Rectangles.Keys)
            {
                if (float.IsInfinity(Rectangles[b].Width)) 
                    continue;
                g.FillRectangle(new SolidBrush(Color.FromArgb(50, Color.Black)),
                    Rectangle.Round(Rectangles[b]));
                g.DrawRectangle(Pens.Red, Rectangle.Round(Rectangles[b]));
            }
        }

        /// <summary>
        /// Gets the baseline Height of the rectangle
        /// </summary>
        /// <param name="g"></param>
        /// <returns></returns>
        public float GetBaseLineHeight(CssBox b, Graphics g)
        {
            Font f = b.ActualFont;
            FontFamily ff = f.FontFamily;
            FontStyle s = f.Style;
            return f.GetHeight(g) * ff.GetCellAscent(s) / ff.GetLineSpacing(s);
        }

        /// <summary>
        /// Sets the baseline of the words of the specified box to certain height
        /// </summary>
        /// <param name="g">Device info</param>
        /// <param name="b">box to check words</param>
        /// <param name="baseline">baseline</param>
        internal void SetBaseLine(Graphics g,CssBox b, float baseline)
        {
            //TODO: Aqui me quede, checar poniendo "by the" con un font-size de 3em
            List<CssBoxWord> ws = WordsOf(b);

            if (!Rectangles.ContainsKey(b)) return;

            RectangleF r = Rectangles[b];

            //Save top of words related to the top of rectangle
            float gap = 0f;

            if (ws.Count > 0)
            {
                gap = ws[0].Top - r.Top;
            }
            else
            {
                CssBoxWord firstw = b.FirstWordOccourence(b, this);

                if (firstw != null)
                {
                    gap = firstw.Top - r.Top;
                }
            }

            //New top that words will have
            //float newtop = baseline - (Height - OwnerBox.FontDescent - 3); //OLD
            float newtop = baseline - GetBaseLineHeight(b, g); //OLD

            if (b.ParentBox != null &&
                b.ParentBox.Rectangles.ContainsKey(this) &&
                r.Height < b.ParentBox.Rectangles[this].Height)
            {
                //Do this only if rectangle is shorter than parent's
                float recttop = newtop - gap;
                RectangleF newr = new RectangleF(r.X, recttop, r.Width, r.Height);
                Rectangles[b] = newr;
                b.OffsetRectangle(this, gap);
            }
            foreach (CssBoxWord w in ws)
                if (!w.IsImage)
                    w.Top = newtop;
        }

        /// <summary>
        /// Returns the words of the linebox
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string[] ws = new string[Words.Count];
            for (int i = 0; i < ws.Length; i++)
            {
                ws[i] = Words[i].Text;
            }
            return string.Join(" ", ws);
        }
    }
}
