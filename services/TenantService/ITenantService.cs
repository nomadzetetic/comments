using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.Models;
using Comments.Services.TenantService.Models;

namespace Comments.Services.TenantService
{
  public interface ITenantService
  {
    public Task<GenericPagedResult<Tenant>> GetListAsync(GetListInput input);
    public Task<Tenant> CreateAsync(string name);
    public Task<Tenant> AddTokenAsync(Guid providerId);
    public Task<Tenant> DeleteTokenAsync(Guid providerId, string token);
    public Task<Tenant> DisableAsync(Guid providerId);
    public Task<Tenant> EnableAsync(Guid providerId);
    public Task<Tenant> GetByIdAsync(Guid providerId, bool throwNotFoundException = true);
    public Task<Tenant> RenameAsync(Guid providerId, string name);
  }
}