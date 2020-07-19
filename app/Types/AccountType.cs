using Comments.Data.Entities;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class AccountType : ObjectType<Account>
  {
    protected override void Configure(IObjectTypeDescriptor<Account> descriptor)
    {
      descriptor
        .Field(x => x.Banned)
        .Type<NonNullType<BooleanType>>();

      descriptor
        .Field(x => x.Created)
        .Type<NonNullType<DateTimeType>>();

      descriptor
        .Field(x => x.Id)
        .Type<NonNullType<UuidType>>();

      descriptor
        .Field(x => x.Name)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.Tenant)
        .Ignore();

      descriptor
        .Field(x => x.Updated)
        .Type<NonNullType<DateTimeType>>();

      descriptor
        .Field(x => x.AvatarUrl)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.TenantId)
        .Ignore();
    }
  }
}