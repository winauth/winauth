/*
 * Copyright (C) 2010 Colin Mackie.
 * This software is distributed under the terms of the GNU General Public License.
 *
 * This program is free software: you can redistribute it and/or modify
 * it under the terms of the GNU General Public License as published by
 * the Free Software Foundation, either version 3 of the License, or
 * (at your option) any later version.
 *
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 *
 * You should have received a copy of the GNU General Public License
 * along with this program.  If not, see <http://www.gnu.org/licenses/>.
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Form to prompt the user to create a password to encrpyt the config data
	/// </summary>
	public partial class RequestPasswordForm : Form
	{
		/// <summary>
		/// Create the form object
		/// </summary>
		public RequestPasswordForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get/set flag to use a password
		/// </summary>
		public Authenticator.PasswordTypes PasswordType
		{
			get
			{
				if (rbAccountPassword.Checked == true)
				{
					return Authenticator.PasswordTypes.User;
				}
				else if (rbMachinePassword.Checked == true)
				{
					return Authenticator.PasswordTypes.Machine;
				}
				else if (rbPassword.Checked == true)
				{
					return Authenticator.PasswordTypes.Explicit;
				}
				else
				{
					return Authenticator.PasswordTypes.None;
				}
			}
			set
			{
				if (value == Authenticator.PasswordTypes.User)
				{
					rbAccountPassword.Checked = true;
				}
				else if (value == Authenticator.PasswordTypes.Machine)
				{
					rbMachinePassword.Checked = true;
				}
				else if (value == Authenticator.PasswordTypes.Explicit)
				{
					rbPassword.Checked = true;
				}
				else
				{
					rbNoPassword.Checked = true;
				}

				tbPassword.Enabled = (value == Authenticator.PasswordTypes.Explicit);
				tbVerify.Enabled = (value == Authenticator.PasswordTypes.Explicit);
			}
		}

		/// <summary>
		/// Get/set the password
		/// </summary>
		public string Password
		{
			get
			{
				return tbPassword.Text;
			}
			set
			{
				tbVerify.Text = value;
				tbVerify.Text = value;
			}
		}

		/// <summary>
		/// Load the form. Default to use a password.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RequestPasswordForm_Load(object sender, EventArgs e)
		{
			rbNoPassword.Tag = Authenticator.PasswordTypes.None;
			rbAccountPassword.Tag = Authenticator.PasswordTypes.User;
			rbMachinePassword.Tag = Authenticator.PasswordTypes.Machine;
			rbPassword.Tag = Authenticator.PasswordTypes.Explicit;

			rbAccountPassword.Text += " (" + System.Environment.UserName + ")";
			rbMachinePassword.Text += " (" + System.Environment.MachineName + ")";

			//this.ActiveControl = rbNoPassword;
		}

		/// <summary>
		/// Click OK and verify form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (rbAccountPassword.Checked == false && rbMachinePassword.Checked == false && rbNoPassword.Checked == false && rbPassword.Checked == false)
			{
				MessageBox.Show(this, "Please choose a password method.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (PasswordType == Authenticator.PasswordTypes.Explicit && this.tbPassword.Text.Length == 0)
			{
				MessageBox.Show(this, "Please enter a password.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tbPassword.Focus();
				return;
			}
			if (this.tbPassword.Text.Length != 0 && string.Compare(this.tbPassword.Text, this.tbVerify.Text) != 0)
			{
				MessageBox.Show(this, "Your passwords do not match.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				tbPassword.Focus();
				tbPassword.SelectAll();
				return;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		/// <summary>
		/// Click any of the password type radio button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PasswordType_CheckedChanged(object sender, EventArgs e)
		{
			if (sender is RadioButton && ((RadioButton)sender).Checked == true)
			{
				this.PasswordType = (Authenticator.PasswordTypes)((RadioButton)sender).Tag;
				if (this.PasswordType == Authenticator.PasswordTypes.Explicit)
				{
					tbPassword.Focus();
				}
			}
		}

	}
}
