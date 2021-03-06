using System;
using System.Linq;
using Comments.App.GraphQL.Types;
using Comments.App.GraphQL.Types.Enums;
using Comments.App.GraphQL.Types.Inputs;
using Comments.Core;
using HotChocolate;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.App.Utils
{
  public static class Extensions
  {
    public static void SetupGraphql(this IServiceCollection services)
    {
      services.AddGraphQL(Schema);
    }

    public static bool IsCommentsAdministrator(this IHttpContextAccessor httpContextAccessor)
    {
      return httpContextAccessor
        .HttpContext
        .User
        .IsInRole(Constants.CommentsAdministratorRoleName);
    }

    public static Guid? AccountId(this IHttpContextAccessor httpContextAccessor)
    {
      var claim = httpContextAccessor
        .HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == Constants.AccountIdClaim);

      if (Guid.TryParse(claim?.Value, out var accountId))
        return accountId;

      return null;
    }

    public static Guid AccountIdExact(this IHttpContextAccessor httpContextAccessor)
    {
      var claim = httpContextAccessor
        .HttpContext
        .User
        .Claims
        .First(x => x.Type == Constants.AccountIdClaim);

      return Guid.Parse(claim.Value);
    }

    public static string AccountDisplayName(this IHttpContextAccessor httpContextAccessor)
    {
      var claim = httpContextAccessor
        .HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == Constants.AccountDisplayNameClaim);

      return claim?.Value?.Trim();
    }

    private static ISchema Schema(IServiceProvider serviceProvider)
    {
      return SchemaBuilder
        .New()
        .AddServices(serviceProvider)
        .AddAuthorizeDirectiveType()
        .AddType<AccountType>()
        .AddType<CommentType>()
        .AddType<CommentsPagedResultType>()
        .AddType<OrderByEnumType>()
        .AddType<CommentFieldEnumType>()
        .AddType<NewCommentInputType>()
        .AddType<GetCommentsInputType>()
        .AddType<UpdateCommentInputType>()
        .AddType<ReactionInputType>()
        .AddType<DeleteCommentInputType>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
        .Create();
    }
  }
}