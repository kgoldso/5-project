using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;
using TaskManager.Client.Services.Interfaces;

namespace TaskManager.Client.Services;

/// <summary>
/// Обертка над HttpClient для удобного общения с REST API.
/// Сама берет токены из хранилища и добавляет в заголовки.
/// </summary>
public class ApiClient(ITokenStorageService tokenStorage) : IApiClient
{
    private readonly HttpClient _httpClient = new() { BaseAddress = new Uri("http://localhost:5000") };
    private static readonly JsonSerializerOptions JsonOptions = new() { PropertyNameCaseInsensitive = true };

    public async Task<T?> GetAsync<T>(string endpoint)
    {
        SetAuthorizationHeader();
        var response = await _httpClient.GetAsync(endpoint);
        return await HandleResponseAsync<T>(response);
    }

    public async Task<T?> PostAsync<T>(string endpoint, object body)
    {
        SetAuthorizationHeader();
        var content = SerializeBody(body);
        var response = await _httpClient.PostAsync(endpoint, content);
        return await HandleResponseAsync<T>(response);
    }

    public async Task<T?> PutAsync<T>(string endpoint, object body)
    {
        SetAuthorizationHeader();
        var content = SerializeBody(body);
        var response = await _httpClient.PutAsync(endpoint, content);
        return await HandleResponseAsync<T>(response);
    }

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

    // Добавляем JWT токен в заголовок запроса
    private void SetAuthorizationHeader()
    {
        var token = tokenStorage.GetAccessToken();
        _httpClient.DefaultRequestHeaders.Authorization = string.IsNullOrEmpty(token)
            ? null
            : new AuthenticationHeaderValue("Bearer", token);
    }

    private static StringContent SerializeBody(object body)
        => new(JsonSerializer.Serialize(body, JsonOptions), Encoding.UTF8, "application/json");

    private static async Task<T?> HandleResponseAsync<T>(HttpResponseMessage response)
    {
        var json = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
            throw new HttpRequestException($"Ошибка API ({(int)response.StatusCode}): {json}");

        return JsonSerializer.Deserialize<T>(json, JsonOptions);
    }
}
