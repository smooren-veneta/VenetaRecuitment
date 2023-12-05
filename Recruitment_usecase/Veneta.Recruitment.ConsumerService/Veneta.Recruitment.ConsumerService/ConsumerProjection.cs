using NEventStore;
using NEventStore.PollingClient;
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
                _client = new PollingClient2(_storeEvents.Advanced, commit =>
                {
                    foreach (var eventMessage in commit.Events)
                    {
                        var @event = eventMessage.Body;
                        //use the events to create and store the projection
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
    }
}
