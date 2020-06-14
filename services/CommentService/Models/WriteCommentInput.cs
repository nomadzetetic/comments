using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.CommentService.Models
{
  public class WriteCommentInput
  {
    public Guid TenantId { get; set; }
    public string ResourceId { get; set; }
    public Guid? ParentId { get; set; }
    private string _message { get; set; }
    public string Message {
      get => _message;
      set => _message = value?.Trim() ?? string.Empty;
    }
    public Guid CommentatorId { get; set; }
    private string _commentatorName;
    public string CommentatorName
    {
      get => _commentatorName;
      set => _commentatorName = value?.Trim() ?? string.Empty;
    }

    public class Validator : AbstractValidator<WriteCommentInput>
    {
      private Validator()
      {
        RuleFor(x => x.CommentatorName)
          .Matches("^[a-zA-Z0-9_-]*$")
          .WithMessage("Allowed only characters, numbers and '_' or '-'.");

        RuleFor(x => x.ResourceId)
          .MaximumLength(1000)
          .Matches("^[a-zA-Z0-9/_-]*$")
          .WithMessage("Allowed only characters, numbers, '_', '-' and '/'.");

        RuleFor(x => x.Message)
          .NotEmpty()
          .MinimumLength(1)
          .MaximumLength(5000);
      }
      
      public static async Task ValidateAndThrowAsync(WriteCommentInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}
