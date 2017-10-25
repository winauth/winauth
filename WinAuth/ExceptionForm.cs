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
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using System.Reflection;
using System.Security;
using System.Security.Cryptography;
using System.Windows.Forms;
using System.Xml;
using System.Net;
using System.Web;

using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// General error report form
	/// </summary>
	public partial class ExceptionForm : WinAuth.ResourceForm
	{
		/// <summary>
		/// Exception that caused the error report
		/// </summary>
		public Exception ErrorException { get; set; }

		/// <summary>
		/// Current config
		/// </summary>
		public WinAuthConfig Config { get; set; }

		/// <summary>
		/// Create the  Form
		/// </summary>
		public ExceptionForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the error report form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExceptionForm_Load(object sender, EventArgs e)
		{
			errorIcon.Image = SystemIcons.Error.ToBitmap();
			this.Height = detailsButton.Top + detailsButton.Height + 45;

			this.errorLabel.Text = string.Format(this.errorLabel.Text, (ErrorException != null ? ErrorException.Message : strings.UnknownError));

			// build data
#if DEBUG
			dataText.Text = string.Format("{0}\n\n{1}", this.ErrorException.Message, new System.Diagnostics.StackTrace(this.ErrorException).ToString());
#else
			try
			{
				dataText.Text = WinAuthHelper.PGPEncrypt(BuildDiagnostics(), WinAuthHelper.WINAUTH_PGP_PUBLICKEY);
			}
			catch (Exception ex)
			{
				dataText.Text = string.Format("{0}\n\n{1}", ex.Message, new System.Diagnostics.StackTrace(ex).ToString());
			}
#endif
		}

		/// <summary>
		/// Build a diagnostics string for the current Config and any exception that had been thrown
		/// </summary>
		/// <returns>diagnostics information</returns>
		private string BuildDiagnostics()
		{
			StringBuilder diag = new StringBuilder();

			Version version;
#if NETFX_4
			if (Version.TryParse(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion, out version) == true)
			{
				diag.Append("Version:" + version.ToString(4));
			}
#endif
#if NETFX_3
			try
			{
				version = new Version(FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location).FileVersion);
				diag.Append("Version:" + version.ToString(4));
			}
			catch (Exception) { }
#endif

			// add winauth log
			try
			{
				string dir = Path.Combine(System.Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), WinAuthMain.APPLICATION_NAME);
				if (Directory.Exists(dir) == true)
				{
					string winauthlog = Path.Combine(dir, "winauth.log");
					if (File.Exists(winauthlog) == true)
					{
						diag.Append("--WINAUTH.LOG--").Append(Environment.NewLine);
						diag.Append(File.ReadAllText(winauthlog)).Append(Environment.NewLine).Append(Environment.NewLine);
					}

					// add authenticator.xml
					foreach (string file in Directory.GetFiles(dir, "*.xml"))
					{
						diag.Append("--" + file + "--").Append(Environment.NewLine);
						diag.Append(File.ReadAllText(file)).Append(Environment.NewLine).Append(Environment.NewLine);
					}
				}
			}
			catch (Exception) { }

			// add the current config
			if (this.Config != null)
			{
				using (var ms = new MemoryStream())
				{
					XmlWriterSettings settings = new XmlWriterSettings();
					settings.Indent = true;
					using (var xml = XmlWriter.Create(ms, settings))
					{
						this.Config.WriteXmlString(xml);
					}

					ms.Position = 0;

					diag.Append("-- Config --").Append(Environment.NewLine);
					diag.Append(new StreamReader(ms).ReadToEnd()).Append(Environment.NewLine).Append(Environment.NewLine);
				}
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

		/// <summary>
		/// Click the Quit button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void quitButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Click the Continue button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void continueButton_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		/// <summary>
		/// Click to show the details
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void detailsButton_Click(object sender, EventArgs e)
		{
			dataText.Visible = !dataText.Visible;
			if (dataText.Visible == true)
			{
				this.detailsButton.Text = strings.HideDetails;
				this.Height += 160;
			}
			else
			{
				this.detailsButton.Text = strings._ExceptionForm_detailsButton_;
				this.Height -= 160;
			}
		}

	}
}
