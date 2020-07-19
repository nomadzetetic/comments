using System;
using System.Linq;
using System.Threading.Tasks;
using Comments.Services.Constants;
using Comments.Services.Exceptions;
using Comments.Services.Validators;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.Services.CommentsSecurityPolicy
{
  public class AuthorizationHandler : AuthorizationHandler<Requirement, IResolverContext>
  {
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public AuthorizationHandler(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }
 
    private Guid GetAccountId()
    {
      var accountIdClaim = HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.AccountId);

      if (!Guid.TryParse(accountIdClaim?.Value, out var accountId))
        throw new ForbiddenException($"{ClaimTypeName.AccountId} has invalid claim",
          ClaimTypeName.AccountId, accountIdClaim?.Value);

      return accountId;
    }

    private string GetAccountDisplayName()
    {
      var accountDisplayNameClaim = HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.AccountDisplayName);

      var accountDisplayName = accountDisplayNameClaim?.Value?.Trim();
      AccountDisplayNameValidator.ValidateAndThrow(accountDisplayName);
      
      return accountDisplayName;
    }

    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement,
      IResolverContext resource)
    {
      var accountId = GetAccountId();
      var accountDisplayName = GetAccountDisplayName();
      var isAdministrator = context.User.IsInRole(Constants.Roles.CommentsAdministrator);

      HttpContext.Items["AccountId"] = accountId;
      HttpContext.Items["IsAdministrator"] = isAdministrator;
      HttpContext.Items["AccountDisplayName"] = accountDisplayName;
      context.Succeed(requirement);
      return Task.CompletedTask;
    }
  }
}