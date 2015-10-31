namespace WinAuth
{
	partial class AddTrionAuthenticator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddTrionAuthenticator));
			this.allowCopyNewButton = new MetroFramework.Controls.MetroCheckBox();
			this.enrollAuthenticatorButton = new MetroFramework.Controls.MetroButton();
			this.label3 = new MetroFramework.Controls.MetroLabel();
			this.label8 = new MetroFramework.Controls.MetroLabel();
			this.label2 = new MetroFramework.Controls.MetroLabel();
			this.label1 = new MetroFramework.Controls.MetroLabel();
			this.restoreAnswer2Field = new MetroFramework.Controls.MetroTextBox();
			this.restoreAnswer1Field = new MetroFramework.Controls.MetroTextBox();
			this.restoreGetQuestionsButton = new MetroFramework.Controls.MetroButton();
			this.restoreQuestion1Label = new MetroFramework.Controls.MetroLabel();
			this.label13 = new MetroFramework.Controls.MetroLabel();
			this.restoreQuestion2Label = new MetroFramework.Controls.MetroLabel();
			this.label12 = new MetroFramework.Controls.MetroLabel();
			this.label5 = new MetroFramework.Controls.MetroLabel();
			this.restoreDeviceIdField = new MetroFramework.Controls.MetroTextBox();
			this.restorePasswordField = new MetroFramework.Controls.MetroTextBox();
			this.restoreEmailField = new MetroFramework.Controls.MetroTextBox();
			this.label7 = new MetroFramework.Controls.MetroLabel();
			this.label4 = new MetroFramework.Controls.MetroLabel();
			this.label9 = new MetroFramework.Controls.MetroLabel();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.archeageIconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.riftIconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.glyphIconRadioButton = new WinAuth.GroupMetroRadioButton();
			this.label10 = new MetroFramework.Controls.MetroLabel();
			this.label6 = new MetroFramework.Controls.MetroLabel();
			this.nameField = new MetroFramework.Controls.MetroTextBox();
			this.tabControl1 = new MetroFramework.Controls.MetroTabControl();
			this.newAuthenticatorTab = new MetroFramework.Controls.MetroTabPage();
			this.newRestoreCodeField = new WinAuth.SecretTextBox();
			this.newLoginCodeField = new WinAuth.SecretTextBox();
			this.newSerialNumberField = new WinAuth.SecretTextBox();
			this.recoverAuthenticatorTab = new MetroFramework.Controls.MetroTabPage();
			this.trionAuthenticatorRadioButton = new WinAuth.GroupMetroRadioButton();
			this.iconTrion = new System.Windows.Forms.PictureBox();
			this.archeageIcon = new System.Windows.Forms.PictureBox();
			this.iconRift = new System.Windows.Forms.PictureBox();
			this.iconGlyph = new System.Windows.Forms.PictureBox();
			this.tabControl1.SuspendLayout();
			this.newAuthenticatorTab.SuspendLayout();
			this.recoverAuthenticatorTab.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconTrion)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.archeageIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iconRift)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iconGlyph)).BeginInit();
			this.SuspendLayout();
			// 
			// allowCopyNewButton
			// 
			this.allowCopyNewButton.AutoSize = true;
			this.allowCopyNewButton.Location = new System.Drawing.Point(351, 141);
			this.allowCopyNewButton.Name = "allowCopyNewButton";
			this.allowCopyNewButton.Size = new System.Drawing.Size(87, 15);
			this.allowCopyNewButton.TabIndex = 2;
			this.allowCopyNewButton.Text = "Allow copy?";
			this.allowCopyNewButton.UseSelectable = true;
			this.allowCopyNewButton.CheckedChanged += new System.EventHandler(this.allowCopyNewButton_CheckedChanged);
			// 
			// enrollAuthenticatorButton
			// 
			this.enrollAuthenticatorButton.Location = new System.Drawing.Point(64, 73);
			this.enrollAuthenticatorButton.Name = "enrollAuthenticatorButton";
			this.enrollAuthenticatorButton.Size = new System.Drawing.Size(177, 24);
			this.enrollAuthenticatorButton.TabIndex = 0;
			this.enrollAuthenticatorButton.Text = "Create Authenticator";
			this.enrollAuthenticatorButton.UseSelectable = true;
			this.enrollAuthenticatorButton.Click += new System.EventHandler(this.enrollAuthenticatorButton_Click);
			// 
			// label3
			// 
			this.label3.Location = new System.Drawing.Point(7, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 25);
			this.label3.TabIndex = 1;
			this.label3.Text = "3. Click:";
			// 
			// label8
			// 
			this.label8.Location = new System.Drawing.Point(7, 173);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(431, 81);
			this.label8.TabIndex = 1;
			this.label8.Text = resources.GetString("label8.Text");
			// 
			// label2
			// 
			this.label2.Location = new System.Drawing.Point(7, 106);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(431, 25);
			this.label2.TabIndex = 1;
			this.label2.Text = "4. Add the following Authenticator Serial Key with your secret answers:";
			// 
			// label1
			// 
			this.label1.Location = new System.Drawing.Point(7, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(431, 58);
			this.label1.TabIndex = 0;
			this.label1.Text = "1. Go to www.trionworlds.com and login to your account.\r\n2. Click the Security ta" +
    "b, and \"Add the RIFT Mobile Authenticator\". You must have already added your secr" +
    "et questions and answers.";
			// 
			// restoreAnswer2Field
			// 
			this.restoreAnswer2Field.Location = new System.Drawing.Point(130, 299);
			this.restoreAnswer2Field.MaxLength = 32767;
			this.restoreAnswer2Field.Name = "restoreAnswer2Field";
			this.restoreAnswer2Field.PasswordChar = '\0';
			this.restoreAnswer2Field.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.restoreAnswer2Field.SelectedText = "";
			this.restoreAnswer2Field.Size = new System.Drawing.Size(257, 22);
			this.restoreAnswer2Field.TabIndex = 5;
			this.restoreAnswer2Field.UseSelectable = true;
			// 
			// restoreAnswer1Field
			// 
			this.restoreAnswer1Field.Location = new System.Drawing.Point(130, 252);
			this.restoreAnswer1Field.MaxLength = 32767;
			this.restoreAnswer1Field.Name = "restoreAnswer1Field";
			this.restoreAnswer1Field.PasswordChar = '\0';
			this.restoreAnswer1Field.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.restoreAnswer1Field.SelectedText = "";
			this.restoreAnswer1Field.Size = new System.Drawing.Size(257, 22);
			this.restoreAnswer1Field.TabIndex = 4;
			this.restoreAnswer1Field.UseSelectable = true;
			// 
			// restoreGetQuestionsButton
			// 
			this.restoreGetQuestionsButton.Location = new System.Drawing.Point(130, 100);
			this.restoreGetQuestionsButton.Name = "restoreGetQuestionsButton";
			this.restoreGetQuestionsButton.Size = new System.Drawing.Size(166, 23);
			this.restoreGetQuestionsButton.TabIndex = 2;
			this.restoreGetQuestionsButton.Text = "Get Security Questions";
			this.restoreGetQuestionsButton.UseSelectable = true;
			this.restoreGetQuestionsButton.Click += new System.EventHandler(this.restoreGetQuestionsButton_Click);
			// 
			// restoreQuestion1Label
			// 
			this.restoreQuestion1Label.Location = new System.Drawing.Point(25, 231);
			this.restoreQuestion1Label.Name = "restoreQuestion1Label";
			this.restoreQuestion1Label.Size = new System.Drawing.Size(413, 17);
			this.restoreQuestion1Label.TabIndex = 1;
			this.restoreQuestion1Label.Text = "Your first security question";
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Location = new System.Drawing.Point(25, 75);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(66, 19);
			this.label13.TabIndex = 1;
			this.label13.Text = "Password:";
			// 
			// restoreQuestion2Label
			// 
			this.restoreQuestion2Label.Location = new System.Drawing.Point(25, 278);
			this.restoreQuestion2Label.Name = "restoreQuestion2Label";
			this.restoreQuestion2Label.Size = new System.Drawing.Size(413, 17);
			this.restoreQuestion2Label.TabIndex = 1;
			this.restoreQuestion2Label.Text = "Your second security question";
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Location = new System.Drawing.Point(25, 47);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(95, 19);
			this.label12.TabIndex = 1;
			this.label12.Text = "Email Address:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Location = new System.Drawing.Point(10, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(194, 19);
			this.label5.TabIndex = 1;
			this.label5.Text = "3. Answer your secret questions:";
			// 
			// restoreDeviceIdField
			// 
			this.restoreDeviceIdField.Location = new System.Drawing.Point(130, 164);
			this.restoreDeviceIdField.MaxLength = 32767;
			this.restoreDeviceIdField.Name = "restoreDeviceIdField";
			this.restoreDeviceIdField.PasswordChar = '\0';
			this.restoreDeviceIdField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.restoreDeviceIdField.SelectedText = "";
			this.restoreDeviceIdField.Size = new System.Drawing.Size(257, 22);
			this.restoreDeviceIdField.TabIndex = 3;
			this.restoreDeviceIdField.UseSelectable = true;
			// 
			// restorePasswordField
			// 
			this.restorePasswordField.Location = new System.Drawing.Point(130, 72);
			this.restorePasswordField.MaxLength = 32767;
			this.restorePasswordField.Name = "restorePasswordField";
			this.restorePasswordField.PasswordChar = '●';
			this.restorePasswordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.restorePasswordField.SelectedText = "";
			this.restorePasswordField.Size = new System.Drawing.Size(257, 22);
			this.restorePasswordField.TabIndex = 1;
			this.restorePasswordField.UseSelectable = true;
			this.restorePasswordField.UseSystemPasswordChar = true;
			// 
			// restoreEmailField
			// 
			this.restoreEmailField.Location = new System.Drawing.Point(130, 44);
			this.restoreEmailField.MaxLength = 32767;
			this.restoreEmailField.Name = "restoreEmailField";
			this.restoreEmailField.PasswordChar = '\0';
			this.restoreEmailField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.restoreEmailField.SelectedText = "";
			this.restoreEmailField.Size = new System.Drawing.Size(257, 22);
			this.restoreEmailField.TabIndex = 0;
			this.restoreEmailField.UseSelectable = true;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Location = new System.Drawing.Point(25, 165);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(66, 19);
			this.label7.TabIndex = 1;
			this.label7.Text = "Device ID:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Location = new System.Drawing.Point(10, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(413, 19);
			this.label4.TabIndex = 1;
			this.label4.Text = "2. Enter the Device ID you saved when you created your authenticator:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Location = new System.Drawing.Point(7, 13);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(251, 19);
			this.label9.TabIndex = 1;
			this.label9.Text = "1. Enter your account email and password:";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(323, 539);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 5;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(404, 539);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 6;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Enabled = true;
			this.newAuthenticatorTimer.Interval = 500;
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// archeageIconRadioButton
			// 
			this.archeageIconRadioButton.Group = "ICON";
			this.archeageIconRadioButton.Location = new System.Drawing.Point(232, 122);
			this.archeageIconRadioButton.Name = "archeageIconRadioButton";
			this.archeageIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.archeageIconRadioButton.TabIndex = 3;
			this.archeageIconRadioButton.Tag = "ArcheAgeIcon.png";
			this.archeageIconRadioButton.UseSelectable = true;
			this.archeageIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// riftIconRadioButton
			// 
			this.riftIconRadioButton.Group = "ICON";
			this.riftIconRadioButton.Location = new System.Drawing.Point(156, 122);
			this.riftIconRadioButton.Name = "riftIconRadioButton";
			this.riftIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.riftIconRadioButton.TabIndex = 2;
			this.riftIconRadioButton.Tag = "RiftIcon.png";
			this.riftIconRadioButton.UseSelectable = true;
			this.riftIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// glyphIconRadioButton
			// 
			this.glyphIconRadioButton.Checked = true;
			this.glyphIconRadioButton.Group = "ICON";
			this.glyphIconRadioButton.Location = new System.Drawing.Point(80, 122);
			this.glyphIconRadioButton.Name = "glyphIconRadioButton";
			this.glyphIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.glyphIconRadioButton.TabIndex = 1;
			this.glyphIconRadioButton.TabStop = true;
			this.glyphIconRadioButton.Tag = "GlyphIcon.png";
			this.glyphIconRadioButton.UseSelectable = true;
			this.glyphIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
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
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Location = new System.Drawing.Point(23, 70);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 19);
			this.label6.TabIndex = 3;
			this.label6.Text = "Name:";
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
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.newAuthenticatorTab);
			this.tabControl1.Controls.Add(this.recoverAuthenticatorTab);
			this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl1.ItemSize = new System.Drawing.Size(120, 18);
			this.tabControl1.Location = new System.Drawing.Point(23, 174);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(456, 352);
			this.tabControl1.TabIndex = 4;
			this.tabControl1.UseSelectable = true;
			this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
			// 
			// newAuthenticatorTab
			// 
			this.newAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
			this.newAuthenticatorTab.Controls.Add(this.allowCopyNewButton);
			this.newAuthenticatorTab.Controls.Add(this.enrollAuthenticatorButton);
			this.newAuthenticatorTab.Controls.Add(this.label8);
			this.newAuthenticatorTab.Controls.Add(this.newRestoreCodeField);
			this.newAuthenticatorTab.Controls.Add(this.label2);
			this.newAuthenticatorTab.Controls.Add(this.label3);
			this.newAuthenticatorTab.Controls.Add(this.newLoginCodeField);
			this.newAuthenticatorTab.Controls.Add(this.label1);
			this.newAuthenticatorTab.Controls.Add(this.newSerialNumberField);
			this.newAuthenticatorTab.HorizontalScrollbarBarColor = true;
			this.newAuthenticatorTab.HorizontalScrollbarHighlightOnWheel = false;
			this.newAuthenticatorTab.HorizontalScrollbarSize = 10;
			this.newAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
			this.newAuthenticatorTab.Name = "newAuthenticatorTab";
			this.newAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.newAuthenticatorTab.Size = new System.Drawing.Size(448, 326);
			this.newAuthenticatorTab.TabIndex = 0;
			this.newAuthenticatorTab.Text = "New Authenticator";
			this.newAuthenticatorTab.VerticalScrollbarBarColor = true;
			this.newAuthenticatorTab.VerticalScrollbarHighlightOnWheel = false;
			this.newAuthenticatorTab.VerticalScrollbarSize = 10;
			// 
			// newRestoreCodeField
			// 
			this.newRestoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newRestoreCodeField.Location = new System.Drawing.Point(21, 257);
			this.newRestoreCodeField.Multiline = true;
			this.newRestoreCodeField.Name = "newRestoreCodeField";
			this.newRestoreCodeField.SecretMode = false;
			this.newRestoreCodeField.Size = new System.Drawing.Size(324, 26);
			this.newRestoreCodeField.SpaceOut = 0;
			this.newRestoreCodeField.TabIndex = 3;
			// 
			// newLoginCodeField
			// 
			this.newLoginCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
			this.newLoginCodeField.Location = new System.Drawing.Point(286, 69);
			this.newLoginCodeField.Multiline = true;
			this.newLoginCodeField.Name = "newLoginCodeField";
			this.newLoginCodeField.SecretMode = false;
			this.newLoginCodeField.Size = new System.Drawing.Size(74, 26);
			this.newLoginCodeField.SpaceOut = 4;
			this.newLoginCodeField.TabIndex = 3;
			this.newLoginCodeField.Visible = false;
			// 
			// newSerialNumberField
			// 
			this.newSerialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newSerialNumberField.Location = new System.Drawing.Point(21, 134);
			this.newSerialNumberField.Multiline = true;
			this.newSerialNumberField.Name = "newSerialNumberField";
			this.newSerialNumberField.SecretMode = false;
			this.newSerialNumberField.Size = new System.Drawing.Size(324, 26);
			this.newSerialNumberField.SpaceOut = 0;
			this.newSerialNumberField.TabIndex = 1;
			// 
			// recoverAuthenticatorTab
			// 
			this.recoverAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
			this.recoverAuthenticatorTab.Controls.Add(this.restoreAnswer2Field);
			this.recoverAuthenticatorTab.Controls.Add(this.label9);
			this.recoverAuthenticatorTab.Controls.Add(this.restorePasswordField);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreAnswer1Field);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreDeviceIdField);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreEmailField);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreGetQuestionsButton);
			this.recoverAuthenticatorTab.Controls.Add(this.label5);
			this.recoverAuthenticatorTab.Controls.Add(this.label7);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreQuestion1Label);
			this.recoverAuthenticatorTab.Controls.Add(this.label12);
			this.recoverAuthenticatorTab.Controls.Add(this.label4);
			this.recoverAuthenticatorTab.Controls.Add(this.label13);
			this.recoverAuthenticatorTab.Controls.Add(this.restoreQuestion2Label);
			this.recoverAuthenticatorTab.HorizontalScrollbarBarColor = true;
			this.recoverAuthenticatorTab.HorizontalScrollbarHighlightOnWheel = false;
			this.recoverAuthenticatorTab.HorizontalScrollbarSize = 10;
			this.recoverAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
			this.recoverAuthenticatorTab.Name = "recoverAuthenticatorTab";
			this.recoverAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.recoverAuthenticatorTab.Size = new System.Drawing.Size(448, 326);
			this.recoverAuthenticatorTab.TabIndex = 1;
			this.recoverAuthenticatorTab.Text = "Recover Authenticator";
			this.recoverAuthenticatorTab.VerticalScrollbarBarColor = true;
			this.recoverAuthenticatorTab.VerticalScrollbarHighlightOnWheel = false;
			this.recoverAuthenticatorTab.VerticalScrollbarSize = 10;
			// 
			// trionAuthenticatorRadioButton
			// 
			this.trionAuthenticatorRadioButton.Group = "ICON";
			this.trionAuthenticatorRadioButton.Location = new System.Drawing.Point(304, 122);
			this.trionAuthenticatorRadioButton.Name = "trionAuthenticatorRadioButton";
			this.trionAuthenticatorRadioButton.Size = new System.Drawing.Size(14, 13);
			this.trionAuthenticatorRadioButton.TabIndex = 3;
			this.trionAuthenticatorRadioButton.Tag = "TrionAuthenticatorIcon.png";
			this.trionAuthenticatorRadioButton.UseSelectable = true;
			this.trionAuthenticatorRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// iconTrion
			// 
			this.iconTrion.Image = global::WinAuth.Properties.Resources.TrionAuthenticatorIcon;
			this.iconTrion.Location = new System.Drawing.Point(324, 107);
			this.iconTrion.Name = "iconTrion";
			this.iconTrion.Size = new System.Drawing.Size(48, 48);
			this.iconTrion.TabIndex = 4;
			this.iconTrion.TabStop = false;
			this.iconTrion.Tag = "TrionAuthenticatorIcon.png";
			this.iconTrion.Click += new System.EventHandler(this.trionIcon_Click);
			// 
			// archeageIcon
			// 
			this.archeageIcon.Image = global::WinAuth.Properties.Resources.ArcheAgeIcon;
			this.archeageIcon.Location = new System.Drawing.Point(252, 107);
			this.archeageIcon.Name = "archeageIcon";
			this.archeageIcon.Size = new System.Drawing.Size(48, 48);
			this.archeageIcon.TabIndex = 4;
			this.archeageIcon.TabStop = false;
			this.archeageIcon.Tag = "ArcheAgeIcon.png";
			this.archeageIcon.Click += new System.EventHandler(this.iconArcheAge_Click);
			// 
			// iconRift
			// 
			this.iconRift.Image = global::WinAuth.Properties.Resources.RiftIcon;
			this.iconRift.Location = new System.Drawing.Point(176, 107);
			this.iconRift.Name = "iconRift";
			this.iconRift.Size = new System.Drawing.Size(48, 48);
			this.iconRift.TabIndex = 4;
			this.iconRift.TabStop = false;
			this.iconRift.Tag = "RiftIcon.png";
			this.iconRift.Click += new System.EventHandler(this.iconRift_Click);
			// 
			// iconGlyph
			// 
			this.iconGlyph.Image = global::WinAuth.Properties.Resources.GlyphIcon;
			this.iconGlyph.Location = new System.Drawing.Point(100, 107);
			this.iconGlyph.Name = "iconGlyph";
			this.iconGlyph.Size = new System.Drawing.Size(48, 48);
			this.iconGlyph.TabIndex = 4;
			this.iconGlyph.TabStop = false;
			this.iconGlyph.Tag = "GlyphIcon.png";
			this.iconGlyph.Click += new System.EventHandler(this.iconGlyph_Click);
			// 
			// AddTrionAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.BorderStyle = MetroFramework.Forms.MetroFormBorderStyle.FixedSingle;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(502, 585);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.trionAuthenticatorRadioButton);
			this.Controls.Add(this.archeageIconRadioButton);
			this.Controls.Add(this.riftIconRadioButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.glyphIconRadioButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.iconTrion);
			this.Controls.Add(this.archeageIcon);
			this.Controls.Add(this.label6);
			this.Controls.Add(this.iconRift);
			this.Controls.Add(this.nameField);
			this.Controls.Add(this.iconGlyph);
			this.Controls.Add(this.label10);
			this.MaximizeBox = false;
			this.Name = "AddTrionAuthenticator";
			this.Resizable = false;
			this.ShowIcon = false;
			this.Text = "Add Glyph Authenticator";
			this.Load += new System.EventHandler(this.AddTrionAuthenticator_Load);
			this.tabControl1.ResumeLayout(false);
			this.newAuthenticatorTab.ResumeLayout(false);
			this.newAuthenticatorTab.PerformLayout();
			this.recoverAuthenticatorTab.ResumeLayout(false);
			this.recoverAuthenticatorTab.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.iconTrion)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.archeageIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iconRift)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iconGlyph)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroLabel label1;
		private WinAuth.SecretTextBox newRestoreCodeField;
		private WinAuth.SecretTextBox newLoginCodeField;
		private WinAuth.SecretTextBox newSerialNumberField;
		private MetroFramework.Controls.MetroButton enrollAuthenticatorButton;
		private MetroFramework.Controls.MetroLabel label3;
		private MetroFramework.Controls.MetroLabel label2;
		private MetroFramework.Controls.MetroCheckBox allowCopyNewButton;
		private MetroFramework.Controls.MetroLabel label7;
		private MetroFramework.Controls.MetroTextBox restoreDeviceIdField;
		private MetroFramework.Controls.MetroLabel label9;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroButton cancelButton;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private MetroFramework.Controls.MetroLabel label13;
		private MetroFramework.Controls.MetroLabel label12;
		private MetroFramework.Controls.MetroTextBox restorePasswordField;
		private MetroFramework.Controls.MetroTextBox restoreEmailField;
		private MetroFramework.Controls.MetroLabel restoreQuestion1Label;
		private MetroFramework.Controls.MetroTextBox restoreAnswer1Field;
		private MetroFramework.Controls.MetroTextBox restoreAnswer2Field;
		private MetroFramework.Controls.MetroLabel restoreQuestion2Label;
		private MetroFramework.Controls.MetroButton restoreGetQuestionsButton;
		private MetroFramework.Controls.MetroLabel label8;
		private MetroFramework.Controls.MetroLabel label5;
		private MetroFramework.Controls.MetroLabel label4;
		private WinAuth.GroupMetroRadioButton archeageIconRadioButton;
		private WinAuth.GroupMetroRadioButton riftIconRadioButton;
		private WinAuth.GroupMetroRadioButton glyphIconRadioButton;
		private System.Windows.Forms.PictureBox archeageIcon;
		private System.Windows.Forms.PictureBox iconRift;
		private System.Windows.Forms.PictureBox iconGlyph;
		private MetroFramework.Controls.MetroLabel label6;
		private MetroFramework.Controls.MetroTextBox nameField;
		private MetroFramework.Controls.MetroLabel label10;
		private MetroFramework.Controls.MetroTabControl tabControl1;
		private MetroFramework.Controls.MetroTabPage newAuthenticatorTab;
		private MetroFramework.Controls.MetroTabPage recoverAuthenticatorTab;
		private System.Windows.Forms.PictureBox iconTrion;
		private GroupMetroRadioButton trionAuthenticatorRadioButton;
	}
}