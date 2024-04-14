﻿using CRUDExample.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Filters.ActionFilters
{
    public class PersonCreateAndEditPostActionFilter : IAsyncActionFilter
    {
        private readonly ICountriesService _countriesService;

        public PersonCreateAndEditPostActionFilter(ICountriesService countriesService)
        {
            _countriesService = countriesService;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            //TODO: before logic
            if (context.Controller is PeopleController peopleController)
            {
                if (!peopleController.ModelState.IsValid)
                {
                    List<CountryResponse?> countries = await _countriesService.GetAllCountries();
                    peopleController.ViewBag.Countries = countries.Select(temp =>
                        new SelectListItem() { Text = temp?.CountryName, Value = temp?.CountryID.ToString() }); ;
                    peopleController.ViewBag.Errors = peopleController.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage).ToList();

                    var personRequest = context.ActionArguments["personRequest"];
                    context.Result = peopleController.View(personRequest);  //short-circuits or skips the subsequent action filters & action methods
                }
                else
                {
                    await next(); // invokes the subsequent filter or action method
                }
            }
            else
            {
                await next();
            }

            //TODO: before logic
        }
    }
}
