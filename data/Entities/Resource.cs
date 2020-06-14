using System;

namespace Comments.Data.Entities
{
  public class Resource
  {
    public string ResourceId { get; set; }
    public Guid TenantId { get; set; }
    public Tenant Tenant { get; set; }
    public int Replies { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
  }
}