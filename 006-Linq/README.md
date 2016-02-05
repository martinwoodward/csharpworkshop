# Language Integrated Query (LINQ)

This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Authors: [Ken Chen](https://github.com/chenkennt), [Martin Woodward](https://github.com/martinwoodward)

LINQ is a powerful set of features for .NET that allow you to write simple, declarative code for
operating on data. LINQ provides language-level data querying capabilities and a 
[higher-order function](https://en.wikipedia.org/wiki/Higher-order_function) API in .NET as a way to write
expressive, declarative code. The data being queries can be in many forms (such as in-memory objects, 
in a SQL database, or an XML document), but the LINQ code you write typically won’t look different regardless 
of the data source.

The LINQ query syntax should look familiar to people familiar with traditional SQL queries:
```cs
var linqExperts = from p in programmers
                  where p.IsNewToLINQ
                  select new LINQExpert(p);

```

Same example using the `IEnumerable<T>` API syntax:

```cs
var linqExperts = programmers.Where(p => IsNewToLINQ)
                             .Select(p => new LINQExpert(p));

```

## LINQ is Expressive

Imagine you have a list of pets, but want to convert it into a `Dictionary`
where you can access a pet directly by its `RFID` value. Traditionally you
could do this with a `foreach`

```cs
var petLookup = new Dictionary<int, Pet>();

foreach (var pet in pets)
{
    petLookup.Add(pet.RFID, pet);
}

```
The equivalent LINQ expression (using a [Lambda](../005-Landas) expression):

```cs
var petLookup = pets.ToDictionary(pet => pet.RFID);

```
The code above is much more expressive as it reads exactly as per the intent (converting a `List` to a `Dictionary`)
It's also a lot more terse.

## LINQ Providers Simplify Data Access

For a significant chunk of software in the world, it revolves around dealing with data from some
source (Databases, JSON, XML, etc). Often this involves learning a new API for each data source. 
LINQ simplifies this by abstracting common elements of data access into a query syntax which looks the same
no matter which data source you pick.

Consider the following example when finding all XML elements with a specific attribute value.

```cs
public static FindAllElementsWithAttribute(XElement documentRoot, string elementName,
                                           string attributeName, string value)
{
    return from el in documentRoot.Elements(elementName)
           where (string)el.Element(attributeName) == value
           select el;
}

```
Writing code to manually traverse the XML document to perform this task would be far more challenging.

Interacting with XML isn’t the only thing you can do with LINQ Providers.
[Linq to SQL](https://msdn.microsoft.com/en-us/library/bb386976.aspx) is a fairly bare-bones
Object-Relational Mapper (ORM) for an MSSQL Server Database. 
The [JSON.NET](http://www.newtonsoft.com/json/help/html/LINQtoJSON.htm) library provides efficient
JSON Document traversal via LINQ. Furthermore, if there isn’t a library which does what you need,
you can also [write your own LINQ Provider](https://msdn.microsoft.com/en-us/library/vstudio/Bb546158.aspx).

## Difference between the API and Query syntax

As stated previously, there are two main ways to use Linq. The query syntax:
```cs
var filteredItems = from item in myItems
                    where item.Foo
                    select item;

```
And the API/Method syntax that the compiler ultimately converts the query syntax to.

```cs
var filteredItems = myItems.Where(item => item.Foo);

```
Query syntax and method syntax are semantically identical, but many people find query syntax simpler
and easier to read. Some queries must be expressed as method calls. For example, you must use a method
call to express a query that retrieves the number of elements that match a specified condition.
You also must use a method call for a query that retrieves the element that has the maximum value
in a source sequence.

In addition, the query syntax allows for the use the `let` clause, which allows you to introduce and bind a
variable within the scope of the expression and then using it in subsequent pieces of the expression. Reproducing
the same code with only the API syntax can be done but the code generated can be more difficult to read.

An example of use of the the `let` clause is as follows
```c#
	string[] strings = 
	{
	    "A penny saved is a penny earned.",
	    "The early bird catches the worm.",
	    "The pen is mightier than the sword." 
	};
	
	// Split the sentence into an array of words
	// and select those whose first letter is a vowel.
	var earlyBirdQuery =
	    from sentence in strings
	    let words = sentence.Split(' ')
	    from word in words
	    let w = word.ToLower()
	    where w[0] == 'a' || w[0] == 'e'
	        || w[0] == 'i' || w[0] == 'o'
	        || w[0] == 'u'
	    select word;
	
	// Execute the query.
	foreach (var v in earlyBirdQuery)
	{
	    Console.WriteLine("\"{0}\" starts with a vowel", v);
	}
```

## When should you use the query syntax or the API/method syntax?
Ultimately the answer is which-ever way results in your writing your code quickly and in the most
maintainable way.

Consider use of the query syntax when:
 - You need to scope variables within your queries due to complexity
 - Your existing codebase already uses the query syntax
 - You prefer the query syntax and it won’t distract from your codebase

Consider the use of the API/method syntax when

 - You have no need to scope variables within your queries
 - Your existing codebase already uses the API syntax
 - You prefer the API syntax and it won’t distract from your codebase

Ultimately the query syntax is broken down into method/api calls so there is no performance difference, it
is purely about code readability and matintainability.

## LINQ Samples

For a truly comprehensive list of LINQ samples, visit [101 LINQ Samples](https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b).
You can also see the same samples using the API syntax along with extensive use of lambda expressions on
[Frode Nilsen's Blog](http://linq101.nilzorblog.com/linq101-lambda.php)  

### Use of `Where`, `Select`, and `Aggregate`

```cs
// Filtering a list of dogs to get the German Shepards
var germanShepards = dogs.Where(dog => dog.Breed == DogBreed.GermanShepard);

// The same, using the query syntax
var queryGermanShepards = from dog in dogs
                          where dog.Breed == DogBreed.GermanShepard
                          select dog;

// Use the method of dog called TurnIntoACat to turn a list of dogs into cats
var cats = dogs.Select(dog => dog.TurnIntoACat());

// Using the query syntax
var queryCats = from dog in dogs
                select dog.TurnIntoACat();

// Sum the lengths of a list of strings using Aggregate
int seed = 0;
int sumOfStrings = strings.Aggregate(seed, (s1, s2) => s1.Length + s2.Length);
```

### Union between two sets (using a custom comparator):

```cs
// Define the logic to be used to determine if two breeds of dog have
// equal hair length
public class DogHairLengthComparer : IEqualityComparer<Dog>
{
    public bool Equals(Dog a, Dog b)
    {
        if (a == null && a == null)
        {
            return true;
        }
        else if ((a == null && b != null) ||
                 (a != null && b == null))
        {
            return false;
        }
        else
        {
            return a.HairLengthType == b.HairLengthType;
        }
    }

    public int GetHashCode(Dog d)
    {
        // default hashcode is enough here, as these are simple objects.
        return b.GetHashCode();
    }
}

...

// Now get all the short-haired dogs living in either of two different kennels
var allShortHairedDogs = kennel1.Dogs.Union(kennel2.Dogs, new DogHairLengthComparer())
                                     .Where(dog => dog.HairLengthType = DogHairLengthType.Short);

```

*   Intersection between two sets:

```cs
// Get the list of volunteers who give time at both charity1 and charity2.
var volunteers = charity1.Volunteers.Intersect(charity2.Volunteers,
                                                     new VolunteerTimeComparer());
```

*   Ordering:

```cs
// Return a collection of driving directions but put the routes without tolls first even if
// they are slower.
var results = DirectionsProcessor.GetDirections(start, end)
              .OrderBy(direction => direction.HasNoTolls)
              .ThenBy(direction => direction.EstimatedTime);

```
Note that `OrderBy` is ascending. There is also an equivalent `OrderByDescending` method

## PLINQ

PLINQ, or Parallel LINQ, is a parallel execution engine for LINQ expressions. In other words it allows a regular 
LINQ expression to be trivially parallelized across any number of threads. This is accomplished via a
call to `AsParallel()` preceding the expression.

Consider the following:

```cs
public static string GetAllFacebookUserLikesMessage(IEnumerable<FacebookUser> facebookUsers)
{
    var seed = default(UInt64);

    Func<UInt64, UInt64, UInt64> threadAccumulator = (t1, t2) => t1 + t2;
    Func<UInt64, UInt64, UInt64> threadResultAccumulator = (t1, t2) => t1 + t2;
    Func<Uint64, string> resultSelector = total => $"Facebook has {total} likes!";

    return facebookUsers.AsParallel()
                        .Aggregate(seed, threadAccumulator, threadResultAccumulator, resultSelector);
}

```

This code will partition `facebookUsers` across system threads as necessary, sum up the total 
likes on each thread in parallel, sum the results computed by each thread, and project that result
into a human readable string.

Parallelizable CPU-bound jobs which can be easily expressed via LINQ (in other words, ones that are pure functions
and have no side effects) are a great candidate for PLINQ. For jobs which _do_ have a side effect, 
consider using the [Task Parallel Library](https://msdn.microsoft.com/en-us/library/dd460717.aspx).

## Exercises
   1. Take the following paragraph and select the words that do not contain an 'e' `A penny saved is a penny earned. The early bird catches the worm. The pen is mightier than the sword."`
   2. Sort the words in the paragraph displaying the short ones first
   3. Take the list of people from Tutorial 4 and display them in age order, youngest first.
   4. Display the name of the oldest person in the list.
 
## Summary
In this tutorial you learned about the LINQ syntax introduced to C# 3.0 and saw how the same syntax is able to be
used to query any `Enumerable` data source. You learned about the different ways of specifying a query and when
one might be more expressive than another.

## Additional Information
 - [System.Linq Enumerable Methods](https://msdn.microsoft.com/library/system.linq.enumerable_methods.aspx)
 - [101 LINQ Samples](https://code.msdn.microsoft.com/101-LINQ-Samples-3fb9811b)
 - [Linqpad](https://www.linqpad.net/), a playground environment and Database querying engine for C#/F#/VB
 - [EduLinq](http://codeblog.jonskeet.uk/2011/02/23/reimplementing-linq-to-objects-part-45-conclusion-and-list-of-posts/), an e-book for learning how LINQ-to-objects is implemented


---
 - Next: [Tutorial 7 - Asynchronous Programming](../007-Async/)
 - Previous: [Tutorial 5 - Delegates and Lambda Expressions](../005-Lambdas/)
 - Back to [Table of Contents](../README.md)

