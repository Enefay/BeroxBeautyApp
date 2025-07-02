using Xunit;

namespace BeroxApp.EntityFrameworkCore;

[CollectionDefinition(BeroxAppTestConsts.CollectionDefinitionName)]
public class BeroxAppEntityFrameworkCoreCollection : ICollectionFixture<BeroxAppEntityFrameworkCoreFixture>
{

}
