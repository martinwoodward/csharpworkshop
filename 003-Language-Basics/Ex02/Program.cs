using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            foreach (var name in args)
            {
                Console.WriteLine("Hello {0}!", name);
            }
        }
    }
}
