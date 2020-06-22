using FluentValidation;
using FluentValidation.Results;

namespace Comments.Services.CommentsService.Validators
{
  public class CommentatorNameValidator : AbstractValidator<CommentatorNameValidator.FluentValidatableString>
  {
    public class FluentValidatableString
    {
      public string CommentatorName { get; }

      public FluentValidatableString(string commentatorName)
      {
        CommentatorName = commentatorName;
      }
    }
    private CommentatorNameValidator()
    {
      RuleFor(x => x.CommentatorName)
        .NotEmpty()
        .MaximumLength(50)
        .Matches("^[a-zA-ZА-Яа-я0-9_-]*$")
        .WithMessage("Commentator Name contains not allowed characters.");
    }

    public static ValidationResult Validate(string commentatorName)
    {
      var validator = new CommentatorNameValidator();
      var validatableString = new FluentValidatableString(commentatorName);
      return validator.Validate(validatableString);
    }
  }
}