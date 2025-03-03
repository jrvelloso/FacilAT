using System.Text.Json.Serialization;

namespace FaturasHandler.IoC.Dto
{
    public class RecibosVerdesDto
    {
        [JsonPropertyName("success")]
        public bool Success { get; set; }

        [JsonPropertyName("listaDocumentos")]
        public List<RecibosVerdesDocumentsDto> ListaDocumentos { get; set; }

        [JsonPropertyName("totalDocs")]
        public int TotalDocs { get; set; }
    }

    public class RecibosVerdesDocumentsDto
    {
        [JsonPropertyName("numeroUnico")]
        public string NumeroUnico { get; set; }

        [JsonPropertyName("numDocumento")]
        public int NumDocumento { get; set; }

        [JsonPropertyName("situacao")]
        public string Situacao { get; set; }

        [JsonPropertyName("tipoDocumento")]
        public string TipoDocumento { get; set; }

        [JsonPropertyName("dataEmissao")]
        public string DataEmissao { get; set; }

        [JsonPropertyName("valorBase")]
        public decimal? ValorBase { get; set; }

        [JsonPropertyName("valorIVA")]
        public decimal? ValorIVA { get; set; }

        [JsonPropertyName("valorIRS")]
        public decimal? ValorIRS { get; set; }

        [JsonPropertyName("valorTotalCImpostos")]
        public decimal? ValorTotalCImpostos { get; set; }

        [JsonPropertyName("importanciaRecebida")]
        public decimal? ImportanciaRecebida { get; set; }
    }
}
