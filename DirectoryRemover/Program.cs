namespace DirectoryRemover
{
	using System;
	using System.IO;

	public class Program
	{
		public static void Main(string[] args)
		{
			if (args == null || args.Length == 0)
			{
				Console.WriteLine("No Directories Specified");
				return;
			}

			foreach (var directory in args)
			{
				if (!Directory.Exists(directory))
				{
					Console.WriteLine(string.Format("Directory not found: {0}", directory));
					continue;
				}

				try
				{
					Directory.Delete(directory, true);
					Console.WriteLine(string.Format("Deleted {0}", directory));
				}
				catch (Exception ex)
				{
					Console.WriteLine(string.Format("Unable to Delete: {0} ({1})", directory, ex.Message));
				}
			}
		}
	}
}