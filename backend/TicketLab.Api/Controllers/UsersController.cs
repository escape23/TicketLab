using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TicketLab.Api.Data;
using TicketLab.Api.Dtos;

namespace TicketLab.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(TicketLabDbContext db) : ControllerBase
{
    // Used by the frontend's "act as user" switcher (until real login in phase 6).
    [HttpGet]
    public async Task<List<UserDto>> GetUsers() =>
        await db.Users
            .OrderBy(u => u.Id)
            .Select(u => new UserDto(u.Id, u.Name, u.Role))
            .ToListAsync();
}
