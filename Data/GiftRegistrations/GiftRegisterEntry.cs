using System.ComponentModel.DataAnnotations;

namespace WeddingApp.Data.GiftRegistration;

public class GiftRegisterEntry : BaseEntity
{
    [Required] [StringLength(500)] public string Description { get; set; } = string.Empty;

    public bool IsUserCreated { get; set; }

    // Navigation
    public virtual ICollection<GiftRegisterTag> Tags { get; set; } = new List<GiftRegisterTag>();
}