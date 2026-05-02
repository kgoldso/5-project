using System.Net;
using System.Net.Http.Json;
using Microsoft.AspNetCore.Mvc.Testing;
using TaskManager.Shared.DTOs;
using Xunit;

namespace TaskManager.API.Tests.Controllers;

public class AuthValidationTests(WebApplicationFactory<Program> factory) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    [Theory]
    [InlineData("", "test@test.com", "password")]
    [InlineData("user", "invalid-email", "password")]
    [InlineData("user", "test@test.com", "123")]
    public async Task Register_ReturnsBadRequest_WhenInputIsInvalid(string username, string email, string password)
    {
        var request = new RegisterRequest(username, email, password);
        var response = await _client.PostAsJsonAsync("/api/auth/register", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }

    [Theory]
    [InlineData("", "password")]
    [InlineData("user", "")]
    public async Task Login_ReturnsBadRequest_WhenInputIsInvalid(string username, string password)
    {
        var request = new LoginRequest(username, password);
        var response = await _client.PostAsJsonAsync("/api/auth/login", request);
        Assert.Equal(HttpStatusCode.BadRequest, response.StatusCode);
    }
}
