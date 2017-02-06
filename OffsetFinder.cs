using System;
using System.Text;

namespace ashr.net.hacktools
{
	public class OffsetFinder
	{
		public OffsetFinder (){}

		static char currentFirst = 'Z';
		static char currentSecond ='z';
		static int currentThird = 9;

		private static string ReverseString(string s)
		{
			char[] arr = s.ToCharArray();
			Array.Reverse(arr);
			return new string(arr);
		}

		private bool resetPattern(){
			currentFirst = 'Z';
			currentSecond = 'z';
			currentThird = '9';
			return true;
		}

		private static string getNextNumber(bool LittleEnd){
			char incCurrentFirst = currentFirst;//(char)((int)currentFirst + 1);
			char incCurrentSecond = currentSecond;//(char)((int)currentSecond + 1);
			int incCurrentThird = ++currentThird;

			if (incCurrentThird > 9) {
				incCurrentThird = 0;

				incCurrentSecond = (char)((int)currentSecond + 1);

				if (((int)incCurrentSecond) > ((int)'z')) {
					incCurrentSecond = 'a';

					incCurrentFirst = (char)((int)currentFirst + 1);

					if (((int)incCurrentFirst) > ((int)'Z')) {
						incCurrentFirst = 'A';
					}
				}
			}

			currentFirst = incCurrentFirst;
			currentSecond = incCurrentSecond;
			currentThird = incCurrentThird;

			return (LittleEnd ? 
				incCurrentThird.ToString () + incCurrentSecond.ToString () + incCurrentFirst.ToString ()
				: 
				incCurrentFirst.ToString () + incCurrentSecond.ToString () + incCurrentThird.ToString ());
		}

		public string GenerateLittleEndPattern(int Length){
			resetPattern ();
			StringBuilder output = new StringBuilder ();

			while (output.Length < Length) {
				output.Append (getNextNumber (true));
			}

			return (output.ToString ().Substring(0,Length));			
		}

		public string GenerateBigEndPattern(int Length){
			resetPattern ();
			StringBuilder output = new StringBuilder ();

			while (output.Length < Length) {
				output.Append (getNextNumber (false));
			}

			return (output.ToString ().Substring(0,Length));			
		}

		public void PrintOffset(string Pattern, int Length){
			string internalPattern = GenerateBigEndPattern (Length);
			Console.WriteLine ("Internal Big:" + internalPattern);
			//Easy mode (Exact Matches) 4 bytes
			int indexOfPattern = internalPattern.IndexOf (Pattern);

			if (indexOfPattern > -1) {
				Console.WriteLine ("Exact match at:" + indexOfPattern);
				return;
			}
				
			//Derp1 mode 3 bytes and less
			//BigEnd i dont know what im doing
			int searchStringLength = 3;
			while (searchStringLength > 1) {
				string searchString = Pattern.Remove (searchStringLength);
				int startIndex = 0;
				indexOfPattern = internalPattern.IndexOf (searchString);

				while (indexOfPattern != -1) {
					Console.WriteLine ("Found a possible match at " + indexOfPattern + " byte offset " + searchStringLength);
					startIndex=indexOfPattern+1;
					indexOfPattern = internalPattern.IndexOf (searchString, startIndex);
				}
				searchStringLength--;
			}
				
			//internalPattern = GenerateLittleEndPattern (Length);
			//Console.WriteLine ("Internal Little:" + internalPattern);

			//Easy mode (Exact Matches) 4 bytes
			indexOfPattern = internalPattern.IndexOf (ReverseString(Pattern));

			if (indexOfPattern > -1) {
				Console.WriteLine ("Rev Exact match at:" + indexOfPattern);
				return;
			}
			//Derp2 mode 3 bytes and less
			//LittleEnd i dont know what im doing
			searchStringLength = 3;
			while (searchStringLength > 1) {
				string searchString = ReverseString(Pattern.Remove (searchStringLength));
				int startIndex = 0;
				indexOfPattern = internalPattern.IndexOf (searchString);

				while (indexOfPattern != -1) {
					Console.WriteLine ("Rev Found a possible match at " + indexOfPattern + " byte offset " + searchStringLength);
					startIndex=indexOfPattern+1;
					indexOfPattern = internalPattern.IndexOf (searchString, startIndex);
				}
				searchStringLength--;
			}
		}
	}
}

