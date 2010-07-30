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
		public bool UsePassword
		{
			get
			{
				return rbPassword.Checked;
			}
			set
			{
				rbNoPassword.Checked = !value;
				rbPassword.Checked = value;
				tbPassword.Enabled = value;
				tbVerify.Enabled = value;
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
				UsePassword = string.IsNullOrEmpty(value);
			}
		}

		/// <summary>
		/// Load the form. Default to use a password.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RequestPasswordForm_Load(object sender, EventArgs e)
		{
			UsePassword = true;
			this.ActiveControl = tbPassword;
		}

		/// <summary>
		/// Toggle password choice
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void rbPassword_CheckedChanged(object sender, EventArgs e)
		{
			UsePassword = rbPassword.Checked;
		}

		/// <summary>
		/// Click OK and verify form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (UsePassword == true && this.tbPassword.Text.Length == 0)
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

	}
}
