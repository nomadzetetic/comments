using System;
using System.Threading.Tasks;
using Comments.Data;
using Comments.Data.Entities;
using Comments.Data.Validators;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Comments.Services
{
  public class AccountService : IAccountService
  {
    private readonly CommentsDbContext _dbContext;

    public AccountService(CommentsDbContext dbContext)
    {
      _dbContext = dbContext;
    }

    public Task<Account> GetAccount(Guid tenantId, Guid accountId)
    {
      return _dbContext
        .Accounts
        .FirstOrDefaultAsync(x => x.Id == accountId && x.TenantId == tenantId);
    }

    public async Task<Account> CreateAccount(Guid tenantId, Guid accountId, string name)
    {
      var now = DateTimeOffset.Now;
      var account = new Account
      {
        Id = accountId,
        Name = name,
        Banned = false,
        Created = now,
        Updated = now,
        TenantId = tenantId
      };
      var accountValidator = new AccountValidator();
      await accountValidator.ValidateAndThrowAsync(account);
      await _dbContext.Accounts.AddAsync(account);
      await _dbContext.SaveChangesAsync();
      return account;
    }

    public async Task<Account> UpdateAccount(Account account)
    {
      var accountValidator = new AccountValidator();
      await accountValidator.ValidateAndThrowAsync(account);
      account.Updated = DateTimeOffset.Now;
      _dbContext.Attach(account);
      await _dbContext.SaveChangesAsync();
      return account;
    }
  }
}
