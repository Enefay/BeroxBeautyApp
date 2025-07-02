using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Services
{
    public class Service : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int Duration { get; set; } // Dakika
        public decimal Price { get; set; } // Güncel fiyat
        public bool IsActive { get; set; }

        public virtual ICollection<ServicePriceHistory> PriceHistories { get; set; }

        protected Service()
        {
            PriceHistories = new List<ServicePriceHistory>();
        }

        public Service(Guid id, string name, decimal price, int duration) : base(id)
        {
            Name = name;
            Price = price;
            Duration = duration;
            IsActive = true;
            PriceHistories = new List<ServicePriceHistory>();
        }

        public void UpdatePrice(decimal newPrice)
        {
            // Fiyat değişikliğini history'e ekle
            PriceHistories.Add(new ServicePriceHistory(Id, Price, newPrice));
            Price = newPrice;
        }
    }
}
