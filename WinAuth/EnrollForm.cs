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
	/// Form to register agree to register a new Authenticator
	/// </summary>
	public partial class EnrollForm : Form
	{
		public EnrollForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get the selected region from the Form
		/// </summary>
		public string SelectedRegion
		{
			get
			{
				if (rbRegionUS.Checked == true)
				{
					return "US";
				}
				else // if (rbRegionUS.Checked == true)
				{
					return "EU";
				}
			}
		}

		/// <summary>
		/// Load the form and set defaults
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnrollForm_Load(object sender, EventArgs e)
		{
			rbRegionUS.Checked = true;
		}
	}
}
