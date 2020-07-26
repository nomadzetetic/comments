using System;
using System.Text;
using Comments.App.GraphQL.Security;
using Comments.Data;
using Comments.Services.CommentsService;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App
{
  public class Startup
  {
    private readonly IConfiguration _configuration;
    private readonly IWebHostEnvironment _environment;
    
    public Startup(IWebHostEnvironment environment,IConfiguration configuration)
    {
      _environment = environment;
      _configuration = configuration;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddHttpContextAccessor();
      services.AddDbContext<CommentsDbContext>(options =>
      {
        var databaseConnectionString = Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
          _configuration.GetConnectionString("DefaultConnection");

        options.UseNpgsql(databaseConnectionString,
          builder => { builder.MigrationsAssembly("comments.data"); });
      });
      services.AddScoped<ICommentsService, CommentsServiceImpl>();
      var jwtSecret = Environment.GetEnvironmentVariable("JWT_SECRET") ?? _configuration["JwtSecret"];

      var symmetricSecurityKeyValue = Encoding.UTF8.GetBytes(jwtSecret);
      services.AddAuthentication(x =>
        {
          x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
          x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(x =>
        {
          x.RequireHttpsMetadata = !_environment.IsDevelopment();
          x.SaveToken = true;
          x.TokenValidationParameters = new TokenValidationParameters
          {
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = new SymmetricSecurityKey(symmetricSecurityKeyValue)
          };
        });

      services.AddAuthorization(options =>
      {
        options.AddPolicy(Constants.AccountPolicyName,
          policy => policy.Requirements.Add(new AccountPolicyRequirement()));
      }) ;
      services.AddScoped<IAuthorizationHandler, AccountPolicyAuthorizationHandler>();
      services.SetupGraphql();
    }

    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      using var serviceScope = app.ApplicationServices.GetRequiredService<IServiceScopeFactory>().CreateScope();
      using var dbContext = serviceScope.ServiceProvider.GetService<CommentsDbContext>();
      dbContext.Database.Migrate();

      app.UseWebSockets();
      
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
        app.UsePlayground(new PlaygroundOptions
        {
          QueryPath = "/graphql",
          Path = "/playground",
          EnableSubscription = true
        });
      }

      app.UseHttpsRedirection();
      app.UseCors(x => x
        .AllowAnyOrigin()
        .AllowAnyMethod()
        .AllowAnyHeader()
      );

      app.UseAuthentication();
      app.UseGraphQL(new QueryMiddlewareOptions
      {
        Path = "/graphql",
        EnableSubscriptions = true
      });
    }
  }
}