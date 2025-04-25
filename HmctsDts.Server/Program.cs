using HmctsDts.Server.Extensions;

namespace HmctsDts.Server;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
        builder.Services.AddApplicationServices(builder.Configuration);

        var app = builder.Build();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();
        }

        app.UseCors(x => x.AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
            .WithOrigins("http://localhost:5173")
        );

        app.UseAuthorization();

        app.MapControllers();

        app.MapGet("/status", () => Results.Ok());

        app.Run();
    }
}