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
using System.IO;
using System.Reflection;
using System.Diagnostics;
using System.Globalization;
using System.Drawing;

namespace MetroFramework.Drawing.Html
{
    [CLSCompliant(false)]
    public static class CssValue
    {
        /// <summary>
        /// Evals a number and returns it. If number is a percentage, it will be multiplied by <see cref="hundredPercent"/>
        /// </summary>
        /// <param name="number">Number to be parsed</param>
        /// <param name="factor">Number that represents the 100% if parsed number is a percentage</param>
        /// <returns>Parsed number. Zero if error while parsing.</returns>
        public static float ParseNumber(string number, float hundredPercent)
        {
            if (string.IsNullOrEmpty(number))
            {
                return 0f;
            }

            string toParse = number;
            bool isPercent = number.EndsWith("%");
            float result = 0f;

            if (isPercent) toParse = number.Substring(0, number.Length - 1);

            if (!float.TryParse(toParse, NumberStyles.Number, NumberFormatInfo.InvariantInfo, out result))
            {
                return 0f;
            }

            if (isPercent)
            {
                result = (result / 100f) * hundredPercent;
            }

            return result;
        }

        /// <summary>
        /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <returns></returns>
        public static float ParseLength(string length, float hundredPercent, CssBox box)
        {
            return ParseLength(length, hundredPercent, box, box.GetEmHeight(), false);
        }

        /// <summary>
        /// Parses a length. Lengths are followed by an unit identifier (e.g. 10px, 3.1em)
        /// </summary>
        /// <param name="length">Specified length</param>
        /// <param name="hundredPercent">Equivalent to 100 percent when length is percentage</param>
        /// <param name="box"></param>
        /// <param name="useParentsEm"></param>
        /// <param name="returnPoints">Allows the return float to be in points. If false, result will be pixels</param>
        /// <returns></returns>
        public static float ParseLength(string length, float hundredPercent, CssBox box, float emFactor, bool returnPoints)
        {
            //Return zero if no length specified, zero specified
            if (string.IsNullOrEmpty(length) || length == "0") return 0f;

            //If percentage, use ParseNumber
            if (length.EndsWith("%")) return ParseNumber(length, hundredPercent);

            //If no units, return zero
            if (length.Length < 3) return 0f;

            //Get units of the length
            string unit = length.Substring(length.Length - 2, 2);

            //Factor will depend on the unit
            float factor = 1f;

            //Number of the length
            string number = length.Substring(0, length.Length - 2);

            //TODO: Units behave different in paper and in screen!
            switch (unit)
            {
                case CssConstants.Em:
                    factor = emFactor;
                    break;
                case CssConstants.Px:
                    factor = 1f;
                    break;
                case CssConstants.Mm:
                    factor = 3f; //3 pixels per millimeter
                    break;
                case CssConstants.Cm:
                    factor = 37f; //37 pixels per centimeter
                    break;
                case CssConstants.In:
                    factor = 96f; //96 pixels per inch
                    break;
                case CssConstants.Pt:
                    factor = 96f / 72f; // 1 point = 1/72 of inch

                    if (returnPoints)
                    {
                        return ParseNumber(number, hundredPercent);
                    }

                    break;
                case CssConstants.Pc:
                    factor = 96f / 72f * 12f; // 1 pica = 12 points
                    break;
                default:
                    factor = 0f;
                    break;
            }

            

            return factor * ParseNumber(number, hundredPercent);
        }

        /// <summary>
        /// Parses a color value in CSS style; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)
        /// </summary>
        /// <param name="colorValue">Specified color value; e.g. #ff0000, red, rgb(255,0,0), rgb(100%, 0, 0)</param>
        /// <returns>System.Drawing.Color value</returns>
        public static Color GetActualColor(string colorValue)
        {
            int r = 0;
            int g = 0;
            int b = 0;
            Color onError = Color.Empty;

            if (string.IsNullOrEmpty(colorValue)) return onError;

            colorValue = colorValue.ToLower().Trim();

            if (colorValue.StartsWith("#"))
            {
                #region hexadecimal forms
                string hex = colorValue.Substring(1);

                if (hex.Length == 6)
                {
                    r = int.Parse(hex.Substring(0, 2), System.Globalization.NumberStyles.HexNumber);
                    g = int.Parse(hex.Substring(2, 2), System.Globalization.NumberStyles.HexNumber);
                    b = int.Parse(hex.Substring(4, 2), System.Globalization.NumberStyles.HexNumber);
                }
                else if (hex.Length == 3)
                {
                    r = int.Parse(new String(hex.Substring(0, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
                    g = int.Parse(new String(hex.Substring(1, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
                    b = int.Parse(new String(hex.Substring(2, 1)[0], 2), System.Globalization.NumberStyles.HexNumber);
                }
                else
                {
                    return onError;
                } 
                #endregion
            }
            else if (colorValue.StartsWith("rgb(") && colorValue.EndsWith(")"))
            {
                #region RGB forms

                string rgb = colorValue.Substring(4, colorValue.Length - 5);
                string[] chunks = rgb.Split(',');

                if (chunks.Length == 3)
                {
                    unchecked
                    {
                        r = Convert.ToInt32(ParseNumber(chunks[0].Trim(), 255f));
                        g = Convert.ToInt32(ParseNumber(chunks[1].Trim(), 255f));
                        b = Convert.ToInt32(ParseNumber(chunks[2].Trim(), 255f)); 
                    }
                }
                else
                {
                    return onError;
                }

                #endregion
            }
            else
            {
                #region Color Constants

                string hex = string.Empty;

                switch (colorValue)
                {
                    case CssConstants.Maroon:
                        hex = "#800000"; break;
                    case CssConstants.Red:
                        hex = "#ff0000"; break;
                    case CssConstants.Orange:
                        hex = "#ffA500"; break;
                    case CssConstants.Olive:
                        hex = "#808000"; break;
                    case CssConstants.Purple:
                        hex = "#800080"; break;
                    case CssConstants.Fuchsia:
                        hex = "#ff00ff"; break;
                    case CssConstants.White:
                        hex = "#ffffff"; break;
                    case CssConstants.Lime:
                        hex = "#00ff00"; break;
                    case CssConstants.Green:
                        hex = "#008000"; break;
                    case CssConstants.Navy:
                        hex = "#000080"; break;
                    case CssConstants.Blue:
                        hex = "#0000ff"; break;
                    case CssConstants.Aqua:
                        hex = "#00ffff"; break;
                    case CssConstants.Teal:
                        hex = "#008080"; break;
                    case CssConstants.Black:
                        hex = "#000000"; break;
                    case CssConstants.Silver:
                        hex = "#c0c0c0"; break;
                    case CssConstants.Gray:
                        hex = "#808080"; break;
                    case CssConstants.Yellow:
                        hex = "#FFFF00"; break;
                }

                if (string.IsNullOrEmpty(hex))
                {
                    return onError;
                }
                else
                {
                    Color c = GetActualColor(hex);
                    r = c.R;
                    g = c.G;
                    b = c.B;
                }

                #endregion
            }

            return Color.FromArgb(r, g, b);
        }

        /// <summary>
        /// Parses a border value in CSS style; e.g. 1px, 1, thin, thick, medium
        /// </summary>
        /// <param name="borderValue"></param>
        /// <returns></returns>
        public static float GetActualBorderWidth(string borderValue, CssBox b)
        {
            if (string.IsNullOrEmpty(borderValue))
            {
                return GetActualBorderWidth(CssConstants.Medium, b);
            }

            switch (borderValue)
            {
                case CssConstants.Thin:
                    return 1f;
                case CssConstants.Medium:
                    return 2f;
                case CssConstants.Thick:
                    return 4f;
                default:
                    return Math.Abs(ParseLength(borderValue, 1, b));
            }
        }

         /// <summary>
        /// Split the value by spaces; e.g. Useful in values like 'padding:5 4 3 inherit'
        /// </summary>
        /// <param name="value">Value to be splitted</param>
        /// <returns>Splitted and trimmed values</returns>
        public static string[] SplitValues(string value)
        {
            return SplitValues(value, ' ');
        }

        /// <summary>
        /// Split the value by the specified separator; e.g. Useful in values like 'padding:5 4 3 inherit'
        /// </summary>
        /// <param name="value">Value to be splitted</param>
        /// <returns>Splitted and trimmed values</returns>
        public static string[] SplitValues(string value, char separator)
        {
            //TODO: CRITICAL! Don't split values on parenthesis (like rgb(0, 0, 0)) or quotes ("strings")


            if (string.IsNullOrEmpty(value)) return new string[] { };

            string[] values = value.Split(separator);
            List<string> result = new List<string>();

            for (int i = 0; i < values.Length; i++)
            {
                string val = values[i].Trim();

                if (!string.IsNullOrEmpty(val))
                {
                    result.Add(val);
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Detects the type name in a path. 
        /// E.g. Gets System.Drawing.Graphics from a path like System.Drawing.Graphics.Clear
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static Type GetTypeInfo(string path, ref string moreInfo)
        {
            int lastDot = path.LastIndexOf('.');

            if (lastDot < 0) return null;

            string type = path.Substring(0, lastDot);
            moreInfo = path.Substring(lastDot + 1);
            moreInfo = moreInfo.Replace("(", string.Empty).Replace(")", string.Empty);


            foreach (Assembly a in HtmlRenderer.References)
            {
                Type t = a.GetType(type, false, true);

                if (t != null) return t;
            }

            return null;
        }

        /// <summary>
        /// Returns the object specific to the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns>One of the following possible objects: FileInfo, MethodInfo, PropertyInfo</returns>
        private static object DetectSource(string path)
        {
            if (path.StartsWith("method:", StringComparison.CurrentCultureIgnoreCase))
            {
                string methodName = string.Empty;
                Type t = GetTypeInfo(path.Substring(7), ref methodName); if (t == null) return null;
                MethodInfo method = t.GetMethod(methodName);

                if (!method.IsStatic || method.GetParameters().Length > 0)
                {
                    return null;
                }

                return method;
            }
            else if (path.StartsWith("property:", StringComparison.CurrentCultureIgnoreCase))
            {
                string propName = string.Empty;
                Type t = GetTypeInfo(path.Substring(9), ref propName); if (t == null) return null;
                PropertyInfo prop = t.GetProperty(propName);

                return prop;
            }
            else if (Uri.IsWellFormedUriString(path, UriKind.RelativeOrAbsolute))
            {
                return new Uri(path);
            }
            else
            {
                return new FileInfo(path);
            }
        }

        /// <summary>
        /// Gets the image of the specified path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static Image GetImage(string path)
        {
            object source = DetectSource(path);

            FileInfo finfo = source as FileInfo;
            PropertyInfo prop = source as PropertyInfo;
            MethodInfo method = source as MethodInfo;

            try
            {
                if (finfo != null)
                {
                    if (!finfo.Exists) return null;

                    return Image.FromFile(finfo.FullName);

                }
                else if (prop != null)
                {
                    if (!prop.PropertyType.IsSubclassOf(typeof(Image)) && !prop.PropertyType.Equals(typeof(Image))) return null;
                    
                    return prop.GetValue(null, null) as Image;
                }
                else if (method != null)
                {
                    if (!method.ReturnType.IsSubclassOf(typeof(Image))) return null;

                    return method.Invoke(null, null) as Image;
                }
                else
                {
                    return null;
                }
            }
            catch
            {
                return new Bitmap(50, 50); //TODO: Return error image
            }
        }

        /// <summary>
        /// Gets the content of the stylesheet specified in the path
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public static string GetStyleSheet(string path)
        {
            object source = DetectSource(path);

            FileInfo finfo = source as FileInfo;
            PropertyInfo prop = source as PropertyInfo;
            MethodInfo method = source as MethodInfo;

            try
            {
                if (finfo != null)
                {
                    if (!finfo.Exists) return null;

                    StreamReader sr = new StreamReader(finfo.FullName);
                    string result = sr.ReadToEnd();
                    sr.Dispose();

                    return result;
                }
                else if (prop != null)
                {
                    if (!prop.PropertyType.Equals(typeof(string))) return null;

                    return prop.GetValue(null, null) as string;
                }
                else if (method != null)
                {
                    if (!method.ReturnType.Equals(typeof(string))) return null;

                    return method.Invoke(null, null) as string;
                }
                else
                {
                    return string.Empty;
                }
            }
            catch
            {
                return string.Empty;
            }
        }

        /// <summary>
        /// Executes the desired action when the user clicks a link
        /// </summary>
        /// <param name="href"></param>
        public static void GoLink(string href)
        {
            object source = DetectSource(href);

            FileInfo finfo = source as FileInfo;
            PropertyInfo prop = source as PropertyInfo;
            MethodInfo method = source as MethodInfo;
            Uri uri = source as Uri;

            try
            {
                if (finfo != null || uri != null)
                {
                    ProcessStartInfo nfo = new ProcessStartInfo(href);
                    nfo.UseShellExecute = true;

                    Process.Start(nfo);

                }
                else if (method != null)
                {
                    method.Invoke(null, null);
                }
                else
                {
                    //Nothing to do.
                }
            }
            catch
            {
                throw;
            }
        }
    }
}
