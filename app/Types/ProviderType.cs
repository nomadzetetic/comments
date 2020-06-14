using Comments.Data.Entities;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class ProviderType : ObjectType<Tenant>
  {
    protected override void Configure(IObjectTypeDescriptor<Tenant> descriptor)
    {
      descriptor
        .Field(x => x.Id)
        .Type<NonNullType<IdType>>();

      descriptor
        .Field(x => x.Name)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.Enabled)
        .Type<NonNullType<BooleanType>>();

      descriptor
        .Field(x => x.Created)
        .Type<NonNullType<DateTimeType>>();

      descriptor
        .Field(x => x.Updated)
        .Type<DateTimeType>();

      descriptor
        .Field(x => x.Tokens)
        .Type<NonNullType<ListType<NonNullType<StringType>>>>();
    }
  }
}
