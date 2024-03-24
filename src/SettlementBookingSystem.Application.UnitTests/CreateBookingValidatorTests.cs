using FluentAssertions;
using Microsoft.Extensions.Options;
using Moq;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Options;
using System.Linq;
using System.Threading;
using Xunit;

namespace SettlementBookingSystem.Application.UnitTests
{
    public class CreateBookingValidatorTests
    {
        private readonly Mock<IOptions<BookingOptions>> _mockBookingOptions;

        public CreateBookingValidatorTests()
        {
            _mockBookingOptions = new Mock<IOptions<BookingOptions>>();
            _mockBookingOptions.Setup(x => x.Value).Returns(new BookingOptions
            {
                OpenBookingHour = 9,
                ClosedBookingHour = 16,
                DurationInHours = 1,
                SimultaneousSettlements = 4
            });
        }

        [Fact]
        public async void GivenEmptyName_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = string.Empty,
                BookingTime = "09:00",
            };

            var validator = new CreateBookingValidator(_mockBookingOptions.Object);

            var result = await validator.ValidateAsync(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.FirstOrDefault().PropertyName.Should().Be(nameof(CreateBookingCommand.Name));
        }

        [Fact]
        public async void GivenBookingTimeFormat_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "sfsdfsd",
            };

            var validator = new CreateBookingValidator(_mockBookingOptions.Object);

            var result = await validator.ValidateAsync(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.FirstOrDefault().PropertyName.Should().Be(nameof(CreateBookingCommand.BookingTime));
        }

        [Fact]
        public async void GivenOutOfHoursBookingTime_WhenBooking_ThenValidationFails()
        {
            var command = new CreateBookingCommand
            {
                Name = "test",
                BookingTime = "00:00",
            };

            var validator = new CreateBookingValidator(_mockBookingOptions.Object);

            var result = await validator.ValidateAsync(command, CancellationToken.None);

            result.Should().NotBeNull();
            result.IsValid.Should().BeFalse();
            result.Errors.Should().NotBeEmpty();
            result.Errors.FirstOrDefault().PropertyName.Should().Be(nameof(CreateBookingCommand.BookingTime));
        }
    }
}
