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
using System.IO;
using System.Net.Mail;
using System.Text;
using System.Windows.Forms;

using ICSharpCode.SharpZipLib.Zip;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Form to send a copy of the current Authenticator data to an email account
	/// </summary>
	public partial class BackupForm : Form
	{
		/// <summary>
		/// Inner class holding SmtpServer definition for our quick pick
		/// </summary>
		class SmtpServer
		{
			/// <summary>
			/// Smtp server host name
			/// </summary>
			public string Host;

			/// <summary>
			/// Smtp server port
			/// </summary>
			public int Port;

			/// <summary>
			/// Smtp server SSL flag
			/// </summary>
			public bool SSL;

			/// <summary>
			/// Create an SmtpServer object for display
			/// </summary>
			/// <param name="host"></param>
			/// <param name="port"></param>
			/// <param name="ssl"></param>
			public SmtpServer(string host, int port, bool ssl)
			{
				Host = host;
				Port = port;
				SSL = ssl;
			}

			/// <summary>
			/// Get string version showing Host
			/// </summary>
			/// <returns></returns>
			public override string ToString()
			{
				return Host;
			}
		}

		/// <summary>
		/// Prebuilt list of Smtp servers for quick pick
		/// </summary>
		private static List<SmtpServer> m_smtpServers = new List<SmtpServer>()
		{
			{new SmtpServer("smtp.gmail.com", 587, true)},
			{new SmtpServer("plus.smtp.mail.yahoo.com", 465, true)},
			{new SmtpServer("smtp.att.yahoo.com", 465, true)}			
		};

		/// <summary>
		/// The current WinAuth config
		/// </summary>
		public WinAuthConfig CurrentConfig { get; set; }

		/// <summary>
		/// Create a new form obbject
		/// </summary>
		public BackupForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Send the current config file as a zipped file by email
		/// </summary>
		/// <returns>DialogResult for Retry, Cancel or OK</returns>
		private DialogResult SendBackup()
		{
			// get the authenticator file
			string datafile = CurrentConfig.AuthenticatorFile;

			// any password?
			string password = tbPassword.Text.Trim();

			// create a temp file containing the zipped data file
			string zipfile = Path.Combine(Path.GetTempPath(), Path.GetFileName(datafile) + ".zip");
			try {
				// send wait cursor
				Cursor.Current = Cursors.WaitCursor;

				using (FileStream zipfs = File.Create(zipfile))
				{
					using (ZipOutputStream zos = new ZipOutputStream(zipfs))
					{
						// encrypt if we have a password
						bool encrypt = false;
						if (password.Length != 0)
						{
							zos.Password = password;
							encrypt = true;
						}

						// add the authenticator file
						ZipEntry entry = new ZipEntry(Path.GetFileName(datafile));
						entry.IsCrypted = encrypt;
						entry.DateTime = new FileInfo(datafile).LastWriteTime;
						zos.PutNextEntry(entry);
						using (FileStream fs = File.OpenRead(datafile))
						{
							byte[] buffer = new byte[4096];
							int count;
							while ((count = fs.Read(buffer, 0, buffer.Length)) != 0)
							{
								zos.Write(buffer, 0, count);
							}
						}

						// close zip file
						zos.Finish();
						zos.Close();
					}
				}

				// now send the file
				MailSender ms = new MailSender();
				if (this.ckSmtpServer.Checked == true)
				{
					ms.SmtpServer = this.cbSmtpServer.Text;
					ms.SmtpPort = Convert.ToInt32(cbSmtpPorts.Text);
					ms.SmtpSSL = ckSmtpSSL.Checked;

					if (this.tbSmtpUsername.Text.Length != 0)
					{
						ms.SmtpUsername = this.tbSmtpUsername.Text;
						ms.SmtpPasword = this.tbSmtpPassword.Text;
					}
				}
				ms.Send(new MailAddress(WinAuth.WINAUTHBACKUP_EMAIL), new MailAddress(this.tbEmail.Text), this.tbSubject.Text, string.Empty, new string[] { zipfile });

				// reset cursor
				Cursor.Current = Cursors.Default;

				// confirm dialog
				MessageBox.Show(this, "Your email has been sent.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
			catch (Exception ex)
			{
				// reset cursor
				Cursor.Current = Cursors.Default;
				return MessageBox.Show(this, "An error occured sending your backup: " + ex.Message, this.Text, MessageBoxButtons.RetryCancel, MessageBoxIcon.Exclamation);
			}
			finally
			{
				try {
					File.Delete(zipfile);
				} catch (Exception ) {}
			}

			return DialogResult.OK;
		}

		#region Form Events

		/// <summary>
		/// Load the form and setup controls
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void BackupForm_Load(object sender, EventArgs e)
		{
			// load the smtp servers
			this.cbSmtpServer.Items.Clear();
			foreach (SmtpServer smtpserver in m_smtpServers)
			{
				this.cbSmtpServer.Items.Add(smtpserver);
			}

			this.ActiveControl = tbEmail;
		}

		/// <summary>
		/// Click to send the backup
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnBackupSave_Click(object sender, EventArgs e)
		{
			// validate
			if (this.tbPassword.Text.Length != 0 && this.tbPassword.Text != this.tbPasswordVerify.Text)
			{
				MessageBox.Show(this, "Passwords do not match", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}
			if (this.ckSmtpServer.Checked == true && this.cbSmtpServer.Text.Length == 0)
			{
				MessageBox.Show(this, "Please enter your SMTP Server details.", this.Text, MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
				return;
			}

			DialogResult result;
			while ((result = SendBackup()) == DialogResult.Retry) { }
			if (result == System.Windows.Forms.DialogResult.OK)
			{
				this.DialogResult = result;
			}
		}

		/// <summary>
		/// Enable the custom smtp server settings
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ckSmtpServer_CheckedChanged(object sender, EventArgs e)
		{
			grpSmtpServer.Enabled = ckSmtpServer.Checked;
		}

		/// <summary>
		/// Change a quick pick of Smtp Sever
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cbSmtpServer_SelectedIndexChanged(object sender, EventArgs e)
		{
			if (cbSmtpServer.SelectedItem != null && cbSmtpServer.SelectedItem is SmtpServer)
			{
				this.cbSmtpPorts.Text = ((SmtpServer)cbSmtpServer.SelectedItem).Port.ToString();
				this.ckSmtpSSL.Checked = ((SmtpServer)cbSmtpServer.SelectedItem).SSL;
			}
		}

		#endregion


	}
}
