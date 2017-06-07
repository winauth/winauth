namespace WinAuth
{
    partial class AddOktaVerifyAuthenticator
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddOktaVerifyAuthenticator));
            this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
            this.codeField = new WinAuth.SecretTextBox();
            this.verifyAuthenticatorButton = new MetroFramework.Controls.MetroButton();
            this.secretCodeField = new MetroFramework.Controls.MetroTextBox();
            this.setupLabel = new MetroFramework.Controls.MetroLabel();
            this.okButton = new MetroFramework.Controls.MetroButton();
            this.cancelButton = new MetroFramework.Controls.MetroButton();
            this.iconRadioButton = new WinAuth.GroupRadioButton();
            this.oktaIcon = new System.Windows.Forms.PictureBox();
            this.iconLabel = new MetroFramework.Controls.MetroLabel();
            this.nameLabel = new MetroFramework.Controls.MetroLabel();
            this.nameField = new MetroFramework.Controls.MetroTextBox();
            this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
            this.step8Label = new MetroFramework.Controls.MetroLabel();
            this.step9Label = new MetroFramework.Controls.MetroLabel();
            this.linkLabel1 = new System.Windows.Forms.LinkLabel();
            this.label1 = new System.Windows.Forms.Label();
            this.helpLabel = new MetroFramework.Controls.MetroLabel();
            this.helpLink = new MetroFramework.Controls.MetroLink();
            ((System.ComponentModel.ISupportInitialize)(this.oktaIcon)).BeginInit();
            this.SuspendLayout();
            // 
            // newAuthenticatorProgress
            // 
            this.newAuthenticatorProgress.Location = new System.Drawing.Point(26, 436);
            this.newAuthenticatorProgress.Maximum = 30;
            this.newAuthenticatorProgress.Minimum = 1;
            this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
            this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
            this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
            this.newAuthenticatorProgress.TabIndex = 0;
            this.newAuthenticatorProgress.Value = 1;
            this.newAuthenticatorProgress.Visible = false;
            // 
            // codeField
            // 
            this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
            this.codeField.Location = new System.Drawing.Point(26, 408);
            this.codeField.Multiline = true;
            this.codeField.Name = "codeField";
            this.codeField.SecretMode = false;
            this.codeField.Size = new System.Drawing.Size(158, 26);
            this.codeField.SpaceOut = 3;
            this.codeField.TabIndex = 0;
            this.codeField.TabStop = false;
            // 
            // verifyAuthenticatorButton
            // 
            this.verifyAuthenticatorButton.Location = new System.Drawing.Point(289, 351);
            this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
            this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
            this.verifyAuthenticatorButton.TabIndex = 2;
            this.verifyAuthenticatorButton.Text = "Verify Authenticator";
            this.verifyAuthenticatorButton.UseSelectable = true;
            this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
            // 
            // secretCodeField
            // 
            this.secretCodeField.AllowDrop = true;
            this.secretCodeField.CausesValidation = false;
            this.secretCodeField.Location = new System.Drawing.Point(26, 317);
            this.secretCodeField.MaxLength = 32767;
            this.secretCodeField.Name = "secretCodeField";
            this.secretCodeField.PasswordChar = '\0';
            this.secretCodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.secretCodeField.SelectedText = "";
            this.secretCodeField.Size = new System.Drawing.Size(423, 22);
            this.secretCodeField.TabIndex = 1;
            this.secretCodeField.UseSelectable = true;
            // 
            // setupLabel
            // 
            this.setupLabel.BackColor = System.Drawing.Color.Transparent;
            this.setupLabel.Location = new System.Drawing.Point(23, 177);
            this.setupLabel.Name = "setupLabel";
            this.setupLabel.Size = new System.Drawing.Size(444, 134);
            this.setupLabel.TabIndex = 0;
            this.setupLabel.Text = resources.GetString("setupLabel.Text");
            // 
            // okButton
            // 
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(293, 528);
            this.okButton.Name = "okButton";
            this.okButton.Size = new System.Drawing.Size(75, 23);
            this.okButton.TabIndex = 3;
            this.okButton.Text = "OK";
            this.okButton.UseSelectable = true;
            this.okButton.Click += new System.EventHandler(this.okButton_Click);
            // 
            // cancelButton
            // 
            this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.cancelButton.Location = new System.Drawing.Point(374, 528);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 4;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseSelectable = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            // 
            // iconRadioButton
            // 
            this.iconRadioButton.AutoSize = true;
            this.iconRadioButton.Checked = true;
            this.iconRadioButton.Group = "ICON";
            this.iconRadioButton.Location = new System.Drawing.Point(78, 122);
            this.iconRadioButton.Name = "iconRadioButton";
            this.iconRadioButton.Size = new System.Drawing.Size(14, 13);
            this.iconRadioButton.TabIndex = 1;
            this.iconRadioButton.TabStop = true;
            this.iconRadioButton.Tag = "OktaVerifyAuthenticatorIcon.png";
            this.iconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
            // 
            // oktaIcon
            // 
            this.oktaIcon.Image = ((System.Drawing.Image)(resources.GetObject("oktaIcon.Image")));
            this.oktaIcon.Location = new System.Drawing.Point(98, 110);
            this.oktaIcon.Name = "oktaIcon";
            this.oktaIcon.Size = new System.Drawing.Size(35, 35);
            this.oktaIcon.TabIndex = 4;
            this.oktaIcon.TabStop = false;
            this.oktaIcon.Tag = "";
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
            this.step8Label.BackColor = System.Drawing.Color.Transparent;
            this.step8Label.Location = new System.Drawing.Point(23, 383);
            this.step8Label.Name = "step8Label";
            this.step8Label.Size = new System.Drawing.Size(434, 22);
            this.step8Label.TabIndex = 0;
            this.step8Label.Text = "8. Click \"Next\" in the setup flow and enter the code provided here:";
            // 
            // step9Label
            // 
            this.step9Label.BackColor = System.Drawing.Color.Transparent;
            this.step9Label.Location = new System.Drawing.Point(23, 454);
            this.step9Label.Name = "step9Label";
            this.step9Label.Size = new System.Drawing.Size(434, 35);
            this.step9Label.TabIndex = 0;
            this.step9Label.Text = "9. Click \"Verify\" to finish.";
            // 
            // linkLabel1
            // 
            this.linkLabel1.AutoSize = true;
            this.linkLabel1.Location = new System.Drawing.Point(130, 526);
            this.linkLabel1.Name = "linkLabel1";
            this.linkLabel1.Size = new System.Drawing.Size(0, 13);
            this.linkLabel1.TabIndex = 13;
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 526);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(0, 13);
            this.label1.TabIndex = 14;
            // 
            // helpLabel
            // 
            this.helpLabel.AutoSize = true;
            this.helpLabel.BackColor = System.Drawing.Color.Transparent;
            this.helpLabel.FontSize = MetroFramework.MetroLabelSize.Small;
            this.helpLabel.Location = new System.Drawing.Point(23, 495);
            this.helpLabel.Name = "helpLabel";
            this.helpLabel.Size = new System.Drawing.Size(116, 15);
            this.helpLabel.TabIndex = 0;
            this.helpLabel.Text = "Visit Okta Support for";
            // 
            // helpLink
            // 
            this.helpLink.BackColor = System.Drawing.Color.Transparent;
            this.helpLink.ForeColor = System.Drawing.SystemColors.Highlight;
            this.helpLink.Location = new System.Drawing.Point(134, 491);
            this.helpLink.Name = "helpLink";
            this.helpLink.Size = new System.Drawing.Size(122, 23);
            this.helpLink.TabIndex = 5;
            this.helpLink.Text = "detailed instructions";
            this.helpLink.UseCustomForeColor = true;
            this.helpLink.UseSelectable = true;
            this.helpLink.Click += new System.EventHandler(this.helpLink_Click);
            // 
            // AddOktaVerifyAuthenticator
            // 
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(472, 574);
            this.Controls.Add(this.helpLabel);
            this.Controls.Add(this.helpLink);
            this.Controls.Add(this.linkLabel1);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.step9Label);
            this.Controls.Add(this.step8Label);
            this.Controls.Add(this.newAuthenticatorProgress);
            this.Controls.Add(this.iconRadioButton);
            this.Controls.Add(this.cancelButton);
            this.Controls.Add(this.codeField);
            this.Controls.Add(this.oktaIcon);
            this.Controls.Add(this.okButton);
            this.Controls.Add(this.iconLabel);
            this.Controls.Add(this.nameLabel);
            this.Controls.Add(this.nameField);
            this.Controls.Add(this.verifyAuthenticatorButton);
            this.Controls.Add(this.setupLabel);
            this.Controls.Add(this.secretCodeField);
            this.MaximizeBox = false;
            this.Name = "AddOktaVerifyAuthenticator";
            this.Resizable = false;
            this.ShowIcon = false;
            this.Text = "Okta Verify Authenticator";
            this.Load += new System.EventHandler(this.AddOktaVerifyAuthenticator_Load);
            ((System.ComponentModel.ISupportInitialize)(this.oktaIcon)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private MetroFramework.Controls.MetroLabel setupLabel;
        private MetroFramework.Controls.MetroButton okButton;
        private MetroFramework.Controls.MetroButton cancelButton;
        private MetroFramework.Controls.MetroTextBox secretCodeField;
        private System.Windows.Forms.PictureBox oktaIcon;
        private MetroFramework.Controls.MetroLabel iconLabel;
        private MetroFramework.Controls.MetroLabel nameLabel;
        private MetroFramework.Controls.MetroTextBox nameField;
        private MetroFramework.Controls.MetroButton verifyAuthenticatorButton;
        private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
        private SecretTextBox codeField;
        private System.Windows.Forms.Timer newAuthenticatorTimer;
        private GroupRadioButton iconRadioButton;
        private MetroFramework.Controls.MetroLabel step8Label;
        private MetroFramework.Controls.MetroLabel step9Label;
        private System.Windows.Forms.LinkLabel linkLabel1;
        private System.Windows.Forms.Label label1;
        private MetroFramework.Controls.MetroLabel helpLabel;
        private MetroFramework.Controls.MetroLink helpLink;
    }
}