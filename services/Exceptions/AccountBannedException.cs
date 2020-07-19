using System;

namespace Comments.Services.Exceptions
{
  public class AccountBannedException : CommentsException
  {
    public AccountBannedException(Guid accountId) : base("Account banned")
    {
      PropertyName = "AccountId";
      Value = accountId;
    }
  }
}
