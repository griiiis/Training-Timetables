using System.Net;
using System.Text.Json;
using AngleSharp.Html.Dom;
using App.BLL.DTO.Enums;
using App.BLL.DTO.Identity;
using App.DTO.v1_0;
using Base.Test.Helpers;
using Helpers;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;

namespace App.Test.Integration.MVC;

[Collection("NonParallel")]
public class HappyPathTest : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public HappyPathTest(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }


    [Fact]
    public async Task HappyPath()
    {
        //Login as Contest Admin
        string loginUri = "/Identity/Account/Login";

        var getLoginResponse = await _client.GetAsync(loginUri);
        getLoginResponse.EnsureSuccessStatusCode();

        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);
        var formLogin = (IHtmlFormElement)getLoginContent.QuerySelector("#loginForm")!;
        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin@eesti.ee",
            ["Input_Password"] = "Mina!1",
        };
        var postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);

        //Get contest Id
        string contestUri = "/ContestAdmin/Contests";
        var getContestResponse = await _client.GetAsync(contestUri);
        getContestResponse.EnsureSuccessStatusCode();

        var getContestContent = await HtmlHelpers.GetDocumentAsync(getContestResponse);
        var overViewButton = (IHtmlAnchorElement)getContestContent.QuerySelector("#OverView")!;
        var contestId = overViewButton.Href.Split("contestId=")[1];


        //REGISTER NEW USER
        const string registerUri = "/Identity/Account/Register";
        var getRegisterResponse = await _client.GetAsync(registerUri);
        getRegisterResponse.EnsureSuccessStatusCode();

        var getRegisterContent = await HtmlHelpers.GetDocumentAsync(getRegisterResponse);
        var formRegister = (IHtmlFormElement)getRegisterContent.QuerySelector("#registerForm")!;
        var formRegisterValues = new Dictionary<string, string>
        {
            ["Input_FirstName"] = "First Name",
            ["Input_LastName"] = "Last Name",
            ["Input_Age"] = "20",
            ["Input_Gender"] = "0",
            ["Input_Email"] = "new1@gmail.com",
            ["Input_Password"] = "Uus!12",
            ["Input_ConfirmPassword"] = "Uus!12",
        };
        var postRegisterResponse = await _client.SendAsync(formRegister, formRegisterValues);
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);

        //Join the Contest
        string contestJoinUri = "/UserContestPackages/Create?contestId=" + contestId;
        var getJoinTheContestResponse = await _client.GetAsync(contestJoinUri);
        getJoinTheContestResponse.EnsureSuccessStatusCode();

        var getJoinTheContestContent = await HtmlHelpers.GetDocumentAsync(getJoinTheContestResponse);
        var joinRegister = (IHtmlFormElement)getJoinTheContestContent.QuerySelector("#joinForm")!;
        var packageId =
            ((IHtmlSelectElement)getJoinTheContestContent.QuerySelector("#UserContestPackage_PackageGameTypeTimeId")!)
            .Children[0].GetAttribute("value");
        var levelId = ((IHtmlSelectElement)getJoinTheContestContent.QuerySelector("#UserContestPackage_LevelId")!).Children[0].GetAttribute("value");

        var formContestValues = new Dictionary<string, string>
        {
            ["PackageGameTypeTimeId"] = packageId!,
            ["HoursAvailable"] = "20",
            ["ContestId"] = contestId,
            ["LevelId"] = levelId!
        };
        
        var postJoinContestResponse = await _client.SendAsync(joinRegister, formContestValues);
        Assert.Equal(HttpStatusCode.Found, postJoinContestResponse.StatusCode);
        
        //Get Participant Joined Contest Information
        
        const string mycontestsUri = "/Contests/MyContests";
        var getMyContestsResponse = await _client.GetAsync(mycontestsUri);
        getMyContestsResponse.EnsureSuccessStatusCode();

        var getMyContestsContent = await HtmlHelpers.GetDocumentAsync(getMyContestsResponse);
        var addTeammatesButton = (IHtmlAnchorElement)getMyContestsContent.QuerySelector("#addTeammates")!;

        Assert.NotNull(addTeammatesButton);
        
        //REGISTER NEW USER
        getRegisterResponse = await _client.GetAsync(registerUri);
        getRegisterResponse.EnsureSuccessStatusCode();

        getRegisterContent = await HtmlHelpers.GetDocumentAsync(getRegisterResponse);
        formRegister = (IHtmlFormElement)getRegisterContent.QuerySelector("#registerForm")!;
        formRegisterValues = new Dictionary<string, string>
        {
            ["Input_FirstName"] = "Trainer",
            ["Input_LastName"] = "Last Name",
            ["Input_Age"] = "20",
            ["Input_Gender"] = "0",
            ["Input_Email"] = "trainer@gmail.com",
            ["Input_Password"] = "Uus!12",
            ["Input_ConfirmPassword"] = "Uus!12",
        };
        postRegisterResponse = await _client.SendAsync(formRegister, formRegisterValues);
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);
        
        //Make new user as a trainer
        
        //Login as Contest Admin
        string login = "/Identity/Account/Login";

        getLoginResponse = await _client.GetAsync(loginUri);
        getLoginResponse.EnsureSuccessStatusCode();

        getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);
        formLogin = (IHtmlFormElement)getLoginContent.QuerySelector("#loginForm")!;
        formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin@eesti.ee",
            ["Input_Password"] = "Mina!1",
        };
        postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        string participantsUri = "ContestAdmin/AppUser?contestId=" + contestId;
        var getParticipantsResponse = await _client.GetAsync(participantsUri);
        getParticipantsResponse.EnsureSuccessStatusCode();
        
        var getParticipantsContent = await HtmlHelpers.GetDocumentAsync(getParticipantsResponse);
        var editButton = (IHtmlAnchorElement)getParticipantsContent.QuerySelector("#EditButton")!;
        var userId = editButton.Href.Split("Edit/")[1];

        var newParticipantsUri = "ContestAdmin/AppUser/Edit/" + userId;
        var getEditParticipantsResponse = await _client.GetAsync(newParticipantsUri);
        getEditParticipantsResponse.EnsureSuccessStatusCode();
        
        var getEditParticipantsContent = await HtmlHelpers.GetDocumentAsync(getEditParticipantsResponse);
        var editForm = (IHtmlFormElement)getEditParticipantsContent.QuerySelector("#EditParticipant")!;
        var roleId = ((IHtmlSelectElement)editForm.QuerySelector("#SelectedRoleId")!).Children[1].GetAttribute("value");

        var appUser = new AppUser
        {
            Id = Guid.Parse(userId),
            FirstName = "Trainer",
            LastName = "Last Name",
            Age = 10,
            Gender = 0,
            Email = "trainer@gmail.com",
        };
        var editParticipantFormValues = new Dictionary<string, string>
        {
            ["AppUser"] = JsonConvert.SerializeObject(appUser),
            ["SelectedRoleId"] = roleId!
        };
        
        var editParticipantResponse = await _client.SendAsync(editForm, editParticipantFormValues);
        Assert.Equal(HttpStatusCode.Found, editParticipantResponse.StatusCode);
        
        
        //Login as Trainer
        loginUri = "/Identity/Account/Login";

        getLoginResponse = await _client.GetAsync(loginUri);
        getLoginResponse.EnsureSuccessStatusCode();

        getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);
        formLogin = (IHtmlFormElement)getLoginContent.QuerySelector("#loginForm")!;
        formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "trainer@gmail.com",
            ["Input_Password"] = "Uus!12",
        };
        postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        Assert.Equal(HttpStatusCode.Found, postLoginResponse.StatusCode);
        
        
        //Change trainer preferences

        string preferencesUri = "RolePreferences/Create?contestId=" + contestId;
        var getRolePreferencesResponse = await _client.GetAsync(preferencesUri);
        getRolePreferencesResponse.EnsureSuccessStatusCode();

        var getRolePreferencesContent = await HtmlHelpers.GetDocumentAsync(getRolePreferencesResponse);
        var rolePreferencesForm = (IHtmlFormElement)getRolePreferencesContent.QuerySelector("#rolePreferences")!;
        
        var levelId1 =
            ((IHtmlSelectElement)getRolePreferencesContent.QuerySelector("#SelectedLevelsList_0_")!)
            .Children[1].GetAttribute("value");

        var levels = new List<List<string>>(){new List<string>(){levelId1}};
        
        var json = JsonConvert.SerializeObject(levels);
        
        var rolePreferenceFormValues = new Dictionary<string, string>
        {
            ["SelectedLevelsList[0]"] = json,
            ["ContestId"] = contestId
        };
        


        
        //Login as Contest Admin
        login = "/Identity/Account/Login";

        getLoginResponse = await _client.GetAsync(login);
        getLoginResponse.EnsureSuccessStatusCode();

        getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);
        formLogin = (IHtmlFormElement)getLoginContent.QuerySelector("#loginForm")!;
        formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "admin@eesti.ee",
            ["Input_Password"] = "Mina!1",
        };
        postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);
        
        //Create Games
        // Game stats:
        // 8 People
        // 2 Times
        // 2 court
        // 4 games
        string gamesCreateUri = "ContestAdmin/Games/Create?contestId=" + contestId;
        var getGamesResponse = await _client.GetAsync(gamesCreateUri);
        getGamesResponse.EnsureSuccessStatusCode();
        
        var getGamesContent = await HtmlHelpers.GetDocumentAsync(getGamesResponse);
        var formGames = (IHtmlFormElement)getGamesContent.QuerySelector("#gameForm")!;
        
    }
}