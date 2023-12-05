using Microsoft.EntityFrameworkCore;
using Veneta.Recruitment.ConsumerService.Models;

namespace Veneta.Recruitment.ConsumerService.Repository
{
    public class AppDbContext : DbContext
    {
        public DbSet<ConsumerView> Consumers { get; set; }
        public DbSet<AddressView> Addresses { get; set; }
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options)
        {
        }

    }
}
