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
	/// General enroll form to choose region
	/// </summary>
	public partial class EnrollForm : Form
	{
		/// <summary>
		/// Inner class holding the regions information
		/// </summary>
		class BattleNetRegion
		{
			/// <summary>
			/// Code for region, e.g. "US"
			/// </summary>
			public string Code;

			/// <summary>
			/// Display name of region, e.g. "Americas"
			/// </summary>
			public string Name;

			/// <summary>
			/// Create a new BattleNetRegion object
			/// </summary>
			/// <param name="code">region code</param>
			/// <param name="name">region name</param>
			public BattleNetRegion(string code, string name)
			{
				Code = code;
				Name = name;
			}

			/// <summary>
			/// Get the display sting for the region
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return Name;
			}
		}

		/// <summary>
		/// List of known battle.net regions
		/// </summary>
		private static List<BattleNetRegion> Regions = new List<BattleNetRegion>
		{
			new BattleNetRegion("US", "Americas/Oceania"),
			new BattleNetRegion("EU", "Europe/Russia"),
			new BattleNetRegion("CN", "China"),
			new BattleNetRegion("KR", "Korea"),
			new BattleNetRegion("TW", "Taiwan")
		};

		/// <summary>
		/// Quicklist of ISO3166 country codes that are in EU region
		/// </summary>
		private static List<string> EU_COUNTRIES = new List<string>
		{
			"AT", "BA", "BE", "BG", "CH", "CZ", "DE", "DK", "ES", "FI", "FR", "GB", "GG", "GI", "GR", "HR", "HU", "IE", "IM", "IS", "IT", "JE", "LT", "LU", "LV", "MD", "ME", "MT", "NL", "NO", "PL", "PT", "RO", "RU", "SE", "SI", "UK"
		};

		/// <summary>
		/// current region code
		/// </summary>
		private string m_currentRegion;

		/// <summary>
		/// Create the  Form
		/// </summary>
		public EnrollForm()
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
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnrollForm_Load(object sender, EventArgs e)
		{
			string region = CurrentRegion;
			this.regionsList.Items.Clear();
			foreach (BattleNetRegion br in Regions)
			{
				this.regionsList.Items.Add(br);
				if (string.Compare(br.Code, region) == 0)
				{
					regionsList.SelectedItem = br;
				}
			}
			if (regionsList.SelectedItem == null)
			{
				regionsList.SelectedItem = regionsList.Items[0];
			}
		}

		/// <summary>
		/// Get/set the current region
		/// </summary>
		public string CurrentRegion
		{
			get
			{
				return m_currentRegion;
			}
			set
			{
				string region = value.ToUpper();
				if (string.IsNullOrEmpty(region) == false && region != "CN" && region != "US" && region != "KR" && region != "TW")
				{
					// rewrite obvious ones
					if (EU_COUNTRIES.Contains(region) == true)
					{
						region = "EU";
					}
					else if (region == "CA" || region == "NZ" || region == "AU" || region == "BR")
					{
						region = "US";
					}
					else if (region == "KP")
					{
						region = "KR";
					}
				}

				m_currentRegion = region;
			}
		}

		/// <summary>
		/// Change the region selection
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void regionsList_SelectedIndexChanged(object sender, EventArgs e)
		{
			CurrentRegion = ((BattleNetRegion)regionsList.SelectedItem).Code;
		}

	}

}
