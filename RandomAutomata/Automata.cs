using System;
using System.Threading.Tasks;

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
			get { return (byte[])this.states.Clone (); }
		}

		public void Evolve ()
		{
			Parallel.For (0, this.Length, i => {
				int neighbourhood = this.cells [i].GetNeighbourhood ();
				this.states [i] = rule.GetNextState (neighbourhood);
			});
			Parallel.For (0, this.Length, i => {
				this.cells [i].State = this.states [i];
			});
		}

		private Cell[] cells;
		private byte[] states;
		private Rule rule;
	
	}

}

