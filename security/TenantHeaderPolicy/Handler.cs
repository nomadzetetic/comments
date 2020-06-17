using System;
using System.Linq;
using System.Threading.Tasks;
using Comments.Services.TenantService;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.Security.TenantHeaderPolicy
{
  public class TenantHeaderAuthorizationHandler : AuthorizationHandler<TenantHeaderRequirement, IResolverContext>
  {
    private readonly ITenantService _tenantService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    public TenantHeaderAuthorizationHandler(IHttpContextAccessor httpContextAccessor, ITenantService tenantService)
    {
      _httpContextAccessor = httpContextAccessor;
      _tenantService = tenantService;
    }
    
    protected override async Task HandleRequirementAsync(
      AuthorizationHandlerContext context,
      TenantHeaderRequirement requirement,
      IResolverContext resource)
    {
      var foundHeaders = _httpContextAccessor
        .HttpContext
        .Request
        .Headers
        .TryGetValue("x-tenant-id", out var tenantIdValues);

      if (!foundHeaders)
        return;

      var tenantIdParsed = Guid.TryParse(tenantIdValues.FirstOrDefault(), out var tenantId);
      
      if (!tenantIdParsed)
        return;

      var tenant = await _tenantService.GetByIdAsync(tenantId);
      if (tenant.Enabled) {
        _httpContextAccessor.HttpContext.Items.Add("tenant", tenant);
        context.Succeed(requirement);
      }
    }
  }
}
