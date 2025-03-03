using FaturasHandler.Data.Models;

namespace FaturasHandler.Data.Repository
{
    public interface IReciboVerdeRepository : IRepository<ReciboVerde>
    {
        Task<IEnumerable<ReciboVerde>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}