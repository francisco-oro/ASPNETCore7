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
JWT (JSON Web Token) is a compact and self-contained way to transmit information between parties as a JSON object. It is a standardized format for token-based authentication and authorization in web applications.


A JWT consists of three parts separated by dots: the header, the payload, and the signature.


**Header**: The header contains metadata about the token, such as the type of token and the algorithm used for signing the token. It is base64Url encoded.


**Payload**: The payload contains the claims or statements about the user or entity, such as the user's identity, roles, and additional data. Claims can include standard claims defined by the JWT specification or custom claims specific to the application. The payload is also base64Url encoded.


**Signature**: The signature is created by combining the base64Url encoded header and payload, along with a secret key known only to the server. It is used to verify the integrity of the token and ensure that it has not been tampered with.


JWTs are commonly used for authentication and authorization in web applications. When a user logs in or authenticates, the server generates a JWT and sends it back to the client. The client includes the JWT in subsequent requests as an authorization mechanism. The server can then verify the token's signature and extract the claims to determine the identity and permissions of the user.


One of the advantages of JWTs is that they are self-contained, meaning the server can validate and extract the necessary information without needing to store the token on the server-side. This makes JWTs suitable for stateless authentication systems and distributed environments.


## How can you configure JWT authentication in ASP.NET Core Web API?
To configure JWT authentication in ASP.NET Core Web API with top-level statements, modify the Program.cs file as follows:

```csharp
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

// Add the following code after builder.Services.AddIdentity()
var jwtSettings = builder.Configuration.GetSection("JwtSettings");
var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
var tokenValidationParameters = new TokenValidationParameters
{
ValidateIssuer = true,
ValidateAudience = true,
ValidateIssuerSigningKey = true,
ValidIssuer = jwtSettings["Issuer"],
ValidAudience = jwtSettings["Audience"],
IssuerSigningKey = key
};

builder.Services.AddAuthentication(options =>
{
options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
options.TokenValidationParameters = tokenValidationParameters;
});
```



This code configures JWT authentication by using the JwtBearerDefaults.AuthenticationScheme and sets the token validation parameters, including the issuer, audience, and signing key.


## How can you generate a JWT token in ASP.NET Core Web API?
To generate a JWT token in ASP.NET Core Web API, you can use the System.IdentityModel.Tokens.Jwt package. Modify the Program.cs file as follows:
```csharp
using System.IdentityModel.Tokens.Jwt;

// Inside a method or controller action
var tokenHandler = new JwtSecurityTokenHandler();
var key = Encoding.UTF8.GetBytes(jwtSettings["Key"]);
var tokenDescriptor = new SecurityTokenDescriptor
{
Subject = new ClaimsIdentity(new[]
{
new Claim(ClaimTypes.Name, "John Doe"),
// Add additional claims as needed
}),
Expires = DateTime.UtcNow.AddHours(1),
SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
};

var token = tokenHandler.CreateToken(tokenDescriptor);
var tokenString = tokenHandler.WriteToken(token);
```

This code creates a new instance of `JwtSecurityTokenHandler` and generates a JWT token by specifying the claims, expiration time, signing credentials, and other necessary information.


## How can you authorize API endpoints using JWT tokens?
To authorize API endpoints using JWT tokens in ASP.NET Core Web API, you can use the [Authorize] attribute on API endpoints or controllers. Modify the Program.cs file as follows:

```csharp
// Add the following code after app.UseAuthorization()
app.MapControllers().RequireAuthorization();
```

This code ensures that all API endpoints require authorization. You can also apply the [Authorize] attribute selectively on individual controllers or action methods for more granular control.


Remember to update the appsettings.json file with the required JWT settings, such as the issuer, audience, and signing key.
## How can you implement role-based authorization using Identity in ASP.NET Core Web API?
To implement role-based authorization using Identity in ASP.NET Core Web API, follow these steps:


Define the roles in the database using the RoleManager provided by Identity.

Assign roles to users during user registration or through an admin interface.

Add the `[Authorize(Roles = "RoleName")]` attribute to API endpoints or controllers that should be accessible only to users in specific roles.


In the `Program.cs` file, configure the authorization policy by adding the following code:
```csharp
builder.Services.AddAuthorization(options =>
{
options.AddPolicy("RoleNamePolicy", policy =>
policy.RequireRole("RoleName"));
});
```

Apply the policy to the desired endpoints by using the `[Authorize(Policy = "RoleNamePolicy")]` attribute.
## What is refresh token and what is its purpose in asp.net core?
A refresh token is a type of token used in token-based authentication systems, to obtain a new access (JWT) token without requiring the user to reauthenticate. It is an additional token issued alongside the access token and serves a specific purpose:


**Longer-lived sessions**: Refresh tokens have a longer expiration time compared to access tokens. While access (JWT) tokens are typically short-lived and have a limited validity period, refresh tokens are designed to be used for a longer duration, often lasting days, weeks, or even months.


**Renewing access tokens**: When an access token expires, the refresh token can be used to request a new access token from the authentication server without prompting the user for their credentials again. This allows the user to maintain their session and continue accessing protected resources seamlessly.


**Revoking access**: Refresh tokens can be revoked independently of access tokens. This provides better control and security if a refresh token is suspected to be compromised or if a user wants to revoke access to their account.


**Reduced reliance on authentication server**: By using refresh tokens, client applications can minimize the frequency of interactions with the authentication server for token renewal. This reduces the load on the authentication server and enhances the overall performance of the system.


Overall, the purpose of a refresh token is to provide a long-lived, secure mechanism for obtaining new access tokens without requiring the user to reauthenticate. It improves user experience, security, and reduces the need for frequent authentication requests, making it a valuable component of token-based authentication systems.
## How to implement refresh tokens with JWT tokens in asp.net core?
To implement refresh token, you can modify the ApplicationUser model to store refresh token and its details inside the ApplicationUser model itself (in the AspNetUsers tabe).


Steps:

1. **Modify the ApplicationUser model**: Add properties for the refresh token and its details inside the ApplicationUser class.
```csharp
public class ApplicationUser : IdentityUser
{
// Other user properties

public string RefreshToken { get; set; }
public DateTime RefreshTokenExpiration { get; set; }
}
```

2. **Update the registration/login process**: During the registration or login process, generate a refresh token and assign it to the RefreshToken property of the ApplicationUser model. Set the RefreshTokenExpiration property to the appropriate expiration date.


3. **Store and retrieve the ApplicationUser**: When saving or retrieving the ApplicationUser from the database, ensure that the refresh token and its details are persisted along with the user data.


4. **Token generation and validation**: During the generation of the JWT access token, include the refresh token and its expiration details in the token payload. When validating the JWT, make sure to also validate the refresh token details and expiration.


5. **Token refresh**: When a client sends a request with an expired access token, check if the associated ApplicationUser has a valid refresh token. If the refresh token is valid, generate a new access token and update the RefreshToken and RefreshTokenExpiration properties of the ApplicationUser model. Return the new access token to the client.


By adding the refresh token and its details directly inside the ApplicationUser model, you can maintain the token information alongside the user data, simplifying the process of managing and associating refresh tokens with users.