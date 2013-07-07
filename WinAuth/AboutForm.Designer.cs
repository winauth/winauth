namespace WinAuth
{
	using MetroFramework;
	using MetroFramework.Forms;

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
			this.components = new System.ComponentModel.Container();
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AboutForm));
			this.metroStyleManager = new MetroFramework.Components.MetroStyleManager(this.components);
			this.metroStyleExtender = new MetroFramework.Components.MetroStyleExtender(this.components);
			this.aboutLabel = new MetroFramework.Controls.MetroLabel();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.richTextBox2 = new System.Windows.Forms.RichTextBox();
			this.richTextBox1 = new System.Windows.Forms.RichTextBox();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOK = new MetroFramework.Controls.MetroButton();
			this.reportButton = new MetroFramework.Controls.MetroButton();
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).BeginInit();
			this.groupBox1.SuspendLayout();
			this.SuspendLayout();
			// 
			// metroStyleManager
			// 
			this.metroStyleManager.Owner = this;
			// 
			// aboutLabel
			// 
			this.aboutLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.aboutLabel.Location = new System.Drawing.Point(6, 16);
			this.aboutLabel.Name = "aboutLabel";
			this.aboutLabel.Size = new System.Drawing.Size(385, 45);
			this.aboutLabel.TabIndex = 1;
			this.aboutLabel.Text = "WinAuth {0}\r\nCopyright (C) {1}. Colin Mackie. All rights reserved.\r\n";
			// 
			// groupBox1
			// 
			this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.groupBox1.Controls.Add(this.richTextBox2);
			this.groupBox1.Controls.Add(this.richTextBox1);
			this.groupBox1.Controls.Add(this.label2);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Controls.Add(this.aboutLabel);
			this.groupBox1.Location = new System.Drawing.Point(12, 63);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(397, 356);
			this.groupBox1.TabIndex = 2;
			this.groupBox1.TabStop = false;
			// 
			// richTextBox2
			// 
			this.richTextBox2.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox2.Location = new System.Drawing.Point(6, 257);
			this.richTextBox2.Name = "richTextBox2";
			this.richTextBox2.ReadOnly = true;
			this.richTextBox2.Size = new System.Drawing.Size(385, 91);
			this.richTextBox2.TabIndex = 4;
			this.richTextBox2.Text = resources.GetString("richTextBox2.Text");
			// 
			// richTextBox1
			// 
			this.richTextBox1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.richTextBox1.Location = new System.Drawing.Point(6, 93);
			this.richTextBox1.Name = "richTextBox1";
			this.richTextBox1.ReadOnly = true;
			this.richTextBox1.Size = new System.Drawing.Size(385, 133);
			this.richTextBox1.TabIndex = 3;
			this.richTextBox1.Text = resources.GetString("richTextBox1.Text");
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(6, 71);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(385, 19);
			this.label2.TabIndex = 1;
			this.label2.Text = "License";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 240);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(385, 19);
			this.label1.TabIndex = 1;
			this.label1.Text = "Copyright, Trademarks and other notices:";
			// 
			// btnOK
			// 
			this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOK.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnOK.Location = new System.Drawing.Point(334, 426);
			this.btnOK.Name = "btnOK";
			this.btnOK.Size = new System.Drawing.Size(75, 23);
			this.btnOK.TabIndex = 3;
			this.btnOK.Text = "Close";
			this.btnOK.UseSelectable = true;
			this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
			// 
			// reportButton
			// 
			this.reportButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
			this.reportButton.Location = new System.Drawing.Point(12, 424);
			this.reportButton.Name = "reportButton";
			this.reportButton.Size = new System.Drawing.Size(112, 23);
			this.reportButton.TabIndex = 3;
			this.reportButton.Text = "Error Report...";
			this.reportButton.UseSelectable = true;
			this.reportButton.Click += new System.EventHandler(this.reportButton_Click);
			// 
			// AboutForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.btnOK;
			this.ClientSize = new System.Drawing.Size(421, 459);
			this.Controls.Add(this.reportButton);
			this.Controls.Add(this.btnOK);
			this.Controls.Add(this.groupBox1);
			this.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.Name = "AboutForm";
			this.Resizable = false;
			this.ShowIcon = false;
			this.StartPosition = System.Windows.Forms.FormStartPosition.WindowsDefaultLocation;
			this.StyleManager = this.metroStyleManager;
			this.Text = "WinAuth";
			this.Load += new System.EventHandler(this.AboutForm_Load);
			((System.ComponentModel.ISupportInitialize)(this.metroStyleManager)).EndInit();
			this.groupBox1.ResumeLayout(false);
			this.ResumeLayout(false);

		}

		#endregion

		private MetroFramework.Components.MetroStyleManager metroStyleManager;
		private MetroFramework.Components.MetroStyleExtender metroStyleExtender;
		private MetroFramework.Controls.MetroLabel aboutLabel;
		private System.Windows.Forms.GroupBox groupBox1;
		private MetroFramework.Controls.MetroButton btnOK;
		private System.Windows.Forms.RichTextBox richTextBox1;
		private MetroFramework.Controls.MetroButton reportButton;
		private System.Windows.Forms.RichTextBox richTextBox2;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Label label2;
	}
}