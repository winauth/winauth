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
using System.Drawing;
using System.Collections.Generic;
using System.Text;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Interface defining a control as being a transparent container
	/// </summary>
	public interface ITransparentConatiner
	{
		/// <summary>
		/// Get the current background image
		/// </summary>
		Image BackgroundImage { get; }
	}

	/// <summary>
	/// Base class for all transparent controls
	/// </summary>
	public class TransparentControl : Control
	{
		/// <summary>
		/// Flag if this control has a background
		/// </summary>
		protected bool Background;

		/// <summary>
		/// Overridden backgroun paint to paint the background image 
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaintBackground(PaintEventArgs e)
		{
			ITransparentConatiner parent = this.Parent as ITransparentConatiner;
			if (parent == null)
			{
				// not a transparent container
				base.OnPaintBackground(e);
				return;
			}

			// set this as being background
			Background = true;

			// draw the image
			e.Graphics.DrawImage(parent.BackgroundImage, 0, 0, this.Bounds, GraphicsUnit.Pixel);
		}

		/// <summary>
		/// If the text has changed invalidate the control
		/// </summary>
		/// <param name="e"></param>
		protected override void OnTextChanged(EventArgs e)
		{
			base.OnTextChanged(e);
			Invalidate();
		}

		/// <summary>
		/// Invalidate control if control is moved to another parent
		/// </summary>
		/// <param name="e"></param>
		protected override void OnParentChanged(EventArgs e)
		{
			base.OnParentChanged(e);
			Invalidate();
		}
	}

	/// <summary>
	/// A transparent label control
	/// </summary>
	public class TransparentLabel : TransparentControl
	{
		/// <summary>
		/// Text alignment for label
		/// </summary>
		ContentAlignment m_alignment = ContentAlignment.TopLeft;

		/// <summary>
		/// Current formatting
		/// </summary>
		StringFormat m_format = new StringFormat();

		/// <summary>
		/// Background image
		/// </summary>
		Bitmap m_backImage = null;

		/// <summary>
		/// Create a new transparent label
		/// </summary>
		public TransparentLabel()
		{
		}

		/// <summary>
		/// Get/set the text alignment
		/// </summary>
		public ContentAlignment TextAlign
		{
			get
			{
				return m_alignment;
			}
			set
			{
				m_alignment = value;

				// set the format
				switch (value)
				{
					case ContentAlignment.TopLeft:
						m_format.Alignment = StringAlignment.Near;
						m_format.LineAlignment = StringAlignment.Near;
						break;

					case ContentAlignment.TopCenter:
						m_format.Alignment = StringAlignment.Center;
						m_format.LineAlignment = StringAlignment.Center;
						break;

					case ContentAlignment.TopRight:
						m_format.Alignment = StringAlignment.Far;
						m_format.LineAlignment = StringAlignment.Far;
						break;
				}
			}
		}

		/// <summary>
		/// Draw the label
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			if (base.Background == false)
			{
				// build the image if we don't have one
				if (m_backImage == null)
				{
					m_backImage = new Bitmap(ClientSize.Width, ClientSize.Height);
				}
				// clear the background image
				using (Graphics g = Graphics.FromImage(m_backImage))
				{
					using (SolidBrush brush = new SolidBrush(Parent.BackColor))
					{
						g.Clear(BackColor);
						g.FillRectangle(brush, ClientRectangle);
					}
				}
			}
			else
			{
				// draw the text into the context
				using (SolidBrush brush = new SolidBrush(ForeColor))
				{
					e.Graphics.DrawString(Text, Font, brush, new Rectangle(0, 0, Width, Height), m_format);
				}
			}
		}
	}

	public class TransparentForm : Form, ITransparentConatiner
	{
		/// <summary>
		/// The background image
		/// </summary>
		private Bitmap m_background;

		/// <summary>
		/// Create a new form
		/// </summary>
		public TransparentForm()
		{
			// get the background image
			m_background = Properties.Resources.background;
		}

		/// <summary>
		/// Paint the form by painting the background image
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			e.Graphics.DrawImage(m_background, 0, 0);
		}

		/// <summary>
		/// Get the background image
		/// </summary>
		public Image BackgroundImage
		{
			get
			{
				return m_background;
			}
		}
	}
}
