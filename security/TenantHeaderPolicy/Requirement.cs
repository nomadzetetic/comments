using Microsoft.AspNetCore.Authorization;

namespace Comments.Security.TenantHeaderPolicy
{
  public class TenantHeaderRequirement : IAuthorizationRequirement { }
}
