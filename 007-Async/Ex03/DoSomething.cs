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
		
		public async Task<int> SlowAsync()
		{
		 Console.WriteLine("Start: Slow operation on thread {0}", Thread.CurrentThread.ManagedThreadId);
		 await Task.Delay(100);
		 Console.WriteLine("End:   Slow operation on thread id {0}", Thread.CurrentThread.ManagedThreadId);
		 return 42;
		}
		
		public int Fast()
		{
		 Console.WriteLine("Start: Fast operation on thread {0}", Thread.CurrentThread.ManagedThreadId);
		 for (int i = 1; i < 5000; i++)
		 {
		    System.Console.WriteLine(i);
		 }
		 Console.WriteLine("End:   Fast operation on thread id {0}", Thread.CurrentThread.ManagedThreadId);
		 return 1;
		}
	}    
}
