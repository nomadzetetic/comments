using Comments.App.GraphQL.Resolvers;
using Comments.App.GraphQL.Types.Inputs;
using Comments.App.Utils;
using Comments.Core;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types
{
  public class MutationType : ObjectType<MutationResolver>
  {
    protected override void Configure(IObjectTypeDescriptor<MutationResolver> descriptor)
    {
      descriptor.Name("Mutation");

      descriptor.Field(x => x.AddComment(default))
        .Authorize(Constants.AccountPolicyName)
        .Argument("input", x => x.Type<NonNullType<NewCommentInputType>>())
        .Type<NonNullType<CommentType>>();

      descriptor.Field(x => x.UpdateComment(default))
        .Authorize(Constants.AccountPolicyName)
        .Argument("input", x => x.Type<NonNullType<UpdateCommentInputType>>())
        .Type<NonNullType<CommentType>>();

      descriptor.Field(x => x.DeleteComment(default))
        .Authorize(Constants.AccountPolicyName)
        .Argument("input", x => x.Type<NonNullType<DeleteCommentInputType>>())
        .Type<NonNullType<BooleanType>>();

      descriptor.Field(x => x.Like(default))
        .Authorize(Constants.AccountPolicyName)
        .Argument("input", x => x.Type<NonNullType<ReactionInputType>>())
        .Type<NonNullType<CommentType>>();
    }
  }
}