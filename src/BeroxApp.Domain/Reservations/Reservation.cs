using BeroxApp.Customers;
using BeroxApp.Employees;
using BeroxApp.ReservationServiceEmployees;
using BeroxApp.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Domain.Entities.Auditing;
using Volo.Abp.MultiTenancy;

namespace BeroxApp.Reservations
{
    public class Reservation : FullAuditedAggregateRoot<Guid>, IMultiTenant
    {
        public Guid? TenantId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime ReservationDate { get; set; }
        public ReservationStatus Status { get; set; }
        public decimal ServicePrice { get; set; } // Rezervasyon anındaki fiyat
        public decimal FinalPrice { get; set; } // İndirim/ekstra sonrası fiyat
        public decimal? DiscountAmount { get; set; }
        public decimal? ExtraAmount { get; set; }
        public string Notes { get; set; }
        public DateTime? CompletedDate { get; set; }

        public virtual Customer Customer { get; set; }
        public virtual ICollection<ReservationServiceEmployee> ReservationServices { get; set; }


        protected Reservation() { }

        public Reservation(
           Guid id,
           Guid customerId,
           DateTime reservationDate,
           decimal servicePrice
       ) : base(id)
        {
            CustomerId = customerId;
            ReservationDate = reservationDate;
            ServicePrice = servicePrice;
            FinalPrice = servicePrice;
            Status = ReservationStatus.Pending;
            ReservationServices = new List<ReservationServiceEmployee>();
        }

        public void ApproveReservation()
        {
            Status = ReservationStatus.Approved;
        }

        public void CompleteReservation()
        {
            Status = ReservationStatus.Completed;
            CompletedDate = DateTime.Now;
        }

        public void CancelReservation()
        {
            Status = ReservationStatus.Cancelled;
        }

        public void ApplyDiscount(decimal discount)
        {
            DiscountAmount = discount;
            CalculateFinalPrice();
        }

        public void AddExtraCharge(decimal extra)
        {
            ExtraAmount = extra;
            CalculateFinalPrice();
        }

        private void CalculateFinalPrice()
        {
            FinalPrice = ServicePrice - (DiscountAmount ?? 0) + (ExtraAmount ?? 0);
        }
    }

}
