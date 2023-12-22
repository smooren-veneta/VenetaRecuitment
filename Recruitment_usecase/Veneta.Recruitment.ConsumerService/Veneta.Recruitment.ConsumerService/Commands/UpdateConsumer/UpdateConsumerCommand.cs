using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Commands.UpdateConsumer;

public sealed record UpdateConsumerCommand(ConsumerId ConsumerId, FirstName? FirstName, LastName? LastName, Address? Address) : ICommand;