namespace WinAuth
{
	partial class UnprotectPasswordForm
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
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.invalidPasswordLabel = new MetroFramework.Controls.MetroLabel();
			this.invalidPasswordTimer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// passwordField
			// 
			this.passwordField.Location = new System.Drawing.Point(23, 63);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(277, 23);
			this.passwordField.TabIndex = 1;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(225, 116);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 2;
			this.cancelButton.Text = "strings.Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(144, 116);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "strings.OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// invalidPasswordLabel
			// 
			this.invalidPasswordLabel.AutoSize = true;
			this.invalidPasswordLabel.ForeColor = System.Drawing.Color.Red;
			this.invalidPasswordLabel.Location = new System.Drawing.Point(23, 89);
			this.invalidPasswordLabel.Name = "invalidPasswordLabel";
			this.invalidPasswordLabel.Size = new System.Drawing.Size(140, 19);
			this.invalidPasswordLabel.TabIndex = 3;
			this.invalidPasswordLabel.Text = "strings.InvalidPassword";
			this.invalidPasswordLabel.UseCustomForeColor = true;
			this.invalidPasswordLabel.Visible = false;
			// 
			// invalidPasswordTimer
			// 
			this.invalidPasswordTimer.Interval = 2000;
			this.invalidPasswordTimer.Tick += new System.EventHandler(this.invalidPasswordTimer_Tick);
			// 
			// UnprotectPasswordForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(331, 162);
			this.Controls.Add(this.invalidPasswordLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.passwordField);
			this.Name = "UnprotectPasswordForm";
			this.Resizable = false;
			this.Text = "_UnprotectPasswordForm_";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroLabel invalidPasswordLabel;
		private System.Windows.Forms.Timer invalidPasswordTimer;
	}
}