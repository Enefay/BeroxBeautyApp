using System;
using System.Collections.Generic;
using System.Text;
using BeroxApp.Localization;
using Volo.Abp.Application.Services;

namespace BeroxApp;

/* Inherit your application services from this class.
 */
public abstract class BeroxAppAppService : ApplicationService
{
    protected BeroxAppAppService()
    {
        LocalizationResource = typeof(BeroxAppResource);
    }
}
