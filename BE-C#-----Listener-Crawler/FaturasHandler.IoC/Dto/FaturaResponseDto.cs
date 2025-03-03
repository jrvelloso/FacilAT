namespace FaturasHandler.IoC.Dto
{
    public class FaturaResponseDto
    {
        public bool Success { get; set; }
        public MessagesDto Messages { get; set; }
        public string DataProcessamento { get; set; }
        public List<FaturaDto> Linhas { get; set; }
        public int NumElementos { get; set; }
        public int TotalElementos { get; set; }
    }

    public class MessagesDto
    {
        public List<string> Error { get; set; }
        public List<string> Success { get; set; }
        public List<string> Info { get; set; }
        public List<string> Warning { get; set; }
    }

    public class FaturaDto
    {
        public long IdDocumento { get; set; }
        public string OrigemRegisto { get; set; }
        public string OrigemRegistoDesc { get; set; }
        public long NifEmitente { get; set; }
        public string NomeEmitente { get; set; }
        public long NifAdquirente { get; set; }
        public string NomeAdquirente { get; set; }
        public string PaisAdquirente { get; set; }
        public string NifAdquirenteInternac { get; set; }
        public string TipoDocumento { get; set; }
        public string TipoDocumentoDesc { get; set; }
        public string NumeroDocumento { get; set; }
        public string HashDocumento { get; set; }
        public string DataEmissaoDocumento { get; set; }
        public decimal ValorTotal { get; set; }
        public decimal ValorTotalBaseTributavel { get; set; }
        public decimal ValorTotalIva { get; set; }
        public decimal? ValorTotalBeneficioProv { get; set; }
        public decimal? ValorTotalSetorBeneficio { get; set; }
        public decimal? ValorTotalDespesasGerais { get; set; }
        public string EstadoBeneficio { get; set; }
        public string EstadoBeneficioDesc { get; set; }
        public string EstadoBeneficioEmitente { get; set; }
        public string EstadoBeneficioDescEmitente { get; set; }
        public bool? ExisteTaxaNormal { get; set; }
        public string ActividadeEmitente { get; set; }
        public string ActividadeEmitenteDesc { get; set; }
        public string ActividadeProf { get; set; }
        public string ActividadeProfDesc { get; set; }
        public bool ComunicacaoComerciante { get; set; }
        public bool ComunicacaoConsumidor { get; set; }
        public bool IsDocumentoEstrangeiro { get; set; }
        public string Atcud { get; set; }
        public bool Autofaturacao { get; set; }

        public string AmbitoDeAtividade { get; set; }
    }

}
