using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.ProviderService.Validators
{
  public class ProviderTokenValidator : AbstractValidator<ProviderTokenValidator.FluentValidatableString>
  {
    public class FluentValidatableString
    {
      public string Token { get; }
      public FluentValidatableString(string token)
      {
        Token = token;
      }
    }
    
    private ProviderTokenValidator()
    {
      RuleFor(x => x.Token).NotEmpty();
    }

    public static async Task ValidateAndThrowAsync(string token)
    {
      var validator = new ProviderTokenValidator();
      var validatableString = new FluentValidatableString(token);
      await validator.ValidateAndThrowAsync(validatableString);
    }
  }
}
