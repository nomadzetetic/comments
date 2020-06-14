using System;

namespace Comments.Data.Entities
{
  public class Like
  {
    public Guid CommentId { get; set; }
    public Comment Comment { get; set; }
    public Guid AuthorId { get; set; }
    public Author Author { get; set; }
    public bool Value { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
