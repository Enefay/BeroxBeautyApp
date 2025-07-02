using BeroxApp.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace BeroxApp.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(BeroxAppEntityFrameworkCoreModule),
    typeof(BeroxAppApplicationContractsModule)
    )]
public class BeroxAppDbMigratorModule : AbpModule
{
}
