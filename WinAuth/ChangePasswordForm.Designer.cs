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
			this.metroCheckBox1 = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel1 = new MetroFramework.Controls.MetroLabel();
			this.metroLabel2 = new MetroFramework.Controls.MetroLabel();
			this.metroCheckBox2 = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel3 = new MetroFramework.Controls.MetroLabel();
			this.metroCheckBox3 = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel4 = new MetroFramework.Controls.MetroLabel();
			this.metroCheckBox4 = new MetroFramework.Controls.MetroCheckBox();
			this.metroLabel5 = new MetroFramework.Controls.MetroLabel();
			this.SuspendLayout();
			// 
			// metroCheckBox1
			// 
			this.metroCheckBox1.AutoSize = true;
			this.metroCheckBox1.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.metroCheckBox1.Location = new System.Drawing.Point(23, 105);
			this.metroCheckBox1.Name = "metroCheckBox1";
			this.metroCheckBox1.Size = new System.Drawing.Size(250, 19);
			this.metroCheckBox1.TabIndex = 0;
			this.metroCheckBox1.Text = "I do NOT want to use any protection";
			this.metroCheckBox1.UseSelectable = true;
			// 
			// metroLabel1
			// 
			this.metroLabel1.Location = new System.Drawing.Point(23, 60);
			this.metroLabel1.Name = "metroLabel1";
			this.metroLabel1.Size = new System.Drawing.Size(628, 42);
			this.metroLabel1.TabIndex = 1;
			this.metroLabel1.Text = "Select how you would like to protect your authenticators. A combination of user p" +
    "roection and your own password is recommended.";
			// 
			// metroLabel2
			// 
			this.metroLabel2.Location = new System.Drawing.Point(23, 127);
			this.metroLabel2.Name = "metroLabel2";
			this.metroLabel2.Size = new System.Drawing.Size(628, 49);
			this.metroLabel2.TabIndex = 1;
			this.metroLabel2.Text = "Your authenticators will not be protected by a password and will not be encrypted" +
    ". This is very insecure and should only be used if you are copying your authenti" +
    "cators between different computers.";
			// 
			// metroCheckBox2
			// 
			this.metroCheckBox2.AutoSize = true;
			this.metroCheckBox2.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.metroCheckBox2.Location = new System.Drawing.Point(23, 187);
			this.metroCheckBox2.Name = "metroCheckBox2";
			this.metroCheckBox2.Size = new System.Drawing.Size(213, 19);
			this.metroCheckBox2.TabIndex = 0;
			this.metroCheckBox2.Text = "Only useable on this computer";
			this.metroCheckBox2.UseSelectable = true;
			// 
			// metroLabel3
			// 
			this.metroLabel3.Location = new System.Drawing.Point(23, 209);
			this.metroLabel3.Name = "metroLabel3";
			this.metroLabel3.Size = new System.Drawing.Size(628, 69);
			this.metroLabel3.TabIndex = 1;
			this.metroLabel3.Text = resources.GetString("metroLabel3.Text");
			// 
			// metroCheckBox3
			// 
			this.metroCheckBox3.AutoSize = true;
			this.metroCheckBox3.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.metroCheckBox3.Location = new System.Drawing.Point(23, 280);
			this.metroCheckBox3.Name = "metroCheckBox3";
			this.metroCheckBox3.Size = new System.Drawing.Size(288, 19);
			this.metroCheckBox3.TabIndex = 0;
			this.metroCheckBox3.Text = "Only useable by this user on this computer";
			this.metroCheckBox3.UseSelectable = true;
			// 
			// metroLabel4
			// 
			this.metroLabel4.Location = new System.Drawing.Point(23, 302);
			this.metroLabel4.Name = "metroLabel4";
			this.metroLabel4.Size = new System.Drawing.Size(628, 69);
			this.metroLabel4.TabIndex = 1;
			this.metroLabel4.Text = resources.GetString("metroLabel4.Text");
			// 
			// metroCheckBox4
			// 
			this.metroCheckBox4.AutoSize = true;
			this.metroCheckBox4.FontSize = MetroFramework.MetroCheckBoxSize.Medium;
			this.metroCheckBox4.Location = new System.Drawing.Point(23, 387);
			this.metroCheckBox4.Name = "metroCheckBox4";
			this.metroCheckBox4.Size = new System.Drawing.Size(229, 19);
			this.metroCheckBox4.TabIndex = 0;
			this.metroCheckBox4.Text = "Protected with my own password";
			this.metroCheckBox4.UseSelectable = true;
			// 
			// metroLabel5
			// 
			this.metroLabel5.Location = new System.Drawing.Point(23, 409);
			this.metroLabel5.Name = "metroLabel5";
			this.metroLabel5.Size = new System.Drawing.Size(628, 69);
			this.metroLabel5.TabIndex = 1;
			this.metroLabel5.Text = resources.GetString("metroLabel5.Text");
			// 
			// ChangePasswordForm
			// 
			this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
			this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
			this.ClientSize = new System.Drawing.Size(674, 516);
			this.Controls.Add(this.metroLabel5);
			this.Controls.Add(this.metroLabel4);
			this.Controls.Add(this.metroCheckBox4);
			this.Controls.Add(this.metroLabel3);
			this.Controls.Add(this.metroCheckBox3);
			this.Controls.Add(this.metroLabel2);
			this.Controls.Add(this.metroCheckBox2);
			this.Controls.Add(this.metroLabel1);
			this.Controls.Add(this.metroCheckBox1);
			this.Name = "ChangePasswordForm";
			this.Text = "Change Password";
			this.ResumeLayout(false);
			this.PerformLayout();

		}

		#endregion

		private MetroFramework.Controls.MetroCheckBox metroCheckBox1;
		private MetroFramework.Controls.MetroLabel metroLabel1;
		private MetroFramework.Controls.MetroLabel metroLabel2;
		private MetroFramework.Controls.MetroCheckBox metroCheckBox2;
		private MetroFramework.Controls.MetroLabel metroLabel3;
		private MetroFramework.Controls.MetroCheckBox metroCheckBox3;
		private MetroFramework.Controls.MetroLabel metroLabel4;
		private MetroFramework.Controls.MetroCheckBox metroCheckBox4;
		private MetroFramework.Controls.MetroLabel metroLabel5;

	}
}