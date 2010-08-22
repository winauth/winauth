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
			this.codeField = new System.Windows.Forms.Label();
			this.mainTimer = new System.Windows.Forms.Timer();
			this.enrollLabel = new WindowsAuthenticator.TransparentLabel();
			this.enrollProgressBar = new System.Windows.Forms.ProgressBar();
			this.enrollIntroLabel = new WindowsAuthenticator.TransparentLabel();
			this.rbRegionUS = new System.Windows.Forms.RadioButton();
			this.enrollRegionPanel = new System.Windows.Forms.Panel();
			this.rbRegionEU = new System.Windows.Forms.RadioButton();
			this.enrollMenu = new System.Windows.Forms.MainMenu();
			this.enrollExitMenuItem = new System.Windows.Forms.MenuItem();
			this.enrollRegisterMenuItem = new System.Windows.Forms.MenuItem();
			this.serialField = new WindowsAuthenticator.TransparentLabel();
			this.serialLabel = new WindowsAuthenticator.TransparentLabel();
			this.enrollRegionPanel.SuspendLayout();
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
			this.timeToLiveBar.Location = new System.Drawing.Point(20, 70);
			this.timeToLiveBar.Maximum = 300;
			this.timeToLiveBar.Name = "timeToLiveBar";
			this.timeToLiveBar.Size = new System.Drawing.Size(199, 14);
			// 
			// codeField
			// 
			this.codeField.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.codeField.Font = new System.Drawing.Font("Tahoma", 18F, System.Drawing.FontStyle.Regular);
			this.codeField.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.codeField.Location = new System.Drawing.Point(20, 30);
			this.codeField.Name = "codeField";
			this.codeField.Size = new System.Drawing.Size(199, 32);
			this.codeField.Text = "12345678";
			this.codeField.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// mainTimer
			// 
			this.mainTimer.Interval = 200;
			this.mainTimer.Tick += new System.EventHandler(this.mainTimer_Tick);
			// 
			// enrollLabel
			// 
			this.enrollLabel.BackColor = System.Drawing.Color.Black;
			this.enrollLabel.ForeColor = System.Drawing.Color.White;
			this.enrollLabel.Location = new System.Drawing.Point(20, 212);
			this.enrollLabel.Name = "enrollLabel";
			this.enrollLabel.Size = new System.Drawing.Size(199, 20);
			this.enrollLabel.TabIndex = 2;
			this.enrollLabel.Text = "statusLabel";
			this.enrollLabel.Visible = false;
			// 
			// enrollProgressBar
			// 
			this.enrollProgressBar.Location = new System.Drawing.Point(20, 235);
			this.enrollProgressBar.Maximum = 20;
			this.enrollProgressBar.Name = "enrollProgressBar";
			this.enrollProgressBar.Size = new System.Drawing.Size(199, 20);
			this.enrollProgressBar.Visible = false;
			// 
			// enrollIntroLabel
			// 
			this.enrollIntroLabel.BackColor = System.Drawing.Color.Black;
			this.enrollIntroLabel.ForeColor = System.Drawing.Color.White;
			this.enrollIntroLabel.Location = new System.Drawing.Point(20, 15);
			this.enrollIntroLabel.Name = "enrollIntroLabel";
			this.enrollIntroLabel.Size = new System.Drawing.Size(199, 161);
			this.enrollIntroLabel.TabIndex = 1;
			this.enrollIntroLabel.Text = resources.GetString("enrollIntroLabel.Text");
			this.enrollIntroLabel.Visible = false;
			// 
			// rbRegionUS
			// 
			this.rbRegionUS.Checked = true;
			this.rbRegionUS.ForeColor = System.Drawing.Color.White;
			this.rbRegionUS.Location = new System.Drawing.Point(25, 3);
			this.rbRegionUS.Name = "rbRegionUS";
			this.rbRegionUS.Size = new System.Drawing.Size(79, 20);
			this.rbRegionUS.TabIndex = 1;
			this.rbRegionUS.Text = "US";
			// 
			// enrollRegionPanel
			// 
			this.enrollRegionPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.enrollRegionPanel.BackColor = System.Drawing.Color.Black;
			this.enrollRegionPanel.Controls.Add(this.rbRegionUS);
			this.enrollRegionPanel.Controls.Add(this.rbRegionEU);
			this.enrollRegionPanel.Location = new System.Drawing.Point(20, 182);
			this.enrollRegionPanel.Name = "enrollRegionPanel";
			this.enrollRegionPanel.Size = new System.Drawing.Size(199, 27);
			this.enrollRegionPanel.Visible = false;
			// 
			// rbRegionEU
			// 
			this.rbRegionEU.ForeColor = System.Drawing.Color.White;
			this.rbRegionEU.Location = new System.Drawing.Point(110, 3);
			this.rbRegionEU.Name = "rbRegionEU";
			this.rbRegionEU.Size = new System.Drawing.Size(79, 20);
			this.rbRegionEU.TabIndex = 1;
			this.rbRegionEU.TabStop = false;
			this.rbRegionEU.Text = "EU";
			// 
			// enrollMenu
			// 
			this.enrollMenu.MenuItems.Add(this.enrollExitMenuItem);
			this.enrollMenu.MenuItems.Add(this.enrollRegisterMenuItem);
			// 
			// enrollExitMenuItem
			// 
			this.enrollExitMenuItem.Text = "Exit";
			this.enrollExitMenuItem.Click += new System.EventHandler(this.enrollExitMenuItem_Click);
			// 
			// enrollRegisterMenuItem
			// 
			this.enrollRegisterMenuItem.Text = "Register";
			this.enrollRegisterMenuItem.Click += new System.EventHandler(this.registerMenuItem_Click);
			// 
			// serialField
			// 
			this.serialField.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.serialField.Font = new System.Drawing.Font("Tahoma", 16F, System.Drawing.FontStyle.Regular);
			this.serialField.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.serialField.Location = new System.Drawing.Point(20, 132);
			this.serialField.Name = "serialField";
			this.serialField.Size = new System.Drawing.Size(199, 32);
			this.serialField.TabIndex = 5;
			this.serialField.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// serialLabel
			// 
			this.serialLabel.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.serialLabel.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
			this.serialLabel.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.serialLabel.Location = new System.Drawing.Point(87, 109);
			this.serialLabel.Name = "serialLabel";
			this.serialLabel.Size = new System.Drawing.Size(60, 23);
			this.serialLabel.TabIndex = 4;
			this.serialLabel.Text = "Serial";
			this.serialLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(240, 268);
			this.Controls.Add(this.enrollProgressBar);
			this.Controls.Add(this.enrollIntroLabel);
			this.Controls.Add(this.enrollLabel);
			this.Controls.Add(this.enrollRegionPanel);
			this.Controls.Add(this.serialLabel);
			this.Controls.Add(this.serialField);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.timeToLiveBar);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Menu = this.mainMenu;
			this.Name = "MainForm";
			this.Text = "MainForm";
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.enrollRegionPanel.ResumeLayout(false);
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
		private System.Windows.Forms.Label codeField;
		private System.Windows.Forms.Timer mainTimer;
		private System.Windows.Forms.MenuItem autoRefreshMenuItem;
		private System.Windows.Forms.MenuItem menuItem1;
		private TransparentLabel enrollLabel;
		private System.Windows.Forms.ProgressBar enrollProgressBar;
		private TransparentLabel enrollIntroLabel;
		private System.Windows.Forms.RadioButton rbRegionUS;
		private System.Windows.Forms.Panel enrollRegionPanel;
		private System.Windows.Forms.RadioButton rbRegionEU;
		private System.Windows.Forms.MainMenu enrollMenu;
		private System.Windows.Forms.MenuItem enrollExitMenuItem;
		private System.Windows.Forms.MenuItem enrollRegisterMenuItem;
		private System.Windows.Forms.MenuItem showSerialMenuItem;
		private TransparentLabel serialField;
		private TransparentLabel serialLabel;
	}
}

