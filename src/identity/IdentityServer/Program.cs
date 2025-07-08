using Authorization;
using Duende.IdentityServer.Services;
using Duende.IdentityServer.Validation;
using IdentityServer;
using IdentityServer.Services;
using Microsoft.EntityFrameworkCore;
using Serilog;

var builder = WebApplication.CreateBuilder(args);

builder.Host.UseSerilog((context, configuration) =>
    configuration.ReadFrom.Configuration(context.Configuration));

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
var migrationsAssembly = typeof(Config).Assembly.GetName().Name;

builder.Services.AddIdentityServer(options =>
{
    options.Events.RaiseErrorEvents = true;
    options.Events.RaiseInformationEvents = true;
    options.Events.RaiseFailureEvents = true;
    options.Events.RaiseSuccessEvents = true;
    options.EmitStaticAudienceClaim = true;
    options.Authentication.CookieAuthenticationScheme = null;
}).AddConfigurationStore(options =>
    options.ConfigureDbContext = b =>
        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly))
)
.AddOperationalStore(options =>
{
    options.ConfigureDbContext = b =>
        b.UseSqlServer(connectionString, sql => sql.MigrationsAssembly(migrationsAssembly));
    options.EnableTokenCleanup = true;
    options.TokenCleanupInterval = 3600;
});

builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowAll", policy =>
    {
        policy
            .AllowAnyOrigin()
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

builder.Services.AddScoped<IResourceOwnerPasswordValidator, GrpcResourceOwnerPasswordValidator>();
builder.Services.AddScoped<IProfileService, GrpcProfileService>();

builder.Services.AddGrpcClient<AuthService.AuthServiceClient>(options =>
{
    var grpcUrl = builder.Configuration["AuthServiceUrl"];

    if (string.IsNullOrEmpty(grpcUrl))
    {
        throw new InvalidOperationException("AuthServiceUrl configuration is missing");
    }

    options.Address = new Uri(grpcUrl);
});

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseCors("AllowAll");

app.UseSerilogRequestLogging();

app.UseHttpsRedirection();

app.UseIdentityServer();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

Log.Information("Seeding database.");
SeedData.EnsureSeedData(app);
Log.Information("Done seeding database.");

app.Run();
