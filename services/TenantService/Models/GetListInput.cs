using Comments.Services.Enums;
using Comments.Services.TenantService.Enums;

namespace Comments.Services.TenantService.Models
{
  public class GetListInput
  {
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public SortDirectionEnum? Sort { get; set; }
    public OrderByEnum? OrderBy { get; set; }
  }
}