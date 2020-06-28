using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class GetTenantsListInputType : InputObjectType<GetTenantsListOptions>
  {
    protected override void Configure(IInputObjectTypeDescriptor<GetTenantsListOptions> descriptor)
    {
      descriptor.Name("GetTenantsListInput");
      
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
        .Type<TenantOrderByEnumType>();

      descriptor
        .Field(x => x.Sort)
        .Description("Default 'ASC'")
        .Type<TenantOrderByEnumType>();
    }
  }
}
