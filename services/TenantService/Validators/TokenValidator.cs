using System.Threading.Tasks;
using FluentValidation;

namespace Comments.Services.TenantService.Validators
{
  public class TenantTokenValidator : AbstractValidator<TenantTokenValidator.FluentValidatableString>
  {
    public class FluentValidatableString
    {
      public string Token { get; }
      public FluentValidatableString(string token)
      {
        Token = token;
      }
    }
    
    private TenantTokenValidator()
    {
      RuleFor(x => x.Token).NotEmpty();
    }

    public static async Task ValidateAndThrowAsync(string token)
    {
      var validator = new TenantTokenValidator();
      var validatableString = new FluentValidatableString(token);
      await validator.ValidateAndThrowAsync(validatableString);
    }
  }
}
