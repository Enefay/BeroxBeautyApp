using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeroxApp.Services;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Services
{
    public class IndexModel : AbpPageModel
    {
        public string NameFilter { get; set; }
        public bool? IsActiveFilter { get; set; }

        private readonly IServiceAppService _serviceAppService;

        public IndexModel(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }

        public async Task OnGetAsync()
        {

        }
    }
}