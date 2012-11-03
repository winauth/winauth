/*
 * Copyright (C) 2010 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;
using System.Windows;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Owner draw progress bar that uses ForeColor as the bar colour even if EnableVisualStyles is set
	/// </summary>
	public class ColouredProgressBar : ProgressBar
	{
		/// <summary>
		/// Create the progress bar
		/// </summary>
		public ColouredProgressBar()
		{
			this.Style = ProgressBarStyle.Continuous;
			this.SetStyle(ControlStyles.UserPaint, true);
		}

		/// <summary>
		/// Draw the progress bar
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			Rectangle rec = e.ClipRectangle;

			rec.Width = (int)(rec.Width * ((double)Value / Maximum));
			if (ProgressBarRenderer.IsSupported)
			{
				ProgressBarRenderer.DrawHorizontalBar(e.Graphics, e.ClipRectangle);
			}
			rec.Height = rec.Height; // -4;
			using (Brush forebrush = new SolidBrush(this.ForeColor))
			{
				e.Graphics.FillRectangle(forebrush, 0, 0, rec.Width, rec.Height);
			}
		}
	}
}
