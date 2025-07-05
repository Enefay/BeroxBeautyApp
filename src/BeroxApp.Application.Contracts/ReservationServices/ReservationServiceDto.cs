using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeroxApp.ReservationServices
{
    public class ReservationServiceDto
    {
        public Guid ServiceId { get; set; }
        public string ServiceName { get; set; }
        public Guid EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class CreateReservationDto
    {
        public Guid CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
        public string Notes { get; set; }

        public List<ReservationServiceDto> Services { get; set; } = new();
    }

}
