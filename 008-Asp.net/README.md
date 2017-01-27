# Web Programming with ASP.NET Core
This file is part of a multi-series workshop on learning C# on Linux available [here](../README.md).

Author: [Bill Wagner](https://github.com/BillWagner), [Martin Woodward](https://github.com/martinwoodward)

## Getting Started
The easiest way to get started with a new ASP.NET Core app is to go into a new directory and then use the `dotnet new -t web`
command to specify that you want to create a new application with the web template.
```
dotnet new -t web
dotnet restore
dotnet run
```

By default, a new ASP.NET application listens on port 5000. Therefore if you open a browser on http://localhost:5000 you should be
able to see your new website.

## Web Applications Basics
The first thing to understand is that in .NET Core, and ASP.NET web application is just a console application.  If you open `Program.cs` you can examine the Main method to see how the application runs the WebHost.  You can easily modify this, for example if you want the web server to listen on port 8080 on all interfaces rather than just port 5000 on localhost you could modify your Main method as follows
```
public static void Main(string[] args)
{
    var host = new WebHostBuilder()
                .UseKestrel()
                .UseContentRoot(Directory.GetCurrentDirectory())
                .UseIISIntegration()
                .UseStartup<Startup>()
                .UseUrls("http://*:8080")
                .Build();
    host.Run();
}
```
If you take a look at `project.json` you'll also see that this contains much more information now. There are many more dependencies now, but also the publish options contain additional information on how the application should be prepared when performing a `dotnet publish` in order to deploy.

The `web.config` file contains basic configuration information for the web application and is parsed by framework libraries in ASP.NET.

We can also take a look at the additional code in `Startup.cs`. This startup code enables other features in ASP.NET that are part of this web application. Within the `Configure` method, there’s a call to `UseStaticFiles()`. This method enables delivery of static files like images or javascript files from the web server file system. Second, there’s a call to `UseMvc()`, which enables the MVC framework in this application:
```c#
app.UseMvc(routes =>
{
    routes.MapRoute(
        name: "default",
        template: "{controller=Home}/{action=Index}/{id?}");
});
```

The call to `MapRoute()` configures a mapping from the URL request to code in your application. The sample creates the default mapping. The template is how the MVC core classes map a URL to code. This route maps a URL of the form
`http://mywebapplication.com/Catalog/List/12` To a class, and a method with a single parameter.

The first portion of the URL after the host, `Catalog` defines the name of a Controller class. That maps to a class in the Controller directory of the default project. The default mapping defines the default controller as the `Home` controller. By convention, controller classes must end in the test `Controller`. The default controller is a class named `HomeController`.

The second portion of the route maps to a method name in that controller. The default method is a method named `Index`.
 
The final portion maps to a single argument, named `id`. The `?` defines that this argument is optional. If the argument is supplied, it would be converted to the type specified on the Controller method.

Next, let’s look at the new files created by this template. We’ll start with `gulpfile.js`. This file defines many of the build tasks related to client side assets (javascript, css files). As you build more ASP.NET projects, you’ll learn that ASP.NET uses standard web tools for many client side web development tasks.

The other major sections are the `Controllers` directory, and the `Views` directory. The controller directory has the `HomeController` class. The `HomeController` provides an example of an MVC Controller. A Controller is responsible for generating the response from an HTTP request. Notice that all the methods in the HomeController end with a call to `View()`. The `View()` method is a member of the Controller base class. This method generates the HTML response by looking for a View template, which we’ll discuss shortly. You can see the four methods that map to the four URLs that are configured in this application.

The `Views` folder contains the templates that generate the HTML views. The Views are HTML templates that are processed by the MVC Razor engine. The design enables a great deal of reuse in the HTML that you’ll use in a website. The View engine processes the view templates. The `Shared` sub-directory under the `Views` directory contains View templates that contain the HTML that is shared across all the pages in your application. In the `_Layout.cshtml` file, you’ll see the shared layout, with sections that are placeholders for the views for the specific pages. The views for each page are in a subdirectory of the same name as the controller (e.g. `Home`). The names of the view files match the name of the method in the controller (e.g. `Index`, `About`).
 
You can see that the MVC framework follows these conventions to make it easier to create an application with a minimum of extra configuration.

The remaining assets are the images and css styles that are part of the starter application.

## Yeoman Scaffolding
While the `dotnet` command has some basic templating features, and more are being added all the time you can also make use
of the popular Yeoman tool to help you scaffold and ASP.NET application.

You’ll need to install yeoman, bower, the grunt CLI and gulp. Then
install the ASP.NET website generator template to get started quickly
```
sudo npm install -g yo bower grunt-cli gulp
sudo npm install -g generator-aspnet
```

You can generated an ASP.NET core site by running the yeoman generator:
```
yo aspnet
```
This give you the option to build several different starter applications. One of the great features of the ASP.NET
ecosystem is that the tools generate boiler plate example code for many common scenarios. The downside of this boilerplate code is that it is easy to create an application that has code you don’t need for features that aren’t
intended for your site.
    
## Summary
In this tutorial we explored ASP.NET Core and examined the default starter template. For more information, [Jon Galloway has an extensive workshop on ASP.NET](https://github.com/jongalloway/aspnetcore-workshop).   

## Additional Information
 - [ASP.NET Workshop](https://github.com/jongalloway/aspnetcore-workshop)
 - [ASP.NET Documentation](https://docs.asp.net)
 - [Visual Studio Code](https://code.visualstudio.com/)
 - [OmniSharp](http://www.omnisharp.net/)

---
 - Previous: [Tutorial 7 - Asynchronous Programming](../007-Async/)
 - Back to [Table of Contents](../README.md)

