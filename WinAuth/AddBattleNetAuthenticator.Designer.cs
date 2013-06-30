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
			this.newAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
			this.allowCopyNewButton = new System.Windows.Forms.CheckBox();
			this.newRestoreCodeField = new WinAuth.SecretTextBox();
			this.newLoginCodeField = new WinAuth.SecretTextBox();
			this.label6 = new System.Windows.Forms.Label();
			this.newSerialNumberField = new WinAuth.SecretTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.enrollAuthenticatorButton = new System.Windows.Forms.Button();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.restoreAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.restoreRestoreCodeField = new System.Windows.Forms.TextBox();
			this.label7 = new System.Windows.Forms.Label();
			this.restoreSerialNumberField = new System.Windows.Forms.TextBox();
			this.label8 = new System.Windows.Forms.Label();
			this.label9 = new System.Windows.Forms.Label();
			this.importAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.importPrivateKeyField = new System.Windows.Forms.TextBox();
			this.label11 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.restoreAuthenticatorButton = new WinAuth.GroupRadioButton();
			this.createAuthenticatorButton = new WinAuth.GroupRadioButton();
			this.importAuthenticatorButton = new WinAuth.GroupRadioButton();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.newAuthenticatorGroup.SuspendLayout();
			this.restoreAuthenticatorGroup.SuspendLayout();
			this.importAuthenticatorGroup.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorGroup
			// 
			this.newAuthenticatorGroup.Controls.Add(this.newAuthenticatorProgress);
			this.newAuthenticatorGroup.Controls.Add(this.allowCopyNewButton);
			this.newAuthenticatorGroup.Controls.Add(this.newRestoreCodeField);
			this.newAuthenticatorGroup.Controls.Add(this.newLoginCodeField);
			this.newAuthenticatorGroup.Controls.Add(this.label6);
			this.newAuthenticatorGroup.Controls.Add(this.newSerialNumberField);
			this.newAuthenticatorGroup.Controls.Add(this.label5);
			this.newAuthenticatorGroup.Controls.Add(this.enrollAuthenticatorButton);
			this.newAuthenticatorGroup.Controls.Add(this.label4);
			this.newAuthenticatorGroup.Controls.Add(this.label3);
			this.newAuthenticatorGroup.Controls.Add(this.label2);
			this.newAuthenticatorGroup.Controls.Add(this.label1);
			this.newAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newAuthenticatorGroup.Location = new System.Drawing.Point(12, 78);
			this.newAuthenticatorGroup.Name = "newAuthenticatorGroup";
			this.newAuthenticatorGroup.Size = new System.Drawing.Size(458, 256);
			this.newAuthenticatorGroup.TabIndex = 1;
			this.newAuthenticatorGroup.TabStop = false;
			// 
			// newAuthenticatorProgress
			// 
			this.newAuthenticatorProgress.Location = new System.Drawing.Point(120, 210);
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
			this.allowCopyNewButton.Location = new System.Drawing.Point(284, 159);
			this.allowCopyNewButton.Name = "allowCopyNewButton";
			this.allowCopyNewButton.Size = new System.Drawing.Size(83, 17);
			this.allowCopyNewButton.TabIndex = 4;
			this.allowCopyNewButton.Text = "Allow copy?";
			this.allowCopyNewButton.UseVisualStyleBackColor = true;
			this.allowCopyNewButton.CheckedChanged += new System.EventHandler(this.allowCopyNewButton_CheckedChanged);
			// 
			// newRestoreCodeField
			// 
			this.newRestoreCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newRestoreCodeField.Location = new System.Drawing.Point(120, 221);
			this.newRestoreCodeField.Multiline = true;
			this.newRestoreCodeField.Name = "newRestoreCodeField";
			this.newRestoreCodeField.SecretMode = false;
			this.newRestoreCodeField.Size = new System.Drawing.Size(158, 26);
			this.newRestoreCodeField.SpaceOut = 2;
			this.newRestoreCodeField.TabIndex = 3;
			// 
			// newLoginCodeField
			// 
			this.newLoginCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newLoginCodeField.Location = new System.Drawing.Point(120, 182);
			this.newLoginCodeField.Multiline = true;
			this.newLoginCodeField.Name = "newLoginCodeField";
			this.newLoginCodeField.SecretMode = false;
			this.newLoginCodeField.Size = new System.Drawing.Size(158, 26);
			this.newLoginCodeField.SpaceOut = 4;
			this.newLoginCodeField.TabIndex = 2;
			// 
			// label6
			// 
			this.label6.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label6.AutoSize = true;
			this.label6.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label6.Location = new System.Drawing.Point(17, 226);
			this.label6.Name = "label6";
			this.label6.Size = new System.Drawing.Size(95, 16);
			this.label6.TabIndex = 1;
			this.label6.Text = "Restore Code:";
			// 
			// newSerialNumberField
			// 
			this.newSerialNumberField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.newSerialNumberField.Location = new System.Drawing.Point(120, 153);
			this.newSerialNumberField.Multiline = true;
			this.newSerialNumberField.Name = "newSerialNumberField";
			this.newSerialNumberField.SecretMode = false;
			this.newSerialNumberField.Size = new System.Drawing.Size(158, 26);
			this.newSerialNumberField.SpaceOut = 0;
			this.newSerialNumberField.TabIndex = 1;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(19, 186);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(80, 16);
			this.label5.TabIndex = 1;
			this.label5.Text = "Login Code:";
			// 
			// enrollAuthenticatorButton
			// 
			this.enrollAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.enrollAuthenticatorButton.Location = new System.Drawing.Point(63, 100);
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
			this.label4.Location = new System.Drawing.Point(17, 158);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(97, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Serial Number:";
			// 
			// label3
			// 
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(6, 104);
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
			this.label2.Location = new System.Drawing.Point(6, 133);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(446, 25);
			this.label2.TabIndex = 1;
			this.label2.Text = "5. You will need to add the following details to your account.";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 25);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(446, 72);
			this.label1.TabIndex = 0;
			this.label1.Text = resources.GetString("label1.Text");
			// 
			// restoreAuthenticatorGroup
			// 
			this.restoreAuthenticatorGroup.Controls.Add(this.restoreRestoreCodeField);
			this.restoreAuthenticatorGroup.Controls.Add(this.label7);
			this.restoreAuthenticatorGroup.Controls.Add(this.restoreSerialNumberField);
			this.restoreAuthenticatorGroup.Controls.Add(this.label8);
			this.restoreAuthenticatorGroup.Controls.Add(this.label9);
			this.restoreAuthenticatorGroup.Enabled = false;
			this.restoreAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreAuthenticatorGroup.Location = new System.Drawing.Point(12, 340);
			this.restoreAuthenticatorGroup.Name = "restoreAuthenticatorGroup";
			this.restoreAuthenticatorGroup.Size = new System.Drawing.Size(458, 111);
			this.restoreAuthenticatorGroup.TabIndex = 3;
			this.restoreAuthenticatorGroup.TabStop = false;
			// 
			// restoreRestoreCodeField
			// 
			this.restoreRestoreCodeField.Location = new System.Drawing.Point(120, 75);
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
			this.label7.Location = new System.Drawing.Point(17, 48);
			this.label7.Name = "label7";
			this.label7.Size = new System.Drawing.Size(97, 16);
			this.label7.TabIndex = 1;
			this.label7.Text = "Serial Number:";
			// 
			// restoreSerialNumberField
			// 
			this.restoreSerialNumberField.Location = new System.Drawing.Point(120, 47);
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
			this.label8.Location = new System.Drawing.Point(17, 76);
			this.label8.Name = "label8";
			this.label8.Size = new System.Drawing.Size(95, 16);
			this.label8.TabIndex = 1;
			this.label8.Text = "Restore Code:";
			// 
			// label9
			// 
			this.label9.AutoSize = true;
			this.label9.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label9.Location = new System.Drawing.Point(6, 22);
			this.label9.Name = "label9";
			this.label9.Size = new System.Drawing.Size(273, 16);
			this.label9.TabIndex = 1;
			this.label9.Text = "Enter your Serial Number and Restore Code.";
			// 
			// importAuthenticatorGroup
			// 
			this.importAuthenticatorGroup.Controls.Add(this.importPrivateKeyField);
			this.importAuthenticatorGroup.Controls.Add(this.label11);
			this.importAuthenticatorGroup.Enabled = false;
			this.importAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.importAuthenticatorGroup.Location = new System.Drawing.Point(12, 457);
			this.importAuthenticatorGroup.Name = "importAuthenticatorGroup";
			this.importAuthenticatorGroup.Size = new System.Drawing.Size(458, 73);
			this.importAuthenticatorGroup.TabIndex = 5;
			this.importAuthenticatorGroup.TabStop = false;
			// 
			// importPrivateKeyField
			// 
			this.importPrivateKeyField.Location = new System.Drawing.Point(120, 33);
			this.importPrivateKeyField.Name = "importPrivateKeyField";
			this.importPrivateKeyField.Size = new System.Drawing.Size(332, 22);
			this.importPrivateKeyField.TabIndex = 0;
			// 
			// label11
			// 
			this.label11.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label11.AutoSize = true;
			this.label11.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label11.Location = new System.Drawing.Point(17, 36);
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
			this.okButton.Location = new System.Drawing.Point(315, 542);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseVisualStyleBackColor = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.cancelButton.Location = new System.Drawing.Point(396, 542);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			this.cancelButton.Click += new System.EventHandler(this.cancelButton_Click);
			// 
			// restoreAuthenticatorButton
			// 
			this.restoreAuthenticatorButton.AutoSize = true;
			this.restoreAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.restoreAuthenticatorButton.Group = "Authenticator";
			this.restoreAuthenticatorButton.Location = new System.Drawing.Point(20, 337);
			this.restoreAuthenticatorButton.Name = "restoreAuthenticatorButton";
			this.restoreAuthenticatorButton.Size = new System.Drawing.Size(154, 20);
			this.restoreAuthenticatorButton.TabIndex = 2;
			this.restoreAuthenticatorButton.TabStop = true;
			this.restoreAuthenticatorButton.Text = "Restore Authenticator";
			this.restoreAuthenticatorButton.UseVisualStyleBackColor = true;
			this.restoreAuthenticatorButton.CheckedChanged += new System.EventHandler(this.restoreAuthenticatorButton_CheckedChanged);
			// 
			// createAuthenticatorButton
			// 
			this.createAuthenticatorButton.AutoSize = true;
			this.createAuthenticatorButton.Checked = true;
			this.createAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.createAuthenticatorButton.Group = "Authenticator";
			this.createAuthenticatorButton.Location = new System.Drawing.Point(20, 75);
			this.createAuthenticatorButton.Name = "createAuthenticatorButton";
			this.createAuthenticatorButton.Size = new System.Drawing.Size(176, 20);
			this.createAuthenticatorButton.TabIndex = 0;
			this.createAuthenticatorButton.TabStop = true;
			this.createAuthenticatorButton.Text = "Create New Authenticator";
			this.createAuthenticatorButton.UseVisualStyleBackColor = true;
			this.createAuthenticatorButton.CheckedChanged += new System.EventHandler(this.createAuthenticatorButton_CheckedChanged);
			// 
			// importAuthenticatorButton
			// 
			this.importAuthenticatorButton.AutoSize = true;
			this.importAuthenticatorButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.importAuthenticatorButton.Group = "Authenticator";
			this.importAuthenticatorButton.Location = new System.Drawing.Point(20, 455);
			this.importAuthenticatorButton.Name = "importAuthenticatorButton";
			this.importAuthenticatorButton.Size = new System.Drawing.Size(143, 20);
			this.importAuthenticatorButton.TabIndex = 4;
			this.importAuthenticatorButton.TabStop = true;
			this.importAuthenticatorButton.Text = "Import Authenticator";
			this.importAuthenticatorButton.UseVisualStyleBackColor = true;
			this.importAuthenticatorButton.CheckedChanged += new System.EventHandler(this.importAuthenticatorButton_CheckedChanged);
			// 
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Interval = 500;
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
			this.pictureBox1.Location = new System.Drawing.Point(12, 12);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(456, 60);
			this.pictureBox1.TabIndex = 8;
			this.pictureBox1.TabStop = false;
			// 
			// AddBattleNetAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(483, 577);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.restoreAuthenticatorButton);
			this.Controls.Add(this.createAuthenticatorButton);
			this.Controls.Add(this.importAuthenticatorButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.importAuthenticatorGroup);
			this.Controls.Add(this.restoreAuthenticatorGroup);
			this.Controls.Add(this.newAuthenticatorGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddBattleNetAuthenticator";
			this.ShowIcon = false;
			this.Text = "Add Battle.net Authenticator";
			this.Load += new System.EventHandler(this.AddBattleNetAuthenticator_Load);
			this.newAuthenticatorGroup.ResumeLayout(false);
			this.newAuthenticatorGroup.PerformLayout();
			this.restoreAuthenticatorGroup.ResumeLayout(false);
			this.restoreAuthenticatorGroup.PerformLayout();
			this.importAuthenticatorGroup.ResumeLayout(false);
			this.importAuthenticatorGroup.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private System.Windows.Forms.GroupBox newAuthenticatorGroup;
		private System.Windows.Forms.Label label1;
		private WinAuth.GroupRadioButton restoreAuthenticatorButton;
		private WinAuth.GroupRadioButton createAuthenticatorButton;
		private WinAuth.SecretTextBox newRestoreCodeField;
		private WinAuth.SecretTextBox newLoginCodeField;
		private System.Windows.Forms.Label label6;
		private WinAuth.SecretTextBox newSerialNumberField;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Button enrollAuthenticatorButton;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.GroupBox restoreAuthenticatorGroup;
		private System.Windows.Forms.GroupBox importAuthenticatorGroup;
		private WinAuth.GroupRadioButton importAuthenticatorButton;
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
		private System.Windows.Forms.PictureBox pictureBox1;
	}
}