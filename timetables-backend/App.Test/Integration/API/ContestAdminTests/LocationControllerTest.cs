using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.Test.Integration.api.ContestAdminTests;
[Collection("NonParallel")]
public class LocationControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public LocationControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task IndexLocationsAsContestAdmin()
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


        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Locations");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task IndexLocationsAsRandomUser()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Locations");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(msg);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    
    [Fact]
    public async Task CreateLocation()
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
        
        var Location = new App.DTO.v1_0.Location
        {
            LocationName = "Uued väljakud seal",
            State = "Harjumaa",
            Country = "Eesti",
        };
        
        var json = JsonConvert.SerializeObject(Location);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Locations")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        
        Assert.Equal("Created",response.ReasonPhrase);
    }
}