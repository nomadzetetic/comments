using System;

namespace Comments.Services.CommentsService.Exceptions
{
  public class CommentsNestingException : ApplicationException
  {
    public Guid ParentId { get; }
    public CommentsNestingException(Guid parentId)
    {
      ParentId = parentId;
    }
  }
}