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
using System.Runtime.InteropServices;
using System.Runtime.Serialization;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
#if NETFX_4
using System.Threading.Tasks;
#endif
using System.Windows.Forms;
using MetroFramework.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace WinAuth
{
	/// <summary>
	/// Show Steam trade confirmations
	/// </summary>
	public partial class ShowSteamTradesForm : ResourceForm
	{
		[DllImport("wininet.dll", CharSet = CharSet.Auto, SetLastError = true)]
		static extern bool InternetSetCookie(string lpszUrlName, string lpszCookieName, string lpszCookieData);

		class PollerActionItem
		{
			public string Text;
			public SteamClient.PollerAction Value;

			public override string ToString()
			{
				return this.Text;
			}
		}

		/// <summary>
		/// Form instantiation
		/// </summary>
		public ShowSteamTradesForm()
		{
			InitializeComponent();
		}

		/// <summary>
		/// Authenticator
		/// </summary>
		public WinAuthAuthenticator Authenticator { get; set; }

		/// <summary>
		/// If been warned about auto
		/// </summary>
		private bool AutoWarned;

		/// <summary>
		/// Steam authenticator
		/// </summary>
		public SteamAuthenticator AuthenticatorData
		{
			get
			{
				return this.Authenticator != null ? this.Authenticator.AuthenticatorData as SteamAuthenticator : null;
			}
		}

		/// <summary>
		/// Trade info state
		/// </summary>
		private List<SteamClient.Confirmation> m_trades;

		/// <summary>
		/// Saved height of browser for showing details
		/// </summary>
		private int m_browserHeight;

		/// <summary>
		/// When form has been loaded
		/// </summary>
		private bool m_loaded;

#if NETFX_4
		/// <summary>
		/// Cancellation token for confirm all
		/// </summary>
		private CancellationTokenSource cancelComfirmAll;

		/// <summary>
		/// Cancellation token for cancel all
		/// </summary>
		private CancellationTokenSource cancelCancelAll;
#endif

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
		private void ShowSteamTradesForm_Load(object sender, EventArgs e)
		{
			this.MinimumSize = this.Size;

			m_browserHeight = browserContainer.Height;
			browserContainer.Height = 0;
			tradesContainer.Height += m_browserHeight;

			this.pollAction.Items.Clear();

			BindingList<object> items = new BindingList<object>();
			items.Add(new PollerActionItem { Text = "Show Notification", Value = SteamClient.PollerAction.Notify });
			items.Add(new PollerActionItem { Text = "Auto-Confirm", Value = SteamClient.PollerAction.AutoConfirm });
			items.Add(new PollerActionItem { Text = "Auto-Confirm (silently)", Value = SteamClient.PollerAction.SilentAutoConfirm });

			this.pollAction.DataSource = items;
			this.pollAction.DisplayMember = "Text";
			//this.pollAction.ValueMember = "Value";
			this.pollAction.SelectedIndex = 0;

			m_tabPages.Clear();
			for (var i = 0; i < tabs.TabPages.Count; i++)
			{
				m_tabPages.Add(tabs.TabPages[i].Name, tabs.TabPages[i]);
			}

			Init();

			m_loaded = true;
		}

		/// <summary>
		/// Set focus when loading
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowSteamTradesForm_Shown(object sender, EventArgs e)
		{
			this.usernameField.Focus();
		}

		/// <summary>
		/// If we close after adding, make sure we save it
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowSteamTradesForm_FormClosing(object sender, FormClosingEventArgs e)
		{
			// update poller
			SetPolling();

			this.DialogResult = System.Windows.Forms.DialogResult.OK;
		}

		/// <summary>
		/// Press the form's cancel button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.Cancel;
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

			Process(usernameField.Text.Trim(), passwordField.Text.Trim(), this.AuthenticatorData.GetClient().CaptchaId, captchacodeField.Text.Trim());
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

			Process(usernameField.Text.Trim(), passwordField.Text.Trim());
		}

		/// <summary>
		/// CLick the close button to save
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void closeButton_Click(object sender, EventArgs e)
		{
			this.DialogResult = System.Windows.Forms.DialogResult.OK;
			this.Close();
		}

		/// <summary>
		/// Handle the enter key on the form
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void ShowSteamTradesForm_KeyPress(object sender, KeyPressEventArgs e)
		{
			if (e.KeyChar == 13)
			{
				switch (tabs.SelectedTab.Name)
				{
					case "loginTab":
						e.Handled = true;

						if (this.AuthenticatorData.GetClient().RequiresCaptcha == true)
						{
							captchaButton_Click(sender, new EventArgs());
						}
						else
						{
							loginButton_Click(sender, new EventArgs());
						}
						break;
					case "tradesTab":
						e.Handled = true;
						//authcodeButton_Click(sender, new EventArgs());
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
		/// Click the Trade row where we load the details and show in a browser
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void Trade_Click(object sender, EventArgs e)
		{
			// get the Trade object
			Label panel = sender as Label;
			var trade = m_trades.Where(t => t.Id == panel.Tag as string).FirstOrDefault();
			if (trade == null)
			{
				return;
			}

			// inject browser
			TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel browser;
			if (m_browserHeight != 0)
			{
				tradesContainer.Height -= m_browserHeight;
				browserContainer.Height = m_browserHeight;
				m_browserHeight = 0;

				browser = new TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel();
				browser.Dock = DockStyle.Fill;
				browser.Location = new Point(0, 0);
				browser.Size = new Size(browserContainer.Width, browserContainer.Height);
				browserContainer.Controls.Add(browser);
			}
			else
			{
				browser = browserContainer.Controls[0] as TheArtOfDev.HtmlRenderer.WinForms.HtmlPanel;
			}

			// pull details segment and merge with normal html
			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				browser.Text = this.AuthenticatorData.GetClient().GetConfirmationDetails(trade);
			}
			catch (Exception ex)
			{
				browser.Text = "<html><head></head><body><p>" + ex.Message + "</p>" + ex.StackTrace.Replace(Environment.NewLine, "<br>") + "</body></html>";
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		/// <summary>
		/// Click the Accept trade button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
#if NETFX_4
		private async void tradeAccept_Click(object sender, EventArgs e)
#endif
#if NETFX_3
		private void tradeAccept_Click(object sender, EventArgs e)
#endif
		{
			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				MetroButton button = sender as MetroButton;
				string tradeId = button.Tag as string;
#if NETFX_4
				await AcceptTrade(tradeId);
#endif
#if NETFX_3
				AcceptTrade(tradeId);
#endif
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		/// <summary>
		/// Click the Reject trade button
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
#if NETFX_4
		private async void tradeReject_Click(object sender, EventArgs e)
#endif
#if NETFX_3
		private void tradeReject_Click(object sender, EventArgs e)
#endif
		{
			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				MetroButton button = sender as MetroButton;
				string tradeId = button.Tag as string;
#if NETFX_4
				await RejectTrade(tradeId);
#endif
#if NETFX_3
				RejectTrade(tradeId);
#endif
			}
			finally
			{
				Cursor.Current = cursor;
			}
		}

		/// <summary>
		/// Refresh the list
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void refreshButton_Click(object sender, EventArgs e)
		{
			Process();
		}

		/// <summary>
		/// Logout of session
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void logoutButton_Click(object sender, EventArgs e)
		{
			var steam = this.AuthenticatorData.GetClient();
			steam.Logout();

			if (String.IsNullOrEmpty(AuthenticatorData.SessionData) == false)
			{
				AuthenticatorData.SessionData = null;
				//AuthenticatorData.PermSession = false;
				this.Authenticator.MarkChanged();
			}

			Init();
		}

#if NETFX_4
		/// <summary>
		/// Click the button to confirm all confirmations
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void confirmAllButton_Click(object sender, EventArgs e)
		{
			if (cancelComfirmAll != null)
			{
				confirmAllButton.Text = "Stopping";
				cancelComfirmAll.Cancel();
				return;
			}

			if (WinAuthForm.ConfirmDialog(this, "This will CONFIRM all your current trade confirmations." + Environment.NewLine + Environment.NewLine +
				"Are you sure you want to continue?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
			{
				return;
			}

			cancelComfirmAll = new CancellationTokenSource();

			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				confirmAllButton.Tag = confirmAllButton.Text;
				confirmAllButton.Text = "Stop";
				cancelAllButton.Enabled = false;
				closeButton.Enabled = false;

				var rand = new Random();
				var tradeIds = m_trades.Select(t => t.Id).Reverse().ToArray();
				for (var i = tradeIds.Length - 1; i >= 0; i--)
				{
					if (cancelComfirmAll.IsCancellationRequested)
					{
						break;
					}

					DateTime start = DateTime.Now;

					var result = await AcceptTrade(tradeIds[i]);
					if (result == false || cancelComfirmAll.IsCancellationRequested == true)
					{
						break;
					}
					if (i != 0)
					{
						var duration = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
						var delay = SteamClient.CONFIRMATION_EVENT_DELAY + rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY / 2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
						if (delay > duration)
						{
							await Task.Run(() => { Thread.Sleep(delay - duration); }, cancelComfirmAll.Token);
						}
					}
				}

			}
			finally
			{
				cancelComfirmAll = null;

				confirmAllButton.Text = (string)confirmAllButton.Tag;

				cancelAllButton.Enabled = true;
				closeButton.Enabled = true;

				this.Authenticator.MarkChanged();

				Cursor.Current = cursor;
			}
		}
#endif
#if NETFX_3
		/// <summary>
		/// Click the button to confirm all confirmations
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void confirmAllButton_Click(object sender, EventArgs e)
		{
			if (WinAuthForm.ConfirmDialog(this, "This will CONFIRM all your current trade confirmations." + Environment.NewLine + Environment.NewLine +
				"Are you sure you want to continue?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
			{
				return;
			}

			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				cancelAllButton.Enabled = false;
				closeButton.Enabled = false;

				var rand = new Random();
				var tradeIds = m_trades.Select(t => t.Id).Reverse().ToArray();
				for (var i = tradeIds.Length - 1; i >= 0; i--)
				{
					DateTime start = DateTime.Now;

					var result = AcceptTrade(tradeIds[i]);
					if (result == false)
					{
						break;
					}
					if (i != 0)
					{
						var duration = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
						var delay = SteamClient.CONFIRMATION_EVENT_DELAY + rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY / 2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
						if (delay > duration)
						{
							Thread.Sleep(delay - duration);
						}
					}
				}

			}
			finally
			{
				cancelAllButton.Enabled = true;
				closeButton.Enabled = true;

				this.Authenticator.MarkChanged();

				Cursor.Current = cursor;
			}
		}
#endif

#if NETFX_4
		/// <summary>
		/// Click the button to cancel all confirmations
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private async void cancelAllButton_Click(object sender, EventArgs e)
		{
			if (cancelCancelAll != null)
			{
				cancelAllButton.Text = "Stopping";
				cancelCancelAll.Cancel();
				return;
			}

			if (WinAuthForm.ConfirmDialog(this, "This will CANCEL all your current trade confirmations." + Environment.NewLine + Environment.NewLine +
				"Are you sure you want to continue?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
			{
				return;
			}

			cancelCancelAll = new CancellationTokenSource();

			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				cancelAllButton.Tag = cancelAllButton.Text;
				cancelAllButton.Text = "Stop";
				confirmAllButton.Enabled = false;
				closeButton.Enabled = false;

				var rand = new Random();
				var tradeIds = m_trades.Select(t => t.Id).Reverse().ToArray();
				for (var i=tradeIds.Length-1; i >= 0; i--)
				{
					if (cancelCancelAll.IsCancellationRequested)
					{
						break;
					}

					DateTime start = DateTime.Now;

					var result = await RejectTrade(tradeIds[i]);
					if (result == false || cancelCancelAll.IsCancellationRequested == true)
					{
						break;
					}
					if (i != 0)
					{
						var duration = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
						var delay = SteamClient.CONFIRMATION_EVENT_DELAY + rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY / 2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
						if (delay > duration)
						{
							await Task.Run(() => { Thread.Sleep(delay - duration); }, cancelCancelAll.Token);
						}
					}
				}

			}
			finally
			{
				cancelCancelAll = null;

				cancelAllButton.Text = (string)cancelAllButton.Tag;

				confirmAllButton.Enabled = true;
				closeButton.Enabled = true;

				this.Authenticator.MarkChanged();

				Cursor.Current = cursor;
			}
		}
#endif
#if NETFX_3
		/// <summary>
		/// Click the button to cancel all confirmations
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void cancelAllButton_Click(object sender, EventArgs e)
		{
			if (WinAuthForm.ConfirmDialog(this, "This will CANCEL all your current trade confirmations." + Environment.NewLine + Environment.NewLine +
				"Are you sure you want to continue?",
				MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
			{
				return;
			}

			var cursor = Cursor.Current;
			try
			{
				Cursor.Current = Cursors.WaitCursor;
				Application.DoEvents();

				cancelAllButton.Text = "Stop";
				confirmAllButton.Enabled = false;
				closeButton.Enabled = false;

				var rand = new Random();
				var tradeIds = m_trades.Select(t => t.Id).Reverse().ToArray();
				for (var i=tradeIds.Length-1; i >= 0; i--)
				{
					DateTime start = DateTime.Now;

					var result = RejectTrade(tradeIds[i]);
					if (result == false)
					{
						break;
					}
					if (i != 0)
					{
						var duration = (int)DateTime.Now.Subtract(start).TotalMilliseconds;
						var delay = SteamClient.CONFIRMATION_EVENT_DELAY + rand.Next(SteamClient.CONFIRMATION_EVENT_DELAY / 2); // delay is 100%-150% of CONFIRMATION_EVENT_DELAY
						if (delay > duration)
						{
							Thread.Sleep(delay - duration);
						}
					}
				}

			}
			finally
			{
				confirmAllButton.Enabled = true;
				closeButton.Enabled = true;

				this.Authenticator.MarkChanged();

				Cursor.Current = cursor;
			}
		}
#endif

		/// <summary>
		/// Change the poller action
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void pollAction_SelectedIndexChanged(object sender, EventArgs e)
		{
			// display autoconfirm warning
			if (m_loaded == true
				&& pollAction.SelectedValue != null
				&& pollAction.SelectedValue is SteamClient.PollerAction
				&& ((SteamClient.PollerAction)pollAction.SelectedValue == SteamClient.PollerAction.AutoConfirm || (SteamClient.PollerAction)pollAction.SelectedValue == SteamClient.PollerAction.SilentAutoConfirm)
				&& AutoWarned == false)
			{
				if (WinAuthForm.ConfirmDialog(this, "WARNING: Using auto-confirm will automatically confirm all new Confirmations on your "
					+ "account. Do not use this unless you want to ignore trade confirmations." + Environment.NewLine + Environment.NewLine
					+ "This WILL remove items from your inventory." + Environment.NewLine + Environment.NewLine
					+ "Are you sure you want to continue?", MessageBoxButtons.YesNo, MessageBoxIcon.Warning, MessageBoxDefaultButton.Button2) != DialogResult.Yes)
				{
					pollAction.SelectedIndex = 0;
				}
				else
				{
					AutoWarned = true;
				}
			}
		}

#endregion

#region Private methods

		/// <summary>
		/// Initialise the new Steam session or from saved data
		/// </summary>
		private void Init()
		{
			if (this.AuthenticatorData.GetClient().IsLoggedIn() == true)
			{
				Process();
			}
			else
			{
				ShowTab("loginTab");

				this.closeButton.Visible = false;
				this.cancelButton.Visible = true;
				this.refreshButton.Visible = false;
				this.logoutButton.Visible = false;
				this.pollPanel.Visible = false;
				this.confirmAllButton.Visible = false;
				this.cancelAllButton.Visible = false;
			}
		}

		/// <summary>
		/// Process the enrolling calling the authenticator method, checking the state and displaying appropriate tab
		/// </summary>
		private void Process(string username = null, string password = null, string captchaId = null, string captchaText = null)
		{
			do
			{
				var cursor = Cursor.Current;
				try
				{
					//Application.DoEvents();
					Cursor.Current = Cursors.WaitCursor;
					Application.DoEvents();

					var steam = this.AuthenticatorData.GetClient();

					if (steam.IsLoggedIn() == false)
					{
						if (steam.Login(username, password, captchaId, captchaText) == false)
						{
							if (steam.Error == "Incorrect Login")
							{
								WinAuthForm.ErrorDialog(this, "Invalid username or password", null, MessageBoxButtons.OK);
								return;
							}
							if (steam.Requires2FA == true)
							{
								WinAuthForm.ErrorDialog(this, "Invalid authenticator code: are you sure this is the current authenticator for your account?", null, MessageBoxButtons.OK);
								return;
							}

							if (steam.RequiresCaptcha == true)
							{
								WinAuthForm.ErrorDialog(this, (string.IsNullOrEmpty(steam.Error) == false ? steam.Error : "Please enter the captcha"), null, MessageBoxButtons.OK);

								using (var web = new WebClient())
								{
									byte[] data = web.DownloadData(steam.CaptchaUrl);

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

							if (string.IsNullOrEmpty(steam.Error) == false)
							{
								WinAuthForm.ErrorDialog(this, steam.Error, null, MessageBoxButtons.OK);
								return;
							}

							if (tabs.TabPages.ContainsKey("authTab") == true)
							{
								tabs.TabPages.RemoveByKey("authTab");
							}

							ShowTab("loginTab");
							usernameField.Focus();

							return;
						}

						AuthenticatorData.SessionData = (rememberBox.Checked == true ? steam.Session.ToString() : null);
						//AuthenticatorData.PermSession = (rememberBox.Checked == true && rememberPermBox.Checked == true);
						this.Authenticator.MarkChanged();
					}

					try
					{
						m_trades = steam.GetConfirmations();

						// save after get new trades
						if (string.IsNullOrEmpty(AuthenticatorData.SessionData) == false)
						{
							this.Authenticator.MarkChanged();
						}
					}
					catch (SteamClient.UnauthorisedSteamRequestException)
					{
						// Family view is probably on
						WinAuthForm.ErrorDialog(this, "You are not allowed to view confirmations. Have you enabled 'community-generated content' in Family View?", null, MessageBoxButtons.OK);
						return;
					}
					catch (SteamClient.InvalidSteamRequestException)
					{
						// likely a bad session so try a refresh first
						try
						{
							steam.Refresh();
							m_trades = steam.GetConfirmations();
						}
						catch (Exception)
						{
							// reset and show normal login
							steam.Clear();
							Init();
							return;
						}
					}

					Cursor.Current = cursor;

					var tab = ShowTab("tradesTab");

					tab.SuspendLayout();
					tradesContainer.Controls.Remove(this.tradePanelMaster);
					foreach (var control in tradesContainer.Controls.Cast<Control>().ToArray())
					{
						if (control is Panel)
						{
							tradesContainer.Controls.Remove(control);
						}
					}

					for (var row = 0; row < m_trades.Count; row++)
					{
						var trade = m_trades[row];

						// clone the panel
						Panel tradePanel = Clone(this.tradePanelMaster, "_" + trade.Id) as Panel;
						tradePanel.SuspendLayout();

						using (WebClient wc = new WebClient())
						{
						    byte[] imageData = null;

                            try
						    {
						        imageData = wc.DownloadData(trade.Image);
                            }
						    catch (WebException ex)
						    {
						        // ignore error 404 for missing images
                                if (((HttpWebResponse)ex.Response).StatusCode != HttpStatusCode.NotFound)
                                {
                                    throw;
                                }
                            }
							if (imageData != null && imageData.Length != 0)
							{
								using (MemoryStream ms = new MemoryStream(imageData))
								{
									PictureBox imageBox = FindControl<PictureBox>(tradePanel, "tradeImage");
									imageBox.Image = Image.FromStream(ms);
								}
							}
						}

						Label label = FindControl<Label>(tradePanel, "tradeLabel");
						label.Text = trade.Details + Environment.NewLine + trade.Traded + Environment.NewLine + trade.When;
						label.Tag = trade.Id;
						label.Click += Trade_Click;

						MetroButton tradeAcceptButton = FindControl<MetroButton>(tradePanel, "tradeAccept");
						tradeAcceptButton.Tag = trade.Id;
						tradeAcceptButton.Click += tradeAccept_Click;

						MetroButton tradeRejectButton = FindControl<MetroButton>(tradePanel, "tradeReject");
						tradeRejectButton.Tag = trade.Id;
						tradeRejectButton.Click += tradeReject_Click;

						tradePanel.Top = tradePanel.Height * row;

						tradePanel.ResumeLayout();

						tradesContainer.Controls.Add(tradePanel);
					}
					tradesEmptyLabel.Visible = (m_trades.Count == 0);

					confirmAllButton.Visible = (m_trades.Count != 0);
					cancelAllButton.Visible = (m_trades.Count != 0);

					tab.ResumeLayout();

					this.closeButton.Location = this.cancelButton.Location;
					this.closeButton.Visible = true;
					this.cancelButton.Visible = false;
					this.refreshButton.Visible = true;
					if (string.IsNullOrEmpty(AuthenticatorData.SessionData) == false)
					{
						this.logoutButton.Visible = true;

						if (steam.Session.Confirmations != null)
						{
							this.pollCheckbox.Checked = true;
							this.pollNumeric.Value = Convert.ToDecimal(steam.Session.Confirmations.Duration);
							int selected = 0;
							for (var i = 0; i < pollAction.Items.Count; i++)
							{
								var item = pollAction.Items[i] as PollerActionItem;
								if (item != null && item.Value == steam.Session.Confirmations.Action)
								{
									selected = i;

									if (steam.Session.Confirmations.Action == SteamClient.PollerAction.AutoConfirm || steam.Session.Confirmations.Action == SteamClient.PollerAction.SilentAutoConfirm)
									{
										AutoWarned = true;
									}
									break;
								}
							}
							this.pollAction.SelectedIndex = selected;
						}
						else
						{
							this.pollCheckbox.Checked = false;
							this.pollAction.SelectedIndex = 0;
						}

						this.pollPanel.Visible = true;
						this.confirmAllButton.Visible = true;
						this.cancelAllButton.Visible = true;
					}

					break;
				}
				catch (SteamClient.InvalidSteamRequestException iere)
				{
					Cursor.Current = cursor;
					if (WinAuthForm.ErrorDialog(this, "An error occurred while loading trades", iere, MessageBoxButtons.RetryCancel) != System.Windows.Forms.DialogResult.Retry)
					{
						break;
					}
				}
				finally
				{
					Cursor.Current = cursor;
				}
			} while (true);
		}

		/// <summary>
		/// Accept the trade Confirmation
		/// </summary>
		/// <param name="tradeId">Id of Confirmation</param>
#if NETFX_4
		private async Task<bool> AcceptTrade(string tradeId)
#endif
#if NETFX_3
		private bool AcceptTrade(string tradeId)
#endif
		{
			try
			{
				var trade = m_trades.Where(t => t.Id == tradeId).FirstOrDefault();
				if (trade == null)
				{
					throw new ApplicationException("Invalid trade");
				}

#if NETFX_4
				var result = await Task.Factory.StartNew<bool>((t) =>
				{
					return this.AuthenticatorData.GetClient().ConfirmTrade(((SteamClient.Confirmation)t).Id, ((SteamClient.Confirmation)t).Key, true);
				}, trade);
#endif
#if NETFX_3
				var result = this.AuthenticatorData.GetClient().ConfirmTrade(trade.Id, trade.Key, true);
#endif
				if (result == false)
				{
					throw new ApplicationException("Trade cannot be confirmed");
				}

				m_trades.Remove(trade);

				MetroButton button = FindControl<MetroButton>(tabs.SelectedTab, "tradeAccept_" + trade.Id);
				button.Visible = false;
				button = FindControl<MetroButton>(tabs.SelectedTab, "tradeReject_" + trade.Id);
				button.Visible = false;
				MetroLabel status = FindControl<MetroLabel>(tabs.SelectedTab, "tradeStatus_" + trade.Id);
				status.Text = "Confirmed";
				status.Visible = true;

				return true;
			}
			catch (InvalidTradesResponseException ex)
			{
				WinAuthForm.ErrorDialog(this, "Error trying to accept trade", ex, MessageBoxButtons.OK);
				return false;
			}
			catch (ApplicationException ex)
			{
				WinAuthForm.ErrorDialog(this, ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Reject the trade Confirmation
		/// </summary>
		/// <param name="tradeId">ID of Confirmation</param>
#if NETFX_4
		private async Task<bool> RejectTrade(string tradeId)
#endif
#if NETFX_3
		private bool RejectTrade(string tradeId)
#endif
		{
			try
			{
				var trade = m_trades.Where(t => t.Id == tradeId).FirstOrDefault();
				if (trade == null)
				{
					throw new ApplicationException("Invalid trade");
				}

#if NETFX_4
				var result = await Task.Factory.StartNew<bool>((t) =>
				{
					return this.AuthenticatorData.GetClient().ConfirmTrade(((SteamClient.Confirmation)t).Id, ((SteamClient.Confirmation)t).Key, false);
				}, trade);
#endif
#if NETFX_3
				var result = this.AuthenticatorData.GetClient().ConfirmTrade(trade.Id, trade.Key, false);
#endif
				if (result == false)
				{
					throw new ApplicationException("Trade cannot be cancelled");
				}

				m_trades.Remove(trade);

				MetroButton button = FindControl<MetroButton>(tabs.SelectedTab, "tradeAccept_" + trade.Id);
				button.Visible = false;
				button = FindControl<MetroButton>(tabs.SelectedTab, "tradeReject_" + trade.Id);
				button.Visible = false;
				MetroLabel status = FindControl<MetroLabel>(tabs.SelectedTab, "tradeStatus_" + trade.Id);
				status.Text = "Cancelled";
				status.Visible = true;

				return true;
			}
			catch (InvalidTradesResponseException ex)
			{
				WinAuthForm.ErrorDialog(this, "Error trying to reject trade", ex, MessageBoxButtons.OK);
				return false;
			}
			catch (ApplicationException ex)
			{
				WinAuthForm.ErrorDialog(this, ex.Message);
				return false;
			}
		}

		/// <summary>
		/// Find a child control of a given type and name starting with a value
		/// </summary>
		/// <typeparam name="T">Type of control</typeparam>
		/// <param name="control">parent control</param>
		/// <param name="name">first part of name</param>
		/// <returns>Control or null</returns>
		private T FindControl<T>(Control control, string name) where T : Control
		{
			if (control.Name.StartsWith(name) == true && control is T)
			{
				return (T)control;
			}

			foreach (Control child in control.Controls)
			{
				T found = FindControl<T>(child, name);
				if (found != null)
				{
					return found;
				}
			}

			return null;
		}

		/// <summary>
		/// Clone a Control and any child controls
		/// </summary>
		/// <param name="control">Control to clone</param>
		/// <param name="index">index to append to name for cloned control, e.g. "Button" -> "Button1"</param>
		/// <returns>Cloned control</returns>
		private Control Clone(Control control, string suffix)
		{
			var type = control.GetType();
			Control clone = Activator.CreateInstance(type) as Control;
			clone.Name = control.Name + (string.IsNullOrEmpty(suffix) == false ? suffix : string.Empty);

			clone.SuspendLayout();
			if (clone is ISupportInitialize)
			{
				((ISupportInitialize)(clone)).BeginInit();
			}

			// copy public properties
			foreach (var pi in type.GetProperties(System.Reflection.BindingFlags.FlattenHierarchy | System.Reflection.BindingFlags.Instance | System.Reflection.BindingFlags.Public))
			{
				if (pi.CanWrite == false || pi.CanRead == false)
				{
					continue;
				}
				if (pi.Name == "Controls" || pi.Name == "Name" || pi.Name == "WindowTarget")
				{
					continue;
				}

				object value = pi.GetValue(control, (object[])null);
				if (value != null && value.GetType().IsValueType == false)
				{
					if (value is ICloneable)
					{
						value = ((ICloneable)value).Clone();
					}
					else if (value is ISerializable)
					{
						object newvalue = JsonConvert.DeserializeObject(JsonConvert.SerializeObject(value));
						if (newvalue.GetType() == value.GetType())
						{
							value = newvalue;
						}
					}
				}
				pi.SetValue(clone, value, (object[])null);
			}

			// copy child controls
			if (control.Controls != null)
			{
				foreach (Control child in control.Controls)
				{
					clone.Controls.Add(Clone(child, suffix));
				}
			}

			clone.ResumeLayout();
			if (clone is ISupportInitialize)
			{
				((ISupportInitialize)(clone)).EndInit();
			}

			return clone;
		}

		/// <summary>
		/// Show the named tab hiding all others
		/// </summary>
		/// <param name="name">name of tab to show</param>
		/// <param name="only">hide all others, or append if false</param>
		private TabPage ShowTab(string name, bool only = true)
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
			if (name == "loginTab")
			{
				// oddity with MetroFrame controls in have to set focus away and back to field to make it stick
				this.Invoke((MethodInvoker)delegate { this.passwordField.Focus(); this.usernameField.Focus(); });
			}

			return tabs.SelectedTab;
    }

		/// <summary>
		/// Set the new polling
		/// </summary>
		private void SetPolling()
		{
			// ignore setup changes
			if (m_loaded == false || pollAction.SelectedValue == null)
			{
				return;
			}

			var steam = this.AuthenticatorData.GetClient();
			int timeInMins = (pollCheckbox.Checked == true && steam.IsLoggedIn() == true ? (int)pollNumeric.Value : 0);

			var p = new SteamClient.ConfirmationPoller
			{
				Duration = (pollCheckbox.Checked == true && steam.IsLoggedIn() == true ? (int)pollNumeric.Value : 0),
				Action = ((PollerActionItem)pollAction.SelectedValue).Value
			};
			if (p.Duration != 0)
			{
				if (steam.Session.Confirmations == null || steam.Session.Confirmations.Duration != p.Duration || steam.Session.Confirmations.Action != p.Action)
				{
					steam.PollConfirmations(p);
					AuthenticatorData.SessionData = steam.Session.ToString();
					this.Authenticator.MarkChanged();
				}
			}
			else
			{
				if (steam.Session.Confirmations != null)
				{
					steam.PollConfirmations(null);
					AuthenticatorData.SessionData = steam.Session.ToString();
					this.Authenticator.MarkChanged();
				}
			}
		}

#endregion

	}
}
