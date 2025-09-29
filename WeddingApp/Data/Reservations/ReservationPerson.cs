using System.ComponentModel.DataAnnotations;
using WeddingApp.Identity;

namespace WeddingApp.Data.Reservations;

public class ReservationPerson : BaseEntity
{
    [Required] public Guid ReservationId { get; set; }
    public string? UserId { get; set; }

    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;
    
    [StringLength(100)] public string PreferredName { get; set; } = string.Empty;
    [EmailAddress] [StringLength(100)] public string? Email { get; set; }

    [Phone] [StringLength(20)] public string? Phone { get; set; }

    [StringLength(200)] public string? Address { get; set; }

    
    public bool Over18 { get; set; } = false;
    public bool DietaryRequirements { get; set; } = false;
    [StringLength(500)] public string? DietaryRequirementsDescription { get; set; }

    public bool IsPrimaryGuest { get; set; }

    // Navigation
    public virtual Reservation? Reservation { get; set; }
    public virtual WeddingAppUser? User { get; set; }
}