using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.Models;

namespace Comments.Services.CommentsService
{
  public interface ICommentsService
  {
    public Task UpdateRepliesStats(Comment comment, Resource resource, DateTimeOffset now);
    public Task UpdateSocialActivitiesStats(Comment comment, Resource resource, DateTimeOffset now);
    public Task<Account> GetAccount(Guid accountId, bool throwIfBanned = true, bool throwIfNotFound = false);
    public Task<Account> CreateAccount(Guid accountId, string accountDisplayName, DateTimeOffset now);
    public Task<Comment> Like(ReactionInput input);
    public Task<Comment> AddComment(NewCommentInput input);
    public Task<bool> DeleteComment(DeleteCommentInput input);
    public Task<Comment> UpdateComment(UpdateCommentInput input);
    public Task<GenericPagedResult<Comment>> GetComments(GetCommentsInput input);
    public Task<GenericPagedResult<Comment>> GetSubComments(GetSubCommentsInput input);
  }
}
