using TaskManager.Client.Services.Interfaces;
using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services;

/// <summary>
/// Клиентский сервис аутентификации: вход, регистрация, обновление токена.
/// </summary>
public class AuthApiService(IApiClient apiClient, ITokenStorageService tokenStorage) : IAuthApiService
{
    private UserDto? _currentUser;

    public bool IsAuthenticated => tokenStorage.HasTokens();
    public UserDto? CurrentUser => _currentUser;

    /// <summary>Выполняет вход пользователя.</summary>
    public async Task<AuthResponse> LoginAsync(string username, string password)
    {
        var request = new LoginRequest(username, password);
        var response = await apiClient.PostAsync<AuthResponse>("api/auth/login", request)
            ?? throw new InvalidOperationException("Не удалось выполнить вход.");

        tokenStorage.SaveTokens(response.AccessToken, response.RefreshToken);
        _currentUser = response.User;
        return response;
    }

    /// <summary>Регистрирует нового пользователя.</summary>
    public async Task<AuthResponse> RegisterAsync(string username, string email, string password)
    {
        var request = new RegisterRequest(username, email, password);
        var response = await apiClient.PostAsync<AuthResponse>("api/auth/register", request)
            ?? throw new InvalidOperationException("Не удалось выполнить регистрацию.");

        tokenStorage.SaveTokens(response.AccessToken, response.RefreshToken);
        _currentUser = response.User;
        return response;
    }

    /// <summary>Обновляет пару токенов.</summary>
    public async Task<AuthResponse> RefreshTokenAsync()
    {
        var refreshToken = tokenStorage.GetRefreshToken()
            ?? throw new InvalidOperationException("Refresh-токен отсутствует.");

        var request = new RefreshTokenRequest(refreshToken);
        var response = await apiClient.PostAsync<AuthResponse>("api/auth/refresh", request)
            ?? throw new InvalidOperationException("Не удалось обновить токен.");

        tokenStorage.SaveTokens(response.AccessToken, response.RefreshToken);
        _currentUser = response.User;
        return response;
    }

    /// <summary>Выполняет выход из системы.</summary>
    public void Logout()
    {
        tokenStorage.ClearTokens();
        _currentUser = null;
    }
}
