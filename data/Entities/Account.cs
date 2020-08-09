using System;

namespace Comments.Data.Entities
{
  public class Account
  {
    public Guid Id { get; set; }
    public bool Banned { get; set; }
    public string DisplayName { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public string AvatarUrl => $"/avatar/{Id:N}";
  }
}