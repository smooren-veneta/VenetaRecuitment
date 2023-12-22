using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.Models;
using Veneta.Recruitment.ConsumerService.Repository;

namespace Veneta.Recruitment.ConsumerService.Queries.GetConsumer;

public interface IGetConsumerQueryHandler : IQueryHandler<GetConsumerQuery, ConsumerView?>;

public class GetConsumerQueryHandler : IGetConsumerQueryHandler
{
    private readonly ConsumerRepository _consumerRepository;

    public GetConsumerQueryHandler(ConsumerRepository consumerRepository)
    {
        _consumerRepository = consumerRepository;
    }

    public async Task<ConsumerView?> Handle(GetConsumerQuery request, CancellationToken cancellationToken)
    {
        var consumer = await _consumerRepository.Get(request.ConsumerId, cancellationToken);

        return consumer ?? null;
    }
}