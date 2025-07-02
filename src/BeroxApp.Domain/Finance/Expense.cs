using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Finance
{
    public class Expense : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public decimal Amount { get; set; }
        public ExpenseType Type { get; set; }
        public DateTime ExpenseDate { get; set; }
        public Guid? EmployeeId { get; set; } // Maaş gideri ise

        public virtual Employees.Employee Employee { get; set; }

        protected Expense() { }

        public Expense(Guid id, string title, decimal amount, ExpenseType type, DateTime expenseDate) : base(id)
        {
            Title = title;
            Amount = amount;
            Type = type;
            ExpenseDate = expenseDate;
        }
    }

 
}
