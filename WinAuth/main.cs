/* Copyright (C) 2010 Colin Mackie.
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
	public partial class main : Form
	{
		#region Initialization

		public main()
		{
			InitializeComponent();
		}

		#endregion

		#region Member Data

		private Authenticator m_authenticator;
		private bool m_autoRefresh = true;
		private bool m_alwaysOnTop = true;
		private bool m_copyOnCode = true;
		private DateTime m_nextRefresh = DateTime.MaxValue;

		#endregion


		#region Properties

		public Authenticator Authenticator
		{
			get
			{
				return m_authenticator;
			}
			set
			{
				m_authenticator = value;
			}
		}

		public bool AutoRefresh
		{
			get
			{
				return m_autoRefresh;
			}
			set
			{
				m_autoRefresh = value;
				progressBar.Visible = (Authenticator != null && m_autoRefresh == true);
			}
		}

		public bool AlwaysOnTop
		{
			get
			{
				return m_alwaysOnTop;
			}
			set
			{
				m_alwaysOnTop = value;
				this.TopMost = m_alwaysOnTop;
			}
		}

		public bool CopyOnCode
		{
			get
			{
				return m_copyOnCode;
			}
			set
			{
				m_copyOnCode = value;
			}
		}

		public DateTime NextRefresh
		{
			get
			{
				return m_nextRefresh;
			}
			set
			{
				m_nextRefresh = value;
			}
		}

		#endregion

		#region Private Functions

		private void Enroll()
		{
			// initialise the new authenticator
			//m_authenticator = new Authenticator("US");
			//m_authenticator.Enroll();

			AuthenticatorData data = new AuthenticatorData();
			data.MobileAuthenticatorHash = "0fea429a64105901525080d61591f35b37f22713100fbd4ddcc7aa9be30868b7aa6e4a86e15c445cf0c8894d27fa5b494834642e76bdb95ca9";
			Authenticator = new Authenticator("US", data, -75481L);

			MessageBox.Show(this,
				"Your authenticator has been sucessfully initialized.\n\nYou will need to add the follow serial number onto your account:\n\n"
					+ m_authenticator.Data.Serial + "\n\n"
					+ "You should also right it down as you may be asked to provide it if you ever need to remove the authenticator.",
				"Initailized", MessageBoxButtons.OK, MessageBoxIcon.Information);
		}

		private void ShowCode()
		{
			if (Authenticator == null)
			{
				DialogResult result = MessageBox.Show(this, "This authenticator has not been initialized. Would you like to continue?", "Initialize", MessageBoxButtons.YesNo, MessageBoxIcon.Exclamation);
				if (result != System.Windows.Forms.DialogResult.Yes)
				{
					return;
				}

				Enroll();
			}

			if (Authenticator != null)
			{
				string code = Authenticator.CalculateCode();
				codeField.Text = code;
				if (CopyOnCode == true)
				{
					Clipboard.Clear();
					Clipboard.SetDataObject(code, true);
				}

				serialLabel.Text = Authenticator.Data.Serial;
			}
			else
			{
				codeField.Text = string.Empty;
				serialLabel.Text = string.Empty;
			}

			refreshTimer.Enabled = true;
			progressBar.Visible = (Authenticator != null && AutoRefresh == true);
		}

		#endregion

		#region Events

		private void main_Load(object sender, EventArgs e)
		{
			serialLabel.Text = string.Empty;
			codeField.Text = string.Empty;
			progressBar.Value = 0;
			progressBar.Visible = (Authenticator != null && AutoRefresh == true);
		}

		private void calcCodeButton_Click(object sender, EventArgs e)
		{
			ShowCode();
		}

		private void contextMenuStrip_Opening(object sender, CancelEventArgs e)
		{
			syncServerTimeMenuItem.Enabled = (Authenticator != null);
			copyOnCodeMenuItem.Enabled = (Authenticator != null);
			autoRefreshMenuItem.Enabled = (Authenticator != null);

			autoRefreshMenuItem.Checked = (autoRefreshMenuItem.Enabled == true ? AutoRefresh : false);
			copyOnCodeMenuItem.Checked = (copyOnCodeMenuItem.Enabled == true ? CopyOnCode : false);
			alwaysOnTopMenuItem.Checked = (alwaysOnTopMenuItem.Enabled == true ? AlwaysOnTop : false);
		}

		private void syncServerTimeMenuItem_Click(object sender, EventArgs e)
		{
			Authenticator.Sync();
			MessageBox.Show(this, "Time synced successfully.", "Sync Time", MessageBoxButtons.OK);
		}

		private void registerMenuItem_Click(object sender, EventArgs e)
		{
			Enroll();
			ShowCode();
		}

		private void exitMeuuItem_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void autoRefreshMenuItem_Click(object sender, EventArgs e)
		{
			AutoRefresh = !autoRefreshMenuItem.Checked;
		}

		private void copyOnCodeMenuItem_Click(object sender, EventArgs e)
		{
			CopyOnCode = !copyOnCodeMenuItem.Checked;
		}

		private void alwaysOnTopMenuItem_Click(object sender, EventArgs e)
		{
			AlwaysOnTop = !alwaysOnTopMenuItem.Checked;
		}

		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			DateTime now = DateTime.Now;

			if (Authenticator == null)
			{
				refreshTimer.Enabled = false;
				ShowCode();
				return;
			}

			if (progressBar.Visible == true)
			{
				int tillUpdate = (int)((Authenticator.ServerTime % 30000L) / 1000L);
				progressBar.Value = tillUpdate;
				if (tillUpdate == 0)
				{
					NextRefresh = now;
				}
			}
			if (AutoRefresh == true && Authenticator != null && now >= NextRefresh)
			{
				NextRefresh = now.AddSeconds(30);
				ShowCode();
			}
		}

		#endregion

	}
}
