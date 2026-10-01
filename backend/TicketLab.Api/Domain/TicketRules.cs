using TicketLab.Api.Models;

namespace TicketLab.Api.Domain;

// Business rules without any database or HTTP code,
// so they are easy to unit test later (phase 4).
public static class TicketRules
{
    // Which status a ticket may move to from its current status.
    //   New -> InProgress -> Resolved -> Closed
    //               ^           |
    //               +- reopen --+
    private static readonly Dictionary<TicketStatus, TicketStatus[]> AllowedTransitions = new()
    {
        [TicketStatus.New] = [TicketStatus.InProgress, TicketStatus.Closed],
        [TicketStatus.InProgress] = [TicketStatus.Resolved],
        [TicketStatus.Resolved] = [TicketStatus.InProgress, TicketStatus.Closed],
        [TicketStatus.Closed] = []
    };

    public static bool CanChangeStatus(TicketStatus from, TicketStatus to) =>
        AllowedTransitions[from].Contains(to);

    // A closed ticket is read-only: no priority changes, no new comments.
    public static bool IsReadOnly(TicketStatus status) =>
        status == TicketStatus.Closed;
}
