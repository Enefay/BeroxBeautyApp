using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BeroxApp.Reservations
{
    public enum ReservationStatus
    {
        Pending = 0,      // Bekliyor
        Approved = 1,     // Onaylandı
        Completed = 2,    // Tamamlandı
        Cancelled = 3     // İptal Edildi
    }
}
