using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Primitives;
using MiddlewareLoginAssignment.Exceptions;

namespace MiddlewareLoginAssignment.CustomMiddleware
{
    // You may need to install the Microsoft.AspNetCore.Http.Abstractions package into your project
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly string Email = "admin@example.com"; 
        private readonly string Password = "admin1234";

        public AuthenticationMiddleware( RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            StreamReader reader = new StreamReader(httpContext.Request.Body);
            string body = await reader.ReadToEndAsync();

            Dictionary<string, StringValues> queryDictionary = QueryHelpers.ParseQuery(body);
            try
            {
                queryDictionary.TryGetValue("password", out StringValues password);
                queryDictionary.TryGetValue("email", out StringValues username);
                if (username.Count == 0)
                {
                    throw new NullUserNameException();
                }

                if (password.Count == 0)
                {
                    throw new NullPasswordException();
                }

                if (!(bool)ValidateEmail(username[0]))
                {
                    throw new IncorrectUserNameException();
                }

                if (!(bool)ValidatePassword(password[0]))
                {
                    throw new IncorrectPasswordException();
                }

                httpContext.Response.StatusCode = 200;
                await httpContext.Response.WriteAsync("Successful login");

            }
            catch (IncorrectUserNameException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Unsuccessful login");
            }
            catch (IncorrectPasswordException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Unsuccessful login");
            }
            catch (NullPasswordException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid input for 'password'");
            }
            catch (NullUserNameException)
            {
                httpContext.Response.StatusCode = 400;
                await httpContext.Response.WriteAsync("Invalid input for 'email'");
            }

            await _next(httpContext);
        }

        private bool? ValidateEmail(string email)
        {
            try
            {
                return email.Equals(Email);
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }

        private bool? ValidatePassword(string password)
        {
            try
            {
                return password.Equals(Password);
            }
            catch (NullReferenceException e)
            {
                return null;
            }
        }
    }

    // Extension method used to add the middleware to the HTTP request pipeline.
    public static class AuthenticationMiddlewareExtensions
    {
        public static IApplicationBuilder UseAuthenticationMiddleware(this IApplicationBuilder builder)
        {
            return builder.UseMiddleware<AuthenticationMiddleware>();
        }
    }
}
