using System;
using System.Net;

namespace Comments.Core
{
  public class CommentsException : ApplicationException
  {
    public object Value { get; protected set; }
    public HttpStatusCode StatusCode { get; }
    public CommentsException(string message, HttpStatusCode statusCode) : base(message)
    {
      StatusCode = statusCode;
    }
  }
}