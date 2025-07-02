using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BeroxApp.Reservations
{
    public interface IReservationAppService : IApplicationService
    {
        Task<ReservationDto> GetAsync(Guid id);
        Task<PagedResultDto<ReservationDto>> GetListAsync(GetReservationListDto input);
        Task<ReservationDto> CreateAsync(CreateReservationDto input);
        Task<ReservationDto> UpdateAsync(Guid id, UpdateReservationDto input);
        Task DeleteAsync(Guid id);

        Task<ReservationDto> ApproveAsync(Guid id);
        Task<ReservationDto> CompleteAsync(Guid id);
        Task<ReservationDto> CancelAsync(Guid id);

        Task<ListResultDto<ReservationDto>> GetTodayReservationsAsync();
    }
}
