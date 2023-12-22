using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Dependencies;

public abstract class Aggregate<TEvent, TAggregateState> 
    where TEvent : IAggregateEvent<TEvent>
    where TAggregateState : IAggregateState<TAggregateState>
{
    public AggregateId? AggregateId { get; protected set; }
    public TAggregateState State { get; protected set; } = default!;
    
    public IReadOnlyCollection<IAggregateEvent<TEvent>> Events => _events.ToList();

    private IList<IAggregateEvent<TEvent>> _events = new List<IAggregateEvent<TEvent>>();

    protected void Apply(IAggregateEvent<TEvent> @event)
    {
        _events.Add(@event);
        
        State = MapState(@event);
    }
    
    protected abstract TAggregateState MapState(IAggregateEvent<TEvent> @event);
    
    public abstract Task Initialize(AggregateId aggregateId, ICollection<IAggregateEvent<TEvent>> events);

    public abstract Task Handle(ICommand command);
}

public interface IAggregateState<out T>
{
    static abstract T Default { get; }
}