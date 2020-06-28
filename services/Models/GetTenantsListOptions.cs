using Comments.Services.Enums;

namespace Comments.Services.Models
{
  public class GetTenantsListOptions
  {
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public SortDirectionEnum? Sort { get; set; }
    public TenantOrderByEnum? OrderBy { get; set; }
  }
}