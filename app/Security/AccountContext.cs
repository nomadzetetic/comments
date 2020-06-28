using Comments.Data.Entities;
using Microsoft.AspNetCore.Http;

namespace Comments.App.Security
{
  public class AccountContext : IAccountContext
  {
    public AccountContext(IHttpContextAccessor httpContextAccessor)
    {
      _httpContextAccessor = httpContextAccessor;
    }

    private readonly IHttpContextAccessor _httpContextAccessor;

    public bool IsAdmin => _httpContextAccessor.HttpContext.Items.ContainsKey("isAdmin") &&
                           (bool) _httpContextAccessor.HttpContext.Items["isAdmin"];

    public bool IsModerator => _httpContextAccessor.HttpContext.Items.ContainsKey("isModerator") &&
                               (bool) _httpContextAccessor.HttpContext.Items["isModerator"];

    public Tenant Tenant => _httpContextAccessor.HttpContext.Items["tenant"] as Tenant;
    public Account Account => _httpContextAccessor.HttpContext.Items["account"] as Account;
  }
}
