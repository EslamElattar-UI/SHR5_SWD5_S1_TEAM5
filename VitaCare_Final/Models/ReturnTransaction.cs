using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Pharmacy_Management_System.Models
{
    public class ReturnTransaction
    {
        public int Id { get; set; }

        [Required]
        [StringLength(20)]
        public string ReturnType { get; set; } = "SaleReturn"; // "SaleReturn" or "PurchaseReturn"

        public int? SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int? PurchaseInvoiceId { get; set; }
        public PurchaseInvoice? PurchaseInvoice { get; set; }

        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        public int? MedicineBatchId { get; set; }
        public MedicineBatch? MedicineBatch { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal RefundAmount { get; set; }

        [StringLength(250)]
        public string? Reason { get; set; }

        public DateTime ReturnDate { get; set; } = DateTime.Now;

        public string? ProcessedByUserId { get; set; }
    }
}