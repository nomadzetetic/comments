using Comments.Data.Entities;

namespace Comments.App.Security
{
  public interface IAccountContext
  {
    public bool IsAdmin { get; }
    public bool IsModerator { get; }
    public Tenant Tenant { get; }
    public Account Account { get; }
  }
}