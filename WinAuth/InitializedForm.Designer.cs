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
	partial class InitializedForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(InitializedForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.progressBar = new System.Windows.Forms.ProgressBar();
			this.restoreCodeField = new WindowsAuthenticator.SecretTextBox();
			this.codeField = new WindowsAuthenticator.SecretTextBox();
			this.serialNumberField = new WindowsAuthenticator.SecretTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.progressBar);
			this.groupBox1.Controls.Add(this.restoreCodeField);
			this.groupBox1.Controls.Add(this.codeField);
			this.groupBox1.Controls.Add(this.serialNumberField);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(373, 406);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// progressBar
			// 
			this.progressBar.Location = new System.Drawing.Point(131, 278);
			this.progressBar.Maximum = 29;
			this.progressBar.Name = "progressBar";
			this.progressBar.Size = new System.Drawing.Size(224, 9);
			this.progressBar.Step = 1;
			this.progressBar.TabIndex = 5;
			// 
			// restoreCodeField
			// 
			this.restoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreCodeField.Location = new System.Drawing.Point(131, 362);
			this.restoreCodeField.Multiline = true;
			this.restoreCodeField.Name = "restoreCodeField";
			this.restoreCodeField.SecretMode = false;
			this.restoreCodeField.Size = new System.Drawing.Size(224, 30);
			this.restoreCodeField.SpaceOut = 0;
			this.restoreCodeField.TabIndex = 2;
			// 
			// codeField
			// 
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.codeField.Location = new System.Drawing.Point(131, 246);
			this.codeField.Multiline = true;
			this.codeField.Name = "codeField";
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(224, 30);
			this.codeField.SpaceOut = 0;
			this.codeField.TabIndex = 2;
			// 
			// serialNumberField
			// 
			this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serialNumberField.Location = new System.Drawing.Point(131, 212);
			this.serialNumberField.Multiline = true;
			this.serialNumberField.Name = "serialNumberField";
			this.serialNumberField.SecretMode = false;
			this.serialNumberField.Size = new System.Drawing.Size(224, 30);
			this.serialNumberField.SpaceOut = 0;
			this.serialNumberField.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 367);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Restore Code";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(7, 251);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(118, 19);
			this.label6.TabIndex = 1;
			this.label6.Text = "Login Code";
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(7, 217);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(118, 19);
			this.label5.TabIndex = 1;
			this.label5.Text = "Serial Number";
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(6, 301);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(361, 58);
			this.label7.TabIndex = 1;
			this.label7.Text = "Now copy down the SERIAL NUMBER and following RESTORE CODE. You can use these cod" +
    "es if you lose your authenticator or want to transfer it to another device.";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 44);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(361, 158);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(361, 28);
			this.label1.TabIndex = 1;
			this.label1.Text = "To add your Authenticator to your account:";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOk.Location = new System.Drawing.Point(304, 433);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// refreshTimer
			// 
			this.refreshTimer.Enabled = true;
			this.refreshTimer.Interval = 500;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// InitializedForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnOk;
			this.ClientSize = new System.Drawing.Size(398, 468);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.Name = "InitializedForm";
			this.ShowIcon = false;
			this.Text = "Authenticator";
			this.Load += new System.EventHandler(this.InitializedForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private SecretTextBox codeField;
		private SecretTextBox serialNumberField;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.ProgressBar progressBar;
		private System.Windows.Forms.Timer refreshTimer;
		private SecretTextBox restoreCodeField;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Label label7;
	}
}