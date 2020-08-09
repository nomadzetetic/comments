using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Comments.App.Utils;
using Comments.Core;
using Comments.Services.CommentsService;
using Comments.Services.Models;
using Microsoft.IdentityModel.Tokens;

namespace Comments.App.GraphQL.Resolvers
{
  public class QueryResolver
  {
    private readonly ICommentsService _commentsService;
    private readonly IConfig _config;

    public QueryResolver(IConfig config, ICommentsService commentsService)
    {
      _config = config;
      _commentsService = commentsService;
    }

    public Task<CommentsPagedResult> GetComments(GetCommentsInput input)
    {
      return _commentsService.GetComments(input);
    }

#if TEST
    public string GetJwtToken(bool commentsAdministrator, string accountDisplayName, string accountId)
    {
      var tokenHandler = new JwtSecurityTokenHandler();
      var key = Encoding.UTF8.GetBytes(_config.GetJwtTokenSecret());

      var claimsIdentity = new ClaimsIdentity();
      if (commentsAdministrator)
        claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Constants.CommentsAdministratorRoleName));

      if (!string.IsNullOrWhiteSpace(accountDisplayName))
        claimsIdentity.AddClaim(new Claim(Constants.AccountDisplayNameClaim, accountDisplayName.Trim()));

      if (!string.IsNullOrWhiteSpace(accountId))
        claimsIdentity.AddClaim(new Claim(Constants.AccountIdClaim, accountId.Trim()));

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