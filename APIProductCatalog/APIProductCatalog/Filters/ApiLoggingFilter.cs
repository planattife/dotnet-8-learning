using Microsoft.AspNetCore.Mvc.Filters;

namespace APIProductCatalog.Filters;

public class ApiLoggingFilter : IActionFilter
{
    private readonly ILogger<ApiLoggingFilter> _logger;

    public ApiLoggingFilter(ILogger<ApiLoggingFilter> logger)
    {
        _logger = logger;
    }

    public void OnActionExecuted(ActionExecutedContext context)
    {
        _logger.LogInformation("OnActionExecuting");
        _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
        _logger.LogInformation($"Status Code: {context.HttpContext.Response.StatusCode}");
    }

    public void OnActionExecuting(ActionExecutingContext context)
    {
        _logger.LogInformation("OnActionExecuting");
        _logger.LogInformation($"{DateTime.Now.ToLongDateString()}");
        _logger.LogInformation($"ModelState: {context.ModelState.IsValid}");
    }
}
