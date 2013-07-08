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
using System.Drawing;
using System.Collections.Generic;

namespace MetroFramework
{
    public sealed class MetroPens
    {
        private static Dictionary<string, Pen> metroPens = new Dictionary<string ,Pen>();
        private static Pen GetSavePen(string key, Color color)
        {
            lock (metroPens)
            {
                if (!metroPens.ContainsKey(key))
                    metroPens.Add(key, new Pen(color, 1f));

                return metroPens[key].Clone() as Pen;
            }
        }

        public static Pen Black
        {
            get
            {
                return GetSavePen("Black", MetroColors.Black);
            }
        }

        public static Pen White
        {
            get
            {
                return GetSavePen("White", MetroColors.White);
            }
        }

        public static Pen Silver
        {
            get
            {
                return GetSavePen("Silver", MetroColors.Silver);
            }
        }

        public static Pen Blue
        {
            get
            {
                return GetSavePen("Blue", MetroColors.Blue);
            }
        }

        public static Pen Green
        {
            get
            {
                return GetSavePen("Green", MetroColors.Green);
            }
        }

        public static Pen Lime
        {
            get
            {
                return GetSavePen("Lime", MetroColors.Lime);
            }
        }

        public static Pen Teal
        {
            get
            {
                return GetSavePen("Teal", MetroColors.Teal);
            }
        }

        public static Pen Orange
        {
            get
            {
                return GetSavePen("Orange", MetroColors.Orange);
            }
        }

        public static Pen Brown
        {
            get
            {
                return GetSavePen("Brown", MetroColors.Brown);
            }
        }

        public static Pen Pink
        {
            get
            {
                return GetSavePen("Pink", MetroColors.Pink);
            }
        }

        public static Pen Magenta
        {
            get
            {
                return GetSavePen("Magenta", MetroColors.Magenta);
            }
        }

        public static Pen Purple
        {
            get
            {
                return GetSavePen("Purple", MetroColors.Purple);
            }
        }

        public static Pen Red
        {
            get
            {
                return GetSavePen("Red", MetroColors.Red);
            }
        }

        public static Pen Yellow
        {
            get
            {
                return GetSavePen("Yellow", MetroColors.Yellow);
            }
        }
    }
}
