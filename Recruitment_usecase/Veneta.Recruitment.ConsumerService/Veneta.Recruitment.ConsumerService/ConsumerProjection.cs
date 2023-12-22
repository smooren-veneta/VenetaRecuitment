using NEventStore;
using NEventStore.PollingClient;
using Veneta.Recruitment.ConsumerService.Models;
using Veneta.Recruitment.ConsumerService.Repository;

namespace Veneta.Recruitment.ConsumerService
{
    public class ConsumerProjection : IHostedService
    {
        private readonly IStoreEvents _storeEvents;
        private readonly IServiceProvider _serviceProvider;
        private PollingClient2 _client;
        private IServiceScope _scope;

        public ConsumerProjection(IStoreEvents storeEvents, IServiceProvider serviceProvider)
        {
            _storeEvents = storeEvents;
            _serviceProvider = serviceProvider;
        }

        public Task StartAsync(CancellationToken cancellationToken)
        {
            Task.Run(() =>
            {
                _scope = _serviceProvider.CreateScope();
                var consumerRepository = _scope.ServiceProvider.GetRequiredService<ConsumerRepository>();
                var logger = _scope.ServiceProvider.GetRequiredService<ILogger<ConsumerProjection>>();
                _client = new PollingClient2(_storeEvents.Advanced, (commit) =>
                    {
                        var consumerId = Guid.Parse(commit.StreamId);
                        var consumer = consumerRepository.Get(consumerId, cancellationToken).GetAwaiter().GetResult();

                        foreach (var eventMessage in commit.Events)
                        {
                            if (eventMessage.Body is not ConsumerAggregateEvent @event)
                            {
                                logger.LogWarning("Event {EventType} is not a ConsumerAggregateEvent",
                                    eventMessage.Body.GetType().Name);
                                continue;
                            }

                            logger.LogInformation("Processing event {EventType}", @event.GetType().Name);

                            var projection = MapToProjection(@event, consumer);

                            consumerRepository.Save(projection).GetAwaiter().GetResult();
                        }

                        return PollingClient2.HandlingResult.MoveToNext;
                    },
                    waitInterval: 500);
                _client.StartFromBucket("consumer");
            }, cancellationToken);
            return Task.CompletedTask;
        }

        public Task StopAsync(CancellationToken cancellationToken)
        {
            _client.Stop();
            _client.Dispose();
            _scope.Dispose();
            return Task.CompletedTask;
        }

        private ConsumerView MapToProjection(ConsumerAggregateEvent @event, ConsumerView? consumer)
        {
            if (@event is ConsumerAggregateEvent.ConsumerCreatedEvent createdEvent)
            {
                return new ConsumerView
                {
                    Id = createdEvent.ConsumerId.Value,
                    Address = createdEvent.Address.Map(createdEvent.ConsumerId),
                    FirstName = createdEvent.FirstName.Value,
                    LastName = createdEvent.LastName.Value
                };
            }


            if (@event is ConsumerAggregateEvent.ConsumerNameUpdatedEvent nameUpdatedEvent)
            {
                consumer!.FirstName = nameUpdatedEvent.FirstName?.Value;
                consumer!.LastName = nameUpdatedEvent.LastName?.Value;

                return consumer!;
            }

            if (@event is ConsumerAggregateEvent.ConsumerAddressUpdatedEvent addressUpdatedEvent)
            {
                consumer!.Address = addressUpdatedEvent.Address.Map(addressUpdatedEvent.ConsumerId);

                return consumer!;
            }

            throw new ArgumentOutOfRangeException(nameof(@event));
        }
    }
}