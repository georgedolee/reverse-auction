using AuctionService.API.Extensions;
using AuctionService.Application.Extensions;
using AuctionService.Infrastructure.Extensions;
using AuctionService.Infrastructure.Scheduling;
using Hangfire;
using Serilog;
using SharedInfrastructure.Middlewares;

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

app.UseSerilogRequestLogging();

app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.UseHangfireDashboard();

app.MapControllers();

using (var scope = app.Services.CreateScope())
{
    var scheduler = scope.ServiceProvider.GetRequiredService<IJobScheduler>();
    scheduler.ConfigureRecurringJobs();
}

app.Run();
