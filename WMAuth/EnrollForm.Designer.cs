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
	partial class EnrollForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnrollForm));
			this.enrollTimer = new System.Windows.Forms.Timer();
			this.enrollLabel = new WindowsAuthenticator.TransparentLabel();
			this.enrollProgressBar = new System.Windows.Forms.ProgressBar();
			this.enrollIntroLabel = new WindowsAuthenticator.TransparentLabel();
			this.rbRegionUS = new System.Windows.Forms.RadioButton();
			this.enrollRegionPanel = new System.Windows.Forms.Panel();
			this.rbImportBMA = new System.Windows.Forms.RadioButton();
			this.rbRegionEU = new System.Windows.Forms.RadioButton();
			this.enrollMenu = new System.Windows.Forms.MainMenu();
			this.registerMenuItem = new System.Windows.Forms.MenuItem();
			this.enrollExitMenuItem = new System.Windows.Forms.MenuItem();
			this.codeLabel = new WindowsAuthenticator.TransparentLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.label2 = new WindowsAuthenticator.TransparentLabel();
			this.label1 = new WindowsAuthenticator.TransparentLabel();
			this.codePictureBox = new System.Windows.Forms.PictureBox();
			this.enrollRegionPanel.SuspendLayout();
			this.SuspendLayout();
			// 
			// enrollTimer
			// 
			this.enrollTimer.Enabled = true;
			this.enrollTimer.Interval = 200;
			this.enrollTimer.Tick += new System.EventHandler(this.enrollTimer_Tick);
			// 
			// enrollLabel
			// 
			this.enrollLabel.BackColor = System.Drawing.Color.Black;
			this.enrollLabel.ForeColor = System.Drawing.Color.White;
			this.enrollLabel.Location = new System.Drawing.Point(40, 377);
			this.enrollLabel.Name = "enrollLabel";
			this.enrollLabel.Size = new System.Drawing.Size(403, 40);
			this.enrollLabel.TabIndex = 7;
			this.enrollLabel.Text = "statusLabel";
			// 
			// enrollProgressBar
			// 
			this.enrollProgressBar.Location = new System.Drawing.Point(40, 351);
			this.enrollProgressBar.Maximum = 20;
			this.enrollProgressBar.Name = "enrollProgressBar";
			this.enrollProgressBar.Size = new System.Drawing.Size(403, 20);
			this.enrollProgressBar.Visible = false;
			// 
			// enrollIntroLabel
			// 
			this.enrollIntroLabel.BackColor = System.Drawing.Color.Black;
			this.enrollIntroLabel.ForeColor = System.Drawing.Color.White;
			this.enrollIntroLabel.Location = new System.Drawing.Point(40, 208);
			this.enrollIntroLabel.Name = "enrollIntroLabel";
			this.enrollIntroLabel.Size = new System.Drawing.Size(103, 36);
			this.enrollIntroLabel.TabIndex = 6;
			this.enrollIntroLabel.Text = "Region:";
			// 
			// rbRegionUS
			// 
			this.rbRegionUS.Checked = true;
			this.rbRegionUS.ForeColor = System.Drawing.Color.White;
			this.rbRegionUS.Location = new System.Drawing.Point(0, 0);
			this.rbRegionUS.Name = "rbRegionUS";
			this.rbRegionUS.Size = new System.Drawing.Size(88, 40);
			this.rbRegionUS.TabIndex = 0;
			this.rbRegionUS.TabStop = false;
			this.rbRegionUS.Text = "US";
			// 
			// enrollRegionPanel
			// 
			this.enrollRegionPanel.BackColor = System.Drawing.Color.Black;
			this.enrollRegionPanel.Controls.Add(this.rbImportBMA);
			this.enrollRegionPanel.Controls.Add(this.rbRegionUS);
			this.enrollRegionPanel.Controls.Add(this.rbRegionEU);
			this.enrollRegionPanel.Location = new System.Drawing.Point(161, 205);
			this.enrollRegionPanel.Name = "enrollRegionPanel";
			this.enrollRegionPanel.Size = new System.Drawing.Size(215, 140);
			// 
			// rbImportBMA
			// 
			this.rbImportBMA.ForeColor = System.Drawing.Color.White;
			this.rbImportBMA.Location = new System.Drawing.Point(0, 100);
			this.rbImportBMA.Name = "rbImportBMA";
			this.rbImportBMA.Size = new System.Drawing.Size(196, 40);
			this.rbImportBMA.TabIndex = 2;
			this.rbImportBMA.TabStop = false;
			this.rbImportBMA.Text = "Import BMA";
			// 
			// rbRegionEU
			// 
			this.rbRegionEU.ForeColor = System.Drawing.Color.White;
			this.rbRegionEU.Location = new System.Drawing.Point(0, 40);
			this.rbRegionEU.Name = "rbRegionEU";
			this.rbRegionEU.Size = new System.Drawing.Size(88, 40);
			this.rbRegionEU.TabIndex = 1;
			this.rbRegionEU.TabStop = false;
			this.rbRegionEU.Text = "EU";
			// 
			// enrollMenu
			// 
			this.enrollMenu.MenuItems.Add(this.registerMenuItem);
			this.enrollMenu.MenuItems.Add(this.enrollExitMenuItem);
			// 
			// registerMenuItem
			// 
			this.registerMenuItem.Text = "OK";
			this.registerMenuItem.Click += new System.EventHandler(this.registerMenuItem_Click);
			// 
			// enrollExitMenuItem
			// 
			this.enrollExitMenuItem.Text = "Cancel";
			this.enrollExitMenuItem.Click += new System.EventHandler(this.enrollExitMenuItem_Click);
			// 
			// codeLabel
			// 
			this.codeLabel.BackColor = System.Drawing.Color.Black;
			this.codeLabel.ForeColor = System.Drawing.Color.White;
			this.codeLabel.Image = ((System.Drawing.Image)(resources.GetObject("codeLabel.Image")));
			this.codeLabel.Location = new System.Drawing.Point(40, 110);
			this.codeLabel.Name = "codeLabel";
			this.codeLabel.Size = new System.Drawing.Size(403, 68);
			this.codeLabel.TabIndex = 4;
			this.codeLabel.Text = "Setup New Authenticator";
			this.codeLabel.TextAlign = System.Drawing.ContentAlignment.TopCenter;
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(40, 20);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(64, 64);
			// 
			// label2
			// 
			this.label2.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.label2.Font = new System.Drawing.Font("Tahoma", 6F, System.Drawing.FontStyle.Regular);
			this.label2.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.label2.Location = new System.Drawing.Point(108, 58);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(335, 26);
			this.label2.TabIndex = 2;
			this.label2.Text = "Windows Mobile Authenticator";
			// 
			// label1
			// 
			this.label1.BackColor = System.Drawing.SystemColors.WindowFrame;
			this.label1.Font = new System.Drawing.Font("Tahoma", 10F, System.Drawing.FontStyle.Regular);
			this.label1.ForeColor = System.Drawing.SystemColors.ControlLight;
			this.label1.Location = new System.Drawing.Point(108, 20);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(335, 38);
			this.label1.TabIndex = 3;
			this.label1.Text = "BATTLE.NET";
			// 
			// codePictureBox
			// 
			this.codePictureBox.Image = ((System.Drawing.Image)(resources.GetObject("codePictureBox.Image")));
			this.codePictureBox.Location = new System.Drawing.Point(40, 110);
			this.codePictureBox.Name = "codePictureBox";
			this.codePictureBox.Size = new System.Drawing.Size(42, 68);
			this.codePictureBox.Visible = false;
			// 
			// EnrollForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(192F, 192F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
			this.AutoScroll = true;
			this.ClientSize = new System.Drawing.Size(480, 696);
			this.ControlBox = false;
			this.Controls.Add(this.enrollLabel);
			this.Controls.Add(this.codePictureBox);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.label1);
			this.Controls.Add(this.codeLabel);
			this.Controls.Add(this.enrollProgressBar);
			this.Controls.Add(this.enrollIntroLabel);
			this.Controls.Add(this.enrollRegionPanel);
			this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
			this.Location = new System.Drawing.Point(0, 52);
			this.Menu = this.enrollMenu;
			this.Name = "EnrollForm";
			this.Text = "s";
			this.Load += new System.EventHandler(this.EnrollForm_Load);
			this.enrollRegionPanel.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Timer enrollTimer;
		private TransparentLabel enrollLabel;
		private System.Windows.Forms.ProgressBar enrollProgressBar;
		private TransparentLabel enrollIntroLabel;
		private System.Windows.Forms.RadioButton rbRegionUS;
		private System.Windows.Forms.Panel enrollRegionPanel;
		private System.Windows.Forms.RadioButton rbRegionEU;
		private System.Windows.Forms.MainMenu enrollMenu;
		private TransparentLabel codeLabel;
		private System.Windows.Forms.MenuItem registerMenuItem;
		private System.Windows.Forms.MenuItem enrollExitMenuItem;
		private System.Windows.Forms.PictureBox pictureBox1;
		private TransparentLabel label2;
		private TransparentLabel label1;
		private System.Windows.Forms.PictureBox codePictureBox;
		private System.Windows.Forms.RadioButton rbImportBMA;
	}
}

