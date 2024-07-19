using Caregivers.Suppliers.Api;
using Caregivers.Suppliers.Api.Data;
using Caregivers.Suppliers.Api.Domain;
using Caregivers.Suppliers.Api.Services;
using HealthChecks.UI.Client;
using Microsoft.EntityFrameworkCore;
using Hangfire;
using Hangfire.MemoryStorage;
using Caregivers.Suppliers.Api.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<SupplierContext>(opt =>
{
    opt.UseInMemoryDatabase("Suppliers");
});
builder.Services.AddHangfire((opt) =>
{
    opt.UseMemoryStorage();
});
builder.Services.AddHangfireServer();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddScoped<SupplierService>();
builder.Services.AddSingleton<SupplierRepository>();
builder.Services.AddHostedService<WorkerService>();
builder.Services.AddScoped<SchedulerWorker>();
builder.Services.AddHealthChecks();


var app = builder.Build();
app.UseHealthChecks("/health", new Microsoft.AspNetCore.Diagnostics.HealthChecks.HealthCheckOptions
{
    Predicate = p => true,
    ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
});

app.UseHttpsRedirection();


app.MapPost("/suppliers", (Supplier supplier, SupplierContext _context) =>
{ 
    if(_context.Suppliers.Any(x => x.Id == supplier.Id))
        _context.Update(supplier);
    else
        _context.Add(supplier);

    _context.SaveChanges();
});

app.MapGet("/suppliers", (double latitude, double longitude, SupplierContext _context) =>
{
    var suppliers = _context.Suppliers.Where(x => x.Ray >= Distance.CalculateDistance(latitude, longitude,
                                                                                  x.Latitude, x.Longitude)).ToList();
    return suppliers;
});

app.UseHangfireDashboard("/hangfire");

app.Run();