using Volo.Abp.Modularity;

namespace BeroxApp;

public abstract class BeroxAppApplicationTestBase<TStartupModule> : BeroxAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
