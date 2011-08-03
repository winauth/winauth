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
	/// Form display initialization confirmation.
	/// </summary>
	public partial class RestoreForm : Form
	{
		/// <summary>
		/// Current authenticator
		/// </summary>
		public Authenticator Authenticator { get; set; }

		/// <summary>
		/// Create a new form
		/// </summary>
		public RestoreForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Click OK button to close form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Form loaded event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void RestoreForm_Load(object sender, EventArgs e)
		{
			serial1Field.Focus();
		}

		/// <summary>
		/// Upper case the region part of the serial
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void serial1Field_Leave(object sender, EventArgs e)
		{
			serial1Field.Text = serial1Field.Text.ToUpper();
		}

		/// <summary>
		/// Click the OK button to restore the authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOk_Click_1(object sender, EventArgs e)
		{
			string serial = (serial1Field.Text + serial2Field.Text + serial3Field.Text + serial4Field.Text).Trim().ToUpper();
			if (serial.Length != 14)
			{
				MessageBox.Show(this, "Invalid serial number.\n\n - must be something like US-1234-1234-1234.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			string restorecode = restoreCodeField.Text.Trim().ToUpper();
			if (restorecode.Length != 10)
			{
				MessageBox.Show(this, "Invalid restore code.\n\n - must contain 10 letters and numbers.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			// restore the authenticator
			try
			{
				Authenticator auth = new Authenticator();
				auth.Restore(serial, restorecode);
				this.Authenticator = auth;

				MessageBox.Show(this, "Your authenticator has been restored.\n\nYou will now be prompted where to save your new authenticator and choose your encryption level.", WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Information);

				// ok to continue
				DialogResult = System.Windows.Forms.DialogResult.OK;
			}
			catch (InvalidRestoreResponseException re)
			{
				MessageBox.Show(this, re.Message, WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

		}

	}
}
