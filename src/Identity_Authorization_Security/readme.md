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








HTTPS








XSRF
XSRF (Cross Site Request Forgery - CSRF) is a process of making a request to a web server from another domain, using an existing authentication of the same web server.

Eg: attacker.com creates a form that sends malicious request to original.com.



Attacker's request without AntiForgeryToken




Attacker's request




Legit request [No attacker.com]


