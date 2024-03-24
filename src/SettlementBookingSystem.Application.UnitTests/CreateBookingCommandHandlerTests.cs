using FluentAssertions;
using FluentValidation;
using Microsoft.Extensions.Options;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Context;
using SettlementBookingSystem.Application.Exceptions;
using SettlementBookingSystem.Application.Options;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingCommandHandlerTests
    {
        private readonly Mock<IOptions<BookingOptions>> _mockBookingOptions;

        private readonly Mock<IBookingContext> _mockContext;

        private static readonly IList<BookingEntity> _dummyData = new List<BookingEntity>
        {
           new() { Name = "Name1", Start = TimeSpan.Parse("09:00"), End = TimeSpan.Parse("10:00") },
           new() { Name = "Name2", Start = TimeSpan.Parse("09:15"), End = TimeSpan.Parse("10:15") },
           new() { Name = "Name3", Start = TimeSpan.Parse("09:30"), End = TimeSpan.Parse("10:30") },
           new() { Name = "Name4", Start = TimeSpan.Parse("09:45"), End = TimeSpan.Parse("10:45") },
        };

        public CreateBookingCommandHandlerTests()
        {
            _mockBookingOptions = new Mock<IOptions<BookingOptions>>();
            _mockContext = new Mock<IBookingContext>();

            _mockBookingOptions.Setup(x => x.Value).Returns(new BookingOptions
            {
                OpenBookingHour = 9,
                ClosedBookingHour = 16,          
                DurationInHours = 1,
                SimultaneousSettlements = 4
            });
            _mockContext.Setup(x => x.Bookings).Returns(_dummyData);
        }

        [Fact]
        public async Task GivenValidBookingTime_WhenNoConflictingBookings_ThenBookingIsAccepted()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "10:01",
            };

            var handler = new CreateBookingCommandHandler(_mockBookingOptions.Object, _mockContext.Object);

            var result = await handler.Handle(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.BookingId.Should().NotBeEmpty();
        }       

        [Fact]
        public void GivenValidBookingTime_WhenBookingIsFull_ThenConflictThrown()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "09:15",
            };

            var handler = new CreateBookingCommandHandler(_mockBookingOptions.Object, _mockContext.Object);

            Func<Task> act = async () => await handler.Handle(command, CancellationToken.None);

            act.Should().Throw<ConflictException>();
        }
    }
}
