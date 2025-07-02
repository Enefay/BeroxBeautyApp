using System.Threading.Tasks;

namespace BeroxApp.Data;

public interface IBeroxAppDbSchemaMigrator
{
    Task MigrateAsync();
}
