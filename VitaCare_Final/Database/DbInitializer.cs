using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Smart_Pharmacy_Management_System.Models;

namespace Smart_Pharmacy_Management_System.Data
{
    public static class DbInitializer
    {
        public static async Task SeedAsync(IServiceProvider serviceProvider)
        {
            var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
            var userManager = serviceProvider.GetRequiredService<UserManager<IdentityUser>>();
            var context = serviceProvider.GetRequiredService<ApplicationDbContext>();

            // 1. إنشاء الصلاحيات (Roles)
            string[] roles = { "Admin", "Pharmacist", "Cashier" };
            foreach (var role in roles)
            {
                if (!await roleManager.RoleExistsAsync(role))
                {
                    await roleManager.CreateAsync(new IdentityRole(role));
                }
            }

            // 2. إنشاء حساب Admin افتراضي
            var adminEmail = "admin@pharmacy.com";
            var adminUser = await userManager.FindByEmailAsync(adminEmail);
            if (adminUser == null)
            {
                adminUser = new IdentityUser
                {
                    UserName = adminEmail,
                    Email = adminEmail,
                    EmailConfirmed = true
                };

                var createResult = await userManager.CreateAsync(adminUser, "Admin@123");
                if (createResult.Succeeded)
                {
                    await userManager.AddToRoleAsync(adminUser, "Admin");
                }
            }

            // 3. التصنيفات الطبية (10 تصنيفات)
            if (!await context.Categories.AnyAsync())
            {
                var categories = new List<Category>
                {
                    new() { Name = "مسكنات وخافض حرارة", Description = "تسكين الآلام وخفض الحرارة" },
                    new() { Name = "مضادات حيوية", Description = "علاج العدوى البكتيرية" },
                    new() { Name = "فيتامينات ومكملات غذائية", Description = "دعم المناعة والفيتامينات والمعادن" },
                    new() { Name = "الجهاز الهضمي والمعدة", Description = "الحموضة، القولون، والهضم" },
                    new() { Name = "الجهاز التنفسي والبرد", Description = "الكحة، الاحتقان، والحساسية" },
                    new() { Name = "القلب والضغط والأوعية", Description = "تنظيم الضغط والسيولة والقلب" },
                    new() { Name = "السكري والغدد", Description = "ضبط سكر الدم والغدة الدرقية" },
                    new() { Name = "العظام والمفاصل والعضلات", Description = "التهابات المفاصل والآلام العضلية" },
                    new() { Name = "الجلدية والعناية الموضعية", Description = "كريمات ومراهم ومضادات الفطريات" },
                    new() { Name = "العيون والأذن والأنف", Description = "قطرات وبخاخات موضعية" }
                };

                context.Categories.AddRange(categories);
                await context.SaveChangesAsync();
            }

            // 4. الموردون (6 موردين)
            if (!await context.Suppliers.AnyAsync())
            {
                var suppliers = new List<Supplier>
                {
                    new() { Name = "دلتا فارما للأدوية", Phone = "01010000001", Email = "sales@deltapharma.eg", Address = "العاشر من رمضان - المنطقة الصناعية" },
                    new() { Name = "المصرية الدولية للمستحضرات", Phone = "01010000002", Email = "orders@eipyco.eg", Address = "مدينة نصر - القاهرة" },
                    new() { Name = "سينا فارما للتوزيع الطبي", Phone = "01010000003", Email = "supply@sinapharma.eg", Address = "العبور - القليوبية" },
                    new() { Name = "إيفا فارما للرعاية الصحية", Phone = "01010000004", Email = "b2b@evapharma.eg", Address = "الجيزة - المنطقة الصناعية" },
                    new() { Name = "مالتي كير للمكملات والفيتامينات", Phone = "01010000005", Email = "info@multicare.eg", Address = "الإسكندرية - سموحة" },
                    new() { Name = "الشرق الأوسط للمستلزمات الطبية", Phone = "01010000006", Email = "care@memsupplies.eg", Address = "الزقازيق - الشرقية" }
                };

                context.Suppliers.AddRange(suppliers);
                await context.SaveChangesAsync();
            }

            // 5. توليد 100 دواء متنوع + ربط التشغيلات (MedicineBatches)
            if (await context.Medicines.CountAsync() < 100)
            {
                var categoryIds = await context.Categories.OrderBy(c => c.Id).Select(c => c.Id).ToListAsync();
                var supplierIds = await context.Suppliers.OrderBy(s => s.Id).Select(s => s.Id).ToListAsync();

                var baseCatalog = new (string Name, string Ingredient, decimal Price, bool Rx)[]
                {
                    ("Panadol Extra", "Paracetamol + Caffeine", 45.00m, false),
                    ("Augmentin 1g", "Amoxicillin + Clavulanic Acid", 145.00m, true),
                    ("Brufen 600mg", "Ibuprofen", 65.00m, false),
                    ("Cataflam 50mg", "Diclofenac Potassium", 72.00m, false),
                    ("Zithromax 500mg", "Azithromycin", 160.00m, true),
                    ("Ciprofar 500mg", "Ciprofloxacin", 95.00m, true),
                    ("Vitamin C 1000mg", "Ascorbic Acid", 85.00m, false),
                    ("Vitamin D3 5000 IU", "Cholecalciferol", 120.00m, false),
                    ("Omega-3 Plus", "Fish Oil + EPA/DHA", 210.00m, false),
                    ("Nexium 40mg", "Esomeprazole", 175.00m, true),
                    ("Controloc 40mg", "Pantoprazole", 150.00m, true),
                    ("Gaviscon Advance", "Sodium Alginate + Potassium Bicarbonate", 130.00m, false),
                    ("Antinal 200mg", "Nifuroxazide", 42.00m, false),
                    ("Spasmomen", "Otilonium Bromide", 115.00m, true),
                    ("Telfast 180mg", "Fexofenadine Hydrochloride", 140.00m, false),
                    ("Claritin 10mg", "Loratadine", 90.00m, false),
                    ("Sinupret Forte", "Gentian Root + Primula Flower", 155.00m, false),
                    ("Mucotec Syrup", "Carbocisteine", 52.00m, false),
                    ("Concor 5mg", "Bisoprolol Fumarate", 98.00m, true),
                    ("Coversyl 5mg", "Perindopril Arginine", 165.00m, true),
                    ("Lipitor 20mg", "Atorvastatin", 190.00m, true),
                    ("Plavix 75mg", "Clopidogrel", 220.00m, true),
                    ("Glucophage XR 1000mg", "Metformin Hydrochloride", 88.00m, true),
                    ("Amaryl 3mg", "Glimepiride", 76.00m, true),
                    ("Euthyrox 50mcg", "Levothyroxine Sodium", 68.00m, true),
                    ("Arcoxia 90mg", "Etoricoxib", 185.00m, true),
                    ("Muscadol", "Paracetamol + Orphenadrine Citrate", 58.00m, false),
                    ("Myolgin", "Paracetamol + Chlorzoxazone", 54.00m, false),
                    ("Fucidin Cream", "Fusidic Acid", 82.00m, false),
                    ("Canesten Cream", "Clotrimazole", 64.00m, false),
                    ("Dermovate Cream", "Clobetasol Propionate", 70.00m, true),
                    ("Refresh Tears Eye Drops", "Carboxymethylcellulose", 110.00m, false),
                    ("Otrivin Nasal Spray", "Xylometazoline Hydrochloride", 60.00m, false),
                    ("Bepanthen Ointment", "Dexpanthenol", 125.00m, false),
                    ("Voltaren Emulgel", "Diclofenac Diethylamine", 105.00m, false)
                };

                var existingCount = await context.Medicines.CountAsync();
                var targetToAdd = 100 - existingCount;
                var newMedicines = new List<Medicine>();
                var today = DateTime.Today;

                for (int i = 0; i < targetToAdd; i++)
                {
                    int index = existingCount + i;
                    var template = baseCatalog[index % baseCatalog.Length];
                    int batchNum = (index / baseCatalog.Length) + 1;

                    string displayName = batchNum == 1
                        ? template.Name
                        : $"{template.Name} - Batch #{batchNum}";

                    int stock = (index % 5) switch
                    {
                        0 => index % 3 == 0 ? 0 : 4 + (index % 7),
                        1 => 12 + (index % 18),
                        2 => 35 + (index % 45),
                        3 => 80 + (index % 60),
                        _ => 6 + (index % 15)
                    };

                    DateTime expiry = (index % 7) switch
                    {
                        0 => today.AddDays(12 + (index % 15)),
                        1 => today.AddMonths(2 + (index % 4)),
                        _ => today.AddMonths(8 + (index % 18))
                    };

                    bool requiresRx = template.Rx || (index % 4 == 0);
                    decimal price = Math.Round(template.Price + ((index % 6) * 3.50m), 2);
                    int supplierId = supplierIds[index % supplierIds.Count];

                    var medicine = new Medicine
                    {
                        Name = displayName,
                        Barcode = $"622{100000 + index}",
                        ActiveIngredient = template.Ingredient,
                        Price = price,
                        StockQuantity = stock,
                        ReorderLevel = 10,
                        ExpiryDate = expiry,
                        RequiresPrescription = requiresRx,
                        CategoryId = categoryIds[index % categoryIds.Count],
                        SupplierId = supplierId
                    };

                    // تقسيم الرصيد لتشغيلتين لخدمة منطق FEFO والتنبيهات
                    if (stock > 0)
                    {
                        int nearQty = stock > 4 ? stock / 3 : stock;
                        int farQty = stock - nearQty;

                        medicine.Batches.Add(new MedicineBatch
                        {
                            BatchNumber = $"B-{1000 + index}-A",
                            ExpiryDate = today.AddDays(10 + (index % 20)),
                            Quantity = nearQty,
                            UnitCost = Math.Round(price * 0.72m, 2),
                            SupplierId = supplierId
                        });

                        if (farQty > 0)
                        {
                            medicine.Batches.Add(new MedicineBatch
                            {
                                BatchNumber = $"B-{1000 + index}-B",
                                ExpiryDate = expiry.AddMonths(6),
                                Quantity = farQty,
                                UnitCost = Math.Round(price * 0.75m, 2),
                                SupplierId = supplierId
                            });
                        }
                    }

                    newMedicines.Add(medicine);
                }

                context.Medicines.AddRange(newMedicines);
                await context.SaveChangesAsync();
            }

            // 6. التداخلات الدوائية (DrugInteractions)
            if (!await context.DrugInteractions.AnyAsync())
            {
                context.DrugInteractions.AddRange(
                    new DrugInteraction
                    {
                        ActiveIngredientA = "Ibuprofen",
                        ActiveIngredientB = "Diclofenac Potassium",
                        Severity = "Major",
                        Description = "الجمع بين مضادين للالتهاب غير الستيرويدي يزيد خطر قرح المعدة والنزيف."
                    },
                    new DrugInteraction
                    {
                        ActiveIngredientA = "Atorvastatin",
                        ActiveIngredientB = "Clopidogrel",
                        Severity = "Moderate",
                        Description = "قد يؤثر على فعالية بعض الأدوية؛ يُفضل مراجعة الطبيب أو الصيدلي."
                    },
                    new DrugInteraction
                    {
                        ActiveIngredientA = "Glimepiride",
                        ActiveIngredientB = "Ciprofloxacin",
                        Severity = "Moderate",
                        Description = "قد يسبب تقلبات في مستوى سكر الدم؛ راقب السكر بانتظام."
                    }
                );
                await context.SaveChangesAsync();
            }

            // 7. فاتورة شراء ومبيعات مبدئية مربوطة بالتشغيلات والأدوية
            if (!await context.PurchaseInvoices.AnyAsync() && !await context.Sales.AnyAsync())
            {
                var firstSupplierId = await context.Suppliers.Select(s => s.Id).FirstAsync();
                var sampleMeds = await context.Medicines
                    .Include(m => m.Batches)
                    .Where(m => m.StockQuantity >= 5)
                    .Take(3)
                    .ToListAsync();

                if (sampleMeds.Count >= 2)
                {
                    var purchase = new PurchaseInvoice
                    {
                        InvoiceNumber = "PO-2026-001",
                        InvoiceDate = DateTime.Now.AddDays(-5),
                        SupplierId = firstSupplierId,
                        Notes = "فاتورة توريد مبدئية للتجربة والاختبار",
                        Items = sampleMeds.Select((m, idx) => new PurchaseInvoiceItem
                        {
                            MedicineId = m.Id,
                            BatchNumber = $"POB-{200 + idx}",
                            ExpiryDate = DateTime.Today.AddMonths(12),
                            Quantity = 10,
                            UnitCost = Math.Round(m.Price * 0.70m, 2)
                        }).ToList()
                    };
                    purchase.TotalAmount = purchase.Items.Sum(i => i.Quantity * i.UnitCost);
                    context.PurchaseInvoices.Add(purchase);

                    var sale = new Sale
                    {
                        SaleDate = DateTime.Now.AddDays(-1),
                        CustomerName = "عميل تجريبي",
                        HasPrescription = sampleMeds.Any(m => m.RequiresPrescription),
                        PrescriptionNotes = "فاتورة مبيعات مبدئية للمرتجعات والتقارير",
                        PharmacistId = adminUser?.Id,
                        SaleDetails = sampleMeds.Take(2).Select(m => new SaleDetail
                        {
                            MedicineId = m.Id,
                            MedicineBatchId = m.Batches.FirstOrDefault()?.Id,
                            Quantity = 2,
                            UnitPrice = m.Price
                        }).ToList()
                    };
                    sale.TotalAmount = sale.SaleDetails.Sum(d => d.Quantity * d.UnitPrice);
                    context.Sales.Add(sale);

                    await context.SaveChangesAsync();
                }
            }
        }
    }
}