using Comments.App.Models;
using HotChocolate.Types;

namespace Comments.App.Types
{
  public class NewCommentInputType : InputObjectType<NewCommentInput>
  {
    protected override void Configure(IInputObjectTypeDescriptor<NewCommentInput> descriptor)
    {
      descriptor
        .Field(x => x.Message)
        .Type<NonNullType<StringType>>();
      
      descriptor
        .Field(x => x.ResourceId)
        .Type<NonNullType<StringType>>();

      descriptor
        .Field(x => x.ParentId)
        .Type<UuidType>();
    }
  }
}