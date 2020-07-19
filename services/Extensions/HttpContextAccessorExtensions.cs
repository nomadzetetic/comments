using System;
using Microsoft.AspNetCore.Http;

namespace Comments.Services.Extensions
{
  public static class HttpContextAccessorExtensions
  {
    public static Guid? GetAccountId(this IHttpContextAccessor httpContextAccessor)
    {
      httpContextAccessor.HttpContext.Items.TryGetValue("AccountId", out var accountId);
      return accountId as Guid?;
    }

    public static string GetAccountDisplayName(this IHttpContextAccessor httpContextAccessor)
    {
      httpContextAccessor.HttpContext.Items.TryGetValue("AccountDisplayName", out var accountDisplayName);
      return accountDisplayName as string;
    }
  }
}