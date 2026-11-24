using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace RealEstateBooking.Models;

public class User
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Name { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    // One User -> Many Bookings
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public class Property
{
    [Key]
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Title { get; set; } = string.Empty;

    [Required, MaxLength(250)]
    public string Address { get; set; } = string.Empty;

    public decimal Price { get; set; }

    public bool IsAvailable { get; set; } = true;

    // One Property -> Many Bookings
    public ICollection<Booking> Bookings { get; set; } = new List<Booking>();
}

public class Booking
{
    [Key]
    public int Id { get; set; }

    public DateTime BookingDate { get; set; } = DateTime.Now;

    // Many Bookings -> One User
    [ForeignKey(nameof(User))]
    public int UserId { get; set; }
    public User User { get; set; } = null!;

    // Many Bookings -> One Property
    [ForeignKey(nameof(Property))]
    public int PropertyId { get; set; }
    public Property Property { get; set; } = null!;
}
