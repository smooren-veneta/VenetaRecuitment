using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Commands.CreateConsumer;

public interface ICreateConsumerCommandHandler : ICommandHandler<CreateConsumerCommand>;

public class CreateConsumerCommandHandler : ICreateConsumerCommandHandler
{
    private readonly IAggregateFactory _aggregateFactory;

    public CreateConsumerCommandHandler(IAggregateFactory aggregateFactory)
    {
        _aggregateFactory = aggregateFactory;
    }

    public async Task Handle(CreateConsumerCommand command, CancellationToken cancellationToken)
    {
        var aggregateId = new AggregateId(command.ConsumerId.Value);
        var aggregate = await _aggregateFactory.AggregateForId<ConsumerAggregate, ConsumerAggregateEvent, ConsumerAggregateState>(aggregateId);

        await aggregate.Handle(command);
        
        await _aggregateFactory.CommitAggregate<ConsumerAggregate, ConsumerAggregateEvent, ConsumerAggregateState>(aggregate);
    }
}