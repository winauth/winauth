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
			this.newAuthenticatorGroup = new System.Windows.Forms.GroupBox();
			this.newAuthenticatorProgress = new System.Windows.Forms.ProgressBar();
			this.allowCopyButton = new System.Windows.Forms.CheckBox();
			this.codeField = new WinAuth.SecretTextBox();
			this.label5 = new System.Windows.Forms.Label();
			this.verifyAuthenticatorButton = new System.Windows.Forms.Button();
			this.secretCodeField = new System.Windows.Forms.TextBox();
			this.label4 = new System.Windows.Forms.Label();
			this.label3 = new System.Windows.Forms.Label();
			this.label1 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.okButton = new System.Windows.Forms.Button();
			this.cancelButton = new System.Windows.Forms.Button();
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.icon2RadioButton = new WinAuth.GroupRadioButton();
			this.icon1RadioButton = new WinAuth.GroupRadioButton();
			this.label10 = new System.Windows.Forms.Label();
			this.label12 = new System.Windows.Forms.Label();
			this.nameField = new System.Windows.Forms.TextBox();
			this.newAuthenticatorTimer = new System.Windows.Forms.Timer(this.components);
			this.icon2 = new System.Windows.Forms.PictureBox();
			this.icon1 = new System.Windows.Forms.PictureBox();
			this.newAuthenticatorGroup.SuspendLayout();
			this.groupBox1.SuspendLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).BeginInit();
			this.SuspendLayout();
			// 
			// newAuthenticatorGroup
			// 
			this.newAuthenticatorGroup.Controls.Add(this.newAuthenticatorProgress);
			this.newAuthenticatorGroup.Controls.Add(this.allowCopyButton);
			this.newAuthenticatorGroup.Controls.Add(this.codeField);
			this.newAuthenticatorGroup.Controls.Add(this.label5);
			this.newAuthenticatorGroup.Controls.Add(this.verifyAuthenticatorButton);
			this.newAuthenticatorGroup.Controls.Add(this.secretCodeField);
			this.newAuthenticatorGroup.Controls.Add(this.label4);
			this.newAuthenticatorGroup.Controls.Add(this.label3);
			this.newAuthenticatorGroup.Controls.Add(this.label1);
			this.newAuthenticatorGroup.Controls.Add(this.label2);
			this.newAuthenticatorGroup.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.newAuthenticatorGroup.Location = new System.Drawing.Point(12, 123);
			this.newAuthenticatorGroup.Name = "newAuthenticatorGroup";
			this.newAuthenticatorGroup.Size = new System.Drawing.Size(458, 361);
			this.newAuthenticatorGroup.TabIndex = 1;
			this.newAuthenticatorGroup.TabStop = false;
			// 
			// newAuthenticatorProgress
			// 
			this.newAuthenticatorProgress.Location = new System.Drawing.Point(130, 277);
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
			this.allowCopyButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.allowCopyButton.Location = new System.Drawing.Point(294, 256);
			this.allowCopyButton.Name = "allowCopyButton";
			this.allowCopyButton.Size = new System.Drawing.Size(83, 17);
			this.allowCopyButton.TabIndex = 7;
			this.allowCopyButton.Text = "Allow copy?";
			this.allowCopyButton.UseVisualStyleBackColor = true;
			this.allowCopyButton.CheckedChanged += new System.EventHandler(this.allowCopyButton_CheckedChanged);
			// 
			// codeField
			// 
			this.codeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F);
			this.codeField.Location = new System.Drawing.Point(130, 249);
			this.codeField.Multiline = true;
			this.codeField.Name = "codeField";
			this.codeField.SecretMode = false;
			this.codeField.Size = new System.Drawing.Size(158, 26);
			this.codeField.SpaceOut = 3;
			this.codeField.TabIndex = 8;
			// 
			// label5
			// 
			this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label5.AutoSize = true;
			this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label5.Location = new System.Drawing.Point(38, 255);
			this.label5.Name = "label5";
			this.label5.Size = new System.Drawing.Size(44, 16);
			this.label5.TabIndex = 6;
			this.label5.Text = "Code:";
			// 
			// verifyAuthenticatorButton
			// 
			this.verifyAuthenticatorButton.Location = new System.Drawing.Point(129, 179);
			this.verifyAuthenticatorButton.Name = "verifyAuthenticatorButton";
			this.verifyAuthenticatorButton.Size = new System.Drawing.Size(159, 23);
			this.verifyAuthenticatorButton.TabIndex = 2;
			this.verifyAuthenticatorButton.Text = "Verify Authenticator";
			this.verifyAuthenticatorButton.UseVisualStyleBackColor = true;
			this.verifyAuthenticatorButton.Click += new System.EventHandler(this.verifyAuthenticatorButton_Click);
			// 
			// secretCodeField
			// 
			this.secretCodeField.AllowDrop = true;
			this.secretCodeField.CausesValidation = false;
			this.secretCodeField.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.secretCodeField.Location = new System.Drawing.Point(130, 151);
			this.secretCodeField.Multiline = true;
			this.secretCodeField.Name = "secretCodeField";
			this.secretCodeField.Size = new System.Drawing.Size(275, 22);
			this.secretCodeField.TabIndex = 0;
			// 
			// label4
			// 
			this.label4.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label4.AutoSize = true;
			this.label4.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label4.Location = new System.Drawing.Point(38, 154);
			this.label4.Name = "label4";
			this.label4.Size = new System.Drawing.Size(86, 16);
			this.label4.TabIndex = 1;
			this.label4.Text = "Secret Code:";
			// 
			// label3
			// 
			this.label3.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label3.Location = new System.Drawing.Point(14, 298);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(438, 56);
			this.label3.TabIndex = 1;
			this.label3.Text = "7. IMPORTANT: Write down you Secret Code and store it somewhere safe and secure. " +
    "You will need it if you ever need to restore your authenticator.";
			// 
			// label1
			// 
			this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(14, 220);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(438, 26);
			this.label1.TabIndex = 1;
			this.label1.Text = "6. Enter the follow code to verify it is working";
			// 
			// label2
			// 
			this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
			this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label2.Location = new System.Drawing.Point(14, 25);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(438, 123);
			this.label2.TabIndex = 1;
			this.label2.Text = resources.GetString("label2.Text");
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.okButton.Location = new System.Drawing.Point(315, 494);
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
			this.cancelButton.Location = new System.Drawing.Point(396, 494);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 3;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseVisualStyleBackColor = true;
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.icon2RadioButton);
			this.groupBox1.Controls.Add(this.icon1RadioButton);
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
			// icon2RadioButton
			// 
			this.icon2RadioButton.AutoSize = true;
			this.icon2RadioButton.Group = "ICON";
			this.icon2RadioButton.Location = new System.Drawing.Point(144, 59);
			this.icon2RadioButton.Name = "icon2RadioButton";
			this.icon2RadioButton.Size = new System.Drawing.Size(14, 13);
			this.icon2RadioButton.TabIndex = 2;
			this.icon2RadioButton.Tag = "ArenaNetIcon.png";
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
			this.icon1RadioButton.Tag = "GuildWarsAuthenticatorIcon.png";
			this.icon1RadioButton.UseVisualStyleBackColor = true;
			this.icon1RadioButton.CheckedChanged += new System.EventHandler(this.iconRadioButton_CheckedChanged);
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
			// newAuthenticatorTimer
			// 
			this.newAuthenticatorTimer.Tick += new System.EventHandler(this.newAuthenticatorTimer_Tick);
			// 
			// icon2
			// 
			this.icon2.Image = global::WinAuth.Properties.Resources.ArenaNetIcon;
			this.icon2.Location = new System.Drawing.Point(164, 44);
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
			this.icon1.Location = new System.Drawing.Point(88, 44);
			this.icon1.Name = "icon1";
			this.icon1.Size = new System.Drawing.Size(48, 48);
			this.icon1.TabIndex = 4;
			this.icon1.TabStop = false;
			this.icon1.Tag = "";
			this.icon1.Click += new System.EventHandler(this.icon1_Click);
			// 
			// AddGuildWarsAuthenticator
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(483, 529);
			this.Controls.Add(this.groupBox1);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.newAuthenticatorGroup);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.MaximizeBox = false;
			this.Name = "AddGuildWarsAuthenticator";
			this.ShowIcon = false;
			this.Text = "Guild Wars 2 Authenticator";
			this.Load += new System.EventHandler(this.AddGuildWarsAuthenticator_Load);
			this.newAuthenticatorGroup.ResumeLayout(false);
			this.newAuthenticatorGroup.PerformLayout();
			this.groupBox1.ResumeLayout(false);
			this.groupBox1.PerformLayout();
			((System.ComponentModel.ISupportInitialize)(this.icon2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.icon1)).EndInit();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox newAuthenticatorGroup;
		private System.Windows.Forms.Label label4;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.Button okButton;
		private System.Windows.Forms.Button cancelButton;
		private System.Windows.Forms.TextBox secretCodeField;
		private System.Windows.Forms.GroupBox groupBox1;
		private GroupRadioButton icon2RadioButton;
		private GroupRadioButton icon1RadioButton;
		private System.Windows.Forms.PictureBox icon2;
		private System.Windows.Forms.PictureBox icon1;
		private System.Windows.Forms.Label label10;
		private System.Windows.Forms.Label label12;
		private System.Windows.Forms.TextBox nameField;
		private System.Windows.Forms.Button verifyAuthenticatorButton;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.ProgressBar newAuthenticatorProgress;
		private System.Windows.Forms.CheckBox allowCopyButton;
		private SecretTextBox codeField;
		private System.Windows.Forms.Label label5;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Timer newAuthenticatorTimer;
	}
}