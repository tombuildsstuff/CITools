using System;
using System.IO;
using NDesk.Options;

namespace BranchDeployer
{
	public class Program
	{
		public static void Main(string[] args)
		{
			var showHelp = false;
			var inputDirectory = "";
			var outputDirectory = "";
			var stringToReplace = "";

			var p = new OptionSet
				{
					{ "i|input=", "", f => inputDirectory = f },
					{ "o|output=", "", f => outputDirectory = f },
					{ "r|replace=", "The string which should be replaced in the branch name", f => stringToReplace = f },
					{ "h|help",  "show this message and exit", v => showHelp = v != null || args == null || args.Length == 0 }
				};

			try
			{
				p.Parse(args);
			}
			catch (OptionException e)
			{
				Console.Write("BranchDeployer: ");
				Console.WriteLine(e.Message);
				Console.WriteLine("Try 'BranchDeployer --help' for more information.");
			}

			if (showHelp)
			{
				ShowHelp(p);
				return;
			}

			var input = inputDirectory.Replace(stringToReplace, string.Empty);
			var output = outputDirectory.Replace(stringToReplace, string.Empty);
			Console.WriteLine(string.Format("Input: {0}", input));
			Console.WriteLine(string.Format("Output: {0}", output));
			DeployToDirectory(input, output);
		}

		private static void DeployToDirectory(string originalDirectory, string deploymentDirectory)
		{
			if (Directory.Exists(deploymentDirectory))
				Directory.Delete(deploymentDirectory, true);

			Directory.CreateDirectory(deploymentDirectory);

			var directories = Directory.GetDirectories(originalDirectory, "*", SearchOption.AllDirectories);
			var files = Directory.EnumerateFiles(originalDirectory, "*.*", SearchOption.AllDirectories);

			foreach (var directory in directories)
			{
				var newDirectory = directory.Replace(originalDirectory, deploymentDirectory);
				try
				{
					Directory.CreateDirectory(newDirectory);
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Format("Unable to create directory: {0} ({1})", newDirectory, ex.Message));
					throw;
				}
			}

			foreach (var file in files)
			{
				var copyTo = file.Replace(originalDirectory, deploymentDirectory);
				try
				{
					File.Copy(file, copyTo, true);
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Format("Unable to copy file: {0} to: {1} ({2})", file, copyTo, ex.Message));
					throw;
				}
			}
		}

		private static void ShowHelp(OptionSet p)
		{
			Console.WriteLine("Usage: BranchDeployer [OPTIONS]");
			Console.WriteLine();
			Console.WriteLine("Options:");
			p.WriteOptionDescriptions(Console.Out);
		}
	}
}