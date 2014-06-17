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
	/// Class for form that prompts for password and unprotects authenticator
	/// </summary>
	public partial class GetPGPKeyForm : ResourceForm
	{
		/// <summary>
		/// Create new form
		/// </summary>
		public GetPGPKeyForm()
			: base()
		{
			InitializeComponent();
		}

		/// <summary>
		/// PGPKey
		/// </summary>
		public string PGPKey { get; private set; }

		/// <summary>
		/// Password
		/// </summary>
		public string Password { get; private set; }

		/// <summary>
		/// Load the form and make it topmost
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void GetPGPKeyForm_Load(object sender, EventArgs e)
		{
			// force this window to the front and topmost
			// see: http://stackoverflow.com/questions/278237/keep-window-on-top-and-steal-focus-in-winforms
			var oldtopmost = this.TopMost;
			this.TopMost = true;
			this.TopMost = oldtopmost;
			this.Activate();
		}

		/// <summary>
		/// Browse the PGP key
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void browseButton_Click(object sender, EventArgs e)
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

		/// <summary>
		/// Click the OK button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// it isn't empty
			if (this.pgpField.Text.Length == 0)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			this.PGPKey = this.pgpField.Text;
			this.Password = this.passwordField.Text;
		}

	}
}
