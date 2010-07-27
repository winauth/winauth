// Round Button in C#
// Thh Code Project
// http://www.codeproject.com/KB/buttons/RoundButton_csharp.aspx
//
// Author: Saikat Sen
//
// No original license

using System;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Windows.Forms;
using System.Drawing.Drawing2D;

namespace WindowsAuthenticator
{
	public class RoundButton : Button
	{
		private System.ComponentModel.Container components = null;

		private Pen _pen = null;
		SolidBrush _brushText = null, _brushInside = null;
		private byte _colorgradient = 2;		// fading effect
		private Point _textStartPoint = new Point(0, 0);
		private byte _colorStepGradient = 2;	// in pixels
		private bool _fadeOut = false;
		private bool _bDrawOutline = false;
		private Pen _dashedPen = null;
		private Pen _blackPen = null;

		// These are for drawing when you hover over the button
		private Color _hoverColor = Color.FromKnownColor(KnownColor.ControlDark);
		private Pen _hoverPen = null;
		private SolidBrush _hoverBrushInside = null;

		[
		Category("Button step-in color"),
		Description("This color will show up when you hover over the button")
		]
		public Color HoverColor
		{
			get
			{
				return _hoverColor;
			}
			set
			{
				_hoverColor = value;
				_hoverPen.Color = value;
				_hoverBrushInside.Color = value;
			}
		}

		[
		Category("Text start point"),
		Description("Point where your text would start getting drawn")
		]
		public Point TextStartPoint
		{
			get
			{
				return _textStartPoint;
			}
			set
			{
				_textStartPoint = value;
			}
		}

		[
		Category("Color gradient"),
		Description("Indicates how sharp a color transition you want")
		]
		public byte ColorGradient
		{
			get
			{
				return _colorgradient;
			}
			set
			{
				_colorgradient = value;
			}
		}

		[
		Category("Color step gradient"),
		Description("Indicates how many every pixels you want color change")
		]
		public byte ColorStepGradient
		{
			get
			{
				return _colorStepGradient;
			}
			set
			{
				_colorStepGradient = value;
			}
		}

		[
		Category("Fade"),
		Description("Fade out")
		]
		public bool FadeOut
		{
			get
			{
				return _fadeOut;
			}
			set
			{
				_fadeOut = value;
			}
		}

		public RoundButton()
		{
			// This call is required by the Windows.Forms Form Designer.
			InitializeComponent();

			// TODO: Add any initialization after the InitComponent call
			_pen = new Pen(BackColor);
			_brushInside = new SolidBrush(BackColor);
			_brushText = new SolidBrush(ForeColor);

			_hoverPen = new Pen(_hoverColor);
			_hoverBrushInside = new SolidBrush(_hoverColor);

			_blackPen = new Pen(Color.FromKnownColor(KnownColor.Black), 2);
			_dashedPen = new Pen(Color.FromKnownColor(KnownColor.Black), 1);
			_dashedPen.DashStyle = DashStyle.Dot;
		}

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		protected override void Dispose(bool disposing)
		{
			if (disposing)
			{
				if (components != null)
					components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Component Designer generated code
		/// <summary>
		/// Required method for Designer support - do not modify 
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			// 
			// RoundButton
			// 
			this.ForeColorChanged += new System.EventHandler(this.RoundButton_ForeColorChanged);
			this.Enter += new System.EventHandler(this.RoundButton_Enter);
			this.MouseEnter += new System.EventHandler(this.RoundButton_MouseEnter);
			this.BackColorChanged += new System.EventHandler(this.RoundButton_BackColorChanged);
			this.Leave += new System.EventHandler(this.RoundButton_Leave);
			this.MouseLeave += new System.EventHandler(this.RoundButton_MouseLeave);

		}
		#endregion

		protected override void OnPaint(PaintEventArgs pe)
		{
			Graphics g = pe.Graphics;
			ColorButton(g);

			GraphicsPath path = new GraphicsPath();
			path.AddEllipse(0, 0, ClientSize.Width, ClientSize.Height);
			this.Region = new Region(path);
		}

		void ColorButton(Graphics g)
		{
			ColorButton5(g, _pen, _brushInside);
		}

		// Fills color. No color gradient.
		void ColorButton1(Graphics g)
		{
			g.DrawEllipse(_pen, 0, 0, ClientSize.Width, ClientSize.Height);
			g.FillEllipse(_brushInside, 0, 0, ClientSize.Width, ClientSize.Height);
		}

		// Fills color with color gradient. Color gets darker toward the righthand-bottom corner.
		void ColorButton2(Graphics g)
		{
			int x = 0, y = 0;
			Color origPenColor = _pen.Color;
			Color origBrushColor = _brushInside.Color;

			for (; x <= ClientSize.Width / 2 && y <= ClientSize.Height / 2; x += _colorStepGradient, y += _colorStepGradient)
			{
				g.DrawEllipse(_pen, x, y, ClientSize.Width, ClientSize.Height);
				g.FillEllipse(_brushInside, x, y, ClientSize.Width, ClientSize.Height);

				byte newR = ((byte)(_pen.Color.R - _colorgradient) > 0 ? (byte)(_pen.Color.R - _colorgradient) : _pen.Color.R);
				byte newG = ((byte)(_pen.Color.G - _colorgradient) > 0 ? (byte)(_pen.Color.G - _colorgradient) : _pen.Color.G);
				byte newB = ((byte)(_pen.Color.B - _colorgradient) > 0 ? (byte)(_pen.Color.B - _colorgradient) : _pen.Color.B);

				Color newcolor = Color.FromArgb(newR, newG, newB);
				_pen.Color = newcolor;
				_brushInside.Color = newcolor;
			}

			_pen.Color = origPenColor;
			_brushInside.Color = origBrushColor;

			DrawText(g);
		}

		// Fills color with color gradient. Color gets darker towards the center. Respects the image property set 
		// by the user.
		void ColorButton3(Graphics g)
		{
			int x = 0, y = 0;
			Color origPenColor = _pen.Color;
			Color origBrushColor = _brushInside.Color;
			int width = ClientSize.Width, height = ClientSize.Height;

			for (; x <= width / 2 && y <= height / 2; x += _colorStepGradient, y += _colorStepGradient, width -= 2 * _colorStepGradient, height -= 2 * _colorStepGradient)
			{
				g.DrawEllipse(_pen, x, y, width, height);
				g.FillEllipse(_brushInside, x, y, width, height);

				byte newR = ((byte)(_pen.Color.R - _colorgradient) > 0 ? (byte)(_pen.Color.R - _colorgradient) : _pen.Color.R);
				byte newG = ((byte)(_pen.Color.G - _colorgradient) > 0 ? (byte)(_pen.Color.G - _colorgradient) : _pen.Color.G);
				byte newB = ((byte)(_pen.Color.B - _colorgradient) > 0 ? (byte)(_pen.Color.B - _colorgradient) : _pen.Color.B);

				Color newcolor = Color.FromArgb(newR, newG, newB);
				_pen.Color = newcolor;
				_brushInside.Color = newcolor;
			}

			_pen.Color = origPenColor;
			_brushInside.Color = origBrushColor;

			DrawText(g);
			DrawImage(g);
		}

		// Implements fade out/ in property. Color gets lighter (fade in) or darker (fade out) towards the center.
		void ColorButton4(Graphics g)
		{
			int x = 0, y = 0;
			Color origPenColor = _pen.Color;
			Color origBrushColor = _brushInside.Color;
			int width = ClientSize.Width, height = ClientSize.Height;

			for (; x <= width / 2 && y <= height / 2; x += _colorStepGradient, y += _colorStepGradient, width -= 2 * _colorStepGradient, height -= 2 * _colorStepGradient)
			{
				g.DrawEllipse(_pen, x, y, width, height);
				g.FillEllipse(_brushInside, x, y, width, height);

				byte newR = _pen.Color.R;
				byte newG = _pen.Color.G;
				byte newB = _pen.Color.B;

				if (_fadeOut)
				{
					if (_pen.Color.R + _colorgradient <= 255)
						newR = (byte)(_pen.Color.R + _colorgradient);

					if (_pen.Color.G + _colorgradient <= 255)
						newG = (byte)(_pen.Color.G + _colorgradient);

					if (_pen.Color.B + _colorgradient <= 255)
						newB = (byte)(_pen.Color.B + _colorgradient);
				}
				else
				{
					if (_pen.Color.R - _colorgradient >= 0)
						newR = (byte)(_pen.Color.R - _colorgradient);

					if (_pen.Color.G - _colorgradient >= 0)
						newG = (byte)(_pen.Color.G - _colorgradient);

					if (_pen.Color.B - _colorgradient >= 0)
						newB = (byte)(_pen.Color.B - _colorgradient);
				}

				Color newcolor = Color.FromArgb(newR, newG, newB);
				_pen.Color = newcolor;
				_brushInside.Color = newcolor;
			}

			_pen.Color = origPenColor;
			_brushInside.Color = origBrushColor;

			DrawText(g);
			DrawImage(g);
		}

		// ColorButton4 modified to take in pen and brush arguments. Needed for hover-coloring.
		// Draws a focus rectangle when the button has focus.
		void ColorButton5(Graphics g, Pen pen, SolidBrush brush)
		{
			int x = 0, y = 0;
			Color origPenColor = pen.Color;
			Color origBrushColor = brush.Color;
			int width = ClientSize.Width, height = ClientSize.Height;

			for (; x <= width / 2 && y <= height / 2; x += _colorStepGradient, y += _colorStepGradient, width -= 2 * _colorStepGradient, height -= 2 * _colorStepGradient)
			{
				// Draw the focus ellipse
				if (_bDrawOutline && (x == 0))
				{
					// Draw solid black outline
					g.DrawEllipse(_blackPen, x, y, width, height);
					x++; y++; width -= 2; height -= 2;
					g.FillEllipse(brush, x, y, width, height);

					g.DrawEllipse(_blackPen, x, y, width, height);
					x++; y++; width -= 2; height -= 2;
					g.FillEllipse(brush, x, y, width, height);
				}
				else
					g.DrawEllipse(pen, x, y, width, height);

				if (_bDrawOutline && (x == 6))
				{
					// Draw dashed black (inner ellipse of focus ellipse)
					g.DrawEllipse(_dashedPen, x, y, width, height);
					x += 1; y += 1; width -= 2; height -= 2;
				}

				g.FillEllipse(brush, x, y, width, height);

				byte newR = pen.Color.R;
				byte newG = pen.Color.G;
				byte newB = pen.Color.B;

				if (_fadeOut)
				// outer rim -> darker color
				{
					if (pen.Color.R + _colorgradient <= 255)
						newR = (byte)(pen.Color.R + _colorgradient);

					if (pen.Color.G + _colorgradient <= 255)
						newG = (byte)(pen.Color.G + _colorgradient);

					if (pen.Color.B + _colorgradient <= 255)
						newB = (byte)(pen.Color.B + _colorgradient);
				}
				else
				// outer rim -> lighter color
				{
					if (pen.Color.R - _colorgradient >= 0)
						newR = (byte)(pen.Color.R - _colorgradient);

					if (pen.Color.G - _colorgradient >= 0)
						newG = (byte)(pen.Color.G - _colorgradient);

					if (pen.Color.B - _colorgradient >= 0)
						newB = (byte)(pen.Color.B - _colorgradient);
				}

				Color newcolor = Color.FromArgb(newR, newG, newB);
				pen.Color = newcolor;
				brush.Color = newcolor;
			}

			// restore the original color
			pen.Color = origPenColor;
			brush.Color = origBrushColor;

			DrawText(g);
			DrawImage(g);
		}

		private void DrawText(Graphics g)
		{
			g.DrawString(this.Text, this.Font, _brushText, new PointF(_textStartPoint.X, _textStartPoint.Y));
		}

		private void DrawImage(Graphics g)
		{
			// depends on ImageAlign
			if (Image != null)
			{
				Rectangle rc = new Rectangle(new Point((this.Width - Image.Width) / 2, (this.Height - Image.Height) / 2), new Size(Image.Width, Image.Height));
				g.DrawImage(this.Image, rc);
			}

		}

		private void RoundButton_MouseEnter(object sender, System.EventArgs e)
		{
			_pen.Color = _hoverColor;
			_brushInside.Color = _hoverColor;
			this.Invalidate();
		}

		private void RoundButton_MouseLeave(object sender, System.EventArgs e)
		{
			_pen.Color = BackColor;
			_brushInside.Color = BackColor;
			this.Invalidate();
		}

		private void RoundButton_Enter(object sender, System.EventArgs e)
		{
			_bDrawOutline = true;
		}

		private void RoundButton_Leave(object sender, System.EventArgs e)
		{
			_bDrawOutline = false;
		}

		private void RoundButton_BackColorChanged(object sender, System.EventArgs e)
		{
			_pen.Color = BackColor;
			_brushInside.Color = BackColor;
		}

		private void RoundButton_ForeColorChanged(object sender, System.EventArgs e)
		{
			_brushText.Color = ForeColor;
		}
	}
}
