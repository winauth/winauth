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
	/// General enroll form to create authenticator
	/// </summary>
	public partial class EnrollForm : Form
	{
		/// <summary>
		/// Get/set the current game
		/// </summary>
		public Type AuthenticatorType { get; set; }

		/// <summary>
		/// The newly created authenticator
		/// </summary>
		public Authenticator CurrentAuthenticator { get; set; }

		/// <summary>
		/// THe battle.net authenticator
		/// </summary>
		public BattleNetAuthenticator m_bnAuthenticator;

		/// <summary>
		/// The GW2 authenicator
		/// </summary>
		public GuildWarsAuthenticator m_gw2Authenticator;

		/// <summary>
		/// Gap between groups for resizing
		/// </summary>
		private int m_gap;

		/// <summary>
		/// Gap at bottom for rezising
		/// </summary>
		private int m_bottomGap;

		/// <summary>
		/// Original height of dialog
		/// </summary>
		private int m_height;

		/// <summary>
		/// Time for next code refresh
		/// </summary>
		private DateTime NextRefresh { get; set; }

		/// <summary>
		/// Current code field
		/// </summary>
		private SecretTextBox m_codeField;

		/// <summary>
		/// Create the  Form
		/// </summary>
		public EnrollForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Load the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnrollForm_Load(object sender, EventArgs e)
		{
			m_gap = bnGroup.Top - (btnBattlenet.Top + btnBattlenet.Height);
			m_bottomGap = this.Height - Math.Max(bnGroup.Top + bnGroup.Height, gw2Group.Top + gw2Group.Height);
			btnGuildwars.Left = btnBattlenet.Left;
			btnGuildwars.Top = btnBattlenet.Top + btnBattlenet.Height + m_gap;
			gw2Group.Left = bnGroup.Left;
			bnGroup.Visible = false;
			gw2Group.Visible = false;
			this.Height = m_height = btnGuildwars.Top + btnGuildwars.Height + m_bottomGap;
			this.Width = btnBattlenet.Width + 42;
		}

		#region Battle.Net

		/// <summary>
		/// Click the Battle.net button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnBattlenet_Click(object sender, EventArgs e)
		{
			AuthenticatorType = typeof(BattleNetAuthenticator);
			m_codeField = null;

			// show the right form
			if (gw2Group.Visible == true)
			{
				this.Height = m_height;
				gw2Group.Visible = false;
			}
			bnGroup.Visible = true;
			this.Height = m_height + bnGroup.Height + m_gap;
			btnGuildwars.Top = bnGroup.Top + bnGroup.Height + m_gap;
			btnGuildwars.Left = btnBattlenet.Left;

			// use saved if we have one
			if (m_bnAuthenticator != null)
			{
				CurrentAuthenticator = m_bnAuthenticator;
				m_codeField = bnCode;
			}
		}

		/// <summary>
		/// Clickc the register button to enroll authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gnRegister_Click(object sender, EventArgs e)
		{
			if (m_bnAuthenticator == null)
			{
				bnSerial.Text = "registering...";
				m_bnAuthenticator = new BattleNetAuthenticator();
				System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(createBnAuthenticator));
				thread.Start();
			}
			CurrentAuthenticator = m_bnAuthenticator;
			refreshTimer.Enabled = true;
		}

		/// <summary>
		/// Enroll the authenticator in a new thread
		/// </summary>
		private void createBnAuthenticator()
		{
			m_bnAuthenticator.Enroll();
			m_codeField = bnCode;
			this.Invoke(new MethodInvoker(delegate()
				{
					bnCode.Text = m_bnAuthenticator.CurrentCode;
					bnSerial.Text = m_bnAuthenticator.Serial.Replace("-", "");
					bnRestoreCode.Text = m_bnAuthenticator.RestoreCode;
				}
			));
		}

		/// <summary>
		/// Change the seceret text boxes to allow copying
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bncopyCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			bnCode.SecretMode = !bnAllowCopy.Checked;
			bnSerial.SecretMode = !bnAllowCopy.Checked;
			bnRestoreCode.SecretMode = !bnAllowCopy.Checked;
		}

		/// <summary>
		/// Click to finish and save authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void bnFinish_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		#endregion

		#region GuildWars2

		/// <summary>
		/// Click the GuildWars2 button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void btnGuildwars_Click(object sender, EventArgs e)
		{
			AuthenticatorType = typeof(GuildWarsAuthenticator);
			m_codeField = null;

			// set sizes
			if (bnGroup.Visible == true)
			{
				this.Height = m_height;
				bnGroup.Visible = false;
			}
			btnGuildwars.Top = btnBattlenet.Top + btnBattlenet.Height + m_gap;
			gw2Group.Visible = true;
			gw2Group.Top = btnGuildwars.Top + btnGuildwars.Height + m_gap;
			this.Height = m_height + gw2Group.Height + m_gap;

			// use existing authenticator if we have one
			if (m_gw2Authenticator != null)
			{
				CurrentAuthenticator = m_gw2Authenticator;
				m_codeField = gw2Code;
			}
		}

		/// <summary>
		/// Click the enroll the new authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gw2GenerateCodeButton_Click(object sender, EventArgs e)
		{
			gw2Code.Text = "registering...";
			if (m_gw2Authenticator == null)
			{
				m_gw2Authenticator = new GuildWarsAuthenticator();
				System.Threading.Thread thread = new System.Threading.Thread(new System.Threading.ThreadStart(createGw2Authenticator));
				thread.Start();
			}
			CurrentAuthenticator = m_gw2Authenticator;
			refreshTimer.Enabled = true;
		}

		/// <summary>
		/// Create (and sync time) for GW2 authenticator
		/// </summary>
		private void createGw2Authenticator()
		{
			m_gw2Authenticator.Enroll(gw2SecretCode.Text.Trim());
			m_codeField = gw2Code;
			this.Invoke(new MethodInvoker(delegate()
				{
					gw2Code.Text = m_gw2Authenticator.CurrentCode;
				}
			));
		}

		/// <summary>
		/// Update the code field for the current authenticator - can be called from differetn thread
		/// </summary>
		private void updateCode()
		{
			if (m_codeField == null || CurrentAuthenticator == null)
			{
				return;
			}

			if (this.InvokeRequired)
			{
				this.Invoke(new MethodInvoker(delegate()
					{
						m_codeField.Text = CurrentAuthenticator.CurrentCode;
					}
				));
			}
			else
			{
				m_codeField.Text = CurrentAuthenticator.CurrentCode;
			}
		}
		
		/// <summary>
		/// Timer tick to update code and progress bar
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void refreshTimer_Tick(object sender, EventArgs e)
		{
			if (CurrentAuthenticator != null && m_codeField != null)
			{
				DateTime now = DateTime.Now;

				int tillUpdate = (int)((CurrentAuthenticator.ServerTime % 30000L) / 1000L);
				if (CurrentAuthenticator is BattleNetAuthenticator)
				{
					bnProgressBar.Value = tillUpdate;
				}
				else if (CurrentAuthenticator is GuildWarsAuthenticator)
				{
					gw2ProgressBar.Value = tillUpdate;
				}
				if (tillUpdate == 0)
				{
					NextRefresh = now;
				}
				if (now >= NextRefresh)
				{
					NextRefresh = now.AddSeconds(30);
					updateCode();
				}
			}
		}

		/// <summary>
		/// Change the secret text boxes to allow copying
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gw2copyCheckbox_CheckedChanged(object sender, EventArgs e)
		{
			gw2Code.SecretMode = !gw2copyCheckbox.Checked;
		}

		/// <summary>
		/// Click to finish and save authenticator
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void gw2Finished_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		#endregion

	}

}

