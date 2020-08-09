using System;
using Microsoft.Extensions.Configuration;

namespace Comments.App.Utils
{
  public interface IConfig
  {
    public string GetJwtTokenSecret();
    public string GetDatabaseConnectionString();
  }

  public class Config : IConfig
  {
    private readonly IConfiguration _configuration;

    public Config(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string GetJwtTokenSecret()
    {
      return Environment.GetEnvironmentVariable("JWT_SECRET") ?? _configuration["JwtSecret"];
    }

    public string GetDatabaseConnectionString()
    {
      return Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
             _configuration.GetConnectionString("DefaultConnection");
    }
  }
}