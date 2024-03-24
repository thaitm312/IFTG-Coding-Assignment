using IntervalTree;
using System;

namespace SettlementBookingSystem.Application.Bookings.Context
{
    public interface IBookingContext
    {
        public IIntervalTree<TimeSpan, string> Bookings { get; set; }
    }
}
