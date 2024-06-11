# Angular and CORS Section Cheat Sheet 

## Angular

Angular is a TypeScript-based, free and open-source web application framework that is used to create dynamic, single-page web applications that run on the browser.


![angular](assets/angular.png)


## CORS

CORS (Cross-Origin Resource Sharing) is a security feature implemented by web browsers to allow or deny a web page from making requests to a different domain than the one that served the web page.

![cors](assets/cors.png)



## How CORS works?

![cors-works](assets/cors-works.png)


## Preflight Request

It is an HTTP OPTIONS request that is sent by the browser to the Web API server before the actual request is made.

The preflight request is used to determine whether the Web API server is willing to accept the actual request.


![preflight-request](assets/preflight-request.png)


## Clean Architecture
![clean-architecture](assets/clean-architecture.png)


# Interview Questions
## How can you invoke Web API services from an Angular application for CRUD operations? Can you explain with sample code for all GET, POST, PUT and DELETE HTTP methods.
To invoke Web API services from an Angular application, it is necessary 
to create a new `service`, which we will preferably store in a dedicated services folder.

Using `angular-cli`:
```shell
ng g s app/services/webApi
```
Verify Web API service file has been successfully created 
in `app/services`

### Import HttpClientModule
In `app/app.module.ts`
```typescript 
// import ...
import {HttpClientModule} from "@angular/common/http";

@NgModule({
  declarations: [
      // ...
  ],
    imports: [
        // ...
        HttpClientModule,
    ],
  providers: [],
    // ...
})
export class AppModule { }

```

### Setting Up WebAPI service to perform http requests
First of all, it is recommended to use a constant `API_BASE_URL`
store the Web API url. We will use `injectable` as the service
decorator for the service, `HttpClient` is the class to be injected in
the service constructor.
<br>
<br>

Make sure to import a `model` for the `HttpResponse` and the `Observable` class as well.
<br>

In `app/services/web-api.service.ts`:
```typescript
import { Injectable } from '@angular/core';
import { City} from "../models/city";
import {HttpClient} from "@angular/common/http";
import {Observable} from "rxjs";

const API_BASE_URL: string = "http://localhost:5218/api";
@Injectable({
  providedIn: 'root'
})
export class CitiesService {
  cities: City[] = [];
  constructor(private httpClient: HttpClient) {
  }

  public getCities(): Observable<City[]> {
    return this.httpClient.get<City[]>(`${API_BASE_URL}/v1.0/Cities`, {headers: headers});
  }

  public postCity(city: City): Observable<City> {

    return this.httpClient.post<City>(`${API_BASE_URL}/v1.0/Cities`,city, {headers: headers});
  }

  public putCity(city: City): Observable<string>{

    return this.httpClient.put<string>(`${API_BASE_URL}/v1.0/Cities/${city.cityID}`,city, {headers: headers});
  }

  public deleteCity(cityID: string | null): Observable<string>{

    return this.httpClient.delete<string>(`${API_BASE_URL}/v1.0/Cities/${cityID}`, {headers: headers});
  }
}

```


## What is CORS (Cross-Origin Resource Sharing)?
It is a security feature implemented in web browsers to manage the rules for requests and responses exchanged
between the client and the server. It focuses on: 
- Providing the list of domains that the server will accept requests from
- Methods that are allowed for each one
## Why is CORS necessary?
Because it provides a layer of security to the server so attackers cannot perform unauthorized actions from 
external domains
## How CORS works internally?
1. It all starts in the web browser. The browser sends a new HTTP request, which includes the following HTTP request header:
`origin: example.com`
2. The Http Response verifies `origin` value from the request headers, and it matches that value with a list of 
authorized domains.
3. If the domain is matched, the server attaches the header `Access-Control-Allow-Origin` to the response and it
is sent back to the client

## How do you enable CORS in an ASP.NET Core Web API?
By adding th
```csharp
// CORS: localhost:4200, localhost:4100
builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins").Get<string[]>() ?? Array.Empty<string>())
            .WithHeaders("Authorization", "origin", "accept", "content-type")
            .WithMethods("GET", "POST", "PUT", "DELETE");
    });
    options.AddPolicy("4100Client" ,corsPolicyBuilder =>
    {
        corsPolicyBuilder
            .WithOrigins(builder.Configuration.GetSection("AllowedOrigins2").Get<string[]>() ?? Array.Empty<string>())
            .WithHeaders("Authorization", "origin", "accept")
            .WithMethods("GET");
    });
});
```
## What are CORS policies?

## How can you create and apply multiple CORS policies in ASP.NET Core Web API?