using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Commands.UpdateConsumer;

public interface IUpdateConsumerCommandHandler : ICommandHandler<UpdateConsumerCommand>;

public class UpdateConsumerCommandHandler : IUpdateConsumerCommandHandler
{
    private readonly IAggregateFactory _aggregateFactory;

    public UpdateConsumerCommandHandler(IAggregateFactory aggregateFactory)
    {
        _aggregateFactory = aggregateFactory;
    }

    public async Task Handle(UpdateConsumerCommand command, CancellationToken cancellationToken)
    {
        var aggregateId = new AggregateId(command.ConsumerId.Value);
        var aggregate =
            await _aggregateFactory.AggregateForId<ConsumerAggregate, ConsumerAggregateEvent, ConsumerAggregateState>(
                aggregateId);

        await aggregate.Handle(command);

        await _aggregateFactory
            .CommitAggregate<ConsumerAggregate, ConsumerAggregateEvent, ConsumerAggregateState>(aggregate);
    }
}