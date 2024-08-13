using XIVLodestoneAPIServer.Helpers;

namespace XIVLodestoneAPIServer;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();

        var app = builder.Build();

        // Configure the HTTP request pipeline.

        app.UseHttpsRedirection();

        app.UseMiddleware<ApiKeyAuthoriseHelper>();
        app.UseAuthorization();


        app.MapControllers();

        app.Run();
    }
}