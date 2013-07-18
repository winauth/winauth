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
	partial class ShowTrionSecretForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ShowTrionSecretForm));
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.allowCopyCheckBox = new System.Windows.Forms.CheckBox();
			this.serialNumberField = new WinAuth.SecretTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.deviceIdField = new WinAuth.SecretTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.btnClose = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.allowCopyCheckBox);
			this.groupBox1.Controls.Add(this.serialNumberField);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.deviceIdField);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(373, 328);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(131, 297);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(77, 17);
			this.allowCopyCheckBox.TabIndex = 4;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.UseVisualStyleBackColor = true;
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// serialNumberField
			// 
			this.serialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.serialNumberField.Location = new System.Drawing.Point(131, 225);
			this.serialNumberField.Multiline = true;
			this.serialNumberField.Name = "serialNumberField";
			this.serialNumberField.SecretMode = false;
			this.serialNumberField.Size = new System.Drawing.Size(224, 30);
			this.serialNumberField.SpaceOut = 0;
			this.serialNumberField.TabIndex = 2;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 230);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(118, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "Serial Number";
			// 
			// deviceIdField
			// 
			this.deviceIdField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.deviceIdField.Location = new System.Drawing.Point(131, 261);
			this.deviceIdField.Multiline = true;
			this.deviceIdField.Name = "deviceIdField";
			this.deviceIdField.SecretMode = false;
			this.deviceIdField.Size = new System.Drawing.Size(224, 30);
			this.deviceIdField.SpaceOut = 0;
			this.deviceIdField.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 266);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Device ID";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(361, 206);
			this.label4.TabIndex = 1;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(310, 356);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseVisualStyleBackColor = true;
			// 
			// ShowTrionSecretForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(398, 391);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ShowTrionSecretForm";
			this.ShowIcon = false;
			this.Text = "Restore Code";
			this.Load += new System.EventHandler(this.ShowTrionSecretForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button btnClose;
		private System.Windows.Forms.Label label4;
		private SecretTextBox deviceIdField;
		private System.Windows.Forms.Label label2;
		private SecretTextBox serialNumberField;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.CheckBox allowCopyCheckBox;
	}
}