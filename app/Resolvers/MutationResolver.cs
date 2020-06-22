using System;
using System.Threading.Tasks;
using Comments.App.Models;
using Comments.Data.Entities;
using Comments.Services.TenantService;

namespace Comments.App.Resolvers
{
  public class MutationResolver
  {
    private readonly ITenantService _tenantService;

    public MutationResolver(ITenantService tenantService)
    {
      _tenantService = tenantService;
    }

    public Task<Tenant> CreateTenant(string name) => _tenantService.CreateAsync(name);
    public Task<Tenant> RenameTenant(Guid tenantId, string name) => 
      _tenantService.RenameAsync(tenantId, name?.Trim());
    public Task<Tenant> EnableTenant(Guid tenantId) => _tenantService.EnableAsync(tenantId);
    public Task<Tenant> DisableTenant(Guid tenantId) => _tenantService.DisableAsync(tenantId);
    public Task<Tenant> AddTenantToken(Guid tenantId) => _tenantService.AddTokenAsync(tenantId);
    public Task<Tenant> DeleteTenantToken(Guid tenantId, string token) =>
      _tenantService.DeleteTokenAsync(tenantId, token?.Trim());

    public Task<Comment> WriteComment(WriteCommentInput input)
    {
      
    }
  }
}
