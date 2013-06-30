namespace WinAuth
{
	partial class AddBattleNetAuthenticator
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(AddBattleNetAuthenticator));
			this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
			this.allowCopyNewButton = new System.Windows.Forms.CheckBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label5 = new System.Windows.Forms.Label();
			this.enrollAuthenticatorButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.restoreRestoreCodeField = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.restoreSerialNumberField = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.importPrivateKeyField = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.icon3RadioButton = new WinAuth.GroupRadioButton();
			this.icon2RadioButton = new WinAuth.GroupRadioButton();
			this.icon1RadioButton = new WinAuth.GroupRadioButton();
			this.icon3 = new System.Windows.Forms.PictureBox();
			this.icon2 = new System.Windows.Forms.PictureBox();
			this.icon1 = new System.Windows.Forms.PictureBox();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.tabControl1 = new System.Windows.Forms.TabControl();
			this.tabPage1 = new System.Windows.Forms.TabPage();
			this.newRestoreCodeField = new WinAuth.SecretTextBox();
			this.newLoginCodeField = new WinAuth.SecretTextBox();
			this.newSerialNumberField = new WinAuth.SecretTextBox();
			this.restoreAuthenticatorTab = new System.Windows.Forms.TabPage();
			this.importAuthenticatorTab = new System.Windows.Forms.TabPage();
			this.label13 = new System.Windows.Forms.Label();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon3)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
			this.tabControl1.SuspendLayout();
			this.tabPage1.SuspendLayout();
			this.restoreAuthenticatorTab.SuspendLayout();
			this.importAuthenticatorTab.SuspendLayout();
			this.SuspendLayout();
			// 
			// newAuthenticatorProgress
			// 
			this.newAuthenticatorProgress.Location = new System.Drawing.Point(121, 205);
			this.newAuthenticatorProgress.Maximum = 30;
			this.newAuthenticatorProgress.Minimum = 1;
			this.newAuthenticatorProgress.Name = "newAuthenticatorProgress";
			this.newAuthenticatorProgress.Size = new System.Drawing.Size(158, 8);
			this.newAuthenticatorProgress.Style = System.Windows.Forms.ProgressBarStyle.Continuous;
			this.newAuthenticatorProgress.TabIndex = 5;
			this.newAuthenticatorProgress.Value = 1;
			this.newAuthenticatorProgress.Visible = false;
			// 
			// allowCopyNewButton
			// 
			this.allowCopyNewButton.AutoSize = true;
			this.allowCopyNewButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.allowCopyNewButton.Location = new System.Drawing.Point(285, 154);
			this.allowCopyNewButton.Name = "allowCopyNewButton";
			this.allowCopyNewButton.Size = new System.Drawing.Size(83, 17);
			this.allowCopyNewButton.TabIndex = 2;
			this.allowCopyNewButton.Text = "Allow copy?";
			this.allowCopyNewButton.UseVisualStyleBackColor = true;
			this.allowCopyNewButton.CheckedChanged += new System.EventHandler(this.allowCopyNewButton_CheckedChanged);
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(18, 221);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(95, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "Restore Code:";
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(20, 181);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "Login Code:";
			// 
			// enrollAuthenticatorButton
			// 
			this.enrollAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.enrollAuthenticatorButton.Location = new System.Drawing.Point(64, 89);
			this.enrollAuthenticatorButton.Name = "enrollAuthenticatorButton";
			this.enrollAuthenticatorButton.Size = new System.Drawing.Size(215, 24);
			this.enrollAuthenticatorButton.TabIndex = 0;
			this.enrollAuthenticatorButton.Text = "Create Authenticator";
			this.enrollAuthenticatorButton.UseVisualStyleBackColor = true;
			this.enrollAuthenticatorButton.Click += new System.EventHandler(this.enrollAuthenticatorButton_Click);
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(18, 153);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Serial Number:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(7, 93);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(51, 25);
			this.label3.TabIndex = 1;
			this.label3.Text = "4. Click";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(7, 122);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(332, 25);
			this.label2.TabIndex = 1;
			this.label2.Text = "5. You will need to add the following details to your account.";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(7, 14);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(428, 72);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// restoreRestoreCodeField
			// 
			this.restoreRestoreCodeField.Location = new System.Drawing.Point(126, 89);
			this.restoreRestoreCodeField.Name = "restoreRestoreCodeField";
			this.restoreRestoreCodeField.Size = new System.Drawing.Size(158, 22);
			this.restoreRestoreCodeField.TabIndex = 1;
			// 
			// label7
			// 
			this.label7.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label7.AutoSize = true;
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(23, 62);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(97, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Serial Number:";
			// 
			// restoreSerialNumberField
			// 
			this.restoreSerialNumberField.Location = new System.Drawing.Point(126, 61);
			this.restoreSerialNumberField.Name = "restoreSerialNumberField";
			this.restoreSerialNumberField.Size = new System.Drawing.Size(158, 22);
			this.restoreSerialNumberField.TabIndex = 0;
			// 
			// label8
			// 
			this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label8.AutoSize = true;
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(23, 90);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(95, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "Restore Code:";
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(6, 17);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(436, 41);
			this.label9.TabIndex = 1;
			this.label9.Text = "Enter your Serial Number and Restore Code from your previous authenticator.";
			// 
			// importPrivateKeyField
			// 
			this.importPrivateKeyField.Location = new System.Drawing.Point(109, 64);
			this.importPrivateKeyField.Name = "importPrivateKeyField";
			this.importPrivateKeyField.Size = new System.Drawing.Size(326, 22);
			this.importPrivateKeyField.TabIndex = 0;
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(24, 67);
			this.label11.Name = "label11";
			this.label11.Size = new System.Drawing.Size(79, 16);
			this.label11.TabIndex = 1;
			this.label11.Text = "Private Key:";
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.Location = new System.Drawing.Point(310, 416);
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
			this.cancelButton.Location = new System.Drawing.Point(391, 416);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Interval = 500;
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.icon3RadioButton);
			this.groupBox1.Controls.Add(this.icon2RadioButton);
			this.groupBox1.Controls.Add(this.icon1RadioButton);
			this.groupBox1.Controls.Add(this.icon3);
			this.groupBox1.Controls.Add(this.icon2);
			this.groupBox1.Controls.Add(this.icon1);
			this.groupBox1.Controls.Add(this.label10);
			this.groupBox1.Controls.Add(this.label12);
			this.groupBox1.Controls.Add(this.nameField);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(456, 105);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// icon3RadioButton
			// 
			this.icon3RadioButton.AutoSize = true;
			this.icon3RadioButton.Group = "ICON";
			this.icon3RadioButton.Location = new System.Drawing.Point(220, 59);
			this.icon3RadioButton.Name = "icon3RadioButton";
			this.icon3RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon3RadioButton.TabIndex = 3;
			this.icon3RadioButton.Tag = "DiabloIcon.png";
			this.icon3RadioButton.UseVisualStyleBackColor = true;
			this.icon3RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon2RadioButton
			// 
			this.icon2RadioButton.AutoSize = true;
			this.icon2RadioButton.Group = "ICON";
			this.icon2RadioButton.Location = new System.Drawing.Point(144, 59);
			this.icon2RadioButton.Name = "icon2RadioButton";
			this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon2RadioButton.TabIndex = 2;
			this.icon2RadioButton.Tag = "WarcraftIcon.png";
			this.icon2RadioButton.UseVisualStyleBackColor = true;
			this.icon2RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon1RadioButton
			// 
			this.icon1RadioButton.AutoSize = true;
			this.icon1RadioButton.Checked = true;
			this.icon1RadioButton.Group = "ICON";
			this.icon1RadioButton.Location = new System.Drawing.Point(68, 59);
			this.icon1RadioButton.Name = "icon1RadioButton";
			this.icon1RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon1RadioButton.TabIndex = 1;
			this.icon1RadioButton.TabStop = true;
			this.icon1RadioButton.Tag = "BattleNetAuthenticatorIcon.png";
			this.icon1RadioButton.UseVisualStyleBackColor = true;
			this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
			// 
			// icon3
			// 
			this.icon3.Image = global::WinAuth.Properties.Resources.DiabloIcon;
			this.icon3.Location = new System.Drawing.Point(240, 44);
			this.icon3.Name = "icon3";
			this.icon3.Size = new System.Drawing.Size(48, 48);
			this.icon3.TabIndex = 4;
			this.icon3.TabStop = false;
			this.icon3.Tag = "DiabloIcon.png";
			this.icon3.Click += new System.EventHandler(this.icon3_Click);
			// 
			// icon2
			// 
			this.icon2.Image = global::WinAuth.Properties.Resources.WarcraftIcon;
			this.icon2.Location = new System.Drawing.Point(164, 44);
			this.icon2.Name = "icon2";
			this.icon2.Size = new System.Drawing.Size(48, 48);
			this.icon2.TabIndex = 4;
			this.icon2.TabStop = false;
			this.icon2.Tag = "WarcraftIcon.png";
			this.icon2.Click += new System.EventHandler(this.icon2_Click);
			// 
			// icon1
			// 
			this.icon1.Image = global::WinAuth.Properties.Resources.BattleNetAuthenticatorIcon;
			this.icon1.Location = new System.Drawing.Point(88, 44);
			this.icon1.Name = "icon1";
			this.icon1.Size = new System.Drawing.Size(48, 48);
			this.icon1.TabIndex = 4;
			this.icon1.TabStop = false;
			this.icon1.Tag = "BattleNetAuthenticatorIcon.png";
			this.icon1.Click += new System.EventHandler(this.icon1_Click);
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
			// label12
			// 
			this.label12.AutoSize = true;
			this.label12.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label12.Location = new System.Drawing.Point(11, 19);
			this.label12.Name = "label12";
			this.label12.Size = new System.Drawing.Size(48, 16);
			this.label12.TabIndex = 3;
			this.label12.Text = "Name:";
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
			this.tabControl1.Controls.Add(this.tabPage1);
			this.tabControl1.Controls.Add(this.restoreAuthenticatorTab);
			this.tabControl1.Controls.Add(this.importAuthenticatorTab);
			this.tabControl1.DrawMode = System.Windows.Forms.TabDrawMode.OwnerDrawFixed;
			this.tabControl1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.tabControl1.Location = new System.Drawing.Point(12, 123);
			this.tabControl1.Name = "tabControl1";
			this.tabControl1.SelectedIndex = 0;
			this.tabControl1.Size = new System.Drawing.Size(456, 283);
			this.tabControl1.TabIndex = 1;
			this.tabControl1.DrawItem += new System.Windows.Forms.DrawItemEventHandler(this.tabControl1_DrawItem);
			// 
			// tabPage1
			// 
			this.tabPage1.BackColor = System.Drawing.SystemColors.Control;
			this.tabPage1.Controls.Add(this.newAuthenticatorProgress);
			this.tabPage1.Controls.Add(this.label1);
			this.tabPage1.Controls.Add(this.allowCopyNewButton);
			this.tabPage1.Controls.Add(this.label2);
			this.tabPage1.Controls.Add(this.newRestoreCodeField);
			this.tabPage1.Controls.Add(this.label3);
			this.tabPage1.Controls.Add(this.newLoginCodeField);
			this.tabPage1.Controls.Add(this.label4);
			this.tabPage1.Controls.Add(this.label6);
			this.tabPage1.Controls.Add(this.enrollAuthenticatorButton);
			this.tabPage1.Controls.Add(this.newSerialNumberField);
			this.tabPage1.Controls.Add(this.label5);
			this.tabPage1.Location = new System.Drawing.Point(4, 25);
			this.tabPage1.Name = "tabPage1";
			this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
			this.tabPage1.Size = new System.Drawing.Size(448, 254);
			this.tabPage1.TabIndex = 0;
			this.tabPage1.Text = "New Authenticator";
			// 
			// newRestoreCodeField
			// 
			this.newRestoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newRestoreCodeField.Location = new System.Drawing.Point(121, 216);
			this.newRestoreCodeField.Multiline = true;
			this.newRestoreCodeField.Name = "newRestoreCodeField";
			this.newRestoreCodeField.SecretMode = false;
			this.newRestoreCodeField.Size = new System.Drawing.Size(158, 26);
			this.newRestoreCodeField.SpaceOut = 2;
			this.newRestoreCodeField.TabIndex = 4;
			// 
			// newLoginCodeField
			// 
			this.newLoginCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newLoginCodeField.Location = new System.Drawing.Point(121, 177);
			this.newLoginCodeField.Multiline = true;
			this.newLoginCodeField.Name = "newLoginCodeField";
			this.newLoginCodeField.SecretMode = false;
			this.newLoginCodeField.Size = new System.Drawing.Size(158, 26);
			this.newLoginCodeField.SpaceOut = 4;
			this.newLoginCodeField.TabIndex = 3;
			// 
			// newSerialNumberField
			// 
			this.newSerialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newSerialNumberField.Location = new System.Drawing.Point(121, 148);
			this.newSerialNumberField.Multiline = true;
			this.newSerialNumberField.Name = "newSerialNumberField";
			this.newSerialNumberField.SecretMode = false;
			this.newSerialNumberField.Size = new System.Drawing.Size(158, 26);
			this.newSerialNumberField.SpaceOut = 0;
			this.newSerialNumberField.TabIndex = 1;
			// 
			// restoreAuthenticatorTab
			// 
			this.restoreAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
			this.restoreAuthenticatorTab.Controls.Add(this.restoreRestoreCodeField);
			this.restoreAuthenticatorTab.Controls.Add(this.label9);
			this.restoreAuthenticatorTab.Controls.Add(this.label7);
			this.restoreAuthenticatorTab.Controls.Add(this.label8);
			this.restoreAuthenticatorTab.Controls.Add(this.restoreSerialNumberField);
			this.restoreAuthenticatorTab.Location = new System.Drawing.Point(4, 25);
			this.restoreAuthenticatorTab.Name = "restoreAuthenticatorTab";
			this.restoreAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.restoreAuthenticatorTab.Size = new System.Drawing.Size(448, 254);
			this.restoreAuthenticatorTab.TabIndex = 1;
			this.restoreAuthenticatorTab.Text = "Restore Authenticator";
			// 
			// importAuthenticatorTab
			// 
			this.importAuthenticatorTab.BackColor = System.Drawing.SystemColors.Control;
			this.importAuthenticatorTab.Controls.Add(this.label13);
			this.importAuthenticatorTab.Controls.Add(this.importPrivateKeyField);
			this.importAuthenticatorTab.Controls.Add(this.label11);
			this.importAuthenticatorTab.Location = new System.Drawing.Point(4, 25);
			this.importAuthenticatorTab.Name = "importAuthenticatorTab";
			this.importAuthenticatorTab.Padding = new System.Windows.Forms.Padding(3);
			this.importAuthenticatorTab.Size = new System.Drawing.Size(448, 254);
			this.importAuthenticatorTab.TabIndex = 2;
			this.importAuthenticatorTab.Text = "Import Authenticator";
			// 
			// label13
			// 
			this.label13.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label13.Location = new System.Drawing.Point(6, 17);
			this.label13.Name = "label13";
			this.label13.Size = new System.Drawing.Size(436, 41);
			this.label13.TabIndex = 2;
			this.label13.Text = "Enter the private key that has been exported from another authenticator program.";
			// 
			// AddBattleNetAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(478, 451);
			this.Controls.Add(this.tabControl1);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddBattleNetAuthenticator";
			this.ShowIcon = false;
			this.Text = "Add Battle.net Authenticator";
			this.Load += new System.EventHandler(this.AddBattleNetAuthenticator_Load);
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon3)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
			this.tabControl1.ResumeLayout(false);
			this.tabPage1.ResumeLayout(false);
			this.tabPage1.PerformLayout();
			this.restoreAuthenticatorTab.ResumeLayout(false);
			this.restoreAuthenticatorTab.PerformLayout();
			this.importAuthenticatorTab.ResumeLayout(false);
			this.importAuthenticatorTab.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Label label1;
		private WinAuth.SecretTextBox newRestoreCodeField;
		private WinAuth.SecretTextBox newLoginCodeField;
		private System.Windows.Forms.Label label6;
		private WinAuth.SecretTextBox newSerialNumberField;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button enrollAuthenticatorButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.CheckBox allowCopyNewButton;
		private System.Windows.Forms.TextBox restoreRestoreCodeField;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.TextBox restoreSerialNumberField;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
		private System.Windows.Forms.TextBox importPrivateKeyField;
		private System.Windows.Forms.Label label11;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
		private System.Windows.Forms.GroupBox groupBox1;
		private GroupRadioButton icon3RadioButton;
		private GroupRadioButton icon2RadioButton;
		private GroupRadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon3;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox nameField;
		private System.Windows.Forms.TabControl tabControl1;
		private System.Windows.Forms.TabPage tabPage1;
		private System.Windows.Forms.TabPage restoreAuthenticatorTab;
		private System.Windows.Forms.TabPage importAuthenticatorTab;
		private System.Windows.Forms.Label label13;
	}
}