namespace SettlementBookingSystem.Application.Options
{
    public class BookingOptions
    {
        public double OpenBookingHour { get; set; }

        public double ClosedBookingHour { get; set; }

        public int SimultaneousSettlements { get; set; }

        public int DurationInMinutes { get; set; }
    }
}
