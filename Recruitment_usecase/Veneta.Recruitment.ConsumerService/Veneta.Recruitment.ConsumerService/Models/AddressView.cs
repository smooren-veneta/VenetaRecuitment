namespace Veneta.Recruitment.ConsumerService.Models
{
    public class AddressView {
        public Guid Id { get; set; }
        public required string Street { get; set; }
        public required string PostalCode { get; set; }
        public required string HouseNumber { get; set; }
        public Guid ConsumerId { get; set; }
    }
}
