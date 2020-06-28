using System;
using System.Net;

namespace Comments.Services.Exceptions
{
  public class TenantNotFoundException : CommonException
  {
    public TenantNotFoundException(Guid tenantId) : base(HttpStatusCode.BadRequest)
    {
      TenantId = tenantId;
    }
    public Guid TenantId { get; }
  }
}