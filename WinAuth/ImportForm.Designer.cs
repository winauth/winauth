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
	partial class ImportForm
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
			this.serial4Field = new WindowsAuthenticator.RestrictedTextBox();
			this.serial3Field = new WindowsAuthenticator.RestrictedTextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.serial2Field = new WindowsAuthenticator.RestrictedTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.timeField = new WindowsAuthenticator.RestrictedTextBox();
			this.keyField = new WindowsAuthenticator.RestrictedTextBox();
			this.serial1Field = new WindowsAuthenticator.RestrictedTextBox();
			this.label1 = new System.Windows.Forms.Label();
			this.label10 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.serial4Field);
			this.groupBox1.Controls.Add(this.serial3Field);
			this.groupBox1.Controls.Add(this.label4);
			this.groupBox1.Controls.Add(this.serial2Field);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.timeField);
			this.groupBox1.Controls.Add(this.keyField);
			this.groupBox1.Controls.Add(this.serial1Field);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label9);
			this.groupBox1.Controls.Add(this.label7);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.label8);
			this.groupBox1.Controls.Add(this.label5);
			this.groupBox1.Controls.Add(this.label3);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(553, 229);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			this.groupBox1.Text = "Raw Key";
			// 
			// serial4Field
			// 
			this.serial4Field.ForceUppercase = true;
			this.serial4Field.HexOnly = false;
			this.serial4Field.LettersAndNumbersOnly = true;
			this.serial4Field.LettersOnly = false;
			this.serial4Field.Location = new System.Drawing.Point(244, 48);
			this.serial4Field.MaxLength = 4;
			this.serial4Field.Name = "serial4Field";
			this.serial4Field.NumbersOnly = false;
			this.serial4Field.Size = new System.Drawing.Size(44, 20);
			this.serial4Field.TabIndex = 3;
			// 
			// serial3Field
			// 
			this.serial3Field.ForceUppercase = true;
			this.serial3Field.HexOnly = false;
			this.serial3Field.LettersAndNumbersOnly = true;
			this.serial3Field.LettersOnly = false;
			this.serial3Field.Location = new System.Drawing.Point(176, 48);
			this.serial3Field.MaxLength = 4;
			this.serial3Field.Name = "serial3Field";
			this.serial3Field.NumbersOnly = false;
			this.serial3Field.Size = new System.Drawing.Size(44, 20);
			this.serial3Field.TabIndex = 2;
			// 
			// label4
			// 
			this.label4.Location = new System.Drawing.Point(226, 51);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(12, 20);
			this.label4.TabIndex = 3;
			this.label4.Text = "-";
			// 
			// serial2Field
			// 
			this.serial2Field.ForceUppercase = true;
			this.serial2Field.HexOnly = false;
			this.serial2Field.LettersAndNumbersOnly = true;
			this.serial2Field.LettersOnly = false;
			this.serial2Field.Location = new System.Drawing.Point(108, 48);
			this.serial2Field.MaxLength = 4;
			this.serial2Field.Name = "serial2Field";
			this.serial2Field.NumbersOnly = false;
			this.serial2Field.Size = new System.Drawing.Size(44, 20);
			this.serial2Field.TabIndex = 1;
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(158, 51);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(12, 20);
			this.label2.TabIndex = 3;
			this.label2.Text = "-";
			// 
			// timeField
			// 
			this.timeField.ForceUppercase = false;
			this.timeField.HexOnly = false;
			this.timeField.LettersAndNumbersOnly = false;
			this.timeField.LettersOnly = false;
			this.timeField.Location = new System.Drawing.Point(125, 189);
			this.timeField.Name = "timeField";
			this.timeField.NumbersOnly = true;
			this.timeField.Size = new System.Drawing.Size(163, 20);
			this.timeField.TabIndex = 5;
			// 
			// keyField
			// 
			this.keyField.ForceUppercase = true;
			this.keyField.HexOnly = true;
			this.keyField.LettersAndNumbersOnly = false;
			this.keyField.LettersOnly = false;
			this.keyField.Location = new System.Drawing.Point(56, 115);
			this.keyField.MaxLength = 80;
			this.keyField.Name = "keyField";
			this.keyField.NumbersOnly = false;
			this.keyField.Size = new System.Drawing.Size(485, 20);
			this.keyField.TabIndex = 4;
			// 
			// serial1Field
			// 
			this.serial1Field.ForceUppercase = true;
			this.serial1Field.HexOnly = false;
			this.serial1Field.LettersAndNumbersOnly = false;
			this.serial1Field.LettersOnly = true;
			this.serial1Field.Location = new System.Drawing.Point(56, 48);
			this.serial1Field.MaxLength = 2;
			this.serial1Field.Name = "serial1Field";
			this.serial1Field.NumbersOnly = false;
			this.serial1Field.Size = new System.Drawing.Size(28, 20);
			this.serial1Field.TabIndex = 0;
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(90, 51);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(12, 20);
			this.label1.TabIndex = 3;
			this.label1.Text = "-";
			// 
			// label10
			// 
			this.label10.Location = new System.Drawing.Point(294, 192);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(160, 20);
			this.label10.TabIndex = 3;
			this.label10.Text = "(leave blank if you aren\'t sure)";
			// 
			// label9
			// 
			this.label9.Location = new System.Drawing.Point(6, 192);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(113, 20);
			this.label9.TabIndex = 3;
			this.label9.Text = "Time Difference (ms)";
			// 
			// label7
			// 
			this.label7.Location = new System.Drawing.Point(6, 51);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(49, 18);
			this.label7.TabIndex = 3;
			this.label7.Text = "Serial";
			// 
			// label6
			// 
			this.label6.Location = new System.Drawing.Point(6, 118);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(49, 20);
			this.label6.TabIndex = 3;
			this.label6.Text = "Key";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(6, 155);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(535, 31);
			this.label8.TabIndex = 3;
			this.label8.Text = "Enter the time difference. This is a number, being the differnce between your dev" +
    "ice\'s time and the Battle.net server\'s time. It may be a normal number, e.g. \"98" +
    "57\" or a negative number, e.g. \"-965\".";
			// 
			// label5
			// 
			this.label5.Location = new System.Drawing.Point(6, 88);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(535, 20);
			this.label5.TabIndex = 3;
			this.label5.Text = "Enter the key. This will usually be seen as a string of 40 characters containing " +
    "0-9 and A-F, e.g. 8F5BC4DA...";
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(6, 25);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(355, 20);
			this.label3.TabIndex = 3;
			this.label3.Text = "Enter the serial number. This will be something like US-1234-1234-1324.";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.Location = new System.Drawing.Point(409, 247);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 1;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(490, 247);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// ImportForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(577, 280);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "ImportForm";
			this.ShowIcon = false;
			this.Text = "Import Battle.Net Key";
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Label label3;
		private RestrictedTextBox serial4Field;
		private RestrictedTextBox serial3Field;
		private System.Windows.Forms.Label label4;
		private RestrictedTextBox serial2Field;
		private System.Windows.Forms.Label label2;
		private RestrictedTextBox timeField;
		private RestrictedTextBox keyField;
		private RestrictedTextBox serial1Field;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label10;
	}
}