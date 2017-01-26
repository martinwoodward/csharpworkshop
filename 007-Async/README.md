# Asynchronous Programming in .NET
This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Authors: [Phillip Carter](https://github.com/cartermp), [Ken Chen](https://github.com/chenkennt), [Martin Woodward](https://github.com/martinwoodward)

Async programming is a first-class concept within .NET, with async support in the runtime,
the framework libraries, and .NET language constructs including in C#. Internally, they are based off of objects 
(such as `Task`) which take advantage of the underlying operating system to perform I/O-bound jobs
as efficiently as possible. In client applications this is useful to ensure the UI thread remains responsive
and for server applications having first class native support for asynchronous programming is a key factor in
helping developers build scalable web applications.

## `async` in C#

The C# language-level asynchronous programming model follows what is known as the
[Task-based Asynchronous Pattern (TAP)](https://msdn.microsoft.com/en-us/library/hh873175.aspx).
The core of TAP are the `Task` and `Task<T>` objects, which model asynchronous operations, supported 
by the `async` and `await` keywords (`Async` and `Await` in VB), which provide a natural developer experience
for interacting with Tasks. The result is the ability to write expressive asynchronous code, as opposed to
callback methods which often do not express the intent as cleanly. There are other ways to approach
async code than `async` and `await` outlined in the TAP article linked above, but this tutorial will focus
on the language-level constructs in C#.

For example, you may need to download some data from a remote web service when a button is pressed,
but don’t want to block the UI thread. It can be accomplished simply like this:

```cs
private readonly HttpClient _httpClient = new HttpClient();

...

button.Clicked += async (o, e) =>
{
    var stringData = await _httpClient.DownloadStringAsync(URL);
    DoStuff(stringData);
};

```

And that’s it! The code expresses the intent (downloading some data asynchronously) without getting bogged down
in interacting with Task objects.

For the theoretically-inclined, this is an implementation of the
[Future/Promise concurrency model](https://en.wikipedia.org/wiki/Futures_and_promises).

A few important things to know before continuing:

*   Async code uses the `Task<T>` and `Task` types, which are constructs used to model the work being done in
    an asynchronous context.
*   When the `await` keyword is applied, it suspends the calling method and yields control back to its caller
    until the awaited task is complete. This is what allows a UI to be responsive and a service to be elastic.
*   `await` can only be used inside an async method.
*   Unless an async method has an `await` inside its body, it will never yield!
*   `async void` should **only** be used on Event Handlers (where it is required).

## Example

The following example shows how to write basic async code for both a client app and a web service. The code,
in both cases, will count the number of times ”.NET” appears in the HTML of “dotnetfoundation.org”.

##### Client application snippet:

```cs
private readonly HttpClient _httpClient = new HttpClient();

private async void SeeTheDotNets_Click(object sender, RoutedEventArgs e)
{
    // Capture the task handle here so we can await the background task later.
    var getDotNetFoundationHtmlTask = _httpClient.GetStringAsync("http://www.dotnetfoundation.org");

    // Any other work on the UI thread can be done here, such as enabling a Progress Bar.
    // This is important to do here, before the "await" call, so that the user
    // sees the progress bar before execution of this method is yielded.
    NetworkProgressBar.IsEnabled = true;
    NetworkProgressBar.Visibility = Visibility.Visible;

    // The await operator suspends SeeTheDotNets_Click, returning control to its caller.
    // This is what allows the app to be responsive and not hang on the UI thread.
    var html = await getDotNetFoundationHtmlTask;
    int count = Regex.Matches(html, ".NET").Count;

    DotNetCountLabel.Text = $"Number of .NETs on dotnetfoundation.org: {count}";

    NetworkProgressBar.IsEnabled = false;
    NetworkProgressBar.Visbility = Visibility.Collapsed;
}

```

##### Web service snippet (ASP.NET MVC):

```cs
private readonly HttpClient _httpClient = new HttpClient();

[HttpGet]
[Route("DotNetCount")]
public async Task<int> GetDotNetCountAsync()
{
    // Suspends GetDotNetCountAsync() to allow the caller (the web server)
    // to accept another request, rather than blocking on this one.
    var html = await _httpClient.GetStringAsync("http://dotnetfoundation.org");

    return Regex.Matches(html, ".NET").Count;
}

```
##### Console application snippet:
```cs
using System;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace ConsoleApplication
{
    public class Program
    {
        public static readonly string ADDRESS = "http://dotnetfoundation.org";
        public static int Main(string[] args)
        {
            HttpClient httpClient = new HttpClient();
            Task<string> getStringTask = Task.Run(async() =>
            {
                return await httpClient.GetStringAsync(ADDRESS);
            });
            Console.WriteLine("Connecting to {0}...", ADDRESS);
            Console.WriteLine("This may take a moment");
            Console.WriteLine("Number of times \".NET\" used: {0}", 
                              Regex.Matches(getStringTask.Result, ".NET").Count);
            return 1;
        }
    }
}

```

## More on Task and Task<T>

As mentioned before, Tasks are constructs used to represent operations working in the background.

*   `Task` represents a single operation which does not return a value.
*   `Task<T>` represents a single operation which returns a value of type `T`.

Tasks are awaitable, meaning that at the `await` will allow your application or service to perform useful
work while the task is running by yielding control to its caller until the task is done.
If you’re using `Task<T>`, the `await` keyword will additionally “unwrap” the value returned when the Task is
complete.

It’s important to reason about Tasks as abstractions of work happening in the background, and _not_ an
abstraction over multithreading. In fact, unless explicitly started on a new thread via `Task.Run`, a Task will
**start on the current thread and delegate work to the Operating System**.

## Typical lifecycle of an `async call` 
To understand the detail of what happens during an async call let's examine the example previously.

The call (such as `GetStringAsync` from `HttpClient`) makes its way through the .NET libraries and runtime
until it reaches a system interop call (such as `P/Invoke` to call out to the underling OS).
This eventually makes the proper System API call (such as `write()` to a socket file descriptor on Linux).
That System API call is then dealt with in the kernel, where the I/O request is sent to the proper subsystem.
Although details about scheduling the work on the appropriate device driver are different for each OS,
eventually an “incomplete task” signal will be sent from the device driver, bubbling its way back up to the .NET
runtime. This will be converted into a `Task` or `Task<T>` by the runtime and returned to the calling method.
When `await` is encountered, execuction is yielded and the system will continue in the calling method 
while the Task is running.

When the device driver has the data, it sends an interrupt which eventually allows the OS to bubble the
result back up to the runtime, which will the queue up the result of the Task. 
Eventually execution will return to the method which called `GetStringAsync` at the `await`,
and will “unwrap” the return value from the `Task<string>` which was being awaited.
The method now has the result!

Although many details were glossed over here (such as how “borrowing” compute time on a thread pool is coordinated),
the important thing to recognize here is that this is an interrupt based system and **no thread is 
100% dedicated to running the initiated task**. This allows threads in the thread pool of a system to handle
a larger volume of work rather than having to wait for I/O to finish.

Although the above may seem like a lot of work to be done, when measured in terms of wall clock time,
it’s miniscule compared to the time it takes to do the actual typical I/O operation. 

Tasks are also used outside of the async programming model. They are the foundation of the Task Parallel Library,
which supports the parallelization of CPU-bound work via
[Data Parallelism](https://msdn.microsoft.com/en-us/library/dd537608.aspx) and 
[Task Parallelism](https://msdn.microsoft.com/en-us/library/dd537609.aspx).

## Important Information and Advice

Although async programming is relatively straightforward in C#, there are some rules to keep in mind which
can prevent unexpected behavior.

### You should add `Async` as the suffix of every async method name you write.

This is the naming convention used in .NET to more-easily differentiate synchronous and asynchronous methods.
Note that certain methods which aren’t explicitly called by your code (such as event handlers or 
web controller methods) don’t necessarily apply. Because these are not explicitly called by your code, 
being explicit about their naming isn’t as important.

### `async void` **should only be used for event handlers.

`async void` is the only way to allow asynchronous event handlers to work because events do not have
return types (thus cannot make use of `Task` and `Task<T>`). Any other use of `async void` does not follow
the TAP model and can be challenging to use, such as:

  *   Exceptions thrown in an `async void` method can’t be caught outside of that method.
  *   `async void` methods are very difficult to test.
  *   `async void` methods can cause bad side effects if the caller isn’t expecting them to be async.

### Tread carefully when using async lambdas in LINQ expressions

I know we've only just learned them and the temptation will be to used all these new tricks at ones but Lambda
expressions in LINQ use deferred execution, meaning code could end up executing at a time when you’re not
expecting it to. The introduction of blocking tasks into this can easily result in a deadlock if not written
correctly. Additionally, the nesting of asynchronous code like this can also make it more difficult to reason
about the execution of the code. Async and LINQ are powerful, but should be used together as carefully
and clearly as possible.

### Write code that awaits Tasks in a non-blocking manner

Blocking the current thread as a means to wait for a Task to complete can result in deadlocks and blocked context
threads, and can require significantly more complex error-handling. The following table provides guidance on how
to deal with waiting for Tasks in a non-blocking way:

| Use this...          | Instead of this... | When wishing to do this |
| -------------------- | ------------------ | ----------------------- |
| `await`              | `Task.Wait` or `Task.Result` | Retrieving the result of a background task |
| `await Task.WhenAny` | `Task.WaitAny`     | Waiting for any task to complete |
| `await Task.WhenAll` | `Task.WaitAll`     | Waiting for all tasks to complete |
| `await Task.Delay`   | `Thread.Sleep`     | Waiting for a period of time |

### Write less stateful code

Don’t depend on the state of global objects or the execution of certain methods. Instead, depend only on the
return values of methods. Why?

  *   Code will be easier to reason about.
  *   Code will be easier to test.
  *   Mixing async and synchronous code is far simpler.
  *   Race conditions can typically be avoided altogether.
  *   Depending on return values makes coordinating async code simple.
  *   (Bonus) it works really well with dependency injection.

A recommended goal is to achieve complete or near-complete
[Referential Transparency](https://en.wikipedia.org/wiki/Referential_transparency_%28computer_science%29)
in your code. Doing so will result in an extremely predictable, testable, and maintainable codebase.

## Exercises
   1. Write a console application that initializes a new `DoSomething` class below, executes the
      `DoSomething.Slow()` method then the `DoSomething.Fast()` method and then displays the result
      `Slow()` on one line followed by the result of `Fast()` on the next. Run the application and
      observe the results.

   2. Create a new `DoSomething.SlowAsync()` method that does the same as the `Slow()` method below
      but instead of `Thread.Sleep(5000);` waits for 5 seconds using the following code instead
      `await Task.Delay(5000);`. Modify your application to call the new Async version instead,
      execute and observe the results.

   3. Adjust the amount of delay in the `Slow()` method and increase the work done in the `Fast()` method
      until you observe the fast method thread finishing after the slow method thread and observe the
      behaviour.

**Sample Code**
```c#
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

```
**Dependencies**
For the exercises, you will also need the following dependencies in your project.json
```
  "dependencies": {
      "System.Threading.Tasks": "4.3.0",
      "System.Threading.Thread": "4.3.0"
  },
```

## Summary
The use of `async` and `await` in C# helps .NET optimize the usage of processing threads and allows your application
to feel more responsive to users and to have better scalability characteristics without introducing significant
additional complexity to your code.  The .NET framework takes care of thread pooling and thread synchronization for the developer
but you still have to be explicit with your code when you want to make use of asynchronous programming. Consider
using Async when calling any potentially long running I/O operations or other long, non-CPU bound activities.

## Additional Information
 - [Async/Await Reference Docs](https://msdn.microsoft.com/en-us/library/hh191443.aspx)
 - [Tasks and the Task Parallel Library](https://msdn.microsoft.com/en-us/library/dd460717.aspx)
 - [Async Programming : Introduction to Async/Await on ASP.NET](https://msdn.microsoft.com/en-gb/magazine/dn802603.aspx)


---
 - Next: [Tutorial 8 - Web Programming with ASP.NET Core](../008-Asp.net/)
 - Previous: [Tutorial 6 - Linq](../006-Linq/)
 - Back to [Table of Contents](../README.md)

