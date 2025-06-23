using FileService.API.Interfaces;
using FileService.API.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddScoped<IPhotoStore, FileSystemPhotoStore>();
builder.Services.AddGrpc();

var app = builder.Build();

app.UseStaticFiles();

app.MapGrpcService<GrpcPhotoService>();

app.UseHttpsRedirection();

app.Run();
