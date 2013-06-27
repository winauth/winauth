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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.allowCopyCheckBox = new System.Windows.Forms.CheckBox();
			this.showKeyButton = new System.Windows.Forms.Button();
			this.secretKeyField = new WinAuth.SecretTextBox();
			this.qrImage = new System.Windows.Forms.PictureBox();
			this.label4 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).BeginInit();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.allowCopyCheckBox);
			this.groupBox1.Controls.Add(this.showKeyButton);
			this.groupBox1.Controls.Add(this.secretKeyField);
			this.groupBox1.Controls.Add(this.qrImage);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(652, 237);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(487, 211);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
			this.allowCopyCheckBox.TabIndex = 5;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.UseVisualStyleBackColor = true;
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// showKeyButton
			// 
			this.showKeyButton.Location = new System.Drawing.Point(131, 177);
			this.showKeyButton.Name = "showKeyButton";
			this.showKeyButton.Size = new System.Drawing.Size(145, 23);
			this.showKeyButton.TabIndex = 4;
			this.showKeyButton.Text = "Show Secret Key";
			this.showKeyButton.UseVisualStyleBackColor = true;
			this.showKeyButton.Click += new System.EventHandler(this.showKeyButton_Click);
			// 
			// secretKeyField
			// 
			this.secretKeyField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.secretKeyField.Location = new System.Drawing.Point(414, 175);
			this.secretKeyField.Multiline = true;
			this.secretKeyField.Name = "secretKeyField";
			this.secretKeyField.SecretMode = false;
			this.secretKeyField.Size = new System.Drawing.Size(223, 30);
			this.secretKeyField.SpaceOut = 0;
			this.secretKeyField.TabIndex = 2;
			// 
			// qrImage
			// 
			this.qrImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.qrImage.BackColor = System.Drawing.SystemColors.ControlDark;
			this.qrImage.Location = new System.Drawing.Point(451, 16);
			this.qrImage.Name = "qrImage";
			this.qrImage.Size = new System.Drawing.Size(150, 150);
			this.qrImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.qrImage.TabIndex = 3;
			this.qrImage.TabStop = false;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(404, 156);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(589, 264);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// ShowSecretKeyForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(677, 299);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ShowSecretKeyForm";
			this.ShowIcon = false;
			this.Text = "Secret Key";
			this.Load += new System.EventHandler(this.ShowSecretKeyForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.qrImage)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label4;
		private SecretTextBox secretKeyField;
		private System.Windows.Forms.PictureBox qrImage;
		private System.Windows.Forms.Button showKeyButton;
		private System.Windows.Forms.CheckBox allowCopyCheckBox;
	}
}