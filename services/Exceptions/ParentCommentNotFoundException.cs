using System;

namespace Comments.Services.Exceptions
{
  public class ParentCommentNotFoundException : ApplicationException
  {
    public Guid TenantId { get; set; }
    public Guid ParentCommentId { get; set; }
    public ParentCommentNotFoundException(Guid tenantId, Guid parentCommentId)
    {
      TenantId = tenantId;
      ParentCommentId = parentCommentId;
    }
  }
}