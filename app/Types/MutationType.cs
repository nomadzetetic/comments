using Comments.App.Resolvers;
using Comments.App.Security;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class MutationType : ObjectType<MutationResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<MutationResolver> descriptor)
    {
      descriptor.Name("Mutation");

      descriptor
        .Field(x => x.Comment(default))
        .Authorize(AccountPolicy.Name)
        .Argument("input", x => x.Type<NonNullType<NewCommentInputType>>())
        .Type<NonNullType<CommentType>>();
      
      descriptor
        .Field(x => x.CreateTenant(default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<TenantType>>();

      descriptor
        .Field(x => x.DisableTenant(default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<TenantType>>();
      
      descriptor
        .Field(x => x.EnableTenant(default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<TenantType>>();
      
      descriptor
        .Field(x => x.RenameTenant(default, default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<TenantType>>();
      
      descriptor
        .Field(x => x.AddTenantToken(default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<TenantType>>();
      
      descriptor
        .Field(x => x.DeleteTenantToken(default, default))
        .Authorize(new [] { Roles.TenantsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Argument("token", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<TenantType>>();
    }
  }
}
