using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.Models;

namespace Comments.Services
{
  public interface ITenantService
  {
    public Task<GenericPagedResult<Tenant>> GetList(GetTenantsListOptions options);
    public Task<Tenant> Create(string name);
    public Task<Tenant> AddToken(Guid tenantId);
    public Task<Tenant> DeleteToken(Guid tenantId, string token);
    public Task<Tenant> Disable(Guid tenantId);
    public Task<Tenant> Enable(Guid tenantId);
    public Task<Tenant> Rename(Guid tenantId, string name);
    public Task<Tenant> GetById(Guid tenantId, bool throwNotFoundException = true);
  }
}
