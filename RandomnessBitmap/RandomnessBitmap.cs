// 
// RandomAutomata/RandomnessBitmap/RandomnessBitmap.cs
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
using System.Drawing;
using System.Drawing.Imaging;
using System.Threading.Tasks;
using System.Diagnostics;
using System.IO;
using RandomAutomata;

namespace RandomnessBitmap
{

	public static class RandomnessBitmap
	{

		public static void Main (string[] args)
		{
			int width;
			int height;
			string path;
			bool color;
			bool exit = CheckArgs (args, out width, out height, out path, out color);
			if (exit) {
				return;
			}
			Console.ForegroundColor = ConsoleColor.Red;
			Console.WriteLine ("Testing RandomAutomata: Creating randomness bitmap ...");
			Console.ResetColor ();
			long totalElapsedTime = 0;
			long elapsedRandomSequenceTime;
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			RandomSequence randomSequence = ConstructRandomSequence (out elapsedRandomSequenceTime);
			totalElapsedTime += elapsedRandomSequenceTime;
			long elapsedBitmapTime;
			Bitmap bitmap = GetBitmap (randomSequence, width, height, color, out elapsedBitmapTime);
			totalElapsedTime += elapsedBitmapTime;
			long elapsedSaveTime;
			SaveBitmap (bitmap, path, out elapsedSaveTime);
			totalElapsedTime += elapsedSaveTime;
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Successfully created randomness bitmap!");
			Console.ResetColor ();
			Console.WriteLine ("Total elapsed time: {0:n0} ms", totalElapsedTime);
		}

		private static bool CheckArgs (string[] args, out int width, out int height, out string path, out bool color)
		{
			try {
				width = Convert.ToInt32 (args [0]);
				height = Convert.ToInt32 (args [1]);
				path = args [2];
				if ("color" == args [3]) {
					color = true;
				} else if ("bw" == args [3]) {
					color = false;
				} else {
					throw new FormatException ();
				}
			} catch (IndexOutOfRangeException) {
				Console.WriteLine ("Invalid number of arguments");
				Console.WriteLine ("Exiting ...");
				width = 0;
				height = 0;
				path = String.Empty;
				color = false;
				return true;
			} catch (FormatException) {
				Console.WriteLine ("Invalid argument format");
				Console.WriteLine ("Exiting ...");
				width = 0;
				height = 0;
				path = String.Empty;
				color = false;
				return true;
			}
			PrintParams (width, height, path, color);
			return CheckFileName (path);
		}

		private static bool CheckFileName (string path)
		{
			if (File.Exists (path)) {
				Console.ForegroundColor = ConsoleColor.Red;
				Console.Write ("Filename {0} already exists, continue anyways? [Y, n]: ", path);
				Console.ResetColor ();
				ConsoleKey consoleKey = Console.ReadKey (false).Key;
				if ((ConsoleKey.Y == consoleKey) || (ConsoleKey.Enter == consoleKey)) {
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine ("Original will image be overwritten");
					Console.ResetColor ();
				} else {
					Console.ForegroundColor = ConsoleColor.DarkYellow;
					Console.WriteLine ("Exiting ...");
					Console.ResetColor ();
					return true;
				}
			}
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Will save bitmap as filename: {0}", path);
			Console.ResetColor ();
			return false;
		}

		private static void PrintParams (int width, int height, string path, bool color)
		{
			Console.ForegroundColor = ConsoleColor.Green;
			Console.WriteLine ("Parameters valid:");
			Console.ResetColor ();
			Console.WriteLine ("Width: {0:n0}", width);
			Console.WriteLine ("Height: {0:n0}", height);
			Console.WriteLine ("Total: {0:n0}", width * height);
			Console.WriteLine ("Color: {0}", color);
		}

		private static RandomSequence ConstructRandomSequence (out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Constructing RandomSequence generator ...");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			RandomSequence randomSequence = new RandomSequence ();
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomSequence;
		}

		private static Bitmap GetBitmap (RandomSequence randomSequence, int width, int height, bool color, out long elapsedTime)
		{
			Bitmap bitmap;
			if (color) {
				bitmap = GenerateColoredBitmap (randomSequence, width, height, out elapsedTime);
			} else {
				bitmap = GenerateBwBitmap (randomSequence, width, height, out elapsedTime);
			}
			return bitmap;
		}

		private static Bitmap GenerateColoredBitmap (RandomSequence randomSequence, int width, int height, out long elapsedTime)
		{
			elapsedTime = 0;
			long elapsedRandomBytesTime;
			byte[] randomBytes = GetRandomBytes (randomSequence, 3 * width * height, out elapsedRandomBytesTime);
			elapsedTime += elapsedRandomBytesTime;
			long elapsedFillTime;
			Bitmap bitmap = FillColoredBitmap (randomBytes, width, height, out elapsedFillTime);
			elapsedTime += elapsedFillTime;
			return bitmap;
		}

		private static byte[] GetRandomBytes (RandomSequence randomSequence, int length, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Getting NextBytes for length: {0:n0} ... ", length);
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			byte[] randomBytes = randomSequence.GetNextBytes (length);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomBytes;
		}

		private static Bitmap FillColoredBitmap (byte[] randomBytes, int width, int height, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Filling bitmap ...");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			Bitmap bitmap = new Bitmap (width, height);
			Parallel.For (0, width, x => {
				Parallel.For (0, height, y => {
					int colorsIndex = 3 * ((y * width) + x);
					Color color = Color.FromArgb (randomBytes [colorsIndex], randomBytes [colorsIndex + 1], randomBytes [colorsIndex + 2]);
					bitmap.SetPixel (x, y, color);
				});
			});
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return bitmap;
		}

		private static Bitmap GenerateBwBitmap (RandomSequence randomSequence, int width, int height, out long elapsedTime)
		{
			elapsedTime = 0;
			long elapsedRandomBytesTime;
			bool[] exclusions = GetRandomBools (randomSequence, width * height, out elapsedRandomBytesTime);
			elapsedTime += elapsedRandomBytesTime;
			long elapsedFillTime;
			Bitmap bitmap = FillBwBitmap (exclusions, width, height, out elapsedFillTime);
			elapsedTime += elapsedFillTime;
			return bitmap;
		}

		private static bool[] GetRandomBools (RandomSequence randomSequence, int length, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Getting NextBools for length: {0:n0} ... ", length);
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			bool[] randomBools = randomSequence.GetNextBools (length);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return randomBools;
		}

		private static Bitmap FillBwBitmap (bool[] randomBools, int width, int height, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Filling bitmap ...");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			Bitmap bitmap = new Bitmap (width, height);
			Parallel.For (0, width, x => {
				Parallel.For (0, height, y => {
					int index = (width * y) + x;
					if (randomBools [index]) {
						bitmap.SetPixel (x, y, Color.White);
					}
				});
			});
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
			return bitmap;
		}

		private static void SaveBitmap (Bitmap bitmap, string path, out long elapsedTime)
		{
			Console.ForegroundColor = ConsoleColor.DarkYellow;
			Console.WriteLine ("Saving bitmap ... ");
			Console.ResetColor ();
			Stopwatch stopwatch = new Stopwatch ();
			Console.WriteLine ("Start time: {0}", DateTime.Now.TimeOfDay.ToString ());
			stopwatch.Start ();
			bitmap.Save (path);
			stopwatch.Stop ();
			elapsedTime = stopwatch.ElapsedMilliseconds;
			Console.WriteLine ("Elapsed time: {0:n0} ms", elapsedTime);
		}

	}

}

