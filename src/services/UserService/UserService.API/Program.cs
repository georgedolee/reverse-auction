using Microsoft.EntityFrameworkCore;
using Serilog;
using SharedInfrastructure.Middlewares;
using UserService.API.Extensions;
using UserService.API.GrpcServices;
using UserService.Application.Extensions;
using UserService.Infrastructure.Extensions;
using UserService.Infrastructure.Persistance;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddApplication();
builder.AddPresentation();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
{
    var context = scope.ServiceProvider.GetService<UsersDbContext>();
    ArgumentNullException.ThrowIfNull(context, nameof(context));
    context.Database.Migrate();
}

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllers();

app.MapGrpcService<GrpcAuthService>();

app.Run();
