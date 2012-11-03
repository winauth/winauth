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
	/// Form to import an existing key from raw data
	/// </summary>
	public partial class ImportForm : Form
	{
		/// <summary>
		/// Create the new form object
		/// </summary>
		public ImportForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Get a new Authenticator based of the field entries
		/// </summary>
		public BattleNetAuthenticator Authenticator
		{
			get
			{
				// not filled in yet
				if (serial1Field.Text.Length == 0)
				{
					return null;
				}

				// create the data and authenticator
				BattleNetAuthenticator auth = new BattleNetAuthenticator();
				auth.Serial = serial1Field.Text + "-" + serial2Field.Text + "-" + serial3Field.Text + "-" + serial4Field.Text;
				//
				// this is an ascii encoded representation, e.g. a3 -> 0x61,0x33,
				string key = keyField.Text.Replace(" ", string.Empty).ToUpper();
				byte[] keyBytes = WindowsAuthenticator.Authenticator.StringToByteArray(key);
				//
				auth.SecretKey = keyBytes;
				//
				auth.ServerTimeDiff = (timeField.Text.Length != 0 ? int.Parse(timeField.Text) : 0);
				//
				return auth;
			}
		}

		/// <summary>
		/// Validate the form fields
		/// </summary>
		/// <returns></returns>
		private bool ValidateForm()
		{
			StringBuilder errors = new StringBuilder();
			do
			{
				// validate the serial
				if (serial1Field.Text.Length == 0 || serial2Field.Text.Length == 0 || serial3Field.Text.Length == 0 || serial4Field.Text.Length == 0)
				{
					errors.Append("You must fill in the serial number fields");
					serial1Field.Focus();
					break;
				}
				if (serial2Field.Text.Length != 4 || serial3Field.Text.Length != 4 || serial4Field.Text.Length != 4)
				{
					errors.Append("The serial number fields should each be 4 characters");
					serial1Field.Focus();
					break;
				}

				// validate the first part of serial
				string serial1 = serial1Field.Text.ToUpper();
				if (serial1 != "US" && serial1 != "EU")
				{
					DialogResult result = MessageBox.Show(this, "The first part of the serial is normally US or EU.\n\nAre you sure '" + serial1 + "' is right?", WinAuth.APPLICATION_NAME, MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2);
					if (result == System.Windows.Forms.DialogResult.No)
					{
						serial1Field.Focus();
						return false;
					}
				}

				// validate the key is 40 hex digits
				string key = keyField.Text.Replace(" ", string.Empty).ToUpper();
				if (key.Length != 40)
				{
					errors.Append("The key field should be 40 characters. You have " + key.Length + ".");
					keyField.Focus();
					break;
				}
				bool failed = false;
				for (int i=1; i<=key.Length; i++)
				{
					char c = key[i - 1];
					if ((c < '0' || c > '9') && (c < 'A' || c > 'F'))
					{
						failed = true;
						errors.Append("The key field must contain only the numbers between 0-9 and letters between A-F. You have a '" + c + "' at position " + i);
						break;
					}
				}
				if (failed == true)
				{
					keyField.Focus();
					break;
				}

				// validate the time
				long time = 0;
				if (timeField.Text.Length != 0 && long.TryParse(timeField.Text, out time) == false)
				{
					errors.Append("The Time Difference must be a number");
					timeField.Focus();
					break;
				}
			} while (false);
			if (errors.Length != 0)
			{
				MessageBox.Show(this, errors.ToString(), WinAuth.APPLICATION_NAME, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return false;
			}

			return true;
		}

		/// <summary>
		/// Clicked OK
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// all valid?
			if (ValidateForm() == false)
			{
				return;
			}

			DialogResult = System.Windows.Forms.DialogResult.OK;
		}

	}

	/// <summary>
	/// Text box that can restrict what is typed into it
	/// </summary>
	public class RestrictedTextBox : TextBox
	{
		/// <summary>
		/// Create a new RestrictedTextBox object
		/// </summary>
		public RestrictedTextBox() : base() { }

		/// <summary>
		/// Only allow numbers
		/// </summary>
		public bool NumbersOnly { get; set; }

		/// <summary>
		/// Only allow hex digits
		/// </summary>
		public bool HexOnly { get; set; }

		/// <summary>
		/// Only allow letters
		/// </summary>
		public bool LettersOnly { get; set; }

		/// <summary>
		/// Only allow number and letters
		/// </summary>
		public bool LettersAndNumbersOnly { get; set; }

		/// <summary>
		/// Force all to uppercase
		/// </summary>
		public bool ForceUppercase { get; set; }

		/// <summary>
		/// Check each key for restrictions
		/// </summary>
		/// <param name="e"></param>
		protected override void OnKeyPress(KeyPressEventArgs e)
		{
			bool passes = true;
			if (passes && NumbersOnly == true)
			{
				// only accept digits
				passes = (e.KeyChar < 32 || char.IsDigit(e.KeyChar) || e.KeyChar == '-' || e.KeyChar == '.' || e.KeyChar == '+');
			}
			if (passes && HexOnly == true)
			{
				// only accept hex digits
				passes = (e.KeyChar < 32 || char.IsDigit(e.KeyChar) || (e.KeyChar >= 'a' && e.KeyChar <= 'f') || (e.KeyChar >= 'A' && e.KeyChar <= 'F'));
			}
			if (passes && LettersOnly == true)
			{
				// only accept letters
				passes = (e.KeyChar < 32 || char.IsLetter(e.KeyChar));
			}
			if (passes && LettersAndNumbersOnly == true)
			{
				// only accept letters
				passes = (e.KeyChar < 32 || char.IsLetterOrDigit(e.KeyChar));
			}
			if (!passes)
			{
				e.Handled = true;
			}
			else
			{
				base.OnKeyPress(e);
			}
		}

		/// <summary>
		/// Make a field uppercase if necessary
		/// </summary>
		/// <param name="e"></param>
		protected override void OnLeave(EventArgs e)
		{
			if (ForceUppercase == true)
			{
				this.Text = this.Text.ToUpper();
			}
			base.OnLeave(e);
		}
	}
}
