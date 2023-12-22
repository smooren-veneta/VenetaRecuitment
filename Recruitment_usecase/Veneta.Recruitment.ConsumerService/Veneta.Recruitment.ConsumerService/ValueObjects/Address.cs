using Veneta.Recruitment.ConsumerService.Models;

namespace Veneta.Recruitment.ConsumerService.ValueObjects;

public sealed record Address(string Street, string PostalCode, string HouseNumber)
{
    public AddressView Map(ConsumerId consumerId)
    {
        return new()
        {
            Street = Street,
            PostalCode = PostalCode,
            HouseNumber = HouseNumber,
            ConsumerId = consumerId.Value
        };
    }
}