using Veneta.Recruitment.ConsumerService.Commands.UpdateConsumer;
using Veneta.Recruitment.ConsumerService.Dependencies;
using Veneta.Recruitment.ConsumerService.ValueObjects;

namespace Veneta.Recruitment.ConsumerService.Requests
{
    public record UpdateConsumerRequest(Address? Address, string? FirstName, string? LastName)
    {
        public ICommand ToCommand(Guid id)
        {
            return new UpdateConsumerCommand(
                new ConsumerId(id),
                !string.IsNullOrEmpty(FirstName) ? new FirstName(FirstName) : null,
                !string.IsNullOrEmpty(LastName) ? new LastName(LastName) : null,
                Address
            );
        }
    }
}