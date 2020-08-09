using System;
using System.Net;

namespace Comments.Core
{
  public class CommentsException : ApplicationException
  {
    public CommentsException(string message, HttpStatusCode statusCode) : base(message)
    {
      StatusCode = statusCode;
    }

    public object Value { get; protected set; }
    public HttpStatusCode StatusCode { get; }
  }
}