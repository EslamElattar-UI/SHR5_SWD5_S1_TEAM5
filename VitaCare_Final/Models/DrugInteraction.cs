using System.ComponentModel.DataAnnotations;

namespace Smart_Pharmacy_Management_System.Models
{
    public class DrugInteraction
    {
        public int Id { get; set; }

        [Required]
        [StringLength(150)]
        public string ActiveIngredientA { get; set; } = string.Empty;

        [Required]
        [StringLength(150)]
        public string ActiveIngredientB { get; set; } = string.Empty;

        [Required]
        [StringLength(20)]
        public string Severity { get; set; } = "Moderate"; // Minor, Moderate, Major

        [Required]
        [StringLength(500)]
        public string Description { get; set; } = string.Empty;
    }

    public class AuditLog
    {
        public int Id { get; set; }

        public string? UserId { get; set; }

        [StringLength(150)]
        public string? UserEmail { get; set; }

        [Required]
        [StringLength(100)]
        public string Action { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string EntityName { get; set; } = string.Empty;

        [StringLength(50)]
        public string? EntityId { get; set; }

        [StringLength(500)]
        public string? Details { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;
    }
}