namespace WinAuth
{
	partial class ChangePasswordForm
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
			System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ChangePasswordForm));
			this.noneCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.introLabel = new MetroFramework.Controls.MetroLabel();
			this.noneLabel = new MetroFramework.Controls.MetroLabel();
			this.machineCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.machineLabel = new MetroFramework.Controls.MetroLabel();
			this.userCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.userLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.passwordLabel = new MetroFramework.Controls.MetroLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.verifyField = new MetroFramework.Controls.MetroTextBox();
			this.verifyFieldLabel = new MetroFramework.Controls.MetroLabel();
			this.passwordFieldLabel = new MetroFramework.Controls.MetroLabel();
			this.pictureBox3 = new System.Windows.Forms.PictureBox();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
			this.SuspendLayout();
			// 
			// noneCheckbox
			// 
			this.noneCheckbox.AutoSize = true;
			this.noneCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.noneCheckbox.Location = new System.Drawing.Point(23, 115);
			this.noneCheckbox.Name = "noneCheckbox";
			this.noneCheckbox.Size = new System.Drawing.Size(250, 19);
			this.noneCheckbox.TabIndex = 0;
			this.noneCheckbox.Text = "I do NOT want to use any protection";
			this.noneCheckbox.UseSelectable = true;
			this.noneCheckbox.CheckedChanged += new System.EventHandler(this.noneCheckbox_CheckedChanged);
			// 
			// introLabel
			// 
			this.introLabel.Location = new System.Drawing.Point(23, 60);
			this.introLabel.Name = "introLabel";
			this.introLabel.Size = new System.Drawing.Size(628, 42);
			this.introLabel.TabIndex = 1;
			this.introLabel.Text = "Select how you would like to protect your authenticators. A combination of user p" +
    "rotection and your own password is recommended.";
			// 
			// noneLabel
			// 
			this.noneLabel.Location = new System.Drawing.Point(23, 137);
			this.noneLabel.Name = "noneLabel";
			this.noneLabel.Size = new System.Drawing.Size(628, 49);
			this.noneLabel.TabIndex = 1;
			this.noneLabel.Text = "Your authenticators will not be protected by a password and will not be encrypted" +
    ". This is very insecure and should only be used if you are copying your authenti" +
    "cators between different computers.";
			// 
			// machineCheckbox
			// 
			this.machineCheckbox.AutoSize = true;
			this.machineCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.machineCheckbox.Location = new System.Drawing.Point(23, 203);
			this.machineCheckbox.Name = "machineCheckbox";
			this.machineCheckbox.Size = new System.Drawing.Size(213, 19);
			this.machineCheckbox.TabIndex = 1;
			this.machineCheckbox.Text = "Only useable on this computer";
			this.machineCheckbox.UseSelectable = true;
			this.machineCheckbox.CheckedChanged += new System.EventHandler(this.machineCheckbox_CheckedChanged);
			// 
			// machineLabel
			// 
			this.machineLabel.Location = new System.Drawing.Point(23, 225);
			this.machineLabel.Name = "machineLabel";
			this.machineLabel.Size = new System.Drawing.Size(628, 64);
			this.machineLabel.TabIndex = 1;
			this.machineLabel.Text = resources.GetString("machineLabel.Text");
			// 
			// userCheckbox
			// 
			this.userCheckbox.AutoSize = true;
			this.userCheckbox.Enabled = false;
			this.userCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.userCheckbox.Location = new System.Drawing.Point(40, 292);
			this.userCheckbox.Name = "userCheckbox";
			this.userCheckbox.Size = new System.Drawing.Size(334, 19);
			this.userCheckbox.TabIndex = 2;
			this.userCheckbox.Text = "Only useable by the current user on this computer";
			this.userCheckbox.UseSelectable = true;
			this.userCheckbox.CheckedChanged += new System.EventHandler(this.userCheckbox_CheckedChanged);
			// 
			// userLabel
			// 
			this.userLabel.Location = new System.Drawing.Point(40, 314);
			this.userLabel.Name = "userLabel";
			this.userLabel.Size = new System.Drawing.Size(611, 30);
			this.userLabel.TabIndex = 1;
			this.userLabel.Text = "Additonal protect and encrypted your authenticators to only allow access by the c" +
    "urrent user.";
			// 
			// passwordCheckbox
			// 
			this.passwordCheckbox.AutoSize = true;
			this.passwordCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.passwordCheckbox.Location = new System.Drawing.Point(23, 368);
			this.passwordCheckbox.Name = "passwordCheckbox";
			this.passwordCheckbox.Size = new System.Drawing.Size(214, 19);
			this.passwordCheckbox.TabIndex = 3;
			this.passwordCheckbox.Text = "Protect with my own password";
			this.passwordCheckbox.UseSelectable = true;
			this.passwordCheckbox.CheckedChanged += new System.EventHandler(this.passwordCheckbox_CheckedChanged);
			// 
			// passwordLabel
			// 
			this.passwordLabel.Location = new System.Drawing.Point(23, 390);
			this.passwordLabel.Name = "passwordLabel";
			this.passwordLabel.Size = new System.Drawing.Size(628, 65);
			this.passwordLabel.TabIndex = 1;
			this.passwordLabel.Text = resources.GetString("passwordLabel.Text");
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::WinAuth.Properties.Resources.BluePixel;
			this.pictureBox1.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox1.Location = new System.Drawing.Point(23, 187);
			this.pictureBox1.Name = "pictureBox1";
			this.pictureBox1.Size = new System.Drawing.Size(620, 1);
			this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox1.TabIndex = 2;
			this.pictureBox1.TabStop = false;
			// 
			// pictureBox2
			// 
			this.pictureBox2.Image = global::WinAuth.Properties.Resources.BluePixel;
			this.pictureBox2.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox2.Location = new System.Drawing.Point(24, 351);
			this.pictureBox2.Name = "pictureBox2";
			this.pictureBox2.Size = new System.Drawing.Size(620, 1);
			this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox2.TabIndex = 3;
			this.pictureBox2.TabStop = false;
			// 
			// cancelButton
			// 
			this.cancelButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.cancelButton.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.cancelButton.Location = new System.Drawing.Point(568, 550);
			this.cancelButton.Name = "cancelButton";
			this.cancelButton.Size = new System.Drawing.Size(75, 23);
			this.cancelButton.TabIndex = 7;
			this.cancelButton.Text = "Cancel";
			this.cancelButton.UseSelectable = true;
			// 
			// okButton
			// 
			this.okButton.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.okButton.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.okButton.Location = new System.Drawing.Point(487, 550);
			this.okButton.Name = "okButton";
			this.okButton.Size = new System.Drawing.Size(75, 23);
			this.okButton.TabIndex = 6;
			this.okButton.Text = "OK";
			this.okButton.UseSelectable = true;
			this.okButton.Click += new System.EventHandler(this.okButton_Click);
			// 
			// passwordField
			// 
			this.passwordField.Enabled = false;
			this.passwordField.Location = new System.Drawing.Point(123, 458);
			this.passwordField.MaxLength = 32767;
			this.passwordField.Name = "passwordField";
			this.passwordField.PasswordChar = '●';
			this.passwordField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.passwordField.SelectedText = "";
			this.passwordField.Size = new System.Drawing.Size(262, 23);
			this.passwordField.TabIndex = 4;
			this.passwordField.UseSelectable = true;
			this.passwordField.UseSystemPasswordChar = true;
			// 
			// verifyField
			// 
			this.verifyField.Enabled = false;
			this.verifyField.Location = new System.Drawing.Point(123, 487);
			this.verifyField.MaxLength = 32767;
			this.verifyField.Name = "verifyField";
			this.verifyField.PasswordChar = '●';
			this.verifyField.ScrollBars = System.Windows.Forms.ScrollBars.None;
			this.verifyField.SelectedText = "";
			this.verifyField.Size = new System.Drawing.Size(262, 23);
			this.verifyField.TabIndex = 5;
			this.verifyField.UseSelectable = true;
			this.verifyField.UseSystemPasswordChar = true;
			// 
			// verifyFieldLabel
			// 
			this.verifyFieldLabel.AutoSize = true;
			this.verifyFieldLabel.Location = new System.Drawing.Point(40, 488);
			this.verifyFieldLabel.Name = "verifyFieldLabel";
			this.verifyFieldLabel.Size = new System.Drawing.Size(41, 19);
			this.verifyFieldLabel.TabIndex = 5;
			this.verifyFieldLabel.Text = "Verify";
			// 
			// passwordFieldLabel
			// 
			this.passwordFieldLabel.AutoSize = true;
			this.passwordFieldLabel.Location = new System.Drawing.Point(40, 459);
			this.passwordFieldLabel.Name = "passwordFieldLabel";
			this.passwordFieldLabel.Size = new System.Drawing.Size(63, 19);
			this.passwordFieldLabel.TabIndex = 5;
			this.passwordFieldLabel.Text = "Password";
			// 
			// pictureBox3
			// 
			this.pictureBox3.Image = global::WinAuth.Properties.Resources.BluePixel;
			this.pictureBox3.ImeMode = System.Windows.Forms.ImeMode.NoControl;
			this.pictureBox3.Location = new System.Drawing.Point(27, 529);
			this.pictureBox3.Name = "pictureBox3";
			this.pictureBox3.Size = new System.Drawing.Size(620, 1);
			this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
			this.pictureBox3.TabIndex = 8;
			this.pictureBox3.TabStop = false;
			// 
			// ChangePasswordForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(674, 596);
			this.Controls.Add(this.pictureBox3);
			this.Controls.Add(this.verifyField);
			this.Controls.Add(this.passwordField);
			this.Controls.Add(this.verifyFieldLabel);
			this.Controls.Add(this.passwordFieldLabel);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.passwordLabel);
			this.Controls.Add(this.userLabel);
			this.Controls.Add(this.passwordCheckbox);
			this.Controls.Add(this.machineLabel);
			this.Controls.Add(this.userCheckbox);
			this.Controls.Add(this.noneLabel);
			this.Controls.Add(this.machineCheckbox);
			this.Controls.Add(this.introLabel);
			this.Controls.Add(this.noneCheckbox);
			this.Name = "ChangePasswordForm";
			this.Resizable = false;
			this.Text = "Protection";
			this.Load += new System.EventHandler(this.ChangePasswordForm_Load);
			this.Shown += new System.EventHandler(this.ChangePasswordForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroCheckBox noneCheckbox;
		private MetroFramework.Controls.MetroLabel introLabel;
		private MetroFramework.Controls.MetroLabel noneLabel;
		private MetroFramework.Controls.MetroCheckBox machineCheckbox;
		private MetroFramework.Controls.MetroLabel machineLabel;
		private MetroFramework.Controls.MetroCheckBox userCheckbox;
		private MetroFramework.Controls.MetroLabel userLabel;
		private MetroFramework.Controls.MetroCheckBox passwordCheckbox;
		private MetroFramework.Controls.MetroLabel passwordLabel;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroTextBox verifyField;
		private MetroFramework.Controls.MetroLabel verifyFieldLabel;
		private MetroFramework.Controls.MetroLabel passwordFieldLabel;
		private System.Windows.Forms.PictureBox pictureBox3;

	}
}