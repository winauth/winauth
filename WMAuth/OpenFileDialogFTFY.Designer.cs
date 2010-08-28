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

namespace WindowsAuthenticator
{
	partial class OpenFileDialogFTFY
	{
		/// <summary>
		/// Required designer variable.
		/// </summary>
		private System.ComponentModel.IContainer components = null;
		private System.Windows.Forms.MainMenu mainMenu;

		/// <summary>
		/// Clean up any resources being used.
		/// </summary>
		/// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
		protected override void Dispose(bool disposing)
		{
			if (disposing && (components != null))
			{
				components.Dispose();
			}
			base.Dispose(disposing);
		}

		#region Windows Form Designer generated code

		/// <summary>
		/// Required method for Designer support - do not modify
		/// the contents of this method with the code editor.
		/// </summary>
		private void InitializeComponent()
		{
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.UpMenuItem = new System.Windows.Forms.MenuItem();
			this.cancelMenuItem = new System.Windows.Forms.MenuItem();
			this.fileListiew = new System.Windows.Forms.ListView();
			this.nameColumn = new System.Windows.Forms.ColumnHeader();
			this.dateColumn = new System.Windows.Forms.ColumnHeader();
			this.sizeColumn = new System.Windows.Forms.ColumnHeader();
			this.dirDropDown = new System.Windows.Forms.ComboBox();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.Add(this.UpMenuItem);
			this.mainMenu.MenuItems.Add(this.cancelMenuItem);
			// 
			// UpMenuItem
			// 
			this.UpMenuItem.Text = "Up";
			this.UpMenuItem.Click += new System.EventHandler(this.UpMenuItem_Click);
			// 
			// cancelMenuItem
			// 
			this.cancelMenuItem.Text = "Cancel";
			this.cancelMenuItem.Click += new System.EventHandler(this.cancelMenuItem_Click);
			// 
			// fileListiew
			// 
			this.fileListiew.Columns.Add(this.nameColumn);
			this.fileListiew.Columns.Add(this.dateColumn);
			this.fileListiew.Columns.Add(this.sizeColumn);
			this.fileListiew.Dock = System.Windows.Forms.DockStyle.Fill;
			this.fileListiew.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
			this.fileListiew.Location = new System.Drawing.Point(0, 24);
			this.fileListiew.Name = "fileListiew";
			this.fileListiew.Size = new System.Drawing.Size(240, 244);
			this.fileListiew.TabIndex = 0;
			this.fileListiew.View = System.Windows.Forms.View.Details;
			this.fileListiew.SelectedIndexChanged += new System.EventHandler(this.fileListiew_SelectedIndexChanged);
			// 
			// nameColumn
			// 
			this.nameColumn.Text = "Name";
			this.nameColumn.Width = 64;
			// 
			// dateColumn
			// 
			this.dateColumn.Text = "Date";
			this.dateColumn.Width = 66;
			// 
			// sizeColumn
			// 
			this.sizeColumn.Text = "Size";
			this.sizeColumn.Width = 60;
			// 
			// dirDropDown
			// 
			this.dirDropDown.Dock = System.Windows.Forms.DockStyle.Top;
			this.dirDropDown.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
			this.dirDropDown.Location = new System.Drawing.Point(0, 0);
			this.dirDropDown.Name = "dirDropDown";
			this.dirDropDown.Size = new System.Drawing.Size(240, 24);
			this.dirDropDown.TabIndex = 1;
			this.dirDropDown.SelectedIndexChanged += new System.EventHandler(this.dirDropDown_SelectedIndexChanged);
			// 
			// OpenFileDialogFTFY
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.fileListiew);
			this.Controls.Add(this.dirDropDown);
			this.Menu = this.mainMenu;
			this.Name = "OpenFileDialogFTFY";
			this.Text = "Open File";
			this.Load += new System.EventHandler(this.OpenFileDialogFTFY_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.ListView fileListiew;
		private System.Windows.Forms.MenuItem UpMenuItem;
		private System.Windows.Forms.MenuItem cancelMenuItem;
		private System.Windows.Forms.ColumnHeader nameColumn;
		private System.Windows.Forms.ColumnHeader dateColumn;
		private System.Windows.Forms.ColumnHeader sizeColumn;
		private System.Windows.Forms.ComboBox dirDropDown;
	}
}