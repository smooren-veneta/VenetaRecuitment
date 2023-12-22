using Microsoft.EntityFrameworkCore;
using NEventStore;
using NEventStore.Serialization.Json;
using Veneta.Recruitment.ConsumerService;
using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.Repository;

var builder = WebApplication.CreateBuilder(args);

//setup journal support
var store = Wireup.Init()
    .UsingInMemoryPersistence()
    .InitializeStorageEngine()
    .UsingJsonSerialization()
    .Build();
builder.Services.AddSingleton(_ => store);

//setup database support
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseInMemoryDatabase("InMemoryDb"));
builder.Services.AddTransient<ConsumerRepository>();

//register Projection Service
builder.Services.AddHostedService<ConsumerProjection>();

// MediatR
builder.Services.AddMediatR(cfg => cfg.RegisterServicesFromAssemblyContaining<Program>());

builder.Services.AddScoped<Aggregate<ConsumerAggregateEvent, ConsumerAggregateState>, ConsumerAggregate>();
builder.Services.AddScoped<IAggregateFactory, AggregateFactory>();

var app = builder.Build();

app.MapEndpoints();

app.Run();

public partial class Program
{
}