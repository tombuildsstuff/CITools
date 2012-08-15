namespace StringReplace
{
	using System;
	using NDesk.Options;

	public class Program
	{
		public static void Main(string[] args)
		{
			var showHelp = false;
			string inputFile = null;
			string outputFile = null;
			string stringToMatch = null;
			string stringToReplaceWith = null;

			var p = new OptionSet
				{
					{ "i|input=", "The input file", v => inputFile = v },
					{ "o|output=", "The output file", v => outputFile = v },
					{ "m|match=", "The string to match", v => stringToMatch = v },
					{ "r|replace=", "The string to replace the matched string with", v => stringToReplaceWith = !string.IsNullOrWhiteSpace(v) ? v : string.Empty },
					{ "h|help",  "show this message and exit", v => showHelp = v != null || args == null || args.Length == 0 }
				};

			try
			{
				p.Parse(args);
			}
			catch (OptionException e)
			{
				Console.Write("StringReplace: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try 'StringReplace --help' for more information.");
			}

			if (showHelp)
			{
				ShowHelp(p);
				return;
			}

			if (string.IsNullOrWhiteSpace(inputFile) || !System.IO.File.Exists(inputFile))
				throw new NotSupportedException("Input File Not Found!");

			if (System.IO.File.Exists(outputFile))
				System.IO.File.Delete(outputFile);

			var content = System.IO.File.ReadAllText(inputFile);
			content = content.Replace(stringToMatch, stringToReplaceWith);
			System.IO.File.WriteAllText(outputFile, content, System.Text.Encoding.UTF8);
		}

		private static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: StringReplace [OPTIONS]");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}