using System;

namespace Airport
{
	public class ConsoleWrapper : ICommandLine
	{
		public void Write(string text)
		{
			Console.WriteLine(text);
		}

		public string Read()
		{
			return Console.ReadLine();
		}
	}
}