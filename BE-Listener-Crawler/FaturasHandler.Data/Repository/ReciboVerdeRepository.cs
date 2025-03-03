using FaturasHandler.Data.Context;
using FaturasHandler.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturasHandler.Data.Repository
{
    public class ReciboVerdeRepository : Repository<ReciboVerde>, IReciboVerdeRepository
    {
        public ReciboVerdeRepository(FaturaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<ReciboVerde>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<ReciboVerde>()
                .Where(d => d.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
