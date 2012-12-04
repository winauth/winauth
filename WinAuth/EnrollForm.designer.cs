namespace WindowsAuthenticator
{
	partial class EnrollForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(EnrollForm));
			this.gw2Label1 = new System.Windows.Forms.Label();
			this.btnCancel = new System.Windows.Forms.Button();
			this.btnBattlenet = new System.Windows.Forms.Button();
			this.btnGuildwars = new System.Windows.Forms.Button();
			this.gw2Group = new System.Windows.Forms.GroupBox();
			this.gw2copyCheckbox = new System.Windows.Forms.CheckBox();
			this.gw2Code = new WindowsAuthenticator.SecretTextBox();
			this.gw2ProgressBar = new System.Windows.Forms.ProgressBar();
			this.gw2Finish = new System.Windows.Forms.Button();
			this.gw2GenerateCodeButton = new System.Windows.Forms.Button();
			this.gw2SecretCode = new System.Windows.Forms.TextBox();
			this.label9 = new System.Windows.Forms.Label();
			this.label8 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.gw2Label2 = new System.Windows.Forms.Label();
			this.refreshTimer = new System.Windows.Forms.Timer(this.components);
			this.bnGroup = new System.Windows.Forms.GroupBox();
			this.bnAllowCopy = new System.Windows.Forms.CheckBox();
			this.bnProgressBar = new System.Windows.Forms.ProgressBar();
			this.bnCode = new WindowsAuthenticator.SecretTextBox();
			this.bnRestoreCode = new WindowsAuthenticator.SecretTextBox();
			this.bnSerial = new WindowsAuthenticator.SecretTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.label7 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.gnRegister = new System.Windows.Forms.Button();
			this.label5 = new System.Windows.Forms.Label();
			this.label4 = new System.Windows.Forms.Label();
			this.bnFinish = new System.Windows.Forms.Button();
			this.bnLabel1 = new System.Windows.Forms.Label();
			this.gw2Group.SuspendLayout();
			this.bnGroup.SuspendLayout();
			this.SuspendLayout();
			// 
			// gw2Label1
			// 
			this.gw2Label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gw2Label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gw2Label1.Location = new System.Drawing.Point(6, 16);
			this.gw2Label1.Name = "gw2Label1";
			this.gw2Label1.Size = new System.Drawing.Size(386, 86);
			this.gw2Label1.TabIndex = 4;
			this.gw2Label1.Text = resources.GetString("gw2Label1.Text");
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.Location = new System.Drawing.Point(759, 514);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 4;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
			// 
			// btnBattlenet
			// 
			this.btnBattlenet.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnBattlenet.BackgroundImage")));
			this.btnBattlenet.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.btnBattlenet.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnBattlenet.Location = new System.Drawing.Point(13, 12);
			this.btnBattlenet.Name = "btnBattlenet";
			this.btnBattlenet.Size = new System.Drawing.Size(398, 108);
			this.btnBattlenet.TabIndex = 0;
			this.btnBattlenet.UseVisualStyleBackColor = true;
			this.btnBattlenet.Click += new System.EventHandler(this.btnBattlenet_Click);
			// 
			// btnGuildwars
			// 
			this.btnGuildwars.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("btnGuildwars.BackgroundImage")));
			this.btnGuildwars.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
			this.btnGuildwars.Cursor = System.Windows.Forms.Cursors.Hand;
			this.btnGuildwars.Location = new System.Drawing.Point(435, 12);
			this.btnGuildwars.Name = "btnGuildwars";
			this.btnGuildwars.Size = new System.Drawing.Size(398, 108);
			this.btnGuildwars.TabIndex = 1;
			this.btnGuildwars.UseVisualStyleBackColor = true;
			this.btnGuildwars.Click += new System.EventHandler(this.btnGuildwars_Click);
			// 
			// gw2Group
			// 
			this.gw2Group.Controls.Add(this.gw2copyCheckbox);
			this.gw2Group.Controls.Add(this.gw2Code);
			this.gw2Group.Controls.Add(this.gw2ProgressBar);
			this.gw2Group.Controls.Add(this.gw2Finish);
			this.gw2Group.Controls.Add(this.gw2GenerateCodeButton);
			this.gw2Group.Controls.Add(this.gw2SecretCode);
			this.gw2Group.Controls.Add(this.label9);
			this.gw2Group.Controls.Add(this.label8);
			this.gw2Group.Controls.Add(this.label1);
			this.gw2Group.Controls.Add(this.gw2Label2);
			this.gw2Group.Controls.Add(this.gw2Label1);
			this.gw2Group.Location = new System.Drawing.Point(435, 126);
			this.gw2Group.Name = "gw2Group";
			this.gw2Group.Size = new System.Drawing.Size(398, 335);
			this.gw2Group.TabIndex = 3;
			this.gw2Group.TabStop = false;
			this.gw2Group.Visible = false;
			// 
			// gw2copyCheckbox
			// 
			this.gw2copyCheckbox.AutoSize = true;
			this.gw2copyCheckbox.Location = new System.Drawing.Point(311, 197);
			this.gw2copyCheckbox.Name = "gw2copyCheckbox";
			this.gw2copyCheckbox.Size = new System.Drawing.Size(82, 17);
			this.gw2copyCheckbox.TabIndex = 3;
			this.gw2copyCheckbox.Text = "allow copy?";
			this.gw2copyCheckbox.UseVisualStyleBackColor = true;
			this.gw2copyCheckbox.CheckedChanged += new System.EventHandler(this.gw2copyCheckbox_CheckedChanged);
			// 
			// gw2Code
			// 
			this.gw2Code.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gw2Code.Location = new System.Drawing.Point(118, 191);
			this.gw2Code.Multiline = true;
			this.gw2Code.Name = "gw2Code";
			this.gw2Code.SecretMode = true;
			this.gw2Code.Size = new System.Drawing.Size(187, 30);
			this.gw2Code.SpaceOut = 0;
			this.gw2Code.TabIndex = 2;
			this.gw2Code.Text = null;
			this.gw2Code.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// gw2ProgressBar
			// 
			this.gw2ProgressBar.Location = new System.Drawing.Point(118, 227);
			this.gw2ProgressBar.Maximum = 29;
			this.gw2ProgressBar.Name = "gw2ProgressBar";
			this.gw2ProgressBar.Size = new System.Drawing.Size(187, 11);
			this.gw2ProgressBar.Step = 1;
			this.gw2ProgressBar.TabIndex = 7;
			// 
			// gw2Finish
			// 
			this.gw2Finish.Location = new System.Drawing.Point(118, 294);
			this.gw2Finish.Name = "gw2Finish";
			this.gw2Finish.Size = new System.Drawing.Size(70, 22);
			this.gw2Finish.TabIndex = 4;
			this.gw2Finish.Text = "Save";
			this.gw2Finish.UseVisualStyleBackColor = true;
			this.gw2Finish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// gw2GenerateCodeButton
			// 
			this.gw2GenerateCodeButton.Location = new System.Drawing.Point(118, 131);
			this.gw2GenerateCodeButton.Name = "gw2GenerateCodeButton";
			this.gw2GenerateCodeButton.Size = new System.Drawing.Size(130, 22);
			this.gw2GenerateCodeButton.TabIndex = 1;
			this.gw2GenerateCodeButton.Text = "Create Authenticator";
			this.gw2GenerateCodeButton.UseVisualStyleBackColor = true;
			this.gw2GenerateCodeButton.Click += new System.EventHandler(this.gw2GenerateCodeButton_Click);
			// 
			// gw2SecretCode
			// 
			this.gw2SecretCode.Location = new System.Drawing.Point(118, 105);
			this.gw2SecretCode.Name = "gw2SecretCode";
			this.gw2SecretCode.Size = new System.Drawing.Size(259, 20);
			this.gw2SecretCode.TabIndex = 0;
			// 
			// label9
			// 
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(19, 196);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(56, 19);
			this.label9.TabIndex = 9;
			this.label9.Text = "Code:";
			// 
			// label8
			// 
			this.label8.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label8.Location = new System.Drawing.Point(19, 106);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(93, 19);
			this.label8.TabIndex = 9;
			this.label8.Text = "Secret Code:";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 252);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(386, 39);
			this.label1.TabIndex = 4;
			this.label1.Text = "7. IMPORTANT: Write down your Secret Code and store it somewhere safe. Click Save" +
    ".";
			// 
			// gw2Label2
			// 
			this.gw2Label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.gw2Label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.gw2Label2.Location = new System.Drawing.Point(6, 169);
			this.gw2Label2.Name = "gw2Label2";
			this.gw2Label2.Size = new System.Drawing.Size(386, 28);
			this.gw2Label2.TabIndex = 4;
			this.gw2Label2.Text = "6. Enter the following code to verify it is working";
			// 
			// refreshTimer
			// 
			this.refreshTimer.Interval = 500;
			this.refreshTimer.Tick += new System.EventHandler(this.refreshTimer_Tick);
			// 
			// bnGroup
			// 
			this.bnGroup.Controls.Add(this.bnAllowCopy);
			this.bnGroup.Controls.Add(this.bnProgressBar);
			this.bnGroup.Controls.Add(this.bnCode);
			this.bnGroup.Controls.Add(this.bnRestoreCode);
			this.bnGroup.Controls.Add(this.bnSerial);
			this.bnGroup.Controls.Add(this.label6);
			this.bnGroup.Controls.Add(this.label7);
			this.bnGroup.Controls.Add(this.label3);
			this.bnGroup.Controls.Add(this.label2);
			this.bnGroup.Controls.Add(this.gnRegister);
			this.bnGroup.Controls.Add(this.label5);
			this.bnGroup.Controls.Add(this.label4);
			this.bnGroup.Controls.Add(this.bnFinish);
			this.bnGroup.Controls.Add(this.bnLabel1);
			this.bnGroup.Location = new System.Drawing.Point(13, 126);
			this.bnGroup.Name = "bnGroup";
			this.bnGroup.Size = new System.Drawing.Size(398, 377);
			this.bnGroup.TabIndex = 2;
			this.bnGroup.TabStop = false;
			this.bnGroup.Visible = false;
			// 
			// bnAllowCopy
			// 
			this.bnAllowCopy.AutoSize = true;
			this.bnAllowCopy.Location = new System.Drawing.Point(312, 180);
			this.bnAllowCopy.Name = "bnAllowCopy";
			this.bnAllowCopy.Size = new System.Drawing.Size(82, 17);
			this.bnAllowCopy.TabIndex = 1;
			this.bnAllowCopy.Text = "allow copy?";
			this.bnAllowCopy.UseVisualStyleBackColor = true;
			this.bnAllowCopy.CheckedChanged += new System.EventHandler(this.bncopyCheckbox_CheckedChanged);
			// 
			// bnProgressBar
			// 
			this.bnProgressBar.Location = new System.Drawing.Point(132, 240);
			this.bnProgressBar.Maximum = 29;
			this.bnProgressBar.Name = "bnProgressBar";
			this.bnProgressBar.Size = new System.Drawing.Size(174, 12);
			this.bnProgressBar.Step = 1;
			this.bnProgressBar.TabIndex = 12;
			// 
			// bnCode
			// 
			this.bnCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bnCode.Location = new System.Drawing.Point(132, 208);
			this.bnCode.Multiline = true;
			this.bnCode.Name = "bnCode";
			this.bnCode.SecretMode = true;
			this.bnCode.Size = new System.Drawing.Size(174, 30);
			this.bnCode.SpaceOut = 4;
			this.bnCode.TabIndex = 2;
			this.bnCode.Text = null;
			this.bnCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// bnRestoreCode
			// 
			this.bnRestoreCode.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bnRestoreCode.Location = new System.Drawing.Point(132, 261);
			this.bnRestoreCode.Multiline = true;
			this.bnRestoreCode.Name = "bnRestoreCode";
			this.bnRestoreCode.SecretMode = true;
			this.bnRestoreCode.Size = new System.Drawing.Size(174, 30);
			this.bnRestoreCode.SpaceOut = 0;
			this.bnRestoreCode.TabIndex = 3;
			this.bnRestoreCode.Text = null;
			this.bnRestoreCode.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// bnSerial
			// 
			this.bnSerial.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bnSerial.Location = new System.Drawing.Point(132, 174);
			this.bnSerial.Multiline = true;
			this.bnSerial.Name = "bnSerial";
			this.bnSerial.SecretMode = true;
			this.bnSerial.Size = new System.Drawing.Size(174, 30);
			this.bnSerial.SpaceOut = 0;
			this.bnSerial.TabIndex = 0;
			this.bnSerial.Text = null;
			this.bnSerial.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(6, 297);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(386, 38);
			this.label6.TabIndex = 4;
			this.label6.Text = "6. IMPORTANT: Write down both the Serial Number and Restore Code and keep them so" +
    "mewhere safe. Click Save.\r\n";
			// 
			// label7
			// 
			this.label7.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label7.Location = new System.Drawing.Point(17, 266);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(109, 19);
			this.label7.TabIndex = 9;
			this.label7.Text = "Restore Code";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(17, 214);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(109, 19);
			this.label3.TabIndex = 9;
			this.label3.Text = "Login Code:";
			// 
			// label2
			// 
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(17, 179);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(109, 19);
			this.label2.TabIndex = 9;
			this.label2.Text = "Serial Number:";
			// 
			// gnRegister
			// 
			this.gnRegister.Location = new System.Drawing.Point(20, 116);
			this.gnRegister.Name = "gnRegister";
			this.gnRegister.Size = new System.Drawing.Size(130, 22);
			this.gnRegister.TabIndex = 8;
			this.gnRegister.Text = "Create Authenticator";
			this.gnRegister.UseVisualStyleBackColor = true;
			this.gnRegister.Click += new System.EventHandler(this.gnRegister_Click);
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(6, 119);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(52, 22);
			this.label5.TabIndex = 4;
			this.label5.Text = "4.\r\n";
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(6, 144);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(386, 22);
			this.label4.TabIndex = 4;
			this.label4.Text = "5. Enter the Serial Number and Login Code into your account:";
			// 
			// bnFinish
			// 
			this.bnFinish.Location = new System.Drawing.Point(132, 338);
			this.bnFinish.Name = "bnFinish";
			this.bnFinish.Size = new System.Drawing.Size(70, 22);
			this.bnFinish.TabIndex = 4;
			this.bnFinish.Text = "Save";
			this.bnFinish.UseVisualStyleBackColor = true;
			this.bnFinish.Click += new System.EventHandler(this.btnFinish_Click);
			// 
			// bnLabel1
			// 
			this.bnLabel1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.bnLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.bnLabel1.Location = new System.Drawing.Point(6, 16);
			this.bnLabel1.Name = "bnLabel1";
			this.bnLabel1.Size = new System.Drawing.Size(386, 100);
			this.bnLabel1.TabIndex = 4;
			this.bnLabel1.Text = resources.GetString("bnLabel1.Text");
			// 
			// EnrollForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(846, 549);
			this.ControlBox = false;
			this.Controls.Add(this.btnGuildwars);
			this.Controls.Add(this.gw2Group);
			this.Controls.Add(this.btnBattlenet);
			this.Controls.Add(this.bnGroup);
			this.Controls.Add(this.btnCancel);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "EnrollForm";
			this.ShowIcon = false;
			this.Text = "New Authenticator";
			this.Load += new System.EventHandler(this.EnrollForm_Load);
			this.gw2Group.ResumeLayout(false);
			this.gw2Group.PerformLayout();
			this.bnGroup.ResumeLayout(false);
			this.bnGroup.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.Button btnCancel;
		private System.Windows.Forms.Label gw2Label1;
		private System.Windows.Forms.Button btnBattlenet;
		private System.Windows.Forms.Button btnGuildwars;
		private System.Windows.Forms.GroupBox gw2Group;
		private System.Windows.Forms.TextBox gw2SecretCode;
		private System.Windows.Forms.Label gw2Label2;
		private System.Windows.Forms.Button gw2GenerateCodeButton;
		private System.Windows.Forms.ProgressBar gw2ProgressBar;
		private SecretTextBox gw2Code;
		private System.Windows.Forms.Timer refreshTimer;
		private System.Windows.Forms.CheckBox gw2copyCheckbox;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button gw2Finish;
		private System.Windows.Forms.GroupBox bnGroup;
		private System.Windows.Forms.Label bnLabel1;
		private System.Windows.Forms.Button bnFinish;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.ProgressBar bnProgressBar;
		private SecretTextBox bnCode;
		private SecretTextBox bnSerial;
		private System.Windows.Forms.CheckBox bnAllowCopy;
		private System.Windows.Forms.Button gnRegister;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label4;
		private SecretTextBox bnRestoreCode;
		private System.Windows.Forms.Label label7;
		private System.Windows.Forms.Label label6;
		private System.Windows.Forms.Label label8;
		private System.Windows.Forms.Label label9;
	}
}