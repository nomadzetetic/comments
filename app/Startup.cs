using System;
using System.Text;
using Comments.App.Extensions;
using Comments.App.Utils;
using Comments.Data;
using Comments.Services.CommentsService;
using Comments.Services.Constants;
using Comments.Services.TenantService;
using HotChocolate.AspNetCore;
using HotChocolate.AspNetCore.Playground;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App
{
  public class Startup
  {
    private readonly IWebHostEnvironment _environment;
    private readonly ICommentsConfig _commentsConfig;
    
    public Startup(ICommentsConfig commentsConfig, IWebHostEnvironment environment)
    {
      _environment = environment;
      _commentsConfig = commentsConfig;
    }

    public void ConfigureServices(IServiceCollection services)
    {
      services.AddCors();
      services.AddHttpContextAccessor();
      services.AddScoped<ITenantService, TenantService>();
      services.AddScoped<ICommentsService, CommentsService>();
      services.AddDbContext<CommentsDbContext>(options =>
      {
        var databaseConnectionString = _commentsConfig.DatabaseConnectionString;
        options.UseNpgsql(databaseConnectionString,
          builder => { builder.MigrationsAssembly("Comments.Data"); });
      });

      var jwtSecret = _commentsConfig.JwtTokenSecret;
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
        options.AddPolicy(
          Policy.WithCommentatorAndTenantValidation,
          policy => policy.Requirements.Add(new Services.CommentsSecurityPolicy.Requirement())
        );
      });
      services.AddScoped<IAuthorizationHandler, Services.CommentsSecurityPolicy.AuthorizationHandler>();
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
          Path = "/graphql",
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