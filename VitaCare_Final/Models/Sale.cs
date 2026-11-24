using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Pharmacy_Management_System.Models
{
    public class Sale
    {
        public int Id { get; set; }

        public DateTime SaleDate { get; set; } = DateTime.Now;

        [StringLength(120)]
        public string? CustomerName { get; set; }

        // لو فاتورة فيها أدوية تحتاج روشتة
        public bool HasPrescription { get; set; }

        [StringLength(250)]
        public string? PrescriptionNotes { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal TotalAmount { get; set; }

        // ربط الفاتورة بالصيدلي/المستخدم اللي نفذها (IdentityUser Id)
        public string? PharmacistId { get; set; }

        public ICollection<SaleDetail> SaleDetails { get; set; } = new List<SaleDetail>();
    }
}