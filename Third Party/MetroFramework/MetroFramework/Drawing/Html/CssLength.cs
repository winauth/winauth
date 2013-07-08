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
using System.Globalization;

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Represents and gets info about a CSS Length
    /// </summary>
    /// <remarks>
    /// http://www.w3.org/TR/CSS21/syndata.html#length-units
    /// </remarks>
    public class CssLength
    {
        #region Enum

        /// <summary>
        /// Represents the possible units of the CSS lengths
        /// </summary>
        /// <remarks>
        /// http://www.w3.org/TR/CSS21/syndata.html#length-units
        /// </remarks>
        public enum CssUnit
        {
            None,

            Ems,

            Pixels,

            Ex,

            Inches,

            Centimeters,

            Milimeters,

            Points,

            Picas
        }

        #endregion

        #region Fields
        private float _number;
        private bool _isRelative;
        private CssUnit _unit;
        private string _length;
        private bool _isPercentage;
        private bool _hasError;

        #endregion

        #region Ctor

        /// <summary>
        /// Creates a new CssLength from a length specified on a CSS style sheet or fragment
        /// </summary>
        /// <param name="length">Length as specified in the Style Sheet or style fragment</param>
        public CssLength(string length)
        {
            _length = length;
            _number = 0f;
            _unit = CssUnit.None;
            _isPercentage = false;

            //Return zero if no length specified, zero specified
            if (string.IsNullOrEmpty(length) || length == "0") return;

            //If percentage, use ParseNumber
            if (length.EndsWith("%"))
            {
                _number = CssValue.ParseNumber(length, 1);
                _isPercentage = true;
                return;
            }

            //If no units, has error
            if (length.Length < 3)
            {
                float.TryParse(length, out _number);
                _hasError = true;
                return;
            }

            //Get units of the length
            string u = length.Substring(length.Length - 2, 2);

            //Number of the length
            string number = length.Substring(0, length.Length - 2);

            //TODO: Units behave different in paper and in screen!
            switch (u)
            {
                case CssConstants.Em:
                    _unit = CssUnit.Ems;
                    _isRelative = true;
                    break;
                case CssConstants.Ex:
                    _unit = CssUnit.Ex;
                    _isRelative = true;
                    break;
                case CssConstants.Px:
                    _unit = CssUnit.Pixels;
                    _isRelative = true;
                    break;
                case CssConstants.Mm:
                    _unit = CssUnit.Milimeters;
                    break;
                case CssConstants.Cm:
                    _unit = CssUnit.Centimeters;
                    break;
                case CssConstants.In:
                    _unit = CssUnit.Inches;
                    break;
                case CssConstants.Pt:
                    _unit = CssUnit.Points;
                    break;
                case CssConstants.Pc:
                    _unit = CssUnit.Picas;
                    break;
                default:
                    _hasError = true;
                    return;
            }

            if (!float.TryParse(number,  System.Globalization.NumberStyles.Number, NumberFormatInfo.InvariantInfo, out _number))
            {
                _hasError = true;
            }

        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the number in the length
        /// </summary>
        public float Number
        {
            get { return _number; }
        }

        /// <summary>
        /// Gets if the length has some parsing error
        /// </summary>
        public bool HasError
        {
            get { return _hasError; }
        }


        /// <summary>
        /// Gets if the length represents a precentage (not actually a length)
        /// </summary>
        public bool IsPercentage
        {
            get { return _isPercentage; }
        }
	

        /// <summary>
        /// Gets if the length is specified in relative units
        /// </summary>
        public bool IsRelative
        {
            get { return _isRelative; }
        }

        /// <summary>
        /// Gets the unit of the length
        /// </summary>
        public CssUnit Unit
        {
            get { return _unit; }
        }

        /// <summary>
        /// Gets the length as specified in the string
        /// </summary>
        public string Length
        {
            get { return _length; }
        }


        #endregion

        #region Methods

        /// <summary>
        /// If length is in Ems, returns its value in points
        /// </summary>
        /// <param name="emSize">Em size factor to multiply</param>
        /// <returns>Points size of this em</returns>
        /// <exception cref="InvalidOperationException">If length has an error or isn't in ems</exception>
        public CssLength ConvertEmToPoints(float emSize)
        {
            if (HasError) throw new InvalidOperationException("Invalid length");
            if (Unit != CssUnit.Ems) throw new InvalidOperationException("Length is not in ems");

            return new CssLength(string.Format("{0}pt", Convert.ToSingle(Number * emSize).ToString("0.0", NumberFormatInfo.InvariantInfo)));
        }

        /// <summary>
        /// If length is in Ems, returns its value in pixels
        /// </summary>
        /// <param name="emSize">Pixel size factor to multiply</param>
        /// <returns>Pixels size of this em</returns>
        /// <exception cref="InvalidOperationException">If length has an error or isn't in ems</exception>
        public CssLength ConvertEmToPixels(float pixelFactor)
        {
            if (HasError) throw new InvalidOperationException("Invalid length");
            if (Unit != CssUnit.Ems) throw new InvalidOperationException("Length is not in ems");

            return new CssLength(string.Format("{0}px", Convert.ToSingle(Number * pixelFactor).ToString("0.0", NumberFormatInfo.InvariantInfo)));
        }

        /// <summary>
        /// Returns the length formatted ready for CSS interpreting.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            if (HasError)
            {
                return string.Empty;
            }
            else if (IsPercentage)
            {
                return string.Format(NumberFormatInfo.InvariantInfo, "{0}%", Number);
            }
            else
            {
                string u = string.Empty;

                switch (Unit)
                {
                    case CssUnit.None:
                        break;
                    case CssUnit.Ems:
                        u = "em";
                        break;
                    case CssUnit.Pixels:
                        u = "px";
                        break;
                    case CssUnit.Ex:
                        u = "ex";
                        break;
                    case CssUnit.Inches:
                        u = "in";
                        break;
                    case CssUnit.Centimeters:
                        u = "cm";
                        break;
                    case CssUnit.Milimeters:
                        u = "mm";
                        break;
                    case CssUnit.Points:
                        u = "pt";
                        break;
                    case CssUnit.Picas:
                        u = "pc";
                        break;
                }

                return string.Format(NumberFormatInfo.InvariantInfo, "{0}{1}", Number, u);
            }
        }

        #endregion
    }
}
