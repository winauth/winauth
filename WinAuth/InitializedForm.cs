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
	public partial class InitializedForm : Form
	{
		/// <summary>
		/// Current authenticator
		/// </summary>
		public Authenticator Authenticator { get; set; }

		/// <summary>
		/// Time for next code refresh
		/// </summary>
		private DateTime NextRefresh {get; set;}

		/// <summary>
		/// Create a new form
		/// </summary>
		public InitializedForm()
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
		private void InitializedForm_Load(object sender, EventArgs e)
		{
			this.serialNumberField.SecretMode = true;
			this.serialNumberField.Text = Authenticator.Serial;
			this.codeField.SecretMode = true;
			this.codeField.Text = Authenticator.CurrentCode;
			this.restoreCodeField.SecretMode = true;
			this.restoreCodeField.Text = Authenticator.RestoreCode;
		}

		/// <summary>
		/// Tick event to update bar and code
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;

			int tillUpdate = (int)((Authenticator.ServerTime % 30000L) / 1000L);
			progressBar.Value = tillUpdate;
			if (tillUpdate == 0)
			{
				NextRefresh = now;
			}
			if (now >= NextRefresh)
			{
				NextRefresh = now.AddSeconds(30);
				this.codeField.Text = Authenticator.CurrentCode;
			}
		}

	}
}
