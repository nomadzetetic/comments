namespace Comments.Services.Validators
{
  public class AccountDisplayNameHolder
  {
    private string _accountDisplayName;

    public AccountDisplayNameHolder(string accountDisplayName)
    {
      AccountDisplayName = accountDisplayName;
    }

    public string AccountDisplayName
    {
      get => _accountDisplayName;
      set => _accountDisplayName = value?.Trim();
    }
  }
}