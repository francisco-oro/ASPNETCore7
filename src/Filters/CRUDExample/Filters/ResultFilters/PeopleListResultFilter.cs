using Microsoft.AspNetCore.Mvc.Filters;

namespace CRUDExample.Filters.ResultFilters
{
    public class PeopleListResultFilter : IAsyncResultFilter
    {
        private readonly ILogger<PeopleListResultFilter> _logger;

        public PeopleListResultFilter(ILogger<PeopleListResultFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
        {
            //TODO: before logic
            _logger.LogInformation("{FilterName}.{MethodName} - before", nameof(PeopleListResultFilter), nameof(OnResultExecutionAsync));
            await next(); //call the subsequent filter [or] IActionResult

            //TODO: after logic
            _logger.LogInformation("{FilterName}.{MethodName} - after", nameof(PeopleListResultFilter), nameof(OnResultExecutionAsync));

            context.HttpContext.Response.Headers["Last-Modified"] = DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
