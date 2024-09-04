using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Text;
using App.DTO.v1_0;
using App.DTO.v1_0.Identity;
using Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using WebApp.Areas.ContestAdmin.ViewModels;
using AppUser = App.BLL.DTO.Identity.AppUser;
using JsonSerializer = System.Text.Json.JsonSerializer;
using TimeOfDaysViewModel = App.DTO.v1_0.TimeOfDaysViewModel;


namespace App.Test.Integration.API.Participant;
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
/*
    [Fact]
    public async Task IndexMyContestsAsLoggedInUser()
    {
        //login as Contest Admin, get jwt
        var adminResponse = await _client.PostAsJsonAsync("api/v1.0/identity/Account/Login",
            new { Email = "admin@eesti.ee", Password = "Mina!1" });
        var contentStr = await adminResponse.Content.ReadAsStringAsync();
        adminResponse.EnsureSuccessStatusCode();
        var loginData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        // Register new participant, get jwt
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

        contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();
        var registerData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        //Get Contest Id
        var msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Contests/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", registerData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var contestDataList =
            JsonSerializer.Deserialize<List<Contest>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var contestId = contestDataList.First().Id;

        //Get Package Id
        msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/PackageGameTypeTimes/{contestId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", registerData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var packageDataList =
            JsonSerializer.Deserialize<List<PackageGameTypeTime>>(await response.Content.ReadAsStringAsync(),
                JsonHelper.CamelCase);
        var packageId = packageDataList.First().Id;

        //Get Level Id
        msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/Levels/{contestId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", registerData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var levelsData =
            JsonSerializer.Deserialize<List<Level>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);
        var levelId = levelsData.First().Id;

        //Join the contest
        var package = new UserContestPackage
        {
            PackageGameTypeTimeId = packageId,
            //AppUserId = Guid.Parse(registerData!.UserId),
            HoursAvailable = 12,
            ContestId = contestId,
            LevelId = levelId,
        };

        var json = JsonConvert.SerializeObject(package);
        var content = new StringContent(json, Encoding.UTF8, "application/json");

        msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/UserContestPackages")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", registerData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        Assert.Equal("Created", response.ReasonPhrase);

        var userContestPackageData =
            JsonSerializer.Deserialize<UserContestPackage>(await response.Content.ReadAsStringAsync(),
                JsonHelper.CamelCase);


        //REGISTER NEW TRAINER
        response =
            await _client.PostAsJsonAsync("api/v1.0/Identity/Account/Register",
                new
                {
                    FirstName = "Trainer",
                    LastName = "Last Name",
                    Age = 20,
                    Gender = 0,
                    Email = "trainer@gmail.com",
                    Password = "Uus!12",
                    ConfirmPassword = "Uus!12"
                });

        contentStr = await response.Content.ReadAsStringAsync();
        response.EnsureSuccessStatusCode();

        var trainerRegisterData = JsonSerializer.Deserialize<JWTResponse>(contentStr, JsonHelper.CamelCase);

        //Join the contest
        package = new UserContestPackage
        {
            PackageGameTypeTimeId = packageId,
            //AppUserId = Guid.Parse(trainerRegisterData!.UserId),
            HoursAvailable = 12,
            ContestId = contestId,
            LevelId = levelId,
        };

        json = JsonConvert.SerializeObject(package);
        content = new StringContent(json, Encoding.UTF8, "application/json");

        msg = new HttpRequestMessage(HttpMethod.Post, "/api/v1.0/UserContestPackages")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", trainerRegisterData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();
        Assert.Equal("Created", response.ReasonPhrase);

        var trainerContestPackageData =
            JsonSerializer.Deserialize<UserContestPackage>(await response.Content.ReadAsStringAsync(),
                JsonHelper.CamelCase);


        //Get Participant Joined Contest Information

        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Contests/user");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", registerData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var contestData =
            JsonSerializer.Deserialize<List<Contest>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        Assert.Equal(1, contestData!.Count);


        //Make user a trainer

        msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/AppUser/{trainerRegisterData!.UserId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);

        response.EnsureSuccessStatusCode();

        var trainerData =
            JsonSerializer.Deserialize<AppUserModel>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        var roleId = trainerData!.RoleSelectList.First(e => e.Name == "Treener").Id;
        var vm = new AppUserEditViewModel
        {
            AppUser = new AppUser
            {
                Id = Guid.Parse(trainerRegisterData!.UserId),
                FirstName = trainerRegisterData!.FirstName,
                LastName = trainerRegisterData!.LastName,
                Age = 10,
                Gender = 0,
                Email = "trainer@gmail.com",
            },
            SelectedRoleId = roleId,
        };

        json = JsonConvert.SerializeObject(vm);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Put, $"/api/v1.0/AppUser/{Guid.Parse(trainerRegisterData.UserId)}")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();

        //Change trainer preferences

        var rolePreference = new RolePreferenceViewModel
        {
            SelectedLevelsList = [[levelId.ToString()]],
            ContestId = contestId.ToString()
        };

        json = JsonConvert.SerializeObject(rolePreference);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Post, $"/api/v1.0/RolePreferences")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", trainerRegisterData.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();


        //Create Games

        //Get Courts
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Courts/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var courtsData =
            JsonSerializer.Deserialize<List<Court>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        //Times
        msg = new HttpRequestMessage(HttpMethod.Get, "/api/v1.0/Times/");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var timesData =
            JsonSerializer.Deserialize<List<Time>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        var startDate = new DateOnly(2024, 5, 20);
        var endDate = new DateOnly(2024, 5, 20);
        var SelectedTimesIds = new List<TimeOfDaysViewModel>();


        // Game stats:
        // 8 People
        // 2 Times
        // 2 court
        // 4 games

        for (int i = 0; i < endDate.Day - startDate.Day + 1; i++)
        {
            var newTime = new TimeOfDaysViewModel
            {
                Date = startDate.AddDays(i),
                SelectedTimesList = timesData.Take(2).Select(e => e.Id).ToList()
            };
            SelectedTimesIds.Add(newTime);
        }

        var games = new CreateGamesData
        {
            SelectedLevelIds = [levelsData.Select(e => e.Id).ToList()],
            SelectedCourtIds = [courtsData.Select(e => e.Id).ToList()],
            SelectedTimesIds = SelectedTimesIds,
            SelectedTrainersIds = [[trainerContestPackageData!.Id]],
            PeoplePerCourtInputs = [4]
        };

        json = JsonConvert.SerializeObject(games);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Post, $"/api/v1.0/Games/{contestId}")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode(); 

        Assert.Equal("Created", response.ReasonPhrase);

        //Check Contest Admin Games
        msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/Games/contestGames/{contestId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var gamesData =
            JsonSerializer.Deserialize<List<Game>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        Assert.Equal(4, gamesData!.Count);


        // Game stats:
        // 8 People
        // 1 Time
        // 2 Courts
        // 2 Games
        /*
        for (int i = 0; i < endDate.Day-startDate.Day + 1; i++)
        {
            var newTime = new TimeOfDaysViewModel
            {
                Date = startDate.AddDays(i),
                SelectedTimesList = timesData.Take(2).Select(e => e.Id).ToList()
            };
            SelectedTimesIds.Add(newTime);
        }

        var games = new CreateGamesData
        {
            SelectedLevelIds = [levelsData.Select(e => e.Id).ToList()],
            SelectedCourtIds = [[courtsData.First().Id]],
            SelectedTimesIds = SelectedTimesIds,
            SelectedTrainersIds = [[trainerContestPackageData!.Id]],
            PeoplePerCourtInputs = [4]
        };

        json = JsonConvert.SerializeObject(games);
        content = new StringContent(json, Encoding.UTF8, "application/json");
        msg = new HttpRequestMessage(HttpMethod.Post, $"/api/v1.0/Games/{contestId}")
        {
            Content = content
        };
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();

        Assert.Equal("Created", response.ReasonPhrase);

        //Check Contest Admin Games
        msg = new HttpRequestMessage(HttpMethod.Get, $"/api/v1.0/Games/contestGames/{contestId}");
        msg.Headers.Authorization = new AuthenticationHeaderValue("Bearer", loginData!.Jwt);
        msg.Headers.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        response = await _client.SendAsync(msg);
        response.EnsureSuccessStatusCode();
        var gamesData = JsonSerializer.Deserialize<List<Game>>(await response.Content.ReadAsStringAsync(), JsonHelper.CamelCase);

        Assert.Equal(2, gamesData!.Count);
     }
    }
*/
}