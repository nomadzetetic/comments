using System;
using System.Net;
using Comments.Core;

namespace Comments.Services.Exceptions
{
  public class AccountNotFoundException : CommentsException
  {
    public AccountNotFoundException(Guid accountId) : base("Account not found", HttpStatusCode.NotFound)
    {
      Value = accountId;
    }
  }
}