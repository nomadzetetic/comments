using Comments.App.Types.Enums;
using Comments.Services.TenantService.Models;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class GetTenantsListInputType : InputObjectType<GetListInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<GetListInput> descriptor)
    {
      descriptor
        .Field(x => x.Page)
        .Description("Default 1")
        .Type<IntType>();
      
      descriptor
        .Field(x => x.Limit)
        .Description("Default 10")
        .Type<IntType>();

      descriptor
        .Field(x => x.OrderBy)
        .Description("Default 'Name'")
        .Type<ProviderOrderByEnumType>();

      descriptor
        .Field(x => x.Sort)
        .Description("Default 'ASC'")
        .Type<ProviderOrderByEnumType>();
    }
  }
}
