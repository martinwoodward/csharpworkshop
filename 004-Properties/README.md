# Properties

This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Author: [Bill Wagner](https://github.com/BillWagner)

Properties are first class citizens in C#. The language
defines syntax that enables developers to write code
that accurately expresses their design intent.

Properties behave like fields when they are accessed.
However, unlike fields, properties are implemented
with accessors that define the statements executed
when a property is accessed or assigned.

## Property Syntax
The syntax for properties is a natural extension to
fields. A field defines a storage location:

```cs
public class Person
{
	public string FirstName;
	// remaining implementation removed from listing
}
```

A property definition contains declarations for a `get` and
`set` accessor that retrieves and assigns the value of that
property:

```cs
public class Person
{
	public string FirstName
	{
		get;
		set;
	}
	// remaining implementation removed from listing
}
```

The syntax shown above is the *auto property* syntax. The compiler
generates the storage location for the field that backs up the 
property. The compiler also implements the body of the `get` and `set` accessors.
You can also define the storage yourself, as shown below:

```cs
public class Person
{
	public string FirstName
	{
		get { return firstName; }
		set { firstName = value; }
	}
	private string firstName;
	// remaining implementation removed from listing
}
```
 
The property definition shown above is a read-write property. Notice
the keyword `value` in the set accessor. The `set` accessor always has
a single parameter named `value`. The `get` accessor must return a value
that is convertible to the type of the property (`string` in this example).
 
That's the basics of the syntax. There are many different variations that support
a variety of different design idioms. Let's explore those, and learn the syntax
options for each. 

## Scenarios

The examples above showed one of the simplest cases of property definition:
a read-write property with no validation. By writing the code you want in the
`get` and `set` accessors, you can create many different scenarios.  

### Validation

You can write code in the `set` accessor to ensure that the values represented
by a property are always valid. For example, suppose one rule for the `Person`
class is that the name cannot be blank, or whitespace. You would write that as
follows:

```cs
public class Person
{
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name must not be blank");
            firstName = value;
        }
    }
    private string firstName;
    // remaining implementation removed from listing
}
```

The example above enforces the rule that the first name must not be blank,
or whitespace. If a developer writes
```cs
foo.FirstName = "";
```
That assignment throws an `ArgumentException`. Because a property set accessor
must have a void return type, you report errors in the set accessor by throwing an exception.

That is a simple case of validation. You can extend this same syntax to anything needed
in your scenario. You can check the relationships between different properties, or validate
against any external conditions. Any valid C# statements are valid in a property accessor.

### Read-only

Up to this point, all the property definitions you have seen are read/write properties
with public accessors. That's not the only valid accessibility for properties.
You can create read-only properties, or give different accessibility to the set and get
accessors. Suppose that your `Person` class should only enable changing the value of the
`FirstName` property from other methods in that class. You could give the set accessor
`private` accessibility instead of `public`:

```cs
public class Person
{
	public string FirstName
	{
		get;
		private set;
	}
	// remaining implementation removed from listing
}
```

Now, the `FirstName` property can be accessed from any code, but it can only be assigned
from other code in the `Person` class.
You can add any restrictive access modifier to either the set or get accessors. Any access modifier
you place on the individual accessor must be more limited than the access modifier on the property
definition. The above is legal because the `FirstName` property is `public`, but the set accessor is
`private`. You could not declare a `private` property with a `public` accessor. Property declarations
can also be declared `protected`, `internal`, `protected internal` or even `private`.   

It is also legal to place the more restrictive modifier on the `get` accessor. For example, you could
have a `public` property, but restrict the `get` accessor to `private`. That scenario is rarely done
in practice.
 
### Computed Properties

A property does not need to simply return the value of a member field. You can create properties
that return a computed value. Let's expand the `Person` object to return the full name, computed
by concatenating the first and last names:

```cs
public class Person
{
    public string FirstName
    {
        get;
        set;
    }

    public string LastName
    {
        get;
        set;
    }

    public string FullName
    {
        get
        {
            return $"{FirstName} {LastName}";
        }
    }
}
```

The example above uses the *String Interpolation* syntax to create
the formatted string for the full name.

You can also use *Expression Bodied Members*, which provides a more
succinct way to create the computed `FullName` property:

```cs
public class Person
{
    public string FirstName
    {
        get;
        set;
    }

    public string LastName
    {
        get;
        set;
    }

    public string FullName =>  $"{FirstName} {LastName}";
}
```
 
*Expression Bodied Members* use the *lambda expression* syntax to
define a method that contain a single expression. Here, that
expression returns the full name for the person object.

### Lazy Evaluated Properties

You can mix the concept of a computed property with storage and create
a *lazy evaluated property*.  For example, you could update the `FullName`
property so that the string formatting only happened the first time it
was accessed:

```cs
public class Person
{
    public string FirstName
    {
        get;
        set;
    }

    public string LastName
    {
        get;
        set;
    }

    private string fullName;
    public string FullName
    {
        get
        {
            if (fullName == null)
                fullName = $"{FirstName} {LastName}";
            return fullName;
        }
    }
}
```

The above code contains a bug though. If code updates the value of
either the `FirstName` or `LastName` property, the previously evaluated
`fullName` field is invalid. You need to update the `set` accessors of the
`FirstName` and `LastName` property so that the `fullName` field is calculated
again:

```cs
public class Person
{
    private string firstName;
    public string FirstName
    {
        get { return firstName; }
        set
        {
            firstName = value;
            fullName = null;
        }
    }

    private string lastName;
    public string LastName
    {
        get { return lastName; }
        set
        {
            lastName = value;
            fullName = null;
        }
    }

    private string fullName;
    public string FullName
    {
        get
        {
            if (fullName == null)
                fullName = $"{FirstName} {LastName}";
            return fullName;
        }
    }
}
```

This final version evaluates the `FullName` property only when needed.
If the previously calculated version is valid, it's used. If another
state change invalidates the previously calculated version, it will be
recalculated. Developers that use this class do not need to know the
details of the implementation. None of these internal changes affect the
use of the Person object. That's the key reason for using Properties to
expose data members of an object. 
 
### INotifyPropertyChanged

A final scenario where you need to write code in a property accessor is to
support the `INotifyPropertyChanged` interface used to notify data binding
clients that a value has changed. When the value of a property changes, the object
raises the `PropertyChanged` event
to indicate the change. The data binding libraries, in turn, update display elements
based on that change. The code below shows how you would implement `INotifyPropertyChanged`
for the `FirstName` property of this person class.

```cs
public class Person : INotifyPropertyChanged
{
    public string FirstName
    {
        get { return firstName; }
        set
        {
            if (string.IsNullOrWhiteSpace(value))
                throw new ArgumentException("First name must not be blank");
            if (value != firstName)
            {
                PropertyChanged?.Invoke(this, 
                    new PropertyChangedEventArgs(nameof(FirstName)));
            }
            firstName = value;
        }
    }
    private string firstName;

    public event PropertyChangedEventHandler PropertyChanged;
    // remaining implementation removed from listing
}
```

The `?.` operator is called
the *null conditional operator*. It checks for a null reference before evaluating
the right side of the operator. The end result is that if there are no subscribers
to the `PropertyChanged` event, the code to raise the event doesn't execute. It would
throw a `NullReferenceException` without this check in that case. This example also uses the new
`nameof` operator to convert from the property name symbol to its text representation.
Using `nameof` can reduce errors where you have mistyped the name of the property.

Again, this is an example of a case where you can write code in your accessors to
support the scenarios you need.

## Exercises
 1. Create a class called `Person` with the following properties that must be defined
	 - First name
	 - Last name
	 - Email address
	 - Date of Birth
 2. Store 3 people in a list and print out their details to the console
 3. Add a read-only property that returns the following information
	 - Adult - a bool indicating if they are over 18 or not
	 - Age - their current age (in years)
 4.	Add a property called `UserID` which can be explicitly set but will default to a practical first time value 
    if not defined
 
## Conclusion
In this section we learnt about Properties in C# and the different ways to use them. We were also introduced to
null conditional operators and learned how to implement the `INotifyPropertyChanged` interface.

Properties are a form of smart fields in a class or object. From
outside the object, they appear like fields in the object. However,
properties can be implemented using the full palette of C# functionality.
You can provide validation, different accessibility, lazy evaluation,
or any requirements your scenarios need.

## Additional Information
 - [Lamda Expressions in C#](https://msdn.microsoft.com/en-gb/library/bb397687.aspx)
 - [Null Conditional Operators in C#](https://msdn.microsoft.com/en-us/library/dn986595.aspx)
 - [Events and Delegates](https://msdn.microsoft.com/en-gb/library/17sde2xt.aspx)
 - [`INotifyPropertyChanged` interface](https://msdn.microsoft.com/en-us/library/system.componentmodel.inotifypropertychanged.aspx)
 - [`nameof` operator](https://msdn.microsoft.com/en-us/library/dn986596.aspx)

---
 - Next: [Tutorial 5 - Delegates and Lambda Expressions](../005-Lambdas/)
 - Previous: [Tutorial 1 - C# Language Basics](../003-Language-Basics/)
 - Back to [Table of Contents](../README.md)

