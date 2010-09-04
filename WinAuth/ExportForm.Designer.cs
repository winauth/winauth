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
	partial class ExportForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.serialField = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.keyField = new System.Windows.Forms.TextBox();
			this.timeField = new System.Windows.Forms.TextBox();
			this.xmlField = new System.Windows.Forms.TextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.xmlField);
			this.groupBox1.Controls.Add(this.timeField);
			this.groupBox1.Controls.Add(this.keyField);
			this.groupBox1.Controls.Add(this.serialField);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(523, 299);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// serialField
			// 
			this.serialField.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.serialField.Location = new System.Drawing.Point(95, 39);
			this.serialField.Name = "serialField";
			this.serialField.ReadOnly = true;
			this.serialField.Size = new System.Drawing.Size(125, 20);
			this.serialField.TabIndex = 0;
			this.serialField.TabStop = false;
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(7, 94);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(91, 20);
			this.label9.TabIndex = 3;
			this.label9.Text = "Time Offset (ms)";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(7, 42);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(82, 18);
			this.label7.TabIndex = 3;
			this.label7.Text = "Serial Number";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(7, 68);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(82, 20);
			this.label6.TabIndex = 3;
			this.label6.Text = "Secret Key";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Location = new System.Drawing.Point(6, 16);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(511, 32);
			this.label3.TabIndex = 3;
			this.label3.Text = "Raw authenticator data that can be used if you need to load the key information i" +
					"nto other applications.";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.okButton.Location = new System.Drawing.Point(460, 317);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "Close";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// keyField
			// 
			this.keyField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.keyField.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.keyField.Location = new System.Drawing.Point(95, 65);
			this.keyField.Name = "keyField";
			this.keyField.ReadOnly = true;
			this.keyField.Size = new System.Drawing.Size(414, 20);
			this.keyField.TabIndex = 0;
			this.keyField.TabStop = false;
			// 
			// timeField
			// 
			this.timeField.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.timeField.Location = new System.Drawing.Point(95, 91);
			this.timeField.Name = "timeField";
			this.timeField.ReadOnly = true;
			this.timeField.Size = new System.Drawing.Size(86, 20);
			this.timeField.TabIndex = 0;
			this.timeField.TabStop = false;
			// 
			// xmlField
			// 
			this.xmlField.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
									| System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.xmlField.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.xmlField.Location = new System.Drawing.Point(10, 156);
			this.xmlField.Multiline = true;
			this.xmlField.Name = "xmlField";
			this.xmlField.ReadOnly = true;
			this.xmlField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.xmlField.Size = new System.Drawing.Size(499, 126);
			this.xmlField.TabIndex = 0;
			this.xmlField.TabStop = false;
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
									| System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Location = new System.Drawing.Point(7, 132);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(502, 21);
			this.label1.TabIndex = 3;
			this.label1.Text = "This is the authenticator file for the official Android Battle.net Mobile Authent" +
					"icator.";
			// 
			// ExportForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.okButton;
			this.ClientSize = new System.Drawing.Size(547, 350);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.Name = "ExportForm";
			this.ShowIcon = false;
			this.Text = "Export";
			this.Load += new System.EventHandler(this.ExportForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.TextBox serialField;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox timeField;
		private System.Windows.Forms.TextBox keyField;
		private System.Windows.Forms.TextBox xmlField;
		private System.Windows.Forms.Label label1;
	}
}