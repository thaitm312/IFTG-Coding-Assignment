using FluentValidation;
using Microsoft.Extensions.Options;
using SettlementBookingSystem.Application.Options;
using System;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingValidator : AbstractValidator<CreateBookingCommand>
    {
        public CreateBookingValidator(IOptions<BookingOptions> options)
        {
            CascadeMode = CascadeMode.Stop;

            RuleFor(b => b.Name).NotEmpty();
            RuleFor(b => b.BookingTime)
                .Matches("[0-9]{1,2}:[0-5][0-9]");

            Transform(x => x.BookingTime, bt => TimeSpan.TryParse(bt, out var result) ? result : TimeSpan.MaxValue)
                .InclusiveBetween(TimeSpan.FromHours(options.Value.OpenBookingHour), TimeSpan.FromHours(options.Value.ClosedBookingHour));
        }
    }
}
