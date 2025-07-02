using BeroxApp.Samples;
using Xunit;

namespace BeroxApp.EntityFrameworkCore.Domains;

[Collection(BeroxAppTestConsts.CollectionDefinitionName)]
public class EfCoreSampleDomainTests : SampleDomainTests<BeroxAppEntityFrameworkCoreTestModule>
{

}
