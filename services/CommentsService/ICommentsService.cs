using System.Collections.Generic;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.Models;

namespace Comments.Services.CommentsService
{
  public interface ICommentsService
  {
    public Task<Comment> Like(ReactionInput input);
    public Task<Comment> AddComment(NewCommentInput input);
    public Task<bool> DeleteComment(DeleteCommentInput input);
    public Task<Comment> UpdateComment(UpdateCommentInput input);
    public Task<ICollection<Comment>> GetComments(GetCommentsInput input);
  }
}
