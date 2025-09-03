using System.ComponentModel.DataAnnotations;
using WeddingApp.Identity;

namespace WeddingApp.Data.GiftRegistration;

public class GiftRegisterTag: BaseEntity
{
    [Required] public Guid GiftRegistryEntryId { get; set; }

    [Required] public string UserId { get; set; } = string.Empty;

    [StringLength(200)] public string? Comment { get; set; }

    // Navigation
    public virtual GiftRegisterEntry? GiftRegistryEntry { get; set; }
    public virtual WeddingAppUser? User { get; set; }
}