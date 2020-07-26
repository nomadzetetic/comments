using Comments.Data.Entities;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types
{
  public class CommentType : ObjectType<Comment>
  {
    protected override void Configure(IObjectTypeDescriptor<Comment> descriptor)
    {
      descriptor
        .Field(x => x.Account)
        .Type<NonNullType<AccountType>>();
      
      descriptor
        .Field(x => x.Created)
        .Type<NonNullType<DateTimeType>>();
      
      descriptor
        .Field(x => x.Dislikes)
        .Type<NonNullType<IntType>>();
      
      descriptor
        .Field(x => x.Id)
        .Type<NonNullType<UuidType>>();

      descriptor
        .Field(x => x.Likes)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Message)
        .Type<NonNullType<StringType>>();
      
      descriptor
        .Field(x => x.Parent)
        .Ignore();

      descriptor
        .Field(x => x.ParentId)
        .Type<UuidType>();

      descriptor
        .Field(x => x.Replies)
        .Type<NonNullType<IntType>>();

      descriptor
        .Field(x => x.Resource)
        .Ignore();

      descriptor
        .Field(x => x.Updated)
        .Type<NonNullType<DateTimeType>>();

      descriptor
        .Field(x => x.AccountId)
        .Ignore();

      descriptor
        .Field(x => x.ResourceKey)
        .Ignore();
      
      descriptor
        .Field(x => x.SubComments)
        .Ignore();
    }
  }
}
