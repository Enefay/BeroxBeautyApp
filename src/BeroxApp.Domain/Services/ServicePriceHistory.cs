using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Services
{
    public class ServicePriceHistory : CreationAuditedEntity<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid ServiceId { get; set; }
        public decimal OldPrice { get; set; }
        public decimal NewPrice { get; set; }

        public virtual Service Service { get; set; }

        protected ServicePriceHistory() { }

        public ServicePriceHistory(Guid serviceId, decimal oldPrice, decimal newPrice)
        {
            ServiceId = serviceId;
            OldPrice = oldPrice;
            NewPrice = newPrice;
        }
    }
}
