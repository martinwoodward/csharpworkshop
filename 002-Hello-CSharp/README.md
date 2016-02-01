# Hello C# World

This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Before you start you want to make sure that you have .NET Core installed and configured in your path
```
 dotnet --version
```
Assuming the command above works, you are good to go!  If not, please see 
[Tutorial 1 - Getting Started with C# on Linux](../001-Getting-Started/).

## My First C# Program

Now you have [.NET Core installed](../001-Getting-Started/), let's create our first C# program.
To begin we need to create a folder to store our C# code project in, i.e.
```
 mkdir ~/source/dotnet/002-hello
 cd ~/source/dotnet/002-hello
```
Then we want to create a new C# project in it
```
 dotnet new
```
This will create the following files in that directory
 - *Program.cs* - the actual program, with a basic Hello World sample ready to run
 - *project.json* - a meta-data file that describes the entire .NET project to the compiler 
   and other tooling.
 - *NuGet.Config* - the configuration of [NuGet](http://nuget.org) (the .NET package manager) 
   for your project

## Getting ready to run
Now we have an application, the next thing we have to do it tell NuGet to download the dependencies
we need for our project. 

### **NOTE - Temporary Hack for RHEL / Fedora Bug**
At the time of writing, the build for Fedora based distributions has a bug. To work-around this to
be able to run your new application you need to do the following:
```
 dotnet restore --runtime "centos.7-x64"
 sed -ie 's:DNXCore,Version=v5.0/centos.7-x64:DNXCore,Version=v5.0/rhel.7.2-x64:g' project.lock.json
``` 

If you are not on RHEL / Fedora then you can download the dependencies from NuGet by simply doing
```
 dotnet restore
```

## Compiling and running your project
Then to build and run your application, simply type
```
 dotnet run
```
That's all there is to it. Now let's take a look at `Program.cs` and make some changes.

## The Structure of a C# program
Open `Program.cs` in your favorite editor and take a look at the contents. After you ran
`dotnet new` the following file would have been created
```c#
using System;

namespace ConsoleApplication
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");
        }
    }
}
```
### Using Directive
The first line you see is the `using` directive. `using` is similar to `import` in other languages like Java, Python or Go.
It tells the C# compiler where to find
the classes that you want to call. In the example above, the full name of `Console` is actually
`System.Console` but the `using System` directive means that the compiler automatically looks
for the `Console` class with-in the `System` namespace.

### Namespaces
A `namespace` is a way of grouping classes together in a way that limits their scope and avoids name
conflicts. This is similar to `package` in Java however in C# while it is convention to have a hirerarchy
of namespaces represented as a tree structure in a directory, you don't have to. A single `.cs` file
could declare multiple classes in multiple namespaces - though that would be a little bit evil.

### Main()
A C# program must contain a `public static` method called `Main` which controls the start and end of 
the process and there can only be one `Main` method per project. When the program is executed
the .NET runtime will look for the class containing the 'Main' method and execute it. 

You can declare the Main method in two ways:
```c#
    public static void Main(string[] args)
    {
        // Do Something
    }
```
Which would always return a `0` exit code, or if you want to have control of the exit codes
returned by your application you can declare your exit code explicitly by indicating that your
`Main` method returns an `int` and then returning an integer in your `Main` method.
```c#
    public static int Main(string[] args)
    {
        // Do Something
        return 0;
    }
```
You can run the code above and verify the exit code returned by typing
```
 dotnet run
 echo $?
```
## Comments
Note in the example above, we are showing comments.  In common with many languages
C# supports C style comments, i.e
```c#
 int x = 1; // Single line comment
 /* You can always
  * spread you comments out over many lines
  */
```

## Exercises

 1. Create a new .NET Project and run it.
 2. Modify the program to change the message it returns and run it.
 3. Modify the program to return an exit code of 1.
 
## Conclusion
In this tutorial we learned how to create a new C# application, the structure of a basic C# 
program, how to run the application and how to change the exit code of the process.

## Additional Information
 - [Using directive in C#](https://msdn.microsoft.com/en-us/library/sf0df423.aspx)
 - [Namespaces](https://msdn.microsoft.com/en-us/library/zz9ayh33.aspx)
 - [System.Console](https://msdn.microsoft.com/en-us/library/system.console.aspx)
 - [Main() and Command-Line Arguments](https://msdn.microsoft.com/en-us/library/acy3edy3.aspx)
 - [NuGet](https://docs.nuget.org/)
 - [project.lock.json](https://github.com/aspnet/Home/wiki/Lock-file)

---
 - Next: TODO
 - Previous: [Tutorial 1 - Getting Started with C# on Linux](../001-Getting-Started/)
 - Back to [Table of Contents](../)

