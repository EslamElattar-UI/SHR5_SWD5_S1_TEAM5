using System.ComponentModel.DataAnnotations;

namespace Smart_Pharmacy_Management_System.Models
{
    public class Category
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Category name is required")]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [StringLength(250)]
        public string? Description { get; set; }

        // Navigation Property: One Category -> Many Medicines
        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
    }
}