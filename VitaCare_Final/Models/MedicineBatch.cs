using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Pharmacy_Management_System.Models
{
    public class MedicineBatch
    {
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string BatchNumber { get; set; } = string.Empty;

        [DataType(DataType.Date)]
        public DateTime ExpiryDate { get; set; }

        [Range(0, 1000000)]
        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitCost { get; set; }

        public DateTime ReceivedDate { get; set; } = DateTime.Now;

        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        public int? SupplierId { get; set; }
        public Supplier? Supplier { get; set; }
    }
}