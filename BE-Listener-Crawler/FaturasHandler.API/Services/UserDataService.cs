using FaturasHandler.Data.Models;
using FaturasHandler.Data.Repository;
using FaturasHandler.Services.Interfaces;

namespace FaturasHandler.Services.Services
{
    public class UserDataService : IUserDataService
    {
        private readonly IUserDataRepository _repository;

        public UserDataService(IUserDataRepository repository)
        {
            _repository = repository;
        }

        // ✅ Create
        public async Task<UserData> AddAsync(UserData user, CancellationToken cancellationToken)
        {
            return await _repository.AddAsync(user, cancellationToken);
        }

        // ✅ Read (All users)
        public async Task<IEnumerable<UserData>> GetAllAsync(CancellationToken cancellationToken)
        {
            return await _repository.GetAllAsync(cancellationToken);
        }

        // ✅ Read (By ID)
        public async Task<UserData> GetByIdAsync(Guid id, CancellationToken cancellationToken)
        {
            return await _repository.GetByIdAsync(id, cancellationToken);
        }

        //// ✅ Read (By Email)
        //public async Task<UserData> GetByEmailAsync(string email, CancellationToken cancellationToken)
        //{
        //    return await _repository.GetByEmailAsync(email, cancellationToken);
        //}

        // ✅ Update
        public async Task<UserData> UpdateAsync(Guid id, UserData user, CancellationToken cancellationToken)
        {
            var existingUser = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingUser == null) return null;

            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.NIF = user.NIF;
            existingUser.Password = user.Password;

            return await _repository.UpdateAsync(existingUser, cancellationToken);
        }

        // ✅ Delete
        public async Task<bool> RemoveAsync(Guid id, CancellationToken cancellationToken)
        {
            var user = await _repository.GetByIdAsync(id, cancellationToken);
            if (user == null) return false;

            return await _repository.RemoveAsync(user, cancellationToken);
        }
    }
}
