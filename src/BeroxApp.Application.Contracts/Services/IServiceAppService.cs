using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace BeroxApp.Services
{
    public interface IServiceAppService : ICrudAppService<ServiceDto, Guid, PagedAndSortedResultRequestDto, CreateUpdateServiceDto>
    {
        Task<ListResultDto<ServiceDto>> GetActiveServicesAsync();
    }
}
