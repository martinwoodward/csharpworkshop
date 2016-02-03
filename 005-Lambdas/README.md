# Delegates and Lambda Expressions
This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Authors: [Ken Chen](https://github.com/chenkennt), [Martin Woodward](https://github.com/martinwoodward)

Delegates are similar to function pointers in C++ function pointers but they are type safe.
They are a disconnected way to pass a call to a method within the .NET Framework type system.

Delegates are used in various APIs in the .NET world usually through lambda expressions,
which in turn are a cornerstone of Language Integrated Query (LINQ). We will dig into LINQ in Tutorial 6.

## Delegates

A `delegate` defines a type, which specifies a particular method signature. 
A method (static or instance) that satisfies this signature can be assigned to a variable of that delegate type,
then can be called directly (with the appropriate arguments) or passed as an argument itself to another method
and then called. The following example demonstrates the use of a delegate.

```cs
public class Program
{
  // Declate a delegate that takes a string and returns a string
  public delegate string Reverse(string s);

  // Statically define an implementation that matches the signaure of the delegate defined above
  public static string ReverseString(string s)
  {
      return new string(s.Reverse().ToArray());
  }

  public static void Main(string[] args)
  {
      // Create an instance of the Reverse delegate implemented by the ReverseString method
      Reverse rev = ReverseString;

      // Invoke the delegate by passing in the appropriate parameters
      Console.WriteLine(rev("a string"));
  }
}

```


In order to streamline the development process, .NET includes a set of generic delegate types that programmers can
reuse and not have to create new types. These are `Func<>`, `Action<>` and `Predicate<>`

 - `Action<>` is used when there is a need to perform an action using the arguments of the delegate.
 - `Func<>` is used usually when you need to transform the arguments of the delegate into a different result.
 - `Predicate<>` is used when you need to determine if the argument satisfies the condition of the delegate. 
    It could also be written as a `Func<T, bool>`.

Therefore instead of a custom type we can now make use of the `Func<>` delegate

```cs
public class Program
{
  static string ReverseString(string s)
  {
      return new string(s.Reverse().ToArray());
  }

  static void Main(string[] args)
  {
      Func<string, string> rev = ReverseString;

      Console.WriteLine(rev("a string"));
  }
}

```

## Anonymous Delegates
Having a method defined outside of the Main() method seems a bit superfluous. It is because of this that
C# 2.0 introduced the concept of anonymous delegates. With their support you are able to create 
“inline” delegates without having to specify any additional type or method. This is similar to anonymous
functions in Javascript or Go. You simply inline the definition of the delegate where you need it.

For an example, lets use an anonymous delegate to filter out a list of only
even numbers and then print them to the console.

```cs
public class Program
{
  public static void Main(string[] args)
  {
    List<int> list = new List<int>();

    for (int i = 1; i <= 100; i++)
    {
        list.Add(i);
    }

    List<int> result = list.FindAll(
      delegate(int no)          // Declare our delegate and pass it to the FindAll
      {                         // method in the List collection to use to match
          return (no%2 == 0);   // all items in the list meeting this criteria
      }                         //
    );

    foreach (var item in result)
    {
        Console.WriteLine(item);
    }
  }
}

```
However, even with this approach, there is still much code that we can throw away. This is where **lambda expressions** come into play.

## Lamda Expressions

Lambda expressions, or just *lambdas* for short, were introduced first in C# 3.0. They are just a more convenient
syntax for using delegates. They declare a signature and a method body, but don’t have an formal identity of
their own, unless they are assigned to a delegate. Unlike delegates, they can be directly assigned as the
left-hand side of event registration or in various LINQ clauses and methods.

Since a lambda expression is just another way of specifying a delegate, we can rewrite the above sample as follows
```cs
public class Program
{

  public static void Main(string[] args)
  {
    List<int> list = new List<int>();

    for (int i = 1; i <= 100; i++)
    {
        list.Add(i);
    }

    List<int> result = list.FindAll(i => i % 2 == 0);  // Our lamda, getting rid of all the boilerplate code

    foreach (var item in result)
    {
        Console.WriteLine(item);
    }
  }
}

```
Let's break that syntax down some more. The delegate type is represented by the following lambda
```c#
 i => i % 2 == 0
```
To create a lambda you specify the input paramaters (if any) on the left hand side of the lambda operator `=>`
and you put the statement block on the other side. So in this example we specify a parameter of `i` and return
the value of `i % 2 == 0` which is our boolean test to indicate if `i` is even or not.

If you take a look at the highlighted lines, you can see how a lambda expression looks like.
Again, it is just a syntactical sugar for using the delegate type in .NET, so what happens under the
covers is similar to what happens with the anonymous delegate.

The example above shows the use of an expression lambda, the value of the right hand side is return.
You can also use lambda to execute statements of code by ending each statement with a semi-colon (`;`)

For example

```c#
TestDelegate myDel = n => { string s = String.Format("Hello {0}!", n); Console.WriteLine(s); };
myDel("Alice");  // Writes out "Hello Alice!"
myDel("Bob");    // Writes out "Hello Bob!"

```

## Exercises
 1. Write a console application that creates a `List` of all the numbers 1-100 and displays the even ones

 2. Extend your application to display the even numbers then the odd numbers
 
 3. Take your application from Tutorial 5 (or the solution on GitHub) and use a `Func<PersonList,int>` delegate
    to calculate the average age of the people in your list and display it on the console


## Additional Information
 - [Delegates](https://msdn.microsoft.com/en-us/library/ms173171.aspx)
 - [Anonymous Functions](https://msdn.microsoft.com/en-us/library/bb882516.aspx)
 - [Lambda expressions](https://msdn.microsoft.com/en-us/library/bb397687.aspx)

---
 - Next: [Tutorial 6 - Linq](../006-Linq/)
 - Previous: [Tutorial 4 - Properties](../004-Properties/)
 - Back to [Table of Contents](../README.md)

