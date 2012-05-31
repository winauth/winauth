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
				Authenticator.PasswordTypes passwordType = Authenticator.PasswordTypes.None;
				if (ckUserPassword.Checked == true)
				{
					passwordType |= Authenticator.PasswordTypes.User;
				}
				if (ckAccountPassword.Checked == true)
				{
					passwordType |= Authenticator.PasswordTypes.Machine;
				}
				if (ckMyPassword.Checked == true)
				{
					passwordType |= Authenticator.PasswordTypes.Explicit;
				}
				return passwordType;
			}
			set
			{
				if ((value & Authenticator.PasswordTypes.User) != 0)
				{
					ckUserPassword.Checked = true;
				}
				if ((value & Authenticator.PasswordTypes.Machine) != 0)
				{
					ckAccountPassword.Checked = true;
				}
				if ((value & Authenticator.PasswordTypes.Explicit) != 0)
				{
					ckMyPassword.Checked = true;
				}
				if (value == Authenticator.PasswordTypes.None)
				{
					ckNoPassword.Checked = true;
				}

				tbPassword.Enabled = ((value & Authenticator.PasswordTypes.Explicit) != 0);
				tbVerify.Enabled = ((value & Authenticator.PasswordTypes.Explicit) != 0);
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
				tbPassword.Text = value;
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
			ckUserPassword.Text = ckUserPassword.Text.Replace("current Windows User account", "user '" + System.Environment.UserName + "'");
			ckAccountPassword.Text = ckAccountPassword.Text.Replace("computer", "computer (" + System.Environment.MachineName + ")");
			//rbAccountPassword.Text += " (" + System.Environment.UserName + ")";
			//rbMachinePassword.Text += " (" + System.Environment.MachineName + ")";

			//ckUserPassword.Checked = true;
			//ckMyPassword.Checked = true;
		}

		/// <summary>
		/// Click OK and verify form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (ckNoPassword.Checked == false && ckUserPassword.Checked == false && ckAccountPassword.Checked == false && ckMyPassword.Checked == false)
			{
				MessageBox.Show(this, "Please choose a password method or select no password.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (ckMyPassword.Checked == true && this.tbPassword.Text.Length == 0)
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
		//private void PasswordType_CheckedChanged(object sender, EventArgs e)
		//{
		//  if (sender is RadioButton && ((RadioButton)sender).Checked == true)
		//  {
		//    this.PasswordType = (Authenticator.PasswordTypes)((RadioButton)sender).Tag;
		//    if (this.PasswordType == Authenticator.PasswordTypes.Explicit)
		//    {
		//      tbPassword.Focus();
		//    }
		//  }
		//}

		/// <summary>
		/// Check the no password box, disable other choices
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckNoPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (ckNoPassword.Checked == true)
			{
				ckAccountPassword.Enabled = false;
				ckAccountPassword.Checked = false;
				ckUserPassword.Enabled = false;
				ckUserPassword.Checked = false;
				ckMyPassword.Enabled = false;
				ckMyPassword.Checked = false;
				tbPassword.Text = "";
				tbVerify.Text = "";
			}
			else
			{
				ckAccountPassword.Enabled = true;
				ckUserPassword.Enabled = true;
				ckMyPassword.Enabled = true;
			}
		}

		/// <summary>
		/// Check the my password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckMyPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (ckMyPassword.Checked == true)
			{
				tbPassword.Enabled = true;
				tbVerify.Enabled = true;
				tbPassword.Focus();
			}
			else
			{
				tbPassword.Text = "";
				tbPassword.Enabled = false;
				tbVerify.Text = "";
				tbVerify.Enabled = false;
			}
		}

		/// <summary>
		/// Check the user encryption choice
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckUserPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (ckUserPassword.Checked == true)
			{
				ckAccountPassword.Checked = false;
			}
		}

		/// <summary>
		/// Check the account encryption choice
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckAccountPassword_CheckedChanged(object sender, EventArgs e)
		{
			if (ckAccountPassword.Checked == true)
			{
				ckUserPassword.Checked = false;
			}
		}

	}
}
