using System;
using System.Text;
using Microsoft.EntityFrameworkCore;

namespace Real_estate;

class Program
{
    static async Task Main(string[] args)
    {
        Console.OutputEncoding = Encoding.UTF8;

        var options = new DbContextOptionsBuilder<AppDbContext>()
            .UseSqlite("Data Source=realestate.db")
            .Options;

        using (var context = new AppDbContext(options))
        {
            await context.Database.EnsureDeletedAsync();
            await context.Database.EnsureCreatedAsync();
        }

        using var dbContext = new AppDbContext(options);

        var userService = new UserService(dbContext);
        var propertyService = new PropertyService(dbContext);
        var bookingService = new BookingService(dbContext);

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("=== Real Estate Booking System Started ===\n");
        Console.ResetColor();

    
        var usersData = new[]
        {
            ("Eslam Mohamed", "eslam@gmail.com"),
            ("Marawan gomaa", "marawan@gmail.com"),
            ("Hana wael", "hana@gmail.com"),
            ("Abdelra7man Ayman", "abdoayman@gmail.com"),
            ("Mariam Mohamed", "mariam@gmail.com")
        };

        var createdUsers = new List<User>();
        foreach (var (name, email) in usersData)
        {
            var user = new User { Name = name, Email = email };
            await userService.AddUserAsync(user);
            createdUsers.Add(user);
        }

        
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("--- Users List ---");
        var users = await userService.GetAllUsersAsync();
        foreach (var u in users)
        {
            Console.WriteLine($"ID: {u.Id} | Name: {u.Name,-15} | Email: {u.Email}");
        }
        Console.ResetColor();

        
        var propertiesData = new[]
        {
            ("Luxury Apartment in Zagazig", 750000m, "El-Qawmia Street", createdUsers[0].Id),
            ("Spacious Villa with Garden", 3200000m, "El-Nasr Street", createdUsers[0].Id),
            ("Modern Studio near Station", 400000m, "Station Square", createdUsers[1].Id),
            ("Duplex in Tahrir St", 1800000m, "Tahrir Street", createdUsers[2].Id),
            ("Cozy Chalet", 950000m, "Corniche Road", createdUsers[3].Id)
        };

        var createdProperties = new List<Property>();
        foreach (var (title, price, address, userId) in propertiesData)
        {
            var prop = new Property
            {
                Title = title,
                Price = price,
                Address = address,
                UserId = userId
            };
            await propertyService.AddPropertyAsync(prop);
            createdProperties.Add(prop);
        }

        
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine("\n--- Properties Available for Booking ---");
        var availableProps = await propertyService.GetAvailablePropertiesAsync();
        foreach (var p in availableProps)
        {
            Console.WriteLine($"ID: {p.Id} | Title: {p.Title,-30} | Price: ${p.Price:N2} | Address: {p.Address}");
        }
        Console.ResetColor();

        
        await bookingService.AddBookingAsync(createdUsers[1].Id, createdProperties[0].Id); // Mahmoud يحجز شقة الزقازيق
        await bookingService.AddBookingAsync(createdUsers[2].Id, createdProperties[2].Id); // Sara تحجز الاستوديو
        await bookingService.AddBookingAsync(createdUsers[4].Id, createdProperties[3].Id); // Nourhan تحجز الدوبلكس

        
        Console.ForegroundColor = ConsoleColor.DarkYellow;
        Console.WriteLine("\n--- Properties Available After Booking ---");
        var remainingProps = await propertyService.GetAvailablePropertiesAsync();
        foreach (var p in remainingProps)
        {
            Console.WriteLine($"ID: {p.Id} | Title: {p.Title,-30} | Price: ${p.Price:N2} | Address: {p.Address}");
        }
        Console.ResetColor();

        
        Console.ForegroundColor = ConsoleColor.Magenta;
        Console.WriteLine("\n--- Booking List ---");
        var bookings = await bookingService.GetAllBookingsAsync();
        foreach (var b in bookings)
        {
            Console.WriteLine($"Booking ID: {b.Id} | User: {b.User?.Name,-15} | Property: {b.Property?.Title,-30} | Date: {b.BookingDate:yyyy-MM-dd HH:mm}");
        }
        Console.ResetColor();

        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine("\n=== Program Ended Successfully ===");
        Console.ResetColor();
    }
}
