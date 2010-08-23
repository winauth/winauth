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
	partial class MainForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.mainMenu = new System.Windows.Forms.MainMenu();
			this.exitMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem2 = new System.Windows.Forms.MenuItem();
			this.showSerialMenuItem = new System.Windows.Forms.MenuItem();
			this.autoRefreshMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem6 = new System.Windows.Forms.MenuItem();
			this.setupMenuItem = new System.Windows.Forms.MenuItem();
			this.menuItem1 = new System.Windows.Forms.MenuItem();
			this.menuItem4 = new System.Windows.Forms.MenuItem();
			this.menuItem7 = new System.Windows.Forms.MenuItem();
			this.menuItem5 = new System.Windows.Forms.MenuItem();
			this.timeToLiveBar = new System.Windows.Forms.ProgressBar();
			this.mainTimer = new System.Windows.Forms.Timer();
			this.serialField = new TransparentLabel();
			this.iconPictureBox = new System.Windows.Forms.PictureBox();
			this.titleLabel = new TransparentLabel();
			this.subtitleLabel = new TransparentLabel();
			this.codePictureBox = new System.Windows.Forms.PictureBox();
			this.codeField = new WindowsAuthenticator.TransparentLabel();
			this.SuspendLayout();
			// 
			// mainMenu
			// 
			this.mainMenu.MenuItems.Add(this.exitMenuItem);
			this.mainMenu.MenuItems.Add(this.menuItem2);
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Text = "Exit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// menuItem2
			// 
			this.menuItem2.MenuItems.Add(this.showSerialMenuItem);
			this.menuItem2.MenuItems.Add(this.autoRefreshMenuItem);
			this.menuItem2.MenuItems.Add(this.menuItem6);
			this.menuItem2.MenuItems.Add(this.setupMenuItem);
			this.menuItem2.MenuItems.Add(this.menuItem1);
			this.menuItem2.MenuItems.Add(this.menuItem4);
			this.menuItem2.MenuItems.Add(this.menuItem7);
			this.menuItem2.MenuItems.Add(this.menuItem5);
			this.menuItem2.Text = "Menu";
			// 
			// showSerialMenuItem
			// 
			this.showSerialMenuItem.Text = "Show Serial";
			this.showSerialMenuItem.Click += new System.EventHandler(this.showSerialMenuItem_Click);
			// 
			// autoRefreshMenuItem
			// 
			this.autoRefreshMenuItem.Text = "Auto Refresh";
			this.autoRefreshMenuItem.Click += new System.EventHandler(this.autoRefreshMenuItem_Click);
			// 
			// menuItem6
			// 
			this.menuItem6.Text = "-";
			// 
			// setupMenuItem
			// 
			this.setupMenuItem.Text = "Setup...";
			this.setupMenuItem.Click += new System.EventHandler(this.setupMenuItem_Click);
			// 
			// menuItem1
			// 
			this.menuItem1.Text = "-";
			// 
			// menuItem4
			// 
			this.menuItem4.Text = "Sync Time";
			// 
			// menuItem7
			// 
			this.menuItem7.Text = "-";
			// 
			// menuItem5
			// 
			this.menuItem5.Text = "About";
			// 
			// timeToLiveBar
			// 
			this.timeToLiveBar.Enabled = false;
			this.timeToLiveBar.Location = new System.Drawing.Point(40, 196);
			this.timeToLiveBar.Maximum = 300;
			this.timeToLiveBar.Name = "timeToLiveBar";
			this.timeToLiveBar.Size = new System.Drawing.Size(402, 20);
			// 
			// mainTimer
			// 
			this.mainTimer.Interval = 200;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// serialField
			// 
			this.serialField.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.serialField.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
			this.serialField.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.serialField.Location = new System.Drawing.Point(40, 232);
			this.serialField.Name = "serialField";
			this.serialField.Size = new System.Drawing.Size(403, 60);
			this.serialField.Text = "SERIAL";
			this.serialField.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// iconPictureBox
			// 
			this.iconPictureBox.Image = ((System.Drawing.Image)(resources.GetObject("iconPictureBox.Image")));
			this.iconPictureBox.Location = new System.Drawing.Point(40, 20);
			this.iconPictureBox.Name = "iconPictureBox";
			this.iconPictureBox.Size = new System.Drawing.Size(64, 64);
			// 
			// titleLabel
			// 
			this.titleLabel.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.titleLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
			this.titleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.titleLabel.Location = new System.Drawing.Point(108, 20);
			this.titleLabel.Name = "titleLabel";
			this.titleLabel.Size = new System.Drawing.Size(335, 38);
			this.titleLabel.Text = "BATTLE.NET";
			// 
			// subtitleLabel
			// 
			this.subtitleLabel.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.subtitleLabel.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
			this.subtitleLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.subtitleLabel.Location = new System.Drawing.Point(108, 60);
			this.subtitleLabel.Name = "subtitleLabel";
			this.subtitleLabel.Size = new System.Drawing.Size(335, 24);
			this.subtitleLabel.Text = "Mobile Authenticator";
			// 
			// codePictureBox
			// 
			this.codePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("codePictureBox.Image")));
			this.codePictureBox.Location = new System.Drawing.Point(40, 110);
			this.codePictureBox.Name = "codePictureBox";
			this.codePictureBox.Size = new System.Drawing.Size(42, 68);
			this.codePictureBox.Visible = false;
			// 
			// codeField
			// 
			this.codeField.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.codeField.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
			this.codeField.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.codeField.Image = ((System.Drawing.Image)(resources.GetObject("codeField.Image")));
			this.codeField.Location = new System.Drawing.Point(40, 110);
			this.codeField.Name = "codeField";
			this.codeField.Size = new System.Drawing.Size(403, 68);
			this.codeField.TabIndex = 4;
			this.codeField.Text = "12345678";
			this.codeField.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(480, 696);
			this.Controls.Add(this.codePictureBox);
			this.Controls.Add(this.iconPictureBox);
			this.Controls.Add(this.serialField);
			this.Controls.Add(this.subtitleLabel);
			this.Controls.Add(this.titleLabel);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.timeToLiveBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(0, 52);
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.MenuItem exitMenuItem;
		private System.Windows.Forms.MenuItem menuItem2;
		private System.Windows.Forms.MenuItem menuItem5;
		private System.Windows.Forms.MenuItem menuItem6;
		private System.Windows.Forms.MenuItem menuItem4;
		private System.Windows.Forms.MenuItem menuItem7;
		private System.Windows.Forms.MenuItem setupMenuItem;
		private System.Windows.Forms.ProgressBar timeToLiveBar;
		private TransparentLabel codeField;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.MenuItem autoRefreshMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private System.Windows.Forms.MenuItem showSerialMenuItem;
		private TransparentLabel serialField;
		private System.Windows.Forms.PictureBox iconPictureBox;
		private TransparentLabel titleLabel;
		private TransparentLabel subtitleLabel;
		private System.Windows.Forms.PictureBox codePictureBox;
	}
}

