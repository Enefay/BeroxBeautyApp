using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Employees
{
    public class EmployeeService : Entity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid EmployeeId { get; set; }
        public Guid ServiceId { get; set; }

        public virtual Employee Employee { get; set; }
        public virtual Services.Service Service { get; set; }

        protected EmployeeService() { }

        public EmployeeService(Guid employeeId, Guid serviceId)
        {
            EmployeeId = employeeId;
            ServiceId = serviceId;
        }
    }
}
