namespace TicketLab.Api.Models;

// One row per changed field, e.g. Field = "Status", OldValue = "New", NewValue = "InProgress".
public class TicketHistory
{
    public int Id { get; set; }
    public string Field { get; set; } = "";
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }
    public DateTime ChangedAt { get; set; }

    public int TicketId { get; set; }
    public Ticket Ticket { get; set; } = null!;

    public int ChangedById { get; set; }
    public User ChangedBy { get; set; } = null!;
}
