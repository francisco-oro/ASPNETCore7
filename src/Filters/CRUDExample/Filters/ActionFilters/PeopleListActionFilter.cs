using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    public class PeopleListActionFilter : IActionFilter
    {
        private readonly ILogger<PeopleListActionFilter> _logger;

        public PeopleListActionFilter(ILogger<PeopleListActionFilter> logger)
        {
            _logger = logger;
        }

        public void OnActionExecuting(ActionExecutingContext context)
        {
            context.HttpContext.Items["arguments"] = context.ActionArguments;
            // TODO: Add before logic here
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PeopleListActionFilter), nameof(OnActionExecuting));
              
            if(context.ActionArguments.TryGetValue("searchBy", out var argument))
            {
                string? searchBy = Convert.ToString(argument);
                // Validate the searchBy parameter value
                if (!string.IsNullOrEmpty(searchBy))
                {
                    var searchByOptions = new List<string>()
                    {
                        nameof(PersonResponse.PersonName),
                        nameof(PersonResponse.Email),
                        nameof(PersonResponse.DateOfBirth),
                        nameof(PersonResponse.Gender),
                        nameof(PersonResponse.CountryID),
                        nameof(PersonResponse.Address),
                    };

                    // Reset the searchBy parameter value 
                    if (searchByOptions.Any(temp => temp == searchBy) == false)
                    {
                        _logger.LogInformation("searchBy actual value {searchBy}", searchBy);
                        context.ActionArguments["searchBy"] = nameof(PersonResponse.PersonName);
                        _logger.LogInformation("searchBy updated value {searchBy}", context.ActionArguments["searchBy"]);
                    }
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context)
        {
            // TODO: Add after logic here
            _logger.LogInformation("{FilterName}.{MethodName} method", nameof(PeopleListActionFilter), nameof(OnActionExecuted));
            PeopleController peopleController = (PeopleController)context.Controller;

            IDictionary<string, object?>? parameters = (IDictionary<string, object?>?)context.HttpContext.Items["arguments"];

            if (parameters != null)
            {
                if (parameters.TryGetValue("searchBy", out var searchBy))
                {
                    peopleController.ViewData["CurrentSearchBy"] = Convert.ToString(searchBy);
                }

                if (parameters.TryGetValue("searchString", out var searchString))
                { 
                    peopleController.ViewData["CurrentSearchString"] = Convert.ToString(searchString);
                }

                if (parameters.TryGetValue("sortBy", out var sortBy))
                {
                    peopleController.ViewData["CurrentSortBy"] = Convert.ToString(sortBy);
                }

                if (parameters.TryGetValue("sortOrder", out var sortOrder))
                {
                    peopleController.ViewData["CurrentSortOrder"] = Convert.ToString(sortOrder);
                }

            }

            peopleController.ViewBag.SearchFields = new Dictionary<string, string>()
            {
                { nameof(PersonResponse.PersonName), "Person Name" },
                { nameof(PersonResponse.Email), "Email" },
                { nameof(PersonResponse.DateOfBirth), "Date of birth" },
                { nameof(PersonResponse.Gender), "Gender" },
                { nameof(PersonResponse.CountryID), "Country" },
                { nameof(PersonResponse.Address), "Address" },
            };

        }
    }
}
