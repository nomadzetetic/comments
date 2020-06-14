using System;
using System.Collections.Generic;

namespace Comments.Data.Entities
{
  public class Provider
  {
    public Guid Id { get; set; }
    public string Name { get; set; }
    public bool Enabled { get; set; }
    public DateTimeOffset Created { get; set; }
    public DateTimeOffset Updated { get; set; }
    public List<string> Tokens { get; set; }
  }
}
