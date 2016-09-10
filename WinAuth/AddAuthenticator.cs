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
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Windows.Forms;

using WinAuth.Resources;

using ZXing;

namespace WinAuth
{
    /// <summary>
    /// Form class for create a new Battle.net authenticator
    /// </summary>
    public partial class AddAuthenticator : ResourceForm
    {
        /// <summary>
        /// HOTP string
        /// </summary>
        private const string HOTP = "hotp";

        /// <summary>
        /// TOTP string
        /// </summary>
        private const string TOTP = "totp";

        /// <summary>
        /// Form instantiation
        /// </summary>
        public AddAuthenticator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Current authenticator
        /// </summary>
        public WinAuthAuthenticator Authenticator { get; set; }

        /// <summary>
        /// If we have already warned about sync error
        /// </summary>
        private bool SyncErrorWarned;

        #region Form Events

        /// <summary>
        /// Load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddAuthenticator_Load(object sender, EventArgs e)
        {
            nameField.Text = this.Authenticator.Name;
            codeField.SecretMode = true;
            cmbHMACType.SelectedIndex = 0;
        }

        /// <summary>
        /// Timer tick to show code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timer_Tick(object sender, EventArgs e)
        {
            if (this.Authenticator.AuthenticatorData != null && !(this.Authenticator.AuthenticatorData is HOTPAuthenticator) && codeProgress.Visible == true)
            {
                int time = (int)(this.Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
                codeProgress.Value = time + 1;
                if (time == 0)
                {
                    codeField.Text = this.Authenticator.AuthenticatorData.CurrentCode;
                }
            }
        }

        /// <summary>
        /// Handle cancel button with warning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.Authenticator.AuthenticatorData != null)
            {
                DialogResult result = WinAuthForm.ConfirmDialog(this.Owner,
                    "WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
                    + "If you have added this authenticator to your online account, you will not be able to login in the future, and you need to click YES to save it." + Environment.NewLine + Environment.NewLine
                    + "Do you want to save this authenticator?", MessageBoxButtons.YesNoCancel);
                if (result == System.Windows.Forms.DialogResult.Yes)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.OK;
                    return;
                }
                else if (result == System.Windows.Forms.DialogResult.Cancel)
                {
                    this.DialogResult = System.Windows.Forms.DialogResult.None;
                    return;
                }
            }
        }

        /// <summary>
        /// Click the OK button to verify and add the authenticator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void okButton_Click(object sender, EventArgs e)
        {
            string privatekey = this.secretCodeField.Text.Trim();
            if (privatekey.Length == 0)
            {
                WinAuthForm.ErrorDialog(this.Owner, "Please enter the Secret Code");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            bool first = (this.Authenticator.AuthenticatorData == null);
            if (verifyAuthenticator(privatekey) == false)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
            if (first == true)
            {
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }

            // if this is a htop we reduce the counter because we are going to immediate get the code and increment
            if (this.Authenticator.AuthenticatorData is HOTPAuthenticator)
            {
                ((HOTPAuthenticator)this.Authenticator.AuthenticatorData).Counter--;
            }
        }

        /// <summary>
        /// Click verify button to load and check code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verifyButton_Click(object sender, EventArgs e)
        {
            string privatekey = this.secretCodeField.Text.Trim();
            if (privatekey.Length == 0)
            {
                WinAuthForm.ErrorDialog(this.Owner, "Please enter the Secret Code");
                return;
            }
            verifyAuthenticator(privatekey);
        }

        /// <summary>
        /// Select the time-based authenticator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timeBasedRadio_CheckedChanged(object sender, EventArgs e)
        {
            counterBasedRadio.Checked = !timeBasedRadio.Checked;
            if (timeBasedRadio.Checked == true)
            {
                timeBasedPanel.Visible = true;
                counterBasedPanel.Visible = false;
            }
        }

        /// <summary>
        /// Select the counter-based authenticator
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void counterBasedRadio_CheckedChanged(object sender, EventArgs e)
        {
            timeBasedRadio.Checked = !counterBasedRadio.Checked;
            if (counterBasedRadio.Checked == true)
            {
                counterBasedPanel.Visible = true;
                timeBasedPanel.Visible = false;
            }
        }

        /// <summary>
        /// Click the button to convert any secret code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void secretCodeButton_Click(object sender, EventArgs e)
        {
            Uri uri;
            Match match;

            if (Regex.IsMatch(secretCodeField.Text, "https?://.*") == true && Uri.TryCreate(secretCodeField.Text, UriKind.Absolute, out uri) == true)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AllowAutoRedirect = true;
                    request.Timeout = 20000;
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK && response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(response.GetResponseStream()))
                            {
                                IBarcodeReader reader = new BarcodeReader();
                                var result = reader.Decode(bitmap);
                                if (result != null && string.IsNullOrEmpty(result.Text) == false)
                                {
                                    secretCodeField.Text = HttpUtility.UrlDecode(result.Text);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WinAuthForm.ErrorDialog(this.Owner, "Cannot load QR code image from " + secretCodeField.Text, ex);
                    return;
                }
            }

            match = Regex.Match(secretCodeField.Text, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
            if (match.Success == true)
            {
                string authtype = match.Groups[1].Value.ToLower();
                string label = match.Groups[2].Value;

                if (authtype == HOTP)
                {
                    counterBasedRadio.Checked = true;

                    NameValueCollection qs = WinAuthHelper.ParseQueryString(match.Groups[3].Value);
                    if (qs["counter"] != null)
                    {
                        long counter;
                        if (long.TryParse(qs["counter"], out counter) == true)
                        {
                            counterField.Text = counter.ToString();
                        }
                    }

                    string issuer = qs["issuer"];
                    if (string.IsNullOrEmpty(issuer) == false)
                    {
                        label = issuer + (string.IsNullOrEmpty(label) == false ? " (" + label + ")" : string.Empty);
                    }

                    this.nameField.Text = label;
                }
                else if (authtype == TOTP)
                {
                    timeBasedRadio.Checked = true;
                    counterField.Text = string.Empty;
                }
            }
        }

        #endregion

        #region Private methods

        /// <summary>
        /// Check if a filename is valid and such a file exists
        /// </summary>
        /// <param name="filename">filename to check</param>
        /// <returns>true if valid and exists</returns>
        private bool IsValidFile(string filename)
        {
            try
            {
                // check path is valid
                new FileInfo(filename);
                return File.Exists(filename);
            }
            catch (Exception) { }

            return false;
        }

        /// <summary>
        /// Verify and create the authenticator if needed
        /// </summary>
        /// <returns>true is successful</returns>
        private bool verifyAuthenticator(string privatekey)
        {
            if (string.IsNullOrEmpty(privatekey) == true)
            {
                return false;
            }

            this.Authenticator.Name = nameField.Text;

            int digits = (this.Authenticator.AuthenticatorData != null ? this.Authenticator.AuthenticatorData.CodeDigits : GoogleAuthenticator.DEFAULT_CODE_DIGITS);
            HMACTypes hmacType = HMACTypes.SHA1;

            switch (cmbHMACType.SelectedIndex)
            {
                case 0:
                    hmacType = HMACTypes.SHA1;
                    break;
                case 1:
                    hmacType = HMACTypes.SHA256;
                    break;
                case 2:
                    hmacType = HMACTypes.SHA512;
                    break;
            }

            string authtype = timeBasedRadio.Checked == true ? TOTP : HOTP;

            long counter = 0;

            // if this is a URL, pull it down
            Uri uri;
            Match match;
            if (Regex.IsMatch(privatekey, "https?://.*") == true && Uri.TryCreate(privatekey, UriKind.Absolute, out uri) == true)
            {
                try
                {
                    var request = (HttpWebRequest)WebRequest.Create(uri);
                    request.AllowAutoRedirect = true;
                    request.Timeout = 20000;
                    request.UserAgent = "Mozilla/4.0 (compatible; MSIE 8.0; Windows NT 6.1; Trident/4.0)";
                    using (var response = (HttpWebResponse)request.GetResponse())
                    {
                        if (response.StatusCode == HttpStatusCode.OK && response.ContentType.StartsWith("image/", StringComparison.OrdinalIgnoreCase) == true)
                        {
                            using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(response.GetResponseStream()))
                            {
                                IBarcodeReader reader = new BarcodeReader();
                                var result = reader.Decode(bitmap);
                                if (result != null)
                                {
                                    privatekey = HttpUtility.UrlDecode(result.Text);
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    WinAuthForm.ErrorDialog(this.Owner, "Cannot load QR code image from " + privatekey, ex);
                    return false;
                }
            }
            else if ((match = Regex.Match(privatekey, @"data:image/([^;]+);base64,(.*)", RegexOptions.IgnoreCase)).Success == true)
            {
                byte[] imagedata = Convert.FromBase64String(match.Groups[2].Value);
                using (MemoryStream ms = new MemoryStream(imagedata))
                {
                    using (Bitmap bitmap = (Bitmap)Bitmap.FromStream(ms))
                    {
                        IBarcodeReader reader = new BarcodeReader();
                        var result = reader.Decode(bitmap);
                        if (result != null)
                        {
                            privatekey = HttpUtility.UrlDecode(result.Text);
                        }
                    }
                }
            }
            else if (IsValidFile(privatekey) == true)
            {
                // assume this is the image file
                using (Bitmap bitmap = (Bitmap)Bitmap.FromFile(privatekey))
                {
                    IBarcodeReader reader = new BarcodeReader();
                    var result = reader.Decode(bitmap);
                    if (result != null)
                    {
                        privatekey = result.Text;
                    }
                }
            }

            string issuer = null;
            string serial = null;

            // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
            match = Regex.Match(privatekey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
            if (match.Success == true)
            {
                authtype = match.Groups[1].Value.ToLower();
                string label = match.Groups[2].Value;
                int p = label.IndexOf(":");
                if (p != -1)
                {
                    issuer = label.Substring(0, p);
                    label = label.Substring(p + 1);
                }

                NameValueCollection qs = WinAuthHelper.ParseQueryString(match.Groups[3].Value);
                privatekey = qs["secret"] ?? privatekey;
                int querydigits;
                if (int.TryParse(qs["digits"], out querydigits) && querydigits != 0)
                {
                    digits = querydigits;
                }
                if (qs["counter"] != null)
                {
                    long.TryParse(qs["counter"], out counter);
                }
                issuer = qs["issuer"];
                if (string.IsNullOrEmpty(issuer) == false)
                {
                    label = issuer + (string.IsNullOrEmpty(label) == false ? " (" + label + ")" : string.Empty);
                }
                serial = qs["serial"];
                if (string.IsNullOrEmpty(label) == false)
                {
                    this.Authenticator.Name = this.nameField.Text = label;
                }
                if (qs["algorithm"] != null)
                {
                    switch (qs["algorithm"].ToUpper())
                    {
                        case "SHA1":
                            cmbHMACType.SelectedIndex = 0;
                            hmacType = HMACTypes.SHA1;
                            break;
                        case "SHA256":
                            cmbHMACType.SelectedIndex = 1;
                            hmacType = HMACTypes.SHA256;
                            break;
                        case "SHA512":
                            cmbHMACType.SelectedIndex = 2;
                            hmacType = HMACTypes.SHA512;
                            break;
                    }
                }

            }

            // just get the hex chars
            privatekey = Regex.Replace(privatekey, @"[^0-9a-z]", "", RegexOptions.IgnoreCase);
            if (privatekey.Length == 0)
            {
                WinAuthForm.ErrorDialog(this.Owner, "The secret code is not valid");
                return false;
            }

            try
            {
                Authenticator auth;
                if (authtype == TOTP)
                {
                    if (string.Compare(issuer, "BattleNet", true) == 0)
                    {
                        if (string.IsNullOrEmpty(serial) == true)
                        {
                            throw new ApplicationException("Battle.net Authenticator does not have a serial");
                        }
                        serial = serial.ToUpper();
                        if (Regex.IsMatch(serial, @"^[A-Z]{2}-?[\d]{4}-?[\d]{4}-?[\d]{4}$") == false)
                        {
                            throw new ApplicationException("Invalid serial for Battle.net Authenticator");
                        }
                        auth = new BattleNetAuthenticator();
                        ((BattleNetAuthenticator)auth).SecretKey = Base32.getInstance().Decode(privatekey);
                        ((BattleNetAuthenticator)auth).Serial = serial;

                        issuer = string.Empty;
                    }
                    else if (issuer == "Steam")
                    {
                        auth = new SteamAuthenticator();
                        ((SteamAuthenticator)auth).SecretKey = Base32.getInstance().Decode(privatekey);
                        ((SteamAuthenticator)auth).Serial = string.Empty;
                        ((SteamAuthenticator)auth).DeviceId = string.Empty;
                        //((SteamAuthenticator)auth).RevocationCode = string.Empty;
                        ((SteamAuthenticator)auth).SteamData = string.Empty;

                        this.Authenticator.Skin = null;

                        issuer = string.Empty;
                    }
                    else
                    {
                        auth = new GoogleAuthenticator();
                        ((GoogleAuthenticator)auth).Enroll(privatekey, hmacType);
                    }
                    timer.Enabled = true;
                    codeProgress.Visible = true;
                    timeBasedRadio.Checked = true;
                }
                else if (authtype == HOTP)
                {
                    auth = new HOTPAuthenticator();
                    if (counterField.Text.Trim().Length != 0)
                    {
                        long.TryParse(counterField.Text.Trim(), out counter);
                    }
                    ((HOTPAuthenticator)auth).Enroll(privatekey, counter); // start with the next code
                    timer.Enabled = false;
                    codeProgress.Visible = false;
                    counterBasedRadio.Checked = true;
                }
                else
                {
                    WinAuthForm.ErrorDialog(this.Owner, "Only TOTP or HOTP authenticators are supported");
                    return false;
                }

                auth.CodeDigits = digits;
                this.Authenticator.AuthenticatorData = auth;

                if (digits > 5)
                {
                    codeField.SpaceOut = digits / 2;
                }
                else
                {
                    codeField.SpaceOut = 0;
                }

                //string key = Base32.getInstance().Encode(this.Authenticator.AuthenticatorData.SecretKey);
                this.codeField.Text = auth.CurrentCode;

                if (!(auth is HOTPAuthenticator) && auth.ServerTimeDiff == 0L && SyncErrorWarned == false)
                {
                    SyncErrorWarned = true;
                    MessageBox.Show(this, string.Format(strings.AuthenticatorSyncError, "Google"), WinAuthMain.APPLICATION_TITLE, MessageBoxButtons.OK, MessageBoxIcon.Warning);
                }
            }
            catch (Exception irre)
            {
                WinAuthForm.ErrorDialog(this.Owner, "Unable to create the authenticator. The secret code is probably invalid.", irre);
                return false;
            }

            return true;
        }

        #endregion


    }
}
