using HmctsDts.Server.Data;
using HmctsDts.Server.Extensions;
using Microsoft.EntityFrameworkCore;

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

        app.UseAuthorization();


        app.MapControllers();

        app.MapGet("/status", () => Results.Ok());

        app.Run();
    }
}