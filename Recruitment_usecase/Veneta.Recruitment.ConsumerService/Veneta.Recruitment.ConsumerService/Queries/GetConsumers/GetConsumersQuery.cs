using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.Models;

namespace Veneta.Recruitment.ConsumerService.Queries.GetConsumers;

public sealed record GetConsumersQuery: IQuery<IList<ConsumerView>>;