# C# Language Basics

This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

In this tutorial we will quickly cover the basics of the C# language. Future tutorials will dig deeper into
features such as Language Integrated Query (LINQ) and Asynchronous programming.

## Introduction

C# is a C++ style language and therefore the basic syntax will be familiar to any developer who understands C, C++
Java or related languages. It is a managed language designed to run in a Common Language Runtime benefiting from 
garbage collection and just-it-time compilation. C# has a simple syntax that is powerful in nature and is designed
to reduce the amount of boiler-plate type code that a developer needs to create for an application. But it also
provides simple mechanisms for interoperability with and library that provides a C style API including the ability
to easily invoke native code and have direct access to memory through use of C++ style pointers.

## Variables

Variables can be declared with a scope that is local or instance (class). C# is a statically typed language so each
variable has a specific type but the compiler also supports the `var` keyword to mean that the type is inferred by
the compiler at compilation time.  Therefore the following are both valid and compile to the same thing.
```c#
int x = 1;
var y = 1;

// Therefore
int z = x + y;
// Would return an int with a value of 2.
```
You can also declare variables with-in the scope of the instance of the class or define constants at the class level
as shown below.
```c#
    public class Program
    {
        private static readonly string _message = "Hello World!";
        public static void Main(string[] args)
        {
            Console.WriteLine(_message);
        }
    }
```
Note that C# has a `const` keyword that declares a constant at compilation time, however in general it is best
practice to instead make use of the `static readonly` keywords to indicate to the compiler that a variable is a
constant at runtime and does not change once created.

In the example above we named our private constant with an underscore prefix and the variable was camelCased.
This is purely by convention. The C# coding styles used by the .NET Core team themselves are 
[available on GitHub](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md).

## Types
As mentioned previously, C# is statically typed. In the examples previously `int` and `string` are simply aliases to 
the underlying implementation of them in the Base Class Library (BCL). You can find the source code for these
implementations on GitHub.
 - Source for [System.Int32](https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/Int32.cs)
 - Source for [System.String](https://github.com/dotnet/coreclr/blob/master/src/mscorlib/src/System/String.cs)

You can create your own types by defining a class for them and providing your implementation. For example
```c#
    public class Food
    {
        private string _foodStuff;
        private int _howNice;

        public Food(string foodStuff, int howNice)
        {
            _foodStuff = foodStuff;
            _howNice = howNice;
        }
    }
```
And then you can create a new instance of your type in your application in either way as follows
```c#
    var chocolate = new Food("Chocolate", 99);
    Food brocoli = new Food("Brocoli", 1);
```

## Inheritance
C# is an Object Orientated language with type-safety and therefore provides strong controls around inheritance.
By default, methods on a base class are not overridable unless they are declared as `virtual` meaning they
may be overidden by a derived class or `abstract` which means they must be implemented in a derived class. You
can define a class as `abstract` if you do not wish it to be directly instantiated with the `new` keyword and must
have an implementation to be created. A class can prevent other classes from inheriting it by declaring itself or
a member as `sealed`. C# deliberately only supports single inheritance of classes so the way that
a class implements multiple behaviours is by the use of interfaces.

In the example below we bring back our `Food` class but add a method of `Eat` and mark it as `virtual`. Then we
derrive two types of `Food`, `Chocolate` and '`Brocoli`. We then overide the `Eat` method of `Brocoli` to throw
an exception. 
```c#
    public class Food
    {
        private string _foodStuff;
        private int _howNice;

        public Food(string foodStuff, int howNice)
        {
            _foodStuff = foodStuff;
            _howNice = howNice;
        }

        public virtual void Eat()
        {
            Console.WriteLine("nom nom");
        }
    }

    public class Chocolate : Food
    {
        public Chocolate() : base("Chocolate", 99)
        { }
    }

    public class Brocoli : Food
    {
        public Brocoli() : base("Brocoli", 1)
        { }

        public override void Eat()
        {
            throw new InvalidOperationException("Sorry, but eating Brocoli is not allowed");
        }
    }
```

Note that a colon (`:`) is used to indicate a class inherits from another class or interface. If providing a list of 
multiple interfaces then they are comma (`,`) separated.  A `:` is also used by a constructor in a derived class
to call a constructor in the base class (instead of something of using the `this` keyword like you would in a language 
like Java).

Also note that an Exception was thrown without it being explicitly declared. In c#, exceptions occur at runtime but may
be caught using a `try`,`catch`,`finally` block.

For more information see the following sections of the C# Programming Guide
 - [Inheritance](https://msdn.microsoft.com/en-us/library/ms173149.aspx)
 - [Polymorphism](https://msdn.microsoft.com/en-us/library/ms173152.aspx)
 - [Abstract and Sealed Classes and Class Members](https://msdn.microsoft.com/en-us/library/ms173150.aspx)
 - [Interfaces](https://msdn.microsoft.com/en-us/library/ms173156.aspx)
 - [Exceptions and Exception Handling](https://msdn.microsoft.com/en-us/library/ms173160.aspx)


## Operators
You can use all the operators you would expect for example
```c#
	int x = (1 + 5 - 2) / (2 * 2);  // x = 1
	x++; // Same as saying x = x + 1 (which would now make x=2)
	x--; // Same as saying x = x - 1 (which would make x=1)
	Console.WriteLine(++x); // Would first increment x to 2 then display the result
	Console.WriteLine(--x); // Would first decrement x to 1 then display the result
    int i = 42
    Console.WriteLine(i == 42); // Would display 'True'
```
The full range of operators in C# is available in the 
[reference documentation](https://msdn.microsoft.com/en-us/library/6a71f45d.aspx)

Note that assignment is performed using a single `=` where-as the double `==` is the test for equality.

You can [implement operators in your own types](https://msdn.microsoft.com/en-us/library/8edha89s.aspx), or make use of some operators with common system types.
For example to append strings you can do
```c#
	String message = "Hello " + "World!";
	Console.WriteLine("Hello World!" == message); // Would display 'True'
```
(To see the overload for the equality operator `==` in `System.String` take 
a look at the [source on GitHub](https://github.com/dotnet/coreclr/blob/21f8416ad4204afc18ce315d99baa5f4ada28d9a/src/mscorlib/src/System/String.cs#L731-L733))

The C# compiler is also able to auto-cast types for you, therefore the following is valid syntax as the compiler will
first convert the integer 42 into it's string representation, `"42"` and then concatenate it to the previous string.
```c#
	Console.WriteLine("The answer is :" + 42);
```

## Collections
.NET has a rich set of collections out the box. As .NET has also provides a rich implementation of generics for a very
long time, all of the collections have generic versions that are commonly used.  Using them is easy
```c#
    // Create a list that must contain objects of type String
    List<string> listOfStrings = new List<string> { "First", "Second", "Third" };

    // listOfStrings can accept only strings, both on read and write.
    listOfStrings.Add("Fourth");

    // Below will throw a compile-time error, since the type parameter
    // specifies this list as containing only strings.
    listOfStrings.Add(1);
```
You can read more about Generics in the [reference documentation](http://dotnet.github.io/docs/concepts/generics.html)
where you can also find a list of [common generic collections](http://dotnet.github.io/api/System.Collections.Generic.html).

## String Formatting
The String class in C# supports the formatting of strings where a format can be passed along with number of parameters
to convert into strings and insert into the result.
```c#
	String primes = String.Format("Prime numbers {0} than {1} are: {2}, {3}, {4}, {5}, {6}",
                       "less", "ten", 1, 2, 3, 5, 7 );
	Console.WriteLine(primes);
```
For convenience, many methods that output messages have an overload that accepts a format string and an array of 
objects. `Console.WriteLine` is one such example, therefore the code above could be simplified into
```c#
	Console.WriteLine("Prime numbers {0} than {1} are: {2}, {3}, {4}, {5}, {6}",
                       "less", "ten", 1, 2, 3, 5, 7);
```

For more information see the reference documentation on [Composite String Formats](https://msdn.microsoft.com/en-us/library/txafckwd.aspx).

## Control Flow
The `if` operator in C# works pretty much as you would expect, remembering that C# likes curly braces and ignores
whitespace.
```c#
	// Change the values of these variables to test the results.
	bool Condition1 = true;
	bool Condition2 = true;
	bool Condition3 = true;
	bool Condition4 = true;
	
	if (Condition1)
	{
	    // Condition1 is true.
	}
	else if (Condition2)
	{
	    // Condition1 is false and Condition2 is true.
	}
	else if (Condition3)
	{
	    if (Condition4)
	    {
	        // Condition1 and Condition2 are false. Condition3 and Condition4 are true.
	    }
	    else
	    {
	        // Condition1, Condition2, and Condition4 are false. Condition3 is true.
	    }
	}
	else
	{
	    // Condition1, Condition2, and Condition3 are false.
	}
```
The `switch` statement in C# is also pretty standard except that it can also use the equality operator. For example
the following is a valid `switch` statement in C#
```c#
		Console.WriteLine("Coffee sizes: 1=small 2=medium 3=large");
        Console.Write("Please enter your selection: ");
        string str = Console.ReadLine();
        int cost = 0;

        // Notice the goto statements in cases 2 and 3. The base cost of 25
        // is added to the additional cost for the medium and large sizes.
        switch (str)
        {
            case "1":
            case "small":
                cost += 25;
                break;
            case "2":
            case "medium":
                cost += 25;
                goto case "1";
            case "3":
            case "large":
                cost += 50;
                goto case "1";
            default:
                Console.WriteLine("Invalid selection. Please select 1, 2, or 3.");
                break;
        }
        if (cost != 0)
        {
            Console.WriteLine("Please insert {0} {1}.", cost, "cents");
        }
        Console.WriteLine("Thank you for your business.");

```
## Loops
In c# you have the usual `do` `while` and `for` loops
```c#
    int x = 0;
    do 
    {
        Console.WriteLine(x++);
    } while (x < 5);

    int y = 1;
    while (n < 6) 
    {
        Console.WriteLine("n = {0}", n++);
    }

    for (int i = 1; i <= 5; i++)
    {
        Console.WriteLine(i);
    }
```
c# also supports the `foreach` loop which is recommended when iterating through arrays or collections.
```c#
    int[] fib = new int[] { 0, 1, 1, 2, 3, 5, 8, 13 };
    foreach (int element in fib)
    {
        System.Console.WriteLine(element);
    }

	List<string> listOfStrings = new List<string> { "First", "Second", "Third" };
    foreach (var element in listOfStrings)
    {
        System.Console.WriteLine(element);
    }

```
Any class that implements the [`IEnumerable`](https://msdn.microsoft.com/en-us/library/system.collections.ienumerable.aspx)
or [`IEnumerable<T>`](https://msdn.microsoft.com/en-us/library/9eekhta0.aspx) interface can be iterated on using
a `foreach` loop. `IEnumerable` is frequently used when passing around collections of data types and performing operations on them
as we will see later.

## Exercises
  1. Create a HelloWorld application in .NET and allow a user to pass in a name to say hello to when running the
     application by typing `dotnet run Alice`
  2. Modify your HelloWorld application to say hello to multiple names when passed in by typing `dotnet run Alice Bob`.
  3. Display a count of the number of names that you have displayed and the sum total of characters in the names.

## Conclusion
In this tutorial we learnt the basics of the C# syntax, how to declare variables and classes and perform operations
on them. In the following tutorial we will learn more about properties in C#.

## Additional Information
 - [.NET Code Coding Style Guide](https://github.com/dotnet/corefx/blob/master/Documentation/coding-guidelines/coding-style.md)
 - [C# Operators](https://msdn.microsoft.com/en-us/library/6a71f45d.aspx)
 - [Overloading Operators](https://msdn.microsoft.com/en-us/library/8edha89s.aspx)
 - [Inheritance](https://msdn.microsoft.com/en-us/library/ms173149.aspx)
 - [Polymorphism](https://msdn.microsoft.com/en-us/library/ms173152.aspx)
 - [Abstract and Sealed Classes and Class Members](https://msdn.microsoft.com/en-us/library/ms173150.aspx)
 - [Exceptions and Exception Handling](https://msdn.microsoft.com/en-us/library/ms173160.aspx)
 - [Interfaces](https://msdn.microsoft.com/en-us/library/ms173156.aspx)
 - [Composite String Formats](https://msdn.microsoft.com/en-us/library/txafckwd.aspx)
 - [if-else (C# Reference)](https://msdn.microsoft.com/en-us/library/5011f09h.aspx)
 - [Generics Overview](http://dotnet.github.io/docs/concepts/generics.html)

---
 - Next: [Tutorial 4 - Properties](../004-Properties/)
 - Previous: [Tutorial 2 - Hello C# World](../002-Hello-CSharp/)
 - Back to [Table of Contents](../README.md)

