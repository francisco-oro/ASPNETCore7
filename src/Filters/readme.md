# Filters Section Cheat Sheet
## Filters
Filters are the code blocks that execute before / after specific stages in "Filter Pipeline".

Filters perform specific tasks such as authorization, caching, exeption handling etc.

![filters](assets/filters.png)


### Filter Pipeline


![filter_pipeline](assets/filter_pipeline.png)




## Overview of Types of Filters
### Authorization Filter

Determines whether the user is authorized to access the action method.



### Resource Filter

Invoking custom model binder explicitly

Caching the response.



### Action Filter

Manipulating & validating the action method parameters.

Manipulating the ViewData.

Overriding the IActionResult provided by action method.



### Exception Filter

Handling unhandled exception that occur in model binding, action filters or action methods.



### Result Filter

Preventing IActionResult from execution.

Adding last-moment changes to response (such as adding response headers).





## Action Filter
![actionfilter](assets/actionfilter.png)

### When it runs

Runs immediately before and after an action method executes.



### 'OnActionExecuting' method

- It can access the action method parameters, read them & do necessary manipulations on them.

- It can validate action method parameters.

- It can short-circuit the action (prevent action method from execution) and return a different IActionResult.



### 'OnActionExecuted' method

- It can manipulate the ViewData.

- It can change the result returned from the action method.



## Filter Arguments
![filterarguments](assets/filterarguments.png)



## Global Filters
### Filter Scopes

![filterscopes](assets/filterscopes.png)



### What are global filters?

Global filters are applied to all action methods of all controllers in the project.



### How to add global filters in Program.cs?
```c#
builder.Services.AddControllersWithViews(options => {
  options.Filters.Add<FilterClassName>(); //add by type
  //or
  options.Filters.Add(new FilterClassName()); //add filter instance
});
```

## Default Order of Filter Execution


![default_order_of_filter_execution](assets/default_order_of_filter_execution.png)



## Custom Order of Filters

![custom_order_of_filters](assets/custom_order_of_filters.png)





## IOrderedFilter
### Example

![iorderedfilter](assets/iorderedfilter.png)





## IOrderedFilter
### Action filter with IOrderedFilter
```c#
public class FilterClassName : IActionFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
 
  public void OnActionExecuting(ActionExecutingContext context)
  {
    //TO DO: before logic here
  }
 
  public void OnActionExecuted(ActionExecutedContext context)
  {
    //TO DO: after logic here
  }
}

```


## Async Filters
### Asynchronous Action Filter
```c#
public class FilterClassName : IAsyncActionFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
 
  public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
  {
    //TO DO: before logic here
    await next();
    //TO DO: after logic here
  }
}
```



## Short-circuiting Filters
### Action Filters

### When it runs

Runs immediately before and after an action method executes.



### 'OnActionExecuting' method

- It can access the action method parameters, read them & do necessary manipulations on them.

- It can validate action method parameters.

- It can short-circuit the action (prevent action method from execution) and return a different IActionResult.



### 'OnActionExecuted' method

- It can manipulate the ViewData.

- It can change the result returned from the action method.

- It can throw exceptions to either return the exception to the exception filter (if exists); or return the error response to the browser.





## Short-Circuiting Action Filter
```c#
public class FilterClassName : IAsyncActionFilter, IOrderedFilter
{
 public int Order { get; set; } //Defines sequence of execution
 
 public FilterClassName(int order)
 {
  Order = order;
 }
 
 public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
 {
  //TO DO: before logic here
  context.Result = some_action_result; //you can return any type of IActionResult
  //Not calling next(). So it leads remaining filters & action method short-circuited.
 }
}
```


## Short-Circuiting (exit) the filters



![short-cirucuiting_the_filters](assets/short-cirucuiting_the_filters.png)


## Result Filter

![result_filter](assets/result_filter.png)


### When it runs

- Runs immediately before and after an IActionResult executes.

- It can access the IActionResult returned by the action method.



### 'OnResultExecuting' method

- It can continue executing the IActionResult normally, by not assigning "Result" property of the context.

- It can short-circuit the action (prevent IActionResult from execution) and return a different IActionResult.



### 'OnResultExecuted' method

- It can manipulate the last-moment changes in the response, such as adding necessary response headers.

- It should not throw exceptions because, exceptions raised in result filters would not be caught by the exception filter.



#### Synchronous Result Filter
```c#
public class FilterClassName : IResultFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
  public void OnResultExecuting(ResultExecutingContext context)
  {
    //TO DO: before logic here
  }
  public void OnResultExecuted(ResultExecutedContext context)
  {
    //TO DO: after logic here
  }
}
```

#### Asynchronous Result Filter
```c#
public class FilterClassName : IAsyncResultFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
 
  public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
  {
  //TO DO: before logic here
  await next();
  //TO DO: after logic here
  }
}
```



## Resource Filter
![resource_filter](assets/resource_filter.png)



### When it runs

Runs immediately after Authorize Filter and after Result Filter executes.



### 'OnResourceExecuting' method

- It can do some work before model binding. Eg: Adding metrics to an action method.

- It can change the way how model binding works (invoking a custom model binder explicitly).

- It can short-circuit the action (prevent IActionResult from execution) and return a different IActionResult.

- Eg: Short-circuit if an unsupported content type is requested.



### 'OnResourceExecuted' method

It can read the response body and store it in cache.



### Synchronous Resource Filter
```c#
public class FilterClassName : IResourceFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
 
  public void OnResourceExecuting(ResourceExecutingContext context)
  {
    //TO DO: before logic here
  }
 
  public void OnResourceExecuted(ResourceExecutedContext context)
  {
    //TO DO: after logic here
  }
}
```

### Asynchronous Resource Filter
```c#
public class FilterClassName : IAsyncResourceFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
 
  public FilterClassName(int order)
  {
    Order = order;
  }
 
  public async Task OnResourceExecutionAsync(ResourceExecutingContext context, ResourceExecutionDelegate next)
  {
    //TO DO: before logic here
    await next();
    //TO DO: after logic here
  }
}
```



## Authorization Filter

![authorization_filter](assets/authorization_filter.png)


### Authorization Filters

Runs before any other filters in the filter pipeline.



### 'OnAuthorize' method

- Determines whether the user is authorized for the request.

- Short-circuits the pipeline if the request is NOT authorized.

- Don't throw exceptions in OnAuthorize method, as they will not be handled by exception filters.



### Synchronous Authorization Filter
```c#
public class FilterClassName : IAuthorizationFilter
{
  public void OnAuthorization(AuthorizationFilterContext context)
  {
    //TO DO: authorization logic here
  }
}
```

### Asynchronous Authorization Filter
```c#
public class FilterClassName : IAsyncAuthorizationFilter
{
  public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
  {
    //TO DO: authorization logic here
  }
}

```


## Exception Filter


![exception_filter](assets/exception_filter.png)

### When it runs

Runs when an exception is raised during the filter pipeline.



### 'OnException method

- Handles unhandled exceptions that occur in controller creation, model binding, action filters or action methods.

- Doesn't handle the unhandled exceptions that occur in authorization filters, resource filters, result filters or IActionResult execution.

- Recommended to be used only when you want a different error handling and generate different result for specific controllers; otherwise, ErrorHandlingMiddleware is recommended over Exception Filters.



### Synchronous Exception Filter
```c#
public class FilterClassName : IAsyncExceptionFilter
{
  public async Task OnExceptionAsync(ExceptionFilterContext context)
  {
    //TO DO: exception handling logic here, as follows
    context.Result = some_action_result;
    //or
    context.ExceptionHandled = true;
    return Task.CompletedTask;
  }
}
```

### Asynchronous Exception Filter
```c#
public class FilterClassName : IAsyncExceptionFilter
{
  public async Task OnExceptionAsync(ExceptionFilterContext context)
  {
    //TO DO: exception handling logic here, as follows
    context.Result = some_action_result;
    //or
    context.ExceptionHandled = true;
    return Task.CompletedTask;
  }
}
```



### Impact of Short-Circuiting
### Short-circuiting Authorization Filter

![short-circuiting_authorization_filter](assets/short-circuiting_authorization_filter.png)



### Short-circuiting Resource Filter

![short-circuiting_resource_filter](assets/short-circuiting_resource_filter.png)



### Short-circuiting Action Filter


![short-circuiting_action_filter](assets/short-circuiting_action_filter.png)


### Short-circuiting Exception Filter


![short-circuiting_exception_filter](assets/short-circuiting_exception_filter.png)


### Short-circuiting Result Filter


![short-circuiting_result_filter](assets/short-circuiting_result_filter.png)


### Short-circuiting the filters

![short-circuiting_the_filters_1](assets/short-circuiting_the_filters_1.png)
![short-circuiting_the_filters_2](assets/short-circuiting_the_filters_2.png)
![short-circuiting_the_filters_3](assets/short-circuiting_the_filters_3.png)









## AlwaysRun Result Filter
### Short-circuiting Authorization Filter

![short-circuiting_authorization_filter_1](assets/short-circuiting_authorization_filter_1.png)



### Short-circuiting Resource Filter

![short-circuiting_resource_filter_1](assets/short-circuiting_resource_filter_1.png)



### Short-circuiting Action Filter

![short-circuiting_action_filter_1](assets/short-circuiting_action_filter_1.png)



### Short-circuiting Exception Filter

![short-circuiting_exception_filter_1](assets/short-circuiting_exception_filter_1.png)



### Short-circuiting Result Filter
![short-circuiting_result_filter_1](assets/short-circuiting_result_filter_1.png)




### When AlwaysRunResultFilter runs

Runs immediately before and after result filters.



### Result filters:

Doesn't execute when authorization filter, resource filter or exception filter short-circuits.



### AlwaysRunResult filter:

Execute always even when authorization filter, resource filter or exception filter short-circuits.



### 'OnResultExecuting' method

Same as Result filter



### 'OnResultExecuted' method

Same as Result filter





### Synchronous Always Run Result Filter
```c#
public class FilterClassName : IAlwaysRunResultFilter
{
  public void OnResultExecuting(ResultExecutingContext context)
  {
    //TO DO: before logic here
  }
  public void OnResultExecuted(ResultExecutedContext context)
  {
    //TO DO: after logic here
  }
}
```



### Asynchronous Always Run Result Filter
```c#
public class FilterClassName : IAsyncAlwaysRunResultFilter
{
  public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
  {
   //TO DO: before logic here
   await next();
   //TO DO: after logic here
  }
}
```

## Filter Overrides
```c#
[TypeFilter(typeof(FilterClassName))] //filter applied at controller level
public class ControllerName : Controller
{
  public IActionResult Action1() //requirement: The filter SHOULD execute
  {
  }
  public IActionResult Action2() //requirement: The filter SHOULD NOT execute. But how??
  {
  }
}
```

### Attribute to be applied to desired action method
```c#
public class SkipFilterAttribute : Attribute, IFilterMetadata
{
}
```

### Action method

```c#
[SkipFilter]
public IActionResult ActionMethod()
{
}

```

### Filter that respects 'SkipFilterAttribute'

```c#
public class FilterClassName : IActionFilter //or any other filter interface
{
  public void OnActionExecuting(ActionExecutingContext context)
  {
    //get list of filters applied to the current working action method
    if (context.Filters.OfType<SkipResultFilter>().Any())
    {
      return;
    }
    //TO DO: before logic here
  }
 
  public void OnActionExecuted(ActionExecutedContext context)
  {
    //TO DO: after logic here
  }
}
```
It skips execution of code of a filter, for specific action methods.







## [ServiceFilter]
### Common purpose

Both are used to apply a filter a controller or action method.



### Type Filter Attribute
```c#
//can supply arguments to filter
[TypeFilter(typeof(FilterClassName), Arguments = new object[] { arg1, arg2 })]
public IActionResult ActionMethod()
{
  �
}
```

### Service Filter Attribute
```c#
//can't supply arguments to filter
[ServiceFilter(typeof(FilterClassName))]
public IActionResult ActionMethod()
{
  �
}
```



### Type Filter

- Can supply arguments to the filter.

- Filter instances are created by using Microsoft.Extensions.DependencyInjection. ObjectFactory.

- They're NOT created using DI (Dependency Injection).

- The lifetime of filter instances is by default transient (a new filter instance gets created every time when it is invoked).

- But optionally, you can re-use the same instance of filter class across multiple requests, by setting a property called TypeFilterAttribute.IsReusable to 'true'.

- Filter classes NEED NOT be registered (added) to the IoC container.

- Filter classes CAN inject services using both constructor injection or method injection.



### Service Filter

- Can't supply arguments to the filter.

- Filter instances are created by using ServiceProvider (using DI).

- The lifetime of filter instances is the actual lifetime of the filter class added in the IoC container.

- Eg: If the filter class is added to the IoC container with AddScoped() method, then its instances are scoped.

- Filter class SHOULD be registered (added) to the IoC container, much like any other service.

- Filter classes CAN inject services using both constructor injection or method injection.
  




### Filter attribtute classes
### IActionFilter [vs] ActionFilterAttribute

### [versus]

### Action filter that implements 'IActionFilter'
```c#
public class FilterClassName : IActionFilter, IOrderedFilter
{
  //supports constructor DI
}
```

### Action filter that inherits 'ActionFilterAttribute'
```c#
public class FilterClassName : ActionFilterAttribute
{
  //doesn't support constructor DI
}
```

### Filter interfaces:

- IAuthorizationFilter

- IResourceFilter

- IActionFilter

- IExceptionFilter

- IResultFilter

- IAsyncAuthorizationFilter

- IAsyncResourceFilter

- IAsyncActionFilter

- IAsyncExceptionFilter

- IAsyncResultFilter



### Filter attributes:

- ActionFilterAttribute

- ExceptionFilterAttribute

- ResultFilterAttribute



### Action filter that implements 'IActionFilter'
```c#
public class FilterClassName : IActionFilter, IOrderedFilter
{
  public int Order { get; set; }
 
  public FilterClassName(IService service, type arg)
  {
  }
 
  public void OnActionExecuting(ActionExecutingContext context)
  {
  }
 
  public void OnActionExecuted(ActionExecutedContext context)
  {
  }
}


[TypeFilter(typeof(FilterClassName),
Arguments = new object[] { arg1, � })]
```



### Action filter that inherits 'ActionFilterAttribute'
```c#
public class FilterClassName : ActionFilterAttribute
{
  public FilterClassName(type arg)
  {
  }
 
  public override void OnActionExecuting(ActionExecutingContext context)
  {
  }
 
  public override void OnActionExecuted(ActionExecutedContext context)
  {
  }
}
[FilterClassName(arg1, � )]

```



## Internal definitions of IActionFilter and ActionFilterAttribute
### IActionFilter
```c#
namespace Microsoft.AspNetCore.Mvc.Filters
{
  public interface IActionFilter : IFilterMetadata
  {
    void OnActionExecuting(ActionExecutingContext context);
    void OnActionExecuted(ActionExecutedContext context);
  }
}
```

### ActionFilterAttribute
```c#
namespace Microsoft.AspNetCore.Mvc.Filters
{
  public class ActionFilterAttribute : Attribute, IActionFilter, IAsyncActionFilter, IOrderedFilter, IResultFilter, IAsyncResultFilter
  {
    public virtual void OnActionExecuting(ActionExecutingContext context) { }
    public virtual void OnActionExecuted(ActionExecutedContext context) { }
    public virtual void OnResultExecuting(ActionExecutingContext context) { }
    public virtual void OnResultExecuted(ActionExecutedContext context) { }
    public virtual Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next) { }
    public virtual Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next) { }
    public int Order { get; set; }
  }
}
```



## Filter interface [vs] FilterAttribute class
### Filter interface [such as IActionFilter, IResultFilter etc.]

- Filter class MUST implement all methods - both "Executing" and "Executed" methods.

- Filter class CAN have DI with either constructor injection or method injection.

- Doesn't implement "Attribute" class.

- Filter should be applied to controller or action methods by using [ServiceFilter] or [TypeFilter] attributes; otherwise can be applied as global filter in the Program.cs.

- Eg: [TypeFilter(typeof(FilterClassName))] //lengthy

- Filter class can receive arguments only through constructor parameters; but only with [TypeFilter] attribute; not with [ServiceFilter] attribute.



### FilterAttribute class [such as ActionFilterAttribute etc.]

- Filter class MAY override desired (either or both methods - "Executing" and "Executed") methods.

- Filter class CAN'T have DI with neither constructor injection nor method injection.

- FilterAttribute class [such as ActionFilterAttribute etc.]

- Filter can be applied to controller or action methods by directly using the filter class name itself (without using [ServiceFilter] or [TypeFilter] attributes); otherwise can be applied as global filter in the Program.cs.

- Eg: [FilterClassName] //simple

- Filter class can receive arguments either through constructor parameters or filter class's properties.







## IFilterFactory
### Filter factory that inherits 'IFilterFactory'
```c#
public class FilterClassNameAttribute : Attribute,
IFilterFactory
{
  public type Prop1 { get; set; }
 
  public FilterClassName(type arg1, type arg2)
  {
    this.Prop1 = arg1; this.Prop2 = arg2;
  }
 
  public IFilterMetadata CreateInstance(IServiceProvider serviceProvider)
  {
    FilterClassName filter = serviceProvider.GetRequiredService<FilterClassName>(); //instantiate the filter
    filter.Property1 = Prop1;
    �
    return filter;
  }
}
[FilterClassName(arg1, arg2, � )]
```




### Action filter that inherits 'ActionFilterAttribute'
```c#
public class FilterClassName : ActionFilterAttribute
{
  public FilterClassName(type arg1, type arg2)
  {
  }
 
  public override void OnActionExecuting(ActionExecutingContext context)
  {
  }
 
  public override void OnActionExecuted(ActionExecutedContext context)
  {
  }
}
[FilterClassName(arg1, arg2, � )]
```



### IFilterFactory
```c#
namespace Microsoft.AspNetCore.Mvc.Filters
{
  public interface IFilterFactory : IFilterMetadata
  {
     IFilterMetadata CreateInstance(IServiceProvider serviceProvider);
     bool IsReusable { get; }
  }
}
```



### FilterAttribute class [such as ActionFilterAttribute etc.]

- Filter CAN be applied as an attribute to the controller or action method. Eg: [FilterClassName]

- Filter class CAN'T have DI with neither constructor injection nor method injection.

- Filter class CAN receive arguments either through constructor parameters or filter class's properties.



### IFilterFactory

- Filter CAN be applied as an attribute to the controller or action method. Eg: [FilterClassName]

- Filter class CAN have DI with either constructor injection or method injection.

- Filter class CAN receive arguments only through filter class's properties, if it is instantiated through ServiceProvider (using DI).



Alternatively, if you don't need to inject services using DI in the filter class; you can instantiate the filter class with 'new' keyword, in the CreateInstance() method of IFilterFactory; then the filter class can receive arguments either as constructor parameters or properties.





## Filters [vs] Middleware

![filters_vs_middleware](assets/filters_vs_middleware.png)

### Middleware

Middleware pipeline is a superset of Filter pipeline, which contains the full-set of all middlewares added to the ApplicationBuilder in the application's startup code (Program.cs).

Middleware pipeline execute for all requests.

Middleware handles application-level functionality such as Logging, HTTPS redirection, Performance profiling, Exception handling, Static files, Authentication etc., by accessing low-level abstractions such as HttpContext.



### Filter

Filter pipeline is a subset of Middleware pipeline which executes under "EndPoint Middleware".

In addition, filter pipeline executes for requests that reach "EndPoint Middleware".

Filters handle MVC-specific functionality such as manipulating or accessing ViewData, ViewBag, ModleState, Action result, Action parameters etc.

### Middleware Pipeline

![middleware_pipeline](assets/middleware_pipeline.png)

# Interview Questions 

## Explain different types of filters
Filters provide the capability to run the code before or after the specific stage in request processing pipeline, it could be either MVC app or Web API service. Filters performs the tasks like Authorization, Caching implementation, Exception handling etc. ASP.NET Core also provide the option to create custom filters. There are 5 types of filters supported in ASP.NET Core Web apps or services.



- **Authorization filters** run before all or first and determine the user is authorized or not.

- **Resource filters** are executed after authorization. OnResourceExecuting filter runs the code before rest of filter pipeline and OnResourceExecuted runs the code after rest of filter pipeline.

- **Action filters** run the code immediately before and after the action method execution. Action filters can change the arguments passed to method and can change returned result.

- **Exception filters** used to handle the exceptions globally before writing the response body

- **Result filters** allow to run the code just before or after successful execution of action results.
## Explain request processing pipeline [or] filter pipeline in asp.net core?
The following diagram explains the complete request processing pipeline (includes with filter pipeline, which is the subset of request pipeline).
![filters_vs_middleware](assets/filters_vs_middleware.png)


- It contains a series of methods to be executed programmatically. These methods handle MVC-specific functionality regarding manipulating ViewData, ViewBag, ModelState, Action Result and Action Parameters.
## How cookies work in asp.net core?
A cookie is a small amount of data that is persisted across requests and even sessions. Cookies store information about the user. The browser stores the cookies on the user�s computer. Most browsers store the cookies as key-value pairs.

At first, server sends a cookie (key/value pair) by using a response header called �Set-Cookie�. Then the browser receives it and stores the cookie in the browser memory.

For each subsequent request, the same cookie will be sent to the server with �Cookie� request header.

**Write a cookie in ASP.NET Core:**
```c#
Response.Cookies.Append(key, value);
```


**Delete a cookie in ASP.NET Core:**
```c#
Response.Cookies.Delete(somekey);
```

## How do you short circuit the request in an action filter?
If I don�t want the action method to be executed, I�ll short-circuit the request in OnActionExecuting() method of Action filter, as it executes before execution of the action method.
```c#
public class MyActionFilter : IActionFilter
{
    public void OnActionExecuting(ActionExecutingContext context)
    {
       if (condition)
       {
            //short-circuit the request
            context.Result = some_action_result;
       }
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
```
## How do you use dependency injection in action filter?
The ServiceFilterAttribute or TypeFilterAttribute helps me to have constructor injection (DI) in an action filter class.

If the filter class needs arguments to constructor, I would use TypeFilterAttribute; otherwise ServiceFilterAttribute.
```c#
public class FilterClassName : IActionFilter
{
   private readonly IService _service;

   public FilterClassName(IService service)
   {
       _service = service;
   }
   public void OnActionExecuting(ActionExecutingContext context)
   {
       //TO DO: before logic here
   }
   public void OnActionExecuted(ActionExecutedContext context)
   {
      //TO DO: after logic here
   }
}
```
## How do you override order of filters?
I would implement an interface called IOrderFilter to have a property called �Order� that assigns to an int value.

The before methods execute in ascending order of �Order� property value; and the after methods execute in descending order.
```c#
public class FilterClassName : IActionFilter, IOrderedFilter
{
  public int Order { get; set; } //Defines sequence of execution
  public FilterClassName(int order)
  {
    Order = order;
  }
  public void OnActionExecuting(ActionExecutingContext context)
  {
    //TO DO: before logic here
  }
  public void OnActionExecuted(ActionExecutedContext context)
  {
    //TO DO: after logic here
  }
}
```
## How will you add global filters?
Global filters once added to the �Filters� collection in the Startup (Program) class, will be applied to all action methods of all controllers in the application.

```c#
builder.Services.AddControllersWithViews(options => {
   options.Filters.Add<FilterClassName>(); //add by type
   //or
   options.Filters.Add(new FilterClassName()); //add filter instance
});
```