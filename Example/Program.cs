/*
 * Created by SharpDevelop.
 * User: janosch.woschitz@gmail.com
 */
using System;
using System.Diagnostics;

namespace Brutus.Example
{
	class Program
	{
		public static void Main(string[] args)
		{
			Console.WriteLine("Enter string to match:");
			string toMatch = Console.ReadLine();
			Console.WriteLine("Start to compute keys, trying to find match for \"{0}\".", toMatch);
			
			char[] keyspace = new char[] {
		        'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j',
		        'k', 'l', 'm', 'n', 'o', 'p', 'q', 'r', 's', 't',
		        'u', 'v', 'w', 'x', 'y', 'z','A','B','C','D','E',
		        'F','G','H','I','J','K','L','M','N','O','P','Q','R',
		        'S','T','U','V','W','X','Y','Z','1','2','3','4','5',
		        '6','7','8','9','0','!','$','#','@','-'
	    	};
			Executor executor = new Executor(keyspace);
			
			var iterations = Program.GetEstimatedIterations(keyspace, toMatch.ToCharArray());
			Console.WriteLine("Need to compute {0} keys to match your input.", iterations);
			// just a rough estimation
			Console.WriteLine("Approximate time needed: {0} ms", iterations / 5000);
			
			char[] lookupKey = toMatch.ToCharArray();
			bool isMatched = false;
			ulong generatedKeyCount = 0;
			
			Stopwatch stopWatch = Stopwatch.StartNew();
			
			while(!isMatched) {
				char[] currentKey = executor.ComputeNextKey();
				generatedKeyCount++;
				if (generatedKeyCount % 500000 == 0) {
					Console.Write('.');
				}
				if (currentKey.Length != lookupKey.Length) {
					continue;
				}
				for(var i = currentKey.Length - 1; i >= 0; i--) {
					if (!(currentKey[i] == lookupKey[i])) {
						break;
					}
					if (i == 0) {
						isMatched = true;
					}
				}
			}
			
			stopWatch.Stop();
			
			Console.WriteLine("Found match.");
			Console.WriteLine("Generated {0} keys, took {1}ms.", generatedKeyCount, stopWatch.ElapsedMilliseconds.ToString());
			Console.Write("Press any key to continue . . . ");
			Console.ReadKey(true);
		}
		
		public static double GetEstimatedIterations(char[] keyspace, char[] sequence) {
			var lookupTable = new char[keyspace.Length + 1];
			lookupTable[0] = '\0';
			for(var i = 0; i < keyspace.Length; i++) {
				lookupTable[i + 1] = keyspace[i];
			}
			
			double iterations = 0;
			for(var i = 0; i < sequence.Length; i++) {
				iterations = (iterations * keyspace.Length) + Array.IndexOf(lookupTable, sequence[i]);
			}
			return iterations;
		}
	}
}