using System;
using System.Threading.Tasks;

namespace RandomAutomata
{
	public class RandomSequence
	{
		public RandomSequence (ulong seedNumber)
		{
			this.seedNumber = seedNumber;
			byte[] seedArray = new byte[automataLength];
			for (int i = 0; i < automataLength; i++) {
				seedArray [i] = (byte)(seedNumber % 2);
				seedNumber >>= 1;
			}
			this.automata = new Automata (seedArray);
		}

		public ulong SeedNumber {
			get { return this.seedNumber; }
		}

		public int BoolsLength {
			get { return automataLength; }
		}

		public bool[] GetNextBools ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			bool[] nextBools = new bool[automataLength];
			Parallel.For (0, nextBools.Length, i => {
				if (0 < states [i]) {
					nextBools [i] = true;
				} else {
					nextBools [i] = false;
				}
			});
			return nextBools;
		}

		public int BytesLength {
			get { return automataLength / byteLength; }
		}

		public byte[] GetNextBytes ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			byte[] nextBytes = new byte[automataLength / byteLength];
			Parallel.For (0, nextBytes.Length, i => {
				for (int ie = 0; ie < byteLength; ie++) {
					nextBytes [i] <<= 1;
					int statesIndex = (i * byteLength) + ie;
					nextBytes [i] |= states [statesIndex];
				}
			});
			return nextBytes;
		}

		public ulong GetNextULong ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			ulong nextULong = 0;
			for (int i = 0; i < automataLength; i++) {
				nextULong <<= 1;
				int statesIndex = (i * byteLength) + i;
				nextULong += (ulong)states [statesIndex];
			}
			return nextULong;
		}

		public long GetNextLong ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			long nextLong = 0;
			for (int i = 0; i < automataLength; i++) {
				nextLong <<= 1;
				int statesIndex = (i * byteLength) + i;
				nextLong += (long)states [statesIndex];
			}
			return nextLong;
		}

		public void Skip (int steps)
		{
			for (int i = 0; i < steps; i++) {
				this.automata.Evolve ();
			}
		}

		private ulong seedNumber;
		private Automata automata;

		private const int automataLength = 64;
		private const int TimeSpace = 4;
		private const int byteLength = 8;

	}
}

