using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.CommentsService.Models;

namespace Comments.Services.CommentsService
{
  public interface ICommentsService
  {
    public Task<Commentator> EnsureCommentator(Guid tenantId, Guid commentatorId, string commentatorName);
    public Task<Comment> WriteCommentAsync(WriteCommentInput input);
    public Task<Comment> UpdateCommentAsync(UpdateCommentInput input);
  }
}
