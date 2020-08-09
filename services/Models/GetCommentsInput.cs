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
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public OrderByEnum? OrderBy { get; set; }
    public CommentFieldEnum? OrderByField { get; set; }
  }
}