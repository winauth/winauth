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
	/// Class for form that prompts for password and unprotects authenticator
	/// </summary>
	public partial class UnprotectPasswordForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public UnprotectPasswordForm() : base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Authenticator to unprotect
		/// </summary>
		public WinAuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Click the OK button to unprotect the authenticator with given password
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// it isn't empty
			string password = this.passwordField.Text.Trim();
			if (password.Length == 0)
			{
				invalidPasswordLabel.Text = strings.EnterPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			// try to unprotect
			try
			{
				Authenticator.AuthenticatorData.Unprotect(password);
			}
			catch (BadPasswordException)
			{
				invalidPasswordLabel.Text = strings.InvalidPassword;
				invalidPasswordLabel.Visible = true;
				invalidPasswordTimer.Enabled = true;
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
		}

		/// <summary>
		/// Display error message for couple seconds
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void invalidPasswordTimer_Tick(object sender, EventArgs e)
		{
			invalidPasswordTimer.Enabled = false;
			invalidPasswordLabel.Visible = false;
		}
	}
}
