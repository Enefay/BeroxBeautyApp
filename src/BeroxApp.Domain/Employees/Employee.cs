using BeroxApp.Reservations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Employees
{
    public class Employee : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public Guid? UserId { get; set; } // ABP User ile ilişki
        public decimal MonthlySalary { get; set; }
        public bool IsActive { get; set; }

        public string FullName => $"{FirstName} {LastName}";

        public virtual ICollection<Reservation> Reservations { get; set; }
        public virtual ICollection<EmployeeService> EmployeeServices { get; set; }

        protected Employee()
        {
            Reservations = new List<Reservation>();
            EmployeeServices = new List<EmployeeService>();
        }

        public Employee(Guid id, string firstName, string lastName) : base(id)
        {
            FirstName = firstName;
            LastName = lastName;
            IsActive = true;
            Reservations = new List<Reservation>();
            EmployeeServices = new List<EmployeeService>();
        }
    }
}
