using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Pharmacy_Management_System.Models
{
    public class Medicine
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Medicine name is required")]
        [StringLength(150)]
        public string Name { get; set; } = string.Empty;

        [StringLength(50)]
        public string? Barcode { get; set; }

        [Required(ErrorMessage = "Active ingredient is required")]
        [StringLength(150)]
        public string ActiveIngredient { get; set; } = string.Empty;

        [Column(TypeName = "decimal(18,2)")]
        [Range(0.01, 1000000)]
        public decimal Price { get; set; }

        [Range(0, 1000000)]
        public int StockQuantity { get; set; }

        public int ReorderLevel { get; set; } = 10;

        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        public bool RequiresPrescription { get; set; }

        [Display(Name = "Category")]
        public int CategoryId { get; set; }
        public Category? Category { get; set; }

        [Display(Name = "Supplier")]
        public int SupplierId { get; set; }
        public Supplier? Supplier { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
        public ICollection<MedicineBatch> Batches { get; set; } = new List<MedicineBatch>();
    }
}