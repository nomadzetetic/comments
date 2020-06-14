using Comments.App.Resolvers;
using Comments.Security.Constants;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class QueryType : ObjectType<QueryResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<QueryResolver> descriptor)
    {
      descriptor.Name("Query");
      
      descriptor
        .Field(x => x.GetProviders(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Name("providers")
        .Argument("input", x => x.Type<GetProvidersInputType>())
        .Type<NonNullType<ProvidersPagedResultType>>();
      
      descriptor
        .Field(x => x.GetProvider(default))
        .Authorize(AuthorizationPolicyName.CommentsAdministrator)
        .Name("provider")
        .Argument("providerId", x => x.Type<NonNullType<IdType>>())
        .Type<NonNullType<ProviderType>>();

      descriptor
        .Field(x => x.GetJwtToken(default, default, default, default))
        .Argument("commentsAdministrator", x => x.Type<BooleanType>())
        .Argument("authorName", x => x.Type<StringType>())
        .Argument("authorId", x => x.Type<StringType>())
        .Argument("authorAvatarUrl", x => x.Type<StringType>())
        .Type<NonNullType<StringType>>();
    }
  }
}
