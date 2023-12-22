using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.Models;

namespace Veneta.Recruitment.ConsumerService.Queries.GetConsumer;

public sealed record GetConsumerQuery : IQuery<ConsumerView?>
{
    public Guid ConsumerId { get; init; }
}