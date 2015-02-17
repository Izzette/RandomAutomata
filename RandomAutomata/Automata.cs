// 
// RandomAutomata/RandomAutomata/Automata.cs
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


namespace RandomAutomata
{

	class Automata
	{

		public Automata (byte[] seed)
		{
			this.cells = Cell.Build (seed);
			this.states = new byte[this.cells.Length];
			this.rule = new Rule ();
		}

		public int Length {
			get { return this.cells.Length; }
		}
		public byte[] States {
			get { return this.states; }
		}

		public void Evolve ()
		{
			for (int i = 0; i < this.Length; i++) {
				int neighbourhood = this.cells [i].GetNeighbourhood ();
				this.states [i] = rule.GetNextState (neighbourhood);
			}
			for (int i = 0; i < this.Length; i++) {
				this.cells [i].State = this.states [i];
			}
		}

		private Cell[] cells;
		private byte[] states;
		private readonly Rule rule;
	
	}

}

