using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace BeroxApp.Customers
{
    [Authorize]
    public class CustomerAppService : CrudAppService<Customer, CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>, ICustomerAppService
    {
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
        }

        public async Task<CustomerDto> GetByPhoneNumberAsync(string phoneNumber)
        {
            var customer = await Repository.FirstOrDefaultAsync(x => x.PhoneNumber == phoneNumber);
            return customer != null ? ObjectMapper.Map<Customer, CustomerDto>(customer) : null;
        }

        public override async Task<CustomerDto> CreateAsync(CreateUpdateCustomerDto input)
        {
            // Telefon numarası kontrolü
            var existingCustomer = await Repository.FirstOrDefaultAsync(x => x.PhoneNumber == input.PhoneNumber);
            if (existingCustomer != null)
            {
                throw new Volo.Abp.BusinessException("Customer:DuplicatePhoneNumber")
                    .WithData("phoneNumber", input.PhoneNumber);
            }

            var customer = new Customer(
                GuidGenerator.Create(),
                input.FirstName,
                input.LastName,
                input.PhoneNumber
            )
            {
                Email = input.Email,
                Notes = input.Notes
            };

            await Repository.InsertAsync(customer);
            return ObjectMapper.Map<Customer, CustomerDto>(customer);
        }
    }
}
