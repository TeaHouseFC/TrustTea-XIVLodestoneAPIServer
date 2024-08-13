namespace XIVLodestoneAPIServer.Helpers;

public class ApiKeyAuthoriseHelper
{
    private readonly RequestDelegate _next;
    private const string APIKEYNAME = "ApiKey";
    private const string KeyValue = "12345";
    public ApiKeyAuthoriseHelper(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        // TODO
        // NOT hardcode this API Key, fine for now in local testing
        // However move to API Key/Name to docker compose config later on
        // var apiKey = configuration.GetValue<string>(APIKEYNAME);
        if (!context.Request.Headers.TryGetValue(APIKEYNAME, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }
        
        if (!KeyValue.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key Provided");
            return;
        }
        await _next(context);
    }
}