using System.ComponentModel.DataAnnotations;

namespace FaturasHandler.Data.Models
{
    public class UserData
    {
        [Key]
        public Guid Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string Name { get; set; }

        [Required]
        [MaxLength(100)]
        public string Email { get; set; }

        [Required]
        [MaxLength(10)]
        public string NIF { get; set; }

        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
}
