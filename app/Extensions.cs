using System;
using Comments.App.Types;
using HotChocolate;
using Microsoft.Extensions.DependencyInjection;

namespace Comments.App
{
  public static class Extensions
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
        .AddType<AccountType>()
        .AddType<CommentType>()
        .AddType<SortDirectionEnumType>()
        .AddType<TenantOrderByEnumType>()
        .AddType<GetTenantsListInputType>()
        .AddType<TenantsPagedResultType>()
        .AddType<NewCommentInputType>()
        .AddType<TenantType>()
        .AddQueryType<QueryType>()
        .AddMutationType<MutationType>()
        .Create();
    }
  }
}
