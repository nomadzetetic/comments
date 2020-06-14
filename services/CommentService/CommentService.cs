// using System;
// using System.Data;
// using System.Threading.Tasks;
// using Comments.Data;
// using Comments.Data.Entities;
// using Comments.Services.CommentService.Models;
// using Comments.Services.Exceptions;
// using Microsoft.EntityFrameworkCore;
//
// namespace Comments.Services.CommentService
// {
//   public class CommentService : ICommentService
//   {
//     private readonly CommentsDbContext _commentsDbContext;
//     
//     public CommentService(CommentsDbContext commentsDbContext)
//     {
//       _commentsDbContext = commentsDbContext;
//     }
//
//     private async Task CheckCommentParentNestingAsync(WriteCommentInput input)
//     {
//       if (input.ParentId.HasValue)
//       {
//         var parentComment = await _commentsDbContext
//           .Comments
//           .FirstOrDefaultAsync(x => x.ParentId == input.ParentId && x.TenantId == input.TenantId);
//
//         if (parentComment == null)
//         {
//           throw new ParentCommentNotFoundException(input.TenantId, input.ParentId.Value);
//         }
//         
//         if (parentComment.ParentId.HasValue)
//         {
//           throw new CommentNestingException(input.ParentId.Value, parentComment.ParentId.Value);
//         }
//       }
//     }
//     private async Task<Commentator> EnsureCommentatorAsync(WriteCommentInput input)
//     {
//       var commentator = await _commentsDbContext
//         .Commentators
//         .FirstOrDefaultAsync(x => x.Id == input.CommentatorId && x.TenantId == input.TenantId);
//
//       if (commentator == null)
//       {
//         var now = DateTimeOffset.Now;
//         commentator = new Commentator
//         {
//           Name = input.CommentatorName,
//           Banned = false,
//           Created = now,
//           Updated = now,
//           TenantId = input.TenantId
//         };
//         await _commentsDbContext.Commentators.AddAsync(commentator);
//         await _commentsDbContext.SaveChangesAsync();
//       }
//       else if (commentator.Banned)
//       {
//         throw new CommentatorBannedException(input.CommentatorId);
//       }
//       else if (commentator.Name != input.CommentatorName)
//       {
//         commentator.Name = input.CommentatorName;
//         commentator.Updated = DateTimeOffset.Now;
//         await _commentsDbContext.SaveChangesAsync();
//       }
//       
//       return commentator;
//     }
//     
//     public async Task<Comment> WriteComment(WriteCommentInput input)
//     {
//       await WriteCommentInput.Validator.ValidateAndThrowAsync(input);
//       await using (var transaction = await _commentsDbContext.Database.BeginTransactionAsync(IsolationLevel.Snapshot))
//       {
//         try
//         {
//           await CheckCommentParentNestingAsync(input);
//           var commentator = await EnsureCommentatorAsync(input);
//           
//         }
//         catch
//         {
//           await transaction.RollbackAsync();
//           throw;
//         }
//       }
//
//       var now = DateTimeOffset.Now;
//       var comment = new Comment
//       {
//         ParentId = input.ParentId,
//         TenantId = input.TenantId,
//         CommentatorId = input.Commentator.Id,
//         ResourceId = input.ResourceId,
//         Message = input.Message,
//         Replies = 0,
//         Likes = 0,
//         Dislikes = 0,
//         Created = now,
//         Updated = now
//       };
//
//       await _commentsDbContext.Comments.AddAsync(comment);
//       await _commentsDbContext.SaveChangesAsync();
//       return comment;
//     }
//
//     
//   }
// }