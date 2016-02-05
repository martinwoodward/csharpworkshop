using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var doSomething = new DoSomething();
            int slowResult = doSomething.Slow();
            int fastResult = doSomething.Fast();
            Console.WriteLine("The result from the slow task is {0}", slowResult);            
            Console.WriteLine("The result from the fast task is {0}", fastResult);
            Console.WriteLine("Done.");
        }
    }
}
