using System.Threading.Tasks;
using Comments.App.Utils;
using Comments.Services.Validators;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.App.GraphQL.Security
{
  public class AccountPolicyAuthorizationHandler : AuthorizationHandler<AccountPolicyRequirement, IResolverContext>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AccountPolicyAuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
      AccountPolicyRequirement requirement,
      IResolverContext resource)
    {
      var accountId = _httpContextAccessor.AccountId();
      var accountDisplayName = _httpContextAccessor.AccountDisplayName();
      AccountDisplayNameValidator.ValidateAndThrow(accountDisplayName);

      if (accountId.HasValue && !string.IsNullOrWhiteSpace(accountDisplayName))
        context.Succeed(requirement);

      return Task.CompletedTask;
    }
  }
}