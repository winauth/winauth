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
using System.Text;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;

namespace WinAuth
{
	/// <summary>
	/// General error report form
	/// </summary>
	public partial class DiagnosticForm : WinAuth.ResourceForm
	{
		/// <summary>
		/// Current Winauth config settings
		/// </summary>
		public WinAuthConfig Config { get; set; }

		/// <summary>
		/// Winauth config file
		/// </summary>
		public string ConfigFileContents { get; set; }

		/// <summary>
		/// Exception that caused the error report
		/// </summary>
		public Exception ErrorException { get; set; }

		/// <summary>
		/// Create the  Form
		/// </summary>
		public DiagnosticForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the error report form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ErrorReportForm_Load(object sender, EventArgs e)
		{
			// build data
			try
			{
				dataText.Text = WinAuthHelper.PGPEncrypt(BuildDiagnostics(), WinAuthHelper.WINAUTH_PGP_PUBLICKEY);
			}
			catch (Exception ex)
			{
				dataText.Text = string.Format("{0}\n\n{1}", ex.Message, new System.Diagnostics.StackTrace(ex).ToString());
			}
		}

		/// <summary>
		/// Build a diagnostics string for the current Config and any exception that had been thrown
		/// </summary>
		/// <returns>diagnostics information</returns>
		private string BuildDiagnostics()
		{
			StringBuilder diag = new StringBuilder();

			if (this.Config != null)
			{
				// clone the current config so we can extract key in case machine/user encrypted
				WinAuthConfig clone = this.Config.Clone() as WinAuthConfig;
				clone.PasswordType = Authenticator.PasswordTypes.None;

				// add the config and authenticator
				try
				{
					StringBuilder xml = new StringBuilder();
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					using (XmlWriter writer = XmlWriter.Create(xml, settings))
					{
						clone.WriteXmlString(writer);
					}
					diag.Append("--CURRENT CONFIG--").Append(Environment.NewLine);
					diag.Append(xml.ToString()).Append(Environment.NewLine).Append(Environment.NewLine);
				}
				catch (Exception ex)
				{
					diag.Append(ex.Message).Append(Environment.NewLine).Append(Environment.NewLine);
				}
			}

			// add each of the entries from the registry
			if (this.Config != null)
			{
				diag.Append("--REGISTRY--").Append(Environment.NewLine);
				diag.Append(WinAuthHelper.ReadBackupFromRegistry(this.Config)).Append(Environment.NewLine).Append(Environment.NewLine);
			}

			// add current config file
			if (string.IsNullOrEmpty(ConfigFileContents) == false)
			{
				diag.Append("--CONFIGFILE--").Append(Environment.NewLine);
				diag.Append(ConfigFileContents).Append(Environment.NewLine).Append(Environment.NewLine);
			}

			// add winauth log
			string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuthMain.APPLICATION_NAME);
			string winauthlog = Path.Combine(dir, "winauth.log");
			if (File.Exists(winauthlog) == true)
			{
				diag.Append("--WINAUTH.LOG--").Append(Environment.NewLine);
				diag.Append(File.ReadAllText(winauthlog)).Append(Environment.NewLine).Append(Environment.NewLine);
			}

			// add the exception
			if (ErrorException != null)
			{
				diag.Append("--EXCEPTION--").Append(Environment.NewLine);

				Exception ex = ErrorException;
				while (ex != null)
				{
					diag.Append("Stack: ").Append(ex.Message).Append(Environment.NewLine).Append(new System.Diagnostics.StackTrace(ex).ToString()).Append(Environment.NewLine);
					ex = ex.InnerException;
				}
				if (ErrorException is InvalidEncryptionException)
				{
					diag.Append("Plain: " + ((InvalidEncryptionException)ErrorException).Plain).Append(Environment.NewLine);
					diag.Append("Password: " + ((InvalidEncryptionException)ErrorException).Password).Append(Environment.NewLine);
					diag.Append("Encrypted: " + ((InvalidEncryptionException)ErrorException).Encrypted).Append(Environment.NewLine);
					diag.Append("Decrypted: " + ((InvalidEncryptionException)ErrorException).Decrypted).Append(Environment.NewLine);
				}
				else if (ErrorException is InvalidSecretDataException)
				{
					diag.Append("EncType: " + ((InvalidSecretDataException)ErrorException).EncType).Append(Environment.NewLine);
					diag.Append("Password: " + ((InvalidSecretDataException)ErrorException).Password).Append(Environment.NewLine);
					foreach (string data in ((InvalidSecretDataException)ErrorException).Decrypted)
					{
						diag.Append("Data: " + data).Append(Environment.NewLine);
					}
				}
			}

			return diag.ToString();
		}

	}
}
