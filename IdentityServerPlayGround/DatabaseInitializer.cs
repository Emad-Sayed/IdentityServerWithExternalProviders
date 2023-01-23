using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using Microsoft.EntityFrameworkCore;

public static class DatabaseInitializer
{
    public static void PopulateIdentityServer(IApplicationBuilder app)
    {
        using var serviceScope = app.ApplicationServices?.GetService<IServiceScopeFactory>()?.CreateScope();

        var context = serviceScope.ServiceProvider.GetRequiredService<ConfigurationDbContext>();


        foreach (var client in Config.Clients)
        {
            var item = context.Clients.SingleOrDefault(c => c.ClientName == client.ClientId);

            if (item == null)
            {
                context.Clients.Add(client.ToEntity());
            }
        }

        foreach (var resource in Config.ApiResources)
        {
            var item = context.ApiResources.SingleOrDefault(c => c.Name == resource.Name);

            if (item == null)
            {
                context.ApiResources.Add(resource.ToEntity());
            }
        }

        foreach (var scope in Config.ApiScopes)
        {
            var item = context.ApiScopes.SingleOrDefault(c => c.Name == scope.Name);

            if (item == null)
            {
                context.ApiScopes.Add(scope.ToEntity());
            }
        }

        context.SaveChanges();
    }
}