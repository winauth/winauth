/*
 * Copyright (C) 2017 Colin Mackie.
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
using System.Windows.Forms;
using System.Web;

using ZXing;

namespace WinAuth
{
    /// <summary>
    /// Okta Verify Authenticator Form
    /// </summary>
    public partial class AddOktaVerifyAuthenticator : ResourceForm
    {    
        /// <summary>
        /// Form instantiation
        /// </summary>
        public AddOktaVerifyAuthenticator()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Current authenticator
        /// </summary>
        public WinAuthAuthenticator Authenticator { get; set; }

        #region Form Events

        /// <summary>
        /// Load the form
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void AddOktaVerifyAuthenticator_Load(object sender, EventArgs e)
        {
            nameField.Text = this.Authenticator.Name;
            codeField.SecretMode = true;
        }

        /// <summary>
        /// Ticker event
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void newAuthenticatorTimer_Tick(object sender, EventArgs e)
        {
            if (this.Authenticator.AuthenticatorData != null && newAuthenticatorProgress.Visible == true)
            {
                int time = (int)(this.Authenticator.AuthenticatorData.ServerTime / 1000L) % 30;
                newAuthenticatorProgress.Value = time + 1;
                if (time == 0)
                {
                    codeField.Text = this.Authenticator.AuthenticatorData.CurrentCode;
                }
            }
        }

     
        /// <summary>
        /// Click to add the code
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void verifyAuthenticatorButton_Click(object sender, EventArgs e)
        {
            string privatekey = this.secretCodeField.Text.Trim();
            if (string.IsNullOrEmpty(privatekey) == true)
            {
                WinAuthForm.ErrorDialog(this, "Please enter the secret code");
                return;
            }

            verifyAuthenticator(privatekey);
        }

        /// <summary>
        /// Click the cancel button and show a warning
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cancelButton_Click(object sender, EventArgs e)
        {
            if (this.Authenticator.AuthenticatorData != null)
            {
                DialogResult result = WinAuthForm.ConfirmDialog(this.Owner,
                    "WARNING: Your authenticator has not been saved." + Environment.NewLine + Environment.NewLine
                    + "If you have added this authenticator to your account, you will not be able to login in the future, and you need to click YES to save it." + Environment.NewLine + Environment.NewLine
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

            bool first = !this.newAuthenticatorProgress.Visible;
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

            if (this.Authenticator.AuthenticatorData == null)
            {
                WinAuthForm.ErrorDialog(this.Owner, "Please enter the Secret Code and click Verify Authenticator");
                this.DialogResult = System.Windows.Forms.DialogResult.None;
                return;
            }
        }

        /// <summary>
        /// Select one of the icons
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

        private void helpLink_Click(object sender, EventArgs e)
        {
            System.Diagnostics.Process.Start("https://support.okta.com/help/Documentation/Knowledge_Article/How-to-Configure-WinAuth-for-Okta-MFA");
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

            string authtype = "totp";

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

            // check for otpauth://, e.g. "otpauth://totp/dc3bf64c-2fd4-40fe-a8cf-83315945f08b@blockchain.info?secret=IHZJDKAEEC774BMUK3GX6SA"
            match = Regex.Match(privatekey, @"otpauth://([^/]+)/([^?]+)\?(.*)", RegexOptions.IgnoreCase);
            if (match.Success == true)
            {
                authtype = match.Groups[1].Value; // @todo we only handle totp (not hotp)
                if (string.Compare(authtype, "totp", true) != 0)
                {
                    WinAuthForm.ErrorDialog(this.Owner, "Only time-based (TOTP) authenticators are supported when adding an Okta Verify Authenticator. Use the general \"Add Authenticator\" for counter-based (HOTP) authenticators.");
                    return false;
                }

                string label = match.Groups[2].Value;
                if (string.IsNullOrEmpty(label) == false)
                {
                    this.Authenticator.Name = this.nameField.Text = label;
                }

                NameValueCollection qs = WinAuthHelper.ParseQueryString(match.Groups[3].Value);
                privatekey = qs["secret"] ?? privatekey;
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
                OktaVerifyAuthenticator authenticator = new OktaVerifyAuthenticator();
                authenticator.Enroll(privatekey);
                this.Authenticator.AuthenticatorData = authenticator;
                this.Authenticator.Name = this.nameField.Text;

                codeField.Text = authenticator.CurrentCode;
                newAuthenticatorProgress.Visible = true;
                newAuthenticatorTimer.Enabled = true;
            }
            catch (Exception ex)
            {
                WinAuthForm.ErrorDialog(this.Owner, "Unable to create the authenticator: " + ex.Message, ex);
                return false;
            }

            return true;
        }

        #endregion
    }
}
