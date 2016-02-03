using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            int numNames = 0;
            int totalChars = 0;
            foreach (var name in args)
            {
                numNames++;
                totalChars += name.Length;
                Console.WriteLine("Hello {0}!", name);
            }
            Console.WriteLine("Total number of names: {0} ({1} characters)", numNames, totalChars);
        }
    }
}
