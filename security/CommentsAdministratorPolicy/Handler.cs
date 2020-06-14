using System.Threading.Tasks;
using Comments.Security.Constants;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;

namespace Comments.Security.CommentsAdministratorPolicy
{
  public class CommentsAdministratorAuthorizationHandler : 
    AuthorizationHandler<CommentsAdministratorRequirement, IResolverContext>
  {
    protected override Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      CommentsAdministratorRequirement requirement,
      IResolverContext resource)
    {
      if (context.User.HasClaim(ClaimTypeName.CommentsAdministrator, "true"))
      {
        context.Succeed(requirement);
      }

      return Task.CompletedTask;
    }
  }
}
