using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.Models;
using Comments.Services.TenantService.Models;

namespace Comments.Services.TenantService
{
  public interface ITenantService
  {
    public Task<GenericPagedResult<Tenant>> GetList(GetListInput input);
    public Task<Tenant> Create(string name);
    public Task<Tenant> AddToken(Guid providerId);
    public Task<Tenant> DeleteToken(Guid providerId, string token);
    public Task<Tenant> Disable(Guid providerId);
    public Task<Tenant> Enable(Guid providerId);
    public Task<Tenant> GetById(Guid providerId);
    public Task<Tenant> Rename(Guid providerId, string name);
  }
}