using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types
{
  public class NewCommentInputType : InputObjectType<NewCommentInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<NewCommentInput> descriptor)
    {
      descriptor
        .Field(x => x.ParentId)
        .Type<UuidType>();

      descriptor
        .Field(x => x.Message)
        .Type<NonNullType<StringType>>();
      
      descriptor
        .Field(x => x.ResourceKey)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.AccountId)
        .Ignore();

      descriptor
        .Field(x => x.AccountDisplayName)
        .Ignore();
    }
  }
}
