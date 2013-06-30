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
			this.allowCopyNewButton = new System.Windows.Forms.CheckBox();
			this.enrollAuthenticatorButton = new System.Windows.Forms.Button();
			this.label3 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.restoreAnswer2Field = new System.Windows.Forms.TextBox();
			this.restoreAnswer1Field = new System.Windows.Forms.TextBox();
			this.restoreGetQuestionsButton = new System.Windows.Forms.Button();
			this.restoreQuestion1Label = new System.Windows.Forms.Label();
			this.label13 = new System.Windows.Forms.Label();
			this.restoreQuestion2Label = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.restoreDeviceIdField = new System.Windows.Forms.TextBox();
			this.restorePasswordField = new System.Windows.Forms.TextBox();
			this.restoreEmailField = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.endOfNationsIcon = new System.Windows.Forms.PictureBox();
			this.defianceIcon = new System.Windows.Forms.PictureBox();
			this.iconRift = new System.Windows.Forms.PictureBox();
			this.iconTrion = new System.Windows.Forms.PictureBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label6 = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.newAuthenticatorTab = new System.Windows.Forms.TabPage();
			this.recoverAuthenticatorTab = new System.Windows.Forms.TabPage();
			this.newRestoreCodeField = new WinAuth.SecretTextBox();
			this.newLoginCodeField = new WinAuth.SecretTextBox();
			this.newSerialNumberField = new WinAuth.SecretTextBox();
			this.endOfNationIconRadioButton = new WinAuth.GroupRadioButton();
			this.defianceIconRadioButton = new WinAuth.GroupRadioButton();
			this.riftIconRadioButton = new WinAuth.GroupRadioButton();
			this.trionIconRadioButton = new WinAuth.GroupRadioButton();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.endOfNationsIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.defianceIcon)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iconRift)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.iconTrion)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.newAuthenticatorTab.SuspendLayout();
			this.recoverAuthenticatorTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// allowCopyNewButton
			// 
			this.allowCopyNewButton.AutoSize = true;
			this.allowCopyNewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.allowCopyNewButton.Location = new System.Drawing.Point(299, 138);
			this.allowCopyNewButton.Name = "allowCopyNewButton";
			this.allowCopyNewButton.Size = new System.Drawing.Size(83, 17);
			this.allowCopyNewButton.TabIndex = 2;
			this.allowCopyNewButton.Text = "Allow copy?";
			this.allowCopyNewButton.UseVisualStyleBackColor = true;
			this.allowCopyNewButton.CheckedChanged += new System.EventHandler(this.allowCopyNewButton_CheckedChanged);
			// 
			// enrollAuthenticatorButton
			// 
			this.enrollAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.enrollAuthenticatorButton.Location = new System.Drawing.Point(64, 71);
			this.enrollAuthenticatorButton.Name = "enrollAuthenticatorButton";
			this.enrollAuthenticatorButton.Size = new System.Drawing.Size(177, 24);
			this.enrollAuthenticatorButton.TabIndex = 0;
			this.enrollAuthenticatorButton.Text = "Create Authenticator";
			this.enrollAuthenticatorButton.UseVisualStyleBackColor = true;
			this.enrollAuthenticatorButton.Click += new System.EventHandler(this.enrollAuthenticatorButton_Click);
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(7, 75);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 25);
			this.label3.TabIndex = 1;
			this.label3.Text = "3. Click";
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(7, 175);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(410, 85);
			this.label8.TabIndex = 1;
			this.label8.Text = resources.GetString("label8.Text");
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 106);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(410, 25);
			this.label2.TabIndex = 1;
			this.label2.Text = "4. Add the following Authenticator Serial Key with your secret answers:";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 10);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(416, 58);
			this.label1.TabIndex = 0;
			this.label1.Text = "1. Go to www.trionworlds.com and login to your account.\r\n2. Click the Security ta" +
    "b, and \"Add the RIFT Moble Authenticator\". You must have already added your secr" +
    "et questions and answers.";
			// 
			// restoreAnswer2Field
			// 
			this.restoreAnswer2Field.Location = new System.Drawing.Point(130, 297);
			this.restoreAnswer2Field.Name = "restoreAnswer2Field";
			this.restoreAnswer2Field.Size = new System.Drawing.Size(257, 20);
			this.restoreAnswer2Field.TabIndex = 5;
			// 
			// restoreAnswer1Field
			// 
			this.restoreAnswer1Field.Location = new System.Drawing.Point(130, 252);
			this.restoreAnswer1Field.Name = "restoreAnswer1Field";
			this.restoreAnswer1Field.Size = new System.Drawing.Size(257, 20);
			this.restoreAnswer1Field.TabIndex = 4;
			// 
			// restoreGetQuestionsButton
			// 
			this.restoreGetQuestionsButton.Location = new System.Drawing.Point(130, 100);
			this.restoreGetQuestionsButton.Name = "restoreGetQuestionsButton";
			this.restoreGetQuestionsButton.Size = new System.Drawing.Size(166, 23);
			this.restoreGetQuestionsButton.TabIndex = 2;
			this.restoreGetQuestionsButton.Text = "Get Security Questions";
			this.restoreGetQuestionsButton.UseVisualStyleBackColor = true;
			this.restoreGetQuestionsButton.Click += new System.EventHandler(this.restoreGetQuestionsButton_Click);
			// 
			// restoreQuestion1Label
			// 
			this.restoreQuestion1Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreQuestion1Label.Location = new System.Drawing.Point(25, 232);
			this.restoreQuestion1Label.Name = "restoreQuestion1Label";
			this.restoreQuestion1Label.Size = new System.Drawing.Size(413, 17);
			this.restoreQuestion1Label.TabIndex = 1;
			// 
			// label13
			// 
			this.label13.AutoSize = true;
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(25, 75);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(71, 16);
			this.label13.TabIndex = 1;
			this.label13.Text = "Password:";
			// 
			// restoreQuestion2Label
			// 
			this.restoreQuestion2Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreQuestion2Label.Location = new System.Drawing.Point(25, 277);
			this.restoreQuestion2Label.Name = "restoreQuestion2Label";
			this.restoreQuestion2Label.Size = new System.Drawing.Size(413, 17);
			this.restoreQuestion2Label.TabIndex = 1;
			// 
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(25, 47);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(99, 16);
			this.label12.TabIndex = 1;
			this.label12.Text = "Email Address:";
			// 
			// label5
			// 
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(10, 200);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(198, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "3. Answer your secret questions:";
			// 
			// restoreDeviceIdField
			// 
			this.restoreDeviceIdField.Location = new System.Drawing.Point(130, 164);
			this.restoreDeviceIdField.Name = "restoreDeviceIdField";
			this.restoreDeviceIdField.Size = new System.Drawing.Size(257, 20);
			this.restoreDeviceIdField.TabIndex = 3;
			// 
			// restorePasswordField
			// 
			this.restorePasswordField.Location = new System.Drawing.Point(130, 72);
			this.restorePasswordField.Name = "restorePasswordField";
			this.restorePasswordField.Size = new System.Drawing.Size(257, 20);
			this.restorePasswordField.TabIndex = 1;
			this.restorePasswordField.UseSystemPasswordChar = true;
			// 
			// restoreEmailField
			// 
			this.restoreEmailField.Location = new System.Drawing.Point(130, 44);
			this.restoreEmailField.Name = "restoreEmailField";
			this.restoreEmailField.Size = new System.Drawing.Size(257, 20);
			this.restoreEmailField.TabIndex = 0;
			// 
			// label7
			// 
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(25, 167);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(70, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Device ID:";
			// 
			// label4
			// 
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(10, 136);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(420, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "2. Enter the Device ID you saved when you created your authenticator.";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(7, 13);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(289, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "1. Enter your Trion account email and password";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.Location = new System.Drawing.Point(309, 512);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 2;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(390, 512);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Enabled = true;
			this.newAuthenticatorTimer.Interval = 500;
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.endOfNationIconRadioButton);
			this.groupBox1.Controls.Add(this.defianceIconRadioButton);
			this.groupBox1.Controls.Add(this.riftIconRadioButton);
			this.groupBox1.Controls.Add(this.trionIconRadioButton);
			this.groupBox1.Controls.Add(this.endOfNationsIcon);
			this.groupBox1.Controls.Add(this.defianceIcon);
			this.groupBox1.Controls.Add(this.iconRift);
			this.groupBox1.Controls.Add(this.iconTrion);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label6);
			this.groupBox1.Controls.Add(this.nameField);
			this.groupBox1.Location = new System.Drawing.Point(13, 6);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 105);
			this.groupBox1.TabIndex = 4;
			this.groupBox1.TabStop = false;
			// 
			// endOfNationsIcon
			// 
			this.endOfNationsIcon.Image = global::WinAuth.Properties.Resources.EndOfNationsIcon;
			this.endOfNationsIcon.Location = new System.Drawing.Point(315, 44);
			this.endOfNationsIcon.Name = "endOfNationsIcon";
			this.endOfNationsIcon.Size = new System.Drawing.Size(48, 48);
			this.endOfNationsIcon.TabIndex = 4;
			this.endOfNationsIcon.TabStop = false;
			this.endOfNationsIcon.Tag = "EndOfNationsIcon.png";
			this.endOfNationsIcon.Click += new System.EventHandler(this.endOfNationsIcon_Click);
			// 
			// defianceIcon
			// 
			this.defianceIcon.Image = global::WinAuth.Properties.Resources.DefianceIcon;
			this.defianceIcon.Location = new System.Drawing.Point(240, 44);
			this.defianceIcon.Name = "defianceIcon";
			this.defianceIcon.Size = new System.Drawing.Size(48, 48);
			this.defianceIcon.TabIndex = 4;
			this.defianceIcon.TabStop = false;
			this.defianceIcon.Tag = "DefianceIcon.png";
			this.defianceIcon.Click += new System.EventHandler(this.defianceIcon_Click);
			// 
			// iconRift
			// 
			this.iconRift.Image = global::WinAuth.Properties.Resources.RiftIcon;
			this.iconRift.Location = new System.Drawing.Point(164, 44);
			this.iconRift.Name = "iconRift";
			this.iconRift.Size = new System.Drawing.Size(48, 48);
			this.iconRift.TabIndex = 4;
			this.iconRift.TabStop = false;
			this.iconRift.Tag = "RiftIcon.png";
			this.iconRift.Click += new System.EventHandler(this.iconRift_Click);
			// 
			// iconTrion
			// 
			this.iconTrion.Image = global::WinAuth.Properties.Resources.TrionAuthenticatorIcon;
			this.iconTrion.Location = new System.Drawing.Point(88, 44);
			this.iconTrion.Name = "iconTrion";
			this.iconTrion.Size = new System.Drawing.Size(48, 48);
			this.iconTrion.TabIndex = 4;
			this.iconTrion.TabStop = false;
			this.iconTrion.Tag = "TrionAuthenticatorIcon.png";
			this.iconTrion.Click += new System.EventHandler(this.iconTrion_Click);
			// 
			// label10
			// 
			this.label10.AutoSize = true;
			this.label10.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label10.Location = new System.Drawing.Point(11, 57);
			this.label10.Name = "label10";
			this.label10.Size = new System.Drawing.Size(36, 16);
			this.label10.TabIndex = 3;
			this.label10.Text = "Icon:";
			// 
			// label6
			// 
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(11, 19);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(48, 16);
			this.label6.TabIndex = 3;
			this.label6.Text = "Name:";
			// 
			// nameField
			// 
			this.nameField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.nameField.Location = new System.Drawing.Point(68, 18);
			this.nameField.Name = "nameField";
			this.nameField.Size = new System.Drawing.Size(371, 22);
			this.nameField.TabIndex = 0;
			// 
			// tabControl1
			// 
			this.tabControl1.Controls.Add(this.newAuthenticatorTab);
			this.tabControl1.Controls.Add(this.recoverAuthenticatorTab);
			this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl1.ItemSize = new System.Drawing.Size(120, 18);
			this.tabControl1.Location = new System.Drawing.Point(13, 117);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(456, 382);
			this.tabControl1.SizeMode = System.Windows.Forms.TabSizeMode.Fixed;
			this.tabControl1.TabIndex = 7;
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
			this.newAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
			this.newAuthenticatorTab.Name = "newAuthenticatorTab";
			this.newAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.newAuthenticatorTab.Size = new System.Drawing.Size(448, 356);
			this.newAuthenticatorTab.TabIndex = 0;
			this.newAuthenticatorTab.Text = "New Authenticator";
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
			this.recoverAuthenticatorTab.Location = new System.Drawing.Point(4, 22);
			this.recoverAuthenticatorTab.Name = "recoverAuthenticatorTab";
			this.recoverAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.recoverAuthenticatorTab.Size = new System.Drawing.Size(448, 356);
			this.recoverAuthenticatorTab.TabIndex = 1;
			this.recoverAuthenticatorTab.Text = "Recover Authenticator";
			// 
			// newRestoreCodeField
			// 
			this.newRestoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newRestoreCodeField.Location = new System.Drawing.Point(64, 263);
			this.newRestoreCodeField.Multiline = true;
			this.newRestoreCodeField.Name = "newRestoreCodeField";
			this.newRestoreCodeField.SecretMode = false;
			this.newRestoreCodeField.Size = new System.Drawing.Size(220, 26);
			this.newRestoreCodeField.SpaceOut = 0;
			this.newRestoreCodeField.TabIndex = 3;
			// 
			// newLoginCodeField
			// 
			this.newLoginCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newLoginCodeField.Location = new System.Drawing.Point(347, 75);
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
			this.newSerialNumberField.Location = new System.Drawing.Point(64, 134);
			this.newSerialNumberField.Multiline = true;
			this.newSerialNumberField.Name = "newSerialNumberField";
			this.newSerialNumberField.SecretMode = false;
			this.newSerialNumberField.Size = new System.Drawing.Size(220, 26);
			this.newSerialNumberField.SpaceOut = 0;
			this.newSerialNumberField.TabIndex = 1;
			// 
			// endOfNationIconRadioButton
			// 
			this.endOfNationIconRadioButton.AutoSize = true;
			this.endOfNationIconRadioButton.Group = "ICON";
			this.endOfNationIconRadioButton.Location = new System.Drawing.Point(295, 59);
			this.endOfNationIconRadioButton.Name = "endOfNationIconRadioButton";
			this.endOfNationIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.endOfNationIconRadioButton.TabIndex = 4;
			this.endOfNationIconRadioButton.Tag = "EndOfNationsIcon.png";
			this.endOfNationIconRadioButton.UseVisualStyleBackColor = true;
			this.endOfNationIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// defianceIconRadioButton
			// 
			this.defianceIconRadioButton.AutoSize = true;
			this.defianceIconRadioButton.Group = "ICON";
			this.defianceIconRadioButton.Location = new System.Drawing.Point(220, 59);
			this.defianceIconRadioButton.Name = "defianceIconRadioButton";
			this.defianceIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.defianceIconRadioButton.TabIndex = 3;
			this.defianceIconRadioButton.Tag = "DefianceIcon.png";
			this.defianceIconRadioButton.UseVisualStyleBackColor = true;
			this.defianceIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// riftIconRadioButton
			// 
			this.riftIconRadioButton.AutoSize = true;
			this.riftIconRadioButton.Group = "ICON";
			this.riftIconRadioButton.Location = new System.Drawing.Point(144, 59);
			this.riftIconRadioButton.Name = "riftIconRadioButton";
			this.riftIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.riftIconRadioButton.TabIndex = 2;
			this.riftIconRadioButton.Tag = "RiftIcon.png";
			this.riftIconRadioButton.UseVisualStyleBackColor = true;
			this.riftIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// trionIconRadioButton
			// 
			this.trionIconRadioButton.AutoSize = true;
			this.trionIconRadioButton.Checked = true;
			this.trionIconRadioButton.Group = "ICON";
			this.trionIconRadioButton.Location = new System.Drawing.Point(68, 59);
			this.trionIconRadioButton.Name = "trionIconRadioButton";
			this.trionIconRadioButton.Size = new System.Drawing.Size(14, 13);
			this.trionIconRadioButton.TabIndex = 1;
			this.trionIconRadioButton.TabStop = true;
			this.trionIconRadioButton.Tag = "TrionAuthenticatorIcon.png";
			this.trionIconRadioButton.UseVisualStyleBackColor = true;
			this.trionIconRadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// AddTrionAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(479, 547);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddTrionAuthenticator";
			this.ShowIcon = false;
			this.Text = "Add Trion Authenticator";
			this.Load += new System.EventHandler(this.AddTrionAuthenticator_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.endOfNationsIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.defianceIcon)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iconRift)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.iconTrion)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.newAuthenticatorTab.ResumeLayout(false);
			this.newAuthenticatorTab.PerformLayout();
			this.recoverAuthenticatorTab.ResumeLayout(false);
			this.recoverAuthenticatorTab.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private WinAuth.SecretTextBox newRestoreCodeField;
		private WinAuth.SecretTextBox newLoginCodeField;
		private WinAuth.SecretTextBox newSerialNumberField;
		private System.Windows.Forms.Button enrollAuthenticatorButton;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox allowCopyNewButton;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox restoreDeviceIdField;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private System.Windows.Forms.Label label13;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox restorePasswordField;
		private System.Windows.Forms.TextBox restoreEmailField;
		private System.Windows.Forms.Label restoreQuestion1Label;
		private System.Windows.Forms.TextBox restoreAnswer1Field;
		private System.Windows.Forms.TextBox restoreAnswer2Field;
		private System.Windows.Forms.Label restoreQuestion2Label;
		private System.Windows.Forms.Button restoreGetQuestionsButton;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.GroupBox groupBox1;
		private WinAuth.GroupRadioButton defianceIconRadioButton;
		private WinAuth.GroupRadioButton riftIconRadioButton;
		private WinAuth.GroupRadioButton trionIconRadioButton;
		private System.Windows.Forms.PictureBox defianceIcon;
		private System.Windows.Forms.PictureBox iconRift;
		private System.Windows.Forms.PictureBox iconTrion;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.TextBox nameField;
		private WinAuth.GroupRadioButton endOfNationIconRadioButton;
		private System.Windows.Forms.PictureBox endOfNationsIcon;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage newAuthenticatorTab;
		private System.Windows.Forms.TabPage recoverAuthenticatorTab;
	}
}