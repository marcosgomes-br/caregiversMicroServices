using Caregivers.Consumers.Api.Data;
using Caregivers.Consumers.Api.Domain;
using Caregivers.Consumers.Api.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<ConsumerService>();
builder.Services.AddDbContext<ConsumerContext>(opt =>
{
    opt.UseInMemoryDatabase("Consumers");
});

var app = builder.Build();

app.UseSwagger();
app.UseSwaggerUI();

app.UseHttpsRedirection();

app.MapPost("/consumer", (Consumer consumer, ConsumerContext _context, ConsumerService _service) =>
{
    _context.Add(consumer);
    _context.SaveChanges();

    _service.SendMessage(consumer);
});

app.MapGet("/consumer", (ConsumerContext _context) => _context.Consumers.ToList());

app.Run();
