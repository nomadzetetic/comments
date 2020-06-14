using Comments.App.Types.Enums;
using Comments.Services.ProviderService.Models;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class GetProvidersInputType : InputObjectType<GetProvidersInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<GetProvidersInput> descriptor)
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
