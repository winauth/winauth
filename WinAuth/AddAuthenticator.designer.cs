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
            this.step5Label = new MetroFramework.Controls.MetroLabel();
            this.timer = new System.Windows.Forms.Timer(this.components);
            this.step4TimerLabel = new MetroFramework.Controls.MetroLabel();
            this.timeBasedRadio = new MetroFramework.Controls.MetroRadioButton();
            this.counterBasedRadio = new MetroFramework.Controls.MetroRadioButton();
            this.timeBasedPanel = new System.Windows.Forms.Panel();
            this.counterBasedPanel = new System.Windows.Forms.Panel();
            this.verifyCounterButton = new MetroFramework.Controls.MetroButton();
            this.counterField = new MetroFramework.Controls.MetroTextBox();
            this.step4CounterLabel = new MetroFramework.Controls.MetroLabel();
            this.secretCodeButton = new MetroFramework.Controls.MetroButton();
            this.step3Label = new MetroFramework.Controls.MetroLabel();
            this.cmbHMACType = new System.Windows.Forms.ComboBox();
            this.timeBasedPanel.SuspendLayout();
            this.counterBasedPanel.SuspendLayout();
            this.SuspendLayout();
            //
            // secretCodeField
            //
            this.secretCodeField.AllowDrop = true;
            this.secretCodeField.CausesValidation = false;
            this.secretCodeField.Location = new System.Drawing.Point(43, 171);
            this.secretCodeField.MaxLength = 32767;
            this.secretCodeField.Name = "secretCodeField";
            this.secretCodeField.PasswordChar = '\0';
            this.secretCodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.secretCodeField.SelectedText = "";
            this.secretCodeField.Size = new System.Drawing.Size(311, 22);
            this.secretCodeField.TabIndex = 1;
            this.secretCodeField.UseSelectable = true;
            //
            // step1Label
            //
            this.step1Label.Location = new System.Drawing.Point(28, 120);
            this.step1Label.Name = "step1Label";
            this.step1Label.Size = new System.Drawing.Size(425, 48);
            this.step1Label.TabIndex = 1;
            this.step1Label.Text = "1. Enter the Secret Code for your authenticator. Spaces don\'t matter. If you have" +
    " a QR code, you can paste the URL of the image instead.\r\n";
            //
            // okButton
            //
            this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.okButton.Location = new System.Drawing.Point(292, 557);
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
            this.cancelButton.Location = new System.Drawing.Point(373, 557);
            this.cancelButton.Name = "cancelButton";
            this.cancelButton.Size = new System.Drawing.Size(75, 23);
            this.cancelButton.TabIndex = 7;
            this.cancelButton.Text = "Cancel";
            this.cancelButton.UseSelectable = true;
            this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
            //
            // nameLabel
            //
            this.nameLabel.AutoSize = true;
            this.nameLabel.Location = new System.Drawing.Point(28, 70);
            this.nameLabel.Name = "nameLabel";
            this.nameLabel.Size = new System.Drawing.Size(48, 19);
            this.nameLabel.TabIndex = 3;
            this.nameLabel.Text = "Name:";
            //
            // nameField
            //
            this.nameField.Location = new System.Drawing.Point(82, 67);
            this.nameField.MaxLength = 32767;
            this.nameField.Name = "nameField";
            this.nameField.PasswordChar = '\0';
            this.nameField.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.nameField.SelectedText = "";
            this.nameField.Size = new System.Drawing.Size(366, 22);
            this.nameField.TabIndex = 0;
            this.nameField.UseSelectable = true;
            //
            // step2Label
            //
            this.step2Label.Location = new System.Drawing.Point(28, 213);
            this.step2Label.Name = "step2Label";
            this.step2Label.Size = new System.Drawing.Size(423, 49);
            this.step2Label.TabIndex = 10;
            this.step2Label.Text = "2. Choose if this is a time-based or a counter-based authenticator. If you don\'t " +
    "know, it\'s likely time-based, so just leave the default choice.";
            //
            // verifyButton
            //
            this.verifyButton.Location = new System.Drawing.Point(122, 43);
            this.verifyButton.Name = "verifyButton";
            this.verifyButton.Size = new System.Drawing.Size(158, 23);
            this.verifyButton.TabIndex = 0;
            this.verifyButton.Text = "Verify Authenticator";
            this.verifyButton.UseSelectable = true;
            this.verifyButton.Click += new System.EventHandler(this.verifyButton_Click);
            //
            // codeProgress
            //
            this.codeProgress.Location = new System.Drawing.Point(124, 524);
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
            this.codeField.Location = new System.Drawing.Point(124, 492);
            this.codeField.Multiline = true;
            this.codeField.Name = "codeField";
            this.codeField.SecretMode = false;
            this.codeField.Size = new System.Drawing.Size(158, 26);
            this.codeField.SpaceOut = 3;
            this.codeField.TabIndex = 5;
            //
            // step5Label
            //
            this.step5Label.AutoSize = true;
            this.step5Label.Location = new System.Drawing.Point(28, 465);
            this.step5Label.Name = "step5Label";
            this.step5Label.Size = new System.Drawing.Size(296, 19);
            this.step5Label.TabIndex = 11;
            this.step5Label.Text = "5. Verify the following code matches your service.";
            //
            // timer
            //
            this.timer.Enabled = true;
            this.timer.Interval = 500;
            this.timer.Tick += new System.EventHandler(this.timer_Tick);
            //
            // step4TimerLabel
            //
            this.step4TimerLabel.Location = new System.Drawing.Point(23, 12);
            this.step4TimerLabel.Name = "step4TimerLabel";
            this.step4TimerLabel.Size = new System.Drawing.Size(423, 28);
            this.step4TimerLabel.TabIndex = 10;
            this.step4TimerLabel.Text = "4. Click the Verify button to check the first code.";
            //
            // timeBasedRadio
            //
            this.timeBasedRadio.AutoSize = true;
            this.timeBasedRadio.Checked = true;
            this.timeBasedRadio.Location = new System.Drawing.Point(82, 265);
            this.timeBasedRadio.Name = "timeBasedRadio";
            this.timeBasedRadio.Size = new System.Drawing.Size(86, 15);
            this.timeBasedRadio.TabIndex = 3;
            this.timeBasedRadio.TabStop = true;
            this.timeBasedRadio.Text = "Time-based";
            this.timeBasedRadio.UseSelectable = true;
            this.timeBasedRadio.CheckedChanged += new System.EventHandler(this.timeBasedRadio_CheckedChanged);
            //
            // counterBasedRadio
            //
            this.counterBasedRadio.AutoSize = true;
            this.counterBasedRadio.Location = new System.Drawing.Point(252, 265);
            this.counterBasedRadio.Name = "counterBasedRadio";
            this.counterBasedRadio.Size = new System.Drawing.Size(102, 15);
            this.counterBasedRadio.TabIndex = 4;
            this.counterBasedRadio.Text = "Counter-based";
            this.counterBasedRadio.UseSelectable = true;
            this.counterBasedRadio.CheckedChanged += new System.EventHandler(this.counterBasedRadio_CheckedChanged);
            //
            // timeBasedPanel
            //
            this.timeBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.timeBasedPanel.Controls.Add(this.step4TimerLabel);
            this.timeBasedPanel.Controls.Add(this.verifyButton);
            this.timeBasedPanel.Location = new System.Drawing.Point(5, 367);
            this.timeBasedPanel.Name = "timeBasedPanel";
            this.timeBasedPanel.Size = new System.Drawing.Size(464, 84);
            this.timeBasedPanel.TabIndex = 15;
            //
            // counterBasedPanel
            //
            this.counterBasedPanel.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
            | System.Windows.Forms.AnchorStyles.Right)));
            this.counterBasedPanel.Controls.Add(this.verifyCounterButton);
            this.counterBasedPanel.Controls.Add(this.counterField);
            this.counterBasedPanel.Controls.Add(this.step4CounterLabel);
            this.counterBasedPanel.Location = new System.Drawing.Point(5, 367);
            this.counterBasedPanel.Name = "counterBasedPanel";
            this.counterBasedPanel.Size = new System.Drawing.Size(464, 84);
            this.counterBasedPanel.TabIndex = 16;
            this.counterBasedPanel.Visible = false;
            //
            // verifyCounterButton
            //
            this.verifyCounterButton.Location = new System.Drawing.Point(204, 56);
            this.verifyCounterButton.Name = "verifyCounterButton";
            this.verifyCounterButton.Size = new System.Drawing.Size(158, 23);
            this.verifyCounterButton.TabIndex = 1;
            this.verifyCounterButton.Text = "Verify Authenticator";
            this.verifyCounterButton.UseSelectable = true;
            this.verifyCounterButton.Click += new System.EventHandler(this.verifyButton_Click);
            //
            // counterField
            //
            this.counterField.AllowDrop = true;
            this.counterField.CausesValidation = false;
            this.counterField.Location = new System.Drawing.Point(119, 58);
            this.counterField.MaxLength = 32767;
            this.counterField.Name = "counterField";
            this.counterField.PasswordChar = '\0';
            this.counterField.ScrollBars = System.Windows.Forms.ScrollBars.None;
            this.counterField.SelectedText = "";
            this.counterField.Size = new System.Drawing.Size(79, 20);
            this.counterField.TabIndex = 0;
            this.counterField.UseSelectable = true;
            //
            // step4CounterLabel
            //
            this.step4CounterLabel.Location = new System.Drawing.Point(23, 12);
            this.step4CounterLabel.Name = "step4CounterLabel";
            this.step4CounterLabel.Size = new System.Drawing.Size(423, 43);
            this.step4CounterLabel.TabIndex = 10;
            this.step4CounterLabel.Text = "4. Enter the initial counter value if known. Click the Verify button that will sh" +
    "ow the last code that was used.";
            //
            // secretCodeButton
            //
            this.secretCodeButton.Location = new System.Drawing.Point(360, 171);
            this.secretCodeButton.Name = "secretCodeButton";
            this.secretCodeButton.Size = new System.Drawing.Size(75, 23);
            this.secretCodeButton.TabIndex = 2;
            this.secretCodeButton.Text = "Decode";
            this.secretCodeButton.UseSelectable = true;
            this.secretCodeButton.Click += new System.EventHandler(this.secretCodeButton_Click);
            //
            // step3Label
            //
            this.step3Label.Location = new System.Drawing.Point(28, 294);
            this.step3Label.Name = "step3Label";
            this.step3Label.Size = new System.Drawing.Size(423, 43);
            this.step3Label.TabIndex = 17;
            this.step3Label.Text = "3. Select the HMAC algorithm type used by this authenticator (most authenticator " +
    "tokens use HMAC-SHA1).";
            //
            // cmbHMACType
            //
            this.cmbHMACType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbHMACType.FormattingEnabled = true;
            this.cmbHMACType.Items.AddRange(new object[] {
            "HMAC-SHA1",
            "HMAC-SHA256",
            "HMAC-SHA512"});
            this.cmbHMACType.Location = new System.Drawing.Point(124, 340);
            this.cmbHMACType.Name = "cmbHMACType";
            this.cmbHMACType.Size = new System.Drawing.Size(243, 21);
            this.cmbHMACType.TabIndex = 18;
            //
            // AddAuthenticator
            //
            this.AcceptButton = this.okButton;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
            this.CancelButton = this.cancelButton;
            this.ClientSize = new System.Drawing.Size(471, 603);
            this.Controls.Add(this.cmbHMACType);
            this.Controls.Add(this.step3Label);
            this.Controls.Add(this.counterBasedPanel);
            this.Controls.Add(this.secretCodeButton);
            this.Controls.Add(this.timeBasedPanel);
            this.Controls.Add(this.counterBasedRadio);
            this.Controls.Add(this.timeBasedRadio);
            this.Controls.Add(this.codeProgress);
            this.Controls.Add(this.codeField);
            this.Controls.Add(this.step5Label);
            this.Controls.Add(this.step2Label);
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
            this.timeBasedPanel.ResumeLayout(false);
            this.counterBasedPanel.ResumeLayout(false);
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
        private MetroFramework.Controls.MetroLabel step5Label;
        private System.Windows.Forms.Timer timer;
        private MetroFramework.Controls.MetroLabel step4TimerLabel;
        private MetroFramework.Controls.MetroRadioButton timeBasedRadio;
        private MetroFramework.Controls.MetroRadioButton counterBasedRadio;
        private System.Windows.Forms.Panel timeBasedPanel;
        private System.Windows.Forms.Panel counterBasedPanel;
        private MetroFramework.Controls.MetroLabel step4CounterLabel;
        private MetroFramework.Controls.MetroTextBox counterField;
        private MetroFramework.Controls.MetroButton verifyCounterButton;
        private MetroFramework.Controls.MetroButton secretCodeButton;
        private MetroFramework.Controls.MetroLabel step3Label;
        private System.Windows.Forms.ComboBox cmbHMACType;
    }
}