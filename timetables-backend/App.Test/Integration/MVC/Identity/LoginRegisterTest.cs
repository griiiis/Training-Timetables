using System.Net;
using AngleSharp.Html.Dom;
using Base.Test.Helpers;
using Microsoft.AspNetCore.Mvc.Testing;

namespace App.Test.Integration.MVC.Identity;

[Collection("NonParallel")]
public class RegistrationFlow: IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;


    public RegistrationFlow(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false
        });
    }
   

    [Fact]
    public async Task RegisterUserAsync()
    {
        // ARRANGE
        const string registerUri = "/Identity/Account/Register";
        // ACT
        // this just gets the headers, body can be xxx length and is loaded later
        var getRegisterResponse = await _client.GetAsync(registerUri);
        // ASSERT
        getRegisterResponse.EnsureSuccessStatusCode();


        // ARRANGE
        // get the actual content from response
        var getRegisterContent = await HtmlHelpers.GetDocumentAsync(getRegisterResponse);


        // get the form element from page content
        var formRegister = (IHtmlFormElement) getRegisterContent.QuerySelector("#registerForm")!;
        // set up the form values - username, pwd, etc
        var formRegisterValues = new Dictionary<string, string>
        {
            ["Input_FirstName"] = "FirstName",
            ["Input_LastName"] = "LastName",
            ["Input_Age"] = "20",
            ["Input_Gender"] = "0",
            ["Input_Email"] = "test@test.ee",
            ["Input_Password"] = "Foo.bar1",
            ["Input_ConfirmPassword"] = "Foo.bar1",
        };

        // ACT
        // send form with data to server, method (POST) is detected from form element
        var postRegisterResponse = await _client.SendAsync(formRegister, formRegisterValues);

        // ASSERT
        // found - 302 - ie user was created and we should redirect
        // https://en.wikipedia.org/wiki/HTTP_302
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);
        
        
        //Arrange
        const string protectedUri = "/";
        //ACT
        var getResponse = await _client.GetAsync(protectedUri);
        //Assert
        getResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.Found, postRegisterResponse.StatusCode);
    }
    
    [Fact]
    public async Task LoginUserAsync()
    {
        // ARRANGE
        const string loginUri = "/Identity/Account/Login";
        // ACT
        // this just gets the headers, body can be xxx length and is loaded later
        var getLoginResponse = await _client.GetAsync(loginUri);
        // ASSERT
        getLoginResponse.EnsureSuccessStatusCode();


        // ARRANGE
        // get the actual content from response
        var getLoginContent = await HtmlHelpers.GetDocumentAsync(getLoginResponse);


        // get the form element from page content
        var formLogin = (IHtmlFormElement) getLoginContent.QuerySelector("#loginForm")!;
        // set up the form values - username, pwd, etc
        var formLoginValues = new Dictionary<string, string>
        {
            ["Input_Email"] = "uus@gmail.com",
            ["Input_Password"] = "Uus!12",
        };

        // ACT
        // send form with data to server, method (POST) is detected from form element
        var postLoginResponse = await _client.SendAsync(formLogin, formLoginValues);

        // ASSERT
        // found - 302 - ie user was created and we should redirect
        // https://en.wikipedia.org/wiki/HTTP_302
        Assert.Equal(HttpStatusCode.OK, postLoginResponse.StatusCode);
        
        
        //Arrange
        const string protectedUri = "/";
        //ACT
        var getResponse = await _client.GetAsync(protectedUri);
        //Assert
        getResponse.EnsureSuccessStatusCode();
        Assert.Equal(HttpStatusCode.OK, postLoginResponse.StatusCode);
    }
    
    
    
    
    

    
    [Fact]
    public async Task LoadProtectedPageRedirects()
    {
        const string protectedUri = "/Contests";
        
        var getResponse = await _client.GetAsync(protectedUri);

        Assert.Equal(HttpStatusCode.OK, getResponse.StatusCode);
    }



}