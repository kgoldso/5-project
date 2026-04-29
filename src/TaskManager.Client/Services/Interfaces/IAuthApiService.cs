using TaskManager.Shared.DTOs;

namespace TaskManager.Client.Services.Interfaces;

/// <summary>
/// Клиентский сервис аутентификации через API.
/// </summary>
public interface IAuthApiService
{
    Task<AuthResponse> LoginAsync(string username, string password);
    Task<AuthResponse> RegisterAsync(string username, string email, string password);
    Task<AuthResponse> RefreshTokenAsync();
    void Logout();
    bool IsAuthenticated { get; }
    UserDto? CurrentUser { get; }
}
