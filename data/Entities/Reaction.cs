using System;

namespace Comments.Data.Entities
{
  public class Reaction
  {
    public Guid CommentId { get; set; }
    public Comment Comment { get; set; }
    public Guid AccountId { get; set; }
    public Account Account { get; set; }
    public string ResourceKey { get; set; }
    public Resource Resource { get; set; }
    public bool Value { get; set; }
    public DateTimeOffset Created { get; set; }
  }
}
