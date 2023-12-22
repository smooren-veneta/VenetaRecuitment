using NEventStore;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Dependencies;

public interface IAggregateFactory
{
    Task<TAggregate> AggregateForId<TAggregate, TAggregateEvent, TAggregateState>(AggregateId aggregateId)
        where TAggregate : Aggregate<TAggregateEvent, TAggregateState>
        where TAggregateEvent : IAggregateEvent<TAggregateEvent>
        where TAggregateState : IAggregateState<TAggregateState>;

    Task CommitAggregate<TAggregate, TAggregateEvent, TAggregateState>(TAggregate aggregate)
        where TAggregate : Aggregate<TAggregateEvent, TAggregateState>
        where TAggregateEvent : IAggregateEvent<TAggregateEvent>
        where TAggregateState : IAggregateState<TAggregateState>;
}

public class AggregateFactory : IAggregateFactory
{
    private readonly IStoreEvents _store;

    public AggregateFactory(IStoreEvents store)
    {
        _store = store;
    }


    public async Task<TAggregate> AggregateForId<TAggregate, TAggregateEvent, TAggregateState>(AggregateId aggregateId)
        where TAggregate : Aggregate<TAggregateEvent, TAggregateState>
        where TAggregateEvent : IAggregateEvent<TAggregateEvent>
        where TAggregateState : IAggregateState<TAggregateState>
    {
        using var stream = _store.OpenStream("consumer", aggregateId.Value);
        var events = stream.CommittedEvents
            .Select(x => x.Body)
            .Cast<IAggregateEvent<TAggregateEvent>>()
            .ToList();
        var aggregate = (TAggregate)Activator.CreateInstance(typeof(TAggregate))!;
        if (aggregate is null)
            throw new InvalidOperationException($"Could not create aggregate of type {typeof(TAggregate)}");
        await aggregate.Initialize(aggregateId, events);

        return aggregate;
    }

    public async Task CommitAggregate<TAggregate, TAggregateEvent, TAggregateState>(TAggregate aggregate)
        where TAggregate : Aggregate<TAggregateEvent, TAggregateState>
        where TAggregateEvent : IAggregateEvent<TAggregateEvent>
        where TAggregateState : IAggregateState<TAggregateState>
    {
        using var stream = _store.OpenStream("consumer", aggregate.AggregateId!.Value);
        foreach (var @event in aggregate.Events)
        {
            if (stream.CommittedEvents.Any(x => x.Body.Equals(@event)))
                continue;

            stream.Add(new EventMessage { Body = @event });
        }

        stream.CommitChanges(Guid.NewGuid());

        await Task.CompletedTask;
    }
}