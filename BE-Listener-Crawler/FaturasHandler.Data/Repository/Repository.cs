using FaturasHandler.Data.Context;
using Microsoft.EntityFrameworkCore;

namespace FaturasHandler.Data.Repository
{
    public abstract class Repository<T> : IRepository<T> where T : class, new()
    {
        protected readonly FaturaDbContext _context;
        protected readonly DbSet<T> DbSet;

        protected Repository(FaturaDbContext context)
        {
            _context = context;
            DbSet = _context.Set<T>();
        }

        public virtual async Task<T> AddAsync(T item, CancellationToken cancellationToken)
        {
            await DbSet.AddAsync(item, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public virtual async Task<int> AddManyAsync(List<T> itemList, CancellationToken cancellationToken)
        {
            await DbSet.AddRangeAsync(itemList, cancellationToken);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // ✅ Fixed: Changed ID type to `Guid`
        public virtual async Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await DbSet.FindAsync(id, cancellationToken);
        }

        public virtual async Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await DbSet.ToListAsync(cancellationToken);
        }

        public virtual async Task<T> UpdateAsync(T item, CancellationToken cancellationToken)
        {
            DbSet.Update(item);
            await _context.SaveChangesAsync(cancellationToken);
            return item;
        }

        public virtual async Task<int> UpdateManyAsync(IEnumerable<T> itemList, CancellationToken cancellationToken)
        {
            DbSet.UpdateRange(itemList);
            return await _context.SaveChangesAsync(cancellationToken);
        }

        // ✅ Fixed: Return `bool` instead of `void`
        public virtual async Task<bool> RemoveAsync(T item, CancellationToken cancellationToken)
        {
            DbSet.Remove(item);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> RemoveManyAsync(IEnumerable<T> itemList, CancellationToken cancellationToken)
        {
            DbSet.RemoveRange(itemList);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public virtual async Task<bool> RemoveAllAsync(CancellationToken cancellationToken)
        {
            var all = await GetAllAsync(cancellationToken);
            DbSet.RemoveRange(all);
            return await _context.SaveChangesAsync(cancellationToken) > 0;
        }

        public void Dispose()
        {
            _context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}
