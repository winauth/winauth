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
using System.Text.RegularExpressions;

namespace MetroFramework.Drawing.Html
{
    public class HtmlTag
    {
        #region Fields

        private string _tagName;
        private bool _isClosing;
        private Dictionary<string, string> _attributes;

        #endregion

        #region Ctor

        private HtmlTag()
        {
            _attributes = new Dictionary<string, string>();
        }

        public HtmlTag(string tag)
            : this()
        {
            tag = tag.Substring(1, tag.Length - 2);

            int spaceIndex = tag.IndexOf(" ");

            //Extract tag name
            if (spaceIndex < 0)
            {
                _tagName = tag;
            }
            else
            {
                _tagName = tag.Substring(0, spaceIndex);
            }

            //Check if is end tag
            if (_tagName.StartsWith("/"))
            {
                _isClosing = true;
                _tagName = _tagName.Substring(1);
            }

            _tagName = _tagName.ToLower();

            //Extract attributes
            MatchCollection atts = Parser.Match(Parser.HmlTagAttributes, tag);

            foreach (Match att in atts)
            {
                //Extract attribute and value
                string[] chunks = att.Value.Split('=');

                if (chunks.Length == 1)
                {
                    if(!Attributes.ContainsKey(chunks[0]))
                        Attributes.Add(chunks[0].ToLower(), string.Empty);
                }
                else if (chunks.Length == 2)
                {
                    string attname = chunks[0].Trim();
                    string attvalue = chunks[1].Trim();

                    if (attvalue.StartsWith("\"") && attvalue.EndsWith("\"") && attvalue.Length > 2)
                    {
                        attvalue = attvalue.Substring(1, attvalue.Length - 2);
                    }

                    if (!Attributes.ContainsKey(attname))
                        Attributes.Add(attname, attvalue);
                }
            }
        }

        #endregion

        #region Props

        /// <summary>
        /// Gets the dictionary of attributes in the tag
        /// </summary>
        public Dictionary<string, string> Attributes
        {
            get { return _attributes; }
        }


        /// <summary>
        /// Gets the name of this tag
        /// </summary>
        public string TagName
        {
            get { return _tagName; }
        }

        /// <summary>
        /// Gets if the tag is actually a closing tag
        /// </summary>
        public bool IsClosing
        {
            get { return _isClosing; }
        }

        /// <summary>
        /// Gets if the tag is single placed; in other words it doesn't need a closing tag; 
        /// e.g. &lt;br&gt;
        /// </summary>
        public bool IsSingle
        {
            get
            {
                return TagName.StartsWith("!")
                    || (new List<string>(
                            new string[]{
                             "area", "base", "basefont", "br", "col",
                             "frame", "hr", "img", "input", "isindex",
                             "link", "meta", "param"
                            }
                        )).Contains(TagName)
                    ;
            }
        }

        internal void TranslateAttributes(CssBox box)
        {
            string t = TagName.ToUpper();

            foreach (string att in Attributes.Keys)
            {
                string value = Attributes[att];

                switch (att)
                {
                    case HtmlConstants.align:
                        if (value == HtmlConstants.left || value == HtmlConstants.center || value == HtmlConstants.right || value == HtmlConstants.justify)
                            box.TextAlign = value;
                        else
                            box.VerticalAlign = value;
                        break;
                    case HtmlConstants.background:
                            box.BackgroundImage = value;
                        break;
                    case HtmlConstants.bgcolor:
                        box.BackgroundColor = value;
                        break;
                    case HtmlConstants.border:
                        box.BorderWidth = TranslateLength(value);
                        
                        if (t == HtmlConstants.TABLE)
                        {
                            ApplyTableBorder(box, value);
                        }
                        else
                        {
                            box.BorderStyle = CssConstants.Solid;
                        }
                        break;
                    case HtmlConstants.bordercolor:
                        box.BorderColor = value;
                        break;
                    case HtmlConstants.cellspacing:
                        box.BorderSpacing = TranslateLength(value);
                        break;
                    case HtmlConstants.cellpadding:
                        ApplyTablePadding(box, value);
                        break;
                    case HtmlConstants.color:
                        box.Color = value;
                        break;
                    case HtmlConstants.dir:
                        box.Direction = value;
                        break;
                    case HtmlConstants.face:
                        box.FontFamily = value;
                        break;
                    case HtmlConstants.height:
                        box.Height = TranslateLength(value);
                        break;
                    case HtmlConstants.hspace:
                        box.MarginRight = box.MarginLeft = TranslateLength(value);
                        break;
                    case HtmlConstants.nowrap:
                        box.WhiteSpace = CssConstants.Nowrap;
                        break;
                    case HtmlConstants.size:
                        if (t == HtmlConstants.HR)
                            box.Height = TranslateLength(value);
                        break;
                    case HtmlConstants.valign:
                        box.VerticalAlign = value;
                        break;
                    case HtmlConstants.vspace:
                        box.MarginTop = box.MarginBottom = TranslateLength(value);
                        break;
                    case HtmlConstants.width:
                        box.Width = TranslateLength(value);
                        break;

                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Converts an HTML length into a Css length
        /// </summary>
        /// <param name="htmlLength"></param>
        /// <returns></returns>
        private string TranslateLength(string htmlLength)
        {
            CssLength len = new CssLength(htmlLength);

            if (len.HasError)
            {
                return htmlLength + "px";
            }

            return htmlLength;
        }

        /// <summary>
        /// Cascades to the TD's the border spacified in the TABLE tag.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="border"></param>
        private void ApplyTableBorder(CssBox table, string border)
        {
            foreach (CssBox box in table.Boxes)
            {
                foreach (CssBox cell in box.Boxes)
                {
                    cell.BorderWidth = TranslateLength(border);
                }
            }
        }

        /// <summary>
        /// Cascades to the TD's the border spacified in the TABLE tag.
        /// </summary>
        /// <param name="table"></param>
        /// <param name="border"></param>
        private void ApplyTablePadding(CssBox table, string padding)
        {
            foreach (CssBox box in table.Boxes)
            {
                foreach (CssBox cell in box.Boxes)
                {
                    cell.Padding = TranslateLength(padding);

                }
            }
        }

        /// <summary>
        /// Gets a boolean indicating if the attribute list has the specified attribute
        /// </summary>
        /// <param name="attribute"></param>
        /// <returns></returns>
        public bool HasAttribute(string attribute)
        {
            return Attributes.ContainsKey(attribute);
        }

        public override string ToString()
        {
            return string.Format("<{1}{0}>", TagName, IsClosing ? "/" : string.Empty);
        }

        #endregion
    }
}
