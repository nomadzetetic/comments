using System;
using System.Data;
using System.Threading.Tasks;
using Comments.Data;
using Comments.Data.Entities;
using Comments.Services.CommentsService.Exceptions;
using Comments.Services.CommentsService.Models;
using Microsoft.EntityFrameworkCore;

namespace Comments.Services.CommentsService
{
  public class CommentsService : ICommentsService
  {
    private readonly CommentsDbContext _commentsDbContext;

    public CommentsService(CommentsDbContext commentsDbContext)
    {
      _commentsDbContext = commentsDbContext;
    }

    private async Task<Commentator> CreateCommentatorAsync(Guid tenantId, Guid commentatorId, string commentatorName)
    {
      var now = DateTimeOffset.Now;
      var commentator = new Commentator
      {
        Id = commentatorId,
        Name = commentatorName,
        Banned = false,
        Created = now,
        Updated = now,
        TenantId = tenantId
      };

      await _commentsDbContext.Commentators.AddAsync(commentator);
      await _commentsDbContext.SaveChangesAsync();
      return commentator;
    }

    public async Task<Commentator> EnsureCommentator(Guid tenantId, Guid commentatorId, string commentatorName)
    {
      var commentator = await _commentsDbContext
        .Commentators
        .FirstOrDefaultAsync(x => x.Id == commentatorId && x.TenantId == tenantId);

      commentator ??= await CreateCommentatorAsync(tenantId, commentatorId, commentatorName);
      
      if (commentator.Name == commentatorName || commentator.Banned) return commentator;
      
      commentator.Name = commentatorName;
      await _commentsDbContext.SaveChangesAsync();

      return commentator;
    }

    private async Task CheckNesting(Guid? parentId)
    {
      if (!parentId.HasValue) return;

      var tooDeep =  await _commentsDbContext
        .Comments
        .AnyAsync(x => x.ParentId != null && x.Id == parentId);

      if (tooDeep) throw new CommentsNestingException(parentId.Value);
    }

    public async Task<Comment> WriteCommentAsync(WriteCommentInput input)
    {
      await WriteCommentInput.Validator.ValidateAndThrowAsync(input);
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
      try
      {
        var now = DateTimeOffset.Now;
        await CheckNesting(input.ParentId);

        var comment = new Comment
        {
          TenantId = input.TenantId,
          CommentatorId = input.CommentatorId,
          Created = now,
          Updated = now,
          Dislikes = 0,
          Likes = 0,
          Replies = 0,
          ParentId = input.ParentId,
          Message = input.Message,
          ResourceId = input.ResourceId,
        };

        await _commentsDbContext.Comments.AddAsync(comment);
        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        
        return comment;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public Task<Comment> UpdateCommentAsync(UpdateCommentInput input)
    {
      throw new NotImplementedException();
    }
  }
}
