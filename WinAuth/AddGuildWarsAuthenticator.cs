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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Web;

using ZXing;

namespace WinAuth
{
	/// <summary>
	/// Form class for create a new Battle.net authenticator
	/// </summary>
	public partial class AddGuildWarsAuthenticator : ResourceForm
	{
		/// <summary>
		/// Form instantiation
		/// </summary>
		public AddGuildWarsAuthenticator()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current authenticator
		/// </summary>
		public WinAuthAuthenticator Authenticator { get; set; }

#region Form Events

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddGuildWarsAuthenticator_Load(object sender, EventArgs e)
		{
			nameField.Text = this.Authenticator.Name;
			codeField.SecretMode = true;
		}

		/// <summary>
		/// Ticker event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void newAuthenticatorTimer_Tick(object sender, EventArgs e)
		{
			if (this.Authenticator.AuthenticatorData != null && newAuthenticatorProgress.Visible == true)
			{
				int time = (int)(this.Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
				newAuthenticatorProgress.Value = time + 1;
				if (time == 0)
				{
					codeField.Text = this.Authenticator.AuthenticatorData.CurrentCode;
				}
			}
		}

		private void allowCopyButton_CheckedChanged(object sender, EventArgs e)
		{
			codeField.SecretMode = !allowCopyButton.Checked;
		}

		/// <summary>
		/// Click to add the code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void verifyAuthenticatorButton_Click(object sender, EventArgs e)
		{
			string privatekey = this.secretCodeField.Text.Trim();
			if (string.IsNullOrEmpty(privatekey) == true)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the secret code");
				return;
			}

			verifyAuthenticator(privatekey);
		}

		/// <summary>
		/// Click the cancel button and show a warning
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			if (this.Authenticator.AuthenticatorData != null)
			{
				DialogResult result = WinAuthForm.ConfirmDialog(this.Owner,
					"WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
					+ "If you have added this authenticator to your account, you will not be able to login in the future, and you need to click YES to save it." + Environment.NewLine + Environment.NewLine
					+ "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
				if (result == System.Windows.Forms.DialogResult.Yes)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.OK;
					return;
				}
				else if (result == System.Windows.Forms.DialogResult.Cancel)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}
			}
		}

		/// <summary>
		/// Click the OK button to verify and add the authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			string privatekey = this.secretCodeField.Text.Trim();
			if (privatekey.Length == 0)
			{
				WinAuthForm.ErrorDialog(this.Owner, "Please enter the Secret Code");
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			bool first = !newAuthenticatorProgress.Visible;
			if (verifyAuthenticator(privatekey) == false)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			if (first == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			if (this.Authenticator.AuthenticatorData == null)
			{
				WinAuthForm.ErrorDialog(this.Owner, "Please enter the Secret Code and click Verify Authenticator");
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked == true)
			{
				this.Authenticator.Skin = (string)((RadioButton)sender).Tag;
			}
		}

		/// <summary>
		/// Click the icon1
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void icon1_Click(object sender, EventArgs e)
		{
			icon1RadioButton.Checked = true;
		}

		/// <summary>
		/// Click the icon2
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void icon2_Click(object sender, EventArgs e)
		{
			icon2RadioButton.Checked = true;
		}

#endregion

#region Private methods

		/// <summary>
		/// Verify and create the authenticator if needed
		/// </summary>
		/// <returns>true is successful</returns>
		private bool verifyAuthenticator(string privatekey)
		{
			this.Authenticator.Name = nameField.Text;

			try
			{
				GuildWarsAuthenticator authenticator = new GuildWarsAuthenticator();
				authenticator.Enroll(privatekey);
				this.Authenticator.AuthenticatorData = authenticator;
				this.Authenticator.Name = this.nameField.Text;

				codeField.Text = authenticator.CurrentCode;
				newAuthenticatorProgress.Visible = true;
				newAuthenticatorTimer.Enabled = true;
			}
			catch (Exception ex)
			{
				WinAuthForm.ErrorDialog(this.Owner, "Unable to create the authenticator: " + ex.Message, ex);
				return false;
			}

			return true;
		}

#endregion

	}
}