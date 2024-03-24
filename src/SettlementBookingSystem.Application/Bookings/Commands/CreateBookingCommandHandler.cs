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
            var endTime = startTime.Add(TimeSpan.FromHours(_options.Value.DurationInHours));

            var bookingCounts = _context.Bookings.Count(x => x.Start < endTime && x.End > startTime);

            if (bookingCounts >= _options.Value.SimultaneousSettlements)
            {
                throw new ConflictException($"Booking settlements limit reached for slot {request.BookingTime}");
            }

            var entity = new BookingEntity
            {
                Name = request.Name,
                Start = startTime,
                End = endTime
            };

            _context.Bookings.Add(entity);

            return Task.FromResult(new BookingDto
            {
                BookingId = entity.BookingId
            });
        }
    }
}
