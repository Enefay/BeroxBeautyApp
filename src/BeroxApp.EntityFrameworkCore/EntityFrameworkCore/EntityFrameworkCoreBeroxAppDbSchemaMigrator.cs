using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using BeroxApp.Data;
using Volo.Abp.DependencyInjection;

namespace BeroxApp.EntityFrameworkCore;

public class EntityFrameworkCoreBeroxAppDbSchemaMigrator
    : IBeroxAppDbSchemaMigrator, ITransientDependency
{
    private readonly IServiceProvider _serviceProvider;

    public EntityFrameworkCoreBeroxAppDbSchemaMigrator(
        IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task MigrateAsync()
    {
        /* We intentionally resolve the BeroxAppDbContext
         * from IServiceProvider (instead of directly injecting it)
         * to properly get the connection string of the current tenant in the
         * current scope.
         */

        await _serviceProvider
            .GetRequiredService<BeroxAppDbContext>()
            .Database
            .MigrateAsync();
    }
}
