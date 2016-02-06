using System;
using System.Collections.Generic;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            List<int> list = new List<int>();

            for (int i = 1; i <= 100; i++)
            {
                list.Add(i);
            }

            Console.WriteLine("--- Evens ---");
            foreach (var item in list.FindAll(i => i % 2 == 0))
            {
                Console.WriteLine(item);
            }

            Console.WriteLine("--- Odds ---");
            foreach (var item in list.FindAll(i => i % 2 != 0))
            {
                Console.WriteLine(item);
            }

        }
    }
}
