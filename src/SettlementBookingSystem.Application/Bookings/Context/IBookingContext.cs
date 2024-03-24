using System.Collections.Generic;

namespace SettlementBookingSystem.Application.Bookings.Context
{
    public interface IBookingContext
    {
        public IList<BookingEntity> Bookings { get; set; }
    }
}
