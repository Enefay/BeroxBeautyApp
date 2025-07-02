using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;

namespace BeroxApp.Finance
{
    public class ExpenseDto : EntityDto<Guid>
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public ExpenseType Type { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid? EmployeeId { get; set; }
        public string EmployeeName { get; set; }
    }

    public class CreateUpdateExpenseDto
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public ExpenseType Type { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid? EmployeeId { get; set; }
    }

    public class GetExpenseListDto : PagedAndSortedResultRequestDto
    {
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public ExpenseType? Type { get; set; }
    }
}
