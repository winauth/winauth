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

using MetroFramework;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Form to check for update
	/// </summary>
	public partial class UpdateCheckForm : ResourceForm
	{
		/// <summary>
		/// Internal item for drop down to hold interval information
		/// </summary>
		class UpdateIntervalItem
		{
			/// <summary>
			/// Display name
			/// </summary>
			public string Name { get; set; }

			/// <summary>
			/// Timespan for interval
			/// </summary>
			public TimeSpan Interval { get; set; }

			/// <summary>
			/// Override string to show display name
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return Name;
			}
		}

		/// <summary>
		/// The current WinAuthUpdate object set by parent
		/// </summary>
		public WinAuthUpdater Updater { get; set; }

		/// <summary>
		/// Create the new form
		/// </summary>
		public UpdateCheckForm()
		{
			InitializeComponent();

			// set the font for the HtmlLabel to match the form
			Font font = MetroFonts.Label(MetroLabelSize.Small, MetroLabelWeight.Regular);
			this.versionInfoLabel.Font = new Font(font.FontFamily, 10);
		}

		#region Form events

		private void UpdateCheckForm_Load(object sender, EventArgs e)
		{
			// set the initial status
			versionInfoLabel.Text = GetHtmlText("<p>Latest Version: checking</p></body></html>");

			// set checkboxes and dropdown
			autoDropdown.Items.Clear();
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every Time", Interval = new TimeSpan(0, 0, 0, 0) });
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every Day", Interval = new TimeSpan(1, 0, 0, 0) });
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every 3 Days", Interval = new TimeSpan(3, 0, 0, 0) });
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every Week", Interval = new TimeSpan(7, 0, 0, 0) });
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every 2 Weeks", Interval = new TimeSpan(14, 0, 0, 0) });
			autoDropdown.Items.Add(new UpdateIntervalItem { Name = "Every Month", Interval = new TimeSpan(30, 0, 0, 0) });
			//
			if (Updater.IsAutoCheck == false)
			{
				autoCheckbox.Checked = false;
				autoDropdown.SelectedIndex = -1;
			}
			else
			{
				autoCheckbox.Checked = true;
				TimeSpan? interval = Updater.UpdateInterval;
				if (interval != null)
				{
					foreach (UpdateIntervalItem item in autoDropdown.Items)
					{
						if (item.Interval == interval)
						{
							autoDropdown.SelectedItem = item;
							break;
						}
					}
				}
			}

			// initiate async call to get latest version
			Updater.GetLatestVersion(Updater_GetLatestVersionCompleted);
		}

		/// <summary>
		/// Click the OK button and update the Updater
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			if (autoCheckbox.Checked == true)
			{
				// get the interval and set in the updater
				UpdateIntervalItem interval = autoDropdown.SelectedItem as UpdateIntervalItem;
				Updater.SetUpdateInterval(interval.Interval);
			}
			else
			{
				Updater.SetUpdateInterval(null);
			}
		}

		/// <summary>
		/// Set the choice of updating if we check the box
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (autoCheckbox.Checked == true && autoDropdown.SelectedIndex < 0)
			{
				autoDropdown.SelectedIndex = 0;
			}
		}

		/// <summary>
		/// Click the label and act as if checkbox was clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoLabel_Click(object sender, EventArgs e)
		{
			autoCheckbox.Checked = !autoCheckbox.Checked;
		}

		/// <summary>
		/// If interval is set, make sure checkbox is checked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void autoDropdown_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (autoDropdown.SelectedIndex >= 0)
			{
				autoCheckbox.Checked = true;
			}
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Build a suitable html string for the html control for best display
		/// </summary>
		/// <param name="message">string formatted body message</param>
		/// <param name="args">optional message arguments</param>
		/// <returns>html formatted string</returns>
		private string GetHtmlText(string message, params string[] args)
		{
			message = string.Format(message, args);
			return string.Format("<html><head><style>body {{width:" + (versionInfoLabel.Width - versionInfoLabel.Margin.Horizontal - versionInfoLabel.Padding.Horizontal - SystemInformation.VerticalScrollBarWidth) + "px;}} html,body,div,p,ul {{margin:0;padding:0;}} p {{padding:0; margin:0 0 8px 0;}} </style></head><body>{0}</body></html>", message);
		}

		/// <summary>
		/// Callback from Updater object with latest version information
		/// </summary>
		/// <param name="latestInfo">latest version or null</param>
		/// <param name="cancelled">flag if operation was cancelled</param>
		/// <param name="error">any error exception</param>
		void Updater_GetLatestVersionCompleted(WinAuthVersionInfo latestInfo, bool cancelled, Exception error)
		{
			if (this.IsDisposed == true || IsHandleCreated == false)
			{
				return;
			}

			string text = string.Empty;
			if (cancelled == true)
			{
				text = "Update was cancelled";
			}
			else if (error != null)
			{
				text = GetHtmlText("Error: " + error.Message);
				//text = "Error: " + error.Message;
			}
			else
			{
				string latest = latestInfo.Version.ToString(3);
				string current = Updater.CurrentVersion.ToString(3);
				if (current == latest)
				{
					text = GetHtmlText("<p>Latest Version: {0}</p><p>You are on the latest version.</p>", latest);
				}
				else
				{
					string info = string.Format("<p>Latest Version: {0} (yours {1})</p><p><a href=\"{2}\">Download version {0}</a></p></body></html>", latest, current, latestInfo.Url);
					text = GetHtmlText("{0}", info);
				}
			}

			// update the textbox in a delegate
			this.Invoke((MethodInvoker)delegate { versionInfoLabel.Text = text; });
		}

		#endregion
	}
}
