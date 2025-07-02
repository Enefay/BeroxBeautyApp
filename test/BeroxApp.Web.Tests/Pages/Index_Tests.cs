using System.Threading.Tasks;
using Shouldly;
using Xunit;

namespace BeroxApp.Pages;

public class Index_Tests : BeroxAppWebTestBase
{
    [Fact]
    public async Task Welcome_Page()
    {
        var response = await GetResponseAsStringAsync("/");
        response.ShouldNotBeNull();
    }
}
