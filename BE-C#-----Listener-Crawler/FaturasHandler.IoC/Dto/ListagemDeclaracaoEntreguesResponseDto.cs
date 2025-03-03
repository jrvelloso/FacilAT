using Newtonsoft.Json;

namespace FaturasHandler.IoC.Dto
{
    public class ListagemDeclaracaoEntreguesResponseDto
    {
        [JsonProperty("declaracoes")]
        public List<ListagemDeclaracaoEntreguesDto> Declaracoes { get; set; }

        [JsonProperty("temMaisDeclaracoes")]
        public bool TemMaisDeclaracoes { get; set; }

        [JsonProperty("nif")]
        public string Nif { get; set; }

        [JsonProperty("ano")]
        public int Ano { get; set; }
    }

    public class ListagemDeclaracaoEntreguesDto
    {
        [JsonProperty("tipo")]
        public string Tipo { get; set; }

        [JsonProperty("situacao")]
        public string Situacao { get; set; }

        [JsonProperty("indicadorPagamentoReembolso")]
        public string IndicadorPagamentoReembolso { get; set; }

        [JsonProperty("baseTributavelCentimos")]
        public long BaseTributavelCentimos { get; set; }

        [JsonProperty("impostoLiquidadoCentimos")]
        public long ImpostoLiquidadoCentimos { get; set; }

        [JsonProperty("impostoDedutivelCentimos")]
        public long ImpostoDedutivelCentimos { get; set; }

        [JsonProperty("valor1")]
        public long Valor1 { get; set; }

        [JsonProperty("valor2")]
        public long Valor2 { get; set; }

        [JsonProperty("periodo")]
        public string Periodo { get; set; }

        [JsonProperty("dataRececao")]
        public string DataRececao { get; set; }

        [JsonProperty("numeroDeclaracao")]
        public string NumeroDeclaracao { get; set; }
    }
}