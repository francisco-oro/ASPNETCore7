# ASP.NET Core Web API Section Cheat Sheet (PPT)
## Introduction to Web API
ASP.NET Core Web API is a component (part) of ASP.NET Core, which is used create HTTP-based RESTful services (also known as HTTP services) that can be consumed (invoked) by wide range of client applications such as single-page web applications, mobile applications etc.



## Asp.Net Core:

- Asp.Net Core MVC

- Asp.Net Core Web API

- Asp.Net Core Blazor

- Asp.Net Core Razor Pages







## RESTful / Web API Services
RESTful services (Representational State Transfer) is an architecture style that defines to create HTTP services that receives HTTP GET, POST, PUT, DELETE requests; perform CRUD operations on the appropriate data source; and returns JSON / XML data as response to the client.

![RESTful_web_api_services](assets/RESTful_web_api_services.png)








## Web API Controllers
### Should be either or both:

- The class name should be suffixed with "Controller". Eg: ProductsController

- The [ApiController] attribute is applied to the same class or to its base class.



### Controller
```c#
[ApiController]
class ClassNameController
{
   //action methods here
}
```

### Optional:

- Is a public class.

- Inherited from Microsoft.AspNetCore.Mvc.ControllerBase.







## Introduction to EntityFrameworkCore
EntityFrameworkCore is light-weight, extensible and cross-platform framework for accessing databases in .NET applications.

It is the most-used database framework for Asp.Net Core Apps.

![entityframeworkcore](assets/entityframeworkcore.png)





### EFCore Models

![efcore_models](assets/efcore_models.png)



## Pros & Cons of EntityFrameworkCore
### 1. Shorter Code

The CRUD operations / calling stored procedures are done with shorter amount of code than ADO.NET.



### 2. Performance

EFCore performs slower than ADO.NET.

So ADO.NET or its alternatives (such as Dapper) are recommended for larger & high-traffic applications.



### 3. Strongly-Typed

The columns as created as properties in model class.



So the Intellisense offers columns of the table as properties, while writing the code.

Plus, the developer need not convert data types of values; it's automatically done by EFCore itself.







## ProblemDetails
### ProblemDetails
```c#
public class ProblemDetails
{
   string? Type { get; set; } //URI references that identifies the problem type
   string? Title { get; set; } //Summary of the problem type
   int? Status { get; set; } //HTTP response status code
   string? Detail { get; set; } //Explanation of the problem
}
```

### ValidationProblemDetails
```c#
public class ValidationProblemDetails : ProblemDetails
{
   string? Type { get; set; } //URI references that identifies the problem type
   string? Title { get; set; } //Summary of the problem type
   int? Status { get; set; } //HTTP response status code
   string? Detail { get; set; } //Explanation of the problem
   IDictionary<string, string[]> Errors { get; set; } //List of validation errors
}
```





## IActionResult [vs] ActionResult
### IActionResult
```c#
public interface IActionResult
{
   Task ExecuteResultAsync(ActionContext context); //converts an object into response
}
```

### ActionResult<T>
```c#
public sealed class ActionResult<T>
{
   IActionResult Convert(); //converts the object into ObjectResult
}
```





## IActionResult
![iactionresult](assets/iactionresult.png)



## ObjectResult

![objectresult](assets/objectresult.png)

# Interview Questions

## What is ASP.NET Web API?
ASP.NET Web API is a framework provided by Microsoft for building HTTP services that can be consumed by various clients, such as web browsers, mobile applications, and desktop applications. It allows developers to create RESTful APIs using the .NET platform.



## How do you define a Web API controller in ASP.NET Web API?
To define a Web API controller, you need to create a new class that inherits from the ControllerBase class. This class represents an HTTP service and contains action methods that handle different HTTP requests.
## Explain the basic syntax of a Web API controller.
A Web API controller consists of a class that inherits from the ControllerBase class and contains action methods. Action methods are public methods decorated with attributes such as [HttpGet], [HttpPost], [HttpPut], or [HttpDelete], depending on the HTTP verb they handle. These methods are responsible for processing incoming requests and returning responses.
## What are Action Results in ASP.NET Web API?
Action Results in ASP.NET Web API represent the result returned by an action method. They encapsulate the data that needs to be sent back to the client. Action Results derive from the IActionResult interface.

## What is the difference between IActionResult and ActionResult<T>?
IActionResult is an interface that represents the result of an action method, whereas ActionResult<T> is a generic class that derives from IActionResult and is used when you want to return a specific type of data as the result of an action method. For example, ActionResult<string> can be used to return a string result.
## Explain the usage of the ProblemDetails class in ASP.NET Web API.
The ProblemDetails class is a part of the ASP.NET Core framework and is used to provide consistent error responses from a Web API. It allows you to return structured error information in the form of a JSON response, including details like the error message, status code, and additional properties.
## Can you create a custom base class for Web API controllers? If so, how?
Yes, you can create a custom base class for Web API controllers. To do this, you need to create a new class that derives from the ControllerBase class. This custom base class can contain common functionality, utility methods, or custom behavior that you want to share across multiple Web API controllers.


## How do you integrate Entity Framework Core with ASP.NET Web API?
To use Entity Framework Core (EF Core) with ASP.NET Web API, you need to follow these steps:

- Install the required NuGet packages for EF Core.

- Define your data model classes as entities.

- Create a DbContext class that represents the database context.

- Configure the DbContext and entity relationships.

- Inject the DbContext into your Web API controllers or services using dependency injection.

- Use the DbContext in your controllers or services to perform database operations such as querying, inserting, updating, or deleting data.

## How can you return data from EF Core queries in Web API controller actions?
You can return data from EF Core queries in Web API controller actions by using the Ok method of the ControllerBase. For example, you can use return Ok(data) to return an HTTP 200 OK response along with the data retrieved from the database.


## What is the purpose of the ActionResult class in Web API?
The ActionResult class is the base class for all action results in Web API. It provides a convenient way to create and return different types of HTTP responses. It encapsulates both the data and the HTTP status code that should be returned to the client.

