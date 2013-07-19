namespace WinAuth
{
	partial class SetPasswordForm
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
			this.setPasswordLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.verifyField = new MetroFramework.Controls.MetroTextBox();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.showCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.errorLabel = new MetroFramework.Controls.MetroLabel();
			this.errorTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// setPasswordLabel
			// 
			this.setPasswordLabel.Location = new System.Drawing.Point(23, 60);
			this.setPasswordLabel.Name = "setPasswordLabel";
			this.setPasswordLabel.Size = new System.Drawing.Size(275, 42);
			this.setPasswordLabel.TabIndex = 0;
			this.setPasswordLabel.Text = "strings.setPasswordLabel";
			// 
			// passwordField
			// 
			this.passwordField.Location = new System.Drawing.Point(23, 105);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.PromptText = "strings.passwordField";
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(277, 23);
			this.passwordField.TabIndex = 0;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			// 
			// verifyField
			// 
			this.verifyField.Location = new System.Drawing.Point(23, 134);
			this.verifyField.MaxLength = 32767;
			this.verifyField.Name = "verifyField";
			this.verifyField.PasswordChar = '●';
			this.verifyField.PromptText = "strings.verifyField";
			this.verifyField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.verifyField.SelectedText = "";
			this.verifyField.Size = new System.Drawing.Size(277, 23);
			this.verifyField.TabIndex = 1;
			this.verifyField.UseSelectable = true;
			this.verifyField.UseSystemPasswordChar = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(225, 195);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 4;
			this.cancelButton.Text = "strings.Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(144, 195);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 3;
			this.okButton.Text = "strings.OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// showCheckbox
			// 
			this.showCheckbox.AutoSize = true;
			this.showCheckbox.Location = new System.Drawing.Point(23, 163);
			this.showCheckbox.Name = "showCheckbox";
			this.showCheckbox.Size = new System.Drawing.Size(141, 15);
			this.showCheckbox.TabIndex = 2;
			this.showCheckbox.Text = "strings.showCheckbox";
			this.showCheckbox.UseSelectable = true;
			this.showCheckbox.CheckedChanged += new System.EventHandler(this.showCheckbox_CheckedChanged);
			// 
			// errorLabel
			// 
			this.errorLabel.ForeColor = System.Drawing.Color.Red;
			this.errorLabel.Location = new System.Drawing.Point(85, 163);
			this.errorLabel.Name = "errorLabel";
			this.errorLabel.Size = new System.Drawing.Size(215, 23);
			this.errorLabel.TabIndex = 5;
			this.errorLabel.Text = "strings.errorLabel";
			this.errorLabel.TextAlign = System.Drawing.ContentAlignment.TopRight;
			this.errorLabel.UseCustomForeColor = true;
			this.errorLabel.Visible = false;
			// 
			// errorTimer
			// 
			this.errorTimer.Interval = 2000;
			this.errorTimer.Tick += new System.EventHandler(this.errorTimer_Tick);
			// 
			// SetPasswordForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(331, 241);
			this.Controls.Add(this.errorLabel);
			this.Controls.Add(this.showCheckbox);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.verifyField);
			this.Controls.Add(this.passwordField);
			this.Controls.Add(this.setPasswordLabel);
			this.Name = "SetPasswordForm";
			this.Resizable = false;
			this.Text = "SetPasswordForm";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel setPasswordLabel;
		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroTextBox verifyField;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroCheckBox showCheckbox;
		private MetroFramework.Controls.MetroLabel errorLabel;
		private System.Windows.Forms.Timer errorTimer;
	}
}