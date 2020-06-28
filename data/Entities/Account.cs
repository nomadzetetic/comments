using System;

namespace Comments.Data.Entities
{
  public class Account
  {
    public Guid Id { get; set; }
    public bool Banned { get; set; }
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public string Name { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public string AvatarUrl => $"/avatar/{Id:N}";
  }
}
