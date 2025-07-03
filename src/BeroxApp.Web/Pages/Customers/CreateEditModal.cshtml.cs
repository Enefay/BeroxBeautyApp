using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeroxApp.Customers;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Customers
{
    public class CreateEditModalModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateCustomerDto Customer { get; set; }

        private readonly ICustomerAppService _customerAppService;

        public CreateEditModalModel(ICustomerAppService customerAppService)
        {
            _customerAppService = customerAppService;
        }

        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                var customerDto = await _customerAppService.GetAsync(Id);
                Customer = new CreateUpdateCustomerDto
                {
                    FirstName = customerDto.FirstName,
                    LastName = customerDto.LastName,
                    PhoneNumber = customerDto.PhoneNumber,
                    Email = customerDto.Email,
                    Notes = customerDto.Notes
                };
            }
            else
            {
                Customer = new CreateUpdateCustomerDto();
            }
        }

        public async Task<IActionResult> OnPostAsync()
        {
            try
            {
                if (Id != Guid.Empty)
                {
                    await _customerAppService.UpdateAsync(Id, Customer);
                }
                else
                {
                    await _customerAppService.CreateAsync(Customer);
                }

                return NoContent();
            }
            catch (Volo.Abp.BusinessException ex)
            {
                // Telefon numarasý duplicate hatasý
                if (ex.Code == "Customer:DuplicatePhoneNumber")
                {
                    ModelState.AddModelError("Customer.PhoneNumber", L["DuplicatePhoneNumber"]);
                    return Page();
                }
                throw;
            }
        }
    }
}