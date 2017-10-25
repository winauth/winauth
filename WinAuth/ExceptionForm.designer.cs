namespace WinAuth
{
	partial class ExceptionForm
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
      System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ExceptionForm));
      this.dataText = new System.Windows.Forms.TextBox();
      this.errorLabel = new System.Windows.Forms.Label();
      this.quitButton = new System.Windows.Forms.Button();
      this.errorIcon = new System.Windows.Forms.PictureBox();
      this.continueButton = new System.Windows.Forms.Button();
      this.detailsButton = new System.Windows.Forms.Button();
      ((System.ComponentModel.ISupportInitialize)(this.errorIcon)).BeginInit();
      this.SuspendLayout();
      // 
      // dataText
      // 
      this.dataText.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.dataText.Location = new System.Drawing.Point(98, 380);
      this.dataText.Margin = new System.Windows.Forms.Padding(4);
      this.dataText.Multiline = true;
      this.dataText.Name = "dataText";
      this.dataText.ReadOnly = true;
      this.dataText.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
      this.dataText.Size = new System.Drawing.Size(571, 130);
      this.dataText.TabIndex = 5;
      this.dataText.Visible = false;
      // 
      // errorLabel
      // 
      this.errorLabel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
      this.errorLabel.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.errorLabel.Location = new System.Drawing.Point(95, 63);
      this.errorLabel.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
      this.errorLabel.Name = "errorLabel";
      this.errorLabel.Size = new System.Drawing.Size(574, 257);
      this.errorLabel.TabIndex = 4;
      this.errorLabel.Text = resources.GetString("errorLabel.Text");
      // 
      // quitButton
      // 
      this.quitButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.quitButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
      this.quitButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.quitButton.Location = new System.Drawing.Point(569, 324);
      this.quitButton.Margin = new System.Windows.Forms.Padding(4);
      this.quitButton.Name = "quitButton";
      this.quitButton.Size = new System.Drawing.Size(100, 28);
      this.quitButton.TabIndex = 0;
      this.quitButton.Text = "Quit";
      this.quitButton.UseVisualStyleBackColor = true;
      this.quitButton.Click += new System.EventHandler(this.quitButton_Click);
      // 
      // errorIcon
      // 
      this.errorIcon.Location = new System.Drawing.Point(23, 63);
      this.errorIcon.Name = "errorIcon";
      this.errorIcon.Size = new System.Drawing.Size(48, 48);
      this.errorIcon.TabIndex = 6;
      this.errorIcon.TabStop = false;
      // 
      // continueButton
      // 
      this.continueButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
      this.continueButton.DialogResult = System.Windows.Forms.DialogResult.OK;
      this.continueButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.continueButton.Location = new System.Drawing.Point(461, 324);
      this.continueButton.Margin = new System.Windows.Forms.Padding(4);
      this.continueButton.Name = "continueButton";
      this.continueButton.Size = new System.Drawing.Size(100, 28);
      this.continueButton.TabIndex = 0;
      this.continueButton.Text = "Continue";
      this.continueButton.UseVisualStyleBackColor = true;
      this.continueButton.Click += new System.EventHandler(this.continueButton_Click);
      // 
      // detailsButton
      // 
      this.detailsButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.detailsButton.Location = new System.Drawing.Point(98, 324);
      this.detailsButton.Margin = new System.Windows.Forms.Padding(4);
      this.detailsButton.Name = "detailsButton";
      this.detailsButton.Size = new System.Drawing.Size(117, 28);
      this.detailsButton.TabIndex = 0;
      this.detailsButton.Text = "Show Details";
      this.detailsButton.UseVisualStyleBackColor = true;
      this.detailsButton.Click += new System.EventHandler(this.detailsButton_Click);
      // 
      // ExceptionForm
      // 
      this.AcceptButton = this.continueButton;
      this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
      this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
      this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
      this.ClientSize = new System.Drawing.Size(685, 523);
      this.ControlBox = false;
      this.Controls.Add(this.errorLabel);
      this.Controls.Add(this.errorIcon);
      this.Controls.Add(this.detailsButton);
      this.Controls.Add(this.continueButton);
      this.Controls.Add(this.quitButton);
      this.Controls.Add(this.dataText);
      this.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
      this.Margin = new System.Windows.Forms.Padding(4);
      this.MaximizeBox = false;
      this.MinimizeBox = false;
      this.Name = "ExceptionForm";
      this.Resizable = false;
      this.ShowIcon = false;
      this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
      this.Text = "WinAuth Error";
      this.Load += new System.EventHandler(this.ExceptionForm_Load);
      ((System.ComponentModel.ISupportInitialize)(this.errorIcon)).EndInit();
      this.ResumeLayout(false);
      this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.Button quitButton;
		private System.Windows.Forms.Label errorLabel;
		private System.Windows.Forms.TextBox dataText;
		private System.Windows.Forms.PictureBox errorIcon;
		private System.Windows.Forms.Button continueButton;
		private System.Windows.Forms.Button detailsButton;
	}
}