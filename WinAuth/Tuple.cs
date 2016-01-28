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
#if NETFX_3
	/// <summary>
	/// Our own Tuple class for .Net 3.5
	/// </summary>
	/// <typeparam name="T1">first type</typeparam>
	/// <typeparam name="T2">second type</typeparam>
	public class Tuple<T1, T2>
	{
		public T1 First { get; private set; }
		public T2 Second { get; private set; }

		public T1 Item1
		{
			get
			{
				return this.First;
			}
			private set
			{
				this.First = value;
			}
		}

		public T2 Item2
		{
			get
			{
				return this.Second;
			}
			private set
			{
				this.Second = value;
			}
		}

		internal Tuple(T1 first, T2 second)
		{
			First = first;
			Second = second;
		}
	}

	public static class Tuple
	{
		public static Tuple<T1, T2> New<T1, T2>(T1 first, T2 second)
		{
			var tuple = new Tuple<T1, T2>(first, second);
			return tuple;
		}
	}
#endif
}
