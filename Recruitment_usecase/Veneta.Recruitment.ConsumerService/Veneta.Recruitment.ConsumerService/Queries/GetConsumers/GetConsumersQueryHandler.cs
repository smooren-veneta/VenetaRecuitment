using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.Models;
using Veneta.Recruitment.ConsumerService.Repository;

namespace Veneta.Recruitment.ConsumerService.Queries.GetConsumers;

public interface IGetConsumersQueryHandler : IQueryHandler<GetConsumersQuery, IList<ConsumerView>>;

public class GetConsumersQueryHandler : IGetConsumersQueryHandler
{
    private readonly ConsumerRepository _consumerRepository;

    public GetConsumersQueryHandler(ConsumerRepository consumerRepository)
    {
        _consumerRepository = consumerRepository;
    }

    public async Task<IList<ConsumerView>> Handle(GetConsumersQuery request, CancellationToken cancellationToken)
    {
        var consumers = await _consumerRepository.GetAll(cancellationToken);

        return consumers;
    }
}