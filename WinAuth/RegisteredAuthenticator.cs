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
using System.Text;

namespace WinAuth
{
	public class RegisteredAuthenticator
	{
		public enum AuthenticatorTypes
		{
			None = 0,
			BattleNet,
			Google,
			GuildWars,
			Trion,
			Microsoft,
			RFC6238_TIME,
			RFC6238_COUNTER
		}

		public string Name;
		public AuthenticatorTypes AuthenticatorType;
		public string Icon;
		public List<RegisteredAuthenticator> Children = new List<RegisteredAuthenticator>();
	}
}
