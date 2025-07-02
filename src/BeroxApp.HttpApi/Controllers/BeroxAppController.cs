using BeroxApp.Localization;
using Volo.Abp.AspNetCore.Mvc;

namespace BeroxApp.Controllers;

/* Inherit your controllers from this class.
 */
public abstract class BeroxAppController : AbpControllerBase
{
    protected BeroxAppController()
    {
        LocalizationResource = typeof(BeroxAppResource);
    }
}
