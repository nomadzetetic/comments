using System;

namespace Comments.Services.Exceptions
{
  public class CommentNotFoundException : CommentsException
  {
    public CommentNotFoundException(Guid commentId) : base("Comment not found")
    {
      PropertyName = "commentId";
      Value = commentId;
    }
  }
}