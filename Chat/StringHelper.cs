
using System;

namespace Chat
{

    public static class StringHelper
    {
        public const string DefaultPrompt = ">> ";
        public static string Prompt(this string prompt)
		{			
			Console.Write($"{prompt}");
			return Console.ReadLine();
		}
        public static void PrintHelp()
		{
			Console.Error.WriteLine("Chat [username]");
		}

		public static void Print(this string message)
		{			
			Console.Write($"{Environment.NewLine}{message}{Environment.NewLine}{StringHelper.DefaultPrompt}");
		}
    } 
}