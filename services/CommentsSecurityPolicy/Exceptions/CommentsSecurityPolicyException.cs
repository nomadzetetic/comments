using System;

namespace Comments.Services.CommentsSecurityPolicy.Exceptions
{
  public class CommentsSecurityPolicyException : ApplicationException
  {
    public object Value { get; }
    public string PropertyName { get; }

    public CommentsSecurityPolicyException(string message) : base(message)
    {
      Value = default;
      PropertyName = default;
    }
    
    public CommentsSecurityPolicyException(string message, string propertyName) : base(message)
    {
      PropertyName = propertyName;
      Value = default;
    }
    
    public CommentsSecurityPolicyException(string message, string propertyName, object value) : base(message)
    {
      Value = value;
      PropertyName = propertyName;
    }
  }
}