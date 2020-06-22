using System;
using Microsoft.Extensions.Configuration;

namespace Comments.App.Utils
{
  public interface ICommentsConfig
  {
    public string JwtTokenSecret { get; }
    public string DatabaseConnectionString { get; }
  }
  
  public class CommentsConfig : ICommentsConfig
  {
    private readonly IConfiguration _configuration;

    public CommentsConfig(IConfiguration configuration)
    {
      _configuration = configuration;
    }

    public string JwtTokenSecret => Environment.GetEnvironmentVariable("JWT_SECRET") ?? _configuration["JwtSecret"];

    public string DatabaseConnectionString =>
      Environment.GetEnvironmentVariable("DB_CONNECTION_STRING") ??
      _configuration.GetConnectionString("DefaultConnection");
  }
}
