using System;

namespace Comments.Services.Exceptions
{
  public class AccountNotFoundException : CommentsException
  {
    public AccountNotFoundException(Guid accountId) : base("Account not found")
    {
      PropertyName = "accountId";
      Value = accountId;
    }
  }
}