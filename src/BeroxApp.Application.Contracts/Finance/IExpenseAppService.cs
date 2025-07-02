using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BeroxApp.Finance
{
    public interface IExpenseAppService : IApplicationService
    {
        Task<ExpenseDto> GetAsync(Guid id);
        Task<PagedResultDto<ExpenseDto>> GetListAsync(GetExpenseListDto input);
        Task<ExpenseDto> CreateAsync(CreateUpdateExpenseDto input);
        Task<ExpenseDto> UpdateAsync(Guid id, CreateUpdateExpenseDto input);
        Task DeleteAsync(Guid id);

        Task<decimal> GetTotalExpenseAsync(DateTime startDate, DateTime endDate);
    }
}
