using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BeroxApp.Employees
{
    public class EmployeeDto : EntityDto<Guid>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal MonthlySalary { get; set; }
        public bool IsActive { get; set; }
        public Guid? UserId { get; set; }
        public string UserName { get; set; }
    }

    public class CreateUpdateEmployeeDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string Email { get; set; }
        public decimal MonthlySalary { get; set; }
        public bool IsActive { get; set; } = true;
    }
}