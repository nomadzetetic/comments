using System;
using System.Threading.Tasks;
using Comments.Data.Entities;
using Comments.Services.ProviderService;

namespace Comments.App.Resolvers
{
  public class MutationResolver
  {
    private readonly IProviderService _providerService;

    public MutationResolver(IProviderService providerService)
    {
      _providerService = providerService;
    }

    public Task<Provider> AddProvider(string name) => _providerService.AddProvider(name);
    public Task<Provider> RenameProvider(Guid providerId, string name) => 
      _providerService.RenameProvider(providerId, name?.Trim());
    public Task<Provider> EnableProvider(Guid providerId) => _providerService.EnableProvider(providerId);
    public Task<Provider> DisableProvider(Guid providerId) => _providerService.DisableProvider(providerId);
    public Task<Provider> AddProviderToken(Guid providerId) => _providerService.AddProviderToken(providerId);
    public Task<Provider> DeleteProviderToken(Guid providerId, string token) =>
      _providerService.DeleteProviderToken(providerId, token?.Trim());
  }
}
