using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.Services;

/// <summary>
/// Хранилище JWT-токенов в памяти приложения.
/// </summary>
public class TokenStorageService : ITokenStorageService
{
    private string? _accessToken;
    private string? _refreshToken;

    public void SaveTokens(string accessToken, string refreshToken)
    {
        _accessToken = accessToken;
        _refreshToken = refreshToken;
    }

    public string? GetAccessToken() => _accessToken;
    public string? GetRefreshToken() => _refreshToken;

    public void ClearTokens()
    {
        _accessToken = null;
        _refreshToken = null;
    }

    public bool HasTokens() => !string.IsNullOrEmpty(_accessToken);
}
