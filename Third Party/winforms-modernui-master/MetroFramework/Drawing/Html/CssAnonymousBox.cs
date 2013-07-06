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

namespace MetroFramework.Drawing.Html
{
    /// <summary>
    /// Represents an anonymous inline box
    /// </summary>
    /// <remarks>
    /// To learn more about anonymous inline boxes visit:
    /// http://www.w3.org/TR/CSS21/visuren.html#anonymous
    /// </remarks>
    [CLSCompliant(false)]
    public class CssAnonymousBox
        : CssBox
    {
        #region Ctor

        public CssAnonymousBox(CssBox parentBox)
            : base(parentBox)
        {

        }

        #endregion
    }

    /// <summary>
    /// Represents an anonymous inline box which contains nothing but blank spaces
    /// </summary>
    [CLSCompliant(false)]
    public class CssAnonymousSpaceBox
        : CssAnonymousBox
    {
        public CssAnonymousSpaceBox(CssBox parentBox)
            : base(parentBox)
        {

        }
    }

}
