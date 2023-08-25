using Persistence;
using Microsoft.EntityFrameworkCore;
using API.Extensions;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddApplicationServices(builder.Configuration);
// note: An extension method is a method that extends a class

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthorization();

app.MapControllers();

// 'using' keyword tells .NET to clean this scope and everything inside it as soon as it's done being used
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try {   
    // essentially doing the same thing as the "dotnet ef database" command
    // creates db or updates db with the pending migrations
    var context = services.GetRequiredService<DataContext>();
    await context.Database.MigrateAsync();

    await Seed.SeedData(context);
}
catch (Exception ex) {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error occured during migration");
    throw;
}

app.Run();
