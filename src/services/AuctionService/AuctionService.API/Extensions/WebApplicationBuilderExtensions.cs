using Photos;
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

        builder.Services.AddGrpcClient<PhotoService.PhotoServiceClient>(options =>
        {
            var grpcUrl = builder.Configuration["FileServiceUrl"];

            if (string.IsNullOrEmpty(grpcUrl))
            {
                throw new InvalidOperationException("FileServiceUrl configuration is missing");
            }

            options.Address = new Uri(grpcUrl);
        });

        return builder;
    }
}
