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
using System.Net;
using System.Text;
using System.Windows.Forms;
using System.Xml;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Show the About form
	/// </summary>
	public partial class AboutForm : ResourceForm
	{
		/// <summary>
		/// Current config object
		/// </summary>
		public WinAuthConfig Config { get; set; }

		/// <summary>
		/// Create the form
		/// </summary>
		public AboutForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the about form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AboutForm_Load(object sender, EventArgs e)
		{
			// get the version of the application
			Version version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
			string debug = string.Empty;
#if NETFX_3
			debug += " for .NET 3.5";
#endif
#if BETA
			debug += " (BETA)";
#endif
#if DEBUG
			debug += " (DEBUG)";
#endif
			this.aboutLabel.Text = string.Format(this.aboutLabel.Text, version.ToString(3) + debug, DateTime.Today.Year);
		}

		/// <summary>
		/// Click the close button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Click the report button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void reportButton_Click(object sender, EventArgs e)
		{
			// display the error form, loading it with current authenticator data
			DiagnosticForm errorreport = new DiagnosticForm();
			errorreport.Config = Config;
			if (string.IsNullOrEmpty(errorreport.Config.Filename) == false)
			{
				errorreport.ConfigFileContents = File.ReadAllText(errorreport.Config.Filename);
			}
			else
			{
				using (MemoryStream ms = new MemoryStream())
				{
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					using (XmlWriter writer = XmlWriter.Create(ms, settings))
					{
						Config.WriteXmlString(writer);
					}
					ms.Position = 0;
					errorreport.ConfigFileContents = new StreamReader(ms).ReadToEnd();
				}
			}
			errorreport.ShowDialog(this);
		}
	}
}
