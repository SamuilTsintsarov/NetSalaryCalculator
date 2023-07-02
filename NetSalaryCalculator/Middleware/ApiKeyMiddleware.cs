namespace NetSalaryCalculator.Middleware
{
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Configuration;
    using Microsoft.Extensions.DependencyInjection;
    using System.Linq;
    using System.Threading.Tasks;

    public class ApiKeyMiddleware
    {
        private readonly RequestDelegate _next;
        private const string APIKEYNAME = "ApiKey";
        private const string APIKEYS = "ApiKeys";
        public ApiKeyMiddleware(RequestDelegate next)
        {
            _next = next;
        }
        public async Task InvokeAsync(HttpContext context)
        {
            if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Api Key was not provided. (Using ApiKeyMiddleware) ");

                return;
            }

            var extractedApiKeyAsString = extractedApiKey.ToString();

            var appSettings = context.RequestServices.GetRequiredService<IConfiguration>();

            var apiKeys = appSettings.GetValue<string>(APIKEYS).Split(";").ToList();

            if (!apiKeys.Contains(extractedApiKeyAsString))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized client. (Using ApiKeyMiddleware)");

                return;
            }

            await _next(context);
        }
    }
}
