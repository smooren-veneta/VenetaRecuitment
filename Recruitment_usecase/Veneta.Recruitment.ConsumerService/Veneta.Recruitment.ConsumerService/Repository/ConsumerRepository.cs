using Microsoft.EntityFrameworkCore;
using Veneta.Recruitment.ConsumerService.Models;

namespace Veneta.Recruitment.ConsumerService.Repository
{
    public class ConsumerRepository
    {
        private readonly AppDbContext _context;

        public ConsumerRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<ConsumerView?> Get(Guid id)
        {
            return await _context.Consumers
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id);
        }
    }
}
