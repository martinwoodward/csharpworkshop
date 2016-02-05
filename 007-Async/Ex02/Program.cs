using System;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var doSomething = new DoSomething();
            Task<int> slowTask = doSomething.SlowAsync();
            int fastResult = doSomething.Fast();
            Console.WriteLine("The result from the slow task is {0}", slowTask.Result);            
            Console.WriteLine("The result from the fast task is {0}", fastResult);
            Console.WriteLine("Done");
        }
    }
}
