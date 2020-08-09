using System;

namespace Comments.Services.Models
{
  public class DeleteCommentInput
  {
    public Guid AccountId { get; set; }
    public Guid CommentId { get; set; }
    public bool IsAdministrator { get; set; }
  }
}