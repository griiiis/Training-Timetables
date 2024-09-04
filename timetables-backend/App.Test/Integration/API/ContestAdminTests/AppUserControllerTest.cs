using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using App.BLL.DTO.Enums;
using App.Domain.Identity;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using NSubstitute;
using WebApp.Areas.ContestAdmin.ViewModels;
using AppUser = App.BLL.DTO.Identity.AppUser;
using JsonSerializer = System.Text.Json.JsonSerializer;



namespace App.Test.Integration.api.ContestAdminTests;
[Collection("NonParallel")]
public class AppUserControllerTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public AppUserControllerTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = _factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
    /*
    [Fact]
    public async Task GetAppUser()
    {
        // Trainer Register get jwt
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
        
        //Admin login, get jwt
        response =
            await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login", new {Email="admin@eesti.ee", Password="Mina!1"});
        contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        var msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/AppUser/{registerData!.UserId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        
        var data = JsonSerializer.Deserialize<AppUserModel>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        Assert.NotNull(data);
        Assert.Equal(data.AppUser.FirstName, registerData.FirstName);
    }
    
    
    [Fact]
    public async Task EditAppUser()
    {
        // Trainer Register get jwt
        var response =
            await _client.PostAsJsonAsync("api/v1.0/Identity/Account/Register",
                new
                {
                    FirstName = "First Name",
                    LastName = "Last Name",
                    Age = 20,
                    Gender = 0,
                    Email = "new1@gmail.com",
                    Password = "Uus!12",
                    ConfirmPassword = "Uus!12"
                });

        var contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var registerData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);
        
        //Admin login, get jwt
        response =
            await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login", new {Email="admin@eesti.ee", Password="Mina!1"});
        contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        var msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/AppUser/{registerData!.UserId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        
        var data = JsonSerializer.Deserialize<AppUserModel>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        
        var roleId = data!.RoleSelectList.First(e => e.Name == "Treener").Id;
        var vm = new AppUserEditViewModel
        {
            AppUser = new AppUser
            {
                Id = Guid.Parse(registerData!.UserId),
                FirstName = "First Name",
                LastName = "Last Name",
                Age = 20,
                Gender = 0,
                Email = "new@gmail.com",
            },
            SelectedRoleId = roleId,
        };

        var json = JsonConvert.SerializeObject(vm);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Put, $"/api/v1.0/AppUser/{registerData.UserId}")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
    }*/
    
}