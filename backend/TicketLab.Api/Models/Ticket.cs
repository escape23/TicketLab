namespace TicketLab.Api.Models;

public class Ticket
{
    public int Id { get; set; }
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public TicketCategory Category { get; set; } = TicketCategory.Other;
    public TicketStatus Status { get; set; } = TicketStatus.New;

    public int CreatedById { get; set; }
    public User CreatedBy { get; set; } = null!;

    // Nullable: a new ticket is not assigned to anyone yet.
    public int? AssignedToId { get; set; }
    public User? AssignedTo { get; set; }

    public DateTime CreatedAt { get; set; }
    public DateTime UpdatedAt { get; set; }

    public List<Comment> Comments { get; set; } = [];
    public List<TicketHistory> History { get; set; } = [];
}
