using Comments.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Comments.Services.CommentsSecurityPolicy.Extensions
{
  public static class HttpContextAccessorExtensions
  {
    public static Tenant GetTenant(this IHttpContextAccessor httpContextAccessor)
    {
      httpContextAccessor.HttpContext.Items.TryGetValue("Tenant", out var tenant);
      return tenant as Tenant;
    }

    public static Commentator GetCommentator(this IHttpContextAccessor httpContextAccessor)
    {
      httpContextAccessor.HttpContext.Items.TryGetValue("Commentator", out var commentator);
      return commentator as Commentator;
    }

    public static bool IsTenantAdministrator(this IHttpContextAccessor httpContextAccessor)
    {
      httpContextAccessor.HttpContext.Items.TryGetValue("TenantAdministrator", out var tenantAdministrator);
      return tenantAdministrator != null && (bool) tenantAdministrator;
    }
  }
}