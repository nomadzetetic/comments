using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comments.App.Utils;
using Comments.Data.Entities;
using Comments.Services.Constants;
using Comments.Services.Models;
using Comments.Services.TenantService;
using Comments.Services.TenantService.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using ClaimTypeName = Comments.Security.Constants.ClaimTypeName;

namespace Comments.App.Resolvers
{
  public class QueryResolver
  {
    private readonly ICommentsConfig _commentsConfig;
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    
    public QueryResolver(
      ICommentsConfig commentsConfig,
      ITenantService tenantService,
      IHttpContextAccessor httpContextAccessor)
    {
      _commentsConfig = commentsConfig;
      _tenantService = tenantService;
      _httpContextAccessor = httpContextAccessor;
    }
    
    public Task<Tenant> GetTenantById(Guid tenantId) => 
      _tenantService.GetByIdAsync(tenantId);

    public Task<GenericPagedResult<Tenant>> GetTenantsList(GetListInput input) => 
      _tenantService.GetListAsync(input);

#if DEBUG
    public string GetJwtToken(bool commentsAdministrator, string commentatorName, string commentatorId)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_commentsConfig.JwtTokenSecret);

      var claimsIdentity = new ClaimsIdentity();
      if (commentsAdministrator)
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.CommentsAdministrator));

      if (!string.IsNullOrWhiteSpace(commentatorName))
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorName, commentatorName.Trim()));

      if (!string.IsNullOrWhiteSpace(commentatorId))
        claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorId, commentatorId.Trim()));

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
