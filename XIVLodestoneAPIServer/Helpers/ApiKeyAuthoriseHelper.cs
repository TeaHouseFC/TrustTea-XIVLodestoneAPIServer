namespace XIVLodestoneAPIServer.Helpers;

public class ApiAuthoriseHelper
{
    private readonly RequestDelegate _next;
    private const string APIKEY = "12345";
    
    public ApiAuthoriseHelper(RequestDelegate next)
    {
        _next = next;
    }
    
    public async Task InvokeAsync(HttpContext context, IConfiguration configuration)
    {
        var apiKey = configuration.GetValue<string>(APIKEY);
        
        if (!context.Request.Headers.TryGetValue(APIKEY, out var extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("API Key was not provided.");
            return;
        }
        
        if (!apiKey.Equals(extractedApiKey))
        {
            context.Response.StatusCode = 401;
            await context.Response.WriteAsync("Invalid API Key Provided");
            return;
        }
        await _next(context);
    }
}