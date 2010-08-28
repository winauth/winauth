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
	partial class BackupForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(BackupForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbPasswordVerify = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.tbSubject = new System.Windows.Forms.TextBox();
			this.tbEmail = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnSend = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.tbSmtpUsername = new System.Windows.Forms.TextBox();
			this.label10 = new System.Windows.Forms.Label();
			this.tbSmtpPassword = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.grpSmtpServer = new System.Windows.Forms.GroupBox();
			this.ckSmtpSSL = new System.Windows.Forms.CheckBox();
			this.cbSmtpServer = new System.Windows.Forms.ComboBox();
			this.cbSmtpPorts = new System.Windows.Forms.ComboBox();
			this.label12 = new System.Windows.Forms.Label();
			this.ckSmtpServer = new System.Windows.Forms.CheckBox();
			this.groupBox1.SuspendLayout();
			this.grpSmtpServer.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.tbPasswordVerify);
			this.groupBox1.Controls.Add(this.tbPassword);
			this.groupBox1.Controls.Add(this.tbSubject);
			this.groupBox1.Controls.Add(this.tbEmail);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(419, 244);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Email";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(14, 165);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(399, 18);
			this.label7.TabIndex = 2;
			this.label7.Text = "Enter a password if you would like to encrypt the file";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(14, 20);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(399, 78);
			this.label3.TabIndex = 2;
			this.label3.Text = resources.GetString("label3.Text");
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(14, 215);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(36, 13);
			this.label4.TabIndex = 2;
			this.label4.Text = "Verify:";
			// 
			// label1
			// 
			this.label1.AutoSize = true;
			this.label1.Location = new System.Drawing.Point(14, 189);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(56, 13);
			this.label1.TabIndex = 2;
			this.label1.Text = "Password:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(14, 133);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(43, 13);
			this.label6.TabIndex = 2;
			this.label6.Text = "Subject";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(14, 107);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(73, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Email Address";
			// 
			// tbPasswordVerify
			// 
			this.tbPasswordVerify.Location = new System.Drawing.Point(95, 212);
			this.tbPasswordVerify.Name = "tbPasswordVerify";
			this.tbPasswordVerify.PasswordChar = '*';
			this.tbPasswordVerify.Size = new System.Drawing.Size(149, 20);
			this.tbPasswordVerify.TabIndex = 3;
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(95, 186);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.PasswordChar = '*';
			this.tbPassword.Size = new System.Drawing.Size(149, 20);
			this.tbPassword.TabIndex = 2;
			// 
			// tbSubject
			// 
			this.tbSubject.Location = new System.Drawing.Point(95, 130);
			this.tbSubject.Name = "tbSubject";
			this.tbSubject.Size = new System.Drawing.Size(307, 20);
			this.tbSubject.TabIndex = 1;
			this.tbSubject.Text = "Backup Authenticator file";
			// 
			// tbEmail
			// 
			this.tbEmail.Location = new System.Drawing.Point(95, 104);
			this.tbEmail.Name = "tbEmail";
			this.tbEmail.Size = new System.Drawing.Size(307, 20);
			this.tbEmail.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(14, 27);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(399, 51);
			this.label8.TabIndex = 2;
			this.label8.Text = resources.GetString("label8.Text");
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(356, 463);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// btnSend
			// 
			this.btnSend.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnSend.Location = new System.Drawing.Point(274, 463);
			this.btnSend.Name = "btnSend";
			this.btnSend.Size = new System.Drawing.Size(75, 23);
			this.btnSend.TabIndex = 1;
			this.btnSend.Text = "Send";
			this.btnSend.UseVisualStyleBackColor = true;
			this.btnSend.Click += new System.EventHandler(this.btnBackupSave_Click);
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(14, 84);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(71, 13);
			this.label5.TabIndex = 2;
			this.label5.Text = "SMTP Server";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(14, 110);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(59, 13);
			this.label9.TabIndex = 2;
			this.label9.Text = "SMTP Port";
			// 
			// tbSmtpUsername
			// 
			this.tbSmtpUsername.Location = new System.Drawing.Point(95, 133);
			this.tbSmtpUsername.Name = "tbSmtpUsername";
			this.tbSmtpUsername.Size = new System.Drawing.Size(149, 20);
			this.tbSmtpUsername.TabIndex = 3;
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(14, 136);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(55, 13);
			this.label10.TabIndex = 2;
			this.label10.Text = "Username";
			// 
			// tbSmtpPassword
			// 
			this.tbSmtpPassword.Location = new System.Drawing.Point(95, 159);
			this.tbSmtpPassword.Name = "tbSmtpPassword";
			this.tbSmtpPassword.Size = new System.Drawing.Size(149, 20);
			this.tbSmtpPassword.TabIndex = 4;
			// 
			// label11
			// 
			this.label11.AutoSize = true;
			this.label11.Location = new System.Drawing.Point(14, 162);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(53, 13);
			this.label11.TabIndex = 2;
			this.label11.Text = "Password";
			// 
			// grpSmtpServer
			// 
			this.grpSmtpServer.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.grpSmtpServer.Controls.Add(this.ckSmtpSSL);
			this.grpSmtpServer.Controls.Add(this.cbSmtpServer);
			this.grpSmtpServer.Controls.Add(this.cbSmtpPorts);
			this.grpSmtpServer.Controls.Add(this.label8);
			this.grpSmtpServer.Controls.Add(this.tbSmtpUsername);
			this.grpSmtpServer.Controls.Add(this.tbSmtpPassword);
			this.grpSmtpServer.Controls.Add(this.label5);
			this.grpSmtpServer.Controls.Add(this.label9);
			this.grpSmtpServer.Controls.Add(this.label12);
			this.grpSmtpServer.Controls.Add(this.label10);
			this.grpSmtpServer.Controls.Add(this.label11);
			this.grpSmtpServer.Enabled = false;
			this.grpSmtpServer.Location = new System.Drawing.Point(12, 262);
			this.grpSmtpServer.Name = "grpSmtpServer";
			this.grpSmtpServer.Size = new System.Drawing.Size(419, 192);
			this.grpSmtpServer.TabIndex = 2;
			this.grpSmtpServer.TabStop = false;
			// 
			// ckSmtpSSL
			// 
			this.ckSmtpSSL.AutoSize = true;
			this.ckSmtpSSL.Location = new System.Drawing.Point(170, 109);
			this.ckSmtpSSL.Name = "ckSmtpSSL";
			this.ckSmtpSSL.Size = new System.Drawing.Size(74, 17);
			this.ckSmtpSSL.TabIndex = 2;
			this.ckSmtpSSL.Text = "Use SSL?";
			this.ckSmtpSSL.UseVisualStyleBackColor = true;
			this.ckSmtpSSL.CheckedChanged += new System.EventHandler(this.ckSmtpServer_CheckedChanged);
			// 
			// cbSmtpServer
			// 
			this.cbSmtpServer.FormattingEnabled = true;
			this.cbSmtpServer.Items.AddRange(new object[] {
            "smtp.gmail.com"});
			this.cbSmtpServer.Location = new System.Drawing.Point(95, 81);
			this.cbSmtpServer.Name = "cbSmtpServer";
			this.cbSmtpServer.Size = new System.Drawing.Size(307, 21);
			this.cbSmtpServer.TabIndex = 0;
			this.cbSmtpServer.SelectedIndexChanged += new System.EventHandler(this.cbSmtpServer_SelectedIndexChanged);
			// 
			// cbSmtpPorts
			// 
			this.cbSmtpPorts.FormattingEnabled = true;
			this.cbSmtpPorts.Items.AddRange(new object[] {
            "25",
            "465",
            "587"});
			this.cbSmtpPorts.Location = new System.Drawing.Point(95, 107);
			this.cbSmtpPorts.Name = "cbSmtpPorts";
			this.cbSmtpPorts.Size = new System.Drawing.Size(58, 21);
			this.cbSmtpPorts.TabIndex = 1;
			this.cbSmtpPorts.Text = "25";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.ForeColor = System.Drawing.SystemColors.ControlDarkDark;
			this.label12.Location = new System.Drawing.Point(250, 136);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(59, 13);
			this.label12.TabIndex = 2;
			this.label12.Text = "(if required)";
			// 
			// ckSmtpServer
			// 
			this.ckSmtpServer.AutoSize = true;
			this.ckSmtpServer.Location = new System.Drawing.Point(29, 261);
			this.ckSmtpServer.Name = "ckSmtpServer";
			this.ckSmtpServer.Size = new System.Drawing.Size(151, 17);
			this.ckSmtpServer.TabIndex = 0;
			this.ckSmtpServer.Text = "Use my own SMTP Server";
			this.ckSmtpServer.UseVisualStyleBackColor = true;
			this.ckSmtpServer.CheckedChanged += new System.EventHandler(this.ckSmtpServer_CheckedChanged);
			// 
			// BackupForm
			// 
			this.AcceptButton = this.btnSend;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(443, 493);
			this.Controls.Add(this.ckSmtpServer);
			this.Controls.Add(this.grpSmtpServer);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.btnSend);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "BackupForm";
			this.ShowIcon = false;
			this.Text = "Send Backup";
			this.Load += new System.EventHandler(this.BackupForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.grpSmtpServer.ResumeLayout(false);
			this.grpSmtpServer.PerformLayout();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button btnSend;
		private System.Windows.Forms.TextBox tbPasswordVerify;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.TextBox tbEmail;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox tbSubject;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox tbSmtpUsername;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox tbSmtpPassword;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.GroupBox grpSmtpServer;
		private System.Windows.Forms.CheckBox ckSmtpServer;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.ComboBox cbSmtpPorts;
		private System.Windows.Forms.CheckBox ckSmtpSSL;
		private System.Windows.Forms.ComboBox cbSmtpServer;
	}
}