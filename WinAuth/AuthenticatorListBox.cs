/*
 * Copyright (C) 2013 Colin Mackie.
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
using System.Runtime.InteropServices;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// An item within the OwnerDraw list that represents an authenticator
	/// </summary>
	public class AuthenticatorListitem
	{
		/// <summary>
		/// Create a new AuthenticatorListitem
		/// </summary>
		/// <param name="auth">authenticator</param>
		/// <param name="index">index of item</param>
		public AuthenticatorListitem(WinAuthAuthenticator auth, int index)
		{
			Authenticator = auth;
			LastUpdate = DateTime.MinValue;
			Index = index;
			DisplayUntil = DateTime.MinValue;
		}

		/// <summary>
		/// Index of item in list
		/// </summary>
		public int Index { get; set; }

		/// <summary>
		/// Authenticator wrapper
		/// </summary>
		public WinAuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// When the item was last updated
		/// </summary>
		public DateTime LastUpdate { get; set;}

		/// <summary>
		/// When the code should be displayed to if not AutoRefresh
		/// </summary>
		public DateTime DisplayUntil { get; set; }

		/// <summary>
		/// The last code to be displayed
		/// </summary>
		public string LastCode { get; set; }

		/// <summary>
		/// A count for password protected to allow multipel unprotect operations
		/// </summary>
		public int UnprotectCount { get; set; }
	}

	/// <summary>
	/// Delegate for event when item is removed
	/// </summary>
	/// <param name="source"></param>
	/// <param name="args"></param>
	public delegate void AuthenticatorListItemRemovedHandler(object source, AuthenticatorListItemRemovedEventArgs args);

	/// <summary>
	/// Event arguments for removing an item
	/// </summary>
	public class AuthenticatorListItemRemovedEventArgs : EventArgs
	{
		/// <summary>
		/// The item that was removed
		/// </summary>
		public AuthenticatorListitem Item { get; private set; }

		/// <summary>
		/// Default constructor
		/// </summary>
		public AuthenticatorListItemRemovedEventArgs(AuthenticatorListitem item)
			: base()
		{
			Item = item;
		}
	}

	/// <summary>
	/// An owner draw listbox that displays wrapped authenticators
	/// </summary>
  public class AuthenticatorListBox : ListBox
  {
		/// <summary>
		/// Event handler for ItemRemoved
		/// </summary>
		public event AuthenticatorListItemRemovedHandler ItemRemoved;

		/// <summary>
		/// Scrolled event handler
		/// </summary>
		[Category("Action")]
		public event ScrollEventHandler Scrolled = null;

		/// <summary>
		/// Rename textbox
		/// </summary>
		private TextBox _renameTextbox;

		/// <summary>
		/// Current item
		/// </summary>
		private AuthenticatorListitem _currentItem;

		/// <summary>
		/// Default constructor for our authenticator list box
		/// </summary>
		public AuthenticatorListBox()
    {
			// set owner draw stlying
      this.SetStyle(ControlStyles.OptimizedDoubleBuffer | ControlStyles.AllPaintingInWmPaint | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
			this.DrawMode = System.Windows.Forms.DrawMode.OwnerDrawFixed;
			this.ReadOnly = true;

			// hook the scroll event
			this.Scrolled += AuthenticatorListBox_Scrolled;

			// hook the context menu
			this.ContextMenuStrip = new ContextMenuStrip();
			this.ContextMenuStrip.Opening += ContextMenuStrip_Opening;

			// preload the content menu
			loadContextMenuStrip();
    }

		#region Control Events

		/// <summary>
		/// Resize event to resize base and fix rename box location
		/// </summary>
		/// <param name="e"></param>
		protected override void OnResize(EventArgs e)
		{
			base.OnResize(e);
			setRenameTextboxLocation();
		}

		/// <summary>
		/// Fix position of rename box when scrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void AuthenticatorListBox_Scrolled(object sender, ScrollEventArgs e)
		{
			if (e.Type == ScrollEventType.EndScroll || e.Type == ScrollEventType.ThumbPosition)
			{
				setRenameTextboxLocation();
			}
		}

		/// <summary>
		/// Click an item in the context menu
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ContextMenu_Click(object sender, EventArgs e)
		{
			ProcessMenu((ToolStripItem)sender);
		}

		/// <summary>
		/// Click to open the contxet menu and set the state
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void ContextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			SetContextMenuItems();
		}

		/// <summary>
		/// Tick event used to update the pie, codes and relock authenticators
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		public void Tick(object sender, EventArgs e)
		{
			for (int index = 0; index < this.Items.Count; index++)
			{
				// get the item
				AuthenticatorListitem item = this.Items[index] as AuthenticatorListitem;
				WinAuthAuthenticator auth = item.Authenticator;

				int y = (this.ItemHeight * index) - (this.TopIndex * this.ItemHeight);
				if (auth.AutoRefresh == true)
				{
					// for autorefresh we repaint the pie or the code too
					int tillUpdate = (int)((auth.AuthenticatorData.ServerTime % 30000) / 1000L);
					if (item.LastUpdate == DateTime.MinValue || tillUpdate == 0)
					{
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
						item.LastUpdate = DateTime.Now;
					}
					else
					{
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
						item.LastUpdate = DateTime.Now;
					}
				}
				else
				{
					// check if we need to redraw
					if (item.DisplayUntil != DateTime.MinValue)
					{
						// clear the item
						if (item.DisplayUntil <= DateTime.Now)
						{
							item.DisplayUntil = DateTime.MinValue;
							item.LastUpdate = DateTime.MinValue;
							item.LastCode = null;

							if (item.Authenticator.AuthenticatorData.PasswordType == Authenticator.PasswordTypes.Explicit)
							{
								ProtectAuthenticator(item);
							}

							SetCursor(this.PointToClient(Control.MousePosition));
						}
						this.Invalidate(new Rectangle(0, y, this.Width, this.ItemHeight), false);
					}
				}
			}
		}

		/// <summary>
		/// Handle the mouse move event to capture cursor position
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseMove(MouseEventArgs e)
		{
			SetCursor(e.Location);

			base.OnMouseMove(e);
		}

		/// <summary>
		/// Handle mouse down event
		/// </summary>
		/// <param name="e"></param>
		protected override void OnMouseDown(MouseEventArgs e)
		{
			// set the currnet item based on position
			SetCurrentItem(e.Location);
			SetCursor(e.Location);

			base.OnMouseDown(e);
		}

		/// <summary>
		/// Handle the mouse up and check for click in the refresh icon
		/// </summary>
		/// <param name="e"></param>
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
					int y = (this.ItemHeight * index) - (this.ItemHeight * this.TopIndex);
					bool hasvscroll = (this.Height < (this.Items.Count * this.ItemHeight));
					if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
						&& new Rectangle(x + this.Width - 56 - (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0), y + 8, 48, 48).Contains(e.Location))
					{
						if (UnprotectAuthenticator(item) == DialogResult.Cancel)
						{
							return;
						}

						item.LastCode = item.Authenticator.AuthenticatorData.CurrentCode;
						item.LastUpdate = DateTime.Now;
						item.DisplayUntil = DateTime.Now.AddSeconds(10);

						if (item.Authenticator.CopyOnCode == true)
						{
							// copy to clipboard
							item.Authenticator.CopyCodeToClipboard(this.Parent as Form);
						}

						RefreshCurrentItem();
					}
				}
			}
		}

		/// <summary>
		/// Main WndProc handler to capture scroll events
		/// </summary>
		/// <param name="msg"></param>
		protected override void WndProc(ref System.Windows.Forms.Message msg)
		{
			if (msg.Msg == WinAPI.WM_VSCROLL)
			{
				if (Scrolled != null)
				{
					WinAPI.ScrollInfoStruct si = new WinAPI.ScrollInfoStruct();
					si.fMask = WinAPI.SIF_ALL;
					si.cbSize = Marshal.SizeOf(si);
					WinAPI.GetScrollInfo(msg.HWnd, 0, ref si);

					if (msg.WParam.ToInt32() == WinAPI.SB_ENDSCROLL)
					{
						ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.EndScroll, si.nPos);
						Scrolled(this, sargs);
					}
					else if (msg.WParam.ToInt32() == WinAPI.SB_THUMBTRACK)
					{
						ScrollEventArgs sargs = new ScrollEventArgs(ScrollEventType.ThumbTrack, si.nPos);
						Scrolled(this, sargs);
					}
				}
			}

			base.WndProc(ref msg);
		}

		#endregion

		#region Item renaming

		/// <summary>
		/// Set the position of the rename textbox
		/// </summary>
		private void setRenameTextboxLocation()
		{
			if (_renameTextbox != null && _renameTextbox.Visible == true)
			{
				AuthenticatorListitem item = _renameTextbox.Tag as AuthenticatorListitem;
				if (item != null)
				{
					int y = (this.ItemHeight * item.Index) - (this.TopIndex * this.ItemHeight) + 8;
					if (RenameTextbox.Location.Y != y)
					{
						RenameTextbox.Location = new Point(RenameTextbox.Location.X, y);
					}
					Refresh();
				}
			}
		}

		/// <summary>
		/// Get or create the rename textbox
		/// </summary>
		public TextBox RenameTextbox
		{
			get
			{
				if (_renameTextbox == null)
				{
					_renameTextbox = new TextBox();
					_renameTextbox.Name = "renameTextBox";
					_renameTextbox.AllowDrop = true;
					_renameTextbox.CausesValidation = false;
					_renameTextbox.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
					_renameTextbox.Location = new System.Drawing.Point(0, 0);
					_renameTextbox.Multiline = false;
					_renameTextbox.Name = "secretCodeField";
					_renameTextbox.Size = new System.Drawing.Size(250, 22);
					_renameTextbox.TabIndex = 0;
					_renameTextbox.Visible = false;
					_renameTextbox.Leave += RenameTextbox_Leave;
					_renameTextbox.KeyPress += _renameTextbox_KeyPress;

					this.Controls.Add(_renameTextbox);
				}

				return _renameTextbox;
			}
		}

		/// <summary>
		/// Get flag is we are renaming
		/// </summary>
		public bool IsRenaming
		{
			get
			{
				return (RenameTextbox.Visible == true);
			}
		}

		/// <summary>
		/// End the renaming and decide to save
		/// </summary>
		/// <param name="save"></param>
		public void EndRenaming(bool save = true)
		{
			if (save == false)
			{
				RenameTextbox.Tag = null;
			}
			RenameTextbox.Visible = false;
		}

		/// <summary>
		/// Keypress event to cancel or commit renaming
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void _renameTextbox_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 27)
			{
				RenameTextbox.Tag = null;
				RenameTextbox.Visible = false;
				e.Handled = true;
			}
			else if (e.KeyChar == 13 || e.KeyChar == 9)
			{
				RenameTextbox.Visible = false;
				e.Handled = true;
			}
		}

		/// <summary>
		/// Handle the focus leave for rename box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		void RenameTextbox_Leave(object sender, EventArgs e)
		{
			RenameTextbox.Visible = false;
			AuthenticatorListitem item = RenameTextbox.Tag as AuthenticatorListitem;
			if (item != null)
			{
				string newname = RenameTextbox.Text.Trim();
				if (newname.Length != 0)
				{
					item.Authenticator.Name = newname;
					RefreshItem(item.Index);
				}
			}
		}

		#endregion

		#region Public Properties

		/// <summary>
		/// Readonly flag
		/// </summary>
		public bool ReadOnly { get; set; }

		/// <summary>
		/// The current (selected) authenticator
		/// </summary>
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

		#endregion

		#region Private methods

		/// <summary>
		/// Set the current item based on the mouse position
		/// </summary>
		/// <param name="mouseLocation">mouse position</param>
		private void SetCurrentItem(Point mouseLocation)
		{
			int index = this.IndexFromPoint(mouseLocation);
			if (index < 0)
			{
				index = 0;
			}
			else if (index >= this.Items.Count)
			{
				index = this.Items.Count - 1;
			}

			if (index >= this.Items.Count)
			{
				CurrentItem = null;
			}
			else
			{
				CurrentItem = this.Items[index] as AuthenticatorListitem;
			}
		}

		/// <summary>
		/// Set the current cursor based on position
		/// </summary>
		/// <param name="mouseLocation">current mouse position</param>
		private void SetCursor(Point mouseLocation)
		{
			// set cursor if we are over a refresh icon
			var cursor = Cursor.Current;
			int index = this.IndexFromPoint(mouseLocation);
			if (index >= 0 && index < this.Items.Count)
			{
				var item = this.Items[index] as AuthenticatorListitem;
				int x = 0;
				int y = (this.ItemHeight * index) - (this.TopIndex * this.ItemHeight);
				bool hasvscroll = (this.Height < (this.Items.Count * this.ItemHeight));
				if (item.Authenticator.AutoRefresh == false && item.DisplayUntil < DateTime.Now
					&& new Rectangle(x + this.Width - 56 - (hasvscroll ? SystemInformation.VerticalScrollBarWidth : 0), y + 8, 48, 48).Contains(mouseLocation))
				{
					cursor = Cursors.Hand;
				}
			}
			if (Cursor.Current != cursor)
			{
				Cursor.Current = cursor;
			}
		}

		/// <summary>
		/// Unprotected an authenticator (if possible)
		/// </summary>
		/// <param name="item">item to unprotect</param>
		/// <returns></returns>
		private DialogResult UnprotectAuthenticator(AuthenticatorListitem item)
		{
			// keep a count so we can have multiples
			if (item.UnprotectCount > 0)
			{
				item.UnprotectCount++;
				return DialogResult.OK;
			}

			// if there is no protection reutrn None
			WinAuthAuthenticator auth = item.Authenticator;
			if (auth.AuthenticatorData.RequiresPassword == false)
			{
				return DialogResult.None;
			}

			// request the password
			UnprotectPasswordForm getPassForm = new UnprotectPasswordForm();
			getPassForm.Authenticator = auth;
			DialogResult result = getPassForm.ShowDialog(this.Parent as Form);
			if (result == DialogResult.OK)
			{
				item.UnprotectCount++;
			}

			return result;
		}

		/// <summary>
		/// Reprotect the authenticator and each action or when the 30second window has expired
		/// </summary>
		/// <param name="item"></param>
		private void ProtectAuthenticator(AuthenticatorListitem item)
		{
			// if alrady protected just decrement counter
			item.UnprotectCount--;
			if (item.UnprotectCount > 0)
			{
				return;
			}

			// reprotect the authenticator
			WinAuthAuthenticator auth = item.Authenticator;
			auth.AuthenticatorData.Protect();
			item.UnprotectCount = 0;
		}

		/// <summary>
		/// Preload the context menu items
		/// </summary>
		private void loadContextMenuStrip()
		{
			this.ContextMenuStrip.Items.Clear();

			ToolStripLabel label = new ToolStripLabel();
			label.Name = "contextMenuItemName";
			label.ForeColor = SystemColors.HotTrack;
			this.ContextMenuStrip.Items.Add(label);
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			EventHandler onclick = new EventHandler(ContextMenu_Click);
			//
			ToolStripMenuItem menuitem;
			ToolStripMenuItem subitem;
			//
			menuitem = new ToolStripMenuItem(strings.SetPassword + "...");
			menuitem.Name = "setPasswordMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.ShowCode);
			menuitem.Name = "showCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.CopyCode);
			menuitem.Name = "copyCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.ShowSerialAndRestoreCode + "...");
			menuitem.Name = "showRestoreCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.ShowSecretKey + "...");
			menuitem.Name = "showGoogleSecretMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.ShowSerialKeyAndDeviceId + "...");
			menuitem.Name = "showTrionSecretMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.Delete);
			menuitem.Name = "deleteMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.Rename);
			menuitem.Name = "renameMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.AutoRefresh);
			menuitem.Name = "autoRefreshMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.CopyOnNewCode);
			menuitem.Name = "copyOnCodeMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			menuitem = new ToolStripMenuItem(strings.Icon);
			menuitem.Name = "iconMenuItem";
			subitem = new ToolStripMenuItem();
			subitem.Text = strings.IconAuto;
			subitem.Name = "iconMenuItem_default";
			subitem.Tag = string.Empty;
			subitem.Click += ContextMenu_Click;
			menuitem.DropDownItems.Add(subitem);
			menuitem.DropDownItems.Add("-");
			this.ContextMenuStrip.Items.Add(menuitem);
			int iconindex = 1;
			foreach (string icon in WinAuthMain.AUTHENTICATOR_ICONS.Keys)
			{
				string iconfile = WinAuthMain.AUTHENTICATOR_ICONS[icon];
				if (iconfile.Length == 0)
				{
					menuitem.DropDownItems.Add(new ToolStripSeparator());
				}
				else
				{
					subitem = new ToolStripMenuItem();
					subitem.Text = icon;
					subitem.Name = "iconMenuItem_" + iconindex++;
					subitem.Tag = iconfile;
					subitem.Image = new Bitmap(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources." + iconfile));
					subitem.ImageAlign = ContentAlignment.MiddleLeft;
					subitem.ImageScaling = ToolStripItemImageScaling.SizeToFit;
					subitem.Click += ContextMenu_Click;
					menuitem.DropDownItems.Add(subitem);
				}
			}
			menuitem.DropDownItems.Add("-");
			subitem = new ToolStripMenuItem();
			subitem.Text = strings.Other + "...";
			subitem.Name = "iconMenuItem_0";
			subitem.Tag = "OTHER";
			subitem.Click += ContextMenu_Click;
			menuitem.DropDownItems.Add(subitem);
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.ShortcutKey + "...");
			menuitem.Name = "shortcutKeyMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
			//
			this.ContextMenuStrip.Items.Add(new ToolStripSeparator());
			//
			menuitem = new ToolStripMenuItem(strings.SyncTime);
			menuitem.Name = "syncMenuItem";
			menuitem.Click += ContextMenu_Click;
			this.ContextMenuStrip.Items.Add(menuitem);
		}

		/// <summary>
		/// Set the options for the context menu items based on the currnet authenticator and state
		/// </summary>
		private void SetContextMenuItems()
		{
			var menu = this.ContextMenuStrip;
			var item = this.CurrentItem;
			WinAuthAuthenticator auth = null;
			if (item == null || (auth = item.Authenticator) == null)
			{
				return;
			}

			ToolStripLabel labelitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "contextMenuItemName").FirstOrDefault() as ToolStripLabel;
			labelitem.Text = item.Authenticator.Name;

			ToolStripMenuItem menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "setPasswordMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Text = (item.Authenticator.AuthenticatorData.PasswordType == Authenticator.PasswordTypes.Explicit ? strings.ChangeOrRemovePassword + "..." : strings.SetPassword + "...");

			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = !auth.AutoRefresh;
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "copyCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Enabled = !(auth.AutoRefresh == false && item.DisplayUntil < DateTime.Now);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showRestoreCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is BattleNetAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showGoogleSecretMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is GoogleAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "showTrionSecretMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.Visible = (auth.AuthenticatorData is TrionAuthenticator);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "autoRefreshMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.CheckState = (auth.AutoRefresh == true ? CheckState.Checked : CheckState.Unchecked);
			menuitem.Enabled = (auth.AuthenticatorData.RequiresPassword == false && auth.AuthenticatorData.PasswordType != Authenticator.PasswordTypes.Explicit);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "copyOnCodeMenuItem").FirstOrDefault() as ToolStripMenuItem;
			menuitem.CheckState = (auth.CopyOnCode == true ? CheckState.Checked : CheckState.Unchecked);
			//
			menuitem = menu.Items.Cast<ToolStripItem>().Where(i => i.Name == "iconMenuItem").FirstOrDefault() as ToolStripMenuItem;
			ToolStripMenuItem subitem = menuitem.DropDownItems.Cast<ToolStripItem>().Where(i => i.Name == "iconMenuItem_default").FirstOrDefault() as ToolStripMenuItem;
			subitem.CheckState = CheckState.Checked;
			foreach (ToolStripItem iconitem in menuitem.DropDownItems)
			{
				if (iconitem is ToolStripMenuItem)
				{
					ToolStripMenuItem iconmenuitem = (ToolStripMenuItem)iconitem;
					if (string.IsNullOrEmpty((string)iconmenuitem.Tag) && string.IsNullOrEmpty(auth.Skin) == true)
					{
						iconmenuitem.CheckState = CheckState.Checked;
					}
					else if (string.Compare((string)iconmenuitem.Tag, auth.Skin) == 0)
					{
						iconmenuitem.CheckState = CheckState.Checked;
					}
					else
					{
						iconmenuitem.CheckState = CheckState.Unchecked;
					}
				}
			}
		}

		/// <summary>
		/// Process the context menu item
		/// </summary>
		/// <param name="menuitem"></param>
		private void ProcessMenu(ToolStripItem menuitem)
		{
			var item = this.CurrentItem;
			var auth = item.Authenticator;

			// check and perform each menu
			if (menuitem.Name == "setPasswordMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					// show the new password form
					SetPasswordForm form = new SetPasswordForm();
					if (form.ShowDialog(this.Parent as Form) != DialogResult.OK)
					{
						return;
					}

					// set the encrpytion
					string password = form.Password;
					if (string.IsNullOrEmpty(password) == false)
					{
						auth.AuthenticatorData.SetEncryption(Authenticator.PasswordTypes.Explicit, password);
						// can't have autorefresh on
						auth.AutoRefresh = false;

						item.UnprotectCount = 0;
						item.DisplayUntil = DateTime.MinValue;
						item.LastUpdate = DateTime.MinValue;
						item.LastCode = null;
					}
					else
					{
						auth.AuthenticatorData.SetEncryption(Authenticator.PasswordTypes.None);
					}
					// make sure authenticator is saved
					auth.MarkChanged();
					RefreshCurrentItem();
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "showCodeMenuItem")
			{
				// check if the authentcated is still protected
				if (UnprotectAuthenticator(item) == DialogResult.Cancel)
				{
					return;
				}

				// reduce unprotect count if already displayed
				if (item.DisplayUntil != DateTime.MinValue)
				{
					ProtectAuthenticator(item);
				}

				item.LastCode = auth.AuthenticatorData.CurrentCode;
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.Now.AddSeconds(10);
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "syncMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					item.Authenticator.AuthenticatorData.Sync();
					item.LastUpdate = DateTime.MinValue;
					RefreshCurrentItem();
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "copyCodeMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					auth.CopyCodeToClipboard(this.Parent as Form, item.LastCode);
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "autoRefreshMenuItem")
			{
				auth.AutoRefresh = !auth.AutoRefresh;
				item.LastUpdate = DateTime.Now;
				item.DisplayUntil = DateTime.MinValue;
				RefreshCurrentItem();
			}
			else if (menuitem.Name == "shortcutKeyMenuItem")
			{
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					SetShortcutKeyForm form = new SetShortcutKeyForm();
					form.Hotkey = auth.HotKey;
					if (form.ShowDialog(this.Parent as Form) != DialogResult.OK)
					{
						return;
					}
					auth.HotKey = form.Hotkey;
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "copyOnCodeMenuItem")
			{
				auth.CopyOnCode = !auth.CopyOnCode;
			}
			else if (menuitem.Name == "showRestoreCodeMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					// show the serial and restore code for Battle.net authenticator				
					ShowRestoreCodeForm form = new ShowRestoreCodeForm();
					form.CurrentAuthenticator = auth;
					form.ShowDialog(this.Parent as Form);
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "showGoogleSecretMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					// show the secret key for Google authenticator				
					ShowSecretKeyForm form = new ShowSecretKeyForm();
					form.CurrentAuthenticator = auth;
					form.ShowDialog(this.Parent as Form);
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "showTrionSecretMenuItem")
			{
				// check if the authentcated is still protected
				DialogResult wasprotected = UnprotectAuthenticator(item);
				if (wasprotected == DialogResult.Cancel)
				{
					return;
				}
				try
				{
					// show the secret key for Trion authenticator				
					ShowTrionSecretForm form = new ShowTrionSecretForm();
					form.CurrentAuthenticator = auth;
					form.ShowDialog(this.Parent as Form);
				}
				finally
				{
					if (wasprotected == DialogResult.OK)
					{
						ProtectAuthenticator(item);
					}
				}
			}
			else if (menuitem.Name == "deleteMenuItem")
			{
				if (WinAuthForm.ConfirmDialog(this.Parent as Form, strings.DeleteAuthenticatorWarning, MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
				{
					int index = item.Index;
					this.Items.Remove(item);
					ItemRemoved(this, new AuthenticatorListItemRemovedEventArgs(item));
					if (index >= this.Items.Count)
					{
						index = this.Items.Count - 1;
					}
					this.CurrentItem = (this.Items.Count != 0 ? this.Items[index] as AuthenticatorListitem : null);
				}
			}
			else if (menuitem.Name == "renameMenuItem")
			{
				int y = (this.ItemHeight * item.Index) - (this.TopIndex * this.ItemHeight) + 8;
				RenameTextbox.Location = new Point(64, y);
				RenameTextbox.Text = auth.Name;
				RenameTextbox.Tag = item;
				RenameTextbox.Visible = true;
				RenameTextbox.Focus();
			}
			else if (menuitem.Name.StartsWith("iconMenuItem_") == true)
			{
				if (menuitem.Tag is string && string.Compare((string)menuitem.Tag, "OTHER") == 0)
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
						ofd.Title = strings.LoadIconImage + " (png or gif @ 48x48)";
						DialogResult result = ofd.ShowDialog(this);
						if (result != System.Windows.Forms.DialogResult.OK)
						{
							return;
						}
						try
						{
							// get the image and create an icon if not already the right size
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
								string.Format(strings.ErrorLoadingIconFile, ex.Message),
								WinAuthMain.APPLICATION_NAME,
								MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) == DialogResult.Yes)
							{
								continue;
							}
						}
						break;
					} while (true);
				}
				else 
				{
					auth.Skin = (((string)menuitem.Tag).Length != 0 ? (string)menuitem.Tag : null);
					RefreshCurrentItem();
				}
			}
		}

		/// <summary>
		/// Callback used for Image's thumbnail creator
		/// </summary>
		/// <returns></returns>
		public bool ThumbnailCallback()
		{
			return false;
		}

		/// <summary>
		/// Refresh the  current item by invalidating it
		/// </summary>
		private void RefreshCurrentItem()
		{
			var item = this.CurrentItem;
			int y = this.ItemHeight * item.Index;
			Rectangle rect = new Rectangle(0, 0, this.Width, this.Height);
			this.Invalidate(rect, false);
		}

		#endregion

		#region Owner Draw

		/// <summary>
		/// Hook into the default WndProc to make sure we get our redraw messages
		/// </summary>
		/// <param name="m"></param>
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

		/// <summary>
		/// Event called when an item requires drawing
		/// </summary>
		/// <param name="e"></param>
		/// <param name="cliprect"></param>
		protected void OnDrawItem(DrawItemEventArgs e, Rectangle cliprect)
		{
			// no need to draw nothing
			if (this.Items.Count == 0)
			{
				return;
			}

			// draw the requested item
			using (var brush = new SolidBrush(e.ForeColor))
			{
				AuthenticatorListitem item = this.Items[e.Index] as AuthenticatorListitem;
				WinAuthAuthenticator auth = item.Authenticator;

				bool showCode = (auth.AutoRefresh == true || item.DisplayUntil > DateTime.Now);

				e.Graphics.SmoothingMode = System.Drawing.Drawing2D.SmoothingMode.AntiAlias;

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
					//string label = (e.Index + 1) + ". " + auth.Name;
					string label = auth.Name;
					SizeF labelsize = e.Graphics.MeasureString(label.ToString(), font);
					// if too big, adjust
					if (labelsize.Width > 255)
					{
						StringBuilder newlabel = new StringBuilder(label + "...");
						while ((labelsize = e.Graphics.MeasureString(newlabel.ToString(), font)).Width > 255)
						{
							newlabel.Remove(newlabel.Length - 4, 1);
						}
						label = newlabel.ToString();
					}
					rect = new Rectangle(e.Bounds.X + 64, e.Bounds.Y + 8, (int)labelsize.Height, (int)labelsize.Width);
					if (cliprect.IntersectsWith(rect) == true)
					{
						//e.Graphics.DrawString(label, font, brush, new PointF(e.Bounds.X + 64, e.Bounds.Y + 8));
						e.Graphics.DrawString(label, font, brush, new RectangleF(e.Bounds.X + 64, e.Bounds.Y + 8, 250, labelsize.Height));
					}

					string code;
					if (showCode == true)
					{
						try
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
						catch (EncrpytedSecretDataException )
						{
							code = "- - - - - -";
						}
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
								//int y = (this.TopIndex * this.ItemHeight) + e.Bounds.y
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
						else if (auth.AuthenticatorData.RequiresPassword == true)
						{
							e.Graphics.DrawImage(WinAuth.Properties.Resources.RefreshIconWithLock, rect);
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

		/// <summary>
		/// Handle the paint event and work out if item needs redrawing
		/// </summary>
		/// <param name="e"></param>
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

		#endregion
	}
}
