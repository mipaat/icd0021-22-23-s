using System.Net;
using System.Web;
using AngleSharp.Html.Dom;
using Base.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Helpers;

namespace Tests.Integration.MVC;

[Collection("IntegrationSequential")]
public class MvcHappyFlowTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MvcHappyFlowTests(CustomWebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    [Fact]
    public async Task TestRegisterApproveSubmitView()
    {
        const string testUserName = "testuser";
        const string testUserPassword = "testpassword";
        await RegisterApproveAndAddAdminTestUser(testUserName, testUserPassword);
        await LoginAddVideoSearchVideoWatchVideoAsAdminUser(testUserName, testUserPassword);
    }

    private async Task LoginAddVideoSearchVideoWatchVideoAsAdminUser(string userName, string password)
    {
        var loginResponse = await LogIn(userName, password);
        Assert.Equal(HttpStatusCode.Found, loginResponse.StatusCode);
        var loginRedirectResponse = await _client.GetAsync(loginResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.Found, loginRedirectResponse.StatusCode);
        var selectAuthorRedirectResponse = await _client.GetAsync(loginRedirectResponse.Headers.Location);
        selectAuthorRedirectResponse.EnsureSuccessStatusCode();

        var homePageDocument = await selectAuthorRedirectResponse.GetDocumentAsync();
        var linkSubmitForm = homePageDocument.QuerySelector("#link-submit-form") as IHtmlFormElement
                             ?? throw new Exception("Link submission form not found");
        var linkSubmitInput = linkSubmitForm
                                  .QuerySelectorAll("input")
                                  .FirstOrDefault(e => e is IHtmlInputElement { Name: "Link" }) as IHtmlInputElement
                              ?? throw new Exception("Link submission input not found");
        linkSubmitInput.Value = "https://www.youtube.com/watch?v=ojxw4K_IWz8";
        var linkSubmitResponse = await SubmitForm(linkSubmitForm);
        Assert.Equal(HttpStatusCode.Found, linkSubmitResponse.StatusCode);
        var linkSubmitRedirectResponse = await _client.GetAsync(linkSubmitResponse.Headers.Location);
        linkSubmitRedirectResponse.EnsureSuccessStatusCode();
        var linkSubmitRedirectDocument = await linkSubmitRedirectResponse.GetDocumentAsync();
        Assert.Contains(linkSubmitRedirectDocument.QuerySelectorAll("*")
, e => e.TextContent.ToLower().Contains("successfully processed"));

        var videoSearchResponse = await _client.GetAsync("/Video/Search/");
        var videoSearchDocument = await videoSearchResponse.GetDocumentAsync();
        var videoSearchForm = videoSearchDocument.QuerySelector("#video-search-form") as IHtmlFormElement
                              ?? throw new Exception("Video search form not found");
        var nameQueryInput = videoSearchForm.QuerySelectorAll("input")
                                 .FirstOrDefault(e => e is IHtmlInputElement { Name: "NameQuery" }) as IHtmlInputElement
                             ?? throw new Exception("Video search name query form element not found");
        const string videoTitle = "happy llama sad llama";
        nameQueryInput.Value = videoTitle;
        var videoSearchWithQueryResponse = await SubmitForm(videoSearchForm, useQueryString: true);
        videoSearchWithQueryResponse.EnsureSuccessStatusCode();

        var videoSearchWithQueryDocument = await videoSearchResponse.GetDocumentAsync();
        var videoWatchLink = videoSearchWithQueryDocument.QuerySelectorAll("a")
            .FirstOrDefault(e => e.TextContent == videoTitle) as IHtmlAnchorElement
            ?? throw new Exception("Video watch link not found");
        var videoWatchPageResponse = await _client.GetAsync(videoWatchLink.Href);
        videoWatchPageResponse.EnsureSuccessStatusCode();
        var videoWatchPageDocument = await videoWatchPageResponse.GetDocumentAsync();
        Assert.Contains(videoWatchPageDocument.QuerySelectorAll("*"), e => e.TextContent == videoTitle);
        Assert.NotNull(videoWatchPageDocument.QuerySelector("video"));

        await LogOut(videoWatchPageDocument);
    }

    private async Task RegisterApproveAndAddAdminTestUser(string testUserName, string testUserPassword)
    {
        var homePageResponse = await _client.GetAsync("/");
        homePageResponse.EnsureSuccessStatusCode();
        var homePageContent = await homePageResponse.GetDocumentAsync();
        var registerLink = homePageContent.QuerySelectorAll("a")
                               .FirstOrDefault(e => e.TextContent.ToLower().Contains("register")) as IHtmlAnchorElement
                           ?? throw new Exception("Register link not found");
        var goToRegisterPageResponse = await _client.GetAsync(registerLink.Href);
        goToRegisterPageResponse.EnsureSuccessStatusCode();
        var registerResponse = await Register(testUserName, testUserPassword, goToRegisterPageResponse);
        Assert.Equal(HttpStatusCode.Found, registerResponse.StatusCode);
        var registerRedirectResponse = await _client.GetAsync(registerResponse.Headers.Location);
        registerRedirectResponse.EnsureSuccessStatusCode();
        Assert.Contains((await registerRedirectResponse.GetDocumentAsync())
            .QuerySelectorAll(".text-danger"),
            e => e.TextContent.ToLower().Contains("must be approved"));

        var adminLoginResponse = await LogIn("root", "root123");
        Assert.Equal(HttpStatusCode.Found, adminLoginResponse.StatusCode);
        var adminLoginRedirectResponse = await _client.GetAsync(adminLoginResponse.Headers.Location);
        Assert.Equal(HttpStatusCode.Found, adminLoginRedirectResponse.StatusCode);
        var adminLoginSelectAuthorResponse = await _client.GetAsync(adminLoginRedirectResponse.Headers.Location);
        adminLoginSelectAuthorResponse.EnsureSuccessStatusCode();
        var adminLoginRedirectDocument = await adminLoginSelectAuthorResponse.GetDocumentAsync();
        var adminDropdownButton = adminLoginRedirectDocument.QuerySelector("#admin-dropdown");
        Assert.NotNull(adminDropdownButton);

        var userManagementIndexResponse = await _client.GetAsync($"/Admin/UserManagement/Index/");
        userManagementIndexResponse.EnsureSuccessStatusCode();
        var userManagementIndexDocument = await userManagementIndexResponse.GetDocumentAsync();
        var userManagementDiv = userManagementIndexDocument.QuerySelector($"#manage-{testUserName}")
                                ?? throw new Exception("User management div not found");
        var userApprovalForm = userManagementDiv.QuerySelector($"#approve-{testUserName}") as IHtmlFormElement
                               ?? throw new Exception("User approval form not found");
        var approveUserResponse = await SubmitForm(userApprovalForm);
        Assert.Equal(HttpStatusCode.Found, approveUserResponse.StatusCode);
        var approveUserRedirectResponse = await _client.GetAsync(approveUserResponse.Headers.Location);
        approveUserRedirectResponse.EnsureSuccessStatusCode();

        userManagementIndexDocument = await approveUserRedirectResponse.GetDocumentAsync();
        userManagementDiv = userManagementIndexDocument.QuerySelector($"#manage-{testUserName}")
                            ?? throw new Exception("User management div not found");
        Assert.Null(userManagementDiv.QuerySelector($"#approve-{testUserName}"));
        var manageRolesLink = userManagementDiv
                                      .QuerySelectorAll("a")
                                      .FirstOrDefault(e => e.TextContent.ToLower().Contains("roles"))
                                  as IHtmlAnchorElement
                              ?? throw new Exception("Role management link not found");

        var manageRolesGetResponse = await _client.GetAsync(manageRolesLink.Href);
        manageRolesGetResponse.EnsureSuccessStatusCode();
        var manageRolesDocument = await manageRolesGetResponse.GetDocumentAsync();
        var addAdminForm = manageRolesDocument.QuerySelector("#add-Admin-form") as IHtmlFormElement
                           ?? throw new Exception("Add admin role form not found");
        var addAdminRoleResponse = await SubmitForm(addAdminForm);
        Assert.Equal(HttpStatusCode.Found, addAdminRoleResponse.StatusCode);
        var addAdminRoleRedirect = await _client.GetAsync(addAdminRoleResponse.Headers.Location);
        addAdminRoleRedirect.EnsureSuccessStatusCode();
        var logOutResponse = await LogOut(await addAdminRoleRedirect.GetDocumentAsync());
        logOutResponse.EnsureSuccessStatusCode();
    }

    private async Task<HttpResponseMessage> LogIn(string username, string password)
    {
        var loginGetResponse = await _client.GetAsync("/Identity/Account/Login");
        loginGetResponse.EnsureSuccessStatusCode();
        var loginGetDocument = await loginGetResponse.GetDocumentAsync();
        var loginForm = loginGetDocument.QuerySelector("#account") as IHtmlFormElement
                        ?? throw new Exception("Login form not found");
        var loginData = new Dictionary<string, string>
        {
            ["Input.UserName"] = username,
            ["Input.Password"] = password,
        };
        return await SubmitForm(loginForm, loginData);
    }

    private async Task<HttpResponseMessage> LogOut(IHtmlDocument document)
    {
        var logoutForm = document.QuerySelector("#logout-form") as IHtmlFormElement
                         ?? throw new Exception("Logout form not found");
        var logOutResponse = await SubmitForm(logoutForm);
        Assert.Equal(HttpStatusCode.Found, logOutResponse.StatusCode);
        return await _client.GetAsync(logOutResponse.Headers.Location);
    }

    private async Task<HttpResponseMessage> Register(string userName, string password,
        HttpResponseMessage? registerPageResponse = null)
    {
        registerPageResponse ??= await _client.GetAsync("/Identity/Account/Register");
        registerPageResponse.EnsureSuccessStatusCode();
        var registerPageContent = await registerPageResponse.GetDocumentAsync();
        var registerForm = registerPageContent.QuerySelector("#registerForm") as IHtmlFormElement
                           ?? throw new Exception("Register form not found");
        var registerFormValues = new Dictionary<string, string>
        {
            ["Input.UserName"] = userName,
            ["Input.Password"] = password,
            ["Input.ConfirmPassword"] = password,
        };
        return await SubmitForm(registerForm, registerFormValues);
    }

    private async Task<HttpResponseMessage> SubmitForm(IHtmlFormElement formElement,
        IDictionary<string, string>? data = null, string? verificationToken = null, bool? useQueryString = null)
    {
        data ??= new Dictionary<string, string>();
        foreach (var formControl in formElement.Elements)
        {
            if (formControl is IHtmlInputElement inputElement && !string.IsNullOrEmpty(inputElement.Name) &&
                !data.Keys.Any(k => string.Equals(k, inputElement.Name, StringComparison.OrdinalIgnoreCase)))
            {
                var value = HttpUtility.UrlDecode(inputElement.Value ?? string.Empty);
                data[inputElement.Name] = value;
            }
            else if (formControl is IHtmlTextAreaElement textAreaElement &&
                     !string.IsNullOrEmpty(textAreaElement.Name) &&
                     !data.Keys.Any(k => string.Equals(k, textAreaElement.Name, StringComparison.OrdinalIgnoreCase)))
            {
                var value = HttpUtility.UrlDecode(textAreaElement.Value ?? string.Empty);
                data[textAreaElement.Name] = value;
            }
        }

        verificationToken ??= (formElement["__RequestVerificationToken"] as IHtmlInputElement)?.Value;
        if (verificationToken != null)
        {
            data["__RequestVerificationToken"] = verificationToken;
        }

        var request = new HttpRequestMessage(new HttpMethod(formElement.Method), formElement.Action);

        useQueryString ??= formElement.Method.ToLower() == "get";
        if (useQueryString.Value)
        {
            var uriBuilder = new UriBuilder(request.RequestUri ?? throw new Exception("Request URI was null"));
            var queryString = string.Join("&", data.Select(kvp => $"{Uri.EscapeDataString(kvp.Key)}={Uri.EscapeDataString(kvp.Value)}"));

            if (!string.IsNullOrEmpty(uriBuilder.Query))
            {
                queryString = uriBuilder.Query + "&" + queryString;
            }

            uriBuilder.Query = queryString;
            request.RequestUri = uriBuilder.Uri;
        }
        else
        {
            request.Content = new FormUrlEncodedContent(data);
        }

        return await _client.SendAsync(request);
    }
}