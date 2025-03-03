using FaturasHandler.Data.Models;

namespace FaturasHandler.Services.Interfaces
{
    public interface IReciboVerdeService
    {
        Task<ReciboVerde> AddAsync(ReciboVerde recibo, Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<ReciboVerde>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<ReciboVerde> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<bool> RemoveAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<ReciboVerde> UpdateAsync(Guid id, Guid userId, ReciboVerde recibo, CancellationToken cancellationToken);
        Task<int> AddManyAsync(List<ReciboVerde> recibo, Guid userId, CancellationToken cancellationToken);
    }
}
