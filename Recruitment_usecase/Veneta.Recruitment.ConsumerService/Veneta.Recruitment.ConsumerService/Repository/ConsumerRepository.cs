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

        public async Task<ConsumerView?> Get(Guid id, CancellationToken cancellationToken = default)
        {
            return await _context.Consumers
                .Include(x => x.Address)
                .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
        }

        public async Task Save(ConsumerView consumer)
        {
            var existing = await _context.Consumers.AnyAsync(x => x.Id == consumer.Id);

            if (!existing)
                await _context.Consumers.AddAsync(consumer);
            else
                _context.Consumers.Update(consumer);

            await _context.SaveChangesAsync();
        }

        public async Task<IList<ConsumerView>> GetAll(CancellationToken cancellationToken)
        {
            return await _context.Consumers
                .Include(x => x.Address)
                .ToListAsync(cancellationToken);
        }
    }
}