using System.ComponentModel.DataAnnotations;

namespace Smart_Pharmacy_Management_System.Models
{
    public class Supplier
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Supplier name is required")]
        [StringLength(120)]
        public string Name { get; set; } = string.Empty;

        [Phone]
        [StringLength(20)]
        public string? Phone { get; set; }

        [EmailAddress]
        [StringLength(120)]
        public string? Email { get; set; }

        [StringLength(250)]
        public string? Address { get; set; }

        public ICollection<Medicine> Medicines { get; set; } = new List<Medicine>();
        public ICollection<MedicineBatch> Batches { get; set; } = new List<MedicineBatch>();
        public ICollection<PurchaseInvoice> PurchaseInvoices { get; set; } = new List<PurchaseInvoice>();
    }
}