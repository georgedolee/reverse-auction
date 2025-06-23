using FluentValidation;
using MediatR;
using Microsoft.Extensions.DependencyInjection;
using SharedInfrastructure.Behaviors;

namespace UserService.Application.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        var appAssambly = typeof(ServiceCollectionExtensions).Assembly;

        services.AddMediatR(config =>
        {
            config.RegisterServicesFromAssembly(appAssambly);
        });

        services.AddValidatorsFromAssembly(appAssambly);

        services.AddTransient(typeof(IPipelineBehavior<,>), typeof(ValidationBehavior<,>));

        return services;
    }
}
