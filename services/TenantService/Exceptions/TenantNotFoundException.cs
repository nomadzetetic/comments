using System;

namespace Comments.Services.TenantService.Exceptions
{
  public class TenantNotFoundException : ApplicationException
  {
    public Guid TenantId { get; }
    public TenantNotFoundException(Guid tenantId)
    {
      TenantId = tenantId;
    }
  }
}