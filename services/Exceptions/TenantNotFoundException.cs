using System;

namespace Comments.Services.Exceptions
{
  public class TenantNotFoundException : ApplicationException
  {
    public Guid TenantId { get; set; }
    public TenantNotFoundException(Guid tenantId)
    {
      TenantId = tenantId;
    }
  }
}