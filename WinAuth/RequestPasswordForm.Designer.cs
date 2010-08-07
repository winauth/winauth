namespace WindowsAuthenticator
{
	partial class RequestPasswordForm
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
			this.groupBox1 = new System.Windows.Forms.GroupBox();
			this.panel1 = new System.Windows.Forms.Panel();
			this.label3 = new System.Windows.Forms.Label();
			this.label2 = new System.Windows.Forms.Label();
			this.tbVerify = new System.Windows.Forms.TextBox();
			this.tbPassword = new System.Windows.Forms.TextBox();
			this.rbPassword = new System.Windows.Forms.RadioButton();
			this.rbNoPassword = new System.Windows.Forms.RadioButton();
			this.label1 = new System.Windows.Forms.Label();
			this.btnOk = new System.Windows.Forms.Button();
			this.btnCancel = new System.Windows.Forms.Button();
			this.groupBox1.SuspendLayout();
			this.panel1.SuspendLayout();
			this.SuspendLayout();
			// 
			// groupBox1
			// 
			this.groupBox1.Controls.Add(this.panel1);
			this.groupBox1.Controls.Add(this.label1);
			this.groupBox1.Location = new System.Drawing.Point(12, 12);
			this.groupBox1.Name = "groupBox1";
			this.groupBox1.Size = new System.Drawing.Size(342, 256);
			this.groupBox1.TabIndex = 0;
			this.groupBox1.TabStop = false;
			// 
			// panel1
			// 
			this.panel1.Controls.Add(this.label3);
			this.panel1.Controls.Add(this.label2);
			this.panel1.Controls.Add(this.tbVerify);
			this.panel1.Controls.Add(this.tbPassword);
			this.panel1.Controls.Add(this.rbPassword);
			this.panel1.Controls.Add(this.rbNoPassword);
			this.panel1.Location = new System.Drawing.Point(9, 75);
			this.panel1.Name = "panel1";
			this.panel1.Size = new System.Drawing.Size(314, 162);
			this.panel1.TabIndex = 0;
			// 
			// label3
			// 
			this.label3.AutoSize = true;
			this.label3.Location = new System.Drawing.Point(17, 126);
			this.label3.Name = "label3";
			this.label3.Size = new System.Drawing.Size(33, 13);
			this.label3.TabIndex = 2;
			this.label3.Text = "Verify";
			// 
			// label2
			// 
			this.label2.AutoSize = true;
			this.label2.Location = new System.Drawing.Point(17, 95);
			this.label2.Name = "label2";
			this.label2.Size = new System.Drawing.Size(53, 13);
			this.label2.TabIndex = 2;
			this.label2.Text = "Password";
			// 
			// tbVerify
			// 
			this.tbVerify.Location = new System.Drawing.Point(76, 123);
			this.tbVerify.Name = "tbVerify";
			this.tbVerify.Size = new System.Drawing.Size(188, 20);
			this.tbVerify.TabIndex = 3;
			this.tbVerify.UseSystemPasswordChar = true;
			// 
			// tbPassword
			// 
			this.tbPassword.Location = new System.Drawing.Point(76, 92);
			this.tbPassword.Name = "tbPassword";
			this.tbPassword.Size = new System.Drawing.Size(188, 20);
			this.tbPassword.TabIndex = 2;
			this.tbPassword.UseSystemPasswordChar = true;
			// 
			// rbPassword
			// 
			this.rbPassword.AutoSize = true;
			this.rbPassword.Checked = true;
			this.rbPassword.Location = new System.Drawing.Point(20, 55);
			this.rbPassword.Name = "rbPassword";
			this.rbPassword.Size = new System.Drawing.Size(253, 17);
			this.rbPassword.TabIndex = 1;
			this.rbPassword.TabStop = true;
			this.rbPassword.Text = "I want to protect my secret data with a password";
			this.rbPassword.UseVisualStyleBackColor = true;
			this.rbPassword.CheckedChanged += new System.EventHandler(this.rbPassword_CheckedChanged);
			// 
			// rbNoPassword
			// 
			this.rbNoPassword.AutoSize = true;
			this.rbNoPassword.Location = new System.Drawing.Point(20, 20);
			this.rbNoPassword.Name = "rbNoPassword";
			this.rbNoPassword.Size = new System.Drawing.Size(176, 17);
			this.rbNoPassword.TabIndex = 0;
			this.rbNoPassword.Text = "I do not want to use a password";
			this.rbNoPassword.UseVisualStyleBackColor = true;
			// 
			// label1
			// 
			this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
			this.label1.Location = new System.Drawing.Point(6, 16);
			this.label1.Name = "label1";
			this.label1.Size = new System.Drawing.Size(317, 56);
			this.label1.TabIndex = 2;
			this.label1.Text = "It is recommended that you use a password to protect your secret data from any ma" +
					"licious programs that might try and read and mimic your Authenticator.\r\n\r\n";
			// 
			// btnOk
			// 
			this.btnOk.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnOk.Location = new System.Drawing.Point(198, 277);
			this.btnOk.Name = "btnOk";
			this.btnOk.Size = new System.Drawing.Size(75, 23);
			this.btnOk.TabIndex = 0;
			this.btnOk.Text = "OK";
			this.btnOk.UseVisualStyleBackColor = true;
			this.btnOk.Click += new System.EventHandler(this.btnOk_Click);
			// 
			// btnCancel
			// 
			this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
			this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
			this.btnCancel.Location = new System.Drawing.Point(279, 277);
			this.btnCancel.Name = "btnCancel";
			this.btnCancel.Size = new System.Drawing.Size(75, 23);
			this.btnCancel.TabIndex = 1;
			this.btnCancel.Text = "Cancel";
			this.btnCancel.UseVisualStyleBackColor = true;
			// 
			// RequestPasswordForm
			// 
			this.AcceptButton = this.btnOk;
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.CancelButton = this.btnCancel;
			this.ClientSize = new System.Drawing.Size(372, 314);
			this.Controls.Add(this.btnCancel);
			this.Controls.Add(this.btnOk);
			this.Controls.Add(this.groupBox1);
			this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
			this.Name = "RequestPasswordForm";
			this.ShowIcon = false;
			this.Text = "Password";
			this.Load += new System.EventHandler(this.RequestPasswordForm_Load);
			this.groupBox1.ResumeLayout(false);
			this.panel1.ResumeLayout(false);
			this.panel1.PerformLayout();
			this.ResumeLayout(false);

		}

		#endregion

		private System.Windows.Forms.GroupBox groupBox1;
		private System.Windows.Forms.Panel panel1;
		private System.Windows.Forms.Label label3;
		private System.Windows.Forms.Label label2;
		private System.Windows.Forms.TextBox tbVerify;
		private System.Windows.Forms.TextBox tbPassword;
		private System.Windows.Forms.RadioButton rbPassword;
		private System.Windows.Forms.RadioButton rbNoPassword;
		private System.Windows.Forms.Label label1;
		private System.Windows.Forms.Button btnOk;
		private System.Windows.Forms.Button btnCancel;
	}
}