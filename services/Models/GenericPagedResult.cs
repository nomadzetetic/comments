using System.Collections.Generic;

namespace Comments.Services.Models
{
  public class GenericPagedResult<T>
  {
    public int Page { get; set; }
    public int Pages { get; set; }
    public int Total { get; set; }
    public int Limit { get; set; }
    public ICollection<T> Data { get; set; }
  }
}
