using BeroxApp.Employees;
using BeroxApp.Reservations;
using BeroxApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.ReservationServiceEmployees
{
    public class ReservationServiceEmployee : FullAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }

        public Guid ReservationId { get; set; }
        public Guid ServiceId { get; set; }
        public Guid EmployeeId { get; set; }

        public virtual Reservation Reservation { get; set; }
        public virtual Service Service { get; set; }
        public virtual Employee Employee { get; set; }
    }

}
