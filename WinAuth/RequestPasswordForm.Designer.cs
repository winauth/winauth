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
	partial class RequestPasswordForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RequestPasswordForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.groupBox3 = new System.Windows.Forms.GroupBox();
			this.groupBox2 = new System.Windows.Forms.GroupBox();
			this.ckMyPassword = new System.Windows.Forms.CheckBox();
			this.ckAccountPassword = new System.Windows.Forms.CheckBox();
			this.ckUserPassword = new System.Windows.Forms.CheckBox();
			this.ckNoPassword = new System.Windows.Forms.CheckBox();
			this.label3 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbVerify = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(688, 447);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.panel1.Controls.Add(this.groupBox3);
			this.panel1.Controls.Add(this.groupBox2);
			this.panel1.Controls.Add(this.ckMyPassword);
			this.panel1.Controls.Add(this.ckAccountPassword);
			this.panel1.Controls.Add(this.ckUserPassword);
			this.panel1.Controls.Add(this.ckNoPassword);
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label6);
			this.panel1.Controls.Add(this.label5);
			this.panel1.Controls.Add(this.label7);
			this.panel1.Controls.Add(this.label4);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.tbVerify);
			this.panel1.Controls.Add(this.tbPassword);
			this.panel1.Location = new System.Drawing.Point(9, 53);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(673, 388);
			this.panel1.TabIndex = 0;
			// 
			// groupBox3
			// 
			this.groupBox3.Location = new System.Drawing.Point(20, 232);
			this.groupBox3.Name = "groupBox3";
			this.groupBox3.Size = new System.Drawing.Size(632, 2);
			this.groupBox3.TabIndex = 6;
			this.groupBox3.TabStop = false;
			// 
			// groupBox2
			// 
			this.groupBox2.Location = new System.Drawing.Point(19, 67);
			this.groupBox2.Name = "groupBox2";
			this.groupBox2.Size = new System.Drawing.Size(632, 2);
			this.groupBox2.TabIndex = 5;
			this.groupBox2.TabStop = false;
			// 
			// ckMyPassword
			// 
			this.ckMyPassword.AutoSize = true;
			this.ckMyPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckMyPassword.Location = new System.Drawing.Point(20, 247);
			this.ckMyPassword.Name = "ckMyPassword";
			this.ckMyPassword.Size = new System.Drawing.Size(255, 24);
			this.ckMyPassword.TabIndex = 4;
			this.ckMyPassword.Text = "Encrypt it with my own password";
			this.ckMyPassword.UseVisualStyleBackColor = true;
			this.ckMyPassword.CheckedChanged += new System.EventHandler(this.ckMyPassword_CheckedChanged);
			// 
			// ckAccountPassword
			// 
			this.ckAccountPassword.AutoSize = true;
			this.ckAccountPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckAccountPassword.Location = new System.Drawing.Point(20, 155);
			this.ckAccountPassword.Name = "ckAccountPassword";
			this.ckAccountPassword.Size = new System.Drawing.Size(370, 24);
			this.ckAccountPassword.TabIndex = 4;
			this.ckAccountPassword.Text = "Encrypt it so it can only by used on this computer";
			this.ckAccountPassword.UseVisualStyleBackColor = true;
			this.ckAccountPassword.CheckedChanged += new System.EventHandler(this.ckAccountPassword_CheckedChanged);
			// 
			// ckUserPassword
			// 
			this.ckUserPassword.AutoSize = true;
			this.ckUserPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckUserPassword.Location = new System.Drawing.Point(20, 86);
			this.ckUserPassword.Name = "ckUserPassword";
			this.ckUserPassword.Size = new System.Drawing.Size(640, 24);
			this.ckUserPassword.TabIndex = 4;
			this.ckUserPassword.Text = "Encrypt it so it can only be used by the current Windows User account on this com" +
    "puter";
			this.ckUserPassword.UseVisualStyleBackColor = true;
			this.ckUserPassword.CheckedChanged += new System.EventHandler(this.ckUserPassword_CheckedChanged);
			// 
			// ckNoPassword
			// 
			this.ckNoPassword.AutoSize = true;
			this.ckNoPassword.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.ckNoPassword.Location = new System.Drawing.Point(20, 15);
			this.ckNoPassword.Name = "ckNoPassword";
			this.ckNoPassword.Size = new System.Drawing.Size(381, 24);
			this.ckNoPassword.TabIndex = 4;
			this.ckNoPassword.Text = "I do NOT want to use any passwords or encryption";
			this.ckNoPassword.UseVisualStyleBackColor = true;
			this.ckNoPassword.CheckedChanged += new System.EventHandler(this.ckNoPassword_CheckedChanged);
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(36, 350);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(42, 16);
			this.label3.TabIndex = 2;
			this.label3.Text = "Verify";
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(36, 179);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(619, 49);
			this.label6.TabIndex = 2;
			this.label6.Text = resources.GetString("label6.Text");
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(36, 110);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(619, 46);
			this.label5.TabIndex = 2;
			this.label5.Text = resources.GetString("label5.Text");
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(36, 271);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(619, 44);
			this.label7.TabIndex = 2;
			this.label7.Text = resources.GetString("label7.Text");
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(36, 40);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(619, 23);
			this.label4.TabIndex = 2;
			this.label4.Text = "The authenticator will not be encrypted. Someone else could copy this file and cl" +
    "one your authenticator.";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(36, 321);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(68, 16);
			this.label2.TabIndex = 2;
			this.label2.Text = "Password";
			// 
			// tbVerify
			// 
			this.tbVerify.Enabled = false;
			this.tbVerify.Location = new System.Drawing.Point(128, 349);
			this.tbVerify.Name = "tbVerify";
			this.tbVerify.Size = new System.Drawing.Size(188, 20);
			this.tbVerify.TabIndex = 3;
			this.tbVerify.UseSystemPasswordChar = true;
			// 
			// tbPassword
			// 
			this.tbPassword.Enabled = false;
			this.tbPassword.Location = new System.Drawing.Point(128, 318);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(188, 20);
			this.tbPassword.TabIndex = 2;
			this.tbPassword.UseSystemPasswordChar = true;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(9, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(673, 43);
			this.label1.TabIndex = 2;
			this.label1.Text = "It is recommended that you use a password to protect your secret data from any ma" +
    "licious programs that might try and read and mimic your Authenticator.\r\n\r\n";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(544, 465);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(625, 465);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// RequestPasswordForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(718, 502);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "RequestPasswordForm";
			this.ShowIcon = false;
			this.Text = "Password";
			this.Load += new System.EventHandler(this.RequestPasswordForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbVerify;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.CheckBox ckNoPassword;
		private System.Windows.Forms.GroupBox groupBox3;
		private System.Windows.Forms.GroupBox groupBox2;
		private System.Windows.Forms.CheckBox ckMyPassword;
		private System.Windows.Forms.CheckBox ckAccountPassword;
		private System.Windows.Forms.CheckBox ckUserPassword;
	}
}