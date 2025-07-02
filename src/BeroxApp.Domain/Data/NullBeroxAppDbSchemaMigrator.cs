using System.Threading.Tasks;
using Volo.Abp.DependencyInjection;

namespace BeroxApp.Data;

/* This is used if database provider does't define
 * IBeroxAppDbSchemaMigrator implementation.
 */
public class NullBeroxAppDbSchemaMigrator : IBeroxAppDbSchemaMigrator, ITransientDependency
{
    public Task MigrateAsync()
    {
        return Task.CompletedTask;
    }
}
