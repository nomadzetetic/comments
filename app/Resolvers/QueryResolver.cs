using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comments.App.Security;
using Comments.Data.Entities;
using Comments.Services;
using Comments.Services.Models;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App.Resolvers
{
  public class QueryResolver
  {
    private readonly IConfig _config;
    private readonly ITenantService _tenantService;

    public QueryResolver(IConfig config, ITenantService tenantService)
    {
      _tenantService = tenantService;
      _config = config;
    }

    public Task<Tenant> GetTenantById(Guid tenantId) =>
      _tenantService.GetById(tenantId);

    public Task<GenericPagedResult<Tenant>> GetTenantsList(GetTenantsListOptions options) =>
      _tenantService.GetList(options);

#if DEBUG
    public string GetJwtToken(bool commentsAdministrator, string commentatorName, string commentatorId)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_config.JwtTokenSecret);

      var claimsIdentity = new ClaimsIdentity();
      if (commentsAdministrator)
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.CommentsAdministrator));

      // if (!string.IsNullOrWhiteSpace(commentatorName))
      //   claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorName, commentatorName.Trim()));
      //
      // if (!string.IsNullOrWhiteSpace(commentatorId))
      //   claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorId, commentatorId.Trim()));

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
#endif
  }
}
