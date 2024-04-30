# Swagger Section Cheat Sheet (PPT)
## Introduction to Swagger
Swagger is a set of open-source tools that help developers to generate interactive UI to document, test RESTful services.

Swagger is a set of tools to implement Open API.



### 1. Swasbuckle.AspNetCore

Framework that makes it easy to use swagger in asp.net core.



### 2. Swagger

Set of tools to generate UI to document & test RESTful services.



### 3. Open API

Specification that defines how to write API specifications in JSON).





## API Versions
API Versioning is the practice of transparently managing changes to your API, where the client requests a specific version of API; and the server executes the same version of the API code.



![api_versions](assets/api_versions.png)






## Content Negotiation
Content negotiation is the process of selecting the appropriate format or language of the content to be exchanged between the client (browser) and Web API.


![content_negotiation](assets/content_negotiation.png)


## What is Swagger/OpenAPI?
Swagger, now known as OpenAPI, is a specification and set of tools for designing, building, documenting, and consuming RESTful APIs. It provides a standardized way to describe APIs using JSON or YAML, enabling developers to understand and interact with APIs more easily.

## What is the purpose of Swagger/OpenAPI?
The main purpose of Swagger/OpenAPI is to enhance the development process of RESTful APIs by providing a machine-readable contract that describes the API's structure, endpoints, request/response formats, authentication requirements, and more. This contract can be used to generate client SDKs, server stubs, and interactive API documentation, simplifying API consumption and integration.
## How can you enable Swagger in ASP.NET Core?
To enable Swagger in ASP.NET Core, you need to follow these steps:



1. Install the "Swashbuckle.AspNetCore" NuGet package.



2. In the Startup.cs file, add the following code within the ConfigureServices method:

```c#
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
});
```

3. In the Configure method, add the following code:

```c#
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
});
```

4. Run your application and navigate to the Swagger UI at "/swagger" to view and interact with the API documentation.


## How can you version your API using Swagger in ASP.NET Core?
To version your API using Swagger in ASP.NET Core, you can follow one of the common versioning approaches like URI versioning or query string versioning. Once you have implemented the versioning mechanism, you can generate separate Swagger documents for each API version. Here's an example of using URI versioning:



1. Install the "Microsoft.AspNetCore.Mvc.Versioning" NuGet package.



2. Configure the API versioning in the Startup.cs file within the ConfigureServices method:

```c#
builder.Services.AddApiVersioning(options =>
{
  options.DefaultApiVersion = new ApiVersion(1, 0);
  options.AssumeDefaultVersionWhenUnspecified = true;
});
```

3. Modify the Swagger configuration in ConfigureServices to generate versioned Swagger documents:
```c#
builder.Services.AddSwaggerGen(c =>
{
  c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API Name", Version = "v1" });
  c.SwaggerDoc("v2", new OpenApiInfo { Title = "Your API Name", Version = "v2" });
});
```

4. Configure Swagger UI in the Configure method:
```c#
app.UseSwagger();
app.UseSwaggerUI(c =>
{
  c.SwaggerEndpoint("/swagger/v1/swagger.json", "Your API Name v1");
  c.SwaggerEndpoint("/swagger/v2/swagger.json", "Your API Name v2");
});
```

Now, you can access the different API versions in the Swagger UI.


## What is content negotiation in the context of ASP.NET Core Web APIs?
Content negotiation in the context of ASP.NET Core Web APIs refers to the process of selecting the appropriate response format (such as JSON, XML, or others) based on the client's preferences and the available options from the server. The negotiation process involves matching the client's requested media types (specified in the Accept header) with the server's supported media types (specified in the Produces attribute or configuration).

## How can you implement content negotiation in ASP.NET Core Web APIs?
Content negotiation in Web APIs can be implemented using the Produces attribute or the AddControllers method configuration.

Here's how you can do it:



1. Using the Produces attribute:
```c#
[ApiController]
[Route("api/[controller]")]
public class MyController : ControllerBase
{
  [HttpGet]
  [Produces("application/json", "application/xml")]
  public IActionResult Get()
  {
    // Action implementation...
  }
}
```
In this example, the Produces attribute is applied to the Get action method of the MyController API. It specifies that the action can produce responses in either JSON or XML format. The appropriate format is selected based on the client's Accept header.



2. Using the AddControllers method configuration in the Startup.cs file:
```c#
builder.Services.AddControllers(options =>
{
  options.OutputFormatters.Add(new XmlSerializerOutputFormatter());
})
.AddXmlDataContractSerializerFormatters();
```

In this example, the AddControllers method is used to configure the content negotiation for the Web API. The ReturnHttpNotAcceptable property is set to true to return a 406 Not Acceptable status code when the requested media type is not supported. The XmlSerializerOutputFormatter is added to the OutputFormatters collection to support XML serialization. Additionally, the AddXmlDataContractSerializerFormatters method is called to enable XML serialization using the DataContractSerializer.