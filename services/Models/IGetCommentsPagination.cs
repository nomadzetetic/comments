using Comments.Services.Enums;

namespace Comments.Services.Models
{
  public interface IGetCommentsPagination
  {
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public CommentFieldEnum? SortByField { get; set; }
    public SortDirectionEnum? SortDirection { get; set; }
  }
}