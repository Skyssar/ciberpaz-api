using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

[AttributeUsage(AttributeTargets.Method | AttributeTargets.Class)]
public class ApiKeyAttribute : Attribute, IAsyncActionFilter
{
    private const string HEADER_NAME = "X-API-KEY";

    public async Task OnActionExecutionAsync(
        ActionExecutingContext context,
        ActionExecutionDelegate next)
    {
        var config = context.HttpContext.RequestServices
            .GetRequiredService<IConfiguration>();

        if (!context.HttpContext.Request.Headers
            .TryGetValue(HEADER_NAME, out var apiKey))
        {
            context.Result = new UnauthorizedObjectResult("API Key missing");
            return;
        }

        if (apiKey != config["ApiKey"])
        {
            context.Result = new UnauthorizedObjectResult("Invalid API Key");
            return;
        }

        await next();
    }
}
