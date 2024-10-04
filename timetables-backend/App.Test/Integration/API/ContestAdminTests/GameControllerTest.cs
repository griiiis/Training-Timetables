using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using App.DTO.v1_0.DTOs;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.Test.Integration.api.ContestAdminTests;
[Collection("NonParallel")]
public class GameControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public GameControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task IndexGamesAsRandomUser()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/Games/contestgames/{Guid.NewGuid()}");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(msg);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    [Fact]
    public async Task IndexGamesAsContestAdmin()
    {
        //login, get jwt
        var response =
            await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login", new {Email="admin@eesti.ee", Password="Mina!1"});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);


        var msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/Games/contestgames/{Guid.NewGuid()}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    [Fact]
    public async Task CreateGame()
    {
        //login, get jwt
        var response =
            await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login", new {Email="admin@eesti.ee", Password="Mina!1"});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);
        
        var day1 = new TimeOfDaysViewModel
        {
            Date = new DateOnly(2024, 05, 17),
            SelectedTimesList = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]
        };
        var day2 = new TimeOfDaysViewModel
        {
            Date = new DateOnly(2024, 05, 18),
            SelectedTimesList = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]
        };
        var day3 = new TimeOfDaysViewModel
        {
            Date = new DateOnly(2024, 05, 19),
            SelectedTimesList = [Guid.NewGuid(), Guid.NewGuid(), Guid.NewGuid()]
        };
        
        var Games = new CreateGamesData
        {
            SelectedLevelIds = [[Guid.NewGuid(),Guid.NewGuid()],[Guid.NewGuid()]],
            SelectedCourtIds =  [[Guid.NewGuid(),Guid.NewGuid()],[Guid.NewGuid(), Guid.NewGuid()]],
            SelectedTimesIds =  [day1, day2, day3],
            SelectedTrainersIds = [[Guid.NewGuid(),Guid.NewGuid()],[Guid.NewGuid()]],
            PeoplePerCourtInputs = [4, 4]
        };

        var json = JsonConvert.SerializeObject(Games);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var msg = new HttpRequestMessage(HttpMethod.Post, $"/api/v1.0/Games/{Guid.NewGuid()}")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        
        Assert.Equal("Created", response.ReasonPhrase);
    }
}