// 
// RandomAutomata/RandomAutomata/Rule.cs
// 
// Author:
//     Isabell Cowan <isabellcowan@gmail.com>
//
// Copyright (c) 2015 Isabell Cowan
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
//
// You should have received a copy of the GNU General Public License
// along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;

namespace RandomAutomata
{

	public class Rule
	{

		public Rule ()
		{
			this.states = new byte[ruleLength];
			ulong workingRuleNumber = ruleNumber;
			for (int i = 0; i < this.states.Length; i++) {
				this.states [i] = (byte)(workingRuleNumber % 2);
				workingRuleNumber >>= 1;
			}
		}

		public byte GetNextState (int neighbourhood)
		{
			return this.states [neighbourhood];
		}

		private byte[] states;

		private const ulong ruleNumber = 1436965290;
		private const int ruleLength = 32;
	
	}

}

