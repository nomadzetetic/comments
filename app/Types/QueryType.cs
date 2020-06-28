using Comments.App.Resolvers;
using Comments.App.Security;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class QueryType : ObjectType<QueryResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<QueryResolver> descriptor)
    {
      descriptor.Name("Query");

      descriptor
        .Field(x => x.GetTenantsList(default))
        .Authorize(new[] {Roles.CommentsAdministrator})
        .Name("tenants")
        .Argument("input", x => x.Type<GetTenantsListInputType>())
        .Type<NonNullType<TenantsPagedResultType>>();

      descriptor
        .Field(x => x.GetTenantById(default))
        .Authorize(new[] {Roles.CommentsAdministrator})
        .Name("tenant")
        .Argument("tenantId", x => x.Type<NonNullType<IdType>>())
        .Type<NonNullType<TenantType>>();

#if DEBUG
      descriptor
        .Field(x => x.GetJwtToken(default, default, default))
        .Argument("commentsAdministrator", x => x.Type<BooleanType>())
        .Argument("commentatorName", x => x.Type<StringType>())
        .Argument("commentatorId", x => x.Type<StringType>())
        .Type<NonNullType<StringType>>();
#endif
    }
  }
}
