using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NEventStore;
using NEventStore.Serialization.Json;
using Veneta.Recruitment.ConsumerService;
using Veneta.Recruitment.ConsumerService.Models.Requests;
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

var app = builder.Build();

//register api endpoints
var consumersGroup = app.MapGroup("/consumers"); 

consumersGroup.MapGet("/{id:guid}",
    async (Guid id, [FromServices] ConsumerRepository c, CancellationToken cancellationToken) =>
    {
        return await c.Get(id);
    });

consumersGroup.MapPost(string.Empty,
    (CreateConsumerRequest createConsumer, [FromServices] IStoreEvents store) =>
    {
        using var stream = store.OpenStream("consumer", createConsumer.Id);
        //example on how to get all committed events for this consumer from the eventStore
        //var previousEvents = stream.CommittedEvents; 

        //example on how to add an event to the journal 
        //stream.Add(new EventMessage{ Body = new object()});
        //stream.CommitChanges(Guid.NewGuid());
    });

//add the missing endpoints here

app.Run(); 

public partial class Program { }