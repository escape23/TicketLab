namespace TicketLab.Api.Models;

public enum UserRole
{
    User,
    Developer,
    Admin
}

public enum TicketStatus
{
    New,
    InProgress,
    Resolved,
    Closed
}

public enum TicketPriority
{
    Low,
    Medium,
    High,
    Critical
}

public enum TicketCategory
{
    Hardware,
    Software,
    Network,
    Access,
    Other
}
