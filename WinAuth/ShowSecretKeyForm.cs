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
using System.Text.RegularExpressions;
using System.Windows.Forms;

using ZXing;

namespace WinAuth
{
	/// <summary>
	/// Form display initialization confirmation.
	/// </summary>
	public partial class ShowSecretKeyForm : ResourceForm
	{
		/// <summary>
		/// Current authenticator
		/// </summary>
		public WinAuthAuthenticator CurrentAuthenticator { get; set; }

		/// <summary>
		/// Create a new form
		/// </summary>
		public ShowSecretKeyForm()
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
		private void ShowSecretKeyForm_Load(object sender, EventArgs e)
		{
			this.secretKeyField.SecretMode = true;

			string key = Base32.getInstance().Encode(CurrentAuthenticator.AuthenticatorData.SecretKey);
			this.secretKeyField.Text = Regex.Replace(key, ".{3}", "$0 ").Trim();

			string type = CurrentAuthenticator.AuthenticatorData is HOTPAuthenticator ? "hotp" : "totp";
			long counter = (CurrentAuthenticator.AuthenticatorData is HOTPAuthenticator ? ((HOTPAuthenticator)CurrentAuthenticator.AuthenticatorData).Counter : 0);
			string issuer = CurrentAuthenticator.AuthenticatorData.Issuer;

			//string url = "otpauth://" + type + "/" + WinAuthHelper.HtmlEncode(CurrentAuthenticator.Name)
			//	+ "?secret=" + key
			//	+ "&digits=" + CurrentAuthenticator.AuthenticatorData.CodeDigits
			//	+ (counter != 0 ? "&counter=" + counter : string.Empty)
			//	+ (string.IsNullOrEmpty(issuer) == false ? "&issuer=" + WinAuthHelper.HtmlEncode(issuer) : string.Empty);
			string url = CurrentAuthenticator.ToUrl(true);

			BarcodeWriter writer = new BarcodeWriter();
			writer.Format = BarcodeFormat.QR_CODE;
			writer.Options = new ZXing.Common.EncodingOptions { Width = qrImage.Width, Height = qrImage.Height };
			qrImage.Image = writer.Write(url);
		}

		/// <summary>
		/// Toggle the secret mode to allow copy
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void allowCopyCheckBox_CheckedChanged(object sender, EventArgs e)
		{
			this.secretKeyField.SecretMode = !allowCopyCheckBox.Checked;

			string key = Base32.getInstance().Encode(CurrentAuthenticator.AuthenticatorData.SecretKey);
			if (this.secretKeyField.SecretMode == true)
			{
				this.secretKeyField.Text = Regex.Replace(key, ".{3}", "$0 ").Trim();
			}
			else
			{
				this.secretKeyField.Text = key;
			}
		}

	}
}
