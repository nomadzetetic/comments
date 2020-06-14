using Comments.Data.Entities;
using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class ProvidersPagedResultType : ObjectType<GenericPagedResult<Tenant>>
  {
    protected override void Configure(IObjectTypeDescriptor<GenericPagedResult<Tenant>> descriptor)
    {
      descriptor
        .Field(x => x.Page)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Limit)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Pages)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Total)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Data)
        .Type<NonNullType<ListType<NonNullType<ProviderType>>>>();
    }
  }
}