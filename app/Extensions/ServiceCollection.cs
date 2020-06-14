using System;
using Comments.App.Types;
using Comments.App.Types.Enums;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.App.Extensions
{
  public static class ServiceCollection
  {
    public static void SetupGraphql(this IServiceCollection services)
    {
      services.AddGraphQL(Schema);
    }
    
    private static ISchema Schema(IServiceProvider serviceProvider)
    {
      return SchemaBuilder
        .New()
        .AddServices(serviceProvider)
        .AddAuthorizeDirectiveType()
        .AddType<SortDirectionEnumType>()
        .AddType<ProviderOrderByEnumType>()
        .AddType<GetTenantsListInputType>()
        .AddType<ProvidersPagedResultType>()
        .AddType<ProviderType>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
        .Create();
    }
  }
}
