using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskManager.Domain.Interfaces;
using TaskManager.API.Mapping;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Controllers;

/// <summary>
/// Контроллер профиля пользователя.
/// </summary>
[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsersController(IUserRepository userRepository) : ControllerBase
{
    /// <summary>Получить профиль текущего пользователя.</summary>
    [HttpGet("me")]
    public async Task<IActionResult> GetCurrentUser()
    {
        var userIdClaim = User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new UnauthorizedAccessException("Не удалось определить пользователя.");

        var user = await userRepository.GetByIdAsync(Guid.Parse(userIdClaim));
        if (user is null)
            return NotFound(new ApiErrorResponse(404, "Пользователь не найден."));

        return Ok(user.ToDto());
    }
}
