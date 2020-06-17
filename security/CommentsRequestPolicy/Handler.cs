using System;
using System.Linq;
using System.Threading.Tasks;
using Comments.Security.Constants;
using Comments.Services.TenantService;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.Security.CommentsRequestPolicy
{
  public class CommentsRequestAuthorizationHandler : AuthorizationHandler<CommentsRequestRequirement, IResolverContext>
  {
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public CommentsRequestAuthorizationHandler(IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
    {
      _httpContextAccessor = httpContextAccessor;
      _tenantService = tenantService;
    }
    
    protected override async Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      CommentsRequestRequirement requirement,
      IResolverContext resource)
    {
      var commentatorIdClaim = context
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorId);

      if (commentatorIdClaim == null)
        return;

      var commentatorNameClaim = context
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorName);

      if (commentatorNameClaim == null)
        return;

      if (!_httpContextAccessor.HttpContext.Request.Headers.TryGetValue("x-tenant-id", out var tenantValuesId))
        return;

      if (!Guid.TryParse(tenantValuesId.FirstOrDefault(), out var tenantId))
        return;

      var tenant = await _tenantService.GetByIdAsync(tenantId);
      _httpContextAccessor.HttpContext.Items.Add("tenant", tenant);
      _httpContextAccessor.HttpContext.Items.Add("commentatorId");.Items.Add("tenant", tenant);


      return Task.CompletedTask;

      // var tenantId = _httpContextAccessor.HttpContext.Request.Headers["x-tenant-id"].ToString() ?? string.Empty;

      // if (!context.User..HasClaim(ClaimTypeName.CommentatorName, "true"))
      // {
      //   context.Succeed(requirement);
      // }
    }
  }
}