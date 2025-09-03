using System.ComponentModel.DataAnnotations;

namespace WeddingApp.Data.Auditing;

public class AuditLog
{
    public Guid Id { get; set; } = Guid.NewGuid();

    [Required] [StringLength(100)] public string EntityName { get; set; } = string.Empty;

    [Required] public string ChangeData { get; set; } = string.Empty;

    public DateTime CreatedDateTime { get; set; }

    public string? CreatedByUserId { get; set; }
}