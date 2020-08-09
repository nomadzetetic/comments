using Comments.App.GraphQL.Types.Enums;
using Comments.Services.Models;
using HotChocolate.Types;

namespace Comments.App.GraphQL.Types.Inputs
{
  public class GetCommentsInputType : InputObjectType<GetCommentsInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<GetCommentsInput> descriptor)
    {
      descriptor
        .Field(x => x.ParentId)
        .Type<UuidType>();

      descriptor
        .Field(x => x.ResourceKey)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.OrderBy)
        .Description("Default DESC")
        .Type<OrderByEnumType>();

      descriptor
        .Field(x => x.OrderByField)
        .Description("Default CREATED")
        .Type<CommentFieldEnumType>();

      descriptor
        .Description("Default 50 (min 1, max 100)")
        .Field(x => x.Limit)
        .Type<IntType>();

      descriptor
        .Description("Default 1 (min 1)")
        .Field(x => x.Page)
        .Type<IntType>();
    }
  }
}