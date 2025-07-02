using BeroxApp.Samples;
using Xunit;

namespace BeroxApp.EntityFrameworkCore.Applications;

[Collection(BeroxAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleAppServiceTests : SampleAppServiceTests<BeroxAppEntityFrameworkCoreTestModule>
{

}
