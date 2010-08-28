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
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Runtime.InteropServices;

#if NUNIT
using NUnit.Framework;
#endif

namespace WindowsAuthenticator
{
	/// <summary>
	/// Class to send an email, directly to SMTP server of user's mailhost if we can by getting the MX records for the domain.
	/// </summary>
	public class MailSender
	{
		/// <summary>
		/// Options for DnsQuery API call
		/// </summary>
		private enum QueryOptions
		{
			DNS_QUERY_BYPASS_CACHE = 8
		}

		/// <summary>
		/// Options for records for DnsQuery
		/// </summary>
		private enum QueryTypes
		{
			DNS_TYPE_MX = 15
		}

		/// <summary>
		/// DNS MX record layout
		/// </summary>
		[StructLayout(LayoutKind.Sequential)]
		private struct MX_Record
		{
			public IntPtr pNext;
			public string pName;
			public short wType;
			public short wDataLength;
			public int flags;
			public int dwTtl;
			public int dwReserved;
			public IntPtr pNameExchange;
			public short wPreference;
			public short Pad;
		}

		/// <summary>
		/// Call to WinAPI DnsQuery
		/// </summary>
		/// <param name="pszName"></param>
		/// <param name="wType"></param>
		/// <param name="options"></param>
		/// <param name="aipServers"></param>
		/// <param name="ppQueryResults"></param>
		/// <param name="pReserved"></param>
		/// <returns></returns>
		[DllImport("dnsapi", EntryPoint = "DnsQuery_W", CharSet = CharSet.Unicode, SetLastError = true, ExactSpelling = true)]
		private static extern int DnsQuery([MarshalAs(UnmanagedType.VBByRefStr)]ref string pszName, QueryTypes wType, QueryOptions options, int aipServers, ref IntPtr ppQueryResults, int pReserved);

		/// <summary>
		/// Call to cleanup buffer returned from DnsQuery
		/// </summary>
		/// <param name="pRecordList"></param>
		/// <param name="FreeType"></param>
		[DllImport("dnsapi", CharSet = CharSet.Auto, SetLastError = true)]
		private static extern void DnsRecordListFree(IntPtr pRecordList, int FreeType);

		/// <summary>
		/// Get an array of MX host names for a given domain
		/// </summary>
		/// <param name="domain">domain name</param>
		/// <returns>array of host names</returns>
		public static string[] MXRecords(string domain)
		{
			IntPtr queryResult = IntPtr.Zero;
			try
			{
				// call DNS query
				int result = DnsQuery(ref domain, QueryTypes.DNS_TYPE_MX, QueryOptions.DNS_QUERY_BYPASS_CACHE, 0, ref queryResult, 0);
				if (result != 0)
				{
					return null;
				}

				// loop through results and extract host names
				List<string> mxrecords = new List<string>();
				IntPtr resultPtr = queryResult;
				while (resultPtr != IntPtr.Zero)
				{
					MX_Record mx = (MX_Record)Marshal.PtrToStructure(resultPtr, typeof(MX_Record));
					if (mx.wType == 15)
					{
						mxrecords.Add(Marshal.PtrToStringAuto(mx.pNameExchange));
					}

					resultPtr = mx.pNext;
				}

				return mxrecords.ToArray();
			}
			finally
			{
				// clean up buffer
				if (queryResult != IntPtr.Zero)
				{
					DnsRecordListFree(queryResult, 0);
				}
			}
		}

		/// <summary>
		/// Preferred Smtp Server
		/// </summary>
		public string SmtpServer { get; set; }

		/// <summary>
		/// Preferred Smtp Port
		/// </summary>
		public int SmtpPort { get; set; }

		/// <summary>
		/// Smtp username
		/// </summary>
		public string SmtpUsername { get; set; }

		/// <summary>
		/// Smtp password
		/// </summary>
		public string SmtpPasword { get; set; }

		/// <summary>
		/// Smtp SSL flag
		/// </summary>
		public bool SmtpSSL { get; set; }

		/// <summary>
		/// Create a blank MailServer object
		/// </summary>
		public MailSender()
		{
		}

		/// <summary>
		/// Create a new MailSender object for a specific smtp server
		/// </summary>
		/// <param name="smtpServer">smtp server</param>
		/// <param name="port">smtp server port e.g. 25</param>
		public MailSender(string smtpServer, int port)
		{
			SmtpServer = smtpServer;
			SmtpPort = port;
		}

		/// <summary>
		/// Create a new MailSender object for a specific smtp server
		/// </summary>
		/// <param name="smtpServer">smtp server</param>
		/// <param name="port">smtp server port e.g. 25</param>
		/// <param name="username">username for smtp auth</param>
		/// <param name="password">password for smtp auth</param>
		public MailSender(string smtpServer, int port, string username, string password)
		{
			SmtpServer = smtpServer;
			SmtpPort = port;
			SmtpUsername = username;
			SmtpPasword = password;
		}

		/// <summary>
		/// Send an email either using the objects SmtpServer or directly to the MX records
		/// </summary>
		/// <param name="from">from addrss</param>
		/// <param name="to">to address</param>
		/// <param name="subject">sibejct of email</param>
		/// <param name="body">body of email</param>
		/// <param name="attachfiles">optional array of attachment files</param>
		public void Send(MailAddress from, MailAddress to, string subject, string body, string[] attachfiles)
		{
			// build a new message
			using (MailMessage message = new MailMessage())
			{
				message.From = from;
				message.To.Add(to);
				message.Subject = subject;

				// add any attachments
				if (attachfiles != null)
				{
					foreach (string attachfile in attachfiles)
					{
						Attachment attach = new Attachment(attachfile);
						message.Attachments.Add(attach);
					}
				}

				// get either the supplied SmtpHost or the MX records
				string[] mxs;
				SmtpClient smtpClient = new SmtpClient();
				smtpClient.Timeout = 20000;
				if (string.IsNullOrEmpty(SmtpServer) == false)
				{
					mxs = new string[1] { SmtpServer };
					//smtpClient.Host = SmtpServer;
					smtpClient.Port = SmtpPort;
					smtpClient.EnableSsl = SmtpSSL;
					if (string.IsNullOrEmpty(SmtpUsername) == false)
					{
						smtpClient.UseDefaultCredentials = false;
						smtpClient.Credentials = new NetworkCredential(SmtpUsername, SmtpPasword);
					}
				}
				else
				{
					// can we get DNS?
					if (Environment.OSVersion.Platform != PlatformID.Win32NT)
					{
						throw new MailSenderException("You operating system does not support DNS lookups. You must specify an SMTP Server");
					}

					// get the MX records for the domain
					mxs = MXRecords(to.Host);
					if (mxs == null || mxs.Length == 0)
					{
						throw new MailSenderNoOutboundServerException();
					}
				}

				// go through each mail host (or MX) and try and send email
				Exception firstException = null;
				foreach (string mx in mxs)
				{
					try
					{
						smtpClient.Host = mx;
						smtpClient.Send(message);
						firstException = null;
						break;
					}
					catch (Exception ex)
					{
						if (firstException == null)
						{
							firstException = ex;
						}
					}
				}
				if (firstException != null)
				{
					throw new MailSenderException(firstException.Message, firstException);
				}
			}
		}
	}

	/// <summary>
	/// Exceptions through from mail sender
	/// </summary>
	public class MailSenderException : ApplicationException
	{
		public MailSenderException(string msg) : base(msg) { }

		public MailSenderException(string msg, Exception ex) : base(msg, ex) { }
	}

	/// <summary>
	/// Exception thorwn when no MX records
	/// </summary>
	public class MailSenderNoOutboundServerException : MailSenderException
	{
		public MailSenderNoOutboundServerException() : base("No outbound SMTP server") { }
	}

#if NUNIT
	[TestFixture]
	public class MailSender_Test
	{
		public MailSender_Test()
		{
		}

		[Test]
		public void MXLookup()
		{
			string[] mx = MailSender.MXRecords("gmail.com");
			Assert.NotNull(mx);
			Assert.IsTrue(mx.Length != 0);
			Assert.IsTrue(mx[0].EndsWith(".google.com"));
			foreach (string s in mx)
			{
				Console.Out.WriteLine(s);
			}

			mx = MailSender.MXRecords("thisiscompletelymadeupandshouldntwork.com");
			Assert.IsNull(mx);
		}

		[Test]
		public void Send()
		{
			MailSender ms = new MailSender();
			ms.Send(new MailAddress("test@test.com"), new MailAddress("winauth@gmail.com"), "Test Send 1", "this is a test", null);
		}
	}
#endif
}
