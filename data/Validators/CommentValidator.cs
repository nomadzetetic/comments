using Comments.Data.Entities;
using FluentValidation;

namespace Comments.Data.Validators
{
  public class CommentValidator : AbstractValidator<Comment>
  {
    public CommentValidator()
    {
      RuleFor(x => x.ResourceId)
        .MaximumLength(1000)
        .Matches("^[a-zA-Z0-9/_-]*$")
        .WithMessage("Allowed only characters, numbers, '_', '-' and '/'.");
        
      RuleFor(x => x.Message)
        .NotEmpty()
        .MinimumLength(1)
        .MaximumLength(5000);
    }
  }
}