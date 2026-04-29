namespace TaskManager.Client.Services.Interfaces;

/// <summary>
/// Хранилище JWT-токенов на стороне клиента.
/// </summary>
public interface ITokenStorageService
{
    /// <summary>Сохранить пару токенов.</summary>
    void SaveTokens(string accessToken, string refreshToken);

    /// <summary>Получить access-токен.</summary>
    string? GetAccessToken();

    /// <summary>Получить refresh-токен.</summary>
    string? GetRefreshToken();

    /// <summary>Очистить сохранённые токены.</summary>
    void ClearTokens();

    /// <summary>Проверить наличие токенов.</summary>
    bool HasTokens();
}
