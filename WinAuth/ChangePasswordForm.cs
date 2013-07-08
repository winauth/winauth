/*
 * Copyright (C) 2013 Colin Mackie.
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
using System.Linq;
using System.Text;
using System.Windows.Forms;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Form for setting the password and encryption for the current authenticators
	/// </summary>
	public partial class ChangePasswordForm : WinAuth.ResourceForm
	{
		/// <summary>
		/// Create the form
		/// </summary>
		public ChangePasswordForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current and new password type
		/// </summary>
		public Authenticator.PasswordTypes PasswordType { get; set; }

		/// <summary>
		/// Current and new password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// Load the form and pretick checkboxes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangePasswordForm_Load(object sender, EventArgs e)
		{
			if (PasswordType == Authenticator.PasswordTypes.None)
			{
				noneCheckbox.Checked = true;
			}
			else if ((PasswordType & Authenticator.PasswordTypes.Machine) != 0)
			{
				machineCheckbox.Checked = true;
			}
			else if ((PasswordType & Authenticator.PasswordTypes.User) != 0)
			{
				userCheckbox.Checked = true;
			}
			if ((PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
			{
				passwordCheckbox.Checked = true;
			}
		}

		/// <summary>
		/// Form has been shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangePasswordForm_Shown(object sender, EventArgs e)
		{
			// Buf in MetroFrame where focus is not set correcty during Load, so we do it here
			if (passwordField.Enabled == true)
			{
				passwordField.Focus();
			}
		}

		/// <summary>
		/// None is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void noneCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (noneCheckbox.Checked == true)
			{
				userCheckbox.Checked = false;
				machineCheckbox.Checked = false;
				passwordCheckbox.Checked = false;
				passwordField.Text = verifyField.Text = string.Empty;
			}
		}

		/// <summary>
		/// Machine encryption is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void machineCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (machineCheckbox.Checked == true)
			{
				noneCheckbox.Checked = false;
				userCheckbox.Checked = false;
			}
		}

		/// <summary>
		/// User encrpytion is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void userCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (userCheckbox.Checked == true)
			{
				noneCheckbox.Checked = false;
				machineCheckbox.Checked = false;
			}
		}

		/// <summary>
		/// Password encrpytion is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			passwordField.Enabled = (passwordCheckbox.Checked);
			verifyField.Enabled = (passwordCheckbox.Checked);
			if (passwordCheckbox.Checked == true)
			{
				noneCheckbox.Checked = false;
				passwordField.Focus();
			}
		}

		/// <summary>
		/// OK button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// check password is set if requried
			if (passwordCheckbox.Checked == true && passwordField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, strings.EnterPassword);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			if (passwordCheckbox.Checked == true && string.Compare(passwordField.Text, verifyField.Text) != 0)
			{
				WinAuthForm.ErrorDialog(this, strings.PasswordsDontMatch);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			// set the valid password type property
			PasswordType = Authenticator.PasswordTypes.None;
			Password = null;
			if (userCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.User;
			}
			else if (machineCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.Machine;
			}
			if (passwordCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.Explicit;
				Password = this.passwordField.Text;
			}
		}

	}
}
