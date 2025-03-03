using FaturasHandler.Data.Models;

namespace FaturasHandler.Services.Interfaces
{
    public interface IUserDataService
    {
        Task<UserData> AddAsync(UserData user, CancellationToken cancellationToken);
        Task<IEnumerable<UserData>> GetAllAsync(CancellationToken cancellationToken);
        Task<UserData> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken);
        Task<UserData> UpdateAsync(Guid id, UserData user, CancellationToken cancellationToken);
    }
}
