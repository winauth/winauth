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
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Form for exporting authenticators to a file
	/// </summary>
	public partial class ExportForm : WinAuth.ResourceForm
	{
		/// <summary>
		/// Create the form
		/// </summary>
		public ExportForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Protect password
		/// </summary>
		public string Password { get; protected set; }

		/// <summary>
		/// Protect PGP key
		/// </summary>
		public string PGPKey { get; protected set; }

		/// <summary>
		/// Export file
		/// </summary>
		public string ExportFile { get; protected set; }

		/// <summary>
		/// Load the form and pretick checkboxes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExportForm_Load(object sender, EventArgs e)
		{
		}

		/// <summary>
		/// Form has been shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExportForm_Shown(object sender, EventArgs e)
		{
			// Buf in MetroFrame where focus is not set correcty during Load, so we do it here
			//if (fileField.Enabled == true)
			//{
			//	fileField.Focus();
			//}
		}

		/// <summary>
		/// Password encryption is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			pgpCheckbox.Checked = false;

			passwordField.Enabled = (passwordCheckbox.Checked);
			verifyField.Enabled = (passwordCheckbox.Checked);
			if (passwordCheckbox.Checked == true)
			{
				passwordField.Focus();
			}
		}

		private void pgpCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			passwordField.Enabled = false;

			pgpField.Enabled = pgpCheckbox.Checked;
			if (pgpCheckbox.Checked == true)
			{
				pgpField.Focus();
			}
		}

		/// <summary>
		/// Use the browse button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pgpBrowseButton_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.CheckFileExists = true;
			ofd.Filter = "All Files (*.*)|*.*";
			ofd.Title = "Choose PGP Key File";

			if (ofd.ShowDialog(this.Parent) == System.Windows.Forms.DialogResult.OK)
			{
				this.pgpField.Text = File.ReadAllText(ofd.FileName);
			}
		}

		private void browseButton_Click(object sender, EventArgs e)
		{
			SaveFileDialog sfd = new SaveFileDialog();
			sfd.AddExtension = true;
			sfd.CheckPathExists = true;
			if (passwordCheckbox.Checked == true)
			{
				sfd.Filter = "Zip File (*.zip)|*.zip";
				sfd.FileName = "winauth-" + DateTime.Today.ToString("yyyy-MM-dd") + ".zip";
			}
			else if (pgpCheckbox.Checked == true)
			{
				sfd.Filter = "PGP File (*.pgp)|*.pgp";
				sfd.FileName = "winauth-" + DateTime.Today.ToString("yyyy-MM-dd") + ".pgp";
			}
			else
			{
				sfd.Filter = "Text File (*.txt)|*.txt|Zip File (*.zip)|*.zip|All Files (*.*)|*.*";
				sfd.FileName = "winauth-" + DateTime.Today.ToString("yyyy-MM-dd") + ".txt";
			}
			sfd.OverwritePrompt = true;
			if (sfd.ShowDialog(this.Parent) != System.Windows.Forms.DialogResult.OK)
			{
				return;
			}

			this.fileField.Text = sfd.FileName;
		}

		/// <summary>
		/// OK button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// check password is set if required
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

			if (pgpCheckbox.Checked == true && this.pgpField.Text.Length == 0)
			{
				WinAuthForm.ErrorDialog(this, strings.MissingPGPKey);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			if (this.fileField.Text.Length == 0)
			{
				WinAuthForm.ErrorDialog(this, strings.MissingFile);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			// set the valid password type property
			this.ExportFile = this.fileField.Text;
			if (this.passwordCheckbox.Checked && this.passwordField.Text.Length != 0)
			{
				this.Password = this.passwordField.Text;
			}
			if (this.pgpCheckbox.Checked && this.pgpField.Text.Length != 0)
			{
				this.PGPKey = this.pgpField.Text;
			}
		}

	}

}
