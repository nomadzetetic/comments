using Comments.Data.Entities;
using FluentValidation;

namespace Comments.Data.Validators
{
  public class AccountValidator : AbstractValidator<Account>
  {
    public AccountValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .Matches("^[a-zA-ZА-Яа-я0-9_-]*$")
        .WithMessage("Allowed only characters, numbers and '_' or '-'.");
    }
  }
}