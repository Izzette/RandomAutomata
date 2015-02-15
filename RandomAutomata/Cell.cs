using System;

namespace RandomAutomata
{

	class Cell
	{

		public static Cell[] Build (byte[] seed)
		{
			Cell[] cells = new Cell[seed.Length];
			for (int s = 0; s < seed.Length; s++) {
				cells [s] = new Cell (seed [s]);
			}
			for (int c = 0; c < cells.Length; c++) {
				for (int n = 0; n < neighbourhoodLength; n++) {
					int neighbourIndex = (c + cells.Length - halfLength + n) % cells.Length;
					cells [c].neighbours [n] = cells [neighbourIndex];
				}
			}
			return cells;
		}

		public byte State {
			get { return this.state; }
			set { this.state = value; }
		}

		public int GetNeighbourhood ()
		{
			byte neighbourhood = 0;
			foreach (Cell neighbour in this.neighbours) {
				neighbourhood <<= 1;
				neighbourhood |= neighbour.state;
			}
			return (int)(neighbourhood);
		}

		private Cell (byte state)
		{
			this.state = state;
			this.neighbours = new Cell [neighbourhoodLength];
		}

		private byte state;
		private Cell[] neighbours;

		private const int neighbourhoodLength = 5;
		private const int halfLength = neighbourhoodLength / 2;

	}

}

