using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace FaturasHandler.Data.Models
{
    public class ReciboVerde
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        public Guid UserId { get; set; } // Foreign Key to UserData

        [ForeignKey("UserId")]
        public virtual UserData User { get; set; }

        [Required]
        [MaxLength(30)]
        public string NumeroUnico { get; set; }

        public int NumDocumento { get; set; }

        [Required]
        [MaxLength(20)]
        public string Situacao { get; set; }

        [Required]
        [MaxLength(20)]
        public string TipoDocumento { get; set; }

        public DateTime DataEmissao { get; set; }
        public decimal ValorBase { get; set; }
        public decimal ValorIVA { get; set; }
        public decimal ValorIRS { get; set; }
        public decimal ValorTotalCImpostos { get; set; }
        public decimal ImportanciaRecebida { get; set; }
    }
}
