using Microsoft.AspNetCore.Mvc;
using TaskManager.API.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.API.Controllers;

/// <summary>
/// Контроллер аутентификации и регистрации.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController(IAuthService authService) : ControllerBase
{
    /// <summary>Регистрация нового пользователя.</summary>
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) ||
            string.IsNullOrWhiteSpace(request.Email) ||
            string.IsNullOrWhiteSpace(request.Password))
        {
            return BadRequest(new ApiErrorResponse(400, "Все поля обязательны для заполнения."));
        }

        if (request.Password.Length < 6)
            return BadRequest(new ApiErrorResponse(400, "Пароль должен содержать минимум 6 символов."));

        var result = await authService.RegisterAsync(request);
        return Ok(result);
    }

    /// <summary>Аутентификация пользователя.</summary>
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username) || string.IsNullOrWhiteSpace(request.Password))
            return BadRequest(new ApiErrorResponse(400, "Имя пользователя и пароль обязательны."));

        var result = await authService.LoginAsync(request);
        return Ok(result);
    }

    /// <summary>Обновление пары токенов.</summary>
    [HttpPost("refresh")]
    public async Task<IActionResult> Refresh([FromBody] RefreshTokenRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.RefreshToken))
            return BadRequest(new ApiErrorResponse(400, "Refresh-токен обязателен."));

        var result = await authService.RefreshTokenAsync(request.RefreshToken);
        return Ok(result);
    }
}
