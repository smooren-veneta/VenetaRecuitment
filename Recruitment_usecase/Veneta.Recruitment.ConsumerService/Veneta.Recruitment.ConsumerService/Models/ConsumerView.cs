using System.ComponentModel.DataAnnotations.Schema;

namespace Veneta.Recruitment.ConsumerService.Models
{
    public class ConsumerView {

        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; }
        public required AddressView Address { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
    }
}
