using FaturasHandler.Data.Models;
using FaturasHandler.Data.Repository;
using FaturasHandler.Services.Interfaces;

namespace FaturasHandler.Services.Services
{
    public class ReciboVerdeService : IReciboVerdeService
    {
        private readonly IReciboVerdeRepository _repository;

        public ReciboVerdeService(IReciboVerdeRepository repository)
        {
            _repository = repository;
        }

        // ✅ Create
        public async Task<ReciboVerde> AddAsync(ReciboVerde recibo, Guid userId, CancellationToken cancellationToken)
        {
            recibo.UserId = userId; // Ensure UserId is set
            return await _repository.AddAsync(recibo, cancellationToken);
        }

        // ✅ CreateMany
        public async Task<int> AddManyAsync(List<ReciboVerde> recibo, Guid userId, CancellationToken cancellationToken)
        {
            return await _repository.AddManyAsync(recibo, cancellationToken);
        }

        // ✅ Read (All for a user)
        public async Task<IEnumerable<ReciboVerde>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _repository.GetAllByUserAsync(userId, cancellationToken);
        }

        // ✅ Read (By ID and User)
        public async Task<ReciboVerde> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var recibo = await _repository.GetByIdAsync(id, cancellationToken);
            return recibo != null && recibo.UserId == userId ? recibo : null;
        }

        // ✅ Update
        public async Task<ReciboVerde> UpdateAsync(Guid id, Guid userId, ReciboVerde recibo, CancellationToken cancellationToken)
        {
            var existingRecibo = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingRecibo == null || existingRecibo.UserId != userId) return null;

            existingRecibo.ValorBase = recibo.ValorBase;
            existingRecibo.ValorIVA = recibo.ValorIVA;
            existingRecibo.ValorIRS = recibo.ValorIRS;
            existingRecibo.ValorTotalCImpostos = recibo.ValorTotalCImpostos;
            existingRecibo.ImportanciaRecebida = recibo.ImportanciaRecebida;
            existingRecibo.DataEmissao = recibo.DataEmissao;

            return await _repository.UpdateAsync(existingRecibo, cancellationToken);
        }

        // ✅ Delete
        public async Task<bool> RemoveAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var recibo = await _repository.GetByIdAsync(id, cancellationToken);
            if (recibo == null || recibo.UserId != userId) return false;

            return await _repository.RemoveAsync(recibo, cancellationToken);
        }
    }
}
