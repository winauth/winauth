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
			this.steamdataField = new WinAuth.SecretTextBox();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.deviceidField = new WinAuth.SecretTextBox();
			this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
			this.SuspendLayout();
			// 
			// allowCopyCheckBox
			// 
			this.allowCopyCheckBox.AutoSize = true;
			this.allowCopyCheckBox.Location = new System.Drawing.Point(222, 159);
			this.allowCopyCheckBox.Name = "allowCopyCheckBox";
			this.allowCopyCheckBox.Size = new System.Drawing.Size(82, 15);
			this.allowCopyCheckBox.TabIndex = 1;
			this.allowCopyCheckBox.Text = "Allow copy";
			this.allowCopyCheckBox.UseSelectable = true;
			this.allowCopyCheckBox.CheckedChanged += new System.EventHandler(this.allowCopyCheckBox_CheckedChanged);
			// 
			// revocationcodeField
			// 
			this.revocationcodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 10F);
			this.revocationcodeField.Location = new System.Drawing.Point(104, 152);
			this.revocationcodeField.Multiline = true;
			this.revocationcodeField.Name = "revocationcodeField";
			this.revocationcodeField.SecretMode = false;
			this.revocationcodeField.Size = new System.Drawing.Size(112, 26);
			this.revocationcodeField.SpaceOut = 0;
			this.revocationcodeField.TabIndex = 0;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 155);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(75, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "Code";
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(23, 60);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(459, 89);
			this.label4.TabIndex = 1;
			this.label4.Text = "When your Authenticator was registered with Steam, a code was created that you ca" +
    "n use within the Steam client to remove your authenticator.\r\n\r\nCopy it down and " +
    "keep it somewhere safe.";
			// 
			// btnClose
			// 
			this.btnClose.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnClose.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnClose.Location = new System.Drawing.Point(393, 419);
			this.btnClose.Name = "btnClose";
			this.btnClose.Size = new System.Drawing.Size(75, 23);
			this.btnClose.TabIndex = 2;
			this.btnClose.Text = "Close";
			this.btnClose.UseSelectable = true;
			// 
			// steamdataField
			// 
			this.steamdataField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.steamdataField.ForeColor = System.Drawing.SystemColors.InactiveCaption;
			this.steamdataField.Location = new System.Drawing.Point(23, 284);
			this.steamdataField.Multiline = true;
			this.steamdataField.Name = "steamdataField";
			this.steamdataField.SecretMode = false;
			this.steamdataField.Size = new System.Drawing.Size(445, 116);
			this.steamdataField.SpaceOut = 0;
			this.steamdataField.TabIndex = 0;
			this.steamdataField.TabStop = false;
			this.steamdataField.Text = "WARNING: There is no additional Steam data.\r\n\r\nYour authenticator was created in " +
    "an early version of WinAuth. You need to remove the authenticator from your Stea" +
    "m account and add a new one.";
			// 
			// metroLabel2
			// 
			this.metroLabel2.Location = new System.Drawing.Point(23, 252);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(75, 19);
			this.metroLabel2.TabIndex = 1;
			this.metroLabel2.Text = "Device ID";
			// 
			// deviceidField
			// 
			this.deviceidField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F);
			this.deviceidField.Location = new System.Drawing.Point(104, 249);
			this.deviceidField.Multiline = true;
			this.deviceidField.Name = "deviceidField";
			this.deviceidField.SecretMode = false;
			this.deviceidField.Size = new System.Drawing.Size(364, 26);
			this.deviceidField.SpaceOut = 0;
			this.deviceidField.TabIndex = 0;
			this.deviceidField.TabStop = false;
			// 
			// metroLabel3
			// 
			this.metroLabel3.Location = new System.Drawing.Point(22, 196);
			this.metroLabel3.Name = "metroLabel3";
			this.metroLabel3.Size = new System.Drawing.Size(446, 40);
			this.metroLabel3.TabIndex = 1;
			this.metroLabel3.Text = "This is the full Steam data for your authenticator that can be used when importin" +
    "g into different software.";
			// 
			// ShowSteamSecretForm
			// 
			this.AcceptButton = this.btnClose;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.btnClose;
			this.ClientSize = new System.Drawing.Size(491, 465);
			this.Controls.Add(this.allowCopyCheckBox);
			this.Controls.Add(this.btnClose);
			this.Controls.Add(this.steamdataField);
			this.Controls.Add(this.deviceidField);
			this.Controls.Add(this.revocationcodeField);
			this.Controls.Add(this.metroLabel3);
			this.Controls.Add(this.label4);
			this.Controls.Add(this.metroLabel2);
			this.Controls.Add(this.label2);
			this.Name = "ShowSteamSecretForm";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Recovery Code";
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
		private SecretTextBox steamdataField;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private SecretTextBox deviceidField;
		private MetroFramework.Controls.MetroLabel metroLabel3;
	}
}