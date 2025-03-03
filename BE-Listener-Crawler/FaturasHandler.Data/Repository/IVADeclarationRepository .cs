using FaturasHandler.Data.Context;
using FaturasHandler.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturasHandler.Data.Repository
{
    public class IVADeclarationRepository : Repository<IVADeclaration>, IIVADeclarationRepository

    {
        public IVADeclarationRepository(FaturaDbContext context) : base(context)
        {

        }

        public async Task<IEnumerable<IVADeclaration>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<IVADeclaration>()
                .Where(d => d.UserId == userId)
                .ToListAsync(cancellationToken);
        }
    }
}
