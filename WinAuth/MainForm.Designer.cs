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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(MainForm));
			this.contextMenuStrip = new System.Windows.Forms.ContextMenuStrip(this.components);
			this.registerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.importMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.createBackupMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoLoginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.startWithWindowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.alwaysOnTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.allowCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideSerialMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyOnCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.syncServerTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.exitMeuuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serialLabel = new System.Windows.Forms.Label();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openMenuItemSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.calcCodeButton = new WindowsAuthenticator.RoundButton();
			this.codeField = new WindowsAuthenticator.SecretTextBox();
			this.contextMenuStrip.SuspendLayout();
			this.SuspendLayout();
			// 
			// contextMenuStrip
			// 
			this.contextMenuStrip.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.openMenuItem,
            this.openMenuItemSeparator,
            this.registerMenuItem,
            this.toolStripSeparator4,
            this.loadMenuItem,
            this.saveMenuItem,
            this.saveAsMenuItem,
            this.toolStripSeparator2,
            this.importMenuItem,
            this.exportKeyMenuItem,
            this.toolStripSeparator7,
            this.createBackupMenuItem,
            this.autoLoginMenuItem,
            this.toolStripSeparator6,
            this.startWithWindowsMenuItem,
            this.useTrayIconMenuItem,
            this.alwaysOnTopMenuItem,
            this.toolStripSeparator8,
            this.allowCopyMenuItem,
            this.hideSerialMenuItem,
            this.copyOnCodeMenuItem,
            this.autoRefreshMenuItem,
            this.toolStripSeparator1,
            this.syncServerTimeMenuItem,
            this.toolStripSeparator3,
            this.aboutMenuItem,
            this.toolStripSeparator5,
            this.exitMeuuItem});
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.ShowCheckMargin = true;
			this.contextMenuStrip.ShowImageMargin = false;
			this.contextMenuStrip.Size = new System.Drawing.Size(229, 498);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// registerMenuItem
			// 
			this.registerMenuItem.Name = "registerMenuItem";
			this.registerMenuItem.Size = new System.Drawing.Size(228, 22);
			this.registerMenuItem.Text = "New Authenticator...";
			this.registerMenuItem.Click += new System.EventHandler(this.registerMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(225, 6);
			// 
			// loadMenuItem
			// 
			this.loadMenuItem.Name = "loadMenuItem";
			this.loadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.loadMenuItem.Size = new System.Drawing.Size(228, 22);
			this.loadMenuItem.Text = "Load Authenticator...";
			this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
			// 
			// saveMenuItem
			// 
			this.saveMenuItem.Name = "saveMenuItem";
			this.saveMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S)));
			this.saveMenuItem.Size = new System.Drawing.Size(228, 22);
			this.saveMenuItem.Text = "Save Authenticator";
			this.saveMenuItem.Click += new System.EventHandler(this.saveMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Name = "saveAsMenuItem";
			this.saveAsMenuItem.Size = new System.Drawing.Size(228, 22);
			this.saveAsMenuItem.Text = "Save Authenticator As...";
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(225, 6);
			// 
			// importMenuItem
			// 
			this.importMenuItem.Name = "importMenuItem";
			this.importMenuItem.Size = new System.Drawing.Size(228, 22);
			this.importMenuItem.Text = "Import Key...";
			this.importMenuItem.Click += new System.EventHandler(this.importMenuItem_Click);
			// 
			// exportKeyMenuItem
			// 
			this.exportKeyMenuItem.Name = "exportKeyMenuItem";
			this.exportKeyMenuItem.Size = new System.Drawing.Size(228, 22);
			this.exportKeyMenuItem.Text = "Export Key...";
			this.exportKeyMenuItem.Click += new System.EventHandler(this.exportKeyMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(225, 6);
			// 
			// createBackupMenuItem
			// 
			this.createBackupMenuItem.Name = "createBackupMenuItem";
			this.createBackupMenuItem.Size = new System.Drawing.Size(228, 22);
			this.createBackupMenuItem.Text = "Backup by Email...";
			this.createBackupMenuItem.Click += new System.EventHandler(this.createBackupMenuItem_Click);
			// 
			// autoLoginMenuItem
			// 
			this.autoLoginMenuItem.Name = "autoLoginMenuItem";
			this.autoLoginMenuItem.Size = new System.Drawing.Size(228, 22);
			this.autoLoginMenuItem.Text = "Auto Login...";
			this.autoLoginMenuItem.Click += new System.EventHandler(this.autoLoginMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(225, 6);
			// 
			// startWithWindowsMenuItem
			// 
			this.startWithWindowsMenuItem.Name = "startWithWindowsMenuItem";
			this.startWithWindowsMenuItem.Size = new System.Drawing.Size(228, 22);
			this.startWithWindowsMenuItem.Text = "Start with Windows";
			this.startWithWindowsMenuItem.Click += new System.EventHandler(this.startWithWindowsMenuItem_Click);
			// 
			// useTrayIconMenuItem
			// 
			this.useTrayIconMenuItem.Name = "useTrayIconMenuItem";
			this.useTrayIconMenuItem.Size = new System.Drawing.Size(228, 22);
			this.useTrayIconMenuItem.Text = "Use System Tray Icon";
			this.useTrayIconMenuItem.Click += new System.EventHandler(this.useTrayIconMenuItem_Click);
			// 
			// alwaysOnTopMenuItem
			// 
			this.alwaysOnTopMenuItem.Name = "alwaysOnTopMenuItem";
			this.alwaysOnTopMenuItem.Size = new System.Drawing.Size(228, 22);
			this.alwaysOnTopMenuItem.Text = "Always on Top";
			this.alwaysOnTopMenuItem.Click += new System.EventHandler(this.alwaysOnTopMenuItem_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(225, 6);
			// 
			// allowCopyMenuItem
			// 
			this.allowCopyMenuItem.Name = "allowCopyMenuItem";
			this.allowCopyMenuItem.Size = new System.Drawing.Size(228, 22);
			this.allowCopyMenuItem.Text = "Allow Copy";
			this.allowCopyMenuItem.Click += new System.EventHandler(this.allowCopyMenuItem_Click);
			// 
			// hideSerialMenuItem
			// 
			this.hideSerialMenuItem.Name = "hideSerialMenuItem";
			this.hideSerialMenuItem.Size = new System.Drawing.Size(228, 22);
			this.hideSerialMenuItem.Text = "Hide Serial Number";
			this.hideSerialMenuItem.Click += new System.EventHandler(this.hideSerialMenuItem_Click);
			// 
			// copyOnCodeMenuItem
			// 
			this.copyOnCodeMenuItem.Name = "copyOnCodeMenuItem";
			this.copyOnCodeMenuItem.Size = new System.Drawing.Size(228, 22);
			this.copyOnCodeMenuItem.Text = "Auto Clipboard Copy";
			this.copyOnCodeMenuItem.Click += new System.EventHandler(this.copyOnCodeMenuItem_Click);
			// 
			// autoRefreshMenuItem
			// 
			this.autoRefreshMenuItem.Name = "autoRefreshMenuItem";
			this.autoRefreshMenuItem.Size = new System.Drawing.Size(228, 22);
			this.autoRefreshMenuItem.Text = "Auto Refresh";
			this.autoRefreshMenuItem.Click += new System.EventHandler(this.autoRefreshMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(225, 6);
			// 
			// syncServerTimeMenuItem
			// 
			this.syncServerTimeMenuItem.Name = "syncServerTimeMenuItem";
			this.syncServerTimeMenuItem.Size = new System.Drawing.Size(228, 22);
			this.syncServerTimeMenuItem.Text = "Sync Server Time";
			this.syncServerTimeMenuItem.Click += new System.EventHandler(this.syncServerTimeMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(225, 6);
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Name = "aboutMenuItem";
			this.aboutMenuItem.Size = new System.Drawing.Size(228, 22);
			this.aboutMenuItem.Text = "About...";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(225, 6);
			// 
			// exitMeuuItem
			// 
			this.exitMeuuItem.Name = "exitMeuuItem";
			this.exitMeuuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitMeuuItem.Size = new System.Drawing.Size(228, 22);
			this.exitMeuuItem.Text = "E&xit";
			this.exitMeuuItem.Click += new System.EventHandler(this.exitMeuuItem_Click);
			// 
			// serialLabel
			// 
			this.serialLabel.AutoSize = true;
			this.serialLabel.BackColor = System.Drawing.Color.Transparent;
			this.serialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serialLabel.ForeColor = System.Drawing.SystemColors.Control;
			this.serialLabel.Location = new System.Drawing.Point(86, 79);
			this.serialLabel.Name = "serialLabel";
			this.serialLabel.Size = new System.Drawing.Size(138, 17);
			this.serialLabel.TabIndex = 3;
			this.serialLabel.Text = "US-1234-1234-1234";
			// 
			// refreshTimer
			// 
			this.refreshTimer.Interval = 500;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(101, 21);
			this.progressBar.Maximum = 29;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(104, 10);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 4;
			// 
			// notifyIcon
			// 
			this.notifyIcon.ContextMenuStrip = this.contextMenuStrip;
			this.notifyIcon.Icon = ((System.Drawing.Icon)(resources.GetObject("notifyIcon.Icon")));
			this.notifyIcon.Text = "WinAuth";
			this.notifyIcon.DoubleClick += new System.EventHandler(this.notifyIcon_DoubleClick);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Name = "openMenuItem";
			this.openMenuItem.Size = new System.Drawing.Size(228, 22);
			this.openMenuItem.Text = "Open";
			this.openMenuItem.Visible = false;
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// openMenuItemSeparator
			// 
			this.openMenuItemSeparator.Name = "openMenuItemSeparator";
			this.openMenuItemSeparator.Size = new System.Drawing.Size(225, 6);
			this.openMenuItemSeparator.Visible = false;
			// 
			// calcCodeButton
			// 
			this.calcCodeButton.BackColor = System.Drawing.Color.Black;
			this.calcCodeButton.ColorGradient = ((byte)(2));
			this.calcCodeButton.ColorStepGradient = ((byte)(2));
			this.calcCodeButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.calcCodeButton.FadeOut = false;
			this.calcCodeButton.HoverColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(15)))), ((int)(((byte)(15)))));
			this.calcCodeButton.Location = new System.Drawing.Point(33, 32);
			this.calcCodeButton.Name = "calcCodeButton";
			this.calcCodeButton.Size = new System.Drawing.Size(49, 46);
			this.calcCodeButton.TabIndex = 1;
			this.calcCodeButton.TextStartPoint = new System.Drawing.Point(0, 0);
			this.calcCodeButton.UseVisualStyleBackColor = false;
			this.calcCodeButton.Click += new System.EventHandler(this.calcCodeButton_Click);
			// 
			// codeField
			// 
			this.codeField.BackColor = System.Drawing.SystemColors.Window;
			this.codeField.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.codeField.Cursor = System.Windows.Forms.Cursors.Hand;
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeField.Location = new System.Drawing.Point(103, 45);
			this.codeField.Name = "codeField";
			this.codeField.ReadOnly = true;
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(100, 19);
			this.codeField.TabIndex = 0;
			this.codeField.TabStop = false;
			this.codeField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(259, 115);
			this.ContextMenuStrip = this.contextMenuStrip;
			this.Controls.Add(this.calcCodeButton);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.serialLabel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.MaximizeBox = false;
			this.Name = "MainForm";
			this.Text = "Windows Authenticator";
			this.TopMost = true;
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.MainForm_FormClosing);
			this.Load += new System.EventHandler(this.MainForm_Load);
			this.Shown += new System.EventHandler(this.MainForm_Shown);
			this.contextMenuStrip.ResumeLayout(false);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private SecretTextBox codeField;
		private RoundButton calcCodeButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem autoRefreshMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitMeuuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Label serialLabel;
		private System.Windows.Forms.ToolStripMenuItem copyOnCodeMenuItem;
		private System.Windows.Forms.Timer refreshTimer;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.ToolStripMenuItem syncServerTimeMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem alwaysOnTopMenuItem;
		private System.Windows.Forms.ToolStripMenuItem registerMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideSerialMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem allowCopyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autoLoginMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem createBackupMenuItem;
		private System.Windows.Forms.ToolStripMenuItem importMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem exportKeyMenuItem;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ToolStripMenuItem useTrayIconMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem startWithWindowsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openMenuItem;
		private System.Windows.Forms.ToolStripSeparator openMenuItemSeparator;

	}
}

