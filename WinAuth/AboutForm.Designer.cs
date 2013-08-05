namespace WinAuth
{
	partial class AboutForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.aboutLabel = new MetroFramework.Controls.MetroLabel();
			this.licenseLabel = new MetroFramework.Controls.MetroLabel();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.trademarkLabel = new MetroFramework.Controls.MetroLabel();
			this.reportButton = new MetroFramework.Controls.MetroButton();
			this.closeButton = new MetroFramework.Controls.MetroButton();
			this.SuspendLayout();
			// 
			// aboutLabel
			// 
			this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.aboutLabel.Location = new System.Drawing.Point(23, 60);
			this.aboutLabel.Name = "aboutLabel";
			this.aboutLabel.Size = new System.Drawing.Size(366, 45);
			this.aboutLabel.TabIndex = 2;
			this.aboutLabel.Text = "WinAuth {0}\r\nCopyright {1}. Colin Mackie. All rights reserved.\r\n";
			// 
			// licenseLabel
			// 
			this.licenseLabel.AutoSize = true;
			this.licenseLabel.Location = new System.Drawing.Point(23, 128);
			this.licenseLabel.Name = "licenseLabel";
			this.licenseLabel.Size = new System.Drawing.Size(50, 19);
			this.licenseLabel.TabIndex = 3;
			this.licenseLabel.Text = "License";
			// 
			// richTextBox1
			// 
			this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(23, 150);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(366, 133);
			this.richTextBox1.TabIndex = 5;
			this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
			// 
			// trademarkLabel
			// 
			this.trademarkLabel.FontSize = MetroFramework.MetroLabelSize.Small;
			this.trademarkLabel.Location = new System.Drawing.Point(23, 308);
			this.trademarkLabel.Name = "trademarkLabel";
			this.trademarkLabel.Size = new System.Drawing.Size(368, 161);
			this.trademarkLabel.TabIndex = 6;
			this.trademarkLabel.Text = resources.GetString("trademarkLabel.Text");
			// 
			// reportButton
			// 
			this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.reportButton.Location = new System.Drawing.Point(23, 491);
			this.reportButton.Name = "reportButton";
			this.reportButton.Size = new System.Drawing.Size(112, 23);
			this.reportButton.TabIndex = 7;
			this.reportButton.Text = "Diagnostics...";
			this.reportButton.UseSelectable = true;
			this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.closeButton.Location = new System.Drawing.Point(314, 491);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 8;
			this.closeButton.Text = "Close";
			this.closeButton.UseSelectable = true;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(415, 537);
			this.Controls.Add(this.reportButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.trademarkLabel);
			this.Controls.Add(this.richTextBox1);
			this.Controls.Add(this.licenseLabel);
			this.Controls.Add(this.aboutLabel);
			this.Name = "AboutForm";
			this.Resizable = false;
			this.Text = "About WinAuth";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel aboutLabel;
		private MetroFramework.Controls.MetroLabel licenseLabel;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private MetroFramework.Controls.MetroLabel trademarkLabel;
		private MetroFramework.Controls.MetroButton reportButton;
		private MetroFramework.Controls.MetroButton closeButton;
	}
}