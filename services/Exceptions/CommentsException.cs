using System;

namespace Comments.Services.Exceptions
{
  public class CommentsException : ApplicationException
  {
    public object Value { get; protected set; }
    public string PropertyName { get; protected set; }

    public CommentsException(string message) : base(message)
    {
      Value = default;
      PropertyName = default;
    }
    
    public CommentsException(string message, string propertyName, object value) : base(message)
    {
      Value = value;
      PropertyName = propertyName;
    }
  }
}