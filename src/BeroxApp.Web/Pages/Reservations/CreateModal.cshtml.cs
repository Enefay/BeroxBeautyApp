using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using BeroxApp.Reservations;
using BeroxApp.Services;
using BeroxApp.Customers;
using BeroxApp.Employees;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Reservations
{
    public class CreateModalModel : AbpPageModel
    {
        [BindProperty]
        public CreateReservationDto Reservation { get; set; }

        public List<SelectListItem> Employees { get; set; }
        public List<ServiceSelectItem> Services { get; set; }

        private readonly IReservationAppService _reservationAppService;
        private readonly IEmployeeAppService _employeeAppService;
        private readonly IServiceAppService _serviceAppService;

        public CreateModalModel(
            IReservationAppService reservationAppService,
            IEmployeeAppService employeeAppService,
            IServiceAppService serviceAppService)
        {
            _reservationAppService = reservationAppService;
            _employeeAppService = employeeAppService;
            _serviceAppService = serviceAppService;
        }

        public async Task OnGetAsync()
        {
            Reservation = new CreateReservationDto
            {
                ReservationDate = DateTime.Today.AddHours(10)
            };

            var employees = await _employeeAppService.GetActiveEmployeesAsync();
            Employees = employees.Items
                .OrderBy(x => x.FullName)
                .Select(x => new SelectListItem
                {
                    Value = x.Id.ToString(),
                    Text = x.FullName
                }).ToList();

            var services = await _serviceAppService.GetActiveServicesAsync();
            Services = services.Items
                .OrderBy(x => x.Name)
                .Select(x => new ServiceSelectItem
                {
                    Id = x.Id.ToString(),
                    Text = x.Name,
                }).ToList();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            await _reservationAppService.CreateAsync(Reservation);
            return NoContent();
        }
    }

    public class ServiceSelectItem
    {
        public string Id { get; set; }
        public string Text { get; set; }
    }
}
