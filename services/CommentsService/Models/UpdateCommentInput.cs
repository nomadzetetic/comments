using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.CommentsService.Models
{
  public class UpdateCommentInput
  {
    public Guid CommentId { get; set; }
    public Guid CommentatorId { get; set; }
    public string Message { get; set; }
    public class Validator : AbstractValidator<UpdateCommentInput>
    {
      private Validator()
      {
        RuleFor(x => x.Message)
          .NotEmpty()
          .MinimumLength(1)
          .MaximumLength(5000);
      }

      public static async Task ValidateAndThrowAsync(UpdateCommentInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}