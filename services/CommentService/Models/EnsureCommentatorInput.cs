using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.CommentService.Models
{
  public class EnsureCommentatorInput
  {
    public Guid TenantId { get; set; }
    public Guid CommentatorId { get; set; }
    private string _commentatorName;
    public string CommentatorName
    {
      get => _commentatorName;
      set => _commentatorName = value?.Trim() ?? string.Empty;
    }

    public class Validator : AbstractValidator<EnsureCommentatorInput>
    {
      private Validator()
      {
        RuleFor(x => x.CommentatorName)
          .NotEmpty()
          .MinimumLength(3)
          .MaximumLength(50);

        RuleFor(x => x.CommentatorName)
          .Matches("^[a-zA-Z0-9_-]*$")
          .WithMessage("Allowed only characters, numbers and '_' or '-'.");
      }
      
      public static async Task ValidateAndThrowAsync(EnsureCommentatorInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}