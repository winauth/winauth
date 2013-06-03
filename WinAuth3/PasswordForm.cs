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

namespace WinAuth
{
	/// <summary>
	/// Form to display to use to prompt for a password if they load a secured config
	/// </summary>
	public partial class PasswordForm : Form
	{
		/// <summary>
		/// Create for object
		/// </summary>
		public PasswordForm()
		{
			InitializeComponent();
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
			}
		}

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void PasswordForm_Load(object sender, EventArgs e)
		{
			this.ActiveControl = this.tbPassword;
		}

		/// <summary>
		/// Click OK button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click(object sender, EventArgs e)
		{
			if (this.tbPassword.Text.Length == 0)
			{
				MessageBox.Show(this, "Please enter a password", "Password", MessageBoxButtons.OK, MessageBoxIcon.Error);
				return;
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
