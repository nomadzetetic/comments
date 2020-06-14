using Comments.Services.Enums;
using Comments.Services.ProviderService.Enums;

namespace Comments.Services.ProviderService.Models
{
  public class GetProvidersInput
  {
    public int? Page { get; set; }
    public int? Limit { get; set; }
    public SortDirectionEnum? Sort { get; set; }
    public OrderByProviderFieldEnum? OrderBy { get; set; }
  }
}