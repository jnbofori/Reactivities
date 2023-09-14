using Persistence;
using Microsoft.EntityFrameworkCore;
using API.Extensions;
using API.Middleware;
using Microsoft.AspNetCore.Identity;
using Domain;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using API.SignalR;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers(opt => 
{
    // note: this globally applies authentication for every endpoint
    var policy = new AuthorizationPolicyBuilder().RequireAuthenticatedUser().Build();
    opt.Filters.Add(new AuthorizeFilter(policy));
});
// note: An extension method is a method that extends a class
// the extensions below extended this class for adding services.
builder.Services.AddApplicationServices(builder.Configuration);
builder.Services.AddIdentityServices(builder.Configuration);

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseMiddleware<ExceptionMiddleware>();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("CorsPolicy");

app.UseAuthentication(); // note: authentication has to come first
app.UseAuthorization();

// note: this is telling the api to serve our static files (react app)
// its job is to look inside the 'wwwroot' folder and fish out anything
// called 'index.html/index.htm', and that's what it will use and serve 
// from server (kestrel)
app.UseDefaultFiles();
// to serve content inside 'wwwroot' (thats the default folder)
// could be configured to use some other folder name
app.UseStaticFiles();

app.MapControllers();
app.MapHub<ChatHub>("/chat");
app.MapFallbackToController("Index", "Fallback");

// note: 'using' keyword tells .NET to clean this scope and everything inside it as soon as it's done being used
using var scope = app.Services.CreateScope();
var services = scope.ServiceProvider;

try {   
    // note: essentially doing the same thing as the "dotnet ef database" command
    // creates db or updates db with the pending migrations
    var context = services.GetRequiredService<DataContext>();
    var userManager = services.GetRequiredService<UserManager<AppUser>>();
    await context.Database.MigrateAsync();

    await Seed.SeedData(context, userManager);
}
catch (Exception ex) {
    var logger = services.GetRequiredService<ILogger<Program>>();
    logger.LogError(ex, "Error occurred during migration");
    throw;
}

app.Run();
