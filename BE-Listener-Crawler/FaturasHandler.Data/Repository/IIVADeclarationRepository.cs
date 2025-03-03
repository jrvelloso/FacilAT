using FaturasHandler.Data.Models;

namespace FaturasHandler.Data.Repository
{
    public interface IIVADeclarationRepository : IRepository<IVADeclaration>
    {
        Task<IEnumerable<IVADeclaration>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
    }
}