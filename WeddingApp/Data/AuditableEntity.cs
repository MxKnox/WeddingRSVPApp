namespace WeddingApp.Data;

// Base auditable interface
public interface IAuditable
{
    Guid Id { get; set; }
    string? CreatedByUserId { get; set; }
    DateTime CreatedDateTime { get; set; }
    string? ModifiedByUserId { get; set; }
    DateTime ModifiedDateTime { get; set; }
}

// Base entity class
public abstract class BaseEntity : IAuditable
{
    public Guid Id { get; set; } = Guid.NewGuid();
    public string? CreatedByUserId { get; set; }
    public DateTime CreatedDateTime { get; set; } = DateTime.Now;
    public string? ModifiedByUserId { get; set; }
    public DateTime ModifiedDateTime { get; set; }
}