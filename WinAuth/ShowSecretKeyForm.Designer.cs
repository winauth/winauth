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
namespace WinAuth
{
	partial class ShowSecretKeyForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowSecretKeyForm));
			this.allowCopyCheckBox = new MetroFramework.Controls.MetroCheckBox();
			this.secretKeyField = new WinAuth.SecretTextBox();
			this.qrImage = new System.Windows.Forms.PictureBox();
			this.label4 = new MetroFramework.Controls.MetroLabel();
			this.btnClose = new MetroFramework.Controls.MetroButton();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).BeginInit();
			this.SuspendLayout();
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(23, 266);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(82, 15);
			this.allowCopyCheckBox.TabIndex = 5;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.UseSelectable = true;
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// secretKeyField
			// 
			this.secretKeyField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.secretKeyField.Location = new System.Drawing.Point(23, 210);
			this.secretKeyField.Multiline = true;
			this.secretKeyField.Name = "secretKeyField";
			this.secretKeyField.SecretMode = false;
			this.secretKeyField.Size = new System.Drawing.Size(445, 50);
			this.secretKeyField.SpaceOut = 0;
			this.secretKeyField.TabIndex = 2;
			// 
			// qrImage
			// 
			this.qrImage.BackColor = System.Drawing.SystemColors.ControlDark;
			this.qrImage.Location = new System.Drawing.Point(160, 337);
			this.qrImage.Name = "qrImage";
			this.qrImage.Size = new System.Drawing.Size(150, 150);
			this.qrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.qrImage.TabIndex = 3;
			this.qrImage.TabStop = false;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(23, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(445, 147);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(393, 506);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseSelectable = true;
			// 
			// metroLabel1
			// 
			this.metroLabel1.Location = new System.Drawing.Point(23, 301);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(361, 33);
			this.metroLabel1.TabIndex = 1;
			this.metroLabel1.Text = "You can also scan the QR code with your mobile device.";
			// 
			// ShowSecretKeyForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(491, 552);
			this.Controls.Add(this.allowCopyCheckBox);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.secretKeyField);
			this.Controls.Add(this.qrImage);
			this.Controls.Add(this.metroLabel1);
			this.Controls.Add(this.label4);
			this.Name = "ShowSecretKeyForm";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Secret Key";
			this.Load += new System.EventHandler(this.ShowSecretKeyForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroButton btnClose;
		private MetroFramework.Controls.MetroLabel label4;
		private SecretTextBox secretKeyField;
		private System.Windows.Forms.PictureBox qrImage;
		private MetroFramework.Controls.MetroCheckBox allowCopyCheckBox;
		private MetroFramework.Controls.MetroLabel metroLabel1;
	}
}