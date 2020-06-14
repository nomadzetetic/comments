using System;

namespace Comments.Data.Entities
{
  public class Reaction
  {
    public Guid CommentId { get; set; }
    public Comment Comment { get; set; }
    public Guid CommentatorId { get; set; }
    public Commentator Commentator { get; set; }
    public bool Value { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
