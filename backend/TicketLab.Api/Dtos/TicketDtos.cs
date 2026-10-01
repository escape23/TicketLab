using System.ComponentModel.DataAnnotations;
using TicketLab.Api.Models;

namespace TicketLab.Api.Dtos;

// ----- Requests (what the client sends) -----
// Validation attributes are checked automatically by [ApiController];
// invalid input returns 400 Bad Request before the action method runs.

public class CreateTicketRequest
{
    [Required, StringLength(200)]
    public string Title { get; init; } = "";

    [StringLength(4000)]
    public string Description { get; init; } = "";

    // Nullable + [Required]: a missing value gives 400 instead of silently becoming the first enum value.
    [Required]
    public TicketPriority? Priority { get; init; }

    [Required]
    public TicketCategory? Category { get; init; }
}

public class ChangeStatusRequest
{
    [Required]
    public TicketStatus? Status { get; init; }
}

public class ChangePriorityRequest
{
    [Required]
    public TicketPriority? Priority { get; init; }
}

public class AddCommentRequest
{
    [Required, StringLength(2000)]
    public string Text { get; init; } = "";
}

// ----- Responses (what the API returns) -----
// Separate from the database models, so the API does not leak internal fields
// and does not run into circular references (Ticket -> Comment -> Ticket ...).

public record TicketSummaryDto(
    int Id,
    string Title,
    TicketPriority Priority,
    TicketCategory Category,
    TicketStatus Status,
    string CreatedBy,
    string? AssignedTo,
    DateTime CreatedAt,
    DateTime UpdatedAt);

public record TicketDetailsDto(
    int Id,
    string Title,
    string Description,
    TicketPriority Priority,
    TicketCategory Category,
    TicketStatus Status,
    string CreatedBy,
    string? AssignedTo,
    DateTime CreatedAt,
    DateTime UpdatedAt,
    List<CommentDto> Comments);

public record CommentDto(int Id, string Text, string Author, DateTime CreatedAt);

public record HistoryDto(int Id, string Field, string? OldValue, string? NewValue, string ChangedBy, DateTime ChangedAt);

public record UserDto(int Id, string Name, UserRole Role);
