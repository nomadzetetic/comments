using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types.Inputs
{
  public class UpdateCommentInputType : InputObjectType<UpdateCommentInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<UpdateCommentInput> descriptor)
    {
      descriptor
        .Field(x => x.Message)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.CommentId)
        .Type<NonNullType<UuidType>>();

      descriptor
        .Field(x => x.AccountId)
        .Ignore();

      descriptor
        .Field(x => x.AccountDisplayName)
        .Ignore();
    }
  }
}