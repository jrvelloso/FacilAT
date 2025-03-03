using FaturasHandler.Data.Models;
using FaturasHandler.Data.Repository;
using FaturasHandler.Services.Interfaces;

namespace FaturasHandler.Services.Services
{
    public class IVADeclarationService : IIVADeclarationService
    {
        private readonly IIVADeclarationRepository _repository;

        public IVADeclarationService(IIVADeclarationRepository repository)
        {
            _repository = repository;
        }

        // ✅ Create
        public async Task<IVADeclaration> AddAsync(IVADeclaration declaration, Guid userId, CancellationToken cancellationToken)
        {
            declaration.UserId = userId; // Ensure UserId is set
            return await _repository.AddAsync(declaration, cancellationToken);
        }

        // ✅ Read (All for a user)
        public async Task<IEnumerable<IVADeclaration>> GetAllByUserAsync(Guid userId, CancellationToken cancellationToken)
        {
            return await _repository.GetAllByUserAsync(userId, cancellationToken);
        }

        // ✅ Read (By ID and User)
        public async Task<IVADeclaration> GetByIdAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var declaration = await _repository.GetByIdAsync(id, cancellationToken);
            return declaration != null && declaration.UserId == userId ? declaration : null;
        }

        public async Task<int> AddManyAsync(List<IVADeclaration> tax, Guid userId, CancellationToken cancellationToken)
        {
            return await _repository.AddManyAsync(tax, cancellationToken);
        }


        // ✅ Update
        public async Task<IVADeclaration> UpdateAsync(Guid id, Guid userId, IVADeclaration declaration, CancellationToken cancellationToken)
        {
            var existingDeclaration = await _repository.GetByIdAsync(id, cancellationToken);
            if (existingDeclaration == null || existingDeclaration.UserId != userId) return null;

            existingDeclaration.Tipo = declaration.Tipo;
            existingDeclaration.Situacao = declaration.Situacao;
            existingDeclaration.IndicadorPagamentoReembolso = declaration.IndicadorPagamentoReembolso;
            existingDeclaration.BaseTributavelCentimos = declaration.BaseTributavelCentimos;
            existingDeclaration.ImpostoLiquidadoCentimos = declaration.ImpostoLiquidadoCentimos;
            existingDeclaration.ImpostoDedutivelCentimos = declaration.ImpostoDedutivelCentimos;
            existingDeclaration.Valor1 = declaration.Valor1;
            existingDeclaration.Valor2 = declaration.Valor2;
            existingDeclaration.Periodo = declaration.Periodo;
            existingDeclaration.DataRececao = declaration.DataRececao;
            existingDeclaration.NumeroDeclaracao = declaration.NumeroDeclaracao;

            return await _repository.UpdateAsync(existingDeclaration, cancellationToken);
        }

        // ✅ Delete
        public async Task<bool> RemoveAsync(Guid id, Guid userId, CancellationToken cancellationToken)
        {
            var declaration = await _repository.GetByIdAsync(id, cancellationToken);
            if (declaration == null || declaration.UserId != userId) return false;

            return await _repository.RemoveAsync(declaration, cancellationToken);
        }
    }
}
