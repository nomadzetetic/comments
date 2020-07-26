namespace Comments.App.GraphQL.Resolvers
{
  public class QueryResolver
  {
#if DEBUG
    // public string GetJwtToken(bool commentsAdministrator, string commentatorName, string commentatorId)
    // {
    //   var tokenHandler = new JwtSecurityTokenHandler();
    //   var key = Encoding.UTF8.GetBytes(_config.JwtTokenSecret);
    //
    //   var claimsIdentity = new ClaimsIdentity();
    //   if (commentsAdministrator)
    //     claimsIdentity.AddClaim(new Claim(ClaimTypes.Role, Roles.CommentsAdministrator));
    //
    //   // if (!string.IsNullOrWhiteSpace(commentatorName))
    //   //   claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorName, commentatorName.Trim()));
    //   //
    //   // if (!string.IsNullOrWhiteSpace(commentatorId))
    //   //   claimsIdentity.AddClaim(new Claim(ClaimTypeName.CommentatorId, commentatorId.Trim()));
    //
    //   var tokenDescriptor = new SecurityTokenDescriptor
    //   {
    //     Subject = claimsIdentity,
    //     IssuedAt = DateTimeOffset.Now.DateTime,
    //     Expires = DateTime.UtcNow.Add(TimeSpan.FromDays(30)),
    //     SigningCredentials =
    //       new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
    //   };
    //
    //   var token = tokenHandler.CreateToken(tokenDescriptor);
    //   var jwtToken = tokenHandler.WriteToken(token);
    //   return jwtToken;
    // }
#endif
  }
}