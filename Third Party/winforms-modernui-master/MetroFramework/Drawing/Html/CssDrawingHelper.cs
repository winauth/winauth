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
using System.Drawing.Drawing2D;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Provides some drawing functionallity
    /// </summary>
    internal static class CssDrawingHelper
    {
        /// <summary>
        /// Border specifiers
        /// </summary>
        internal enum Border
        {
            Top, Right, Bottom, Left
        }

        /// <summary>
        /// Rounds the specified point
        /// </summary>
        /// <param name="p"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static PointF RoundP(PointF p, CssBox b)
        {
            //HACK: Don't round if in printing mode
            //return Point.Round(p);
            return p;
        }

        /// <summary>
        /// Rounds the specified rectangle
        /// </summary>
        /// <param name="p"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        private static RectangleF RoundR(RectangleF r, CssBox b)
        {
            //HACK: Don't round if in printing mode
            return Rectangle.Round(r);
        }

        /// <summary>
        /// Makes a border path
        /// </summary>
        /// <param name="border">Desired border</param>
        /// <param name="b">Box wich the border corresponds</param>
        /// <param name="isLineStart">Specifies if the border is for a starting line (no bevel on left)</param>
        /// <param name="isLineEnd">Specifies if the border is for an ending line (no bevel on right)</param>
        /// <returns>Beveled border path</returns>
        public static GraphicsPath GetBorderPath(Border border, CssBox b, RectangleF r, bool isLineStart, bool isLineEnd)
        {
            PointF[] pts = new PointF[4];
            float bwidth = 0;
            GraphicsPath corner = null;

            switch (border)
            {
                case Border.Top:
                    bwidth = b.ActualBorderTopWidth;
                    pts[0] = RoundP(new PointF(r.Left + b.ActualCornerNW, r.Top), b);
                    pts[1] = RoundP(new PointF(r.Right - b.ActualCornerNE, r.Top), b);
                    pts[2] = RoundP(new PointF(r.Right - b.ActualCornerNE, r.Top + bwidth), b);
                    pts[3] = RoundP(new PointF(r.Left + b.ActualCornerNW, r.Top + bwidth), b);

                    if (isLineEnd && b.ActualCornerNE == 0f) pts[2].X -= b.ActualBorderRightWidth;
                    if (isLineStart && b.ActualCornerNW == 0f) pts[3].X += b.ActualBorderLeftWidth;

                    if (b.ActualCornerNW > 0f) corner = CreateCorner(b, r, 1);
                    
                    break;
                case Border.Right:
                    bwidth = b.ActualBorderRightWidth;
                    pts[0] = RoundP(new PointF(r.Right - bwidth, r.Top + b.ActualCornerNE), b);
                    pts[1] = RoundP(new PointF(r.Right, r.Top + b.ActualCornerNE), b);
                    pts[2] = RoundP(new PointF(r.Right, r.Bottom - b.ActualCornerSE), b);
                    pts[3] = RoundP(new PointF(r.Right - bwidth, r.Bottom - b.ActualCornerSE), b);
                    
                   
                    if (b.ActualCornerNE == 0f) pts[0].Y += b.ActualBorderTopWidth;
                    if (b.ActualCornerSE == 0f) pts[3].Y -= b.ActualBorderBottomWidth; 
                    if (b.ActualCornerNE > 0f) corner = CreateCorner(b, r, 2);
                    break;
                case Border.Bottom:
                    bwidth = b.ActualBorderBottomWidth;
                    pts[0] = RoundP(new PointF(r.Left + b.ActualCornerSW, r.Bottom - bwidth), b);
                    pts[1] = RoundP(new PointF(r.Right - b.ActualCornerSE, r.Bottom - bwidth), b);
                    pts[2] = RoundP(new PointF(r.Right - b.ActualCornerSE, r.Bottom), b);
                    pts[3] = RoundP(new PointF(r.Left + b.ActualCornerSW, r.Bottom), b);

                    if (isLineStart && b.ActualCornerSW == 0f) pts[0].X += b.ActualBorderLeftWidth;
                    if (isLineEnd && b.ActualCornerSE == 0f) pts[1].X -= b.ActualBorderRightWidth;

                    if (b.ActualCornerSE > 0f) corner = CreateCorner(b, r, 3);
                    break;
                case Border.Left:
                    bwidth = b.ActualBorderLeftWidth;
                    pts[0] = RoundP(new PointF(r.Left, r.Top + b.ActualCornerNW), b);
                    pts[1] = RoundP(new PointF(r.Left + bwidth, r.Top + b.ActualCornerNW), b);
                    pts[2] = RoundP(new PointF(r.Left + bwidth, r.Bottom - b.ActualCornerSW), b);
                    pts[3] = RoundP(new PointF(r.Left, r.Bottom - b.ActualCornerSW), b);

                    if (b.ActualCornerNW == 0f) pts[1].Y += b.ActualBorderTopWidth;
                    if (b.ActualCornerSW == 0f) pts[2].Y -= b.ActualBorderBottomWidth;

                    if (b.ActualCornerSW > 0f) corner = CreateCorner(b, r, 4);
                    break;
            }

            GraphicsPath path = new GraphicsPath(pts, new byte[] { (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line, (byte)PathPointType.Line });

            if (corner != null)
            {
                path.AddPath(corner, true);
            }

            return path;
        }

        /// <summary>
        /// Creates the corner to place with the borders
        /// </summary>
        /// <param name="outer"></param>
        /// <param name="inner"></param>
        /// <param name="startAngle"></param>
        /// <param name="sweepAngle"></param>
        /// <returns></returns>
        private static GraphicsPath CreateCorner(CssBox b, RectangleF r, int cornerIndex)
        {
            GraphicsPath corner = new GraphicsPath();

            RectangleF outer = RectangleF.Empty;
            RectangleF inner = RectangleF.Empty;
            float start1 = 0;
            float start2 = 0;

            switch (cornerIndex)
            {
                case 1:
                    outer = new RectangleF(r.Left, r.Top, b.ActualCornerNW, b.ActualCornerNW);
                    inner = RectangleF.FromLTRB(outer.Left + b.ActualBorderLeftWidth, outer.Top + b.ActualBorderTopWidth, outer.Right, outer.Bottom);
                    start1 = 180;
                    start2 = 270;
                    break;
                case 2:
                    outer = new RectangleF(r.Right - b.ActualCornerNE, r.Top, b.ActualCornerNE, b.ActualCornerNE);
                    inner = RectangleF.FromLTRB(outer.Left, outer.Top + b.ActualBorderTopWidth, outer.Right - b.ActualBorderRightWidth, outer.Bottom);
                    outer.X -= outer.Width;
                    inner.X -= inner.Width;
                    start1 = -90;
                    start2 = 0;
                    break;
                case 3:
                    outer = RectangleF.FromLTRB(r.Right - b.ActualCornerSE, r.Bottom - b.ActualCornerSE, r.Right, r.Bottom);
                    inner = new RectangleF(outer.Left, outer.Top, outer.Width - b.ActualBorderRightWidth, outer.Height - b.ActualBorderBottomWidth);
                    outer.X -= outer.Width;
                    outer.Y -= outer.Height;
                    inner.X -= inner.Width;
                    inner.Y -= inner.Height;
                    start1 = 0;
                    start2 = 90;
                    break;
                case 4:
                    outer = new RectangleF(r.Left, r.Bottom - b.ActualCornerSW, b.ActualCornerSW, b.ActualCornerSW);
                    inner = RectangleF.FromLTRB( r.Left + b.ActualBorderLeftWidth , outer.Top , outer.Right, outer.Bottom - b.ActualBorderBottomWidth);
                    start1 = 90;
                    start2 = 180;
                    outer.Y -= outer.Height;
                    inner.Y -= inner.Height;
                    break;
            }

            if (outer.Width <= 0f) outer.Width = 1f;
            if (outer.Height <= 0f) outer.Height = 1f;
            if (inner.Width <= 0f) inner.Width = 1f;
            if (inner.Height <= 0f) inner.Height = 1f;


            outer.Width *= 2; outer.Height *= 2;
            inner.Width *= 2; inner.Height *= 2;

            outer = RoundR(outer, b); inner = RoundR(inner, b);

            corner.AddArc(outer, start1, 90);
            corner.AddArc(inner, start2, -90);

            corner.CloseFigure();

            return corner;
        }

        /// <summary>
        /// Creates a rounded rectangle using the specified corner radius
        /// </summary>
        /// <param name="rect">Rectangle to round</param>
        /// <param name="nwRadius">Radius of the north east corner</param>
        /// <param name="neRadius">Radius of the north west corner</param>
        /// <param name="seRadius">Radius of the south east corner</param>
        /// <param name="swRadius">Radius of the south west corner</param>
        /// <returns>GraphicsPath with the lines of the rounded rectangle ready to be painted</returns>
        public static GraphicsPath GetRoundRect(RectangleF rect, float nwRadius, float neRadius, float seRadius, float swRadius)
        {
            ///  NW-----NE
            ///  |       |
            ///  |       |
            ///  SW-----SE

            GraphicsPath path = new GraphicsPath();

            nwRadius *= 2;
            neRadius *= 2;
            seRadius *= 2;
            swRadius *= 2;

            //NW ---- NE
            path.AddLine(rect.X + nwRadius, rect.Y, rect.Right - neRadius, rect.Y);
            
            //NE Arc
            if (neRadius > 0f)
            {
                path.AddArc(
                    RectangleF.FromLTRB(rect.Right - neRadius, rect.Top, rect.Right, rect.Top + neRadius),
                    -90, 90);
            }

            // NE
            //  |
            // SE
            path.AddLine(rect.Right, rect.Top + neRadius, rect.Right, rect.Bottom - seRadius);

            //SE Arc
            if (seRadius > 0f)
            {
                path.AddArc(
                    RectangleF.FromLTRB(rect.Right - seRadius, rect.Bottom - seRadius, rect.Right, rect.Bottom),
                    0, 90);
            }

            // SW --- SE
            path.AddLine(rect.Right - seRadius, rect.Bottom, rect.Left + swRadius, rect.Bottom);

            //SW Arc
            if (swRadius > 0f)
            {
                path.AddArc(
                    RectangleF.FromLTRB(rect.Left, rect.Bottom - swRadius, rect.Left + swRadius, rect.Bottom),
                    90, 90);
            }

            // NW
            // |
            // SW
            path.AddLine(rect.Left, rect.Bottom - swRadius, rect.Left, rect.Top + nwRadius);

            //NW Arc
            if (nwRadius > 0f)
            {
                path.AddArc(
                    RectangleF.FromLTRB(rect.Left, rect.Top, rect.Left + nwRadius, rect.Top + nwRadius),
                    180, 90);
            }

            path.CloseFigure();

            return path;
        }

        /// <summary>
        /// Makes the specified color darker
        /// </summary>
        /// <param name="c"></param>
        /// <returns></returns>
        public static Color Darken(Color c)
        {
            return Color.FromArgb(c.R / 2, c.G / 2, c.B / 2);
        }
    }
}
