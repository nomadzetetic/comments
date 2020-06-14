using Comments.Services.Enums;
using HotChocolate.Types;

namespace Comments.App.Types.Enums
{
  public class SortDirectionEnumType : EnumType<SortDirectionEnum>
  {
    protected override void Configure(IEnumTypeDescriptor<SortDirectionEnum> descriptor)
    {
      descriptor.Name("SortDirectionEnum");
    }
  }
}
