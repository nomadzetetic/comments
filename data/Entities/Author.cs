using System;

namespace Comments.Data.Entities
{
  public class Author
  {
    public Guid Id { get; set; }
    public bool Banned { get; set; }
    public Guid ProviderId { get; set; }
    public Provider Provider { get; set; }
    public string Name { get; set; }
    public string AvatarUrl { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset? Updated { get; set; }
  }
}
