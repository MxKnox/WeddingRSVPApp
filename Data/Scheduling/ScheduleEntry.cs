using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace WeddingApp.Data.Schedule;

public class ScheduleEntry : BaseEntity
{
    [Required] [StringLength(100)] public string Title { get; set; } = string.Empty;

    [Required] [StringLength(500)] public string Description { get; set; } = string.Empty;

    [Required] public DateTime StartTime { get; set; }

    [Required] public DateTime EndTime { get; set; }

    public Guid? LocationId { get; set; }

    // Navigation
    [JsonIgnore]
    public virtual Location? Location { get; set; }
}