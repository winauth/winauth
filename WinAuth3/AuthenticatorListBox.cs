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
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Windows.Forms;

namespace WinAuth
{
	public class AuthenticatorListitem
	{
		public AuthenticatorListitem(WinAuthAuthenticator auth, int index)
		{
			Authenticator = auth;
			LastUpdate = DateTime.MinValue;
			Index = index;
			DisplayUntil = DateTime.MinValue;
		}

		public int Index { get; set; }
		public WinAuthAuthenticator Authenticator { get; set; }
		public DateTime LastUpdate { get; set;}
		public DateTime DisplayUntil { get; set; }
		public string LastCode { get; set; }
	}

  public class AuthenticatorListBox : ListBox
  {
		public AuthenticatorListBox()
    {
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ReadOnly = true;

			this.ContextMenuStrip = new ContextMenuStrip();
			this.ContextMenuStrip.Opening += ContextMenuStrip_Opening;
			this.ContextMenuStrip.Closed += ContextMenuStrip_Closed;
    }

		private AuthenticatorListitem _currentItem;

		public AuthenticatorListitem CurrentItem
		{
			get
			{
				return _currentItem;
			}
			set
			{
				_currentItem = value;
			}
		}

		private void SetCurrentItem(Point mouseLocation)
		{
			int index = this.IndexFromPoint(mouseLocation);
			if (index < 0 || index >= this.Items.Count)
			{
				CurrentItem = null;
			}
			else
			{
				CurrentItem = this.Items[index] as AuthenticatorListitem;
			}
		}

		private void SetCursor(Point mouseLocation)
		{
			// set cursor if we are over a refresh icon
			var cursor = Cursor.Current;
			int index = this.IndexFromPoint(mouseLocation);
			if (index >= 0 && index < this.Items.Count)
			{
				var item = this.Items[index] as AuthenticatorListitem;
				int x = 0;
				int y = this.ItemHeight * index;
				if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
					&& new Rectangle(x + this.Width - 56, y + 8, 48, 48).Contains(mouseLocation))
				{
					cursor = Cursors.Hand;
				}
			}
			if (Cursor.Current != cursor)
			{
				Cursor.Current = cursor;
			}
		}

		protected override void OnMouseMove(MouseEventArgs e)
		{
			//if (this.ClientRectangle.Contains(this.PointToClient(Control.MousePosition)))
			//{
			//	return;
			//}

			SetCursor(e.Location);

			base.OnMouseMove(e);
		}

		protected override void OnMouseDown(MouseEventArgs e)
		{
			SetCurrentItem(e.Location);
			SetCursor(e.Location);

			base.OnMouseDown(e);
		}

		protected override void OnMouseUp(MouseEventArgs e)
		{
			base.OnMouseUp(e);

			if ((e.Button & System.Windows.Forms.MouseButtons.Left) != 0)
			{
				// if this was in a refresh icon, we do a refresh
				int index = this.IndexFromPoint(e.Location);
				if (index >= 0 && index < this.Items.Count)
				{
					var item = this.Items[index] as AuthenticatorListitem;
					int x = 0;
					int y = this.ItemHeight * index;
					if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
						&& new Rectangle(x + this.Width - 56, y + 8, 48, 48).Contains(e.Location))
					{
						item.LastUpdate = DateTime.Now;
						item.DisplayUntil = DateTime.Now.AddSeconds(10);
						RefreshCurrentItem();
					}
				}
			}
		}

		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			this.ContextMenuStrip.Items.Clear();
			var item = this.CurrentItem;
			if (item == null)
			{
				return;
			}
			var auth = item.Authenticator;
			if (item != null)
			{
				ToolStripLabel label = new ToolStripLabel(item.Authenticator.Name);
				label.ForeColor = SystemColors.HotTrack;
				this.ContextMenuStrip.Items.Add(label);
				this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
				//
				EventHandler onclick = new EventHandler(ContextMenu_Click);
				//
				ToolStripMenuItem menuitem;
				ToolStripMenuItem subitem;
				if (auth.AutoRefresh == false)
				{
					menuitem = new ToolStripMenuItem("Show Code");
					menuitem.Name = "showCodeMenuItem";
					menuitem.Click += ContextMenu_Click;
					this.ContextMenuStrip.Items.Add(menuitem);
				}
				//
				menuitem = new ToolStripMenuItem("Copy Code");
				menuitem.Name = "copyCodeMenuItem";
				if (auth.AutoRefresh == false && item.DisplayUntil < DateTime.Now)
				{
					menuitem.Enabled = false;
				}
				menuitem.Click += ContextMenu_Click;
				this.ContextMenuStrip.Items.Add(menuitem);
				//
				if (auth.AuthenticatorData is BattleNetAuthenticator)
				{
					this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
					menuitem = new ToolStripMenuItem("Show Serial && Restore Code...");
					menuitem.Name = "showRestoreCodeMenuItem";
					menuitem.Click += ContextMenu_Click;
					this.ContextMenuStrip.Items.Add(menuitem);
				}
				if (auth.AuthenticatorData is GoogleAuthenticator)
				{
					this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
					menuitem = new ToolStripMenuItem("Show Secret Key...");
					menuitem.Name = "showSecretKeyMenuItem";
					menuitem.Click += ContextMenu_Click;
					this.ContextMenuStrip.Items.Add(menuitem);
				}
				//
				this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
				//
				menuitem = new ToolStripMenuItem("Auto Refresh");
				menuitem.CheckState = (auth.AutoRefresh == true ? CheckState.Checked : CheckState.Unchecked);
				menuitem.Name = "autoRefreshMenuItem";
				menuitem.Click += ContextMenu_Click;
				this.ContextMenuStrip.Items.Add(menuitem);
				//
				menuitem = new ToolStripMenuItem("Copy On New Code");
				menuitem.CheckState = (auth.CopyOnCode == true ? CheckState.Checked : CheckState.Unchecked);
				menuitem.Name = "copyOnCodeMenuItem";
				menuitem.Click += ContextMenu_Click;
				this.ContextMenuStrip.Items.Add(menuitem);
				//
				menuitem = new ToolStripMenuItem("Icon");
				menuitem.Name = "iconMenuItem";
				subitem = new ToolStripMenuItem();
				subitem.Text = "Auto";
				subitem.Name = "iconMenuItem_default";
				subitem.Tag = string.Empty;
				if (string.IsNullOrEmpty(auth.Skin) == true)
				{
					subitem.CheckState = CheckState.Checked;
				}
				subitem.Click += ContextMenu_Click;
				menuitem.DropDownItems.Add(subitem);
				menuitem.DropDownItems.Add("-");
				this.ContextMenuStrip.Items.Add(menuitem);
				int iconindex = 1;
				foreach (string icon in WinAuthMain.AUTHENTICATOR_ICONS.Keys)
				{
					string iconfile = WinAuthMain.AUTHENTICATOR_ICONS[icon];
					subitem = new ToolStripMenuItem();
					subitem.Text = icon;
					subitem.Name = "iconMenuItem_" + iconindex++;
					subitem.Tag = iconfile;
					subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources." + iconfile));
					subitem.ImageAlign = ContentAlignment.MiddleLeft;
					subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
					if (string.Compare(auth.Skin, iconfile) == 0)
					{
						subitem.CheckState = CheckState.Checked;
					}
					subitem.Click += ContextMenu_Click;
					menuitem.DropDownItems.Add(subitem);
				}
				menuitem.DropDownItems.Add("-");
				subitem = new ToolStripMenuItem();
				subitem.Text = "Other...";
				subitem.Name = "iconMenuItem_0";
				subitem.Tag = null;
				subitem.Click += ContextMenu_Click;
				menuitem.DropDownItems.Add(subitem);
				this.ContextMenuStrip.Items.Add(menuitem);
				//
				this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
				//
				menuitem = new ToolStripMenuItem("Sync Time");
				menuitem.Name = "syncMenuItem";
				menuitem.Click += ContextMenu_Click;
				this.ContextMenuStrip.Items.Add(menuitem);
			}
			//this.ContextMenuStrip.Items.Add("Add");
		}

		void ContextMenuStrip_Closed(object sender, ToolStripDropDownClosedEventArgs e)
		{
			// display any resources
			var menuitem = this.ContextMenuStrip.Items.Cast<ToolStripItem>().Where(i => i is ToolStripMenuItem && ((ToolStripMenuItem)i).Name == "iconMenuItem").FirstOrDefault() as ToolStripMenuItem;
			if (menuitem != null)
			{
				foreach (ToolStripItem subitem in menuitem.DropDownItems)
				{
					if (subitem.Image != null)
					{
						subitem.Image.Dispose();
						subitem.Image = null;
					}
				}
			}
		}

		void ContextMenu_Click(object sender, EventArgs e)
		{
			ToolStripItem menuitem = (ToolStripItem)sender;
			var item = this.CurrentItem;
			var auth = item.Authenticator;

			if (menuitem.Name == "showCodeMenuItem")
			{
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.Now.AddSeconds(10);
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "syncMenuItem")
			{
				item.Authenticator.AuthenticatorData.Sync();
				item.LastUpdate = DateTime.MinValue;
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "copyCodeMenuItem")
			{
				auth.CopyCodeToClipboard(this.Parent as Form, item.LastCode);
			}
			else if (menuitem.Name == "autoRefreshMenuItem")
			{
				auth.AutoRefresh = !auth.AutoRefresh;
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.MinValue;
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "copyOnCodeMenuItem")
			{
				auth.CopyOnCode = !auth.CopyOnCode;
			}
			else if (menuitem.Name == "showRestoreCodeMenuItem")
			{
				// show the serial and restore code for Battle.net authenticator				
				ShowRestoreCodeForm form = new ShowRestoreCodeForm();
				form.CurrentAuthenticator = auth;
				form.ShowDialog(this.Parent as Form);
			}
			else if (menuitem.Name == "showSecretKeyMenuItem")
			{
				// show the secret key for Google authenticator				
				ShowSecretKeyForm form = new ShowSecretKeyForm();
				form.CurrentAuthenticator = auth;
				form.ShowDialog(this.Parent as Form);
			}
			else if (menuitem.Name.StartsWith("iconMenuItem_") == true)
			{
				if (menuitem.Tag != null)
				{
					auth.Skin = (((string)menuitem.Tag).Length != 0 ? (string)menuitem.Tag : null);
					RefreshCurrentItem();
				}
				else
				{
					do
					{
						// other..choose an image file
						OpenFileDialog ofd = new OpenFileDialog();
						ofd.AddExtension = true;
						ofd.CheckFileExists = true;
						ofd.DefaultExt = "png";
						ofd.InitialDirectory = Directory.GetCurrentDirectory();
						ofd.FileName = string.Empty;
						ofd.Filter = "PNG Image Files (*.png)|*.png|GIF Image Files (*.gif)|*.gif|All Files (*.*)|*.*";
						ofd.RestoreDirectory = true;
						ofd.ShowReadOnly = false;
						ofd.Title = "Load Icon Image (png or gif @ 48x48)";
						DialogResult result = ofd.ShowDialog(this);
						if (result != System.Windows.Forms.DialogResult.OK)
						{
							return;
						}
						try
						{
							using (Bitmap iconimage = (Bitmap)Image.FromFile(ofd.FileName))
							{
								if (iconimage.Width != 48 || iconimage.Height != 48)
								{
									Image.GetThumbnailImageAbort thumbNailCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
									using (Bitmap scaled = iconimage.GetThumbnailImage(48, 48, thumbNailCallback, System.IntPtr.Zero) as Bitmap)
									{
										auth.Icon = scaled;
									}
								}
								else
								{
									auth.Icon = iconimage;
								}
								RefreshCurrentItem();
							}
						}
						catch (Exception ex)
						{
							if (MessageBox.Show(this.Parent as Form,
								"Error loading image file: " + ex.Message + Environment.NewLine + Environment.NewLine + "Do you want to try again?",
								WinAuthMain.APPLICATION_NAME,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
							{
								continue;
							}
						}
						break;
					} while (true);
				}
			}
		}

		public bool ThumbnailCallback()
		{
			return false;
		}

		private Bitmap ScaleBitmap(Bitmap bitmap, int width, int height)
		{
			Bitmap scaled = new Bitmap(width, height, PixelFormat.Format24bppRgb);
			float scalex = (float)width / (float)bitmap.Width;
			float scaley = (float)Height / (float)bitmap.Height;

			using (Graphics g = Graphics.FromImage((Image)scaled))
			{
				g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.High;
				g.ScaleTransform(scalex, scaley);

				Rectangle destrectangle = new Rectangle(0, 0, scaled.Size.Width, scaled.Size.Height);
				Rectangle srcrectangle = new Rectangle(0, 0, bitmap.Size.Width, bitmap.Size.Height);
				g.DrawImage(bitmap, destrectangle, srcrectangle, GraphicsUnit.Pixel);
			}

			return scaled;
		}

		private void RefreshCurrentItem()
		{
			var item = this.CurrentItem;
			int y = this.ItemHeight * item.Index;
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			// Rectangle rect = new Rectangle(this.Width - 56, y + 8, 48, 48);
			this.Invalidate(rect, false);
		}

		public void Tick(object sender, EventArgs e)
		{
			for (int index = 0; index < this.Items.Count; index++)
			{
				AuthenticatorListitem item = this.Items[index] as AuthenticatorListitem;
				WinAuthAuthenticator auth = item.Authenticator;

				int y = this.ItemHeight * index;
				if (auth.AutoRefresh == true)
				{
					int tillUpdate = (int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L);
					if (item.LastUpdate == DateTime.MinValue || tillUpdate == 0)
					{
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
						item.LastUpdate = DateTime.Now;
					}
					else
					{
						Rectangle rect = new Rectangle(this.Width - 56, y + 8, 48, 48);
						this.Invalidate(rect, false);
						item.LastUpdate = DateTime.Now;
					}
				}
				else
				{
					if (item.DisplayUntil != DateTime.MinValue)
					{
						if (item.DisplayUntil <= DateTime.Now)
						{
							item.DisplayUntil = DateTime.MinValue;
							item.LastUpdate = DateTime.MinValue;
							item.LastCode = null;

							SetCursor(this.PointToClient(Control.MousePosition));
						}
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
					}
				}
			}
		}

		public bool ReadOnly { get; set; }

		protected override void DefWndProc(ref Message m)
		{
			if (ReadOnly == false || ((m.Msg <= 0x0200 || m.Msg >= 0x020E)
				&& (m.Msg <= 0x0100 || m.Msg >= 0x0109)
				&& m.Msg != 0x2111
				&& m.Msg != 0x87))
			{
				base.DefWndProc(ref m);
			}
		}

		protected void OnDrawItem(DrawItemEventArgs e, Rectangle cliprect)
		{
			if (this.Items.Count > 0)
			{
				using (var brush = new SolidBrush(e.ForeColor))
				{
					AuthenticatorListitem item = this.Items[e.Index] as AuthenticatorListitem;
					WinAuthAuthenticator auth = item.Authenticator;

					bool showCode = (auth.AutoRefresh == true || item.DisplayUntil > DateTime.Now);

					Rectangle rect = new Rectangle(e.Bounds.X + 4, e.Bounds.Y + 8, 48, 48);
					if (cliprect.IntersectsWith(rect) == true)
					{
						using (var icon = auth.Icon)
						{
							e.Graphics.DrawImage(icon, new PointF(e.Bounds.X + 4, e.Bounds.Y + 8));
						}
					}

					using (var font = new Font(e.Font.FontFamily, 12, FontStyle.Regular))
					{
						string label = (e.Index + 1) + ". " + auth.Name;
						SizeF labelsize = e.Graphics.MeasureString(label, font);
						rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 8, (int)labelsize.Height, (int)labelsize.Width);
						if (cliprect.IntersectsWith(rect) == true)
						{
							e.Graphics.DrawString(label, font, brush, new PointF(e.Bounds.X + 64, e.Bounds.Y + 8));
						}

						string code;
						if (showCode == true)
						{
							// we we aren't autorefresh we just keep the same code up for the 10 seconds so it doesn't change even crossing the 30s boundary
							if (auth.AutoRefresh == false)
							{
								if (item.LastCode == null)
								{
									code = auth.AuthenticatorData.CurrentCode;
								}
								else
								{
									code = item.LastCode;
								}
							}
							else
							{
								code = auth.AuthenticatorData.CurrentCode;
								if (code != item.LastCode && auth.CopyOnCode == true)
								{
									// code has changed - copy to clipboard
									auth.CopyCodeToClipboard(this.Parent as Form);
								}
							}
							item.LastCode = code;
							code = code.Insert(code.Length / 2, " ");
						}
						else
						{
							code = "- - - - - -";
						}
						SizeF codesize = e.Graphics.MeasureString(code, e.Font);
						rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 8 + (int)labelsize.Height + 4, (int)codesize.Height, (int)codesize.Width);
						if (cliprect.IntersectsWith(rect) == true)
						{
							e.Graphics.DrawString(code, e.Font, brush, new PointF(e.Bounds.X + 64, e.Bounds.Y + 8 + labelsize.Height + 4));
						}
					}

					rect = new Rectangle(e.Bounds.X + e.Bounds.Width - 56, e.Bounds.Y + 8, 48, 48);
					if (cliprect.IntersectsWith(rect) == true)
					{
						if (auth.AutoRefresh == true)
						{
							using (var piebrush = new SolidBrush(SystemColors.ActiveCaption))
							{
								using (var piepen = new Pen(SystemColors.ActiveCaption))
								{
									int tillUpdate = ((int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L) + 1) * 12;
									e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, 360);
									e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, tillUpdate);
								}
							}
						}
						else
						{
							if (showCode == true)
							{
								using (var piebrush = new SolidBrush(SystemColors.ActiveCaption))
								{
									using (var piepen = new Pen(SystemColors.ActiveCaption))
									{
										int tillUpdate = (int)((item.DisplayUntil.Subtract(DateTime.Now).TotalSeconds * (double)360) / item.DisplayUntil.Subtract(item.LastUpdate).TotalSeconds);
										e.Graphics.DrawPie(piepen, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, 360);
										e.Graphics.FillPie(piebrush, e.Bounds.X + e.Bounds.Width - 54, e.Bounds.Y + 10, 46, 46, 270, tillUpdate);
									}
								}
							}
							else
							{
								e.Graphics.DrawImage(WinAuth.Properties.Resources.RefreshIcon, rect);
							}
						}
					}

					rect = new Rectangle(e.Bounds.X, e.Bounds.Y + this.ItemHeight - 1, 1, 1);
					if (cliprect.IntersectsWith(rect) == true)
					{
						using (Pen pen = new Pen(SystemColors.Control))
						{
							e.Graphics.DrawLine(pen, e.Bounds.X, e.Bounds.Y + this.ItemHeight - 1, e.Bounds.X + e.Bounds.Width, e.Bounds.Y + this.ItemHeight - 1);
						}
					}
				}
			}
		}

		//protected override void OnDrawItem(DrawItemEventArgs e)
		//{
		//	if (this.Items.Count > 0)
		//	{
		//		e.DrawBackground();
		//		OnDrawItem(e, new Rectangle(e.Bounds.X, e.Bounds.X, e.Bounds.Width, e.Bounds.Height));
		//	}
		//	base.OnDrawItem(e);
		//}

    protected override void OnPaint(PaintEventArgs e)
    {
      using (var brush = new SolidBrush(this.BackColor))
      {
        Region region = new Region(e.ClipRectangle);

        e.Graphics.FillRegion(brush, region);
        if (this.Items.Count > 0)
        {
          for (int i = 0; i < this.Items.Count; ++i)
          {
            Rectangle irect = this.GetItemRectangle(i);
            if (e.ClipRectangle.IntersectsWith(irect))
            {
							if ((this.SelectionMode == SelectionMode.One && this.SelectedIndex == i)
							|| (this.SelectionMode == SelectionMode.MultiSimple && this.SelectedIndices.Contains(i))
							|| (this.SelectionMode == SelectionMode.MultiExtended && this.SelectedIndices.Contains(i)))
							{
								DrawItemEventArgs diea = new DrawItemEventArgs(e.Graphics, this.Font,
										irect, i,
										DrawItemState.Selected, this.ForeColor,
										this.BackColor);
								OnDrawItem(diea, e.ClipRectangle);
								base.OnDrawItem(diea);
							}
							else
							{
								DrawItemEventArgs diea = new DrawItemEventArgs(e.Graphics, this.Font,
										irect, i,
										DrawItemState.Default, this.ForeColor,
										this.BackColor);
								OnDrawItem(diea, e.ClipRectangle);
								base.OnDrawItem(diea);
              }
              region.Complement(irect);
            }
          }
        }
      }

      base.OnPaint(e);
    }
  }
}
