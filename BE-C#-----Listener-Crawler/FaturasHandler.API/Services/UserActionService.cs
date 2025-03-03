using FaturasHandler.Data.Models;
using FaturasHandler.Data.Repository;
using FaturasHandler.Services.Interfaces;

namespace FaturasHandler.Services.Services
{
    public class UserActionService : IUserActionService
    {
        private readonly IUserActionRepository _repository;

        public UserActionService(IUserActionRepository repository)
        {
            _repository = repository;
        }

        // ✅ Create a new UserAction
        public async Task<UserAction> CreateUserActionAsync(Guid userId, CancellationToken cancellationToken)
        {
            var userAction = new UserAction
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                RequestedAt = DateTime.UtcNow,
                Active = true
            };

            return await _repository.AddAsync(userAction, cancellationToken);
        }

        public async Task<IEnumerable<UserAction>> GetAllPendingActionsAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetAllPendingActionsAsync(cancellationToken);

        }

        // ✅ Get All Actions for a User
        public async Task<IEnumerable<UserAction>> GetUserActionsByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _repository.GetAllByUserAsync(userId, cancellationToken);
        }

        // ✅ Get a Single UserAction by ID
        public async Task<UserAction> GetUserActionByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }

        // ✅ Remove a UserAction
        public async Task<bool> RemoveUserActionAsync(Guid id, CancellationToken cancellationToken)
        {
            var existingAction = await _repository.GetByIdAsync(id, cancellationToken);

            if (existingAction == null)
                return false; // Return false if the action does not exist

            //await _repository.DeleteAsync(existingAction, cancellationToken);
            return true;
        }

        public async Task<UserAction> UpdateAsync(UserAction updatedAction, CancellationToken cancellationToken)
        {
            var existingAction = await _repository.GetByIdAsync(updatedAction.Id, cancellationToken);

            if (existingAction == null || existingAction.UserId != updatedAction.UserId)
                return null; // Return null if not found or not owned by the user

            // Update fields that are allowed to be changed
            existingAction.RequestedAt = updatedAction.RequestedAt;
            existingAction.Active = updatedAction.Active;

            return await _repository.UpdateAsync(existingAction, cancellationToken);
        }

    }
}
