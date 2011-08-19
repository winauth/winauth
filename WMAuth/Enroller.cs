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
using System.IO;
using System.Text;
using System.Threading;
using System.Xml;
using System.Windows.Forms;

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class that performs the enrolling, which creates a new thread to allow the 
	/// status and progress updates to happen in the GUI thread
	/// </summary>
	public class Enroller
	{
		/// <summary>
		/// Region to enroll
		/// </summary>
		public string Region { get; set; }

		/// <summary>
		/// When we started the enroll
		/// </summary>
		public DateTime Started {get; set;}

		/// <summary>
		/// Delegate for StatusChanged event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void StatusChangedHandler(object sender, StatusChangedEventArgs args);

		/// <summary>
		/// Delegate for Completed event
		/// </summary>
		/// <param name="sender"></param>
		/// <param name="args"></param>
		public delegate void CompleteHandler(object sender, CompletedEventArgs args);

		/// <summary>
		/// StatusChanged event
		/// </summary>
		public event StatusChangedHandler StatusChanged;

		/// <summary>
		/// Completed Event
		/// </summary>
		public event CompleteHandler Completed;

		/// <summary>
		/// Create a new Enroller object
		/// </summary>
		/// <param name="region">region to enroll "US" or "EU"</param>
		public Enroller(string region)
		{
			// set started time
			Started = DateTime.Now;

			// set region
			Region = region;

			// create a new thread and launch to do enrolling
			Thread thread = new Thread(new System.Threading.ThreadStart(EnrollerStart));
			//m_thread.Priority = System.Threading.ThreadPriority.BelowNormal;
			thread.IsBackground = true;
			thread.Start();
		}

		/// <summary>
		/// Thread entry point to do enrolling
		/// </summary>
		public void EnrollerStart()
		{
			// build a default authenticator
			Authenticator auth = null;

			// try and register a new authenticator
			bool retry;
			do
			{
				retry = false;

				// set status
				UpdateStatus("Connecting to Battle.net...");

				try
				{
#if DEBUG
					// this is just a test to save hitting servers each time
					if (MessageBox.Show("Simulate Battle.net?",
								"Debug Version",
								MessageBoxButtons.YesNo,
								MessageBoxIcon.Question,
								MessageBoxDefaultButton.Button1) == DialogResult.Yes)
					{
						XmlDocument testxml = new XmlDocument();
						testxml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8"" standalone=""yes""?><winauth version=""0.7""><servertimediff>-100619</servertimediff><region>US</region><secretdata>00C814C868632E212324F2A366C2F72F418226441B2DB943D9E7D8EBC70F1CE78A3D6985B059447AF0C8894D27FA54494936652976BFB85DA9</secretdata></winauth>");
						auth = new Authenticator();
						auth.Load(testxml.DocumentElement, null);
						System.Threading.Thread.Sleep(3000);
						UpdateStatus("Reading from Battle.net...");
						System.Threading.Thread.Sleep(2000);
					}
					else
					{
						auth = new Authenticator();
						auth.Enroll();
					}
#else
					// enroll the authenticator
					auth = new Authenticator();
					auth.Enroll();
#endif
				}
				catch (InvalidEnrollResponseException ex)
				{
					auth = null;
					UpdateStatus("Failed to connect to Battle.net...");
					if (MessageBox.Show("Cannot connect to Battle.net servers: " + ex.Message,
								WMAuth.APPLICATION_NAME,
								MessageBoxButtons.RetryCancel,
								MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Retry)
					{
						break;
					}
					Started = DateTime.Now;
					retry = true;
				}
				catch (Exception ex)
				{
					auth = null;
					UpdateStatus("Failed to connect to Battle.net...");
					if (MessageBox.Show("Cannot enroll with Battle.net servers: " + ex.Message,
								WMAuth.APPLICATION_NAME,
								MessageBoxButtons.RetryCancel,
								MessageBoxIcon.Exclamation, MessageBoxDefaultButton.Button1) != DialogResult.Retry)
					{
						break;
					}
					Started = DateTime.Now;
					retry = true;
				}
			} while (retry == true);

			// fire the Completed event
			Completed(this, new CompletedEventArgs(auth));
		}

		/// <summary>
		/// Update the status by firing event
		/// </summary>
		/// <param name="status">status text</param>
		private void UpdateStatus(string status)
		{
			// send StatusChanged event
			StatusChanged(this, new StatusChangedEventArgs(status));
		}
	}

	/// <summary>
	/// Class for StatusChangedEvent
	/// </summary>
	public class StatusChangedEventArgs : EventArgs
	{
		/// <summary>
		/// Status string
		/// </summary>
		public string Status { get; set; }

		/// <summary>
		/// Create a new event args object
		/// </summary>
		/// <param name="status">status string</param>
		public StatusChangedEventArgs(string status)
		{
			Status = status;
		}
	}

	/// <summary>
	/// Class for CompletedEvent
	/// </summary>
	public class CompletedEventArgs : EventArgs
	{
		/// <summary>
		/// Enrolled authenticator
		/// </summary>
		public Authenticator Authenticator { get; set; }

		/// <summary>
		/// Create a new event args obejct
		/// </summary>
		/// <param name="auth"></param>
		public CompletedEventArgs(Authenticator auth)
		{
			Authenticator = auth;
		}
	}

}
