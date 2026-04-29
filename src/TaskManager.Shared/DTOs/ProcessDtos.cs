namespace TaskManager.Shared.DTOs;

/// <summary>
/// Информация о процессе (ответ API).
/// </summary>
public record ProcessDto(
    Guid Id,
    string Title,
    string? Description,
    DateTime CreatedAt,
    int TaskCount);

/// <summary>
/// Данные для создания или обновления процесса.
/// </summary>
public record ProcessCreateDto(string Title, string? Description);
