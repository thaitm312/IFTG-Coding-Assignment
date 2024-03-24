
using FluentValidation;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using SettlementBookingSystem.Application.Behaviours;
using SettlementBookingSystem.Application.Bookings.Commands;
using SettlementBookingSystem.Application.Bookings.Context;
using SettlementBookingSystem.Application.Options;

namespace SettlementBookingSystem.Application
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
        {
            var assembly = typeof(DependencyInjection).Assembly;
            services.AddAutoMapper(assembly);

            services.AddValidatorsFromAssemblyContaining<CreateBookingValidator>();
            services.AddMediatR(cfg => cfg.AsScoped(), assembly);

            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(UnhandledExceptionBehaviour<,>));
            services.AddTransient(typeof(IPipelineBehavior<,>), typeof(RequestValidationBehaviour<,>));

            services.AddSingleton<IBookingContext, DummyDataBookingContext>();

            services.Configure<BookingOptions>(configuration.GetSection("BookingOptions"));

            return services;
        }
    }
}
