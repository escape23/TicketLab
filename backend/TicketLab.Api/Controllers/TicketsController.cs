using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketLab.Api.Data;
using TicketLab.Api.Domain;
using TicketLab.Api.Dtos;
using TicketLab.Api.Models;

namespace TicketLab.Api.Controllers;

[ApiController]
[Route("api/tickets")]
public class TicketsController(TicketLabDbContext db) : ControllerBase
{
    [HttpGet]
    public async Task<List<TicketSummaryDto>> GetTickets([FromQuery] TicketStatus? status)
    {
        var query = db.Tickets.AsQueryable();

        if (status is not null)
            query = query.Where(t => t.Status == status);

        return await query
            .OrderByDescending(t => t.CreatedAt)
            .Select(t => new TicketSummaryDto(
                t.Id, t.Title, t.Priority, t.Category, t.Status,
                t.CreatedBy.Name,
                t.AssignedTo != null ? t.AssignedTo.Name : null,
                t.CreatedAt, t.UpdatedAt))
            .ToListAsync();
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<TicketDetailsDto>> GetTicket(int id)
    {
        var ticket = await LoadDetailsAsync(id);
        return ticket is null ? NotFound() : ticket;
    }

    [HttpPost]
    public async Task<ActionResult<TicketDetailsDto>> CreateTicket(CreateTicketRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
            return UnknownUser();

        var now = DateTime.UtcNow;
        var ticket = new Ticket
        {
            Title = request.Title.Trim(),
            Description = request.Description.Trim(),
            Priority = request.Priority!.Value,
            Category = request.Category!.Value,
            Status = TicketStatus.New,
            CreatedById = user.Id,
            CreatedAt = now,
            UpdatedAt = now
        };
        AddHistory(ticket, user, "Status", null, ticket.Status.ToString());

        db.Tickets.Add(ticket);
        await db.SaveChangesAsync();

        // 201 Created + a Location header pointing to GET /api/tickets/{id}
        return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, await LoadDetailsAsync(ticket.Id));
    }

    [HttpPatch("{id:int}/status")]
    public async Task<ActionResult<TicketDetailsDto>> ChangeStatus(int id, ChangeStatusRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
            return UnknownUser();

        var ticket = await db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        var newStatus = request.Status!.Value;
        if (!TicketRules.CanChangeStatus(ticket.Status, newStatus))
            return Problem(
                statusCode: StatusCodes.Status409Conflict,
                title: $"Cannot change status from {ticket.Status} to {newStatus}.");

        AddHistory(ticket, user, "Status", ticket.Status.ToString(), newStatus.ToString());
        ticket.Status = newStatus;
        ticket.UpdatedAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        // The ticket was found above, so it cannot be null here.
        return (await LoadDetailsAsync(id))!;
    }

    [HttpPatch("{id:int}/priority")]
    public async Task<ActionResult<TicketDetailsDto>> ChangePriority(int id, ChangePriorityRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
            return UnknownUser();

        var ticket = await db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        if (TicketRules.IsReadOnly(ticket.Status))
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Closed tickets cannot be changed.");

        var newPriority = request.Priority!.Value;

        // Same value: nothing to change, so no history row either.
        if (ticket.Priority != newPriority)
        {
            AddHistory(ticket, user, "Priority", ticket.Priority.ToString(), newPriority.ToString());
            ticket.Priority = newPriority;
            ticket.UpdatedAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }

        // The ticket was found above, so it cannot be null here.
        return (await LoadDetailsAsync(id))!;
    }

    [HttpPost("{id:int}/comments")]
    public async Task<ActionResult<CommentDto>> AddComment(int id, AddCommentRequest request)
    {
        var user = await GetCurrentUserAsync();
        if (user is null)
            return UnknownUser();

        var ticket = await db.Tickets.FindAsync(id);
        if (ticket is null)
            return NotFound();

        if (TicketRules.IsReadOnly(ticket.Status))
            return Problem(statusCode: StatusCodes.Status409Conflict, title: "Closed tickets cannot be commented.");

        var comment = new Comment
        {
            TicketId = id,
            AuthorId = user.Id,
            Text = request.Text.Trim(),
            CreatedAt = DateTime.UtcNow
        };
        db.Comments.Add(comment);
        await db.SaveChangesAsync();

        return StatusCode(StatusCodes.Status201Created,
            new CommentDto(comment.Id, comment.Text, user.Name, comment.CreatedAt));
    }

    [HttpGet("{id:int}/history")]
    public async Task<ActionResult<List<HistoryDto>>> GetHistory(int id)
    {
        if (!await db.Tickets.AnyAsync(t => t.Id == id))
            return NotFound();

        return await db.TicketHistory
            .Where(h => h.TicketId == id)
            .OrderBy(h => h.ChangedAt)
            .Select(h => new HistoryDto(h.Id, h.Field, h.OldValue, h.NewValue, h.ChangedBy.Name, h.ChangedAt))
            .ToListAsync();
    }

    // ----- Helpers -----

    // MVP shortcut (known technical debt): the current user comes from the X-User-Id header,
    // so anyone can pretend to be anyone. Replaced by real authentication in phase 6.
    private async Task<User?> GetCurrentUserAsync()
    {
        if (!int.TryParse(Request.Headers["X-User-Id"], out var userId))
            return null;

        return await db.Users.FindAsync(userId);
    }

    private ObjectResult UnknownUser() =>
        Problem(statusCode: StatusCodes.Status401Unauthorized, title: "Missing or unknown X-User-Id header.");

    private static void AddHistory(Ticket ticket, User user, string field, string? oldValue, string? newValue) =>
        ticket.History.Add(new TicketHistory
        {
            Field = field,
            OldValue = oldValue,
            NewValue = newValue,
            ChangedById = user.Id,
            ChangedAt = DateTime.UtcNow
        });

    private Task<TicketDetailsDto?> LoadDetailsAsync(int id) =>
        db.Tickets
            .Where(t => t.Id == id)
            .Select(t => new TicketDetailsDto(
                t.Id, t.Title, t.Description, t.Priority, t.Category, t.Status,
                t.CreatedBy.Name,
                t.AssignedTo != null ? t.AssignedTo.Name : null,
                t.CreatedAt, t.UpdatedAt,
                t.Comments
                    .OrderBy(c => c.CreatedAt)
                    .Select(c => new CommentDto(c.Id, c.Text, c.Author.Name, c.CreatedAt))
                    .ToList()))
            .FirstOrDefaultAsync();
}
