using System;

namespace SettlementBookingSystem.Application.Bookings.Dtos
{
    public class BookingDto
    {
        public Guid BookingId { get; }

        public BookingDto()
        {
            BookingId = Guid.NewGuid();
        }
    }
}
