using Comments.App.Resolvers;
using Comments.Security.Constants;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class MutationType : ObjectType<MutationResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<MutationResolver> descriptor)
    {
      descriptor.Name("Mutation");
      
      descriptor
        .Field(x => x.AddProvider(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();

      descriptor
        .Field(x => x.DisableProvider(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("providerId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.EnableProvider(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("providerId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.RenameProvider(default, default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("providerId", x => x.Type<NonNullType<UuidType>>())
        .Argument("name", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.AddProviderToken(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("providerId", x => x.Type<NonNullType<UuidType>>())
        .Type<NonNullType<ProviderType>>();
      
      descriptor
        .Field(x => x.DeleteProviderToken(default, default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Argument("providerId", x => x.Type<NonNullType<UuidType>>())
        .Argument("token", x => x.Type<NonNullType<StringType>>())
        .Type<NonNullType<ProviderType>>();
    }
  }
}
