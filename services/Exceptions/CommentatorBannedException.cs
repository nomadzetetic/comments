using System;

namespace Comments.Services.Exceptions
{
  public class CommentatorBannedException : ApplicationException
  {
    public Guid CommentatorId { get; set; }
    public CommentatorBannedException(Guid commentatorId)
    {
      CommentatorId = commentatorId;
    }
  }
}