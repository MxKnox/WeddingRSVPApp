using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using WeddingApp.Data.Auditing;
using WeddingApp.Data.GiftRegistration;
using WeddingApp.Data.Reservations;
using WeddingApp.Data.Schedule;
using WeddingApp.Identity;

namespace WeddingApp.Data;

// DbContext
public class WeddingDbContext(DbContextOptions<WeddingDbContext> options) : IdentityDbContext<WeddingAppUser>(options)
{
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<ReservationPerson> ReservationPeople { get; set; }
    public DbSet<GiftRegisterEntry> GiftRegistryEntries { get; set; }
    public DbSet<GiftRegisterTag> GiftRegistryTags { get; set; }
    public DbSet<Location> Locations { get; set; }
    public DbSet<ScheduleEntry> ScheduleEntries { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Configure relationships
        builder.Entity<Reservation>()
            .HasMany(r => r.People)
            .WithOne(p => p.Reservation)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<ReservationPerson>()
            .HasOne(p => p.User)
            .WithMany()
            .HasForeignKey(p => p.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        builder.Entity<GiftRegisterEntry>()
            .HasMany(g => g.Tags)
            .WithOne(t => t.GiftRegistryEntry)
            .HasForeignKey(t => t.GiftRegistryEntryId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Entity<GiftRegisterTag>()
            .HasOne(t => t.User)
            .WithMany()
            .HasForeignKey(t => t.UserId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.Entity<Location>()
            .HasMany(l => l.ScheduleEntries)
            .WithOne(s => s.Location)
            .HasForeignKey(s => s.LocationId)
            .OnDelete(DeleteBehavior.SetNull);
        
        builder.Entity<AuditLog>()
            .ToTable("AuditLogs")
            .HasKey(a => a.Id);

        // Indexes
        builder.Entity<Reservation>()
            .HasIndex(r => r.RegistrationToken)
            .IsUnique();

        builder.Entity<ReservationPerson>()
            .HasIndex(p => p.UserId);

        builder.Entity<GiftRegisterTag>()
            .HasIndex(t => new { t.GiftRegistryEntryId, t.UserId })
            .IsUnique();

        // Configure audit fields to be database generated
        foreach (var entityType in builder.Model.GetEntityTypes())
            if (typeof(IAuditable).IsAssignableFrom(entityType.ClrType))
            {
                var entity = builder.Entity(entityType.ClrType);
                entity.HasKey("Id");
                
                entity.Property(nameof(IAuditable.CreatedDateTime))
                    .ValueGeneratedOnAdd()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
                entity.Property(nameof(IAuditable.ModifiedDateTime))
                    .ValueGeneratedOnAddOrUpdate()
                    .HasDefaultValueSql("CURRENT_TIMESTAMP");
            }
    }
}