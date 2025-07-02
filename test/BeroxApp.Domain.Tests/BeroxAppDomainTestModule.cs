using Volo.Abp.Modularity;

namespace BeroxApp;

[DependsOn(
    typeof(BeroxAppDomainModule),
    typeof(BeroxAppTestBaseModule)
)]
public class BeroxAppDomainTestModule : AbpModule
{

}
