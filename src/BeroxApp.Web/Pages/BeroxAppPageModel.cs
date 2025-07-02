using BeroxApp.Localization;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages;

/* Inherit your PageModel classes from this class.
 */
public abstract class BeroxAppPageModel : AbpPageModel
{
    protected BeroxAppPageModel()
    {
        LocalizationResourceType = typeof(BeroxAppResource);
    }
}
