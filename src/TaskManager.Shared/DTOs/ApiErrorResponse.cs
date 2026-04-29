namespace TaskManager.Shared.DTOs;

/// <summary>
/// Стандартный формат ответа при ошибке API.
/// </summary>
public record ApiErrorResponse(int StatusCode, string Message, IEnumerable<string>? Errors = null);
