using FaturasHandler.Data.Models;

namespace FaturasHandler.Services.Interfaces
{
    public interface IUserActionService
    {
        Task<UserAction> CreateUserActionAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<UserAction>> GetUserActionsByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<UserAction> GetUserActionByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> RemoveUserActionAsync(Guid id, CancellationToken cancellationToken);
        Task<IEnumerable<UserAction>> GetAllPendingActionsAsync(CancellationToken cancellationToken);
        Task<UserAction> UpdateAsync(UserAction updatedAction, CancellationToken cancellationToken);
    }
}
