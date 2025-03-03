using FaturasHandler.Data.Models;

namespace FaturasHandler.Data.Repository
{
    public interface IUserActionRepository : IRepository<UserAction>
    {
        //Task<IEnumerable<UserAction>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
        //Task<IEnumerable<UserAction>> GetAllPendingActionsAsync(CancellationToken cancellationToken);
        Task<IEnumerable<UserAction>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken);
        Task<IEnumerable<UserAction>> GetAllPendingActionsAsync(CancellationToken cancellationToken);
    }
}
