/*
 * Copyright (C) 2013 Colin Mackie.
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
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading;

namespace WinAuth
{
	/// <summary>
	/// Class instance that creates a global mutex so we can ensure only one copy of application
	/// runs at a time.
	/// 
	/// http://stackoverflow.com/questions/229565/what-is-a-good-pattern-for-using-a-global-mutex-in-c/229567
	/// 
	/// </summary>
	public class SingleGlobalInstance : IDisposable
	{
		public bool HasHandle { get; set; }

		private Mutex _mutex;

		private void InitMutex()
		{
			HasHandle = false;

			string appGuid = ((GuidAttribute)Assembly.GetExecutingAssembly().GetCustomAttributes(typeof(GuidAttribute), false).GetValue(0)).Value.ToString();
			string userGuid = WindowsIdentity.GetCurrent().User.Value;
			string mutexId = string.Format("Global\\{{{0}}}-{{{1}}}", userGuid, appGuid);
			_mutex = new Mutex(false, mutexId);

			var allowEveryoneRule = new MutexAccessRule(new SecurityIdentifier(WellKnownSidType.WorldSid, null), MutexRights.FullControl, AccessControlType.Allow);
			var securitySettings = new MutexSecurity();
			securitySettings.AddAccessRule(allowEveryoneRule);
			_mutex.SetAccessControl(securitySettings);
		}

		public SingleGlobalInstance(int timeOut = Timeout.Infinite)
		{
			InitMutex();
			try
			{
				HasHandle = _mutex.WaitOne(timeOut, false);
				if (HasHandle == false)
				{
					throw new TimeoutException("Timeout waiting for exclusive access on SingleInstance");
				}
			}
			catch (AbandonedMutexException)
			{
				HasHandle = true;
			}
		}

		public void Dispose()
		{
			if (_mutex != null)
			{
				if (HasHandle)
				{
					_mutex.ReleaseMutex();
				}
				_mutex.Dispose();
			}
		}
	}
}
