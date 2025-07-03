using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeroxApp.Services;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Services
{
    public class CreateEditModalModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateServiceDto Service { get; set; }

        private readonly IServiceAppService _serviceAppService;

        public CreateEditModalModel(IServiceAppService serviceAppService)
        {
            _serviceAppService = serviceAppService;
        }

        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                var serviceDto = await _serviceAppService.GetAsync(Id);
                Service = ObjectMapper.Map<ServiceDto, CreateUpdateServiceDto>(serviceDto);
            }
            else
            {
                Service = new CreateUpdateServiceDto
                {
                    IsActive = true,
                    Duration = 30 // Default 30 dakika
                };
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            if (Id != Guid.Empty)
            {
                await _serviceAppService.UpdateAsync(Id, Service);
            }
            else
            {
                await _serviceAppService.CreateAsync(Service);
            }

            return NoContent();
        }
    }
}