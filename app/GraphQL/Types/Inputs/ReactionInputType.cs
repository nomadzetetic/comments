using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types.Inputs
{
  public class ReactionInputType : InputObjectType<ReactionInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<ReactionInput> descriptor)
    {
      descriptor
        .Field(x => x.CommentId)
        .Type<NonNullType<UuidType>>();

      descriptor
        .Field(x => x.Value)
        .Type<BooleanType>();

      descriptor
        .Field(x => x.AccountId)
        .Ignore();

      descriptor
        .Field(x => x.AccountDisplayName)
        .Ignore();
    }
  }
}