using Serilog;
using SharedInfrastructure.Extensions;

namespace AuctionService.API.Extensions;

public static class WebApplicationBuilderExtensions
{
    public static WebApplicationBuilder AddPresentation(this WebApplicationBuilder builder)
    {
        builder.Host.UseSerilog((context, configuration) => 
            configuration.ReadFrom.Configuration(context.Configuration));

        builder.Services.AddControllers();
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerWithAuth();

        builder.Services.AddJwtAuthentication(builder.Configuration);

        builder.Services.AddRoleBasedPolicies();
        builder.Services.AddAuthorization();

        return builder;
    }
}
