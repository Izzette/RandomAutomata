//
//  RandomAutomata/SequenceHang/SequenceHang.cs
//
//  Author:
//       Isabell Cowan <isabellcowan@gmail.com>
//
//  Copyright (c) 2015 Isabell Cowan
//
//  This program is free software: you can redistribute it and/or modify
//  it under the terms of the GNU General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  (at your option) any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
//  GNU General Public License for more details.
//
//  You should have received a copy of the GNU General Public License
//  along with this program.  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Diagnostics;
using RandomAutomata;

namespace SequenceHang
{
	public static class SequenceHang
	{
		public static void Main (string[] args)
		{
			int iterations;
			BoolsOrBytes boolsOrBytes;
			int length;
			bool overload;
			bool exit = GetParams (args, out iterations, out boolsOrBytes, out length, out overload);
			if (exit) {
				Console.WriteLine ("Exiting ...");
			}
			string methodName;
			if (BoolsOrBytes.bools == boolsOrBytes) {
				methodName = "GetNextBools";
			} else {
				methodName = "GetNextBytes";
			}
			string parenthesis;
			if (overload) {
				parenthesis = String.Format ("({0:n0})", length);
			} else {
				parenthesis = "()";
			}
			Console.WriteLine ("Excecuteing {0:n0} iterations of {1} {2}", iterations, methodName, parenthesis);
			RandomSequence randomSequence = new RandomSequence ();
			Stopwatch stopwatch = new Stopwatch ();
			for (int i = 0; i < iterations; i++) {
				if (overload) {
					if (BoolsOrBytes.bools == boolsOrBytes) {
						stopwatch.Start ();
						randomSequence.GetNextBools (length);
					} else {
						stopwatch.Start ();
						randomSequence.GetNextBytes (length);
					}
				} else {
					if (BoolsOrBytes.bools == boolsOrBytes) {
						stopwatch.Start ();
						randomSequence.GetNextBools ();
					} else {
						stopwatch.Start ();
						randomSequence.GetNextBytes ();
					}
				}
				stopwatch.Stop ();
			}
			Console.WriteLine ("ElapsedTime: {0:n0} ms", stopwatch.ElapsedMilliseconds);
		}

		private enum BoolsOrBytes
		{
			bools,
			bytes
		}

		private static bool GetParams (string[] args, out int iterations, out BoolsOrBytes boolsOrBytes, out int length, out bool overload)
		{

			// gotta get passed the compiler
			iterations = 0;
			boolsOrBytes = BoolsOrBytes.bools;
			length = 0;
			overload = false;

			try {
				iterations = Convert.ToInt32 (args [0]);
				if (3 <= args.Length) {
					length = Convert.ToInt32 (args [2]);
					overload = true;
				} else {
					overload = false;
				}
				if ("bools" == args [1]) {
					boolsOrBytes = BoolsOrBytes.bools;
				} else if ("bytes" == args [1]) {
					boolsOrBytes = BoolsOrBytes.bytes;
				} else {
					throw new FormatException ();
				}
			} catch (IndexOutOfRangeException) {
				Console.WriteLine ("Requires atleast two arguments");
				return true;
			} catch (FormatException) {
				Console.WriteLine ("Input not of right format");
				return true;
			}
			return false;
		
		}

	}
}

