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
	partial class ShowSteamSecretForm
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
			this.allowCopyCheckBox = new MetroFramework.Controls.MetroCheckBox();
			this.revocationcodeField = new WinAuth.SecretTextBox();
			this.label2 = new MetroFramework.Controls.MetroLabel();
			this.label4 = new MetroFramework.Controls.MetroLabel();
			this.btnClose = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(156, 211);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(82, 15);
			this.allowCopyCheckBox.TabIndex = 4;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.UseSelectable = true;
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// revocationcodeField
			// 
			this.revocationcodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F);
			this.revocationcodeField.Location = new System.Drawing.Point(156, 175);
			this.revocationcodeField.Multiline = true;
			this.revocationcodeField.Name = "revocationcodeField";
			this.revocationcodeField.SecretMode = false;
			this.revocationcodeField.Size = new System.Drawing.Size(168, 30);
			this.revocationcodeField.SpaceOut = 0;
			this.revocationcodeField.TabIndex = 2;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 179);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(118, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Revocation Code";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(23, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(459, 112);
			this.label4.TabIndex = 1;
			this.label4.Text = "When your Authenticator was registered with Steam, a revocation code was created " +
    "that you can use within the Steam client to remove your authenticator.\r\n\r\nCopy i" +
    "t down and keep it somewhere safe.";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(407, 232);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 4;
			this.btnClose.Text = "Close";
			this.btnClose.UseSelectable = true;
			// 
			// ShowSteamSecretForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(505, 278);
			this.Controls.Add(this.allowCopyCheckBox);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.revocationcodeField);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.label2);
			this.Name = "ShowSteamSecretForm";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Revocation Code";
			this.Load += new System.EventHandler(this.ShowSteamSecretForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroButton btnClose;
		private MetroFramework.Controls.MetroLabel label4;
		private SecretTextBox revocationcodeField;
		private MetroFramework.Controls.MetroLabel label2;
		private MetroFramework.Controls.MetroCheckBox allowCopyCheckBox;
	}
}