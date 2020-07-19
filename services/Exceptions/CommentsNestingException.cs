using System;

namespace Comments.Services.Exceptions
{
  public class CommentsNestingException : CommentsException
  {
    public CommentsNestingException(Guid parentId) : base("Invalid parentId value (only 2 levels supported)")
    {
      PropertyName = "parentId";
      Value = parentId;
    }
  }
}
