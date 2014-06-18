namespace WinAuth
{
	partial class AddAuthenticator
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
			this.secretCodeField = new MetroFramework.Controls.MetroTextBox();
			this.step1Label = new MetroFramework.Controls.MetroLabel();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.nameLabel = new MetroFramework.Controls.MetroLabel();
			this.nameField = new MetroFramework.Controls.MetroTextBox();
			this.step2Label = new MetroFramework.Controls.MetroLabel();
			this.verifyButton = new MetroFramework.Controls.MetroButton();
			this.codeProgress = new System.Windows.Forms.ProgressBar();
			this.codeField = new WinAuth.SecretTextBox();
			this.step3Label = new MetroFramework.Controls.MetroLabel();
			this.timer = new System.Windows.Forms.Timer(this.components);
			this.SuspendLayout();
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Location = new System.Drawing.Point(25, 171);
			this.secretCodeField.MaxLength = 32767;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.PasswordChar = '\0';
			this.secretCodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.secretCodeField.SelectedText = "";
			this.secretCodeField.Size = new System.Drawing.Size(423, 22);
			this.secretCodeField.TabIndex = 4;
			this.secretCodeField.UseSelectable = true;
			// 
			// step1Label
			// 
			this.step1Label.Location = new System.Drawing.Point(23, 119);
			this.step1Label.Name = "step1Label";
			this.step1Label.Size = new System.Drawing.Size(425, 48);
			this.step1Label.TabIndex = 1;
			this.step1Label.Text = "1. Enter the Secret Code for your authenticator. Spaces don\'t matter. If have a Q" +
    "R code, you can paste the URL of the image instead.\r\n";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(292, 347);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(373, 347);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// nameLabel
			// 
			this.nameLabel.AutoSize = true;
			this.nameLabel.Location = new System.Drawing.Point(23, 70);
			this.nameLabel.Name = "nameLabel";
			this.nameLabel.Size = new System.Drawing.Size(48, 19);
			this.nameLabel.TabIndex = 3;
			this.nameLabel.Text = "Name:";
			// 
			// nameField
			// 
			this.nameField.Location = new System.Drawing.Point(77, 67);
			this.nameField.MaxLength = 32767;
			this.nameField.Name = "nameField";
			this.nameField.PasswordChar = '\0';
			this.nameField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.nameField.SelectedText = "";
			this.nameField.Size = new System.Drawing.Size(371, 22);
			this.nameField.TabIndex = 0;
			this.nameField.UseSelectable = true;
			// 
			// step2Label
			// 
			this.step2Label.Location = new System.Drawing.Point(23, 213);
			this.step2Label.Name = "step2Label";
			this.step2Label.Size = new System.Drawing.Size(51, 25);
			this.step2Label.TabIndex = 10;
			this.step2Label.Text = "2. Click";
			// 
			// verifyButton
			// 
			this.verifyButton.Location = new System.Drawing.Point(113, 212);
			this.verifyButton.Name = "verifyButton";
			this.verifyButton.Size = new System.Drawing.Size(158, 23);
			this.verifyButton.TabIndex = 5;
			this.verifyButton.Text = "Verify Authenticator";
			this.verifyButton.UseSelectable = true;
			this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
			// 
			// codeProgress
			// 
			this.codeProgress.Location = new System.Drawing.Point(113, 310);
			this.codeProgress.Maximum = 30;
			this.codeProgress.Minimum = 1;
			this.codeProgress.Name = "codeProgress";
			this.codeProgress.Size = new System.Drawing.Size(158, 8);
			this.codeProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.codeProgress.TabIndex = 13;
			this.codeProgress.Value = 1;
			this.codeProgress.Visible = false;
			// 
			// codeField
			// 
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.codeField.Location = new System.Drawing.Point(113, 282);
			this.codeField.Multiline = true;
			this.codeField.Name = "codeField";
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(158, 26);
			this.codeField.SpaceOut = 3;
			this.codeField.TabIndex = 12;
			// 
			// step3Label
			// 
			this.step3Label.AutoSize = true;
			this.step3Label.Location = new System.Drawing.Point(23, 251);
			this.step3Label.Name = "step3Label";
			this.step3Label.Size = new System.Drawing.Size(293, 19);
			this.step3Label.TabIndex = 11;
			this.step3Label.Text = "3. Verify the following code matches your service";
			// 
			// timer
			// 
			this.timer.Enabled = true;
			this.timer.Interval = 500;
			this.timer.Tick += new System.EventHandler(this.timer_Tick);
			// 
			// AddAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(471, 393);
			this.Controls.Add(this.codeProgress);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.step3Label);
			this.Controls.Add(this.step2Label);
			this.Controls.Add(this.verifyButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.secretCodeField);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.step1Label);
			this.MaximizeBox = false;
			this.Name = "AddAuthenticator";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Add Authenticator";
			this.Load += new System.EventHandler(this.AddAuthenticator_Load);
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel step1Label;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroTextBox secretCodeField;
		private MetroFramework.Controls.MetroLabel nameLabel;
		private MetroFramework.Controls.MetroTextBox nameField;
		private MetroFramework.Controls.MetroLabel step2Label;
		private MetroFramework.Controls.MetroButton verifyButton;
		private System.Windows.Forms.ProgressBar codeProgress;
		private SecretTextBox codeField;
		private MetroFramework.Controls.MetroLabel step3Label;
		private System.Windows.Forms.Timer timer;
	}
}