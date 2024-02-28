# Dependency Injection Cheat Sheet

## Services

![services](assets/services.png)
'Service' is a class that contains business logic such as business calculations, business validations that are specific to the domain of the client's business.

Service is an abstraction layer (middle layer) between presentation layer (or application layer) and data layer.

It makes the business logic separated from presentation layer and data layer.

It makes the business logic to be unit testable easily.

Will be invoked by controller.



## Direct Dependency

![direct_dependency_1](assets/direct_dependency_1.png)
![direct_dependency_2](assets/direct_dependency_2.png)




Higher-level modules depend on lower-level modules.





## Dependency Problem
> Higher-level modules depend on lower-level modules.

- Means, both are tightly-coupled.

- The developer of higher-level module SHOULD WAIT until the completion of development of lower-level module.

- Requires much code changes in to interchange an alternative lower-level module.

- Any changes made in the lower-level module effects changes in the higher-level module.

- Difficult to test a single module without effecting / testing the other module.



## Dependency Inversion Principle
Dependency Inversion Principle (DIP) is a design principle (guideline), which is a solution for the dependency problem.



> "The higher-level modules (clients) SHOULD NOT depend on low-level modules (dependencies).

> Both should depend on abstractions (interfaces or abstract class)."

> "Abstractions should not depend on details (both client and dependency).

> Details (both client and dependency) should depend on abstractions."


![dependency_inversion_principle](assets/dependency_inversion_principle.png)




The interface is controlled by the client.

Both client and dependency depend on abstraction.


![dependency_inversion_principle_2](assets/dependency_inversion_principle_2.png)






## Inversion of Control (IoC)
- Inversion of Control (IoC) is a design pattern (reusable solution for a common problem), which suggests "IoC container" for implementation of Dependency Inversion Principle (DIP).

- It inverses the control by shifting the control to IoC container.

- "Don't call us, we will call you" pattern.

- It can be implemented by other design patterns such as events, service locator, dependency injection etc.



![ioc](assets/ioc.png)



All dependencies should be added into the IServiceCollection (acts as IoC container).
```c#
builder.Services.Add(
  new ServiceDescriptor(
    typeof (Interface),
    typeof (Service)
    ServiceLifetime.LifeTime //Transient, Scoped, Singleton
  )
);

```


## Dependency Injection (DI)
- Dependency injection (DI) is a design pattern, which is a technique for achieving "Inversion of Control (IoC)" between clients and their dependencies.

- It allows you to inject (supply) a concrete implementation object of a low-level component into a high-level component.

- The client class receives the dependency object as a parameter either in the constructor or in a method.


![dependency_injection](assets/dependency_injection.png)
![dependency_injection_2](assets/dependency_injection_2.png)









## Method Injection

![method_injection_1](assets/method_injection_1.png)
![method_injection_2](assets/method_injection_2.png)









## Service Lifetime
(Transient, Scoped, Singleton)



A service lifetime indicates when a new object of the service has to be created by the IoC / DI container.

- Transient: Per injection

- Scoped: Per scope (browser request)

- Singleton: For entire application lifetime.


![service_lifetime](assets/service_lifetime.png)




### Transient

Transient lifetime service objects are created each time when they are injected.

Service instances are disposed at the end of the scope (usually, a browser request)



### Scoped

Scoped lifetime service objects are created once per a scope (usually, a browser request).

Service instances are disposed at the end of the scope (usually, a browser request).



### Singleton

Singleton lifetime service objects are created for the first time when the are requested.

Service instances are disposed at application shutdown.





### Transient
```c#
builder.Services.AddTransient<IService, Service>(); //Transient Service
```
### Scoped
```c#
builder.Services.AddScoped<IService, Service>(); //Scoped Service
```
### Singleton
```c#
builder.Services.AddSingleton<IService, Service>(); //Singleton Service
```




## Service Scope


![service_scope](assets/service_scope.png)





## View Injection

![view_injection](assets/view_injection.png)
![view_injection_2](assets/view_injection_2.png)







## Best Practices in DI


### Global state in services

> Avoid using static classes to store some data globally for all users / all requests.

> You may use Singleton services for simple scenarios / simple amount of data. In this case, prefer ConcurrentDictionary instead of Dictionary, which better handles concurrent access via multiple threads.

> Alternatively, prefer to use Distributed Cache / Redis for any significant amount of data or complex scenarios.



### Request state in services

> Don't use scoped services to share data among services within the same request, because they are NOT thread-safe.

> Use HttpContext.Items instead.



### Service Locator Pattern

> Avoid using service locator pattern, without creating a child scope, because it will be harder to know about dependencies of a class.

> For example, don't invoke GetService() in the default scope that is created when a new request is received.

> But you can use the IServiceScopeFactory.ServiceProvider. GetService() within a child scope.



### Calling Dispose() method

> Don't invoke the Dispose() method manually for the services injected via DI.

> The IoC container automatically invoke Dispose(), at the end of its scope.



### Captive Dependencies

> Don't inject scoped or transient services in singleton services.

> Because, in this case, transient or scoped services act as singleton services, inside of singleton service.



### Storing reference of service instance

> Don't hold the reference of a resolved service object.

> It may cause memory leaks and you may have access to a disposed service object.






## Autofac
- Autofac is another IoC container library for .Net Core.

- Means, both are tightly-coupled.

- Microsoft.Extensions.DependencyInjection [vs] Autofac

- https://autofac.readthedocs.io/en/latest/getting-started/index.html



### Microsoft.Extensions.DependencyInjection

- Built-in IoC container in asp.net core

- Lifetimes: Transient, Scoped, Singleton

- Metadata for services: Not supported

- Decorators: Not supported



### Autofac

- Alternative to the Microsoft.Extensions

- Lifetimes: InstancePerDependency, InstancePerLifetimeScope, SingleInstance, InstancePerOwned, InstancePerMatchingLifetimeScope

- Metadata for services: Supported

- Decorators: Supported



# Interview Questions

## Explain how dependency injection works in ASP.NET Core?
> ASP.NET Core injects instances of dependency classes by using the built-in IoC (Inversion-of-Control) container. This container is represented by the IServiceProvider interface.



> The types (classes) managed by the container are called services. We first need to register them with the IoC container in the Startup class.



> ASP.NET Core supports two types of services, namely framework and application services. Framework services are a part of ASP.NET Core framework such as ILoggerFactory, IHostingEnvironment, etc. In contrast, a developer creates the application services (custom types or classes) specifically for the application.
![services](assets/services.png)

## “ASP.NET Core has dependency injection to manage services; are you aware of the different lifetimes? What are they, and what does each mean?”
- **Transient**: ASP.NET Core creates transient services when a dependent instance asks for them. I might consider registering dependencies as transient as the “safest” approach to creating dependencies as there’s no chance for contention, race conditions, or deadlocks. Still, it can also come at the expense of performance and resource utilization.
- **Scoped**: As the name suggests, with regard to scoped services, they are created within a scope. The scope is typically the lifetime of an HTTP request, but not necessarily always. I, as a developer, might create custom scopes in code, but anyone should be careful to use this technique sparingly.
- **Singleton**: ASP.NET Core services container will create services registered as a singleton only once for the duration of the application’s lifetime. Singletons are helpful for expensive services or services with little to no internal state.

Each lifetime has its use, and it depends on the dependency we are registering.


## What are the benefits of Dependency Injection?
> DI helps to implement decoupled architecture where you change at one place and changes are reflected at many places.

> Dependency injection is basically providing the objects that an object needs (its dependencies) instead of having it construct them itself. When using dependency injection, objects are given their dependencies at run time rather than compile time (car manufacturing time).

- It allows your code to be more loosely coupled because classes do not have hard-coded dependencies

- Decoupling the creation of an object (in other words, separate usage from the creation of an object)

- Making isolation in unit testing possible/easy. It is harder to isolate components in unit testing without dependency injection.

- Explicitly defining dependencies of a class

- Facilitating good design like the single responsibility principle (SRP) for example

- Promotes Code to an interface, not to implementation principle

Enabling switching/ability to replace dependencies/implementations quickly (Eg: DbLogger instead of ConsoleLogger)
## What is IoC (DI) Container?
A Dependency Injection container, sometimes, referred to as DI container or IoC container, is a framework that helps with DI. It “creates” and “injects” dependencies for us automatically.

## What is Inversion of Control?
Inversion of control is a broad term but for a software developer it's the most commonly described as a pattern used for decoupling components and layers in the system.

It inverses the control by shifting the control to IoC container.



For example, say your application has a text editor component and you want to provide spell checking. Your standard code would look something like this:
```c#
public class TextEditor {
 private SpellChecker checker;

 public TextEditor() {
  this.checker = new SpellChecker();
 }
}
```

What we've done here creates a dependency between the TextEditor and the SpellChecker. In an IoC scenario we would instead do something like this:
SpellChecker. In an IoC scenario we would instead do something like this:
```c#
public class TextEditor {
 private IocSpellChecker checker;

 public TextEditor(IocSpellChecker checker) {
   this.checker = checker;
 }
}
```

You have inverted control by handing the responsibility of instantiating the spell checker from the TextEditor class to the caller.
```c#
SpellChecker sc = new SpellChecker; // dependency
TextEditor textEditor = new TextEditor(sc);
```
## How do you create your own scopes in asp.net core?
We can create child scopes by using IServiceScopeFactory.

```c#
//controller
public ControllerName(IServiceScopeFactory serviceScopeFactory)
{
  _serviceScopeFactory = serviceScopeFactory;
}

[Route("route-path")]
public IActionResult ActionMethod()
{
 using (IServiceScope scope = _serviceScopeFactory.CreateScope())
 {
  IService service = scope.ServiceProvider.GetRequiredService<IService>();
  //call service methods here
 }
 return View();
}
```
## How do you inject a service in view?
We can do that by using @inject directive in razor view.

Eg:
```c#
@inject IService service

```

## Why you prefer Autofac over built-in Microsoft DI?
Autofac is an IoC container for .NET. It manages the dependencies between classes so that applications stay easy to change as they grow in size and complexity. Autofac is the most popular DI/IoC container for ASP.NET core.

Though the default Microsoft DI may offer enough functionality, there is a certain limitations like resolving a service with some associated Metadata, Named/Keyed services, Aggregate Services, Multi-tenant support, lazy instantiation, and much more. As the system grows you might need such features, and Autofac gives you all these features.
## What exception do you get when a specific service that you injected, can’t be found in the IoC container?
I’ll get an “InvalidOperationException” with error message “Unable to resolve service for type 'type’ while attempting to activate ‘class_name'.

