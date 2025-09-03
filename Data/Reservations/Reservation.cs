using System.ComponentModel.DataAnnotations;

namespace WeddingApp.Data.Reservations;

// Entity Classes
public class Reservation : BaseEntity
{
    [Required] [StringLength(10)] public string RegistrationToken { get; set; } = GenerateToken();

    public ReservationStatus Status { get; set; } = ReservationStatus.Pending;

    public int MaxGuests { get; set; } = 5;

    // Navigation
    public virtual ICollection<ReservationPerson> People { get; set; } = new List<ReservationPerson>();

    private static string GenerateToken()
    {
        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
        var random = new Random();
        return new string(Enumerable.Repeat(chars, 8)
            .Select(s => s[random.Next(s.Length)]).ToArray());
    }
}

public enum ReservationStatus
{
    NotAttending,
    Pending,
    Confirmed
}