using FaturasHandler.Data.Models;
using FaturasHandler.IoC.Dto;

namespace FaturasHandler.IoC.Mapper
{
    public static class IVADeclarationMapper
    {
        public static List<IVADeclaration> ToManyEntities(IEnumerable<ListagemDeclaracaoEntreguesDto> dtos, Guid userId)
        {
            return dtos?.Select(dto => ToEntity(dto, userId)).ToList() ?? new List<IVADeclaration>();
        }

        public static IVADeclaration ToEntity(ListagemDeclaracaoEntreguesDto dto, Guid userId)
        {
            return new IVADeclaration
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Tipo = dto.Tipo,
                Situacao = dto.Situacao,
                IndicadorPagamentoReembolso = dto.IndicadorPagamentoReembolso,
                BaseTributavelCentimos = dto.BaseTributavelCentimos,
                ImpostoLiquidadoCentimos = dto.ImpostoLiquidadoCentimos,
                ImpostoDedutivelCentimos = dto.ImpostoDedutivelCentimos,
                Valor1 = dto.Valor1,
                Valor2 = dto.Valor2,
                Periodo = dto.Periodo,
                DataRececao = DateTime.TryParse(dto.DataRececao, out var date) ? date : default,
                NumeroDeclaracao = dto.NumeroDeclaracao
            };
        }

        public static ListagemDeclaracaoEntreguesResponseDto ToDto(IEnumerable<IVADeclaration> entities, string nif, int ano, bool temMaisDeclaracoes)
        {
            return new ListagemDeclaracaoEntreguesResponseDto
            {
                Nif = nif,
                Ano = ano,
                TemMaisDeclaracoes = temMaisDeclaracoes,
                Declaracoes = entities?.Select(ToDto).ToList() ?? new List<ListagemDeclaracaoEntreguesDto>()
            };
        }

        public static ListagemDeclaracaoEntreguesDto ToDto(IVADeclaration entity)
        {
            return new ListagemDeclaracaoEntreguesDto
            {
                Tipo = entity.Tipo,
                Situacao = entity.Situacao,
                IndicadorPagamentoReembolso = entity.IndicadorPagamentoReembolso,
                BaseTributavelCentimos = entity.BaseTributavelCentimos ?? 0,
                ImpostoLiquidadoCentimos = entity.ImpostoLiquidadoCentimos ?? 0,
                ImpostoDedutivelCentimos = entity.ImpostoDedutivelCentimos ?? 0,
                Valor1 = entity.Valor1 ?? 0,
                Valor2 = entity.Valor2 ?? 0,
                Periodo = entity.Periodo,
                DataRececao = entity.DataRececao.ToString("yyyy-MM-dd"),
                NumeroDeclaracao = entity.NumeroDeclaracao
            };
        }
    }
}
