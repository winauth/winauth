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
using System.Xml;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Form to export/show an existing key from raw data
	/// </summary>
	public partial class ExportForm : Form
	{
		/// <summary>
		/// The template for the offical BMA config file
		/// </summary>
		private static string ANDROID_BMA = "<?xml version='1.0' encoding='utf-8' standalone='yes' ?>" + System.Environment.NewLine +
			"<map>" + System.Environment.NewLine +
			"<long name=\"com.blizzard.bma.AUTH_STORE.CLOCK_OFFSET\" value=\"{0}\" />" + System.Environment.NewLine +
			"<string name=\"com.blizzard.bma.AUTH_STORE.HASH\">{1}</string>" + System.Environment.NewLine +
			"</map>" + System.Environment.NewLine;

		/// <summary>
		/// Private encrpytion key used to encrypt BMA data.
		/// </summary>
		private static byte[] MOBILE_AUTHENTICATOR_KEY = new byte[]
			{
				0x39,0x8e,0x27,0xfc,0x50,0x27,0x6a,0x65,0x60,0x65,0xb0,0xe5,0x25,0xf4,0xc0,0x6c,
				0x04,0xc6,0x10,0x75,0x28,0x6b,0x8e,0x7a,0xed,0xa5,0x9d,0xa9,0x81,0x3b,0x5d,0xd6,
				0xc8,0x0d,0x2f,0xb3,0x80,0x68,0x77,0x3f,0xa5,0x9b,0xa4,0x7c,0x17,0xca,0x6c,0x64,
				0x79,0x01,0x5c,0x1d,0x5b,0x8b,0x8f,0x6b,0x9a
			};

		/// <summary>
		/// Create the new form object
		/// </summary>
		public ExportForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get a new Authenticator based of the field entries
		/// </summary>
		public AuthenticatorData AuthenticatorData {get; set;}

		/// <summary>
		/// Build the xml for the offical Android BMA file
		/// </summary>
		/// <returns></returns>
		private string BuildAndroidBMAXml()
		{
			// convert key and serial into combined string (BMA uses lowercase)
			string code = Authenticator.ByteArrayToString(AuthenticatorData.SecretKey).ToLower() + AuthenticatorData.Serial;
			// encrypt with BMA key
			byte[] plain = Encoding.UTF8.GetBytes(code);
			for (int i = plain.Length - 1; i >= 0; i--)
			{
				plain[i] ^= MOBILE_AUTHENTICATOR_KEY[i];
			}
			// convert to string and format from the template
			code = Authenticator.ByteArrayToString(plain).ToLower();
			return string.Format(ANDROID_BMA, AuthenticatorData.ServerTimeDiff, code);
		}

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ExportForm_Load(object sender, EventArgs e)
		{
			if (AuthenticatorData == null)
			{
				throw new ApplicationException("Export should have an AuthenticatorData set");
			}

			// populate the key fields
			this.serialField.Text = AuthenticatorData.Serial;
			this.keyField.Text = Authenticator.ByteArrayToString(AuthenticatorData.SecretKey);
			this.timeField.Text = AuthenticatorData.ServerTimeDiff.ToString();
			this.xmlField.Text = BuildAndroidBMAXml();
		}

		/// <summary>
		/// Clicked Close
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			DialogResult = System.Windows.Forms.DialogResult.OK;
		}
	}
}
