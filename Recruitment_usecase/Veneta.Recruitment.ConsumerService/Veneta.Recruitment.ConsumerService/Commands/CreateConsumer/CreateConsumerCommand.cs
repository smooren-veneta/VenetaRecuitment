using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Commands.CreateConsumer;

public sealed record CreateConsumerCommand(ConsumerId ConsumerId, Address Address, FirstName FirstName, LastName LastName) : ICommand;