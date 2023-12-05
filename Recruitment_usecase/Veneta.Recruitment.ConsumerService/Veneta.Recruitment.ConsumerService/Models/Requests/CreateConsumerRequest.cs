namespace Veneta.Recruitment.ConsumerService.Models.Requests
{
    public record CreateConsumerRequest(Guid Id, Address Address, string? FirstName, string? LastName);
    public record Address(string Street, string PostalCode, string HouseNumber);
}
