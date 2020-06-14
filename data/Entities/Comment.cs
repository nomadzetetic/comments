using System;
using System.Collections.Generic;

namespace Comments.Data.Entities
{
  public class Comment
  {
    public Guid Id { get; set; }
    public Guid? ParentId { get; set; }
    public virtual Comment Parent { get; set; }
    public Guid AuthorId { get; set; }
    public virtual Author Author { get; set; }
    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; }
    public string ResourceKey { get; set; }
    public string Message { get; set; }
    public bool Deleted { get; set; }
    public int RepliesAmount { get; set; }
    public int LikesAmount { get; set; }
    public int DislikesAmount { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
    public ICollection<Comment> SubComments { get; set; }
  }
}
