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
using System.Text;
using System.Xml;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class for main authenticator form
	/// </summary>
	public partial class EnrollForm : TransparentForm
	{
		#region Data Members

		/// <summary>
		/// Region string for US
		/// </summary>
		private const string REGION_US = "US";

		/// <summary>
		/// Regions string for EU
		/// </summary>
		private const string REGION_EU = "EU";

		/// <summary>
		/// Path to the BMA if we had one
		/// </summary>
		private const string BMA_PATH = "\\JavaFX\\blizzard_mobile_*.*";

		/// <summary>
		/// Newly enrolled authenticator
		/// </summary>
		public Authenticator Authenticator { get; set; }

		#endregion

		#region Constructors

		/// <summary>
		/// Create a new form
		/// </summary>
		public EnrollForm()
		{
			InitializeComponent();
		}

		#endregion

		#region Properties

		/// <summary>
		/// Flag to set import state
		/// </summary>
		public bool ImportBMA { get; set; }

		#endregion

		#region Methods

		/// <summary>
		/// Set the status field whilst enrolling
		/// </summary>
		/// <param name="status">status text</param>
		public void UpdateEnrollStatus(string status)
		{
			enrollLabel.Text = status;
		}

		/// <summary>
		/// Update enrolling status bar
		/// </summary>
		public void UpdateEnrollProgressBar()
		{
			// get the enroller object
			Enroller enroller = enrollProgressBar.Tag as Enroller;
			if (enroller != null)
			{
				// just update the bar with number of seconds since we started, wrap if neccessary
				DateTime started = enroller.Started;
				TimeSpan diff = DateTime.Now.Subtract(started);
				int value = (diff.TotalSeconds >= 0 ? (int)diff.TotalSeconds % (enrollProgressBar.Maximum + 1) : 0);
				enrollProgressBar.Value = value;
				enrollProgressBar.Visible = true;
			}
		}

		/// <summary>
		/// Begin the enrolling
		/// </summary>
		/// <param name="region">region to enroll</param>
		private void BeginEnroll(string region)
		{
			// set labels
			enrollLabel.Text = "Enrolling...";
			enrollLabel.Visible = true;
			enrollProgressBar.Visible = true;

			// create enroller and hook events
			Enroller enroller = new Enroller(region);
			enroller.StatusChanged += new Enroller.StatusChangedHandler(enroller_StatusChanged);
			enroller.Completed += new Enroller.CompleteHandler(enroller_Completed);
			enrollProgressBar.Tag = enroller;
		}

		/// <summary>
		/// Delegate to update status since enroller is on different thread
		/// </summary>
		/// <param name="status"></param>
		public delegate void UpdateEnrollStatusCallback(string status);

		/// <summary>
		/// Enroller status changed event. On different thread so we must invoke.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void enroller_StatusChanged(object sender, StatusChangedEventArgs args)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new UpdateEnrollStatusCallback(UpdateEnrollStatus), new object[] { args.Status });
			}
			else
			{
			  UpdateEnrollStatus(args.Status);
			}
		}

		/// <summary>
		/// Delegate for EndEnroll method
		/// </summary>
		/// <param name="auth"></param>
		public delegate void EndEnrollCallback(Authenticator auth);

		/// <summary>
		/// Enroller Completed veenta handler, which is on different thread so we Invoke.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		void enroller_Completed(object sender, CompletedEventArgs args)
		{
			if (this.InvokeRequired == true)
			{
				this.Invoke(new EndEnrollCallback(EndEnroll), new object[] { args.Authenticator });
			}
			else
			{
				EndEnroll(args.Authenticator);
			}
		}

		/// <summary>
		/// End the enrolling in the GUI
		/// </summary>
		/// <param name="auth"></param>
		public void EndEnroll(Authenticator auth)
		{
			// clear status
			UpdateEnrollStatus(string.Empty);
			enrollProgressBar.Tag = null;

			// if we have an authenticator, set it
			if (auth != null)
			{
				UpdateEnrollStatus("Saving authenticator...");

				// set this new authenticator data
				Authenticator = auth;

				DialogResult = DialogResult.OK;
			}
			else
			{
				DialogResult = DialogResult.Cancel;
			}

			this.Close();
		}

		#endregion

		#region Form Events

		/// <summary>
		/// Load the form. Default settings and read authenticator.
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void EnrollForm_Load(object sender, EventArgs e)
		{
			// check for BMA on device. If there we show an additonal option to import it.
			bool haveBMA = false;
			try
			{
				if (Directory.GetFiles(Path.GetDirectoryName(BMA_PATH), Path.GetFileName(BMA_PATH)).Length != 0)
				{
					haveBMA = true;
				}
			}
			catch (Exception) { }
			rbImportBMA.Visible = haveBMA;
			// set the panel size to include the Import or not
			enrollRegionPanel.Width = (haveBMA == true ? rbImportBMA.Width : rbRegionEU.Width);
			enrollRegionPanel.Height = (haveBMA == true ? rbImportBMA.Top + rbImportBMA.Height : rbRegionEU.Top + rbRegionEU.Height);

			// set status
			ImportBMA = false;
			UpdateEnrollStatus(string.Empty);
			rbRegionUS.Focus();
		}

		/// <summary>
		/// Hande the paint to hide the default OK and X button that WM creates
		/// </summary>
		/// <param name="e"></param>
		protected override void OnPaint(PaintEventArgs e)
		{
			WinAPI.HideDoneButton(this.Handle);
			WinAPI.HideXButton(this.Handle);
 			base.OnPaint(e);
		}

		/// <summary>
		/// Click the Register option to start enrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void registerMenuItem_Click(object sender, EventArgs e)
		{
			if (rbImportBMA.Checked == true)
			{
				ImportBMA = true;
				DialogResult = DialogResult.OK;
				this.Close();
			}
			else
			{
				string region = (rbRegionUS.Checked == true ? REGION_US : REGION_EU);
				UpdateEnrollStatus("Registering...");
				BeginEnroll(region);
			}
		}

		/// <summary>
		/// Click the exit whilst enrolling
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enrollExitMenuItem_Click(object sender, EventArgs e)
		{
			this.DialogResult = DialogResult.Cancel;
			this.Close();
		}

		/// <summary>
		/// Tick for timer
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="e"></param>
		private void enrollTimer_Tick(object sender, EventArgs e)
		{
			// if we are enrolling, show progress bar
			if (enrollProgressBar.Tag != null)
			{
				UpdateEnrollProgressBar();
			}
		}

		#endregion

	}

}
