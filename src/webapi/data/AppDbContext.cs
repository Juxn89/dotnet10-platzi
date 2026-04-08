using Microsoft.EntityFrameworkCore;

using webapi.models;

namespace webapi.data;

public class AppDbContext : DbContext
{
  public DbSet<User> Users => Set<User>();
  public DbSet<TaskItem> TaskItems => Set<TaskItem>();

  public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
  {
  }

  protected override void OnModelCreating(ModelBuilder modelBuilder)
  {
    base.OnModelCreating(modelBuilder);

    modelBuilder.Entity<User>(entity => {
      entity.ToTable("User");
      entity.HasKey(u => u.Id);
      entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
      entity.Property(u => u.Email).IsRequired().HasMaxLength(200);
    });

    modelBuilder.Entity<TaskItem>(entity => {
      entity.ToTable("Tasks");
      entity.HasKey(u => u.Id);
      entity.Property(u => u.Title).IsRequired().HasMaxLength(200);
      entity.Property(u => u.IsCompleted).HasDefaultValue(true);

      entity.HasOne(t => t.User)
        .WithMany(t => t.Tasks)
        .HasForeignKey(t => t.UserId)
        .OnDelete(DeleteBehavior.Cascade);
    });
  }
}
