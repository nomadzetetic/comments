using System;

namespace Comments.Services.Models
{
  public class ReactionInput
  {
    private string _accountDisplayName;
    public Guid AccountId { get; set; }
    public Guid CommentId { get; set; }
    public string AccountDisplayName
    {
      get => _accountDisplayName;
      set => _accountDisplayName = value?.Trim();
    }
   
    public bool? Value { get; set; }
  }
}
