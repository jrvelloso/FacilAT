using FaturasHandler.Data.Context;
using FaturasHandler.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace FaturasHandler.Data.Repository
{
    public class UserActionRepository : Repository<UserAction>, IUserActionRepository
    {
        public UserActionRepository(FaturaDbContext context) : base(context)
        {
        }

        public async Task<IEnumerable<UserAction>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _context.Set<UserAction>()
                .Where(d => d.UserId == userId)
                .ToListAsync(cancellationToken);
        }

        public async Task<IEnumerable<UserAction>> GetAllPendingActionsAsync(CancellationToken cancellationToken)
        {
            return await _context.Set<UserAction>()
                .Include(u => u.User)
                .Where(ua => ua.Active)
                .ToListAsync(cancellationToken);
        }
    }
}


