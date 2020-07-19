using System;
using System.Collections.Generic;

namespace Comments.Data.Entities
{
  public class Comment
  {
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public virtual Comment Parent { get; set; }
    public Guid AccountId { get; set; }
    public virtual Account Account { get; set; }
    public string ResourceKey { get; set; }
    public Resource Resource { get; set; }
    public string Message { get; set; }
    public int Replies { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public ICollection<Comment> SubComments { get; set; }
  }
}
