using System.ComponentModel.DataAnnotations.Schema;

namespace Smart_Pharmacy_Management_System.Models
{
    public class SaleDetail
    {
        public int Id { get; set; }

        public int SaleId { get; set; }
        public Sale? Sale { get; set; }

        public int MedicineId { get; set; }
        public Medicine? Medicine { get; set; }

        public int? MedicineBatchId { get; set; }
        public MedicineBatch? MedicineBatch { get; set; }

        public int Quantity { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal UnitPrice { get; set; }
    }
}