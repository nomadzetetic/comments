using Comments.App.Resolvers;
using Comments.Services.Constants;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class MutationType : ObjectType<MutationResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<MutationResolver> descriptor)
    {
      descriptor.Name("Mutation");
      
      descriptor
        .Field(x => x.CreateTenant(default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();

      descriptor
        .Field(x => x.DisableTenant(default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.EnableTenant(default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.RenameTenant(default, default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.AddTenantToken(default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.DeleteTenantToken(default, default))
        .Authorize(new [] { Roles.CommentsAdministrator })
        .Argument("tenantId", x => x.Type<NonNullType<UuidType>>())
        .Argument("token", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();
    }
  }
}
