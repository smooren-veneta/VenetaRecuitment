using Veneta.Recruitment.ConsumerService.Commands.CreateConsumer;
using Veneta.Recruitment.ConsumerService.Commands.UpdateConsumer;
using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService;

public class ConsumerAggregate : Aggregate<ConsumerAggregateEvent, ConsumerAggregateState>
{
    public ConsumerAggregate()
    {
    }

    public override Task Initialize(AggregateId aggregateId,
        ICollection<IAggregateEvent<ConsumerAggregateEvent>> events)
    {
        AggregateId = aggregateId;

        if (events.Any())
            events.ToList().ForEach(Apply);
        else
            State = ConsumerAggregateState.Default;


        return Task.CompletedTask;
    }

    protected override ConsumerAggregateState MapState(IAggregateEvent<ConsumerAggregateEvent> @event)
    {
        return @event switch
        {
            ConsumerAggregateEvent.ConsumerCreatedEvent => new ConsumerAggregateState(ConsumerStatus.Created),
            ConsumerAggregateEvent.ConsumerNameUpdatedEvent or ConsumerAggregateEvent.ConsumerAddressUpdatedEvent =>
                new ConsumerAggregateState(ConsumerStatus.Updated),
            _ => new ConsumerAggregateState(ConsumerStatus.Init)
        };
    }

    public override Task Handle(ICommand command)
    {
        var events = new List<ConsumerAggregateEvent>();

        switch (State.Status)
        {
            case ConsumerStatus.Init:
                events.AddRange(Create((CreateConsumerCommand)command));
                break;
            case ConsumerStatus.Created:
            case ConsumerStatus.Updated:
                events.AddRange(Update((UpdateConsumerCommand)command));
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }

        events.ForEach(Apply);

        return Task.CompletedTask;
    }

    private IList<ConsumerAggregateEvent> Create(CreateConsumerCommand command)
    {
        var consumerId = new ConsumerId(command.ConsumerId.Value);
        var @event =
            new ConsumerAggregateEvent.ConsumerCreatedEvent(consumerId, command.Address, command.FirstName,
                command.LastName);

        return new List<ConsumerAggregateEvent>() { @event };
    }

    private IList<ConsumerAggregateEvent> Update(UpdateConsumerCommand command)
    {
        var result = new List<ConsumerAggregateEvent>();

        var consumerId = new ConsumerId(command.ConsumerId.Value);

        if (command.Address is not null)
        {
            var addressChangeEvent =
                new ConsumerAggregateEvent.ConsumerAddressUpdatedEvent(consumerId, command.Address);
            result.Add(addressChangeEvent);
        }

        if (command.FirstName is not null || command.LastName is not null)
        {
            var nameChangeEvent =
                new ConsumerAggregateEvent.ConsumerNameUpdatedEvent(consumerId, command.FirstName, command.LastName);
            result.Add(nameChangeEvent);
        }

        return result;
    }
}

public record ConsumerAggregateState(ConsumerStatus Status) : IAggregateState<ConsumerAggregateState>
{
    public static ConsumerAggregateState Default => new(ConsumerStatus.Init);
}

public enum ConsumerStatus
{
    Init,
    Created,
    Updated
}

public record ConsumerAggregateEvent : IAggregateEvent<ConsumerAggregateEvent>
{
    public record ConsumerCreatedEvent(ConsumerId ConsumerId, Address Address, FirstName FirstName, LastName LastName)
        : ConsumerAggregateEvent;

    public record ConsumerNameUpdatedEvent(ConsumerId ConsumerId, FirstName? FirstName, LastName? LastName)
        : ConsumerAggregateEvent;

    public record ConsumerAddressUpdatedEvent(ConsumerId ConsumerId, Address Address) : ConsumerAggregateEvent;
}