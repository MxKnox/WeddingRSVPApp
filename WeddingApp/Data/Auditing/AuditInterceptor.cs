using System.Security.Claims;
using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace WeddingApp.Data.Auditing;

// Audit Interceptor
public class AuditInterceptor : SaveChangesInterceptor
{
    private readonly IHttpContextAccessor _httpContextAccessor;

    public AuditInterceptor(IHttpContextAccessor httpContextAccessor)
    {
        _httpContextAccessor = httpContextAccessor;
    }

    public override InterceptionResult<int> SavingChanges(
        DbContextEventData eventData,
        InterceptionResult<int> result)
    {
        if (eventData.Context != null) HandleAudit(eventData.Context);
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context != null) HandleAudit(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void HandleAudit(DbContext context)
    {
        var userId = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        var now = DateTime.UtcNow;

        // Handle audit fields
        foreach (var entry in context.ChangeTracker.Entries())
            if (entry.Entity is IAuditable auditable)
                switch (entry.State)
                {
                    case EntityState.Added:
                        auditable.CreatedDateTime = now;
                        auditable.CreatedByUserId = userId;
                        auditable.ModifiedDateTime = now;
                        auditable.ModifiedByUserId = userId;
                        break;
                    case EntityState.Modified:
                        auditable.ModifiedDateTime = now;
                        auditable.ModifiedByUserId = userId;
                        // Prevent changing creation audit fields
                        entry.Property(nameof(IAuditable.CreatedDateTime)).IsModified = false;
                        entry.Property(nameof(IAuditable.CreatedByUserId)).IsModified = false;
                        break;
                }

        // Create audit log entries
        var auditEntries = new List<AuditLog>();
        foreach (var entry in context.ChangeTracker.Entries())
        {
            if (entry.Entity is AuditLog ||
                entry.State == EntityState.Unchanged ||
                entry.State == EntityState.Detached)
                continue;

            var changeData = new Dictionary<string, object?>
            {
                ["State"] = entry.State.ToString(),
                ["Entity"] = entry.Entity.GetType().Name,
                ["PrimaryKey"] = entry.Properties
                    .FirstOrDefault(p => p.Metadata.IsPrimaryKey())?.CurrentValue
            };

            if (entry.State == EntityState.Modified)
            {
                var changes = new Dictionary<string, object?>();
                foreach (var property in entry.Properties.Where(p => p.IsModified))
                    changes[property.Metadata.Name] = new
                    {
                        Original = property.OriginalValue,
                        Current = property.CurrentValue
                    };
                changeData["Changes"] = changes;
            }
            else if (entry.State == EntityState.Added)
            {
                changeData["NewValues"] = entry.Properties
                    .ToDictionary(p => p.Metadata.Name, p => p.CurrentValue);
            }
            else if (entry.State == EntityState.Deleted)
            {
                changeData["DeletedValues"] = entry.Properties
                    .ToDictionary(p => p.Metadata.Name, p => p.OriginalValue);
            }

            var auditEntry = new AuditLog
            {
                EntityName = entry.Entity.GetType().Name,
                CreatedDateTime = now,
                CreatedByUserId = userId,
                ChangeData = JsonSerializer.Serialize(changeData, new JsonSerializerOptions
                {
                    WriteIndented = true
                })
            };
            auditEntries.Add(auditEntry);
        }

        if (auditEntries.Any()) context.Set<AuditLog>().AddRange(auditEntries);
    }
}