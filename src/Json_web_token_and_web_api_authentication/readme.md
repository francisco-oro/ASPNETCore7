# JWT Section Cheat Sheet

## Identity with Web API

It is an API that manages users, passwords, profile data, roles, tokens, email confirmation, external logins etc.

It is by default built on top of EntityFrameworkCore; you can also create custom data stores.

![identity_with_web_api](assets/identity_with_web_api.png)



## Register Endpoint

![register_endpoint](assets/register_endpoint.png)


## Register UI

![register_ui](assets/register_ui.png)


## Login Endpoint

![login_endpoint](assets/login_endpoint.png)


## Login UI

![login_ui](assets/login_ui.png)


## Introduction to JWT

A JSON Web Token (JWT) is a compact and self-contained object for securely transmitting information between parties as a JSON object.

![introduction_to_jwt](assets/introduction_to_jwt.png)



## Contents of JWT

## 1. Header (base 64 string)

Defines the type of token and the signing algorithm used.

> Eg: eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9


## 2. Payload (base 64 string)

Contains user claims (user details such as name, email or user type).

> Eg: { "userId": "b08f86af-35da-48f2-8fab-cef3904660bd"}

> Eg: eyJ1c2VySWQiOiJiMDhmOZhZi0zNWRhLTQ4ZjItOOTA0NjYwYmQifQ


## 3. Signature (base 64 string)

It is used to verify to ensure that the message wasn't changed along the way.

It is usually signed by using a secret key (HMAC algorithm).

> -xN_h82PHVTA9vdoHrcZxH-x5




## JWT Algorithm

Inputs:

## header

    {
    "typ": "JWT",
    "alg": "HS256"
    }


## payload

    {
    "userId": "b08f86af-35da-48f2-8fab-cef3904660bd"
    }


## secret

MySecret



### Algorithm:
```python
    data = base64Encode( header ) + "." + base64Encode( payload )
    hashedData = hash( data, secret )
    signature = base64encode( hashedData )
    jwtToken = data + "." + signature

```


### Example JWT token:

> eyJ0eXAiOiJKV1QiLCJhbGciOiJIUzI1NiJ9.eyJ1c2VySWQiOiJiMDhmODZhZi0zNWRhLTQ4ZjItOOTA0NjYwYmQifQ.-xN_h82PHVTCMA9vdoHrcZxH-x5mb11y1537t3rGzcM




## Refresh Tokens - JWT

A refresh token is a token (base-64 string of a random number) that is used to obtain a new JWT token every time, when it is expired.

![refresh_tokens_jwt](assets/refresh_tokens_jwt.png)

# Interview Questions

## What is JWT (JSON Web Token)?
It stands for `javascript object notation` and it is one of the most preferable methods modern systems use to exchange information,
mainly between server and client applications
## How can you configure JWT authentication in ASP.NET Core Web API?
By using the `Microsoft.AspNetCore.Authentication.JwtBearer` package and configuring 
`builder.Services.AddAuthentication(options => {}).AddJwtBearer(options => {});`
## How can you generate a JWT token in ASP.NET Core Web API?
By creating a new instance of the JwtSecurityTokenHandler class
## How can you authorize API endpoints using JWT tokens?

## How can you implement role-based authorization using Identity in ASP.NET Core Web API?

## What is refresh token and what is its purpose in asp.net core?

## How to implement refresh tokens with JWT tokens in asp.net core?
