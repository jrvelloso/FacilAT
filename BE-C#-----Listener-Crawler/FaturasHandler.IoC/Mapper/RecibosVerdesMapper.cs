using FaturasHandler.Data.Models;
using FaturasHandler.IoC.Dto;

namespace FaturasHandler.IoC.Mapper

{
    public static class RecibosVerdesMapper
    {
        public static List<ReciboVerde> ToManyEntities(List<RecibosVerdesDto> dtos, Guid userId)
        {
            return dtos.SelectMany(dto => dto.ListaDocumentos?.Select(doc => new ReciboVerde
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                NumeroUnico = doc.NumeroUnico,
                NumDocumento = doc.NumDocumento,
                Situacao = doc.Situacao,
                TipoDocumento = doc.TipoDocumento,
                DataEmissao = DateTime.TryParse(doc.DataEmissao, out var date) ? date : default,
                ValorBase = doc.ValorBase ?? 0,
                ValorIVA = doc.ValorIVA ?? 0,
                ValorIRS = doc.ValorIRS ?? 0,
                ValorTotalCImpostos = doc.ValorTotalCImpostos ?? 0,
                ImportanciaRecebida = doc.ImportanciaRecebida ?? 0
            }) ?? new List<ReciboVerde>()).ToList();
        }

        public static List<ReciboVerde> ToEntities(RecibosVerdesDto dto, Guid userId)
        {
            return dto.ListaDocumentos?.Select(doc => new ReciboVerde
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                NumeroUnico = doc.NumeroUnico,
                NumDocumento = doc.NumDocumento,
                Situacao = doc.Situacao,
                TipoDocumento = doc.TipoDocumento,
                DataEmissao = DateTime.TryParse(doc.DataEmissao, out var date) ? date : default,
                ValorBase = doc.ValorBase ?? 0,
                ValorIVA = doc.ValorIVA ?? 0,
                ValorIRS = doc.ValorIRS ?? 0,
                ValorTotalCImpostos = doc.ValorTotalCImpostos ?? 0,
                ImportanciaRecebida = doc.ImportanciaRecebida ?? 0
            }).ToList() ?? new List<ReciboVerde>();
        }

        public static RecibosVerdesDto ToDto(List<ReciboVerde> entities)
        {
            return new RecibosVerdesDto
            {
                Success = entities.Any(),
                TotalDocs = entities.Count,
                ListaDocumentos = entities.Select(entity => new RecibosVerdesDocumentsDto
                {
                    NumeroUnico = entity.NumeroUnico,
                    NumDocumento = entity.NumDocumento,
                    Situacao = entity.Situacao,
                    TipoDocumento = entity.TipoDocumento,
                    DataEmissao = entity.DataEmissao.ToString("yyyy-MM-dd"),
                    ValorBase = entity.ValorBase,
                    ValorIVA = entity.ValorIVA,
                    ValorIRS = entity.ValorIRS,
                    ValorTotalCImpostos = entity.ValorTotalCImpostos,
                    ImportanciaRecebida = entity.ImportanciaRecebida
                }).ToList()
            };
        }
    }
}
