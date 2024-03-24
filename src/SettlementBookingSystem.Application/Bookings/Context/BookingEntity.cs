using System;

namespace SettlementBookingSystem.Application.Bookings.Context
{
    public class BookingEntity
    {
        public Guid BookingId { get; set; }

        public string Name { get; set; }

        public TimeSpan Start { get; set; }

        public TimeSpan End { get; set; }

        public BookingEntity()
        {
            BookingId = Guid.NewGuid();
        }
    }
}
