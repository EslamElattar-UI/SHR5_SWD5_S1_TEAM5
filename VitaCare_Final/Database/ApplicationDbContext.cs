using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Smart_Pharmacy_Management_System.Models;

namespace Smart_Pharmacy_Management_System.Data
{
    public class ApplicationDbContext : IdentityDbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Category> Categories { get; set; }
        public DbSet<Supplier> Suppliers { get; set; }
        public DbSet<Medicine> Medicines { get; set; }
        public DbSet<MedicineBatch> MedicineBatches { get; set; }
        public DbSet<Sale> Sales { get; set; }
        public DbSet<SaleDetail> SaleDetails { get; set; }
        public DbSet<PurchaseInvoice> PurchaseInvoices { get; set; }
        public DbSet<PurchaseInvoiceItem> PurchaseInvoiceItems { get; set; }
        public DbSet<ReturnTransaction> ReturnTransactions { get; set; }
        public DbSet<DrugInteraction> DrugInteractions { get; set; }
        public DbSet<AuditLog> AuditLogs { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);

            // Prevent multiple cascade delete paths in SQL Server
            builder.Entity<MedicineBatch>()
                .HasOne(b => b.Supplier)
                .WithMany(s => s.Batches)
                .HasForeignKey(b => b.SupplierId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SaleDetail>()
                .HasOne(sd => sd.Medicine)
                .WithMany(m => m.SaleDetails)
                .HasForeignKey(sd => sd.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<SaleDetail>()
                .HasOne(sd => sd.MedicineBatch)
                .WithMany()
                .HasForeignKey(sd => sd.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<PurchaseInvoiceItem>()
                .HasOne(pi => pi.Medicine)
                .WithMany()
                .HasForeignKey(pi => pi.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReturnTransaction>()
                .HasOne(rt => rt.Medicine)
                .WithMany()
                .HasForeignKey(rt => rt.MedicineId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.Entity<ReturnTransaction>()
                .HasOne(rt => rt.MedicineBatch)
                .WithMany()
                .HasForeignKey(rt => rt.MedicineBatchId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}