using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ContactsManager.UI.Filters.ExceptionFilters
{
    public class HandleExceptionFilter : IExceptionFilter
    {
        private readonly ILogger<HandleExceptionFilter> _logger;
        private readonly IHostEnvironment _environment;

        public HandleExceptionFilter(ILogger<HandleExceptionFilter> logger, IHostEnvironment environment)
        {
            _logger = logger;
            _environment = environment;
        }

        public void OnException(ExceptionContext context)
        {
            _logger.LogError("Exception filter {FilterName}. {MethodName}\n{ExceptionType}\n{ExceptionMessage}", nameof(HandleExceptionFilter), nameof(OnException), context.Exception.GetType().ToString(), context.Exception.Message);
            if (_environment.IsDevelopment())
            {
                context.Result = new ContentResult() { Content = context.Exception.Message, StatusCode = 500};
            }
        }
    }
}
