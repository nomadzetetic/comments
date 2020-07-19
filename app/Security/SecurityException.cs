using System.Net;
using Comments.Services.Exceptions;

namespace Comments.App.Security
{
  public class SecurityException : CommonException
  {
    public SecurityException(string message) : base(HttpStatusCode.Forbidden, message) { }
  }
}