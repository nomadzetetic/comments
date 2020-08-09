using FluentValidation;

namespace Comments.Services.Validators
{
  public class AccountDisplayNameValidator : AbstractValidator<AccountDisplayNameHolder>
  {
    private AccountDisplayNameValidator()
    {
      RuleFor(x => x.AccountDisplayName)
        .NotEmpty()
        .MaximumLength(50)
        .Matches("^[\\p{L}0-9_\\-]*$|^[\\p{L}0-9_\\-]*\\s{1}[\\p{L}0-9_\\-]*$")
        .WithMessage("Account Display Name contains not allowed characters.");
    }

    public static void ValidateAndThrow(string accountDisplayName)
    {
      var validator = new AccountDisplayNameValidator();
      var accountDisplayNameHolder = new AccountDisplayNameHolder(accountDisplayName);
      validator.ValidateAndThrow(accountDisplayNameHolder);
    }
  }
}