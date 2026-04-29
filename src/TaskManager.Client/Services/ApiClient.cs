using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.Services;

/// <summary>
/// HTTP-клиент для взаимодействия с REST API.
/// Автоматически добавляет JWT-токен в заголовок Authorization.
/// </summary>
public class ApiClient(ITokenStorageService tokenStorage) : IApiClient
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    /// <summary>Выполняет GET-запрос к API.</summary>
    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponseAsync<T>(response);
    }

    /// <summary>Выполняет POST-запрос к API.</summary>
    public async Task<T?> PostAsync<T>(string endpoint, object body)
    {
        SetAuthorizationHeader();
        var content = SerializeBody(body);
        var response = await _httpClient.PostAsync(endpoint, content);
        return await HandleResponseAsync<T>(response);
    }

    /// <summary>Выполняет PUT-запрос к API.</summary>
    public async Task<T?> PutAsync<T>(string endpoint, object body)
    {
        SetAuthorizationHeader();
        var content = SerializeBody(body);
        var response = await _httpClient.PutAsync(endpoint, content);
        return await HandleResponseAsync<T>(response);
    }

    /// <summary>Выполняет DELETE-запрос к API.</summary>
    public async Task DeleteAsync(string endpoint)
    {
        SetAuthorizationHeader();
        var response = await _httpClient.DeleteAsync(endpoint);
        if (!response.IsSuccessStatusCode)
        {
            var errorBody = await response.Content.ReadAsStringAsync();
            throw new HttpRequestException($"Ошибка API ({(int)response.StatusCode}): {errorBody}");
        }
    }

    /// <summary>Устанавливает Bearer-токен в заголовок запроса.</summary>
    private void SetAuthorizationHeader()
    {
        var token = tokenStorage.GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    /// <summary>Сериализует тело запроса в JSON.</summary>
    private static StringContent SerializeBody(object body)
        => new(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");

    /// <summary>Десериализует ответ API или выбрасывает исключение.</summary>
    private static async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Ошибка API ({(int)response.StatusCode}): {json}");

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
}
