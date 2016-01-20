namespace WinAuth
{
	partial class AddMicrosoftAuthenticator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddMicrosoftAuthenticator));
			this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
			this.allowCopyButton = new MetroFramework.Controls.MetroCheckBox();
			this.codeField = new WinAuth.SecretTextBox();
			this.verifyAuthenticatorButton = new MetroFramework.Controls.MetroButton();
			this.secretCodeField = new MetroFramework.Controls.MetroTextBox();
			this.label3 = new MetroFramework.Controls.MetroLabel();
			this.step9Label = new MetroFramework.Controls.MetroLabel();
			this.label2 = new MetroFramework.Controls.MetroLabel();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.icon3RadioButton = new WinAuth.GroupRadioButton();
			this.icon2RadioButton = new WinAuth.GroupRadioButton();
			this.icon1RadioButton = new WinAuth.GroupRadioButton();
			this.icon3 = new System.Windows.Forms.PictureBox();
			this.icon2 = new System.Windows.Forms.PictureBox();
			this.icon1 = new System.Windows.Forms.PictureBox();
			this.label10 = new MetroFramework.Controls.MetroLabel();
			this.label12 = new MetroFramework.Controls.MetroLabel();
			this.nameField = new MetroFramework.Controls.MetroTextBox();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.step8Label = new MetroFramework.Controls.MetroLabel();
			((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorProgress
			// 
			this.newAuthenticatorProgress.Location = new System.Drawing.Point(98, 493);
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
			this.allowCopyButton.Location = new System.Drawing.Point(262, 472);
			this.allowCopyButton.Name = "allowCopyButton";
			this.allowCopyButton.Size = new System.Drawing.Size(87, 15);
			this.allowCopyButton.TabIndex = 6;
			this.allowCopyButton.Text = "Allow copy?";
			this.allowCopyButton.UseSelectable = true;
			this.allowCopyButton.CheckedChanged += new System.EventHandler(this.allowCopyButton_CheckedChanged);
			// 
			// codeField
			// 
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.codeField.Location = new System.Drawing.Point(98, 465);
			this.codeField.Multiline = true;
			this.codeField.Name = "codeField";
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(158, 26);
			this.codeField.SpaceOut = 3;
			this.codeField.TabIndex = 8;
			// 
			// verifyAuthenticatorButton
			// 
			this.verifyAuthenticatorButton.Location = new System.Drawing.Point(98, 399);
			this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
			this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
			this.verifyAuthenticatorButton.TabIndex = 5;
			this.verifyAuthenticatorButton.Text = "Verify Authenticator";
			this.verifyAuthenticatorButton.UseSelectable = true;
			this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Location = new System.Drawing.Point(23, 361);
			this.secretCodeField.MaxLength = 32767;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.PasswordChar = '\0';
			this.secretCodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.secretCodeField.SelectedText = "";
			this.secretCodeField.Size = new System.Drawing.Size(425, 22);
			this.secretCodeField.TabIndex = 4;
			this.secretCodeField.UseSelectable = true;
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(23, 510);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(425, 56);
			this.label3.TabIndex = 1;
			this.label3.Text = "10. IMPORTANT: Write down you Secret Code and store it somewhere safe and secure." +
    " You will need it if you ever need to restore your authenticator.";
			// 
			// step9Label
			// 
			this.step9Label.Location = new System.Drawing.Point(23, 436);
			this.step9Label.Name = "step9Label";
			this.step9Label.Size = new System.Drawing.Size(425, 26);
			this.step9Label.TabIndex = 1;
			this.step9Label.Text = "9. Enter the follow code to verify it is working";
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(23, 177);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(425, 181);
			this.label2.TabIndex = 1;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(293, 584);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 7;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(374, 584);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 8;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// icon3RadioButton
			// 
			this.icon3RadioButton.AutoSize = true;
			this.icon3RadioButton.Group = "ICON";
			this.icon3RadioButton.Location = new System.Drawing.Point(227, 122);
			this.icon3RadioButton.Name = "icon3RadioButton";
			this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon3RadioButton.TabIndex = 3;
			this.icon3RadioButton.Tag = "Windows7Icon.png";
			this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon2RadioButton
			// 
			this.icon2RadioButton.AutoSize = true;
			this.icon2RadioButton.Group = "ICON";
			this.icon2RadioButton.Location = new System.Drawing.Point(154, 122);
			this.icon2RadioButton.Name = "icon2RadioButton";
			this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon2RadioButton.TabIndex = 2;
			this.icon2RadioButton.Tag = "Windows8Icon.png";
			this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon1RadioButton
			// 
			this.icon1RadioButton.AutoSize = true;
			this.icon1RadioButton.Checked = true;
			this.icon1RadioButton.Group = "ICON";
			this.icon1RadioButton.Location = new System.Drawing.Point(78, 122);
			this.icon1RadioButton.Name = "icon1RadioButton";
			this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon1RadioButton.TabIndex = 1;
			this.icon1RadioButton.TabStop = true;
			this.icon1RadioButton.Tag = "MicrosoftAuthenticatorIcon.png";
			this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon3
			// 
			this.icon3.Image = global::WinAuth.Properties.Resources.Windows7Icon;
			this.icon3.Location = new System.Drawing.Point(247, 107);
			this.icon3.Name = "icon3";
			this.icon3.Size = new System.Drawing.Size(48, 48);
			this.icon3.TabIndex = 4;
			this.icon3.TabStop = false;
			this.icon3.Tag = "";
			this.icon3.Click += new System.EventHandler(this.icon3_Click);
			// 
			// icon2
			// 
			this.icon2.Image = global::WinAuth.Properties.Resources.Windows8Icon;
			this.icon2.Location = new System.Drawing.Point(174, 107);
			this.icon2.Name = "icon2";
			this.icon2.Size = new System.Drawing.Size(48, 48);
			this.icon2.TabIndex = 4;
			this.icon2.TabStop = false;
			this.icon2.Tag = "";
			this.icon2.Click += new System.EventHandler(this.icon2_Click);
			// 
			// icon1
			// 
			this.icon1.Image = global::WinAuth.Properties.Resources.MicrosoftAuthenticatorIcon;
			this.icon1.Location = new System.Drawing.Point(98, 107);
			this.icon1.Name = "icon1";
			this.icon1.Size = new System.Drawing.Size(48, 48);
			this.icon1.TabIndex = 4;
			this.icon1.TabStop = false;
			this.icon1.Tag = "";
			this.icon1.Click += new System.EventHandler(this.icon1_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Location = new System.Drawing.Point(23, 118);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(36, 19);
			this.label10.TabIndex = 3;
			this.label10.Text = "Icon:";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(23, 70);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(48, 19);
			this.label12.TabIndex = 3;
			this.label12.Text = "Name:";
			// 
			// nameField
			// 
			this.nameField.Location = new System.Drawing.Point(77, 70);
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
			// step8Label
			// 
			this.step8Label.Location = new System.Drawing.Point(23, 400);
			this.step8Label.Name = "step8Label";
			this.step8Label.Size = new System.Drawing.Size(51, 25);
			this.step8Label.TabIndex = 11;
			this.step8Label.Text = "8. Click";
			// 
			// AddMicrosoftAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(472, 630);
			this.Controls.Add(this.step8Label);
			this.Controls.Add(this.icon3RadioButton);
			this.Controls.Add(this.newAuthenticatorProgress);
			this.Controls.Add(this.icon2RadioButton);
			this.Controls.Add(this.icon1RadioButton);
			this.Controls.Add(this.allowCopyButton);
			this.Controls.Add(this.icon3);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.icon2);
			this.Controls.Add(this.codeField);
			this.Controls.Add(this.icon1);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.label10);
			this.Controls.Add(this.label12);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.verifyAuthenticatorButton);
			this.Controls.Add(this.label2);
			this.Controls.Add(this.secretCodeField);
			this.Controls.Add(this.step9Label);
			this.Controls.Add(this.label3);
			this.MaximizeBox = false;
			this.Name = "AddMicrosoftAuthenticator";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Microsoft Authenticator";
			this.Load += new System.EventHandler(this.AddMicrosoftAuthenticator_Load);
			((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel label2;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroTextBox secretCodeField;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private MetroFramework.Controls.MetroLabel label10;
		private MetroFramework.Controls.MetroLabel label12;
		private MetroFramework.Controls.MetroTextBox nameField;
		private MetroFramework.Controls.MetroButton verifyAuthenticatorButton;
		private MetroFramework.Controls.MetroLabel step9Label;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private MetroFramework.Controls.MetroCheckBox allowCopyButton;
		private SecretTextBox codeField;
		private MetroFramework.Controls.MetroLabel label3;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private System.Windows.Forms.PictureBox icon3;
		private GroupRadioButton icon2RadioButton;
		private GroupRadioButton icon1RadioButton;
		private GroupRadioButton icon3RadioButton;
		private MetroFramework.Controls.MetroLabel step8Label;
	}
}