using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Comments.Data;
using Comments.Data.Entities;
using Comments.Services.Enums;
using Comments.Services.Exceptions;
using Comments.Services.Models;
using Microsoft.EntityFrameworkCore;

namespace Comments.Services.CommentsService
{
  public class CommentsServiceImpl : ICommentsService
  {
    private readonly CommentsDbContext _dbContext;

    public CommentsServiceImpl(CommentsDbContext dbContext)
    {
      _dbContext = dbContext;
    }
   
    private async Task<Account> GetAccountById(Guid accountId, bool throwIfBanned = true, bool throwIfNotFound = false)
    {
      var account = await _dbContext
        .Accounts
        .FirstOrDefaultAsync(x => x.Id == accountId);

      if (throwIfNotFound && account == null)
        throw new AccountNotFoundException(accountId);
        
      if (throwIfBanned && account != null && account.Banned)
        throw new AccountBannedException(account.Id);
      
      return account;
    }
    
    private async Task<Account> CreateAccount(Guid accountId, string accountDisplayName, DateTimeOffset now)
    {
      var account = new Account
      {
        Id = accountId,
        DisplayName = accountDisplayName,
        Banned = false,
        Created = now,
        Updated = now
      };

      await _dbContext.Accounts.AddAsync(account);
      await _dbContext.SaveChangesAsync();
      return account;
    }

    private async Task<bool> IsParentIdTooDeep(Guid? parentId)
    {
      if (!parentId.HasValue) return false;

      var tooDeep = await _dbContext
        .Comments
        .AnyAsync(x => x.ParentId != null && x.Id == parentId);

      return tooDeep;
    }

    private async Task<Resource> GetOrCreateResource(string resourceKey)
    {
      var resource = await _dbContext.Resources.FirstOrDefaultAsync(x => x.ResourceKey == resourceKey);

      if (resource != null) return resource;

      var now = DateTimeOffset.Now;
      resource = new Resource
      {
        Created = now,
        Updated = now,
        ResourceKey = resourceKey,
        Dislikes = 0,
        Likes = 0,
        Replies = 0
      };

      await _dbContext.Resources.AddAsync(resource);
      await _dbContext.SaveChangesAsync();

      return resource;
    }

    private async Task UpdateSocialActivitiesStats(Comment comment, Resource resource, DateTimeOffset now)
    {
      comment.Likes = await _dbContext
        .Reactions
        .CountAsync(x => x.ResourceKey == resource.ResourceKey && x.CommentId == comment.Id && x.Value);
      
      comment.Dislikes = await _dbContext
        .Reactions
        .CountAsync(x => x.ResourceKey == resource.ResourceKey && x.CommentId == comment.Id && x.Value == false);

      comment.Updated = now;
      
      await _dbContext.SaveChangesAsync();

      if (comment.ParentId.HasValue)
      {
        var parentComment = await _dbContext
          .Comments
          .FirstAsync(x => x.Id == comment.ParentId.Value);

        parentComment.Likes = await _dbContext
          .Comments
          .Where(x => x.ParentId == parentComment.Id)
          .SumAsync(x => x.Likes);
        
        parentComment.Dislikes = await _dbContext
          .Comments
          .Where(x => x.ParentId == parentComment.Id)
          .SumAsync(x => x.Dislikes);

        parentComment.Updated = now;
        
        await _dbContext.SaveChangesAsync();
      }

      resource.Likes = await _dbContext
        .Comments
        .Where(x => x.ResourceKey == resource.ResourceKey && x.ParentId == null)
        .SumAsync(x => x.Likes);
      
      resource.Dislikes = await _dbContext
        .Comments
        .Where(x => x.ResourceKey == resource.ResourceKey && x.ParentId == null)
        .SumAsync(x => x.Dislikes);

      resource.Updated = now;
      
      await _dbContext.SaveChangesAsync();
    }

    public async Task<Comment> AddComment(NewCommentInput input)
    {
      await NewCommentInput.Validator.ValidateAndThrowAsync(input);

      var account = await GetAccountById(input.AccountId);

      await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
      try
      {
        var now = DateTimeOffset.Now;

        account ??= await CreateAccount(input.AccountId, input.AccountDisplayName, now);

        if (input.ParentId.HasValue && await IsParentIdTooDeep(input.ParentId))
          throw new CommentsNestingException(input.ParentId.Value);

        var resource = await GetOrCreateResource(input.ResourceKey);
        var comment = new Comment
        {
          AccountId = account.Id,
          Created = now,
          Updated = now,
          Dislikes = 0,
          Likes = 0,
          Replies = 0,
          ParentId = input.ParentId,
          Message = input.Message,
          ResourceKey = resource.ResourceKey
        };

        await _dbContext.Comments.AddAsync(comment);
        await _dbContext.SaveChangesAsync();

        resource.Replies = await _dbContext
          .Comments
          .CountAsync(x => x.ResourceKey == resource.ResourceKey);
        resource.Updated = now;
        await _dbContext.SaveChangesAsync();

        if (input.ParentId.HasValue)
        {
          var parentComment = await _dbContext
            .Comments
            .FirstAsync(x => x.Id == input.ParentId.Value);

          parentComment.Replies = await _dbContext
            .Comments
            .CountAsync(x => x.ParentId == parentComment.Id);
          parentComment.Updated = now;
          await _dbContext.SaveChangesAsync();
        }

        await transaction.CommitAsync();

        return comment;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
    
    public async Task<Comment> UpdateComment(UpdateCommentInput input)
    {
      await UpdateCommentInput.Validator.ValidateAndThrowAsync(input);

      await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
      try
      {
        var account = await GetAccountById(input.AccountId, throwIfNotFound: true, throwIfBanned: true);
        var now = DateTimeOffset.Now;
        account.DisplayName = input.AccountDisplayName;
        account.Updated = now;

        var comment = await _dbContext.Comments.FirstOrDefaultAsync(x => x.Id == input.CommentId);

        if (comment == null)
          throw new CommentNotFoundException(input.CommentId);

        if (comment.AccountId != account.Id)
          throw new UpdateCommentForbiddenException(
            "This comment does not belong to yours account",
            input.CommentId, account.Id);

        comment.Message = input.Message;
        comment.Updated = now;

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return comment;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<bool> DeleteComment(DeleteCommentInput input)
    {
      await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
      try
      {
        await GetAccountById(input.AccountId, throwIfNotFound: true, throwIfBanned: true);

        var commentQuery = _dbContext.Comments.Where(x => x.Id == input.CommentId);
        if (!input.IsAdministrator)
          commentQuery = commentQuery.Where(x => x.AccountId == input.AccountId);

        var comment = await commentQuery.AsNoTracking().FirstOrDefaultAsync();

        if (comment == null)
          throw new CommentNotFoundException(input.CommentId);

        await _dbContext.Database.ExecuteSqlRawAsync("delete from \"comments\" where ParentId = {0}", comment.Id);
        await _dbContext.Database.ExecuteSqlRawAsync("delete from \"comments\" where Id = {0}", comment.Id);

        var resource = await _dbContext
          .Resources
          .FirstAsync(x => x.ResourceKey == comment.ResourceKey);

        var now = DateTimeOffset.Now;

        resource.Replies = await _dbContext
          .Comments
          .CountAsync(x => x.ResourceKey == comment.ResourceKey);
        
        resource.Updated = now;

        if (comment.ParentId.HasValue)
        {
          var parentComment = await _dbContext
            .Comments
            .FirstAsync(x => x.Id == comment.ParentId);

          parentComment.Replies = await _dbContext
            .Comments
            .CountAsync(x => x.ParentId == parentComment.Id);
          
          parentComment.Updated = now;
        }

        await _dbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return true;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }

    public async Task<ICollection<Comment>> GetComments(GetCommentsInput input)
    {
      var query = _dbContext
        .Comments
        .Include(x => x.Resource)
        .Include(x => x.Account)
        .Where(x => x.ResourceKey == input.ResourceKey);

      var sortDirection = input?.SortDirection ?? SortDirectionEnum.Desc;
      var sortByField = input?.SortByField ?? CommentFieldEnum.Created;

      query = sortByField switch
      {
        CommentFieldEnum.Created => sortDirection == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Created)
          : query.OrderByDescending(x => x.Created),
        CommentFieldEnum.Likes => sortDirection == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Likes)
          : query.OrderByDescending(x => x.Likes),
        CommentFieldEnum.Dislikes => sortDirection == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Dislikes)
          : query.OrderByDescending(x => x.Dislikes),
        CommentFieldEnum.Replies => sortDirection == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Replies)
          : query.OrderByDescending(x => x.Replies),
        _ => query
      };

      query = input?.ParentId != null
        ? query.Where(x => x.ParentId != null && x.ParentId == input.ParentId)
        : query.Where(x => x.ParentId == null);

      query = input?.Cursor != null ? query.SkipWhile(x => x.Id == input.Cursor).Take(50) : query.Take(50);
      var comments = await query.AsNoTracking().ToListAsync();
      return comments;
    }

    public async Task UpdateRepliesStats(Comment comment, Resource resource, DateTimeOffset now)
    {
      resource.Replies = await _dbContext
        .Comments
        .CountAsync(x => x.ResourceKey == resource.ResourceKey);
      
      resource.Updated = now;

      await _dbContext.SaveChangesAsync();

      if (comment.ParentId.HasValue)
      {
        var parentComment = await _dbContext
          .Comments
          .FirstAsync(x => x.Id == comment.ParentId.Value);

        parentComment.Replies = await _dbContext
          .Comments
          .CountAsync(x => x.ParentId == parentComment.Id);
        
        parentComment.Updated = now;

        await _dbContext.SaveChangesAsync();
      }
    }
    
    public async Task<Comment> Like(ReactionInput input)
    {
      await using var transaction = await _dbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot);
      try
      {
        var account = await GetAccountById(input.AccountId);

        var now = DateTimeOffset.Now;
        account ??= await CreateAccount(input.AccountId, input.AccountDisplayName, now);

        var comment = await _dbContext
          .Comments
          .Include(x => x.Resource)
          .FirstOrDefaultAsync(x => x.Id == input.CommentId);

        if (comment == null)
          throw new CommentNotFoundException(input.CommentId);

        var resource = await _dbContext
          .Resources
          .FirstAsync(x => x.ResourceKey == comment.ResourceKey);
        
        var reaction = await _dbContext
          .Reactions
          .FirstOrDefaultAsync(x => x.AccountId == account.Id 
                                    && x.CommentId == comment.Id
                                    && x.ResourceKey == resource.ResourceKey);

        var doUpdateStats = false;
        if (reaction != null && !input.Value.HasValue)
        {
          _dbContext.Reactions.Remove(reaction);
          await _dbContext.SaveChangesAsync();
          doUpdateStats = true;
        }
        else if (reaction != null)
        {
          reaction.Value = input.Value.Value;
          reaction.Created = now;
          await _dbContext.SaveChangesAsync();
          doUpdateStats = true;
        }

        if (doUpdateStats)
          await UpdateSocialActivitiesStats(comment, resource, now);

        await transaction.CommitAsync();

        return comment;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
  }
}
