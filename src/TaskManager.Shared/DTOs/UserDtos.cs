namespace TaskManager.Shared.DTOs;

/// <summary>
/// Информация о пользователе (ответ API).
/// </summary>
public record UserDto(Guid Id, string Username, string Email, string Role);
