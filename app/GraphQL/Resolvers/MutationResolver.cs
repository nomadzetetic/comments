using System.Threading.Tasks;
using Comments.App.Utils;
using Comments.Data.Entities;
using Comments.Services.CommentsService;
using Comments.Services.Models;
using Microsoft.AspNetCore.Http;

namespace Comments.App.GraphQL.Resolvers
{
  public class MutationResolver
  {
    private readonly ICommentsService _commentsService;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public MutationResolver(ICommentsService commentsService, IHttpContextAccessor httpContextAccessor)
    {
      _commentsService = commentsService;
      _httpContextAccessor = httpContextAccessor;
    }

    public async Task<Comment> AddComment(NewCommentInput input)
    {
      input.AccountId = _httpContextAccessor.AccountIdExact();
      input.AccountDisplayName = _httpContextAccessor.AccountDisplayName();
      var comment = await _commentsService.AddComment(input);
      return comment;
    }

    public async Task<Comment> UpdateComment(UpdateCommentInput input)
    {
      input.AccountId = _httpContextAccessor.AccountIdExact();
      input.AccountDisplayName = _httpContextAccessor.AccountDisplayName();
      var comment = await _commentsService.UpdateComment(input);
      return comment;
    }

    public async Task<bool> DeleteComment(DeleteCommentInput input)
    {
      input.AccountId = _httpContextAccessor.AccountIdExact();
      input.IsAdministrator = _httpContextAccessor.IsCommentsAdministrator();
      var result = await _commentsService.DeleteComment(input);
      return result;
    }

    public async Task<Comment> Like(ReactionInput input)
    {
      input.AccountId = _httpContextAccessor.AccountIdExact();
      input.AccountDisplayName = _httpContextAccessor.AccountDisplayName();
      var comment = await _commentsService.Like(input);
      return comment;
    }
  }
}