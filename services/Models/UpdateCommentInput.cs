using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.Models
{
  public class UpdateCommentInput
  {
    private string _accountDisplayName;

    private string _message;

    public Guid CommentId { get; set; }
    public Guid AccountId { get; set; }

    public string AccountDisplayName
    {
      get => _accountDisplayName;
      set => _accountDisplayName = value?.Trim();
    }

    public string Message
    {
      get => _message;
      set => _message = value?.Trim();
    }

    public class Validator : AbstractValidator<UpdateCommentInput>
    {
      private Validator()
      {
        RuleFor(x => x.Message)
          .NotEmpty()
          .MinimumLength(1);
      }

      public static async Task ValidateAndThrowAsync(UpdateCommentInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}