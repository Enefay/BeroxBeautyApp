using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace BeroxApp.Customers
{
    [Authorize]
    public class CustomerAppService : CrudAppService<Customer, CustomerDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateCustomerDto>, ICustomerAppService
    {
        private readonly IRepository<Customer> _customerRepository;
        public CustomerAppService(IRepository<Customer, Guid> repository) : base(repository)
        {
            _customerRepository = repository;
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

        [HttpPost]
        [Route("api/app/customer/search")]
        public async Task<ListResultDto<CustomerLookupDto>> SearchAsync([FromBody] CustomerSearchInput input)
        {
            var queryable = await _customerRepository.GetQueryableAsync();
            var list = queryable
                .Where(x => x.FirstName.Contains(input.Query) || x.LastName.Contains(input.Query))
                .OrderBy(x => x.FirstName)
                .Take(5)
                .ToList();

            return new ListResultDto<CustomerLookupDto>(
                list.Select(x => new CustomerLookupDto
                {
                    Id = x.Id,
                    FullName = $"{x.FirstName} {x.LastName}",
                    PhoneNumber = x.PhoneNumber
                }).ToList()
            );
        }

        public class CustomerSearchInput
        {
            public string Query { get; set; }
        }



    }
}
