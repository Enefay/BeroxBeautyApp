
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace BeroxApp.Employees
{
    [Authorize]
    public class EmployeeAppService : CrudAppService<Employee, EmployeeDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateEmployeeDto>, IEmployeeAppService
    {
        public EmployeeAppService(IRepository<Employee, Guid> repository) : base(repository)
        {
        }

        public async Task<ListResultDto<EmployeeDto>> GetActiveEmployeesAsync()
        {
            var employees = await Repository.GetListAsync(x => x.IsActive);
            var employeeDtos = ObjectMapper.Map<List<Employee>, List<EmployeeDto>>(employees);

            return new ListResultDto<EmployeeDto>(employeeDtos);
        }

        public override async Task<EmployeeDto> CreateAsync(CreateUpdateEmployeeDto input)
        {
            var employee = new Employee(
                GuidGenerator.Create(),
                input.FirstName,
                input.LastName
            )
            {
                PhoneNumber = input.PhoneNumber,
                Email = input.Email,
                MonthlySalary = input.MonthlySalary,
                IsActive = input.IsActive,
                UserId = input.UserId
            };

            await Repository.InsertAsync(employee);
            return ObjectMapper.Map<Employee, EmployeeDto>(employee);
        }
    }
}