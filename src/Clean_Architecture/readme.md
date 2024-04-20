# Clean Architectire Section Cheat Sheet (PPT)

## Overview of Clean Architecture
Instead of "business logic" depend on "data access logic", this dependency is inverted; that means, the "data access logic" depend on "business logic".

**Benefit**: The business logic is highly clean-separated, independent of data storage and UI, unit-testable.

![overview_of_clean_architecture](assets/overview_of_clean_architecture.png)


### Traditional Three-Tier / N-Tier Architecture

- User Interface (UI)

- Business Logic Layer (BLL)

- Repository Layer (RL)

- Data Access Layer (DAL)





### Clean Architecture

### UI

- Controllers, Views, View Models

- Filters, Middleware



### Core

- Business Logic Services

- Business Logic Interfaces

- Data Transfer Objects (DTO)



### Domain

- Repository Interfaces

- Entity Classes



### Infrastructure

- DbContext, Repositories

- External API Calls





## Clean Architecture
### Changing external system

Allows you to change external systems (external APIs / third party services) easily, without affecting the application core.



### Scalable

You can easily scale-up or scale-out, without really affecting overall architecture of the application.



### Database independent

The application core doesn't depend on specific databases; so you can change it any time, without affecting the application core.



### Testable

The application core doesn't depend on any other external APIs or repositories; so that you can write unit tests against business logic services easily by mocking essential repositories.



Clean architecture is earlier named as "Hexagonal architecture", "Onion architecture", "Domain-Driven Design", "Vertical Slice Architecture". Over time, it is popular as "clean architecture".



Clean Architecture

![clean_architecture](assets/clean_architecture.png)

# Interview Questions

### What is Clean Architecture?
`Clean Architecture` a.k.a "Hexagonal Architecture", "Onion Architecture, "Domain-Driven Design" is an architecture suitable
for medium to large-scale applications. It consists of four important modules: 
- Both UI and Infraestructure depend on Core.
- Domain is a subset of Core. So it depends on Core as well. 
### Can you explain the differences between clean architecture and other types of architectures?
- Clean Architecture is suitable for medium and large-scale applications whereas other types of architectures are best suitable for small applications
- It makes it easy to develop each module independently
- It's easy to replace any service or database as Infraestructure is developed independently
- It's easy to test every part of the application as they are not loosely coupled
- It makes it easy to reuse any component for different projects
### What are some examples of good use cases for a clean architecture implementation?
A good case use for a clean architecture implementation would be an application that needs to be very modular an scalable. 
I.e. a large e-commerce website might need to be able to handle a large number of users and a large number of transactions 
### Can you give me a general overview of how to build an application using clean architecture?

### Is clean architecture suitable for all kinds of applications or only specific ones? If it’s not suitable for certain types of apps, then what are the alternatives?