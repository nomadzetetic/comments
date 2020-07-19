using System;
using System.Threading.Tasks;
using Comments.App.Models;
using Comments.App.Security;
using Comments.Data.Entities;
using Comments.Services;

namespace Comments.App.Resolvers
{
  public class MutationResolver
  {
    private readonly ITenantService _tenantService;
    private readonly IAccountContext _accountContext;

    public MutationResolver(ITenantService tenantService, IAccountContext accountContext)
    {
      _tenantService = tenantService;
      _accountContext = accountContext;
    }

    public Task<Tenant> CreateTenant(string name) => _tenantService.Create(name);
    public Task<Tenant> RenameTenant(Guid tenantId, string name) => 
      _tenantService.Rename(tenantId, name?.Trim());
    public Task<Tenant> EnableTenant(Guid tenantId) => _tenantService.Enable(tenantId);
    public Task<Tenant> DisableTenant(Guid tenantId) => _tenantService.Disable(tenantId);
    public Task<Tenant> AddTenantToken(Guid tenantId) => _tenantService.AddToken(tenantId);
    public Task<Tenant> DeleteTenantToken(Guid tenantId, string token) =>
      _tenantService.DeleteToken(tenantId, token?.Trim());

    public Task<Comment> Comment(NewCommentInput input)
    {
      var tenant = _accountContext.Tenant;
      var account = _accountContext.Account;
      return null;
      // return _commentsService.WriteCommentAsync(new Services.CommentsService.Models.WriteCommentInput
      // {
      //   ResourceId = input.ResourceId,
      //   CommentatorId = commentator.Id,
      //   TenantId = tenant.Id,
      //   ParentId = input.ParentId,
      //   Message = input.Message?.Trim() ?? string.Empty
      // });
    }
  }
}
