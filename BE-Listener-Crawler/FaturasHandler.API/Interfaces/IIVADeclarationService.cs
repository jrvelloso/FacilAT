using FaturasHandler.Data.Models;

namespace FaturasHandler.Services.Interfaces
{
    public interface IIVADeclarationService
    {
        Task<IVADeclaration> AddAsync(IVADeclaration declaration, Guid userId, CancellationToken cancellationToken);
        Task<int> AddManyAsync(List<IVADeclaration> tax, Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<IVADeclaration>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<IVADeclaration> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<bool> RemoveAsync(Guid id, Guid userId, CancellationToken cancellationToken);
        Task<IVADeclaration> UpdateAsync(Guid id, Guid userId, IVADeclaration declaration, CancellationToken cancellationToken);
    }
}
