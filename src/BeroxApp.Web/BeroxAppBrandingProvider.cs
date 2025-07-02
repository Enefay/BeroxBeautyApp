using Microsoft.Extensions.Localization;
using BeroxApp.Localization;
using Volo.Abp.Ui.Branding;
using Volo.Abp.DependencyInjection;

namespace BeroxApp.Web;

[Dependency(ReplaceServices = true)]
public class BeroxAppBrandingProvider : DefaultBrandingProvider
{
    private IStringLocalizer<BeroxAppResource> _localizer;

    public BeroxAppBrandingProvider(IStringLocalizer<BeroxAppResource> localizer)
    {
        _localizer = localizer;
    }

    public override string AppName => _localizer["AppName"];
}
