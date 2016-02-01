namespace WinAuth
{
	partial class AddGuildWarsAuthenticator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddGuildWarsAuthenticator));
			this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
			this.allowCopyButton = new MetroFramework.Controls.MetroCheckBox();
			this.codeField = new WinAuth.SecretTextBox();
			this.verifyAuthenticatorButton = new MetroFramework.Controls.MetroButton();
			this.secretCodeField = new MetroFramework.Controls.MetroTextBox();
			this.step8Label = new MetroFramework.Controls.MetroLabel();
			this.step7Label = new MetroFramework.Controls.MetroLabel();
			this.step1Label = new MetroFramework.Controls.MetroLabel();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.icon2RadioButton = new WinAuth.GroupMetroRadioButton();
			this.icon1RadioButton = new WinAuth.GroupMetroRadioButton();
			this.icon2 = new System.Windows.Forms.PictureBox();
			this.icon1 = new System.Windows.Forms.PictureBox();
			this.iconLabel = new MetroFramework.Controls.MetroLabel();
			this.nameLabel = new MetroFramework.Controls.MetroLabel();
			this.nameField = new MetroFramework.Controls.MetroTextBox();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.step6Label = new MetroFramework.Controls.MetroLabel();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorProgress
			// 
			this.newAuthenticatorProgress.Location = new System.Drawing.Point(83, 423);
			this.newAuthenticatorProgress.Maximum = 30;
			this.newAuthenticatorProgress.Minimum = 1;
			this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
			this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
			this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.newAuthenticatorProgress.TabIndex = 9;
			this.newAuthenticatorProgress.Value = 1;
			this.newAuthenticatorProgress.Visible = false;
			// 
			// allowCopyButton
			// 
			this.allowCopyButton.AutoSize = true;
			this.allowCopyButton.Location = new System.Drawing.Point(247, 402);
			this.allowCopyButton.Name = "allowCopyButton";
			this.allowCopyButton.Size = new System.Drawing.Size(87, 15);
			this.allowCopyButton.TabIndex = 5;
			this.allowCopyButton.Text = "Allow copy?";
			this.allowCopyButton.UseSelectable = true;
			this.allowCopyButton.CheckedChanged += new System.EventHandler(this.allowCopyButton_CheckedChanged);
			// 
			// codeField
			// 
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.codeField.Location = new System.Drawing.Point(83, 395);
			this.codeField.Multiline = true;
			this.codeField.Name = "codeField";
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(158, 26);
			this.codeField.SpaceOut = 3;
			this.codeField.TabIndex = 8;
			// 
			// verifyAuthenticatorButton
			// 
			this.verifyAuthenticatorButton.Location = new System.Drawing.Point(83, 332);
			this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
			this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
			this.verifyAuthenticatorButton.TabIndex = 4;
			this.verifyAuthenticatorButton.Text = "Verify Authenticator";
			this.verifyAuthenticatorButton.UseSelectable = true;
			this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Location = new System.Drawing.Point(23, 297);
			this.secretCodeField.MaxLength = 32767;
			this.secretCodeField.Multiline = true;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.PasswordChar = '\0';
			this.secretCodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.secretCodeField.SelectedText = "";
			this.secretCodeField.Size = new System.Drawing.Size(428, 22);
			this.secretCodeField.TabIndex = 3;
			this.secretCodeField.UseSelectable = true;
			// 
			// step8Label
			// 
			this.step8Label.Location = new System.Drawing.Point(23, 452);
			this.step8Label.Name = "step8Label";
			this.step8Label.Size = new System.Drawing.Size(428, 56);
			this.step8Label.TabIndex = 1;
			this.step8Label.Text = "8. IMPORTANT: Write down your Secret Code and store it somewhere safe and secure. " +
    "You will need it if you ever need to restore your authenticator.";
			// 
			// step7Label
			// 
			this.step7Label.Location = new System.Drawing.Point(23, 366);
			this.step7Label.Name = "step7Label";
			this.step7Label.Size = new System.Drawing.Size(428, 26);
			this.step7Label.TabIndex = 1;
			this.step7Label.Text = "7. Enter the following code to verify it is working";
			// 
			// step1Label
			// 
			this.step1Label.Location = new System.Drawing.Point(23, 168);
			this.step1Label.Name = "step1Label";
			this.step1Label.Size = new System.Drawing.Size(428, 121);
			this.step1Label.TabIndex = 1;
			this.step1Label.Text = resources.GetString("step1Label.Text");
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(299, 527);
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
			this.cancelButton.Location = new System.Drawing.Point(380, 527);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// icon2RadioButton
			// 
			this.icon2RadioButton.Group = "ICON";
			this.icon2RadioButton.Location = new System.Drawing.Point(159, 122);
			this.icon2RadioButton.Name = "icon2RadioButton";
			this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon2RadioButton.TabIndex = 2;
			this.icon2RadioButton.Tag = "ArenaNetIcon.png";
			this.icon2RadioButton.UseSelectable = true;
			this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon1RadioButton
			// 
			this.icon1RadioButton.Checked = true;
			this.icon1RadioButton.Group = "ICON";
			this.icon1RadioButton.Location = new System.Drawing.Point(83, 122);
			this.icon1RadioButton.Name = "icon1RadioButton";
			this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon1RadioButton.TabIndex = 1;
			this.icon1RadioButton.TabStop = true;
			this.icon1RadioButton.Tag = "GuildWarsAuthenticatorIcon.png";
			this.icon1RadioButton.UseSelectable = true;
			this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon2
			// 
			this.icon2.Image = global::WinAuth.Properties.Resources.ArenaNetIcon;
			this.icon2.Location = new System.Drawing.Point(179, 105);
			this.icon2.Name = "icon2";
			this.icon2.Size = new System.Drawing.Size(48, 48);
			this.icon2.TabIndex = 4;
			this.icon2.TabStop = false;
			this.icon2.Tag = "";
			this.icon2.Click += new System.EventHandler(this.icon2_Click);
			// 
			// icon1
			// 
			this.icon1.Image = global::WinAuth.Properties.Resources.GuildWarsAuthenticatorIcon;
			this.icon1.Location = new System.Drawing.Point(103, 105);
			this.icon1.Name = "icon1";
			this.icon1.Size = new System.Drawing.Size(48, 48);
			this.icon1.TabIndex = 4;
			this.icon1.TabStop = false;
			this.icon1.Tag = "";
			this.icon1.Click += new System.EventHandler(this.icon1_Click);
			// 
			// iconLabel
			// 
			this.iconLabel.AutoSize = true;
			this.iconLabel.Location = new System.Drawing.Point(23, 118);
			this.iconLabel.Name = "iconLabel";
			this.iconLabel.Size = new System.Drawing.Size(36, 19);
			this.iconLabel.TabIndex = 3;
			this.iconLabel.Text = "Icon:";
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
			this.nameField.Location = new System.Drawing.Point(80, 69);
			this.nameField.MaxLength = 32767;
			this.nameField.Name = "nameField";
			this.nameField.PasswordChar = '\0';
			this.nameField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.nameField.SelectedText = "";
			this.nameField.Size = new System.Drawing.Size(371, 22);
			this.nameField.TabIndex = 0;
			this.nameField.UseSelectable = true;
			// 
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// step6Label
			// 
			this.step6Label.Location = new System.Drawing.Point(23, 334);
			this.step6Label.Name = "step6Label";
			this.step6Label.Size = new System.Drawing.Size(51, 25);
			this.step6Label.TabIndex = 11;
			this.step6Label.Text = "6. Click";
			// 
			// AddGuildWarsAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(478, 573);
			this.Controls.Add(this.step6Label);
			this.Controls.Add(this.icon2RadioButton);
			this.Controls.Add(this.newAuthenticatorProgress);
			this.Controls.Add(this.icon1RadioButton);
			this.Controls.Add(this.icon2);
			this.Controls.Add(this.allowCopyButton);
			this.Controls.Add(this.icon1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.iconLabel);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.verifyAuthenticatorButton);
			this.Controls.Add(this.step1Label);
			this.Controls.Add(this.secretCodeField);
			this.Controls.Add(this.step7Label);
			this.Controls.Add(this.step8Label);
			this.MaximizeBox = false;
			this.Name = "AddGuildWarsAuthenticator";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Guild Wars 2 Authenticator";
			this.Load += new System.EventHandler(this.AddGuildWarsAuthenticator_Load);
			((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel step1Label;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroTextBox secretCodeField;
		private GroupMetroRadioButton icon2RadioButton;
		private GroupMetroRadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private MetroFramework.Controls.MetroLabel iconLabel;
		private MetroFramework.Controls.MetroLabel nameLabel;
		private MetroFramework.Controls.MetroTextBox nameField;
		private MetroFramework.Controls.MetroButton verifyAuthenticatorButton;
		private MetroFramework.Controls.MetroLabel step7Label;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private MetroFramework.Controls.MetroCheckBox allowCopyButton;
		private SecretTextBox codeField;
		private MetroFramework.Controls.MetroLabel step8Label;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private MetroFramework.Controls.MetroLabel step6Label;
	}
}