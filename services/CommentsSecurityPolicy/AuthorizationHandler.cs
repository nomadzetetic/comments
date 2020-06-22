using System;
using System.Linq;
using System.Threading.Tasks;
using Comments.Services.CommentsSecurityPolicy.Exceptions;
using Comments.Services.CommentsService;
using Comments.Services.CommentsService.Validators;
using Comments.Services.Constants;
using Comments.Services.TenantService;
using HotChocolate.Resolvers;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;

namespace Comments.Services.CommentsSecurityPolicy
{
  public class AuthorizationHandler : AuthorizationHandler<Requirement, IResolverContext>
  {
    private readonly ITenantService _tenantService;
    private readonly ICommentsService _commentsService;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private HttpContext HttpContext => _httpContextAccessor.HttpContext;

    public AuthorizationHandler(IHttpContextAccessor httpContextAccessor, ITenantService tenantService,
      ICommentsService commentsService)
    {
      _tenantService = tenantService;
      _commentsService = commentsService;
      _httpContextAccessor = httpContextAccessor;
    }

    private Guid GetTenantId()
    {
      if (!HttpContext.Request.Headers.TryGetValue("x-tenant-id", out var tenantIdValues))
        throw new CommentsSecurityPolicyException("x-tenant-id header not found", "x-tenant-id");

      if (!Guid.TryParse(tenantIdValues.FirstOrDefault(), out var tenantId))
        throw new CommentsSecurityPolicyException("x-tenant-id header has invalid value", "x-tenant-id",
          tenantIdValues.FirstOrDefault());

      return tenantId;
    }

    private string GetTenantToken()
    {
      return HttpContext.Request.Headers.TryGetValue("x-tenant-token", out var tenantTokenValues)
        ? tenantTokenValues.FirstOrDefault()?.Trim()
        : null;
    }

    private Guid GetCommentatorId()
    {
      var commentatorIdClaim = HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorId);

      if (!Guid.TryParse(commentatorIdClaim?.Value, out var commentatorId))
        throw new CommentsSecurityPolicyException($"{ClaimTypeName.CommentatorId} has invalid claim",
          ClaimTypeName.CommentatorId, commentatorIdClaim?.Value);

      return commentatorId;
    }

    private string GetCommentatorName()
    {
      var commentatorNameClaim = HttpContext
        .User
        .Claims
        .FirstOrDefault(x => x.Type == ClaimTypeName.CommentatorName);

      var commentatorName = commentatorNameClaim?.Value?.Trim();
      var validationResult = CommentatorNameValidator.Validate(commentatorName);

      if (!validationResult.IsValid)
        throw new CommentsSecurityPolicyException(validationResult.Errors.FirstOrDefault()?.ErrorMessage,
          ClaimTypeName.CommentatorName, commentatorName);

      return commentatorName;
    }

    protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, Requirement requirement,
      IResolverContext resource)
    {
      var tenantId = GetTenantId();
      var tenantToken = GetTenantToken();
      var commentatorId = GetCommentatorId();
      var commentatorName = GetCommentatorName();

      var tenant = await _tenantService.GetByIdAsync(tenantId, false);

      if (tenant == null)
        throw new CommentsSecurityPolicyException("Tenant not found");

      if (!tenant.Enabled)
        throw new CommentsSecurityPolicyException("Tenant disabled");

      if (!string.IsNullOrWhiteSpace(tenantToken) && tenant.Tokens.All(x => x != tenantToken))
        throw new CommentsSecurityPolicyException("Tenant token invalid");

      var commentator = await _commentsService.EnsureCommentator(tenantId, commentatorId, commentatorName);

      if (commentator.Banned)
        throw new CommentsSecurityPolicyException("Commentator banned");

      HttpContext.Items["Tenant"] = tenant;
      HttpContext.Items["Commentator"] = commentator;
      HttpContext.Items["TenantAdministrator"] = !string.IsNullOrWhiteSpace(tenantToken);
      context.Succeed(requirement);
    }
  }
}