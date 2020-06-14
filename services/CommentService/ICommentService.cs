using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.CommentService.Models;

namespace Comments.Services.CommentService
{
  public interface ICommentService
  {
    public Task<Comment> WriteComment(WriteCommentInput input);
  }
}
