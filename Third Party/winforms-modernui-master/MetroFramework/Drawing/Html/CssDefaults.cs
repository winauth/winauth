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
    public class CssDefaults
    {
        /// <summary>
        /// CSS Specification's Default Style Sheet for HTML 4
        /// </summary>
        /// <remarks>
        /// http://www.w3.org/TR/CSS21/sample.html
        /// </remarks>
        public const string DefaultStyleSheet = @"

        
        html, address,
        blockquote,
        body, dd, div,
        dl, dt, fieldset, form,
        frame, frameset,
        h1, h2, h3, h4,
        h5, h6, noframes,
        ol, p, ul, center,
        dir, hr, menu, pre   { display: block }
        li              { display: list-item }
        head            { display: none }
        table           { display: table }
        tr              { display: table-row }
        thead           { display: table-header-group }
        tbody           { display: table-row-group }
        tfoot           { display: table-footer-group }
        col             { display: table-column }
        colgroup        { display: table-column-group }
        td, th          { display: table-cell }
        caption         { display: table-caption }
        th              { font-weight: bolder; text-align: center }
        caption         { text-align: center }
        body            { margin: 8px }
        h1              { font-size: 2em; margin: .67em 0 }
        h2              { font-size: 1.5em; margin: .75em 0 }
        h3              { font-size: 1.17em; margin: .83em 0 }
        h4, p,
        blockquote, ul,
        fieldset, form,
        ol, dl, dir,
        menu            { margin: 1.12em 0 }
        h5              { font-size: .83em; margin: 1.5em 0 }
        h6              { font-size: .75em; margin: 1.67em 0 }
        h1, h2, h3, h4,
        h5, h6, b,
        strong          { font-weight: bolder; }
        blockquote      { margin-left: 40px; margin-right: 40px }
        i, cite, em,
        var, address    { font-style: italic }
        pre, tt, code,
        kbd, samp       { font-family: monospace }
        pre             { white-space: pre }
        button, textarea,
        input, select   { display: inline-block }
        big             { font-size: 1.17em }
        small, sub, sup { font-size: .83em }
        sub             { vertical-align: sub }
        sup             { vertical-align: super }
        table           { border-spacing: 2px; }
        thead, tbody,
        tfoot           { vertical-align: middle }
        td, th          { vertical-align: inherit }
        s, strike, del  { text-decoration: line-through }
        hr              { border: 1px inset }
        ol, ul, dir,
        menu, dd        { margin-left: 40px }
        ol              { list-style-type: decimal }
        ol ul, ul ol,
        ul ul, ol ol    { margin-top: 0; margin-bottom: 0 }
        u, ins          { text-decoration: underline }
        br:before       { content: ""\A"" }
        :before, :after { white-space: pre-line }
        center          { text-align: center }
        :link, :visited { text-decoration: underline }
        :focus          { outline: thin dotted invert }

        /* Begin bidirectionality settings (do not change) */
        BDO[DIR=""ltr""]  { direction: ltr; unicode-bidi: bidi-override }
        BDO[DIR=""rtl""]  { direction: rtl; unicode-bidi: bidi-override }

        *[DIR=""ltr""]    { direction: ltr; unicode-bidi: embed }
        *[DIR=""rtl""]    { direction: rtl; unicode-bidi: embed }

        @media print {
          h1            { page-break-before: always }
          h1, h2, h3,
          h4, h5, h6    { page-break-after: avoid }
          ul, ol, dl    { page-break-before: avoid }
        }

        /* Not in the specification but necessary */
        a               { color:blue; text-decoration:underline }
        table           { border-color:#dfdfdf; border-style:outset; }
        td, th          { border-color:#dfdfdf; border-style:inset; }
        style, title,
        script, link,
        meta, area,
        base, param     { display:none }
        hr              { border-color: #ccc }  
        pre             { font-size:10pt }
        
        /*This is the background of the HtmlToolTip*/
        .htmltooltipbackground {
              border:solid 1px #767676;
              corner-radius:3px;
              background-color:#white;
              background-gradient:#E4E5F0;
        }

        ";

        /// <summary>
        /// Html Fragment used to draw the icon that shows an error on an IMG HTML element
        /// </summary>
        public const string ErrorOnImageIcon = @"
        <style>
          table { 

               border-bottom:1px solid #bbb;
               border-right:1px solid #bbb;
               border-spacing:0;
          }
          td { 
               border:1px solid #555;
               font:bold 9pt Arial;
               padding:3px;
               color:red;
               background-color:#fbfbfb;
           }
        </style>
        <table>
        <tr>
        <td>X</td>
        </tr>
        </table>";

        /// <summary>
        /// Html Fragment used to draw the icon that shows an error on an OBJECT HTML element
        /// </summary>
        public const string ErrorOnObjectIcon = @"
        <style>
          table { 

               border-bottom:1px solid #bbb;
               border-right:1px solid #bbb;
               border-spacing:0;
          }
          td { 
               border:1px solid #555;
               font:bold 7pt Arial;
               padding:3px;
               color:red;
               background-color:#fbfbfb;
           }
        </style>
        <table>
        <tr>
        <td>X</td>
        </tr>
        </table>";

        /// <summary>
        /// Default font size in points. Change this value to modify the default font size.
        /// </summary>
        public static float FontSize = 12f;

        /// <summary>
        /// Default font used for the generic 'serif' family
        /// </summary>
        public static string FontSerif = System.Drawing.FontFamily.GenericSerif.Name;

        /// <summary>
        /// Default font used for the generic 'sans-serif' family
        /// </summary>
        public static string FontSansSerif = System.Drawing.FontFamily.GenericSansSerif.Name;

        /// <summary>
        /// Default font used for the generic 'cursive' family
        /// </summary>
        public static string FontCursive = "Monotype Corsiva";

        /// <summary>
        /// Default font used for the generic 'fantasy' family
        /// </summary>
        public static string FontFantasy = "Comic Sans MS";

        /// <summary>
        /// Default font used for the generic 'monospace' family
        /// </summary>
        public static string FontMonospace = System.Drawing.FontFamily.GenericMonospace.Name;
    }
}
