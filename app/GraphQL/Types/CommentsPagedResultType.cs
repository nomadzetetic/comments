using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types
{
  public class CommentsPagedResultType : ObjectType<CommentsPagedResult>
  {
    protected override void Configure(IObjectTypeDescriptor<CommentsPagedResult> descriptor)
    {
      descriptor
        .Field(x => x.Data)
        .Type<NonNullType<ListType<CommentType>>>();

      descriptor
        .Field(x => x.Total)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Limit)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Page)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Pages)
        .Type<NonNullType<IntType>>();
    }
  }
}