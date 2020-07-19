namespace Comments.Services.Exceptions
{
  public class ForbiddenException : CommentsException
  {
    public ForbiddenException(string message, string propertyName, object value) : base(message, propertyName, value)
    {
    }
  }
}
