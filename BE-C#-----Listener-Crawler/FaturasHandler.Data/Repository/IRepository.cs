namespace FaturasHandler.Data.Repository
{
    public interface IRepository<T> where T : class, new()
    {
        Task<T> AddAsync(T item, CancellationToken cancellationToken);
        Task<int> AddManyAsync(List<T> itemList, CancellationToken cancellationToken);
        void Dispose();
        Task<IEnumerable<T>> GetAllAsync(CancellationToken cancellationToken);
        Task<T> GetByIdAsync(Guid id, CancellationToken cancellationToken);
        Task<bool> RemoveAllAsync(CancellationToken cancellationToken);
        Task<bool> RemoveAsync(T item, CancellationToken cancellationToken);
        Task<bool> RemoveManyAsync(IEnumerable<T> itemList, CancellationToken cancellationToken);
        Task<T> UpdateAsync(T item, CancellationToken cancellationToken);
        Task<int> UpdateManyAsync(IEnumerable<T> itemList, CancellationToken cancellationToken);
    }
}