using System;

namespace ashr.net.hacktools
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			if (args.Length < 2) {
				showUsage ();
				return;
			}

			string queryToLocate = args [0];
			int length = int.Parse(args [1]);

			OffsetFinder offsetFinder = new OffsetFinder ();
			offsetFinder.PrintOffset (queryToLocate, length);
		}

		private static void showUsage(){
			Console.WriteLine ("pattern_offset querytolocate lengthofpattern (pattern_offset Aa0A 1024)");
		}
	}
}
