using System;
using System.Net;
using Comments.Core;

namespace Comments.Services.Exceptions
{
  public class CommentNotFoundException : CommentsException
  {
    public CommentNotFoundException(Guid commentId) : base("Comment not found", HttpStatusCode.NotFound)
    {
      Value = commentId;
    }
  }
}
