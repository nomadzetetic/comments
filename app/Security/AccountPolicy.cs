using System;
using System.Linq;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.App.Security
{
  public abstract class AccountPolicy
  {
    public const string Name = "AccountPolicy";
    public class Requirement : IAuthorizationRequirement { }
    public class AuthorizationHandler : AuthorizationHandler<Requirement, IResolverContext>
    {
      public AuthorizationHandler(IAccountService accountService, ITenantService tenantService,
        IHttpContextAccessor httpContextAccessor)
      {
        _tenantService = tenantService;
        _accountService = accountService;
        _httpContextAccessor = httpContextAccessor;
      }

      private readonly ITenantService _tenantService;
      private readonly IAccountService _accountService;
      private readonly IHttpContextAccessor _httpContextAccessor;

      private async Task<Tenant> GetTenant()
      {
        if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(RequestHeader.XTenantId,
          out var tenantIdValues))
          throw new SecurityException($"{RequestHeader.XTenantId} header has invalid value");

        if (!Guid.TryParse(tenantIdValues.FirstOrDefault(), out var tenantId))
          throw new SecurityException($"{RequestHeader.XTenantId} header has invalid value");

        var tenant = await _tenantService.GetById(tenantId, false);

        if (tenant == null)
          throw new SecurityException($"{RequestHeader.XTenantId} header has invalid value");

        if (!tenant.Enabled)
          throw new SecurityException("Tenant disabled");

        return tenant;
      }

      private async Task<Account> GetAccount(Guid tenantId)
      {
        var accountIdClaim = _httpContextAccessor
          .HttpContext
          .User
          .Claims
          .FirstOrDefault(x => x.Type == AccountClaim.AccountId);

        if (accountIdClaim == null)
          throw new SecurityException("Invalid account id");

        if (!Guid.TryParse(accountIdClaim.Value, out var accountId))
          throw new SecurityException("Invalid account id");

        var accountNameClaim = _httpContextAccessor
          .HttpContext
          .User
          .Claims
          .FirstOrDefault(x => x.Type == AccountClaim.AccountName);
        
        if (accountNameClaim == null || string.IsNullOrWhiteSpace(accountNameClaim.Value))
          throw new SecurityException("Invalid account name");

        var accountName = accountNameClaim.Value?.Trim();
        var account = await _accountService.GetAccount(tenantId, accountId);

        if (account == null)
          return await _accountService.CreateAccount(tenantId, accountId, accountName);

        if (account.Banned)
          throw new SecurityException("Account banned");

        if (account.Name == accountName) return account;

        account.Name = accountName;
        return await _accountService.UpdateAccount(account);
      }

      private bool IsUserAdmin(Tenant tenant)
      {
        var isAdmin = _httpContextAccessor.HttpContext.User.IsInRole(Roles.CommentsAdministrator);
        if (isAdmin) return true;

        if (tenant.Tokens.Count == 0)
          return false;

        if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue(RequestHeader.XTenantToken, out var tenantTokenValues))
          return false;

        var tokenValid = tenantTokenValues.Where(x => x != null).Intersect(tenant.Tokens).Any();
        return tokenValid;
      }

      private bool IsModerator()
      {
        return _httpContextAccessor.HttpContext.User.IsInRole(Roles.CommentsModerator);
      }
      
      protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement,
        IResolverContext resource)
      {
        var tenant = await GetTenant();
        var account = await GetAccount(tenant.Id);
        var isAdmin = IsUserAdmin(tenant);
        var isModerator = IsModerator();

        _httpContextAccessor.HttpContext.Items.Add("tenant", tenant);
        _httpContextAccessor.HttpContext.Items.Add("account", account);
        _httpContextAccessor.HttpContext.Items.Add("isAdmin", isAdmin);
        _httpContextAccessor.HttpContext.Items.Add("isModerator", isModerator);

        context.Succeed(requirement);
      }
    }
  }
}
