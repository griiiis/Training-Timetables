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
public class TimeControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public TimeControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    
    [Fact]
    public async Task IndexTimesAsContestAdmin()
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


        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Times");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task IndexTimesAsRandomUser()
    {
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Times");
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        var response = await _client.SendAsync(msg);

        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }
    
    
    [Fact]
    public async Task CreateTime()
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
        
        //Time Of Day
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/TimeOfDays/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var timeOfDayData =
            JsonSerializer.Deserialize<List<TimeOfDay>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var timeOfDayId = timeOfDayData.First().Id;
        
        var Time = new App.DTO.v1_0.Time
        {
            From = new TimeOnly(12,30),
            Until = new TimeOnly(13,30),
            TimeOfDayId = timeOfDayId,
        };
        
        var json = JsonConvert.SerializeObject(Time);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Times")
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