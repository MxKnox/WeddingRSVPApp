using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WeddingApp.Data.Schedule;

public class Location : BaseEntity
{
    [Required] [StringLength(100)] public string Name { get; set; } = string.Empty;

    [StringLength(200)] public string? ContactDetails { get; set; }

    [Url] [StringLength(200)] public string? Website { get; set; }

    [StringLength(300)] public string? Address { get; set; }

    // Navigation
    [JsonIgnore]
    public virtual ICollection<ScheduleEntry>? ScheduleEntries { get; set; } = new List<ScheduleEntry>();
}