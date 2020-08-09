using System;
using System.Net;
using Comments.Core;

namespace Comments.Services.Exceptions
{
  public class UpdateCommentForbiddenException : CommentsException
  {
    public UpdateCommentForbiddenException(string message, Guid commentId, Guid updaterAccountId) : base(message,
      HttpStatusCode.Forbidden)
    {
      Value = new
      {
        commentId,
        updaterAccountId
      };
    }
  }
}