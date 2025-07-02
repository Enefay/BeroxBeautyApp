using Volo.Abp.Modularity;

namespace BeroxApp;

/* Inherit from this class for your domain layer tests. */
public abstract class BeroxAppDomainTestBase<TStartupModule> : BeroxAppTestBase<TStartupModule>
    where TStartupModule : IAbpModule
{

}
