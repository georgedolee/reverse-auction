using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using SharedInfrastructure.ExceptionHandling;
using SharedKernel.Constants;
using SharedKernel.Exceptions;

namespace SharedInfrastructure.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddAuthentication(options =>
        {
            options.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.Authority = configuration["Jwt:Authority"];
            options.Audience = configuration["Jwt:Audience"];

            options.TokenValidationParameters.ValidTypes = new[] { "at+jwt" };

            options.Events = new JwtBearerEvents
            {
                OnChallenge = async context =>
                {
                    context.HandleResponse();

                    await ExceptionHandler.HandleUnauthorizedExceptionAsync(context.HttpContext);
                },

                OnForbidden = async context =>
                {
                    await ExceptionHandler.HandleForbiddenExceptionAsync(context.HttpContext, new ForbiddenException());
                }
            };
        });

        return services;
    }

    public static IServiceCollection AddSwaggerWithAuth(this IServiceCollection services)
    {
        services.AddSwaggerGen(c =>
        {
            c.AddSecurityDefinition("bearerAuth", new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "Bearer"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "bearerAuth"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }

    public static IServiceCollection AddRoleBasedPolicies(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy(RolePolicies.AdminsOnly, policy =>
                policy.RequireRole(Roles.Admin));

            options.AddPolicy(RolePolicies.ManagersOnly, policy =>
                policy.RequireRole(Roles.Manager));

            options.AddPolicy(RolePolicies.AdminOrManager, policy =>
                policy.RequireRole(Roles.Admin, Roles.Manager));
        });

        return services;
    }
}
