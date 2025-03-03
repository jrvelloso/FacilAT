using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturasHandler.Data.Models
{
    public class IVADeclaration
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid UserId { get; set; } // Foreign Key to UserData

        [ForeignKey("UserId")]
        public virtual UserData User { get; set; }

        [Required]
        [MaxLength(50)]
        public string Tipo { get; set; }

        [Required]
        [MaxLength(50)]
        public string Situacao { get; set; }

        [Required]
        [MaxLength(1)]
        public string IndicadorPagamentoReembolso { get; set; }

        public long? BaseTributavelCentimos { get; set; }
        public long? ImpostoLiquidadoCentimos { get; set; }
        public long? ImpostoDedutivelCentimos { get; set; }
        public long? Valor1 { get; set; }
        public long? Valor2 { get; set; }

        [Required]
        [MaxLength(6)]
        public string Periodo { get; set; }

        public DateTime DataRececao { get; set; }

        [Required]
        [MaxLength(20)]
        public string NumeroDeclaracao { get; set; }
    }
}
