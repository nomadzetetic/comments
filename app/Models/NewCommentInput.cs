using System;

namespace Comments.App.Models
{
  public class NewCommentInput
  {
    public string ResourceId { get; set; }
    public string Message { get; set; }
    public Guid? ParentId { get; set; }
  }
}