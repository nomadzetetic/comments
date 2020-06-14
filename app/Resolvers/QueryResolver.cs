using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Security.Constants;
using Comments.Services.Models;
using Comments.Services.ProviderService;
using Comments.Services.ProviderService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App.Resolvers
{
  public class QueryResolver
  {
    private readonly IConfiguration _configuration;
    private readonly IProviderService _providerService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public QueryResolver(
      IConfiguration configuration,
      IProviderService providerService,
      IHttpContextAccessor httpContextAccessor)
    {
      _configuration = configuration;
      _providerService = providerService;
      _httpContextAccessor = httpContextAccessor;
    }

    public Task<Provider> GetProvider(Guid providerId) => 
      _providerService.GetProvider(providerId);

    public Task<GenericPagedResult<Provider>> GetProviders(GetProvidersInput input) => 
      _providerService.GetProviders(input);

    public string GetJwtToken(bool commentsAdministrator, string authorName, string authorId, string authorAvatarUrl)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_configuration["JwtSecret"]);

      var claimsIdentity = new ClaimsIdentity();
      if (commentsAdministrator)
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentsAdministrator, "true"));
      }

      if (!string.IsNullOrWhiteSpace(authorName))
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.AuthorName, authorName.Trim()));
      }

      if (!string.IsNullOrWhiteSpace(authorId))
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.AuthorId, authorId.Trim()));
      }

      if (!string.IsNullOrWhiteSpace(authorAvatarUrl))
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.AuthorAvatarUrl, authorAvatarUrl.Trim()));
      }

      var tokenDescriptor = new SecurityTokenDescriptor
      {
        Subject = claimsIdentity,
        IssuedAt = DateTimeOffset.Now.DateTime,
        Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
        SigningCredentials =
          new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
      };

      var token = tokenHandler.CreateToken(tokenDescriptor);
      var jwtToken = tokenHandler.WriteToken(token);
      return jwtToken;
    }
  }
}
