using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WinAuth
{
	public class RegisteredAuthenticator
	{
		public enum AuthenticatorTypes
		{
			None = 0,
			BattleNet,
			RFC6238_TIME,
			RFC6238_COUNTER
		}

		public string Name;
		public AuthenticatorTypes AuthenticatorType;
		public string Icon;
		public List<RegisteredAuthenticator> Children = new List<RegisteredAuthenticator>();
	}
}
