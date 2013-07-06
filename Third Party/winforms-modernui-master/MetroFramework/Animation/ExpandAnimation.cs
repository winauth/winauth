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
using System.Windows.Forms;

namespace MetroFramework.Animation
{
    public sealed class ExpandAnimation : AnimationBase
    {
        public void Start(Control control, Size targetSize, TransitionType transitionType, int duration)
        {
            base.Start(control, transitionType, duration,
                delegate 
                {
                    int width = DoExpandAnimation(control.Width, targetSize.Width);
                    int height = DoExpandAnimation(control.Height, targetSize.Height);

                    control.Size = new Size(width, height);
                }, 
                delegate 
                {
                    return (control.Size.Equals(targetSize));
                });
        }

        private int DoExpandAnimation(int startSize, int targetSize)
        {
            float t = (float)counter - startTime;
            float b = (float)startSize;
            float c = (float)targetSize - startSize;
            float d = (float)targetTime - startTime;

            return MakeTransition(t, b, d, c);
        }
    }
}
