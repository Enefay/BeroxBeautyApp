using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeroxApp.Employees;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Employees
{
    public class CreateEditModalModel : AbpPageModel
    {
        [HiddenInput]
        [BindProperty(SupportsGet = true)]
        public Guid Id { get; set; }

        [BindProperty]
        public CreateUpdateEmployeeDto Employee { get; set; }

        private readonly IEmployeeAppService _employeeAppService;

        public CreateEditModalModel(IEmployeeAppService employeeAppService)
        {
            _employeeAppService = employeeAppService;
        }

        public async Task OnGetAsync()
        {
            if (Id != Guid.Empty)
            {
                var employeeDto = await _employeeAppService.GetAsync(Id);
                Employee = new CreateUpdateEmployeeDto
                {
                    FirstName = employeeDto.FirstName,
                    LastName = employeeDto.LastName,
                    PhoneNumber = employeeDto.PhoneNumber,
                    Email = employeeDto.Email,
                    MonthlySalary = employeeDto.MonthlySalary,
                    IsActive = employeeDto.IsActive,
                    UserId = employeeDto.UserId
                };
            }
            else
            {
                Employee = new CreateUpdateEmployeeDto
                {
                    IsActive = true,
                    MonthlySalary = 20000 // Asgari ücret default
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
                await _employeeAppService.UpdateAsync(Id, Employee);
            }
            else
            {
                await _employeeAppService.CreateAsync(Employee);
            }

            return NoContent();
        }
    }
}