using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BeroxApp.Reservations
{
    public class ReservationDto : EntityDto<Guid>
    {
        public Guid CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string CustomerPhone { get; set; }

        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }

        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }

        public DateTime ReservationDate { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal ServicePrice { get; set; }
        public decimal FinalPrice { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? ExtraAmount { get; set; }
        public string Notes { get; set; }
        public DateTime? CompletedDate { get; set; }
    }

    public class CreateReservationDto
    {
        public Guid CustomerId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ServiceId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Notes { get; set; }
    }

    public class UpdateReservationDto
    {
        public DateTime ReservationDate { get; set; }
        public string Notes { get; set; }
        public decimal? DiscountAmount { get; set; }
        public decimal? ExtraAmount { get; set; }
    }

    public class GetReservationListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ReservationStatus? Status { get; set; }
        public Guid? EmployeeId { get; set; }
        public Guid? CustomerId { get; set; }
    }
}
