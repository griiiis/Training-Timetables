using System.Net.Http.Json;
using System.Text.Json;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace App.Test.Integration.API.Identity;
[Collection("NonParallel")]
public class LoginRegisterTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public LoginRegisterTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }


    [Fact]
    public async Task RegisterNewUser()
    {
        // get jwt
        var response =
            await _client.PostAsJsonAsync("api/v1.0/Identity/Account/Register",
                new
                {
                    FirstName = "First Name",
                    LastName = "Last Name",
                    Age = 20,
                    Gender = 0,
                    Email = "new@gmail.com",
                    Password = "Uus!12",
                    ConfirmPassword = "Uus!12"
                });

        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var registerData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        Assert.NotNull(registerData);
        Assert.NotNull(registerData.Jwt);
        Assert.True(registerData.Jwt.Length > 0);
    }

    [Fact]
    public async Task LoginUser()
    {
        var response =
            await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login",
                new { Email = "admin@eesti.ee", Password = "Mina!1" });
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
    }
}