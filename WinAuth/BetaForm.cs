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
using System.Text;
using System.Windows.Forms;

namespace WinAuth
{
	/// <summary>
	/// General Beta form
	/// </summary>
	public partial class BetaForm : ResourceForm
	{
		/// <summary>
		/// Create the  Form
		/// </summary>
		public BetaForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Click the OK button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnOK_Click(object sender, EventArgs e)
		{
			if (this.ckAgree.Checked == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
				this.Close();
			}
		}

		/// <summary>
		/// Check the agree tickbox
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckAgree_CheckedChanged(object sender, EventArgs e)
		{
			btnOK.Enabled = ckAgree.Checked;
		}

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BetaForm_Load(object sender, EventArgs e)
		{
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			this.Text = string.Format(this.Text, version.ToString(3), DateTime.Today.Year);
		}

	}
}
