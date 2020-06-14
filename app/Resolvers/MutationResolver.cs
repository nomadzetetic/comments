using System;
using System.Threading.Tasks;
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

    public Task<Tenant> CreateTenant(string name) => _tenantService.Create(name);
    public Task<Tenant> RenameTenant(Guid tenantId, string name) => 
      _tenantService.Rename(tenantId, name?.Trim());
    public Task<Tenant> EnableTenant(Guid tenantId) => _tenantService.Enable(tenantId);
    public Task<Tenant> DisableTenant(Guid tenantId) => _tenantService.Disable(tenantId);
    public Task<Tenant> AddTenantToken(Guid tenantId) => _tenantService.AddToken(tenantId);
    public Task<Tenant> DeleteTenantToken(Guid tenantId, string token) =>
      _tenantService.DeleteToken(tenantId, token?.Trim());
  }
}
