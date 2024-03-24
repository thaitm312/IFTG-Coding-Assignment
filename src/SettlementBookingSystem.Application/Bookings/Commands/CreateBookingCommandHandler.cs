using MediatR;
using Microsoft.Extensions.Options;
using SettlementBookingSystem.Application.Bookings.Context;
using SettlementBookingSystem.Application.Bookings.Dtos;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Application.Options;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace SettlementBookingSystem.Application.Bookings.Commands
{
    public class CreateBookingCommandHandler : IRequestHandler<CreateBookingCommand, BookingDto>
    {
        private readonly IOptions<BookingOptions> _options;

        private readonly IBookingContext _context;

        public CreateBookingCommandHandler(IOptions<BookingOptions> options, IBookingContext context)
        {
            _options = options;
            _context = context;
        }

        public Task<BookingDto> Handle(CreateBookingCommand request, CancellationToken cancellationToken)
        {
            var startTime = TimeSpan.Parse(request.BookingTime);
            var endTime = startTime.Add(TimeSpan.FromMinutes(_options.Value.DurationInMinutes));

            var bookings = _context.Bookings.Query(startTime, endTime);

            if (bookings.Count() >= _options.Value.SimultaneousSettlements)
            {
                throw new ConflictException($"Booking settlements limit reached for slot {request.BookingTime}");
            }


            _context.Bookings.Add(startTime, endTime, request.Name);

            return Task.FromResult(new BookingDto());
        }
    }
}
