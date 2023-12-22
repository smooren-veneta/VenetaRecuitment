using Veneta.Recruitment.ConsumerService.Commands.CreateConsumer;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Requests;

public record CreateConsumerRequest(Guid Id, Address Address, string? FirstName, string? LastName)
{
    public CreateConsumerCommand ToCommand()
    {
        return new CreateConsumerCommand(
            new ConsumerId(Id),
            Address,
            new FirstName(FirstName ?? ""),
            new LastName(LastName ?? "")
        );
    }
}