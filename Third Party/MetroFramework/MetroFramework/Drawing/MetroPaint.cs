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
using System.Windows.Forms;

namespace MetroFramework.Drawing
{
    public class MetroPaintEventArgs : EventArgs
    {
        public Color BackColor { get; private set; }
        public Color ForeColor { get; private set; }
        public Graphics Graphics { get; private set; }

        public MetroPaintEventArgs(Color backColor, Color foreColor, Graphics g)
        {
            BackColor = backColor;
            ForeColor = foreColor;
            Graphics = g;
        }
    }

    public sealed class MetroPaint
    {
        public sealed class BorderColor
        {
            public static Color Form(MetroThemeStyle theme)
            {
                if (theme == MetroThemeStyle.Dark)
                    return Color.FromArgb(68, 68, 68);

                return Color.FromArgb(204, 204, 204);
            }

            public static class Button
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class CheckBox
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ComboBox
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(85, 85, 85);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public static class ProgressBar
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }

            public static class TabControl
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(68, 68, 68);

                    return Color.FromArgb(204, 204, 204);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(155, 155, 155);
                }
            }
        }

        public sealed class BackColor
        {
            public static Color Form(MetroThemeStyle theme)
            {
                if (theme == MetroThemeStyle.Dark)
                    return Color.FromArgb(17, 17, 17);

                return Color.FromArgb(255, 255, 255);
            }

            public sealed class Button
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(34, 34, 34);

                    return Color.FromArgb(238, 238, 238);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(102, 102, 102);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(238, 238, 238);

                    return Color.FromArgb(51, 51, 51);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(80, 80, 80);

                    return Color.FromArgb(204, 204, 204);
                }
            }

            public sealed class TrackBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(153, 153, 153);

                        return Color.FromArgb(102, 102, 102);
                    }

                    public static Color Hover(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(85, 85, 85);

                        return Color.FromArgb(179, 179, 179);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Hover(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Press(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(204, 204, 204);
                    }

                    public static Color Disabled(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(34, 34, 34);

                        return Color.FromArgb(230, 230, 230);
                    }
                }
            }

            public sealed class ScrollBar
            {
                public sealed class Thumb
                {
                    public static Color Normal(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }

                    public static Color Hover(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Press(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(204, 204, 204);

                        return Color.FromArgb(17, 17, 17);
                    }

                    public static Color Disabled(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }

                public sealed class Bar
                {
                    public static Color Normal(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }
                }
            }

            public sealed class ProgressBar
            {
                public sealed class Bar
                {
                    public static Color Normal(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Hover(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Press(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(38, 38, 38);

                        return Color.FromArgb(234, 234, 234);
                    }

                    public static Color Disabled(MetroThemeStyle theme)
                    {
                        if (theme == MetroThemeStyle.Dark)
                            return Color.FromArgb(51, 51, 51);

                        return Color.FromArgb(221, 221, 221);
                    }
                }
            }
        }

        public sealed class ForeColor
        {
            public sealed class Button
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(204, 204, 204);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(17, 17, 17);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(109, 109, 109);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public static Color Title(MetroThemeStyle theme)
            {
                if (theme == MetroThemeStyle.Dark)
                    return Color.FromArgb(255, 255, 255);

                return Color.FromArgb(0, 0, 0);
            }

            public sealed class Tile
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(255, 255, 255);

                    return Color.FromArgb(255, 255, 255);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(209, 209, 209);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Link
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(128, 128, 128);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class Label
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class CheckBox
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ComboBox
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Hover(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(17, 17, 17);
                }

                public static Color Press(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(153, 153, 153);

                    return Color.FromArgb(153, 153, 153);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(93, 93, 93);

                    return Color.FromArgb(136, 136, 136);
                }
            }

            public sealed class ProgressBar
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }

            public sealed class TabControl
            {
                public static Color Normal(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(170, 170, 170);

                    return Color.FromArgb(0, 0, 0);
                }

                public static Color Disabled(MetroThemeStyle theme)
                {
                    if (theme == MetroThemeStyle.Dark)
                        return Color.FromArgb(51, 51, 51);

                    return Color.FromArgb(209, 209, 209);
                }
            }
        }

        #region Helper Methods

        public static Color GetStyleColor(MetroColorStyle style)
        {
            switch (style)
            {
                case MetroColorStyle.Black:
                    return MetroColors.Black;

                case MetroColorStyle.White:
                    return MetroColors.White;

                case MetroColorStyle.Silver:
                    return MetroColors.Silver;

                case MetroColorStyle.Blue:
                    return MetroColors.Blue;

                case MetroColorStyle.Green:
                    return MetroColors.Green;

                case MetroColorStyle.Lime:
                    return MetroColors.Lime;

                case MetroColorStyle.Teal:
                    return MetroColors.Teal;

                case MetroColorStyle.Orange:
                    return MetroColors.Orange;

                case MetroColorStyle.Brown:
                    return MetroColors.Brown;

                case MetroColorStyle.Pink:
                    return MetroColors.Pink;

                case MetroColorStyle.Magenta:
                    return MetroColors.Magenta;

                case MetroColorStyle.Purple:
                    return MetroColors.Purple;

                case MetroColorStyle.Red:
                    return MetroColors.Red;

                case MetroColorStyle.Yellow:
                    return MetroColors.Yellow;

                default:
                    return MetroColors.Blue;
            }
        }

        public static SolidBrush GetStyleBrush(MetroColorStyle style)
        {
            switch (style)
            {
                case MetroColorStyle.Black:
                    return MetroBrushes.Black;

                case MetroColorStyle.White:
                    return MetroBrushes.White;

                case MetroColorStyle.Silver:
                    return MetroBrushes.Silver;

                case MetroColorStyle.Blue:
                    return MetroBrushes.Blue;

                case MetroColorStyle.Green:
                    return MetroBrushes.Green;

                case MetroColorStyle.Lime:
                    return MetroBrushes.Lime;

                case MetroColorStyle.Teal:
                    return MetroBrushes.Teal;

                case MetroColorStyle.Orange:
                    return MetroBrushes.Orange;

                case MetroColorStyle.Brown:
                    return MetroBrushes.Brown;

                case MetroColorStyle.Pink:
                    return MetroBrushes.Pink;

                case MetroColorStyle.Magenta:
                    return MetroBrushes.Magenta;

                case MetroColorStyle.Purple:
                    return MetroBrushes.Purple;

                case MetroColorStyle.Red:
                    return MetroBrushes.Red;

                case MetroColorStyle.Yellow:
                    return MetroBrushes.Yellow;

                default:
                    return MetroBrushes.Blue;
            }
        }

        public static Pen GetStylePen(MetroColorStyle style)
        {
            switch (style)
            {
                case MetroColorStyle.Black:
                    return MetroPens.Black;

                case MetroColorStyle.White:
                    return MetroPens.White;
                
                case MetroColorStyle.Silver:
                    return MetroPens.Silver;

                case MetroColorStyle.Blue:
                    return MetroPens.Blue;

                case MetroColorStyle.Green:
                    return MetroPens.Green;

                case MetroColorStyle.Lime:
                    return MetroPens.Lime;

                case MetroColorStyle.Teal:
                    return MetroPens.Teal;

                case MetroColorStyle.Orange:
                    return MetroPens.Orange;

                case MetroColorStyle.Brown:
                    return MetroPens.Brown;

                case MetroColorStyle.Pink:
                    return MetroPens.Pink;

                case MetroColorStyle.Magenta:
                    return MetroPens.Magenta;

                case MetroColorStyle.Purple:
                    return MetroPens.Purple;

                case MetroColorStyle.Red:
                    return MetroPens.Red;

                case MetroColorStyle.Yellow:
                    return MetroPens.Yellow;

                default:
                    return MetroPens.Blue;
            }
        }

        public static StringFormat GetStringFormat(ContentAlignment textAlign)
        {
            StringFormat stringFormat = new StringFormat();
            stringFormat.Trimming = StringTrimming.EllipsisCharacter;

            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    stringFormat.Alignment = StringAlignment.Near;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.TopRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;

                case ContentAlignment.MiddleLeft:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.MiddleCenter:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.MiddleRight:
                    stringFormat.Alignment = StringAlignment.Center;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;

                case ContentAlignment.BottomLeft:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Near;
                    break;
                case ContentAlignment.BottomCenter:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Center;
                    break;
                case ContentAlignment.BottomRight:
                    stringFormat.Alignment = StringAlignment.Far;
                    stringFormat.LineAlignment = StringAlignment.Far;
                    break;
            }

            return stringFormat;
        }

        public static TextFormatFlags GetTextFormatFlags(ContentAlignment textAlign)
        {
					TextFormatFlags controlFlags = TextFormatFlags.EndEllipsis;

            switch (textAlign)
            {
                case ContentAlignment.TopLeft:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.Left;
                    break;
                case ContentAlignment.TopCenter:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.TopRight:
                    controlFlags |= TextFormatFlags.Top | TextFormatFlags.Right;
                    break;

                case ContentAlignment.MiddleLeft:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Left;
                    break;
                case ContentAlignment.MiddleCenter:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.MiddleRight:
                    controlFlags |= TextFormatFlags.VerticalCenter | TextFormatFlags.Right;
                    break;

                case ContentAlignment.BottomLeft:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.Left;
                    break;
                case ContentAlignment.BottomCenter:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.HorizontalCenter;
                    break;
                case ContentAlignment.BottomRight:
                    controlFlags |= TextFormatFlags.Bottom | TextFormatFlags.Right;
                    break;
            }

            return controlFlags;
        }

        #endregion
    }
}
