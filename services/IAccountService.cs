using System;
using System.Threading.Tasks;
using Comments.Data.Entities;

namespace Comments.Services
{
  public interface IAccountService
  {
    public Task<Account> GetAccount(Guid tenantId, Guid accountId);
    public Task<Account> CreateAccount(Guid tenantId, Guid accountId, string name);
    public Task<Account> UpdateAccount(Account account);
  }
}
