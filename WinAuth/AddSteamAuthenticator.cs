/*
 * Copyright (C) 2015 Colin Mackie.
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
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Xml;
using System.Xml.XPath;

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinAuth
{
	/// <summary>
	/// Form class for create a new Battle.net authenticator
	/// </summary>
	public partial class AddSteamAuthenticator : ResourceForm
	{
		/// <summary>
		/// Entry for a single SDA account
		/// </summary>
		class ImportedSDAEntry
		{
			public const int PBKDF2_ITERATIONS = 50000;
			public const int SALT_LENGTH = 8;
			public const int KEY_SIZE_BYTES = 32;
			public const int IV_LENGTH = 16;

			public string Username;
			public string SteamId;
			public string json;

			public override string ToString()
			{
				return Username + " (" + this.SteamId + ")";
			}
		}

		/// <summary>
		/// Form instantiation
		/// </summary>
		public AddSteamAuthenticator()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Current authenticator
		/// </summary>
		public WinAuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// Enrolling state
		/// </summary>
		private SteamAuthenticator.EnrollState m_enroll = new SteamAuthenticator.EnrollState();

		/// <summary>
		/// Current enrolling authenticator
		/// </summary>
		private SteamAuthenticator m_steamAuthenticator = new SteamAuthenticator();

		/// <summary>
		/// Set of tab pages taken from the tab control so we can hide and show them
		/// </summary>
		private Dictionary<string, TabPage> m_tabPages = new Dictionary<string, TabPage>();

		#region Form Events

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_Load(object sender, EventArgs e)
		{
			nameField.Text = this.Authenticator.Name;

			for (var i = 0; i < tabs.TabPages.Count; i++)
			{
				m_tabPages.Add(tabs.TabPages[i].Name, tabs.TabPages[i]);
			}
			tabs.TabPages.RemoveByKey("authTab");
			tabs.TabPages.RemoveByKey("confirmTab");
			tabs.TabPages.RemoveByKey("addedTab");
			tabs.SelectedTab = tabs.TabPages[0];

			revocationcodeField.SecretMode = true;
			revocationcode2Field.SecretMode = true;

			importSDAList.Font = this.Font;

			nameField.Focus();
		}

		/// <summary>
		/// If we close after adding, make sure we save it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_FormClosing(object sender, FormClosingEventArgs e)
		{
			if (m_enroll.Success == true)
			{
				this.Authenticator.Name = nameField.Text;
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}

		/// <summary>
		/// Press the form's cancel button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			// if we press ESC after adding, make sure we save it
			if (m_enroll.Success == true)
			{
				this.DialogResult = System.Windows.Forms.DialogResult.OK;
			}
		}

		/// <summary>
		/// Click the OK button to verify and add the authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void confirmButton_Click(object sender, EventArgs e)
		{
			if (activationcodeField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the activation code from your email");
				this.DialogResult = System.Windows.Forms.DialogResult.None;
				return;
			}

			m_enroll.ActivationCode = activationcodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRift_Click(object sender, EventArgs e)
		{
			steamIconRadioButton.Checked = true;
		}

		/// <summary>
		/// Select one of the icons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconGlyph_Click(object sender, EventArgs e)
		{
			steamAuthenticatorIconRadioButton.Checked = true;
		}

		/// <summary>
		/// Set the authenticator icon
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void iconRadioButton_CheckedChanged(object sender, EventArgs e)
		{
			if (((RadioButton)sender).Checked == true)
			{
				this.Authenticator.Skin = (string)((RadioButton)sender).Tag;
			}
		}

		/// <summary>
		/// Draw the tabs of the tabcontrol so they aren't white
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabControl1_DrawItem(object sender, DrawItemEventArgs e)
		{
			TabPage page = tabs.TabPages[e.Index];
			e.Graphics.FillRectangle(new SolidBrush(page.BackColor), e.Bounds);

			Rectangle paddedBounds = e.Bounds;
			int yOffset = (e.State == DrawItemState.Selected) ? -2 : 1;
			paddedBounds.Offset(1, yOffset);
			TextRenderer.DrawText(e.Graphics, page.Text, this.Font, paddedBounds, page.ForeColor);

			captchaGroup.BackColor = page.BackColor;
		}

		/// <summary>
		/// Answer the captcha
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void captchaButton_Click(object sender, EventArgs e)
		{
			if (captchacodeField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the characters in the image", null, MessageBoxButtons.OK);
				return;
			}

			m_enroll.Username = usernameField.Text.Trim();
			m_enroll.Password = passwordField.Text.Trim();
			m_enroll.CaptchaText = captchacodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Login to steam account
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void loginButton_Click(object sender, EventArgs e)
		{
			if (usernameField.Text.Trim().Length == 0 || passwordField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter your username and password", null, MessageBoxButtons.OK);
				return;
			}

			m_enroll.Username = usernameField.Text.Trim();
			m_enroll.Password = passwordField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// Confirm with the code sent by email
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void authcodeButton_Click(object sender, EventArgs e)
		{
			if (authcodeField.Text.Trim().Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the authorisation code", null, MessageBoxButtons.OK);
				return;
			}

			m_enroll.EmailAuthText = authcodeField.Text.Trim();

			ProcessEnroll();
		}

		/// <summary>
		/// CLick the close button to save
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeButton_Click(object sender, EventArgs e)
		{
			this.Authenticator.Name = nameField.Text;

			if (tabs.SelectedTab.Name == "importAndroidTab")
			{
				if (ImportSteamGuard() == false)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}
			}
			if (tabs.SelectedTab.Name == "importSDATab")
			{
				if (ImportSDA() == false)
				{
					this.DialogResult = System.Windows.Forms.DialogResult.None;
					return;
				}
			}

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Handle the enter key on the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void AddSteamAuthenticator_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				switch (tabs.SelectedTab.Name)
				{
					case "loginTab":
						e.Handled = true;
						if (m_enroll.RequiresCaptcha == true)
						{
							captchaButton_Click(sender, new EventArgs());
						}
						else
						{
							loginButton_Click(sender, new EventArgs());
						}
						break;
					case "authTab":
						e.Handled = true;
						authcodeButton_Click(sender, new EventArgs());
						break;
					case "confirmTab":
						e.Handled = true;
						confirmButton_Click(sender, new EventArgs());
						break;
					case "importAndroidTab":
						e.Handled = true;
						closeButton_Click(sender, new EventArgs());
						break;
					case "importSDATab":
						e.Handled = true;
						closeButton_Click(sender, new EventArgs());
						break;
					default:
						e.Handled = false;
						break;
				}

				return;
			}

			e.Handled = false;
		}

		/// <summary>
		/// Enable the button when we have confirmed
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			confirmButton.Enabled = revocationCheckbox.Checked;
		}

		/// <summary>
		/// Allow the field to be copied
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationcodeCopy_CheckedChanged(object sender, EventArgs e)
		{
			revocationcodeField.SecretMode = !revocationcodeCopy.Checked;
		}

		/// <summary>
		/// Allow the field to be copied
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void revocationcode2Copy_CheckedChanged(object sender, EventArgs e)
		{
			revocationcode2Field.SecretMode = !revocationcode2Copy.Checked;
		}

		/// <summary>
		/// When changing tabs, set the correct buttons
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void tabs_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (tabs.SelectedTab != null && (tabs.SelectedTab.Name == "importAndroidTab" || tabs.SelectedTab.Name == "importSDATab"))
			{
				closeButton.Text = "OK";
				closeButton.Visible = true;
			}
			else
			{
				closeButton.Text = "Close";
				closeButton.Visible = false;
			}
		}

		/// <summary>
		/// Browse the SDA folder
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importSDABrowse_Click(object sender, EventArgs e)
		{
			OpenFileDialog ofd = new OpenFileDialog();
			ofd.AddExtension = true;
			ofd.CheckFileExists = true;
			ofd.CheckPathExists = true;
			ofd.DefaultExt = "*.json";
			ofd.FileName = "manifest.json";
			ofd.Filter = "Manifest file|manifest.json|maFile (*.maFile)|*.maFile";
			ofd.FilterIndex = 0;
			ofd.RestoreDirectory = true;
			ofd.Title = "SteamDesktopAuthenticator";
			if (ofd.ShowDialog() == DialogResult.OK)
			{
				this.importSDAPath.Text = ofd.FileName;
			}
		}

		/// <summary>
		/// Click the load the SDA accounts
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void importSDALoad_Click(object sender, EventArgs e)
		{
			LoadSDA();
		}

		#endregion

		#region Private methods

		/// <summary>
		/// Import an authenticator from the uuid and steamguard files
		/// </summary>
		/// <returns>true if successful</returns>
		private bool ImportSteamGuard()
		{
			string uuid = importUuid.Text.Trim();
			if (uuid.Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the contents of the steam.uuid.xml file or your DeviceId");
				return false;
			}
			string steamguard = this.importSteamguard.Text.Trim();
			if (steamguard.Length == 0)
			{
				WinAuthForm.ErrorDialog(this, "Please enter the contents of your SteamGuard file");
				return false;
			}

			// check the deviceid
			string deviceId;
			if (uuid.IndexOf("?xml") != -1)
			{
				try
				{
					XmlDocument doc = new XmlDocument();
					doc.LoadXml(uuid);
					var node = doc.SelectSingleNode("//string[@name='uuidKey']");
					if (node == null)
					{
						WinAuthForm.ErrorDialog(this, "Cannot find uuidKey in xml");
						return false;
					}

					deviceId = node.InnerText;
				}
				catch (Exception ex)
				{
					WinAuthForm.ErrorDialog(this, "Invalid uuid xml: " + ex.Message);
					return false;
				}
			}
			else
			{
				deviceId = uuid;
			}
			if (string.IsNullOrEmpty(deviceId) || Regex.IsMatch(deviceId, @"android:[0-9abcdef-]+", RegexOptions.Singleline | RegexOptions.IgnoreCase) == false)
			{
				WinAuthForm.ErrorDialog(this, "Invalid deviceid, expecting \"android:NNNN...\"");
				return false;
			}

			// check the steamguard
			byte[] secret;
			string serial;
			try
			{
				var json = JObject.Parse(steamguard);

				var node = json.SelectToken("shared_secret");
				if (node == null)
				{
					throw new ApplicationException("no shared_secret");
				}			
				secret = Convert.FromBase64String(node.Value<string>());

				node = json.SelectToken("serial_number");
				if (node == null)
				{
					throw new ApplicationException("no serial_number");
				}
				serial = node.Value<string>();
			}
			catch (Exception ex)
			{
				WinAuthForm.ErrorDialog(this, "Invalid SteamGuard JSON contents: " + ex.Message);
				return false;
			}

			SteamAuthenticator auth = new SteamAuthenticator();
			auth.SecretKey = secret;
			auth.Serial = serial;
			auth.SteamData = steamguard;
			auth.DeviceId = deviceId;

			this.Authenticator.AuthenticatorData = auth;

			return true;
		}

		/// <summary>
		/// Import the selected SDA account
		/// </summary>
		/// <returns>true if successful</returns>
		private bool ImportSDA()
		{
			var entry = this.importSDAList.SelectedItem as ImportedSDAEntry;
			if (entry == null)
			{
				WinAuthForm.ErrorDialog(this, "Please load and select a Steam account");
				return false;
			}

			SteamAuthenticator auth = new SteamAuthenticator();
			var token = JObject.Parse(entry.json);
			foreach (var prop in token.Root.Children().ToList())
			{
				var child = token.SelectToken(prop.Path);

				string lkey = prop.Path.ToLower();
				if (lkey == "fully_enrolled" || lkey == "session")
				{
					prop.Remove();
				}
				else if (lkey == "device_id")
				{
					auth.DeviceId = child.Value<string>();
					prop.Remove();
				}
				else if (lkey == "serial_number")
				{
					auth.Serial = child.Value<string>();
				}
				else if (lkey == "account_name")
				{
					if (this.nameField.Text.Length == 0)
					{
						this.nameField.Text = "Steam (" + child.Value<string>() + ")";
					}
				}
				else if (lkey == "shared_secret")
				{
					auth.SecretKey = Convert.FromBase64String(child.Value<string>());
				}
			}
			auth.SteamData = token.ToString(Newtonsoft.Json.Formatting.None);

			this.Authenticator.AuthenticatorData = auth;

			return true;
		}

		/// <summary>
		/// Load all the accounts from the SDA manifest into the listbox
		/// </summary>
		private void LoadSDA()
		{
			string manifestfile = this.importSDAPath.Text.Trim();
			if (string.IsNullOrEmpty(manifestfile) == true || File.Exists(manifestfile) == false)
			{
				WinAuthForm.ErrorDialog(this, "Enter a path for SteamDesktopAuthenticator");
				return;
			}

			string password = this.importSDAPassword.Text.Trim();

			importSDAList.Items.Clear();
			try
			{
				string path = Path.GetDirectoryName(manifestfile);

				if (manifestfile.IndexOf("manifest.json") != -1)
				{
					var manifest = JObject.Parse(File.ReadAllText(manifestfile));
					var token = manifest.SelectToken("encrypted");
					bool encrypted = token != null ? token.Value<bool>() : false;
					if (encrypted == true && password.Length == 0)
					{
						throw new ApplicationException("Please enter your password");
					}

					JArray entries = manifest["entries"] as JArray;
					if (entries == null || entries.Count == 0)
					{
						throw new ApplicationException("SteamDesktopAuthenticator has no SteamGuard authenticators");
					}

					foreach (var entry in entries)
					{
						token = entry.SelectToken("filename");
						if (token != null)
						{
							string filename = token.Value<string>();
							string steamid = null;
							string iv = null;
							string salt = null;

							token = entry.SelectToken("steamid");
							if (token != null)
							{
								steamid = token.Value<string>();
							}
							token = entry.SelectToken("encryption_iv");
							if (token != null)
							{
								iv = token.Value<string>();
							}
							token = entry.SelectToken("encryption_salt");
							if (token != null)
							{
								salt = token.Value<string>();
							}

							LoadSDAFile(Path.Combine(path, filename), password, steamid, iv, salt);
						}
					}
				}
				else if (string.IsNullOrEmpty(password) == false)
				{
					throw new ApplicationException("Cannot load an single maFile that has been encrypted");
				}
				else
				{
					LoadSDAFile(manifestfile);
				}
			}
			catch (ApplicationException ex)
			{
				WinAuthForm.ErrorDialog(this, ex.Message);
			}
			catch (Exception ex)
			{
				WinAuthForm.ErrorDialog(this, "Error while importing: " + ex.Message, ex);
			}
		}

		/// <summary>
		/// Load a single maFile with the security credentials
		/// </summary>
		/// <param name="mafile">filename</param>
		/// <param name="password">optional password</param>
		/// <param name="steamid">steamid if known</param>
		/// <param name="iv">optional iv for decryption</param>
		/// <param name="salt">optional salt</param>
		private void LoadSDAFile(string mafile, string password = null, string steamid = null, string iv = null, string salt = null)
		{
			string data;
			if (File.Exists(mafile) == false || (data = File.ReadAllText(mafile)) == null)
			{
				throw new ApplicationException("Cannot read file " + mafile);
			}

			// decrypt
			if (string.IsNullOrEmpty(password) == false)
			{
				byte[] ciphertext = Convert.FromBase64String(data);

#if NETFX_4
				using (Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), ImportedSDAEntry.PBKDF2_ITERATIONS))
#endif
#if NETFX_3
				Rfc2898DeriveBytes pbkdf2 = new Rfc2898DeriveBytes(password, Convert.FromBase64String(salt), ImportedSDAEntry.PBKDF2_ITERATIONS);
#endif
				{
					byte[] key = pbkdf2.GetBytes(ImportedSDAEntry.KEY_SIZE_BYTES);

					using (RijndaelManaged aes256 = new RijndaelManaged())
					{
						aes256.IV = Convert.FromBase64String(iv);
						aes256.Key = key;
						aes256.Padding = PaddingMode.PKCS7;
						aes256.Mode = CipherMode.CBC;

						try
						{
							using (ICryptoTransform decryptor = aes256.CreateDecryptor(aes256.Key, aes256.IV))
							{
								using (MemoryStream ms = new MemoryStream(ciphertext))
								{
									using (CryptoStream cs = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
									{
										using (StreamReader sr = new StreamReader(cs))
										{
											data = sr.ReadToEnd();
										}
									}
								}
							}
						}
						catch (CryptographicException )
						{
							throw new ApplicationException("Invalid password");
						}
					}
				}
			}

			var token = JObject.Parse(data);
			var sdaentry = new ImportedSDAEntry();
			sdaentry.Username = token.SelectToken("account_name") != null ? token.SelectToken("account_name").Value<string>() : null;
			sdaentry.SteamId = steamid;
			if (string.IsNullOrEmpty(sdaentry.SteamId) == true)
			{
				sdaentry.SteamId = token.SelectToken("Session.SteamID") != null ? token.SelectToken("Session.SteamID").Value<string>() : null;
			}
			if (string.IsNullOrEmpty(sdaentry.SteamId) == true)
			{
				sdaentry.SteamId = mafile.Split('.')[0];
			}
			sdaentry.json = data;

			importSDAList.Items.Add(sdaentry);
		}

		/// <summary>
		/// Process the enrolling calling the authenticator method, checking the state and displaying appropriate tab
		/// </summary>
		private void ProcessEnroll()
		{
			do
			{
				try
				{
					var cursor = Cursor.Current;
					Cursor.Current = Cursors.WaitCursor;
					Application.DoEvents();

					var result = m_steamAuthenticator.Enroll(m_enroll);
					Cursor.Current = cursor;
					if (result == false)
					{
						if (string.IsNullOrEmpty(m_enroll.Error) == false)
						{
							WinAuthForm.ErrorDialog(this, m_enroll.Error, null, MessageBoxButtons.OK);
						}

						if (m_enroll.Requires2FA == true)
						{
							WinAuthForm.ErrorDialog(this, "It looks like you already have an authenticator added to you account", null, MessageBoxButtons.OK);
							return;
						}

						if (m_enroll.RequiresCaptcha == true)
						{
							using (var web = new WebClient())
							{
								byte[] data = web.DownloadData(m_enroll.CaptchaUrl);

								using (var ms = new MemoryStream(data))
								{
									captchaBox.Image = Image.FromStream(ms);
								}
							}
							loginButton.Enabled = false;
							captchaGroup.Visible = true;
							captchacodeField.Text = "";
							captchacodeField.Focus();
							return;
						}
						loginButton.Enabled = true;
						captchaGroup.Visible = false;

						if (m_enroll.RequiresEmailAuth == true)
						{
							if (authoriseTabLabel.Tag == null || string.IsNullOrEmpty((string)authoriseTabLabel.Tag) == true)
							{
								authoriseTabLabel.Tag = authoriseTabLabel.Text;
							}
							string email = string.IsNullOrEmpty(m_enroll.EmailDomain) == false ? "***@" + m_enroll.EmailDomain : string.Empty;
							authoriseTabLabel.Text = string.Format((string)authoriseTabLabel.Tag, email);
							authcodeField.Text = "";
							ShowTab("authTab");
							authcodeField.Focus();
							return;
						}
						if (tabs.TabPages.ContainsKey("authTab") == true)
						{
							tabs.TabPages.RemoveByKey("authTab");
						}

						if (m_enroll.RequiresLogin == true)
						{
							ShowTab("loginTab");
							usernameField.Focus();
							return;
						}

						if (m_enroll.RequiresActivation == true)
						{
							m_enroll.Error = null;

							this.Authenticator.AuthenticatorData = m_steamAuthenticator;
							revocationcodeField.Text = m_enroll.RevocationCode;

							ShowTab("confirmTab");

							activationcodeField.Focus();
							return;
						}

						string error = m_enroll.Error;
						if (string.IsNullOrEmpty(error) == true)
						{
							error = "Unable to add the add the authenticator to your account. Please try again later.";
						}
						WinAuthForm.ErrorDialog(this, error, null, MessageBoxButtons.OK);

						return;
					}

					ShowTab("addedTab");

					revocationcode2Field.Text = m_enroll.RevocationCode;
					tabs.SelectedTab = tabs.TabPages["addedTab"];

					this.closeButton.Location = this.cancelButton.Location;
					this.closeButton.Visible = true;
					this.cancelButton.Visible = false;

					break;
				}
				catch (InvalidEnrollResponseException iere)
				{
					if (WinAuthForm.ErrorDialog(this, "An error occurred while registering the authenticator", iere, MessageBoxButtons.RetryCancel) != System.Windows.Forms.DialogResult.Retry)
					{
						break;
					}
				}
			} while (true);
		}

		/// <summary>
		/// Show the named tab hiding all others
		/// </summary>
		/// <param name="name">name of tab to show</param>
		/// <param name="only">hide all others, or append if false</param>
		private void ShowTab(string name, bool only = true)
		{
			if (only == true)
			{
				tabs.TabPages.Clear();
			}

			if (tabs.TabPages.ContainsKey(name) == false)
			{
				tabs.TabPages.Add(m_tabPages[name]);
			}

			tabs.SelectedTab = tabs.TabPages[name];
		}


#endregion

	}
}
