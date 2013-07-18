using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace WinAuth
{
	public partial class SetPasswordForm : ResourceForm
	{
		public SetPasswordForm()
		{
			InitializeComponent();
		}

		public string Password { get; protected set; }

		private void showCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (this.showCheckbox.Checked == true)
			{
				this.passwordField.UseSystemPasswordChar = false;
				this.passwordField.PasswordChar = (char)0;
				this.verifyField.UseSystemPasswordChar = false;
				this.verifyField.PasswordChar = (char)0;
			}
			else
			{
				this.passwordField.UseSystemPasswordChar = true;
				this.passwordField.PasswordChar = '*';
				this.verifyField.UseSystemPasswordChar = true;
				this.verifyField.PasswordChar = '*';
			}
		}

		private void okButton_Click(object sender, EventArgs e)
		{
			string password = this.passwordField.Text.Trim();
			string verify = this.verifyField.Text.Trim();
			if (password != verify)
			{
				WinAuthForm.ErrorDialog(this, "Your passwords do not match.");
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			this.Password = password;
		}
	}
}
