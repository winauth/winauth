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
	partial class RestoreForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(RestoreForm));
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnOk = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.serial1Field = new System.Windows.Forms.TextBox();
			this.serial2Field = new System.Windows.Forms.TextBox();
			this.restoreCodeField = new System.Windows.Forms.TextBox();
			this.serial3Field = new System.Windows.Forms.TextBox();
			this.serial4Field = new System.Windows.Forms.TextBox();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(309, 282);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 2;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(228, 282);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 1;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click_1);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 16);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(361, 156);
			this.label4.TabIndex = 0;
			this.label4.Text = resources.GetString("label4.Text");
			// 
			// label5
			// 
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(9, 183);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(98, 19);
			this.label5.TabIndex = 1;
			this.label5.Text = "Serial Number";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(173, 183);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(10, 19);
			this.label8.TabIndex = 2;
			this.label8.Text = "-";
			// 
			// label6
			// 
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(9, 217);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(118, 19);
			this.label6.TabIndex = 1;
			this.label6.Text = "Restore Code";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(237, 183);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(11, 19);
			this.label9.TabIndex = 4;
			this.label9.Text = "-";
			// 
			// label10
			// 
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(301, 183);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(10, 19);
			this.label10.TabIndex = 6;
			this.label10.Text = "-";
			// 
			// serial1Field
			// 
			this.serial1Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serial1Field.Location = new System.Drawing.Point(133, 180);
			this.serial1Field.MaxLength = 2;
			this.serial1Field.Name = "serial1Field";
			this.serial1Field.Size = new System.Drawing.Size(31, 21);
			this.serial1Field.TabIndex = 1;
			this.serial1Field.Leave += new System.EventHandler(this.serial1Field_Leave);
			// 
			// serial2Field
			// 
			this.serial2Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serial2Field.Location = new System.Drawing.Point(189, 180);
			this.serial2Field.MaxLength = 4;
			this.serial2Field.Name = "serial2Field";
			this.serial2Field.Size = new System.Drawing.Size(42, 21);
			this.serial2Field.TabIndex = 3;
			// 
			// restoreCodeField
			// 
			this.restoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreCodeField.Location = new System.Drawing.Point(133, 214);
			this.restoreCodeField.MaxLength = 10;
			this.restoreCodeField.Name = "restoreCodeField";
			this.restoreCodeField.Size = new System.Drawing.Size(224, 21);
			this.restoreCodeField.TabIndex = 8;
			// 
			// serial3Field
			// 
			this.serial3Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serial3Field.Location = new System.Drawing.Point(254, 180);
			this.serial3Field.MaxLength = 4;
			this.serial3Field.Name = "serial3Field";
			this.serial3Field.Size = new System.Drawing.Size(42, 21);
			this.serial3Field.TabIndex = 5;
			// 
			// serial4Field
			// 
			this.serial4Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.serial4Field.Location = new System.Drawing.Point(315, 180);
			this.serial4Field.MaxLength = 4;
			this.serial4Field.Name = "serial4Field";
			this.serial4Field.Size = new System.Drawing.Size(42, 21);
			this.serial4Field.TabIndex = 7;
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.serial4Field);
			this.groupBox1.Controls.Add(this.serial3Field);
			this.groupBox1.Controls.Add(this.restoreCodeField);
			this.groupBox1.Controls.Add(this.serial2Field);
			this.groupBox1.Controls.Add(this.serial1Field);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(373, 251);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// RestoreForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(398, 314);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "RestoreForm";
			this.ShowIcon = false;
			this.Text = "Restore Battle.Net Authenticator";
			this.Load += new System.EventHandler(this.RestoreForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TextBox serial1Field;
		private System.Windows.Forms.TextBox serial2Field;
		private System.Windows.Forms.TextBox restoreCodeField;
		private System.Windows.Forms.TextBox serial3Field;
		private System.Windows.Forms.TextBox serial4Field;
		private System.Windows.Forms.GroupBox groupBox1;
	}
}