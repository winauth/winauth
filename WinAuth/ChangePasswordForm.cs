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
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
#if NETFX_4
using System.Threading.Tasks;
#endif
using System.Windows.Forms;
using MetroFramework.Controls;
using WinAuth.Resources;

namespace WinAuth
{
	/// <summary>
	/// Form for setting the password and encryption for the current authenticators
	/// </summary>
	public partial class ChangePasswordForm : WinAuth.ResourceForm
	{
		/// <summary>
		/// Used to show a filled password box
		/// </summary>
		private const string EXISTING_PASSWORD = "******";

		/// <summary>
		/// Number of iterations of PBKDF2 for our yubikey phrase
		/// </summary>
		private const int YUBIKEY_PBKDF2_ITERATIONS = 2048;

		/// <summary>
		/// Size of Yubikey secret key
		/// </summary>
		private const int YUBIKEY_PBKDF2_KEYSIZE = 20;

		/// <summary>
		/// Create the form
		/// </summary>
		public ChangePasswordForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current and new password type
		/// </summary>
		public Authenticator.PasswordTypes PasswordType { get; set; }

		/// <summary>
		/// Current and new password
		/// </summary>
		public string Password { get; set; }

		/// <summary>
		/// If have a current password
		/// </summary>
		public bool HasPassword { get; set; }

		/// <summary>
		/// List of seedwords
		/// </summary>
		private List<string> _seedWords = new List<string>();

		/// <summary>
		/// Chosen slot for yubikey
		/// </summary>
		private int YubikeySlot { get; set; }

		/// <summary>
		/// Current YubiKey info
		/// </summary>
		public YubiKey Yubikey {get; set;}

		/// <summary>
		/// Load the form and pretick checkboxes
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangePasswordForm_Load(object sender, EventArgs e)
		{
			if ((PasswordType & Authenticator.PasswordTypes.Machine) != 0 || (PasswordType & Authenticator.PasswordTypes.User) != 0)
			{
				machineCheckbox.Checked = true;
			}
			if ((PasswordType & Authenticator.PasswordTypes.User) != 0)
			{
				userCheckbox.Checked = true;
			}
			userCheckbox.Enabled = machineCheckbox.Checked;

			if ((PasswordType & Authenticator.PasswordTypes.Explicit) != 0)
			{
				passwordCheckbox.Checked = true;
				if (HasPassword == true)
				{
					passwordField.Text = EXISTING_PASSWORD;
					verifyField.Text = EXISTING_PASSWORD;
				}
			}

			yubiPanelConfigure.Visible = false;
			yubiConfigureIntroLabel.Visible = false;
			yubiPanelIntro.Visible = true;
			yubiPanelExists.Visible = false;
			if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot1) != 0 || (PasswordType & Authenticator.PasswordTypes.YubiKeySlot2) != 0)
			{
				yubiPanelIntro.Visible = false;
				yubiPanelExists.Visible = true;
				yubiPanelExists.Size = yubiPanelIntro.Size;
				yubiPanelExists.Location = yubiPanelIntro.Location;
				yubikeyBox.Checked = true;
				if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot1) != 0)
				{
					YubikeySlot = 1;
				}
				else if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot2) != 0)
				{
					YubikeySlot = 2;
				}
			}
		}

		/// <summary>
		/// Form has been shown
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ChangePasswordForm_Shown(object sender, EventArgs e)
		{
			// Buf in MetroFrame where focus is not set correcty during Load, so we do it here
			if (passwordField.Enabled == true)
			{
				passwordField.Focus();
			}
		}

		/// <summary>
		/// Machine encryption is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void machineCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			if (machineCheckbox.Checked == false)
			{
				userCheckbox.Checked = false;
			}
			userCheckbox.Enabled = machineCheckbox.Checked;
		}

		/// <summary>
		/// Toggle the Yubikey
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubikeyBox_CheckedChanged(object sender, EventArgs e)
		{
			if (yubikeyBox.Checked == true)
			{
				if ((PasswordType & Authenticator.PasswordTypes.YubiKeySlot1) != 0 || (PasswordType & Authenticator.PasswordTypes.YubiKeySlot2) != 0)
				{
					yubiPanelIntro.Enabled = false;
					yubiPanelIntro.Visible = false;
					yubiPanelConfigure.Visible = false;
					yubiConfigureIntroLabel.Visible = false;
					yubiPanelExists.Visible = true;
					yubiPanelExists.Location = yubiPanelIntro.Location;
					yubiPanelExists.Size = yubiPanelIntro.Size;
					return;
				}

				yubikeyStatusLabel.Text = "Initialising YubiKey...";
				yubikeyStatusLabel.Visible = true;

#if NETFX_4
				Task.Factory.StartNew(() =>
				{
#endif
					if (this.Yubikey == null)
					{
						this.Yubikey = YubiKey.CreateInstance();
					}
#if NETFX_4
				}).ContinueWith((task) =>
				{
#endif
				if (string.IsNullOrEmpty(this.Yubikey.Info.Error) == false)
					{
						yubikeyStatusLabel.Text = this.Yubikey.Info.Error;
						yubikeyBox.Checked = false;
						this.Yubikey = null;
					}
					else if (this.Yubikey.Info.Status.VersionMajor == 0)
					{
						yubikeyStatusLabel.Text = "Please insert your YubiKey";
						yubikeyBox.Checked = false;
						this.Yubikey = null;
					}
					else
					{
						yubikeyStatusLabel.Text = string.Format("YubiKey {0}.{1}.{2}{3}",
							this.Yubikey.Info.Status.VersionMajor,
							this.Yubikey.Info.Status.VersionMinor,
							this.Yubikey.Info.Status.VersionBuild,
							(this.Yubikey.Info.Serial != 0 ? " (Serial " + this.Yubikey.Info.Serial + ")" : string.Empty));
						yubiPanelIntro.Enabled = true;
					}
#if NETFX_4
				}, TaskScheduler.FromCurrentSynchronizationContext());
#endif
			}
			else
			{
				this.Yubikey = null;

				yubiPanelIntro.Enabled = false;
				yubiPanelIntro.Visible = true;
				yubiPanelConfigure.Visible = false;
				yubiConfigureIntroLabel.Visible = false;

				PasswordType &= ~(Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2);
			}
		}

		/// <summary>
		/// Password encrpytion is ticked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void passwordCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			passwordField.Enabled = (passwordCheckbox.Checked);
			verifyField.Enabled = (passwordCheckbox.Checked);
			if (passwordCheckbox.Checked == true)
			{
				passwordField.Focus();
			}
		}

		/// <summary>
		/// OK button is clicked
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void okButton_Click(object sender, EventArgs e)
		{
			// check password is set if requried
			if (passwordCheckbox.Checked == true && passwordField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, strings.EnterPassword);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			if (passwordCheckbox.Checked == true && string.Compare(passwordField.Text.Trim(), verifyField.Text.Trim()) != 0)
			{
				WinAuthForm.ErrorDialog(this, strings.PasswordsDontMatch);
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}
			if (yubikeyBox.Checked == true && YubikeySlot == 0 && (PasswordType & (Authenticator.PasswordTypes.YubiKeySlot1 | Authenticator.PasswordTypes.YubiKeySlot2)) == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please verify your YubiKey using the Use Slot or Configure Slot buttons.");
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			// set the valid password type property
			PasswordType = Authenticator.PasswordTypes.None;
			Password = null;
			if (userCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.User;
			}
			else if (machineCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.Machine;
			}
			if (passwordCheckbox.Checked == true)
			{
				PasswordType |= Authenticator.PasswordTypes.Explicit;
				if (this.passwordField.Text != EXISTING_PASSWORD)
				{
					Password = this.passwordField.Text.Trim();
				}
			}
			if (yubikeyBox.Checked == true)
			{
				if (YubikeySlot == 1)
				{
					PasswordType |= Authenticator.PasswordTypes.YubiKeySlot1;
				}
				else if (YubikeySlot == 2)
				{
					PasswordType |= Authenticator.PasswordTypes.YubiKeySlot2;
				}
			}
		}

		/// <summary>
		/// Get a new random seed
		/// </summary>
		/// <param name="wordCount">number of words in seed</param>
		/// <returns></returns>
		private string GetSeed(int wordCount)
		{
			if (_seedWords.Count == 0)
			{
				using (var s = new StreamReader(Assembly.GetExecutingAssembly().GetManifestResourceStream("WinAuth.Resources.SeedWords.txt")))
				{
					string line;
					while ((line = s.ReadLine()) != null)
					{
						if (line.Length != 0)
						{
							_seedWords.Add(line);
						}
					}
				}
			}

			List<string> words = new List<string>();
			RNGCryptoServiceProvider random = new RNGCryptoServiceProvider();
			byte[] buffer = new byte[4];
			for (var i = 0; i < wordCount; i++)
			{
				random.GetBytes(buffer);
				int pos = (int)(BitConverter.ToUInt32(buffer, 0) % (uint)_seedWords.Count());

				// check for duplicates
				var word = _seedWords[pos].ToLower();
				if (words.IndexOf(word) != -1)
				{
					i--;
					continue;
				}
				words.Add(word);
			}

			return string.Join(" ", words.ToArray());
		}

		/// <summary>
		/// Get a random seed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubiCreateSecretButton_Click(object sender, EventArgs e)
		{
			yubiSecretField.Text = GetSeed(12);
		}

		/// <summary>
		/// Set up new yubikey
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubikeyCreateButton_Click(object sender, EventArgs e)
		{
			yubiPanelConfigure.Visible = true;
			yubiConfigureIntroLabel.Visible = true;
		}

		/// <summary>
		/// Configure the Yubikey
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubiSecretUpdateButton_Click(object sender, EventArgs e)
		{
			if (yubiSecretField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter a secret phase or password");
				return;
			}

			if (WinAuthForm.ConfirmDialog(this,
				"This will overwrite any existing data on your YubiKey.\n\nAre you sure you want to continue?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
			{
				return;
			}

			int slot = (yubiSlotToggle.Checked == true ? 2 : 1);
			bool press = yubiPressToggle.Checked;

			// bug in YubiKey 3.2.x (and below?) where using keypress doesn't always work
			// see http://forum.yubico.com/viewtopic.php?f=26&t=1571
			if (press == true
				&& (this.Yubikey.Info.Status.VersionMajor < 3
					|| (this.Yubikey.Info.Status.VersionMajor == 3 && this.Yubikey.Info.Status.VersionMinor <= 3)))
			{
				if (WinAuthForm.ConfirmDialog(this,
					"This is a known issue using \"Require button press\" with YubiKeys that have firmware version 3.3 and below. It can cause intermittent problems when reading the Challenge-Response. You can contact Yubico and may be able to get a free replacement.\n\nDo you want to continue anyway?",
					MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button2) != System.Windows.Forms.DialogResult.Yes)
				{
					return;
				}
			}

			// calculate the actual key. This is a byte version of the string, salt="mnemonic"(+user password TBD), PBKDF 2048 times, return 20byte/160bit key
			byte[] bytes = Encoding.UTF8.GetBytes(yubiSecretField.Text.Trim());
			string salt = "mnemonic";
			byte[] saltbytes = Encoding.UTF8.GetBytes(salt);
			Rfc2898DeriveBytes kg = new Rfc2898DeriveBytes(bytes, saltbytes, YUBIKEY_PBKDF2_ITERATIONS);
			byte[] key = kg.GetBytes(YUBIKEY_PBKDF2_KEYSIZE);

			try
			{
				this.Yubikey.SetChallengeResponse(slot, key, key.Length, press);
			}
			catch (YubKeyException ex)
			{
				WinAuthForm.ErrorDialog(this, ex.Message, ex);
				return;
			}

			if (press == true)
			{
				if (WinAuthForm.ConfirmDialog(this,
					"Your YubiKey slot will now be verified. Please click its button when it flashes." + Environment.NewLine + Environment.NewLine + "Continue?",
					MessageBoxButtons.YesNo,
					MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
				{
					WinAuthForm.ErrorDialog(this, "Your YubiKey has been updated. Please verify it before continuing.");
					return;
				}
			}

			// perform the test encryption/decryption using the yubi
			try
			{
				string challenge = "WinAuth";
				string plain = Authenticator.ByteArrayToString(Encoding.ASCII.GetBytes(challenge));
				Authenticator.PasswordTypes passwordType = (slot == 1 ? Authenticator.PasswordTypes.YubiKeySlot1 : Authenticator.PasswordTypes.YubiKeySlot2);
				string encrypted = Authenticator.EncryptSequence(plain, passwordType, null, this.Yubikey);
				plain = Authenticator.DecryptSequence(encrypted, passwordType, null, this.Yubikey);
				string response = Encoding.ASCII.GetString(Authenticator.StringToByteArray(plain));
				if (challenge != response)
				{
					throw new ApplicationException("verification failed");
				}
			}
			catch (ApplicationException ex)
			{
				WinAuthForm.ErrorDialog(this, "The YubiKey test failed. Please try configuring it again or doing it manually. (" + ex.Message + ")");
				return;
			}

			YubikeySlot = slot;

			WinAuthForm.ErrorDialog(this, "Your YubiKey has been successfully updated.");
		}

		/// <summary>
		/// Check the configuration with the current yubikey
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void yubikeyCheckButton_Click(object sender, EventArgs e)
		{
			if (WinAuthForm.ConfirmDialog(this,
				"Your YubiKey slot will now be verified. If it requires you to press the button, please press when the light starts flashing." + Environment.NewLine + Environment.NewLine + "Continue?",
				MessageBoxButtons.YesNo,
				MessageBoxIcon.Warning) != System.Windows.Forms.DialogResult.Yes)
			{
				return;
			}

			int slot = (yubiSlotToggle.Checked == true ? 2 : 1);

			try
			{
				byte[] challenge = Encoding.ASCII.GetBytes("WinAuth");
				// get the hash from the key
				byte[] hash = this.Yubikey.ChallengeResponse(slot, challenge);
			}
			catch (ApplicationException ex)
			{
				WinAuthForm.ErrorDialog(this, "The YubiKey test failed. Please check the configuration is for Challenge-Response in HMAC-SHA1 mode with Variable Input. (" + ex.Message + ")");
				return;
			}

			YubikeySlot = slot;

			WinAuthForm.ErrorDialog(this, "Your YubiKey has been successfully tested.");
		}

	}

}
