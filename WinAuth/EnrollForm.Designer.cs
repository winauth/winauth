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
			this.label1 = new System.Windows.Forms.Label();
			this.rbRegionUS = new System.Windows.Forms.RadioButton();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.rbRegionEU = new System.Windows.Forms.RadioButton();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(5, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(409, 219);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// rbRegionUS
			// 
			this.rbRegionUS.AutoSize = true;
			this.rbRegionUS.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbRegionUS.Location = new System.Drawing.Point(3, 3);
			this.rbRegionUS.Name = "rbRegionUS";
			this.rbRegionUS.Size = new System.Drawing.Size(136, 20);
			this.rbRegionUS.TabIndex = 1;
			this.rbRegionUS.Text = "US (United States)";
			this.rbRegionUS.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(422, 272);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.rbRegionEU);
			this.panel1.Controls.Add(this.rbRegionUS);
			this.panel1.Location = new System.Drawing.Point(6, 238);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(408, 30);
			this.panel1.TabIndex = 2;
			// 
			// rbRegionEU
			// 
			this.rbRegionEU.AutoSize = true;
			this.rbRegionEU.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.rbRegionEU.Location = new System.Drawing.Point(153, 3);
			this.rbRegionEU.Name = "rbRegionEU";
			this.rbRegionEU.Size = new System.Drawing.Size(100, 20);
			this.rbRegionEU.TabIndex = 1;
			this.rbRegionEU.TabStop = true;
			this.rbRegionEU.Text = "EU (Europe)";
			this.rbRegionEU.UseVisualStyleBackColor = true;
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.btnOk.Location = new System.Drawing.Point(278, 289);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 3;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(359, 289);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// EnrollForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(445, 324);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "EnrollForm";
			this.ShowIcon = false;
			this.Text = "New Authenticator";
			this.Load += new System.EventHandler(this.EnrollForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.RadioButton rbRegionUS;
		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.RadioButton rbRegionEU;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}