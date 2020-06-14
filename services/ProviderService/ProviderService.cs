using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Comments.Data;
using Comments.Data.Entities;
using Comments.Services.Enums;
using Comments.Services.Models;
using Comments.Services.ProviderService.Enums;
using Comments.Services.ProviderService.Models;
using Comments.Services.ProviderService.Validators;
using Microsoft.EntityFrameworkCore;

namespace Comments.Services.ProviderService
{
  public class ProviderService : IProviderService
  {
    private readonly CommentsDbContext _commentsDbContext;
    
    public ProviderService(CommentsDbContext commentsDbContext)
    {
      _commentsDbContext = commentsDbContext;
    }
    
    public async Task<GenericPagedResult<Provider>> GetProviders(GetProvidersInput input)
    {
      var inputPage = input?.Page ?? 1;
      var inputLimit = input?.Limit ?? 10;

      if (inputPage < 1)
      {
        inputPage = 1;
      }
      
      var inputSort = input?.Sort ?? SortDirectionEnum.Asc;
      var inputOrderBy = input?.OrderBy ?? OrderByEnum.Name;

      var limit = inputLimit < 1 ? 1 : inputLimit > 100 ? 100 : inputLimit;
      var skip = (inputPage - 1) * limit;

      var query = _commentsDbContext
        .Providers
        .Take(limit)
        .Skip(skip);

      query = inputOrderBy switch
      {
        OrderByEnum.Name => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Name)
          : query.OrderByDescending(x => x.Name),
        OrderByEnum.Created => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Created)
          : query.OrderByDescending(x => x.Created),
        OrderByEnum.Updated => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Updated)
          : query.OrderByDescending(x => x.Updated),
        _ => query
      };

      var providers = await query.ToListAsync();
      var total = await _commentsDbContext.Providers.CountAsync();
      var pages = Math.Ceiling((double) total / limit);

      return new GenericPagedResult<Provider>()
      {
        Page = inputPage,
        Pages = double.IsNaN(pages) ? 0 : (int) pages,
        Total = total,
        Limit = limit,
        Data = providers
      };
    }
    public async Task<Provider> AddProvider(string name)
    {
      await ProviderNameValidator.ValidateAndThrowAsync(name);
      var now = DateTimeOffset.Now;

      var provider = new Provider
      {
        Name = name,
        Enabled = true,
        Created = now,
        Updated = now,
        Tokens = new List<string>()
      };

      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        await _commentsDbContext.Providers.AddAsync(provider);
        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }

      return provider;
    }
    public async Task<Provider> AddProviderToken(Guid providerId)
    {
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        var provider = await _commentsDbContext
          .Providers
          .FirstOrDefaultAsync(x => x.Id == providerId);
        
        if (provider == null)
          throw new ArgumentException(providerId.ToString(), nameof(providerId));

        var tokensHashSet = provider.Tokens.ToHashSet();
        var tokensPrevCount = tokensHashSet.Count;
        while (tokensPrevCount == tokensHashSet.Count)
        {
          var hmac = new HMACSHA256();
          tokensHashSet.Add(Convert.ToBase64String(hmac.Key).Replace("==", string.Empty));
        }

        provider.Tokens = tokensHashSet.ToList();
        provider.Updated = DateTimeOffset.Now;

        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return provider;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
    public async Task<Provider> DeleteProviderToken(Guid providerId, string token)
    {
      await ProviderTokenValidator.ValidateAndThrowAsync(token);
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        var provider = await _commentsDbContext
          .Providers
          .FirstOrDefaultAsync(x => x.Id == providerId);
        
        if (provider == null)
          throw new ArgumentException(providerId.ToString(), nameof(providerId));

        provider.Tokens.Remove(token);
        provider.Updated = DateTimeOffset.Now;

        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();
        return provider;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
    public async Task<Provider> DisableProvider(Guid providerId)
    {
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        var provider = await _commentsDbContext
          .Providers
          .FirstOrDefaultAsync(x => x.Id == providerId);

        if (provider == null)
          throw new ArgumentException(providerId.ToString(), nameof(providerId));
        
        provider.Enabled = false;
        provider.Updated = DateTimeOffset.Now;

        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return provider;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
    public async Task<Provider> EnableProvider(Guid providerId)
    {
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        var provider = await _commentsDbContext
          .Providers
          .FirstOrDefaultAsync(x => x.Id == providerId);
        
        if (provider == null)
          throw new ArgumentException(providerId.ToString(), nameof(providerId));

        provider.Enabled = true;
        provider.Updated = DateTimeOffset.Now;

        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return provider;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
    public async Task<Provider> GetProvider(Guid providerId)
    {
      var provider = await _commentsDbContext
        .Providers
        .FirstOrDefaultAsync(x => x.Id == providerId);

      if (provider == null)
        throw new ArgumentException(providerId.ToString(), nameof(providerId));
      
      return provider;
    }
    public async Task<Provider> RenameProvider(Guid providerId, string name)
    {
      await ProviderNameValidator.ValidateAndThrowAsync(name);
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      try
      {
        var provider = await _commentsDbContext
          .Providers
          .FirstOrDefaultAsync(x => x.Id == providerId);

        if (provider == null)
          throw new ArgumentException(providerId.ToString(), nameof(providerId));

        provider.Name = name;
        provider.Updated = DateTimeOffset.Now;

        await _commentsDbContext.SaveChangesAsync();
        await transaction.CommitAsync();

        return provider;
      }
      catch
      {
        await transaction.RollbackAsync();
        throw;
      }
    }
  }
}