using Microsoft.EntityFrameworkCore;
using RealEstate.ConsoleApp.Data;

var options = new DbContextOptionsBuilder<AppDbContext>()
.UseSqlite("Data Source=realestate.db")
.Options;

Console.WriteLine("1. Create database");
Console.WriteLine("2. Delete database");
Console.Write("Choose an option: ");

string? choice = Console.ReadLine();

using var context = new AppDbContext(options);

if (choice == "1")
{
    bool created = context.Database.EnsureCreated();


Console.WriteLine(
    created
        ? "Database created successfully."
        : "Database already exists.");

    Console.WriteLine(
        $"Database connection: {context.Database.CanConnect()}");


}
else if (choice == "2")
{
    Console.Write("Type DELETE to confirm: ");


string? confirmation = Console.ReadLine();

    if (confirmation == "DELETE")
    {
        bool deleted = context.Database.EnsureDeleted();

        Console.WriteLine(
            deleted
                ? "Database deleted successfully."
                : "Database does not exist.");
    }
    else
    {
        Console.WriteLine("Deletion cancelled.");
    }


}
else
{
    Console.WriteLine("Invalid option.");
}
