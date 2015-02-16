// 
// RandomAutomata/RandomAutomata/RandomSequence.cs
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
using System.Threading.Tasks;

namespace RandomAutomata
{
	public class RandomSequence
	{
		
		public RandomSequence ()
		{
			ulong seedNumber = GetRandomSeed ();
			this.Init (seedNumber);
		}

		public RandomSequence (ulong seedNumber)
		{
			this.Init (seedNumber);
		}

		public ulong SeedNumber {
			get { return this.seedNumber; }
		}

		public bool[] GetNextBools ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			bool[] nextBools = new bool[automataLength];
			Parallel.For (0, nextBools.Length, i => {
				nextBools [i] = (0 < states [i]);
			});
			return nextBools;
		}

		public bool[] GetNextBools (int length)
		{
			bool[] nextBools = new bool[length];
			int quotient = length / BoolsLength;
			if (0 != quotient) {
				for (int i = 0; i < quotient; i++) {
					this.Skip (TimeSpace);
					byte[] states = this.automata.States;
					int startIndex = BoolsLength * i;
					Parallel.For (0, BoolsLength, j => {
						int index = startIndex + j;
						nextBools [index] = (0 < states [j]);
					});
				}
			}
			int remainder = length % BoolsLength;
			if (0 != remainder) {
				this.Skip (TimeSpace);
				byte[] states = this.automata.States;
				int startIndex = BoolsLength * quotient;
				Parallel.For (0, remainder, i => {
					int index = startIndex + i;
					nextBools [index] = (0 < states [i]);
				});
			}
			return nextBools;
		}

		public byte[] GetNextBytes ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			byte[] nextBytes = new byte[BytesLength];
			Parallel.For (0, nextBytes.Length, i => {
				int startIndex = lengthOfByte * i;
				for (int ie = 0; ie < lengthOfByte; ie++) {
					nextBytes [i] <<= 1;
					int statesIndex = startIndex + ie;
					nextBytes [i] |= states [statesIndex];
				}
			});
			return nextBytes;
		}

		public byte[] GetNextBytes (int length)
		{
			byte[] nextBytes = new byte[length];
			int quotient = length / BytesLength;
			if (0 != quotient) {
				for (int i = 0; i < quotient; i++) {
					this.Skip (TimeSpace);
					byte[] states = this.automata.States;
					int startBytesIndex = i * BytesLength;
					Parallel.For (0, BytesLength, j => {
						int startStatesIndex = j * lengthOfByte;
						int bytesIndex = startBytesIndex + j;
						for (int je = 0; je < lengthOfByte; je++) {
							nextBytes [bytesIndex] <<= 1;
							int statesIndex = startStatesIndex + je;
							nextBytes [bytesIndex] |= states [statesIndex];
						}
					});
				}
			}
			int remainder = length % BytesLength;
			if (0 != remainder) {
				this.Skip (TimeSpace);
				byte[] states = this.automata.States;
				int startBytesIndex = BytesLength * quotient;
				Parallel.For (0, remainder, i => {
					int startStatesIndex = i * lengthOfByte;
					int bytesIndex = startBytesIndex + i;
					for (int ie = 0; ie < lengthOfByte; ie++) {
						nextBytes [bytesIndex] <<= 1;
						int statesIndex = startStatesIndex + ie;
						nextBytes [bytesIndex] |= states [statesIndex];
					}
				});
			}
			return nextBytes;
		}

		public ulong GetNextULong ()
		{
			this.Skip (TimeSpace);
			byte[] states = this.automata.States;
			ulong nextULong = 0;
			for (int i = 0; i < automataLength; i++) {
				nextULong <<= 1;
				int statesIndex = (i * lengthOfByte) + i;
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
				int statesIndex = (i * lengthOfByte) + i;
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

		public static int BoolsLength {
			get { return automataLength; }
		}

		public static int BytesLength {
			get { return automataLength / lengthOfByte; }
		}

		private ulong seedNumber;
		private Automata automata;

		private void Init (ulong seedNumber)
		{
			this.seedNumber = seedNumber;
			byte[] seedArray = GetSeedArray (seedNumber);
			this.automata = new Automata (seedArray);
		}

		private byte[] GetSeedArray (ulong seedNumber)
		{
			byte[] seedArray = new byte[automataLength];
			for (int i = 0; i < automataLength; i++) {
				seedArray [i] = (byte)(seedNumber % 2);
				seedNumber >>= 1;
			}
			return seedArray;
		}

		private const int automataLength = 64;
		private const int TimeSpace = 4;
		private const int lengthOfByte = 8;

		private static ulong GetRandomSeed ()
		{
			Random random = new Random ();
			ulong seed = 0;
			for (int i = 0; i < 2; i++) {
				seed <<= 32;
				seed |= (ulong)(random.Next ());
				seed <<= 1;
				seed |= (ulong)(random.Next (0, 2));
			}
			return seed;
		}

	}
}

