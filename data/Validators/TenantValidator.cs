using Comments.Data.Entities;
using FluentValidation;

namespace Comments.Data.Validators
{
  public class TenantValidator : AbstractValidator<Tenant>
  {
    public TenantValidator()
    {
      RuleFor(x => x.Name)
        .NotEmpty()
        .MaximumLength(50)
        .Matches("^[a-zA-Z0-9_-]*$")
        .WithMessage("Allowed only latin characters, numbers and '_' or '-'.");
    }
  }
}