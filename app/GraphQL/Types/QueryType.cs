using Comments.App.GraphQL.Resolvers;
using Comments.App.GraphQL.Types.Inputs;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types
{
  public class QueryType : ObjectType<QueryResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<QueryResolver> descriptor)
    {
      descriptor.Name("Query");

      descriptor
        .Field(x => x.GetComments(default))
        .Argument("input", x => x.Type<NonNullType<GetCommentsInputType>>())
        .Type<NonNullType<CommentsPagedResultType>>();

#if TEST
      descriptor
        .Field(x => x.GetJwtToken(default, default, default))
        .Argument("commentsAdministrator", x => x.Type<NonNullType<BooleanType>>())
        .Argument("accountDisplayName", x => x.Type<StringType>())
        .Argument("accountId", x => x.Type<StringType>())
        .Type<NonNullType<StringType>>();
#endif
    }
  }
}