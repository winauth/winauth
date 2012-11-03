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
			this.openMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.openMenuItemSeparator = new System.Windows.Forms.ToolStripSeparator();
			this.registerMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator4 = new System.Windows.Forms.ToolStripSeparator();
			this.loadMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.saveAsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
			this.syncServerTimeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
			this.showRestoreCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.restoreMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator9 = new System.Windows.Forms.ToolStripSeparator();
			this.importMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.exportKeyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator7 = new System.Windows.Forms.ToolStripSeparator();
			this.autoLoginMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator6 = new System.Windows.Forms.ToolStripSeparator();
			this.menuItemSkin = new System.Windows.Forms.ToolStripMenuItem();
			this.defaultSkinMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator10 = new System.Windows.Forms.ToolStripSeparator();
			this.startWithWindowsMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.useTrayIconMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.alwaysOnTopMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator8 = new System.Windows.Forms.ToolStripSeparator();
			this.allowCopyMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.hideSerialMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.copyOnCodeMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.autoRefreshMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator3 = new System.Windows.Forms.ToolStripSeparator();
			this.aboutMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.toolStripSeparator5 = new System.Windows.Forms.ToolStripSeparator();
			this.exitMenuItem = new System.Windows.Forms.ToolStripMenuItem();
			this.serialLabel = new System.Windows.Forms.Label();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.progressBar = new WindowsAuthenticator.ColouredProgressBar();
			this.notifyIcon = new System.Windows.Forms.NotifyIcon(this.components);
			this.syncTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.clipboardTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.calcCodeButton = new System.Windows.Forms.Button();
			this.codeTooltip = new System.Windows.Forms.ToolTip(this.components);
			this.copyClipboardButton = new WindowsAuthenticator.TransparentButton();
			this.syncButton = new WindowsAuthenticator.TransparentButton();
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
            this.saveAsMenuItem,
            this.toolStripSeparator2,
            this.syncServerTimeMenuItem,
            this.toolStripSeparator1,
            this.showRestoreCodeMenuItem,
            this.restoreMenuItem,
            this.toolStripSeparator9,
            this.importMenuItem,
            this.exportKeyMenuItem,
            this.toolStripSeparator7,
            this.autoLoginMenuItem,
            this.toolStripSeparator6,
            this.menuItemSkin,
            this.defaultSkinMenuItem,
            this.toolStripSeparator10,
            this.startWithWindowsMenuItem,
            this.useTrayIconMenuItem,
            this.alwaysOnTopMenuItem,
            this.toolStripSeparator8,
            this.allowCopyMenuItem,
            this.hideSerialMenuItem,
            this.copyOnCodeMenuItem,
            this.autoRefreshMenuItem,
            this.toolStripSeparator3,
            this.aboutMenuItem,
            this.toolStripSeparator5,
            this.exitMenuItem});
			this.contextMenuStrip.Name = "contextMenuStrip1";
			this.contextMenuStrip.ShowCheckMargin = true;
			this.contextMenuStrip.ShowImageMargin = false;
			this.contextMenuStrip.Size = new System.Drawing.Size(254, 532);
			this.contextMenuStrip.Opening += new System.ComponentModel.CancelEventHandler(this.contextMenuStrip_Opening);
			// 
			// openMenuItem
			// 
			this.openMenuItem.Name = "openMenuItem";
			this.openMenuItem.Size = new System.Drawing.Size(253, 22);
			this.openMenuItem.Text = "Open";
			this.openMenuItem.Visible = false;
			this.openMenuItem.Click += new System.EventHandler(this.openMenuItem_Click);
			// 
			// openMenuItemSeparator
			// 
			this.openMenuItemSeparator.Name = "openMenuItemSeparator";
			this.openMenuItemSeparator.Size = new System.Drawing.Size(250, 6);
			this.openMenuItemSeparator.Visible = false;
			// 
			// registerMenuItem
			// 
			this.registerMenuItem.Name = "registerMenuItem";
			this.registerMenuItem.Size = new System.Drawing.Size(253, 22);
			this.registerMenuItem.Text = "Create New Authenticator...";
			this.registerMenuItem.Click += new System.EventHandler(this.registerMenuItem_Click);
			// 
			// toolStripSeparator4
			// 
			this.toolStripSeparator4.Name = "toolStripSeparator4";
			this.toolStripSeparator4.Size = new System.Drawing.Size(250, 6);
			// 
			// loadMenuItem
			// 
			this.loadMenuItem.Name = "loadMenuItem";
			this.loadMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.O)));
			this.loadMenuItem.Size = new System.Drawing.Size(253, 22);
			this.loadMenuItem.Text = "Load...";
			this.loadMenuItem.Click += new System.EventHandler(this.loadMenuItem_Click);
			// 
			// saveAsMenuItem
			// 
			this.saveAsMenuItem.Name = "saveAsMenuItem";
			this.saveAsMenuItem.Size = new System.Drawing.Size(253, 22);
			this.saveAsMenuItem.Text = "Save...";
			this.saveAsMenuItem.Click += new System.EventHandler(this.saveAsMenuItem_Click);
			// 
			// toolStripSeparator2
			// 
			this.toolStripSeparator2.Name = "toolStripSeparator2";
			this.toolStripSeparator2.Size = new System.Drawing.Size(250, 6);
			// 
			// syncServerTimeMenuItem
			// 
			this.syncServerTimeMenuItem.Name = "syncServerTimeMenuItem";
			this.syncServerTimeMenuItem.Size = new System.Drawing.Size(253, 22);
			this.syncServerTimeMenuItem.Text = "Sync Server Time";
			this.syncServerTimeMenuItem.Click += new System.EventHandler(this.syncServerTimeMenuItem_Click);
			// 
			// toolStripSeparator1
			// 
			this.toolStripSeparator1.Name = "toolStripSeparator1";
			this.toolStripSeparator1.Size = new System.Drawing.Size(250, 6);
			// 
			// showRestoreCodeMenuItem
			// 
			this.showRestoreCodeMenuItem.Name = "showRestoreCodeMenuItem";
			this.showRestoreCodeMenuItem.Size = new System.Drawing.Size(253, 22);
			this.showRestoreCodeMenuItem.Text = "Show Battle.Net Restore Code...";
			this.showRestoreCodeMenuItem.Click += new System.EventHandler(this.showRestoreCodeMenuItem_Click);
			// 
			// restoreMenuItem
			// 
			this.restoreMenuItem.Name = "restoreMenuItem";
			this.restoreMenuItem.Size = new System.Drawing.Size(253, 22);
			this.restoreMenuItem.Text = "Restore Battle.Net Authenticator...";
			this.restoreMenuItem.Click += new System.EventHandler(this.restoreMenuItem_Click);
			// 
			// toolStripSeparator9
			// 
			this.toolStripSeparator9.Name = "toolStripSeparator9";
			this.toolStripSeparator9.Size = new System.Drawing.Size(250, 6);
			// 
			// importMenuItem
			// 
			this.importMenuItem.Name = "importMenuItem";
			this.importMenuItem.Size = new System.Drawing.Size(253, 22);
			this.importMenuItem.Text = "Import Raw Battle.Net Key...";
			this.importMenuItem.Click += new System.EventHandler(this.importMenuItem_Click);
			// 
			// exportKeyMenuItem
			// 
			this.exportKeyMenuItem.Name = "exportKeyMenuItem";
			this.exportKeyMenuItem.Size = new System.Drawing.Size(253, 22);
			this.exportKeyMenuItem.Text = "Export Raw Battle.Net Key...";
			this.exportKeyMenuItem.Click += new System.EventHandler(this.exportKeyMenuItem_Click);
			// 
			// toolStripSeparator7
			// 
			this.toolStripSeparator7.Name = "toolStripSeparator7";
			this.toolStripSeparator7.Size = new System.Drawing.Size(250, 6);
			// 
			// autoLoginMenuItem
			// 
			this.autoLoginMenuItem.Name = "autoLoginMenuItem";
			this.autoLoginMenuItem.Size = new System.Drawing.Size(253, 22);
			this.autoLoginMenuItem.Text = "Auto Login...";
			this.autoLoginMenuItem.Click += new System.EventHandler(this.autoLoginMenuItem_Click);
			// 
			// toolStripSeparator6
			// 
			this.toolStripSeparator6.Name = "toolStripSeparator6";
			this.toolStripSeparator6.Size = new System.Drawing.Size(250, 6);
			// 
			// menuItemSkin
			// 
			this.menuItemSkin.Name = "menuItemSkin";
			this.menuItemSkin.Size = new System.Drawing.Size(253, 22);
			this.menuItemSkin.Text = "Choose Skin...";
			this.menuItemSkin.Click += new System.EventHandler(this.menuItemSkin_Click);
			// 
			// defaultSkinMenuItem
			// 
			this.defaultSkinMenuItem.Name = "defaultSkinMenuItem";
			this.defaultSkinMenuItem.Size = new System.Drawing.Size(253, 22);
			this.defaultSkinMenuItem.Tag = "";
			this.defaultSkinMenuItem.Text = "Always use this skin";
			this.defaultSkinMenuItem.Click += new System.EventHandler(this.defaultSkinMenuItem_Click);
			// 
			// toolStripSeparator10
			// 
			this.toolStripSeparator10.Name = "toolStripSeparator10";
			this.toolStripSeparator10.Size = new System.Drawing.Size(250, 6);
			// 
			// startWithWindowsMenuItem
			// 
			this.startWithWindowsMenuItem.Name = "startWithWindowsMenuItem";
			this.startWithWindowsMenuItem.Size = new System.Drawing.Size(253, 22);
			this.startWithWindowsMenuItem.Text = "Start with Windows";
			this.startWithWindowsMenuItem.Click += new System.EventHandler(this.startWithWindowsMenuItem_Click);
			// 
			// useTrayIconMenuItem
			// 
			this.useTrayIconMenuItem.Name = "useTrayIconMenuItem";
			this.useTrayIconMenuItem.Size = new System.Drawing.Size(253, 22);
			this.useTrayIconMenuItem.Text = "Use System Tray Icon";
			this.useTrayIconMenuItem.Click += new System.EventHandler(this.useTrayIconMenuItem_Click);
			// 
			// alwaysOnTopMenuItem
			// 
			this.alwaysOnTopMenuItem.Name = "alwaysOnTopMenuItem";
			this.alwaysOnTopMenuItem.Size = new System.Drawing.Size(253, 22);
			this.alwaysOnTopMenuItem.Text = "Always on Top";
			this.alwaysOnTopMenuItem.Click += new System.EventHandler(this.alwaysOnTopMenuItem_Click);
			// 
			// toolStripSeparator8
			// 
			this.toolStripSeparator8.Name = "toolStripSeparator8";
			this.toolStripSeparator8.Size = new System.Drawing.Size(250, 6);
			// 
			// allowCopyMenuItem
			// 
			this.allowCopyMenuItem.Name = "allowCopyMenuItem";
			this.allowCopyMenuItem.Size = new System.Drawing.Size(253, 22);
			this.allowCopyMenuItem.Text = "Allow Copying of Code";
			this.allowCopyMenuItem.Click += new System.EventHandler(this.allowCopyMenuItem_Click);
			// 
			// hideSerialMenuItem
			// 
			this.hideSerialMenuItem.Name = "hideSerialMenuItem";
			this.hideSerialMenuItem.Size = new System.Drawing.Size(253, 22);
			this.hideSerialMenuItem.Text = "Hide Serial Number";
			this.hideSerialMenuItem.Click += new System.EventHandler(this.hideSerialMenuItem_Click);
			// 
			// copyOnCodeMenuItem
			// 
			this.copyOnCodeMenuItem.Name = "copyOnCodeMenuItem";
			this.copyOnCodeMenuItem.Size = new System.Drawing.Size(253, 22);
			this.copyOnCodeMenuItem.Text = "Auto Clipboard Copy";
			this.copyOnCodeMenuItem.Click += new System.EventHandler(this.copyOnCodeMenuItem_Click);
			// 
			// autoRefreshMenuItem
			// 
			this.autoRefreshMenuItem.Name = "autoRefreshMenuItem";
			this.autoRefreshMenuItem.Size = new System.Drawing.Size(253, 22);
			this.autoRefreshMenuItem.Text = "Auto Refresh";
			this.autoRefreshMenuItem.Click += new System.EventHandler(this.autoRefreshMenuItem_Click);
			// 
			// toolStripSeparator3
			// 
			this.toolStripSeparator3.Name = "toolStripSeparator3";
			this.toolStripSeparator3.Size = new System.Drawing.Size(250, 6);
			// 
			// aboutMenuItem
			// 
			this.aboutMenuItem.Name = "aboutMenuItem";
			this.aboutMenuItem.Size = new System.Drawing.Size(253, 22);
			this.aboutMenuItem.Text = "About...";
			this.aboutMenuItem.Click += new System.EventHandler(this.aboutMenuItem_Click);
			// 
			// toolStripSeparator5
			// 
			this.toolStripSeparator5.Name = "toolStripSeparator5";
			this.toolStripSeparator5.Size = new System.Drawing.Size(250, 6);
			// 
			// exitMenuItem
			// 
			this.exitMenuItem.Name = "exitMenuItem";
			this.exitMenuItem.ShortcutKeys = ((System.Windows.Forms.Keys)((System.Windows.Forms.Keys.Alt | System.Windows.Forms.Keys.F4)));
			this.exitMenuItem.Size = new System.Drawing.Size(253, 22);
			this.exitMenuItem.Text = "E&xit";
			this.exitMenuItem.Click += new System.EventHandler(this.exitMenuItem_Click);
			// 
			// serialLabel
			// 
			this.serialLabel.AutoSize = true;
			this.serialLabel.BackColor = System.Drawing.Color.Transparent;
			this.serialLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serialLabel.ForeColor = System.Drawing.SystemColors.Control;
			this.serialLabel.Location = new System.Drawing.Point(92, 160);
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
			// WindowsAuthenticator.ColouredProgressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(25, 180);
			this.progressBar.Maximum = 29;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(270, 6);
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
			// syncTooltip
			// 
			this.syncTooltip.ToolTipTitle = "Synchronize Time";
			// 
			// calcCodeButton
			// 
			this.calcCodeButton.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
			this.calcCodeButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.calcCodeButton.FlatAppearance.BorderColor = System.Drawing.Color.Black;
			this.calcCodeButton.FlatAppearance.BorderSize = 0;
			this.calcCodeButton.FlatAppearance.MouseDownBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
			this.calcCodeButton.FlatAppearance.MouseOverBackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
			this.calcCodeButton.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
			this.calcCodeButton.Font = new System.Drawing.Font("Arial Narrow", 18F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.calcCodeButton.ForeColor = System.Drawing.Color.White;
			this.calcCodeButton.Location = new System.Drawing.Point(70, 210);
			this.calcCodeButton.Name = "calcCodeButton";
			this.calcCodeButton.Size = new System.Drawing.Size(180, 37);
			this.calcCodeButton.TabIndex = 1;
			this.calcCodeButton.TabStop = false;
			this.calcCodeButton.Text = "View Code";
			this.calcCodeButton.UseVisualStyleBackColor = false;
			this.calcCodeButton.Visible = false;
			this.calcCodeButton.Click += new System.EventHandler(this.calcCodeButton_Click);
			// 
			// codeTooltip
			// 
			this.codeTooltip.ToolTipTitle = "Refresh Code";
			// 
			// copyClipboardButton
			// 
			this.copyClipboardButton.BackColor = System.Drawing.Color.Transparent;
			this.copyClipboardButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.copyClipboardButton.DialogResult = System.Windows.Forms.DialogResult.None;
			this.copyClipboardButton.Location = new System.Drawing.Point(260, 360);
			this.copyClipboardButton.Name = "copyClipboardButton";
			this.copyClipboardButton.Size = new System.Drawing.Size(45, 45);
			this.copyClipboardButton.TabIndex = 5;
			this.copyClipboardButton.TabStop = false;
			this.copyClipboardButton.Text = "Copy";
			this.copyClipboardButton.UseVisualStyleBackColor = true;
			this.copyClipboardButton.Click += new System.EventHandler(this.copyClipboardButton_Click);
			// 
			// syncButton
			// 
			this.syncButton.BackColor = System.Drawing.Color.Transparent;
			this.syncButton.Cursor = System.Windows.Forms.Cursors.Hand;
			this.syncButton.DialogResult = System.Windows.Forms.DialogResult.None;
			this.syncButton.Location = new System.Drawing.Point(15, 360);
			this.syncButton.Name = "syncButton";
			this.syncButton.Size = new System.Drawing.Size(45, 45);
			this.syncButton.TabIndex = 5;
			this.syncButton.TabStop = false;
			this.syncButton.Text = "Sync";
			this.syncButton.UseVisualStyleBackColor = true;
			this.syncButton.Click += new System.EventHandler(this.syncButton_Click);
			// 
			// codeField
			// 
			this.codeField.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(15)))), ((int)(((byte)(45)))), ((int)(((byte)(53)))));
			this.codeField.BorderStyle = System.Windows.Forms.BorderStyle.None;
			this.codeField.Cursor = System.Windows.Forms.Cursors.Hand;
			this.codeField.Font = new System.Drawing.Font("Arial", 22F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeField.ForeColor = System.Drawing.Color.White;
			this.codeField.Location = new System.Drawing.Point(26, 120);
			this.codeField.Name = "codeField";
			this.codeField.ReadOnly = true;
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(269, 34);
			this.codeField.SpaceOut = 0;
			this.codeField.TabIndex = 0;
			this.codeField.TabStop = false;
			this.codeField.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// MainForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BackgroundImage = global::WindowsAuthenticator.Properties.Resources.BattleNetBackground;
			this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Center;
			this.ClientSize = new System.Drawing.Size(320, 419);
			this.ContextMenuStrip = this.contextMenuStrip;
			this.Controls.Add(this.progressBar);
			this.Controls.Add(this.copyClipboardButton);
			this.Controls.Add(this.syncButton);
			this.Controls.Add(this.calcCodeButton);
			this.Controls.Add(this.serialLabel);
			this.Controls.Add(this.codeField);
			this.DoubleBuffered = true;
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
		private System.Windows.Forms.Button calcCodeButton;
		private System.Windows.Forms.ContextMenuStrip contextMenuStrip;
		private System.Windows.Forms.ToolStripMenuItem autoRefreshMenuItem;
		private System.Windows.Forms.ToolStripMenuItem exitMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
		private System.Windows.Forms.Label serialLabel;
		private System.Windows.Forms.ToolStripMenuItem copyOnCodeMenuItem;
		private System.Windows.Forms.Timer refreshTimer;
		private WindowsAuthenticator.ColouredProgressBar progressBar;
		private System.Windows.Forms.ToolStripMenuItem syncServerTimeMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator3;
		private System.Windows.Forms.ToolStripMenuItem alwaysOnTopMenuItem;
		private System.Windows.Forms.ToolStripMenuItem registerMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator4;
		private System.Windows.Forms.ToolStripMenuItem loadMenuItem;
		private System.Windows.Forms.ToolStripMenuItem saveAsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem hideSerialMenuItem;
		private System.Windows.Forms.ToolStripMenuItem aboutMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator5;
		private System.Windows.Forms.ToolStripMenuItem allowCopyMenuItem;
		private System.Windows.Forms.ToolStripMenuItem autoLoginMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator6;
		private System.Windows.Forms.ToolStripMenuItem importMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator7;
		private System.Windows.Forms.ToolStripMenuItem exportKeyMenuItem;
		private System.Windows.Forms.NotifyIcon notifyIcon;
		private System.Windows.Forms.ToolStripMenuItem useTrayIconMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator8;
		private System.Windows.Forms.ToolStripMenuItem startWithWindowsMenuItem;
		private System.Windows.Forms.ToolStripMenuItem openMenuItem;
		private System.Windows.Forms.ToolStripSeparator openMenuItemSeparator;
		private System.Windows.Forms.ToolStripMenuItem restoreMenuItem;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator9;
		private System.Windows.Forms.ToolStripMenuItem showRestoreCodeMenuItem;
		private WindowsAuthenticator.TransparentButton copyClipboardButton;
		private WindowsAuthenticator.TransparentButton syncButton;
		private System.Windows.Forms.ToolStripMenuItem menuItemSkin;
		private System.Windows.Forms.ToolStripSeparator toolStripSeparator10;
		private System.Windows.Forms.ToolStripMenuItem defaultSkinMenuItem;
		private System.Windows.Forms.ToolTip syncTooltip;
		private System.Windows.Forms.ToolTip clipboardTooltip;
		private System.Windows.Forms.ToolTip codeTooltip;

	}
}

