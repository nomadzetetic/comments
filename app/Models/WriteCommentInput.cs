using System;

namespace Comments.App.Models
{
  public class WriteCommentInput
  {
    public string Message { get; set; }
    public Guid? ParentId { get; set; }
  }
}