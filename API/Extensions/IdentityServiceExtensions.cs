using System.Text;
using API.Services;
using Domain;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Persistence;

namespace API.Extensions
{
  public static class IdentityServiceExtensions
  {
    public static IServiceCollection AddIdentityServices(this IServiceCollection services, IConfiguration config)
    {
      services.AddIdentityCore<AppUser>(opt =>
      {
        opt.Password.RequireNonAlphanumeric = false;
        opt.User.RequireUniqueEmail = true;
      })
      .AddEntityFrameworkStores<DataContext>(); // note: this allows us to query our entity framework store (ie. the db)

      var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["TokenKey"]));

      services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
          .AddJwtBearer(opt =>
          {
            opt.TokenValidationParameters = new TokenValidationParameters
            {
              // note: telling it what to validate
              ValidateIssuerSigningKey = true,
              IssuerSigningKey = key,
              ValidateIssuer = false,
              ValidateAudience = false
            };
          });

      // note: this means the token service is scoped to the http request itself
      services.AddScoped<TokenService>();

      return services;
    }
  }
}