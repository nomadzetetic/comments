using System.Linq;
using System.Threading.Tasks;
using Comments.Security.Constants;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;

namespace Comments.Security.CommentsRequestPolicy
{
  public class CommentsRequestAuthorizationHandler : AuthorizationHandler<CommentsRequestRequirement, IResolverContext>
  {
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      CommentsRequestRequirement requirement,
      IResolverContext resource)
    {
      var commentatorIdClaim = context
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorId);

      if (commentatorIdClaim == null)
        return Task.CompletedTask;

      var commentatorNameClaim = context
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorName);

      return Task.CompletedTask;

      // var tenantId = _httpContextAccessor.HttpContext.Request.Headers["x-tenant-id"].ToString() ?? string.Empty;

      // if (!context.User..HasClaim(ClaimTypeName.CommentatorName, "true"))
      // {
      //   context.Succeed(requirement);
      // }
    }
  }
}