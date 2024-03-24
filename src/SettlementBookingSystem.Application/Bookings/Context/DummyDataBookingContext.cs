using System.Collections.Generic;

namespace SettlementBookingSystem.Application.Bookings.Context
{
    public class DummyDataBookingContext : IBookingContext
    {
        public IList<BookingEntity> Bookings { get; set; } = new List<BookingEntity>();
    }
}
