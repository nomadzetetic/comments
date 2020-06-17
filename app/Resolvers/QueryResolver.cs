using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Security.Constants;
using Comments.Services.Models;
using Comments.Services.TenantService;
using Comments.Services.TenantService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App.Resolvers
{
  public class QueryResolver
  {
    private readonly IConfiguration _configuration;
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public QueryResolver(
      IConfiguration configuration,
      ITenantService tenantService,
      IHttpContextAccessor httpContextAccessor)
    {
      _configuration = configuration;
      _tenantService = tenantService;
      _httpContextAccessor = httpContextAccessor;
    }

    public int Comment()
    {
      return 43;
    }
    
    public Task<Tenant> GetTenantById(Guid tenantId) => 
      _tenantService.GetByIdAsync(tenantId);

    public Task<GenericPagedResult<Tenant>> GetTenantsList(GetListInput input) => 
      _tenantService.GetList(input);

    public string GetJwtToken(bool commentsAdministrator, string commentatorName, string commentatorId)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_configuration["JwtSecret"]);

      var claimsIdentity = new ClaimsIdentity();
      if (commentsAdministrator)
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentsAdministrator, "true"));
      }

      if (!string.IsNullOrWhiteSpace(commentatorName))
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorName, commentatorName.Trim()));
      }

      if (!string.IsNullOrWhiteSpace(commentatorId))
      {
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorId, commentatorId.Trim()));
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
