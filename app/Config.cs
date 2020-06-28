using System;
using Microsoft.Extensions.Configuration;

namespace Comments.App
{
  public interface IConfig
  {
    public string JwtTokenSecret { get; }
    public string DatabaseConnectionString { get; }
  }
  
  public class Config : IConfig
  {
    private readonly IConfiguration _configuration;

    public Config(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string JwtTokenSecret => Environment.GetEnvironmentVariable("JWT_SECRET") ?? _configuration["JwtSecret"];

    public string DatabaseConnectionString =>
      Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
      _configuration.GetConnectionString("DefaultConnection");
  }
}
