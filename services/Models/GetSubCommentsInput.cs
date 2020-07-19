using System;
using Comments.Services.Enums;

namespace Comments.Services.Models
{
  public class GetSubCommentsInput : IGetCommentsPagination
  {
    private string _resourceKey;
    
    public string ResourceKey
    {
      get => _resourceKey;
      set => _resourceKey = value?.Trim();
    }
    
    public Guid ParentId { get; set; }
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public CommentFieldEnum? SortByField { get; set; }
    public SortDirectionEnum? SortDirection { get; set; }
  }
}
