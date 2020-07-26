using System;
using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.Models
{
  public class NewCommentInput
  {
    private string _resourceKey;
    private string _accountDisplayName;
    
    public Guid AccountId { get; set; }
    public Guid? ParentId { get; set; }

    public string AccountDisplayName
    {
      get => _accountDisplayName;
      set => _accountDisplayName = value?.Trim();
    }

    public string ResourceKey
    {
      get => _resourceKey;
      set => _resourceKey = value?.Trim();
    }
    
    private string _message;
    public string Message
    {
      get => _message;
      set => _message = value?.Trim();
    }

    public class Validator : AbstractValidator<NewCommentInput>
    {
      private Validator()
      {
        RuleFor(x => x.ResourceKey)
          .MaximumLength(1000)
          .Matches("^[a-zA-Z0-9/_-]*$")
          .WithMessage("Allowed only characters, numbers, '_', '-' and '/'.");
        
        RuleFor(x => x.Message)
          .NotEmpty()
          .MinimumLength(1)
          .MaximumLength(5000);
      }

      public static async Task ValidateAndThrowAsync(NewCommentInput input)
      {
        var validator = new Validator();
        await validator.ValidateAndThrowAsync(input);
      }
    }
  }
}