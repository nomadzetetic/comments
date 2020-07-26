using System;
using Comments.Services.Enums;

namespace Comments.Services.Models
{
  public class GetCommentsInput
  {
    private string _resourceKey;
    public string ResourceKey
    {
      get => _resourceKey;
      set => _resourceKey = value?.Trim();
    }
    public Guid? ParentId { get; set; }
    public Guid? Cursor { get; set; }
    public CommentFieldEnum? SortByField { get; set; }
    public SortDirectionEnum? SortDirection { get; set; }
  }
}
