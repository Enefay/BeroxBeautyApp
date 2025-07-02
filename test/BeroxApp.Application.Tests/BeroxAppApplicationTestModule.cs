using Volo.Abp.Modularity;

namespace BeroxApp;

[DependsOn(
    typeof(BeroxAppApplicationModule),
    typeof(BeroxAppDomainTestModule)
)]
public class BeroxAppApplicationTestModule : AbpModule
{

}
