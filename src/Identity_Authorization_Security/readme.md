# Identity, Authorization and Security Section Cheat Sheet (PPT)
## Introduction to Identity
It is an API that manages users, passwords, profile data, roles, tokens, email confirmation, external logins etc.

It is by default built on top of EntityFrameworkCore; you can also create custom data stores.

![introduction_to_identity](assets/introduction_to_identity.png)


## IdentityUser<T>
Acts as a base class for ApplicationUser class that acts as model class to store user details.

You can add additional properties to the ApplicationUser class.

### sBuilt-in Properties:

- Id

- UserName

- PasswordHash

- Email

- PhoneNumber



## IdentityRole<T>
Acts as a base class for ApplicationRole class that acts as model class to store role details. Eg: "admin"

You can add additional properties to the ApplicationRole class.

### Built-in Properties:

- Id

- Name





## Register View

![register_viww](assets/register_viww.png)


## Managers

![managers](assets/managers.png)


## UserManager
Provides business logic methods for managing users.

It provides methods for creating, searching, updating and deleting users.

### Methods:

- CreateAsync()

- DeleteAsync()

- UpdateAsync()

- IsInRoleAsync()FindByEmailAsync()

- FindByIdAsync()

- FindByNameAsync()



### SignInManager
Provides business logic methods for sign-in and sign-in functionality of the users.

It provides methods for creating, searching, updating and deleting users.

### Methods:

- SignInAsync()

- PasswordSignInAsync()

- SignOutAsync()

- IsSignedIn()





## Password Complexity Configuration
```c#
services.AddIdentity<ApplicationUser, ApplicationRole>(options => {
  options.Password.RequiredLength = 6; //number of characters required in password
  options.Password.RequireNonAlphanumeric = true; //is non-alphanumeric characters (symbols)
required in password
  options.Password.RequireUppercase = true; //is at least one upper case character required in password
  options.Password.RequireLowercase = true; //is at least one lower case character required in password
  options.Password.RequireDigit = true; //is at least one digit required in password
  options.Password.RequiredUniqueChars = 1; //number of distinct characters required in password
})
.AddEntityFrameworkStores<ApplicationDbContext>()
.AddDefaultTokenProviders()
.AddUserStore<UserStore<ApplicationUser, ApplicationRole, ApplicationDbContext, Guid>>()
.AddRoleStore<RoleStore<ApplicationRole, ApplicationDbContext, Guid>>();
```



## Login/Logout Buttons

![login_logout_buttons](assets/login_logout_buttons.png)


## Login View

![login_view](assets/login_view.png)




## Authorization Policy
```c#
services.AddAuthorization(options =>
{
  var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
  options.FallbackPolicy = policy;
});
```
![returnurl_1](assets/returnurl_1.png)
![returnurl_2](assets/returnurl_2.png)




## ReturnUrl








## Remote Validation

### Model class
```c#
public class ModelClassName
{
  [Remote(action: "action name", controller: "controller name", ErrorMessage = "error message")]
  public type PropertyName { get; set; }
}
```





## Conventional Routing
Conventional routing is a type of routing system in asp.net core that defines route templates applied on all controllers in the entire application.



You can override this using attribute routing on a specific action method.
```c#
endpoints.MapControllerRoute(
  name: "default",
  pattern: "{controller=Persons}/{action=Index}/{id?}"
);
```





## Areas
Area is a group of related controllers, views and models that are related to specific module or specific user.

![areas](assets/areas.png)



## User Roles
![user_roles](assets/user_roles.png)







## Role Based Authentication
User-role defines type of the user that has access to specific resources of the application.

Examples: Administrator role, Customer role etc.


![role_based_authentication](assets/role_based_authentication.png)








## HTTPS
![https](assets/https.png)
![https_2](assets/https_2.png)








## XSRF
XSRF (Cross Site Request Forgery - CSRF) is a process of making a request to a web server from another domain, using an existing authentication of the same web server.

Eg: attacker.com creates a form that sends malicious request to original.com.



### Attacker's request without AntiForgeryToken


![request_without_antiforgery_token](assets/request_without_antiforgery_token.png)


### Attacker's request


![attackers_request](assets/attackers_request.png)


### Legit request [No attacker.com]


![login_request](assets/login_request.png)


# Interview Questinos 

## Can you explain the concept of ASP.NET Core Identity and its role in building secure web applications?
ASP.NET Core Identity is a membership system that provides authentication and authorization functionalities for building secure web applications. It enables developers to manage user accounts, including user registration, login, password management, and user profile customization.

Additionally, ASP.NET Core Identity integrates with external login providers like Microsoft, Facebook, and Google, allowing users to log in using their existing accounts on these platforms. It also supports role-based authorization, allowing developers to control access to different parts of the application based on user roles.
## What are the key components of ASP.NET Core Identity and how do they work together?
### Identity User
ASP.NET Core Identity consists of the following key components:



- **User**: Represents a registered user in the system. It contains properties such as username, password hash, email, and other customizable user attributes.

- **Role**: Defines a set of permissions or responsibilities. Users can be assigned to one or more roles, which determine their access rights within the application.

- **UserManager**: Provides APIs for managing user-related operations, such as creating new users, deleting users, and resetting passwords.

- **SignInManager**: Handles the authentication process, including user login, logout, and session management. It also supports features like two-factor authentication and external login providers.

- **Claims**: Represent additional information about the user, such as user roles or custom attributes. Claims are used for authorization purposes and can be customized based on application requirements.


## How can you customize ASP.NET Core Identity to meet specific application requirements?
ASP.NET Core Identity offers various customization points to adapt to specific application needs.

Here are a few customization options:

- **User and Role Models:** You can extend the default User and Role models by adding additional properties or associating them with other entities in your application's domain.

- **Validation:** You can modify the validation rules for user registration, password complexity, and other user-related operations by configuring the IdentityOptions.

- **Storage:** ASP.NET Core Identity supports multiple data storage providers such as SQL Server, SQLite, and in-memory databases. You can choose the appropriate provider based on your application's requirements.

- **Authentication and Authorization:** You can configure external login providers, implement custom authentication schemes, and define fine-grained authorization policies using the built-in policy-based authorization system.

Views and UI: ASP.NET Core Identity provides default views and UI components for user registration, login, and account management. You can customize these views or create your own to match the application's visual style.

## How does ASP.NET Core Identity handle authentication and authorization in a web application?
ASP.NET Core Identity uses authentication and authorization middleware to handle these processes. When a user logs in, the SignInManager validates the user's credentials and creates an authentication ticket (token), which is stored as a cookie or other authentication token. Subsequent requests from the user contain this ticket, allowing the application to identify the user.



For authorization, ASP.NET Core Identity provides a flexible role-based system. Developers can assign users to specific roles, and then use the Authorize attribute or the policy-based authorization system to protect actions and resources. The framework checks the user's assigned roles and authorizes or denies access based on the defined rules.

## How can you handle user registration and password management in ASP.NET Core Identity?
ASP.NET Core Identity provides APIs and features to handle user registration and password management. Here's how it can be done:



- **User Registration:** To enable user registration, you can create a registration form that collects user information such as username, email, and password. In the registration action, you use the UserManager to create a new user by providing the user details. The UserManager takes care of hashing the password and storing the user in the underlying user store.



- **Password Management:** ASP.NET Core Identity provides various features for password management, including password hashing, password reset, and password change. When a user registers or changes their password, the password is automatically hashed and stored securely.



- **Password Policies:** ASP.NET Core Identity allows you to configure password policies to enforce password complexity requirements. You can customize the password length, required characters, and other criteria by configuring the IdentityOptions in the application's startup code.

## How can you implement role-based authorization in ASP.NET Core Identity?
ASP.NET Core Identity provides built-in support for role-based authorization. Here's how you can implement it:



 - **Define Roles:** First, you need to define roles that represent different levels of access or responsibilities within your application. You can create roles using the RoleManager provided by ASP.NET Core Identity.



 - **Assign Roles:** Once roles are defined, you can assign roles to individual users or groups of users. This can be done using the UserManager by associating the appropriate roles with user accounts.



 - **Protect Resources:** In your application's controllers or actions, you can use the Authorize attribute with role-based policies to restrict access to specific resources. For example, you can decorate an action with [Authorize(Roles = "Admin")] to allow only users in the "Admin" role to access it.
 


 - **Check Role-Based Authorization:** Within the controller or action, you can use the User property to check the currently authenticated user's assigned roles and perform additional authorization logic based on the user's role membership.



By implementing role-based authorization, you can control access to various parts of your application based on the user's assigned roles.## What are some common security considerations when using ASP.NET Core Identity?
## What are some common security considerations when using ASP.NET Core Identity?

When working with ASP.NET Core Identity, there are several important security considerations to keep in mind:



- **Password Storage:** Ensure that passwords are securely stored by using proper hashing and salting techniques. ASP.NET Core Identity handles this automatically, but it's essential to choose a secure password hashing algorithm and keep passwords hashed and protected from unauthorized access.



- **Authentication Security:** Implement secure authentication practices such as enforcing strong password policies, enabling two-factor authentication for sensitive accounts, and using secure protocols (HTTPS) to protect authentication credentials during transit.



- **Authorization and Role-Based Access Control (RBAC):** Carefully define and manage roles and permissions to ensure that only authorized users have access to specific resources and actions within your application. Avoid granting excessive privileges to user roles and regularly review and update role assignments as needed.



- **Cross-Site Scripting (XSS) and Cross-Site Request Forgery (CSRF):** Implement measures to prevent and mitigate common web vulnerabilities like XSS and CSRF attacks. Use appropriate input validation and output encoding techniques to protect against malicious user input and ensure that request forgery prevention mechanisms are in place.



- **Account Lockout and Brute Force Protection:** Protect user accounts from brute force attacks by implementing account lockout policies and rate limiting mechanisms. Set limits on the number of failed login attempts and implement measures to detect and respond to suspicious or malicious activities.



- **User Input Validation:** Always validate and sanitize user input to prevent security vulnerabilities such as SQL injection and other forms of code injection attacks. Use proper input validation techniques and parameterized queries when interacting with the database.



- **Error Handling and Logging:** Implement secure error handling and logging practices to avoid exposing sensitive information or system details in error messages. Ensure that exception details are properly handled and logged without revealing potentially sensitive data.



- **Regular Updates and Patches:** Keep your ASP.NET Core Identity framework and its dependencies up to date by regularly applying security patches and updates. Stay informed about any security vulnerabilities or best practices related to the framework and follow recommended guidelines for secure application development.



By considering these security aspects and implementing the necessary measures, you can enhance the overall security of your ASP.NET Core Identity-based application.

## What are different managers in ASP.NET Core?

In ASP.NET Core, managers are classes that provide APIs and functionalities for managing various aspects of an application. In the context of ASP.NET Core Identity, the following managers are commonly used:



**UserManager:** The UserManager class provides operations for managing user-related functionalities, such as creating new users, updating user information, validating user credentials, and managing user roles and claims.



**RoleManager:** The RoleManager class is responsible for managing roles within the application. It allows you to create new roles, update role information, assign or remove roles from users, and perform role-related operations.



**SignInManager:** The SignInManager class handles user authentication and sign-in processes. It provides methods for signing in a user, signing out a user, and managing user sessions. Additionally, it supports features like two-factor authentication and external login providers.



**UserStore and RoleStore:** While not managers themselves, the UserStore and RoleStore classes are implementations of the store interfaces used by the UserManager and RoleManager. These store classes handle data persistence and provide methods for interacting with the underlying data storage, such as a database or other data repositories.
## What architecture is used in ASP.NET Core Identity (with store and managers)?
ASP.NET Core Identity follows a layered architecture with separation of concerns. The architecture consists of the following components:



- **User Interface (UI) Layer:** This layer represents the user-facing part of the application, which includes views, controllers, and client-side code. The UI layer interacts with the application's business logic layer and utilizes ASP.NET Core Identity functionalities through the managers and services.



- **Business Logic Layer:** The business logic layer contains the application's core logic and is responsible for implementing application-specific rules and workflows. It uses the managers provided by ASP.NET Core Identity, such as UserManager and RoleManager, to perform user and role-related operations.



- **Data Access Layer:** The data access layer is responsible for interacting with the underlying data storage, such as a database. ASP.NET Core Identity uses the concept of stores, such as UserStore and RoleStore, which encapsulate the operations for reading and writing data related to users and roles. These stores abstract away the specific data storage implementation and provide a consistent interface for the managers to access and manipulate the data.



- **Identity Services:** ASP.NET Core Identity provides a set of services that are registered in the application's dependency injection container. These services include UserManager, RoleManager, SignInManager, and other related services. The managers and services encapsulate the core functionality of ASP.NET Core Identity and are consumed by the business logic layer and the UI layer.



Overall, the architecture of ASP.NET Core Identity follows a layered approach with clear separation of concerns, allowing for modular and extensible application development.
## What is Cross-Site Request Forgery (XSRF) and how does it impact web applications?
Cross-Site Request Forgery (XSRF), also known as CSRF or session riding, is a web security vulnerability. It occurs when an attacker tricks a user's browser into performing an unwanted action on a target website on behalf of the user. This can lead to unauthorized actions, such as making changes, submitting forms, or performing transactions, without the user's knowledge or consent. XSRF attacks exploit the trust between a website and a user's authenticated session, potentially resulting in data breaches, unauthorized access, or unintended modifications.
## How does ASP.NET Core protect against XSRF attacks?
ASP.NET Core provides built-in protection against XSRF attacks through anti-forgery tokens. Here's how it works:



**Anti-Forgery Tokens:** ASP.NET Core generates unique anti-forgery tokens for each user session. These tokens are included in HTML forms or Ajax requests as hidden fields or headers.



**Token Validation:** When a form or Ajax request is submitted, ASP.NET Core automatically validates the anti-forgery token. It compares the token sent by the client with the token stored in the server's session or cookie. If the tokens match, the request is considered valid; otherwise, it is rejected as a potential XSRF attack.



**Automatic Integration:** ASP.NET Core automatically adds anti-forgery tokens to forms generated by Razor views, making it easier to implement XSRF protection without explicit configuration.
## How can you implement XSRF protection in ASP.NET Core manually?
In ASP.NET Core, you can implement XSRF protection manually by following these steps:



**Generating Anti-Forgery Tokens:** Generate and include anti-forgery tokens in your HTML forms or Ajax requests. This can be done using the @Html.AntiForgeryToken() helper in Razor views or by manually adding the token as a hidden field or header.



**Validating Anti-Forgery Tokens:** In your server-side code, validate the anti-forgery token for each submitted form or Ajax request. Use the [ValidateAntiForgeryToken] attribute on controller actions or manually validate the token using the ValidateAntiForgeryToken attribute or Request.Form property.



**Configuring Anti-Forgery Options:** Customize the anti-forgery options in the ASP.NET Core configuration. You can set properties like cookie name, header name, and token lifespan to match your application's requirements.