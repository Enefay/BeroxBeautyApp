// BeroxApp.Web/Pages/Services/Index.cshtml.cs
using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;
using BeroxApp.Services;
using Volo.Abp.Application.Dtos;

namespace BeroxApp.Web.Pages.Services
{
    public class IndexModel : AbpPageModel
    {
        private readonly IServiceAppService _serviceAppService;

        public PagedResultDto<ServiceDto> Services { get; set; }

        public IndexModel(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }

        public async Task OnGetAsync()
        {
            Services = await _serviceAppService.GetListAsync(new PagedAndSortedResultRequestDto
            {
                MaxResultCount = 20
            });
        }

        public async Task<IActionResult> OnPostDeleteAsync(Guid id)
        {
            await _serviceAppService.DeleteAsync(id);
            return NoContent();
        }
    }
}