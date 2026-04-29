namespace TaskManager.API.Configuration;

/// <summary>
/// Параметры конфигурации JWT-токенов.
/// </summary>
public class JwtSettings
{
    /// <summary>Секретный ключ для подписи токенов.</summary>
    public string Secret { get; set; } = string.Empty;

    /// <summary>Издатель токена.</summary>
    public string Issuer { get; set; } = string.Empty;

    /// <summary>Аудитория токена.</summary>
    public string Audience { get; set; } = string.Empty;

    /// <summary>Время жизни access-токена в минутах.</summary>
    public int AccessTokenExpirationMinutes { get; set; } = 30;

    /// <summary>Время жизни refresh-токена в днях.</summary>
    public int RefreshTokenExpirationDays { get; set; } = 7;
}
