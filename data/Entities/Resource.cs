using System;

namespace Comments.Data.Entities
{
  public class Resource
  {
    public string ResourceKey { get; set; }
    public int Replies { get; set; }
    public int Likes { get; set; }
    public int Dislikes { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
  }
}
