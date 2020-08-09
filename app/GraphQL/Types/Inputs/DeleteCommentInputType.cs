using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types.Inputs
{
  public class DeleteCommentInputType : InputObjectType<DeleteCommentInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<DeleteCommentInput> descriptor)
    {
      descriptor
        .Field(x => x.CommentId)
        .Type<NonNullType<UuidType>>();

      descriptor
        .Field(x => x.AccountId)
        .Ignore();

      descriptor
        .Field(x => x.IsAdministrator)
        .Ignore();
    }
  }
}