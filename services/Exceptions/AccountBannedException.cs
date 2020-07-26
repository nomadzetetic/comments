using System;
using System.Net;
using Comments.Core;

namespace Comments.Services.Exceptions
{
  public class AccountBannedException : CommentsException
  {
    public AccountBannedException(Guid accountId) : base( "Account banned", HttpStatusCode.Forbidden)
    {
      Value = accountId;
    }
  }
}
