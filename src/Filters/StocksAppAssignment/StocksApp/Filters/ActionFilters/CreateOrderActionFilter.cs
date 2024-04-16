using Microsoft.AspNetCore.Mvc.Filters;

namespace StocksApp.Filters.ActionFilters
{
    public class CreateOrderActionFilter : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            await next();
        }
    }
}
