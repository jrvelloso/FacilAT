using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using FaturasHandler.Data.Models;

namespace FaturasHandler.IoC.Dto
{
    public class UserActionDto // ✅ Changed from Action to UserAction
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public DateTime RequestedAt { get; set; } = DateTime.UtcNow;

        [Required]
        public bool Active { get; set; } = true; // Default to true

        [Required]
        public Guid UserId { get; set; } // Foreign Key

        [ForeignKey("UserId")]
        public virtual UserData User { get; set; } // ✅ Fixed reference
    }
}
