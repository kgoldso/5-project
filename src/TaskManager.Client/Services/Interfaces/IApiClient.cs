namespace TaskManager.Client.Services.Interfaces;

/// <summary>
/// Базовый HTTP-клиент для взаимодействия с API.
/// </summary>
public interface IApiClient
{
    /// <summary>GET-запрос.</summary>
    Task<T?> GetAsync<T>(string endpoint);

    /// <summary>POST-запрос с телом.</summary>
    Task<T?> PostAsync<T>(string endpoint, object body);

    /// <summary>PUT-запрос с телом.</summary>
    Task<T?> PutAsync<T>(string endpoint, object body);

    /// <summary>DELETE-запрос.</summary>
    Task DeleteAsync(string endpoint);
}
