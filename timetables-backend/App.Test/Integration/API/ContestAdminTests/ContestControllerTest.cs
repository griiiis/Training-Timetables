using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NuGet.Protocol.Core.Types;
using JsonSerializer = System.Text.Json.JsonSerializer;

namespace App.Test.Integration.api;

[Collection("NonParallel")]
public class ContestControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public ContestControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }

    [Fact]
    public async Task IndexRequiresLogin()
    {
        // Act
        var response = await _client.GetAsync("/api/v1.0/Contests/owner");
        // Assert
        Assert.Equal(HttpStatusCode.Unauthorized, response.StatusCode);
    }

    [Fact]
    public async Task IndexWithUser()
    {
        // get jwt
        var response =
            await _client.PostAsJsonAsync("api/v1.0/Identity/Account/Login", new {Email="admin@eesti.ee", Password="Mina!1"});
        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        Assert.NotNull(loginData);
        Assert.NotNull(loginData.Jwt);
        Assert.True(loginData.Jwt.Length > 0);

        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Contests/owner");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }
    
    [Fact]
    public async Task CreateContest()
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
        
        //ContestType
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/ContestTypes/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var contestTypeDataList =
            JsonSerializer.Deserialize<List<ContestType>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var contestTypeId = contestTypeDataList.First().Id;
        
        //LocationId
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Locations/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var LocationDataList =
            JsonSerializer.Deserialize<List<Location>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var LocationId = LocationDataList.First().Id;
        
        //LevelIds
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Levels/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var levelDataList =
            JsonSerializer.Deserialize<List<Level>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var levelId = levelDataList.Take(2).Select(e => e.Id).ToList();
        
        //Times
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Times/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var timeDataList =
            JsonSerializer.Deserialize<List<Time>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var timeIds = timeDataList.Take(2).Select(e => e.Id).ToList();
        
        //Packages
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/PackageGameTypeTimes/owner");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var packageDataList =
            JsonSerializer.Deserialize<List<PackageGameTypeTime>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var packageIds = packageDataList.Take(2).Where(e => e.GameType!.GameTypeName == "Tennis").Select(e => e.Id).ToList();

         
        
        var contest = new App.DTO.v1_0.ContestEditModel
        {
            Contest = new Contest()
            {
                ContestName = "New Contest",
                Description = "Something",
                TotalHours = 10,
                From = new DateTime(new DateOnly(2024,
                        05,
                        20),
                    new TimeOnly(10,
                        00)),
                Until = new DateTime(new DateOnly(2024,
                        05,
                        24),
                    new TimeOnly(22,
                        00)),
                ContestTypeId = contestTypeId,
                LocationId = LocationId,

            },
            SelectedLevelIds = levelId,
            SelectedTimesIds = timeIds,
            SelectedPackagesIds = packageIds,
        };
        
        var json = JsonConvert.SerializeObject(contest);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/Contests")
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