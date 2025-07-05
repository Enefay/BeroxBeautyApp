using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using BeroxApp.Reservations;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc.UI.RazorPages;

namespace BeroxApp.Web.Pages.Reservations
{
    public class IndexModel : AbpPageModel
    {
        private readonly IReservationAppService _reservationAppService;

        public IndexModel(IReservationAppService reservationAppService)
        {
            _reservationAppService = reservationAppService;
        }

        public async Task OnGetAsync()
        {

        }
    }
}