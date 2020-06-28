using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Threading.Tasks;
using Comments.Data;
using Comments.Data.Entities;
using Comments.Data.Validators;
using Comments.Services.Enums;
using Comments.Services.Exceptions;
using Comments.Services.Models;
using FluentValidation;
using Microsoft.EntityFrameworkCore;

namespace Comments.Services
{
  public class TenantService : ITenantService
  {
    private readonly CommentsDbContext _commentsDbContext;

    public TenantService(CommentsDbContext commentsDbContext)
    {
      _commentsDbContext = commentsDbContext;
    }

    public async Task<GenericPagedResult<Tenant>> GetList(GetTenantsListOptions options)
    {
      var inputPage = options?.Page ?? 1;
      var inputLimit = options?.Limit ?? 10;

      if (inputPage < 1)
      {
        inputPage = 1;
      }

      var inputSort = options?.Sort ?? SortDirectionEnum.Asc;
      var inputOrderBy = options?.OrderBy ?? TenantOrderByEnum.Name;

      var limit = inputLimit < 1 ? 1 : inputLimit > 100 ? 100 : inputLimit;
      var skip = (inputPage - 1) * limit;

      var query = _commentsDbContext
        .Tenants
        .Take(limit)
        .Skip(skip);

      query = inputOrderBy switch
      {
        TenantOrderByEnum.Name => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Name)
          : query.OrderByDescending(x => x.Name),
        TenantOrderByEnum.Created => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Created)
          : query.OrderByDescending(x => x.Created),
        TenantOrderByEnum.Updated => inputSort == SortDirectionEnum.Asc
          ? query.OrderBy(x => x.Updated)
          : query.OrderByDescending(x => x.Updated),
        _ => query
      };

      var tenants = await query.AsNoTracking().ToListAsync();
      var total = await _commentsDbContext.Tenants.CountAsync();
      var pages = Math.Ceiling((double) total / limit);

      return new GenericPagedResult<Tenant>()
      {
        Page = inputPage,
        Pages = double.IsNaN(pages) ? 0 : (int) pages,
        Total = total,
        Limit = limit,
        Data = tenants
      };
    }

    public async Task<Tenant> Create(string name)
    {
      var now = DateTimeOffset.Now;

      var tenant = new Tenant
      {
        Name = name,
        Enabled = true,
        Created = now,
        Updated = now,
        Tokens = new List<string>()
      };
      
      var tenantValidator = new TenantValidator();
      await tenantValidator.ValidateAndThrowAsync(tenant);

      await _commentsDbContext.Tenants.AddAsync(tenant);
      await _commentsDbContext.SaveChangesAsync();

      return tenant;
    }

    public async Task<Tenant> AddToken(Guid tenantId)
    {
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null)
        throw new TenantNotFoundException(tenantId);

      var tokensHashSet = tenant.Tokens.ToHashSet();
      var tokensPrevCount = tokensHashSet.Count;
      while (tokensPrevCount == tokensHashSet.Count)
      {
        var hmac = new HMACSHA256();
        tokensHashSet.Add(Convert.ToBase64String(hmac.Key).Replace("==", string.Empty));
      }

      tenant.Tokens = tokensHashSet.ToList();
      tenant.Updated = DateTimeOffset.Now;

      await _commentsDbContext.SaveChangesAsync();
      return tenant;
    }

    public async Task<Tenant> DeleteToken(Guid tenantId, string token)
    {
      await using var transaction = await _commentsDbContext.Database.BeginTransactionAsync();
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null)
        throw new TenantNotFoundException(tenantId);

      tenant.Tokens.Remove(token);
      tenant.Updated = DateTimeOffset.Now;

      await _commentsDbContext.SaveChangesAsync();
      await transaction.CommitAsync();
      return tenant;
    }

    public async Task<Tenant> Disable(Guid tenantId)
    {
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null)
        throw new TenantNotFoundException(tenantId);

      tenant.Enabled = false;
      tenant.Updated = DateTimeOffset.Now;

      await _commentsDbContext.SaveChangesAsync();
      return tenant;
    }

    public async Task<Tenant> Enable(Guid tenantId)
    {
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null)
        throw new TenantNotFoundException(tenantId);

      tenant.Enabled = true;
      tenant.Updated = DateTimeOffset.Now;

      await _commentsDbContext.SaveChangesAsync();
      return tenant;
    }

    public async Task<Tenant> GetById(Guid tenantId, bool throwNotFoundException = true)
    {
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null && throwNotFoundException)
        throw new TenantNotFoundException(tenantId);

      return tenant;
    }

    public async Task<Tenant> Rename(Guid tenantId, string name)
    {
      var tenant = await _commentsDbContext
        .Tenants
        .FirstOrDefaultAsync(x => x.Id == tenantId);

      if (tenant == null)
        throw new TenantNotFoundException(tenantId);

      tenant.Name = name;
      tenant.Updated = DateTimeOffset.Now;
      
      var tenantValidator = new TenantValidator();
      await tenantValidator.ValidateAndThrowAsync(tenant);

      await _commentsDbContext.SaveChangesAsync();

      return tenant;
    }
  }
}

