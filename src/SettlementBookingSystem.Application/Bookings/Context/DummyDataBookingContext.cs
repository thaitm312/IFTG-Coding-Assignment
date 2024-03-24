using IntervalTree;
using System;
using System.Collections.Generic;

namespace SettlementBookingSystem.Application.Bookings.Context
{
    public class DummyDataBookingContext : IBookingContext
    {
        public IIntervalTree<TimeSpan, string> Bookings { get; set; } = new IntervalTree<TimeSpan, string>();
    }
}
