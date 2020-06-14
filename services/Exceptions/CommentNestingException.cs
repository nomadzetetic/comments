using System;

namespace Comments.Services.Exceptions
{
  public class CommentNestingException : ApplicationException
  {
    public Guid ParentId { get; set; }
    public Guid ParentParentId { get; set; }
    public CommentNestingException(Guid parentId, Guid parentParentId)
    {
      ParentId = parentId;
      ParentParentId = parentParentId;
    }
  }
}