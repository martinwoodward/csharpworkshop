using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Define a default name
            string name = "World";
            if (args != null && args.Length > 0)
            {
                // If we've passed any command line arguments, grab the first as the name
                name = args[0];
            }
            // Use a format string to print the name in the message
            Console.WriteLine("Hello {0}!", name);
        }
    }
}
