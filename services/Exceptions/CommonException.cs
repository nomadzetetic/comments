using System;
using System.Net;

namespace Comments.Services.Exceptions
{
  public class CommonException : ApplicationException
  {
    public CommonException(HttpStatusCode code)
    {
      Code = code;
    }
    public CommonException(HttpStatusCode code, string message) : base(message)
    {
      Code = code;
    }
    public HttpStatusCode Code { get; }
  }
}