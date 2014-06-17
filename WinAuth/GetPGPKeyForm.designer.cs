namespace WinAuth
{
	partial class GetPGPKeyForm
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
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.pgpLabel = new MetroFramework.Controls.MetroLabel();
			this.browseLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordLabel = new MetroFramework.Controls.MetroLabel();
			this.browseButton = new MetroFramework.Controls.MetroButton();
			this.pgpField = new MetroFramework.Controls.MetroTextBox();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.SuspendLayout();
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(261, 310);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "strings.OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(342, 310);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "strings.Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// pgpLabel
			// 
			this.pgpLabel.AutoSize = true;
			this.pgpLabel.Location = new System.Drawing.Point(23, 60);
			this.pgpLabel.Name = "pgpLabel";
			this.pgpLabel.Size = new System.Drawing.Size(174, 19);
			this.pgpLabel.TabIndex = 4;
			this.pgpLabel.Text = "Enter or select your PGP key";
			// 
			// browseLabel
			// 
			this.browseLabel.AutoSize = true;
			this.browseLabel.Location = new System.Drawing.Point(23, 197);
			this.browseLabel.Name = "browseLabel";
			this.browseLabel.Size = new System.Drawing.Size(136, 19);
			this.browseLabel.TabIndex = 4;
			this.browseLabel.Text = "Or, select your key file";
			// 
			// passwordLabel
			// 
			this.passwordLabel.AutoSize = true;
			this.passwordLabel.Location = new System.Drawing.Point(23, 239);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(161, 19);
			this.passwordLabel.TabIndex = 4;
			this.passwordLabel.Text = "If you key has a password ";
			// 
			// browseButton
			// 
			this.browseButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
			this.browseButton.Location = new System.Drawing.Point(165, 197);
			this.browseButton.Name = "browseButton";
			this.browseButton.Size = new System.Drawing.Size(75, 23);
			this.browseButton.TabIndex = 2;
			this.browseButton.Text = "strings.Browse";
			this.browseButton.UseSelectable = true;
			this.browseButton.Click += new System.EventHandler(this.browseButton_Click);
			// 
			// pgpField
			// 
			this.pgpField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.pgpField.Location = new System.Drawing.Point(23, 82);
			this.pgpField.MaxLength = 32767;
			this.pgpField.Multiline = true;
			this.pgpField.Name = "pgpField";
			this.pgpField.PasswordChar = '\0';
			this.pgpField.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
			this.pgpField.SelectedText = "";
			this.pgpField.Size = new System.Drawing.Size(394, 103);
			this.pgpField.TabIndex = 1;
			this.pgpField.UseSelectable = true;
			this.pgpField.UseSystemPasswordChar = true;
			// 
			// passwordField
			// 
			this.passwordField.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.passwordField.Location = new System.Drawing.Point(23, 261);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.PromptText = "Optional password";
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(394, 23);
			this.passwordField.TabIndex = 1;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			// 
			// GetPGPKeyForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(448, 356);
			this.Controls.Add(this.pgpLabel);
			this.Controls.Add(this.browseLabel);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.browseButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.pgpField);
			this.Controls.Add(this.passwordField);
			this.MaximizeBox = false;
			this.MinimizeBox = false;
			this.Name = "GetPGPKeyForm";
			this.Resizable = false;
			this.Text = "_GetPGPKeyForm_";
			this.Load += new System.EventHandler(this.GetPGPKeyForm_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroTextBox pgpField;
		private MetroFramework.Controls.MetroButton browseButton;
		private MetroFramework.Controls.MetroLabel passwordLabel;
		private MetroFramework.Controls.MetroLabel browseLabel;
		private MetroFramework.Controls.MetroLabel pgpLabel;
	}
}