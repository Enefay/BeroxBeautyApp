using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;
using Volo.Abp.ObjectMapping;

namespace BeroxApp.Services
{
    [Authorize]
    public class ServiceAppService : CrudAppService<Service, ServiceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateServiceDto>, IServiceAppService
    {
        public ServiceAppService(IRepository<Service, Guid> repository) : base(repository)
        {
        }

        public async Task<ListResultDto<ServiceDto>> GetActiveServicesAsync()
        {
            var services = await Repository.GetListAsync(x => x.IsActive);
            var serviceDtos = ObjectMapper.Map<List<Service>, List<ServiceDto>>(services);

            return new ListResultDto<ServiceDto>(serviceDtos);
        }

        public override async Task<ServiceDto> CreateAsync(CreateUpdateServiceDto input)
        {
            var service = new Service(
                GuidGenerator.Create(),
                input.Name,
                input.Price,
                input.Duration
            )
            {
                Description = input.Description,
                IsActive = input.IsActive
            };

            await Repository.InsertAsync(service);
            return ObjectMapper.Map<Service, ServiceDto>(service);
        }

        public override async Task<ServiceDto> UpdateAsync(Guid id, CreateUpdateServiceDto input)
        {
            var service = await Repository.GetAsync(id);

            // Fiyat değişikliği kontrolü
            if (service.Price != input.Price)
            {
                service.UpdatePrice(input.Price);
            }

            service.Name = input.Name;
            service.Description = input.Description;
            service.Duration = input.Duration;
            service.IsActive = input.IsActive;

            await Repository.UpdateAsync(service);
            return ObjectMapper.Map<Service, ServiceDto>(service);
        }
    }
}