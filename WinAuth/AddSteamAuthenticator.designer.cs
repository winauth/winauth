namespace WinAuth
{
	partial class AddSteamAuthenticator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddSteamAuthenticator));
			this.loginButton = new MetroFramework.Controls.MetroButton();
			this.authoriseTabLabel = new MetroFramework.Controls.MetroLabel();
			this.loginTabLabel = new MetroFramework.Controls.MetroLabel();
			this.captchaButton = new MetroFramework.Controls.MetroButton();
			this.captchacodeField = new MetroFramework.Controls.MetroTextBox();
			this.usernameField = new MetroFramework.Controls.MetroTextBox();
			this.captchaTabLabel = new MetroFramework.Controls.MetroLabel();
			this.confirmButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.steam2IconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.steamIconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.steamAuthenticatorIconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.iconLabel = new MetroFramework.Controls.MetroLabel();
			this.nameLabel = new MetroFramework.Controls.MetroLabel();
			this.nameField = new MetroFramework.Controls.MetroTextBox();
			this.tabs = new MetroFramework.Controls.MetroTabControl();
			this.loginTab = new MetroFramework.Controls.MetroTabPage();
			this.captchaGroup = new System.Windows.Forms.Panel();
			this.captchaBox = new System.Windows.Forms.PictureBox();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.passwordLabel = new MetroFramework.Controls.MetroLabel();
			this.usernameLabel = new MetroFramework.Controls.MetroLabel();
			this.authTab = new MetroFramework.Controls.MetroTabPage();
			this.authcodeLabel = new MetroFramework.Controls.MetroLabel();
			this.authcodeButton = new MetroFramework.Controls.MetroButton();
			this.authcodeField = new MetroFramework.Controls.MetroTextBox();
			this.confirmTab = new MetroFramework.Controls.MetroTabPage();
			this.revocationCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.revocationcodeCopy = new MetroFramework.Controls.MetroCheckBox();
			this.revocationcodeText = new MetroFramework.Controls.MetroLabel();
			this.revocationcodeField = new WinAuth.SecretTextBox();
			this.revocationcodeLabel = new MetroFramework.Controls.MetroLabel();
			this.activationcodeLabel = new MetroFramework.Controls.MetroLabel();
			this.activationcodeField = new MetroFramework.Controls.MetroTextBox();
			this.confirmTabLabel = new MetroFramework.Controls.MetroLabel();
			this.addedTab = new MetroFramework.Controls.MetroTabPage();
			this.secretCopy = new MetroFramework.Controls.MetroCheckBox();
			this.revocationcode2Copy = new MetroFramework.Controls.MetroCheckBox();
			this.serialCopy = new MetroFramework.Controls.MetroCheckBox();
			this.serialLabel = new MetroFramework.Controls.MetroLabel();
			this.secretkeyLabel = new MetroFramework.Controls.MetroLabel();
			this.revocationcode2Label = new MetroFramework.Controls.MetroLabel();
			this.serialField = new WinAuth.SecretTextBox();
			this.secretkeyField = new WinAuth.SecretTextBox();
			this.revocationcode2Field = new WinAuth.SecretTextBox();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.steam3IconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.closeButton = new MetroFramework.Controls.MetroButton();
			this.steam3Icon = new System.Windows.Forms.PictureBox();
			this.steam2Icon = new System.Windows.Forms.PictureBox();
			this.steamIcon = new System.Windows.Forms.PictureBox();
			this.steamAuthenticatorIcon = new System.Windows.Forms.PictureBox();
			this.tabs.SuspendLayout();
			this.loginTab.SuspendLayout();
			this.captchaGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.captchaBox)).BeginInit();
			this.authTab.SuspendLayout();
			this.confirmTab.SuspendLayout();
			this.addedTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.steam3Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.steam2Icon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.steamIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.steamAuthenticatorIcon)).BeginInit();
			this.SuspendLayout();
			// 
			// loginButton
			// 
			this.loginButton.Location = new System.Drawing.Point(104, 127);
			this.loginButton.Name = "loginButton";
			this.loginButton.Size = new System.Drawing.Size(110, 24);
			this.loginButton.TabIndex = 2;
			this.loginButton.Text = "Login";
			this.loginButton.UseSelectable = true;
			this.loginButton.Click += new System.EventHandler(this.loginButton_Click);
			// 
			// authoriseTabLabel
			// 
			this.authoriseTabLabel.Location = new System.Drawing.Point(4, 12);
			this.authoriseTabLabel.Name = "authoriseTabLabel";
			this.authoriseTabLabel.Size = new System.Drawing.Size(449, 45);
			this.authoriseTabLabel.TabIndex = 1;
			this.authoriseTabLabel.Text = "Please enter the code sent to your {0} email address approving access from a new " +
    "device.";
			// 
			// loginTabLabel
			// 
			this.loginTabLabel.Location = new System.Drawing.Point(7, 10);
			this.loginTabLabel.Name = "loginTabLabel";
			this.loginTabLabel.Size = new System.Drawing.Size(431, 46);
			this.loginTabLabel.TabIndex = 0;
			this.loginTabLabel.Text = "Enter your steam username and password. This is needed to login to your account t" +
    "o add a new authenticator.";
			// 
			// captchaButton
			// 
			this.captchaButton.Location = new System.Drawing.Point(97, 118);
			this.captchaButton.Name = "captchaButton";
			this.captchaButton.Size = new System.Drawing.Size(110, 23);
			this.captchaButton.TabIndex = 1;
			this.captchaButton.Text = "Login";
			this.captchaButton.UseSelectable = true;
			this.captchaButton.Click += new System.EventHandler(this.captchaButton_Click);
			// 
			// captchacodeField
			// 
			this.captchacodeField.Location = new System.Drawing.Point(97, 78);
			this.captchacodeField.MaxLength = 32767;
			this.captchacodeField.Name = "captchacodeField";
			this.captchacodeField.PasswordChar = '\0';
			this.captchacodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.captchacodeField.SelectedText = "";
			this.captchacodeField.Size = new System.Drawing.Size(206, 22);
			this.captchacodeField.TabIndex = 0;
			this.captchacodeField.UseSelectable = true;
			// 
			// usernameField
			// 
			this.usernameField.Location = new System.Drawing.Point(104, 59);
			this.usernameField.MaxLength = 32767;
			this.usernameField.Name = "usernameField";
			this.usernameField.PasswordChar = '\0';
			this.usernameField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.usernameField.SelectedText = "";
			this.usernameField.Size = new System.Drawing.Size(177, 22);
			this.usernameField.TabIndex = 0;
			this.usernameField.UseSelectable = true;
			// 
			// captchaTabLabel
			// 
			this.captchaTabLabel.AutoSize = true;
			this.captchaTabLabel.Location = new System.Drawing.Point(97, 10);
			this.captchaTabLabel.Name = "captchaTabLabel";
			this.captchaTabLabel.Size = new System.Drawing.Size(249, 19);
			this.captchaTabLabel.TabIndex = 0;
			this.captchaTabLabel.Text = "Enter the characters you see in the image";
			// 
			// confirmButton
			// 
			this.confirmButton.Enabled = false;
			this.confirmButton.Location = new System.Drawing.Point(137, 243);
			this.confirmButton.Name = "confirmButton";
			this.confirmButton.Size = new System.Drawing.Size(116, 23);
			this.confirmButton.TabIndex = 2;
			this.confirmButton.Text = "Confirm";
			this.confirmButton.UseSelectable = true;
			this.confirmButton.Click += new System.EventHandler(this.confirmButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(403, 518);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 0;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// steam2IconRadioButton
			// 
			this.steam2IconRadioButton.Group = "ICON";
			this.steam2IconRadioButton.Location = new System.Drawing.Point(232, 122);
			this.steam2IconRadioButton.Name = "steam2IconRadioButton";
			this.steam2IconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.steam2IconRadioButton.TabIndex = 3;
			this.steam2IconRadioButton.Tag = "Steam2Icon.png";
			this.steam2IconRadioButton.UseSelectable = true;
			this.steam2IconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// steamIconRadioButton
			// 
			this.steamIconRadioButton.Group = "ICON";
			this.steamIconRadioButton.Location = new System.Drawing.Point(156, 122);
			this.steamIconRadioButton.Name = "steamIconRadioButton";
			this.steamIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.steamIconRadioButton.TabIndex = 2;
			this.steamIconRadioButton.Tag = "RiftIcon.png";
			this.steamIconRadioButton.UseSelectable = true;
			this.steamIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// steamAuthenticatorIconRadioButton
			// 
			this.steamAuthenticatorIconRadioButton.Checked = true;
			this.steamAuthenticatorIconRadioButton.Group = "ICON";
			this.steamAuthenticatorIconRadioButton.Location = new System.Drawing.Point(80, 122);
			this.steamAuthenticatorIconRadioButton.Name = "steamAuthenticatorIconRadioButton";
			this.steamAuthenticatorIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.steamAuthenticatorIconRadioButton.TabIndex = 1;
			this.steamAuthenticatorIconRadioButton.TabStop = true;
			this.steamAuthenticatorIconRadioButton.Tag = "SteamAuthenticatorIcon.png";
			this.steamAuthenticatorIconRadioButton.UseSelectable = true;
			this.steamAuthenticatorIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
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
			this.nameField.Location = new System.Drawing.Point(77, 69);
			this.nameField.MaxLength = 32767;
			this.nameField.Name = "nameField";
			this.nameField.PasswordChar = '\0';
			this.nameField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.nameField.SelectedText = "";
			this.nameField.Size = new System.Drawing.Size(388, 22);
			this.nameField.TabIndex = 0;
			this.nameField.UseSelectable = true;
			// 
			// tabs
			// 
			this.tabs.Controls.Add(this.loginTab);
			this.tabs.Controls.Add(this.authTab);
			this.tabs.Controls.Add(this.confirmTab);
			this.tabs.Controls.Add(this.addedTab);
			this.tabs.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabs.ItemSize = new System.Drawing.Size(120, 18);
			this.tabs.Location = new System.Drawing.Point(15, 180);
			this.tabs.Name = "tabs";
			this.tabs.SelectedIndex = 0;
			this.tabs.Size = new System.Drawing.Size(464, 327);
			this.tabs.TabIndex = 0;
			this.tabs.UseSelectable = true;
			this.tabs.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
			// 
			// loginTab
			// 
			this.loginTab.BackColor = System.Drawing.SystemColors.Control;
			this.loginTab.Controls.Add(this.captchaGroup);
			this.loginTab.Controls.Add(this.loginButton);
			this.loginTab.Controls.Add(this.passwordField);
			this.loginTab.Controls.Add(this.usernameField);
			this.loginTab.Controls.Add(this.passwordLabel);
			this.loginTab.Controls.Add(this.usernameLabel);
			this.loginTab.Controls.Add(this.loginTabLabel);
			this.loginTab.ForeColor = System.Drawing.SystemColors.ControlText;
			this.loginTab.HorizontalScrollbarBarColor = true;
			this.loginTab.HorizontalScrollbarHighlightOnWheel = false;
			this.loginTab.HorizontalScrollbarSize = 10;
			this.loginTab.Location = new System.Drawing.Point(4, 22);
			this.loginTab.Name = "loginTab";
			this.loginTab.Padding = new System.Windows.Forms.Padding(3);
			this.loginTab.Size = new System.Drawing.Size(456, 301);
			this.loginTab.TabIndex = 0;
			this.loginTab.Tag = "";
			this.loginTab.Text = "Login";
			this.loginTab.VerticalScrollbarBarColor = true;
			this.loginTab.VerticalScrollbarHighlightOnWheel = false;
			this.loginTab.VerticalScrollbarSize = 10;
			// 
			// captchaGroup
			// 
			this.captchaGroup.BackColor = System.Drawing.SystemColors.ControlLightLight;
			this.captchaGroup.Controls.Add(this.captchaBox);
			this.captchaGroup.Controls.Add(this.captchaTabLabel);
			this.captchaGroup.Controls.Add(this.captchacodeField);
			this.captchaGroup.Controls.Add(this.captchaButton);
			this.captchaGroup.Location = new System.Drawing.Point(7, 115);
			this.captchaGroup.Name = "captchaGroup";
			this.captchaGroup.Size = new System.Drawing.Size(431, 167);
			this.captchaGroup.TabIndex = 4;
			this.captchaGroup.Visible = false;
			// 
			// captchaBox
			// 
			this.captchaBox.Location = new System.Drawing.Point(97, 32);
			this.captchaBox.Name = "captchaBox";
			this.captchaBox.Size = new System.Drawing.Size(206, 40);
			this.captchaBox.TabIndex = 3;
			this.captchaBox.TabStop = false;
			// 
			// passwordField
			// 
			this.passwordField.Location = new System.Drawing.Point(104, 87);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(177, 22);
			this.passwordField.TabIndex = 1;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			// 
			// passwordLabel
			// 
			this.passwordLabel.Location = new System.Drawing.Point(18, 87);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(80, 25);
			this.passwordLabel.TabIndex = 1;
			this.passwordLabel.Text = "Password";
			// 
			// usernameLabel
			// 
			this.usernameLabel.Location = new System.Drawing.Point(18, 60);
			this.usernameLabel.Name = "usernameLabel";
			this.usernameLabel.Size = new System.Drawing.Size(80, 25);
			this.usernameLabel.TabIndex = 1;
			this.usernameLabel.Text = "Username";
			// 
			// authTab
			// 
			this.authTab.Controls.Add(this.authcodeLabel);
			this.authTab.Controls.Add(this.authcodeButton);
			this.authTab.Controls.Add(this.authcodeField);
			this.authTab.Controls.Add(this.authoriseTabLabel);
			this.authTab.HorizontalScrollbarBarColor = true;
			this.authTab.HorizontalScrollbarHighlightOnWheel = false;
			this.authTab.HorizontalScrollbarSize = 10;
			this.authTab.Location = new System.Drawing.Point(4, 22);
			this.authTab.Name = "authTab";
			this.authTab.Size = new System.Drawing.Size(456, 301);
			this.authTab.TabIndex = 2;
			this.authTab.Tag = "";
			this.authTab.Text = "Authorise";
			this.authTab.VerticalScrollbarBarColor = true;
			this.authTab.VerticalScrollbarHighlightOnWheel = false;
			this.authTab.VerticalScrollbarSize = 10;
			// 
			// authcodeLabel
			// 
			this.authcodeLabel.Location = new System.Drawing.Point(18, 60);
			this.authcodeLabel.Name = "authcodeLabel";
			this.authcodeLabel.Size = new System.Drawing.Size(80, 25);
			this.authcodeLabel.TabIndex = 2;
			this.authcodeLabel.Text = "Code";
			// 
			// authcodeButton
			// 
			this.authcodeButton.Location = new System.Drawing.Point(104, 101);
			this.authcodeButton.Name = "authcodeButton";
			this.authcodeButton.Size = new System.Drawing.Size(110, 24);
			this.authcodeButton.TabIndex = 1;
			this.authcodeButton.Text = "Continue";
			this.authcodeButton.UseSelectable = true;
			this.authcodeButton.Click += new System.EventHandler(this.authcodeButton_Click);
			// 
			// authcodeField
			// 
			this.authcodeField.Location = new System.Drawing.Point(104, 60);
			this.authcodeField.MaxLength = 32767;
			this.authcodeField.Name = "authcodeField";
			this.authcodeField.PasswordChar = '\0';
			this.authcodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.authcodeField.SelectedText = "";
			this.authcodeField.Size = new System.Drawing.Size(206, 22);
			this.authcodeField.TabIndex = 0;
			this.authcodeField.UseSelectable = true;
			// 
			// confirmTab
			// 
			this.confirmTab.Controls.Add(this.revocationCheckbox);
			this.confirmTab.Controls.Add(this.revocationcodeCopy);
			this.confirmTab.Controls.Add(this.revocationcodeText);
			this.confirmTab.Controls.Add(this.revocationcodeField);
			this.confirmTab.Controls.Add(this.revocationcodeLabel);
			this.confirmTab.Controls.Add(this.activationcodeLabel);
			this.confirmTab.Controls.Add(this.confirmButton);
			this.confirmTab.Controls.Add(this.activationcodeField);
			this.confirmTab.Controls.Add(this.confirmTabLabel);
			this.confirmTab.HorizontalScrollbarBarColor = true;
			this.confirmTab.HorizontalScrollbarHighlightOnWheel = false;
			this.confirmTab.HorizontalScrollbarSize = 10;
			this.confirmTab.Location = new System.Drawing.Point(4, 22);
			this.confirmTab.Name = "confirmTab";
			this.confirmTab.Size = new System.Drawing.Size(456, 301);
			this.confirmTab.TabIndex = 3;
			this.confirmTab.Tag = "";
			this.confirmTab.Text = "Confirm";
			this.confirmTab.VerticalScrollbarBarColor = true;
			this.confirmTab.VerticalScrollbarHighlightOnWheel = false;
			this.confirmTab.VerticalScrollbarSize = 10;
			// 
			// revocationCheckbox
			// 
			this.revocationCheckbox.AutoSize = true;
			this.revocationCheckbox.Location = new System.Drawing.Point(137, 208);
			this.revocationCheckbox.Name = "revocationCheckbox";
			this.revocationCheckbox.Size = new System.Drawing.Size(235, 15);
			this.revocationCheckbox.TabIndex = 1;
			this.revocationCheckbox.Text = "I have written down my revocation code";
			this.revocationCheckbox.UseSelectable = true;
			this.revocationCheckbox.CheckedChanged += new System.EventHandler(this.revocationCheckbox_CheckedChanged);
			// 
			// revocationcodeCopy
			// 
			this.revocationcodeCopy.AutoSize = true;
			this.revocationcodeCopy.Location = new System.Drawing.Point(320, 169);
			this.revocationcodeCopy.Name = "revocationcodeCopy";
			this.revocationcodeCopy.Size = new System.Drawing.Size(87, 15);
			this.revocationcodeCopy.TabIndex = 3;
			this.revocationcodeCopy.Text = "Allow copy?";
			this.revocationcodeCopy.UseSelectable = true;
			this.revocationcodeCopy.CheckedChanged += new System.EventHandler(this.revocationcodeCopy_CheckedChanged);
			// 
			// revocationcodeText
			// 
			this.revocationcodeText.Location = new System.Drawing.Point(4, 95);
			this.revocationcodeText.Name = "revocationcodeText";
			this.revocationcodeText.Size = new System.Drawing.Size(442, 65);
			this.revocationcodeText.TabIndex = 17;
			this.revocationcodeText.Text = "This is your Revocation code.  It can be used to remove the authenticator from yo" +
    "ur account if you ever lose it. Please write it down now and put it somewhere sa" +
    "fe.";
			// 
			// revocationcodeField
			// 
			this.revocationcodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.revocationcodeField.Location = new System.Drawing.Point(137, 163);
			this.revocationcodeField.Multiline = true;
			this.revocationcodeField.Name = "revocationcodeField";
			this.revocationcodeField.SecretMode = false;
			this.revocationcodeField.Size = new System.Drawing.Size(177, 26);
			this.revocationcodeField.SpaceOut = 0;
			this.revocationcodeField.TabIndex = 16;
			// 
			// revocationcodeLabel
			// 
			this.revocationcodeLabel.Location = new System.Drawing.Point(20, 164);
			this.revocationcodeLabel.Name = "revocationcodeLabel";
			this.revocationcodeLabel.Size = new System.Drawing.Size(111, 25);
			this.revocationcodeLabel.TabIndex = 5;
			this.revocationcodeLabel.Text = "Revocation code";
			// 
			// activationcodeLabel
			// 
			this.activationcodeLabel.Location = new System.Drawing.Point(20, 49);
			this.activationcodeLabel.Name = "activationcodeLabel";
			this.activationcodeLabel.Size = new System.Drawing.Size(111, 25);
			this.activationcodeLabel.TabIndex = 5;
			this.activationcodeLabel.Text = "Activation code";
			// 
			// activationcodeField
			// 
			this.activationcodeField.Location = new System.Drawing.Point(137, 49);
			this.activationcodeField.MaxLength = 32767;
			this.activationcodeField.Name = "activationcodeField";
			this.activationcodeField.PasswordChar = '\0';
			this.activationcodeField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.activationcodeField.SelectedText = "";
			this.activationcodeField.Size = new System.Drawing.Size(177, 22);
			this.activationcodeField.TabIndex = 0;
			this.activationcodeField.UseSelectable = true;
			// 
			// confirmTabLabel
			// 
			this.confirmTabLabel.Location = new System.Drawing.Point(4, 12);
			this.confirmTabLabel.Name = "confirmTabLabel";
			this.confirmTabLabel.Size = new System.Drawing.Size(449, 34);
			this.confirmTabLabel.TabIndex = 2;
			this.confirmTabLabel.Text = "Enter the activation code you should receive by email. ";
			// 
			// addedTab
			// 
			this.addedTab.Controls.Add(this.secretCopy);
			this.addedTab.Controls.Add(this.revocationcode2Copy);
			this.addedTab.Controls.Add(this.serialCopy);
			this.addedTab.Controls.Add(this.serialLabel);
			this.addedTab.Controls.Add(this.secretkeyLabel);
			this.addedTab.Controls.Add(this.revocationcode2Label);
			this.addedTab.Controls.Add(this.serialField);
			this.addedTab.Controls.Add(this.secretkeyField);
			this.addedTab.Controls.Add(this.revocationcode2Field);
			this.addedTab.Controls.Add(this.metroLabel1);
			this.addedTab.HorizontalScrollbarBarColor = true;
			this.addedTab.HorizontalScrollbarHighlightOnWheel = false;
			this.addedTab.HorizontalScrollbarSize = 10;
			this.addedTab.Location = new System.Drawing.Point(4, 22);
			this.addedTab.Name = "addedTab";
			this.addedTab.Size = new System.Drawing.Size(456, 301);
			this.addedTab.TabIndex = 4;
			this.addedTab.Tag = "";
			this.addedTab.Text = "Added";
			this.addedTab.VerticalScrollbarBarColor = true;
			this.addedTab.VerticalScrollbarHighlightOnWheel = false;
			this.addedTab.VerticalScrollbarSize = 10;
			// 
			// secretCopy
			// 
			this.secretCopy.AutoSize = true;
			this.secretCopy.Location = new System.Drawing.Point(320, 195);
			this.secretCopy.Name = "secretCopy";
			this.secretCopy.Size = new System.Drawing.Size(87, 15);
			this.secretCopy.TabIndex = 2;
			this.secretCopy.Text = "Allow copy?";
			this.secretCopy.UseSelectable = true;
			this.secretCopy.CheckedChanged += new System.EventHandler(this.secretCopy_CheckedChanged);
			// 
			// revocationcode2Copy
			// 
			this.revocationcode2Copy.AutoSize = true;
			this.revocationcode2Copy.Location = new System.Drawing.Point(320, 111);
			this.revocationcode2Copy.Name = "revocationcode2Copy";
			this.revocationcode2Copy.Size = new System.Drawing.Size(87, 15);
			this.revocationcode2Copy.TabIndex = 0;
			this.revocationcode2Copy.Text = "Allow copy?";
			this.revocationcode2Copy.UseSelectable = true;
			this.revocationcode2Copy.CheckedChanged += new System.EventHandler(this.revocationcode2Copy_CheckedChanged);
			// 
			// serialCopy
			// 
			this.serialCopy.AutoSize = true;
			this.serialCopy.Location = new System.Drawing.Point(320, 162);
			this.serialCopy.Name = "serialCopy";
			this.serialCopy.Size = new System.Drawing.Size(87, 15);
			this.serialCopy.TabIndex = 1;
			this.serialCopy.Text = "Allow copy?";
			this.serialCopy.UseSelectable = true;
			this.serialCopy.CheckedChanged += new System.EventHandler(this.serialCopy_CheckedChanged);
			// 
			// serialLabel
			// 
			this.serialLabel.Location = new System.Drawing.Point(20, 156);
			this.serialLabel.Name = "serialLabel";
			this.serialLabel.Size = new System.Drawing.Size(111, 25);
			this.serialLabel.TabIndex = 8;
			this.serialLabel.Text = "Serial Number";
			// 
			// secretkeyLabel
			// 
			this.secretkeyLabel.Location = new System.Drawing.Point(20, 189);
			this.secretkeyLabel.Name = "secretkeyLabel";
			this.secretkeyLabel.Size = new System.Drawing.Size(111, 25);
			this.secretkeyLabel.TabIndex = 8;
			this.secretkeyLabel.Text = "Secret Key";
			// 
			// revocationcode2Label
			// 
			this.revocationcode2Label.Location = new System.Drawing.Point(20, 105);
			this.revocationcode2Label.Name = "revocationcode2Label";
			this.revocationcode2Label.Size = new System.Drawing.Size(111, 25);
			this.revocationcode2Label.TabIndex = 8;
			this.revocationcode2Label.Text = "Revocation code";
			// 
			// serialField
			// 
			this.serialField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.serialField.Location = new System.Drawing.Point(137, 156);
			this.serialField.Multiline = true;
			this.serialField.Name = "serialField";
			this.serialField.SecretMode = false;
			this.serialField.Size = new System.Drawing.Size(177, 26);
			this.serialField.SpaceOut = 0;
			this.serialField.TabIndex = 7;
			// 
			// secretkeyField
			// 
			this.secretkeyField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.secretkeyField.Location = new System.Drawing.Point(137, 188);
			this.secretkeyField.Multiline = true;
			this.secretkeyField.Name = "secretkeyField";
			this.secretkeyField.SecretMode = false;
			this.secretkeyField.Size = new System.Drawing.Size(177, 26);
			this.secretkeyField.SpaceOut = 0;
			this.secretkeyField.TabIndex = 7;
			// 
			// revocationcode2Field
			// 
			this.revocationcode2Field.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.revocationcode2Field.Location = new System.Drawing.Point(137, 105);
			this.revocationcode2Field.Multiline = true;
			this.revocationcode2Field.Name = "revocationcode2Field";
			this.revocationcode2Field.SecretMode = false;
			this.revocationcode2Field.Size = new System.Drawing.Size(177, 26);
			this.revocationcode2Field.SpaceOut = 0;
			this.revocationcode2Field.TabIndex = 7;
			// 
			// metroLabel1
			// 
			this.metroLabel1.Location = new System.Drawing.Point(4, 12);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(449, 90);
			this.metroLabel1.TabIndex = 2;
			this.metroLabel1.Text = resources.GetString("metroLabel1.Text");
			// 
			// steam3IconRadioButton
			// 
			this.steam3IconRadioButton.Group = "ICON";
			this.steam3IconRadioButton.Location = new System.Drawing.Point(304, 122);
			this.steam3IconRadioButton.Name = "steam3IconRadioButton";
			this.steam3IconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.steam3IconRadioButton.TabIndex = 4;
			this.steam3IconRadioButton.Tag = "Steam3Icon.png";
			this.steam3IconRadioButton.UseSelectable = true;
			this.steam3IconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// closeButton
			// 
			this.closeButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.closeButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.closeButton.Location = new System.Drawing.Point(324, 518);
			this.closeButton.Name = "closeButton";
			this.closeButton.Size = new System.Drawing.Size(75, 23);
			this.closeButton.TabIndex = 1;
			this.closeButton.Text = "Close";
			this.closeButton.UseSelectable = true;
			this.closeButton.Visible = false;
			this.closeButton.Click += new System.EventHandler(this.closeButton_Click);
			// 
			// steam3Icon
			// 
			this.steam3Icon.Image = global::WinAuth.Properties.Resources.Steam3Icon;
			this.steam3Icon.Location = new System.Drawing.Point(324, 107);
			this.steam3Icon.Name = "steam3Icon";
			this.steam3Icon.Size = new System.Drawing.Size(48, 48);
			this.steam3Icon.TabIndex = 4;
			this.steam3Icon.TabStop = false;
			this.steam3Icon.Tag = "Steam3Icon.png";
			this.steam3Icon.Click += new System.EventHandler(this.steamIcon_Click);
			// 
			// steam2Icon
			// 
			this.steam2Icon.Image = global::WinAuth.Properties.Resources.Steam2Icon;
			this.steam2Icon.Location = new System.Drawing.Point(252, 107);
			this.steam2Icon.Name = "steam2Icon";
			this.steam2Icon.Size = new System.Drawing.Size(48, 48);
			this.steam2Icon.TabIndex = 4;
			this.steam2Icon.TabStop = false;
			this.steam2Icon.Tag = "Steam2Icon.png";
			this.steam2Icon.Click += new System.EventHandler(this.iconArcheAge_Click);
			// 
			// steamIcon
			// 
			this.steamIcon.Image = global::WinAuth.Properties.Resources.SteamIcon;
			this.steamIcon.Location = new System.Drawing.Point(176, 107);
			this.steamIcon.Name = "steamIcon";
			this.steamIcon.Size = new System.Drawing.Size(48, 48);
			this.steamIcon.TabIndex = 4;
			this.steamIcon.TabStop = false;
			this.steamIcon.Tag = "SteamIcon.png";
			this.steamIcon.Click += new System.EventHandler(this.iconRift_Click);
			// 
			// steamAuthenticatorIcon
			// 
			this.steamAuthenticatorIcon.Image = global::WinAuth.Properties.Resources.SteamAuthenticatorIcon;
			this.steamAuthenticatorIcon.Location = new System.Drawing.Point(100, 107);
			this.steamAuthenticatorIcon.Name = "steamAuthenticatorIcon";
			this.steamAuthenticatorIcon.Size = new System.Drawing.Size(48, 48);
			this.steamAuthenticatorIcon.TabIndex = 4;
			this.steamAuthenticatorIcon.TabStop = false;
			this.steamAuthenticatorIcon.Tag = "SteamAuthenticatorIcon.png";
			this.steamAuthenticatorIcon.Click += new System.EventHandler(this.iconGlyph_Click);
			// 
			// AddSteamAuthenticator
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(501, 564);
			this.Controls.Add(this.tabs);
			this.Controls.Add(this.steam3IconRadioButton);
			this.Controls.Add(this.steam2IconRadioButton);
			this.Controls.Add(this.steamIconRadioButton);
			this.Controls.Add(this.closeButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.steamAuthenticatorIconRadioButton);
			this.Controls.Add(this.steam3Icon);
			this.Controls.Add(this.steam2Icon);
			this.Controls.Add(this.nameLabel);
			this.Controls.Add(this.steamIcon);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.steamAuthenticatorIcon);
			this.Controls.Add(this.iconLabel);
			this.KeyPreview = true;
			this.MaximizeBox = false;
			this.Name = "AddSteamAuthenticator";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Add Steam Authenticator";
			this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.AddSteamAuthenticator_FormClosing);
			this.Load += new System.EventHandler(this.AddSteamAuthenticator_Load);
			this.KeyPress += new System.Windows.Forms.KeyPressEventHandler(this.AddSteamAuthenticator_KeyPress);
			this.tabs.ResumeLayout(false);
			this.loginTab.ResumeLayout(false);
			this.captchaGroup.ResumeLayout(false);
			this.captchaGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.captchaBox)).EndInit();
			this.authTab.ResumeLayout(false);
			this.confirmTab.ResumeLayout(false);
			this.confirmTab.PerformLayout();
			this.addedTab.ResumeLayout(false);
			this.addedTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.steam3Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.steam2Icon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.steamIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.steamAuthenticatorIcon)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel loginTabLabel;
		private MetroFramework.Controls.MetroButton loginButton;
		private MetroFramework.Controls.MetroLabel authoriseTabLabel;
		private MetroFramework.Controls.MetroLabel captchaTabLabel;
		private MetroFramework.Controls.MetroButton confirmButton;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroTextBox captchacodeField;
		private MetroFramework.Controls.MetroTextBox usernameField;
		private MetroFramework.Controls.MetroButton captchaButton;
		private WinAuth.GroupMetroRadioButton steam2IconRadioButton;
		private WinAuth.GroupMetroRadioButton steamIconRadioButton;
		private WinAuth.GroupMetroRadioButton steamAuthenticatorIconRadioButton;
		private System.Windows.Forms.PictureBox steam2Icon;
		private System.Windows.Forms.PictureBox steamIcon;
		private System.Windows.Forms.PictureBox steamAuthenticatorIcon;
		private MetroFramework.Controls.MetroLabel nameLabel;
		private MetroFramework.Controls.MetroTextBox nameField;
		private MetroFramework.Controls.MetroLabel iconLabel;
		private MetroFramework.Controls.MetroTabControl tabs;
		private MetroFramework.Controls.MetroTabPage loginTab;
		private System.Windows.Forms.PictureBox steam3Icon;
		private GroupMetroRadioButton steam3IconRadioButton;
		private MetroFramework.Controls.MetroLabel passwordLabel;
		private MetroFramework.Controls.MetroLabel usernameLabel;
		private MetroFramework.Controls.MetroButton authcodeButton;
		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroTabPage authTab;
		private MetroFramework.Controls.MetroTabPage confirmTab;
		private System.Windows.Forms.PictureBox captchaBox;
		private MetroFramework.Controls.MetroLabel authcodeLabel;
		private MetroFramework.Controls.MetroTextBox authcodeField;
		private MetroFramework.Controls.MetroLabel confirmTabLabel;
		private MetroFramework.Controls.MetroLabel activationcodeLabel;
		private MetroFramework.Controls.MetroTextBox activationcodeField;
		private System.Windows.Forms.Panel captchaGroup;
		private MetroFramework.Controls.MetroTabPage addedTab;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroLabel revocationcode2Label;
		private SecretTextBox revocationcode2Field;
		private MetroFramework.Controls.MetroButton closeButton;
		private MetroFramework.Controls.MetroCheckBox revocationcodeCopy;
		private MetroFramework.Controls.MetroLabel revocationcodeText;
		private SecretTextBox revocationcodeField;
		private MetroFramework.Controls.MetroCheckBox revocationCheckbox;
		private MetroFramework.Controls.MetroLabel revocationcodeLabel;
		private MetroFramework.Controls.MetroLabel serialLabel;
		private MetroFramework.Controls.MetroLabel secretkeyLabel;
		private SecretTextBox serialField;
		private SecretTextBox secretkeyField;
		private MetroFramework.Controls.MetroCheckBox secretCopy;
		private MetroFramework.Controls.MetroCheckBox serialCopy;
		private MetroFramework.Controls.MetroCheckBox revocationcode2Copy;
	}
}