using System;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApplication
{
	public class DoSomething
	{
		public int Slow()
		{
		 Console.WriteLine("Start: Slow operation on thread {0}", Thread.CurrentThread.ManagedThreadId);
		 Thread.Sleep(5000);
		 Console.WriteLine("End:   Slow operation on thread id {0}", Thread.CurrentThread.ManagedThreadId);
		 return 42;
		}

		public int Fast()
		{
		 Console.WriteLine("Start: Fast operation on thread {0}", Thread.CurrentThread.ManagedThreadId);
		 int[] fib = new int[] { 0, 1, 1, 2, 3, 5, 8, 13 };
		 foreach (int element in fib)
		 {
			System.Console.WriteLine(element);
		 }
		 Console.WriteLine("End:   Fast operation on thread id {0}", Thread.CurrentThread.ManagedThreadId);
		 return 1;
		}
	}    
}
