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
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.machineCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
			this.userCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
			this.passwordCheckbox = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
			this.pictureBox1 = new System.Windows.Forms.PictureBox();
			this.pictureBox2 = new System.Windows.Forms.PictureBox();
			this.cancelButton = new MetroFramework.Controls.MetroButton();
			this.okButton = new MetroFramework.Controls.MetroButton();
			this.passwordField = new MetroFramework.Controls.MetroTextBox();
			this.verifyField = new MetroFramework.Controls.MetroTextBox();
			this.metroLabel7 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel6 = new MetroFramework.Controls.MetroLabel();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
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
			// metroLabel1
			// 
			this.metroLabel1.Location = new System.Drawing.Point(23, 60);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(628, 42);
			this.metroLabel1.TabIndex = 1;
			this.metroLabel1.Text = "Select how you would like to protect your authenticators. A combination of user p" +
    "rotection and your own password is recommended.";
			// 
			// metroLabel2
			// 
			this.metroLabel2.Location = new System.Drawing.Point(23, 137);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(628, 49);
			this.metroLabel2.TabIndex = 1;
			this.metroLabel2.Text = "Your authenticators will not be protected by a password and will not be encrypted" +
    ". This is very insecure and should only be used if you are copying your authenti" +
    "cators between different computers.";
			// 
			// machineCheckbox
			// 
			this.machineCheckbox.AutoSize = true;
			this.machineCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.machineCheckbox.Location = new System.Drawing.Point(23, 292);
			this.machineCheckbox.Name = "machineCheckbox";
			this.machineCheckbox.Size = new System.Drawing.Size(213, 19);
			this.machineCheckbox.TabIndex = 2;
			this.machineCheckbox.Text = "Only useable on this computer";
			this.machineCheckbox.UseSelectable = true;
			this.machineCheckbox.CheckedChanged += new System.EventHandler(this.machineCheckbox_CheckedChanged);
			// 
			// metroLabel3
			// 
			this.metroLabel3.Location = new System.Drawing.Point(23, 314);
			this.metroLabel3.Name = "metroLabel3";
			this.metroLabel3.Size = new System.Drawing.Size(628, 63);
			this.metroLabel3.TabIndex = 1;
			this.metroLabel3.Text = resources.GetString("metroLabel3.Text");
			// 
			// userCheckbox
			// 
			this.userCheckbox.AutoSize = true;
			this.userCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.userCheckbox.Location = new System.Drawing.Point(23, 201);
			this.userCheckbox.Name = "userCheckbox";
			this.userCheckbox.Size = new System.Drawing.Size(288, 19);
			this.userCheckbox.TabIndex = 1;
			this.userCheckbox.Text = "Only useable by this user on this computer";
			this.userCheckbox.UseSelectable = true;
			this.userCheckbox.CheckedChanged += new System.EventHandler(this.userCheckbox_CheckedChanged);
			// 
			// metroLabel4
			// 
			this.metroLabel4.Location = new System.Drawing.Point(23, 223);
			this.metroLabel4.Name = "metroLabel4";
			this.metroLabel4.Size = new System.Drawing.Size(628, 69);
			this.metroLabel4.TabIndex = 1;
			this.metroLabel4.Text = resources.GetString("metroLabel4.Text");
			// 
			// passwordCheckbox
			// 
			this.passwordCheckbox.AutoSize = true;
			this.passwordCheckbox.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.passwordCheckbox.Location = new System.Drawing.Point(23, 400);
			this.passwordCheckbox.Name = "passwordCheckbox";
			this.passwordCheckbox.Size = new System.Drawing.Size(214, 19);
			this.passwordCheckbox.TabIndex = 3;
			this.passwordCheckbox.Text = "Protect with my own password";
			this.passwordCheckbox.UseSelectable = true;
			this.passwordCheckbox.CheckedChanged += new System.EventHandler(this.passwordCheckbox_CheckedChanged);
			// 
			// metroLabel5
			// 
			this.metroLabel5.Location = new System.Drawing.Point(23, 422);
			this.metroLabel5.Name = "metroLabel5";
			this.metroLabel5.Size = new System.Drawing.Size(628, 65);
			this.metroLabel5.TabIndex = 1;
			this.metroLabel5.Text = resources.GetString("metroLabel5.Text");
			// 
			// pictureBox1
			// 
			this.pictureBox1.Image = global::WinAuth.Properties.Resources.BluePixel;
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
			this.pictureBox2.Location = new System.Drawing.Point(24, 383);
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
			this.cancelButton.Location = new System.Drawing.Point(576, 557);
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
			this.okButton.Location = new System.Drawing.Point(495, 557);
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
			this.passwordField.Location = new System.Drawing.Point(123, 490);
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
			this.verifyField.Location = new System.Drawing.Point(123, 519);
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
			// metroLabel7
			// 
			this.metroLabel7.AutoSize = true;
			this.metroLabel7.Location = new System.Drawing.Point(40, 520);
			this.metroLabel7.Name = "metroLabel7";
			this.metroLabel7.Size = new System.Drawing.Size(41, 19);
			this.metroLabel7.TabIndex = 5;
			this.metroLabel7.Text = "Verify";
			// 
			// metroLabel6
			// 
			this.metroLabel6.AutoSize = true;
			this.metroLabel6.Location = new System.Drawing.Point(40, 491);
			this.metroLabel6.Name = "metroLabel6";
			this.metroLabel6.Size = new System.Drawing.Size(63, 19);
			this.metroLabel6.TabIndex = 5;
			this.metroLabel6.Text = "Password";
			// 
			// ChangePasswordForm
			// 
			this.AcceptButton = this.okButton;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.cancelButton;
			this.ClientSize = new System.Drawing.Size(674, 603);
			this.Controls.Add(this.verifyField);
			this.Controls.Add(this.passwordField);
			this.Controls.Add(this.metroLabel7);
			this.Controls.Add(this.metroLabel6);
			this.Controls.Add(this.okButton);
			this.Controls.Add(this.cancelButton);
			this.Controls.Add(this.pictureBox2);
			this.Controls.Add(this.pictureBox1);
			this.Controls.Add(this.metroLabel5);
			this.Controls.Add(this.metroLabel4);
			this.Controls.Add(this.passwordCheckbox);
			this.Controls.Add(this.metroLabel3);
			this.Controls.Add(this.userCheckbox);
			this.Controls.Add(this.metroLabel2);
			this.Controls.Add(this.machineCheckbox);
			this.Controls.Add(this.metroLabel1);
			this.Controls.Add(this.noneCheckbox);
			this.Name = "ChangePasswordForm";
			this.Resizable = false;
			this.Text = "Protection";
			this.Load += new System.EventHandler(this.ChangePasswordForm_Load);
			this.Shown += new System.EventHandler(this.ChangePasswordForm_Shown);
			((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
			((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroCheckBox noneCheckbox;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private MetroFramework.Controls.MetroCheckBox machineCheckbox;
		private MetroFramework.Controls.MetroLabel metroLabel3;
		private MetroFramework.Controls.MetroCheckBox userCheckbox;
		private MetroFramework.Controls.MetroLabel metroLabel4;
		private MetroFramework.Controls.MetroCheckBox passwordCheckbox;
		private MetroFramework.Controls.MetroLabel metroLabel5;
		private System.Windows.Forms.PictureBox pictureBox1;
		private System.Windows.Forms.PictureBox pictureBox2;
		private MetroFramework.Controls.MetroButton cancelButton;
		private MetroFramework.Controls.MetroButton okButton;
		private MetroFramework.Controls.MetroTextBox passwordField;
		private MetroFramework.Controls.MetroTextBox verifyField;
		private MetroFramework.Controls.MetroLabel metroLabel7;
		private MetroFramework.Controls.MetroLabel metroLabel6;

	}
}