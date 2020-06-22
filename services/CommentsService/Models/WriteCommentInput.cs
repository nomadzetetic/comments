using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.CommentsService.Models
{
  public class WriteCommentInput
  {
    public Guid? ParentId { get; set; }
    public string ResourceId { get; set; }
    public string Message { get; set; }
    public Guid TenantId { get; set; }
    public Guid CommentatorId { get; set; }

    public class Validator : AbstractValidator<WriteCommentInput>
    {
      private Validator()
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

      public static async Task ValidateAndThrowAsync(WriteCommentInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}
