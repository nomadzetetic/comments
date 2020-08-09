using System.Collections.Generic;
using Comments.Data.Entities;

namespace Comments.Services.Models
{
  public class CommentsPagedResult
  {
    public int Page { get; set; }
    public int Limit { get; set; }
    public int Total { get; set; }
    public int Pages { get; set; }
    public ICollection<Comment> Data { get; set; }
  }
}