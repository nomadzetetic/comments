using System;
using System.Net;
using Comments.Core;

namespace Comments.Services.Exceptions
{
  public class CommentsNestingException : CommentsException
  {
    public CommentsNestingException(Guid parentId) : base("Invalid parentId value (only 2 levels supported)",
      HttpStatusCode.Forbidden)
    {
      Value = parentId;
    }
  }
}