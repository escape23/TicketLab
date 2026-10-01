using Microsoft.EntityFrameworkCore;
using TicketLab.Api.Models;

namespace TicketLab.Api.Data;

public class TicketLabDbContext(DbContextOptions<TicketLabDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();
    public DbSet<Ticket> Tickets => Set<Ticket>();
    public DbSet<Comment> Comments => Set<Comment>();
    public DbSet<TicketHistory> TicketHistory => Set<TicketHistory>();

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        // Store enums as readable text ("InProgress") instead of numbers (1),
        // so the data makes sense when you query the tables directly in SQL.
        configurationBuilder.Properties<UserRole>().HaveConversion<string>().HaveMaxLength(20);
        configurationBuilder.Properties<TicketStatus>().HaveConversion<string>().HaveMaxLength(20);
        configurationBuilder.Properties<TicketPriority>().HaveConversion<string>().HaveMaxLength(20);
        configurationBuilder.Properties<TicketCategory>().HaveConversion<string>().HaveMaxLength(20);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>(user =>
        {
            user.Property(u => u.Name).HasMaxLength(100);
            user.Property(u => u.Email).HasMaxLength(200);
            user.HasIndex(u => u.Email).IsUnique();
        });

        modelBuilder.Entity<Ticket>(ticket =>
        {
            ticket.Property(t => t.Title).HasMaxLength(200);
            ticket.Property(t => t.Description).HasMaxLength(4000);

            // Restrict: a user who has tickets cannot be deleted by accident.
            // It also avoids SQL Server's "multiple cascade paths" error,
            // because Ticket points to User twice (CreatedBy and AssignedTo).
            ticket.HasOne(t => t.CreatedBy).WithMany()
                .HasForeignKey(t => t.CreatedById)
                .OnDelete(DeleteBehavior.Restrict);

            ticket.HasOne(t => t.AssignedTo).WithMany()
                .HasForeignKey(t => t.AssignedToId)
                .OnDelete(DeleteBehavior.Restrict);

            ticket.HasIndex(t => t.Status);
        });

        modelBuilder.Entity<Comment>(comment =>
        {
            comment.Property(c => c.Text).HasMaxLength(2000);

            // Cascade: deleting a ticket deletes its comments.
            comment.HasOne(c => c.Ticket).WithMany(t => t.Comments)
                .HasForeignKey(c => c.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            comment.HasOne(c => c.Author).WithMany()
                .HasForeignKey(c => c.AuthorId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        modelBuilder.Entity<TicketHistory>(history =>
        {
            history.Property(h => h.Field).HasMaxLength(50);
            history.Property(h => h.OldValue).HasMaxLength(200);
            history.Property(h => h.NewValue).HasMaxLength(200);

            history.HasOne(h => h.Ticket).WithMany(t => t.History)
                .HasForeignKey(h => h.TicketId)
                .OnDelete(DeleteBehavior.Cascade);

            history.HasOne(h => h.ChangedBy).WithMany()
                .HasForeignKey(h => h.ChangedById)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // Test users, one per role. Fixed ids so the migration stays the same every time.
        modelBuilder.Entity<User>().HasData(
            new User { Id = 1, Name = "Anna User", Email = "anna@ticketlab.local", Role = UserRole.User },
            new User { Id = 2, Name = "Ola Developer", Email = "ola@ticketlab.local", Role = UserRole.Developer },
            new User { Id = 3, Name = "Kari Admin", Email = "kari@ticketlab.local", Role = UserRole.Admin }
        );
    }
}
