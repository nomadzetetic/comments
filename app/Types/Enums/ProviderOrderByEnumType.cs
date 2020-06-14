using Comments.Services.TenantService.Enums;
using HotChocolate.Types;

namespace Comments.App.Types.Enums
{
  public class ProviderOrderByEnumType : EnumType<OrderByEnum>
  {
    protected override void Configure(IEnumTypeDescriptor<OrderByEnum> descriptor)
    {
      descriptor.Name("ProviderOrderByEnum");
    }
  }
}
