using System.Net;
using System.Web;
using AngleSharp.Html.Dom;
using Base.Tests;
using Microsoft.AspNetCore.Mvc.Testing;
using Tests.Helpers;

namespace Tests.Integration.MVC;

public class MvcHappyFlowTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly CustomWebApplicationFactory<Program> _factory;

    public MvcHappyFlowTests(CustomWebApplicationFactory<Program> factory)
    {
        _factory = factory;
        _client = factory.CreateClient(new WebApplicationFactoryClientOptions
        {
            AllowAutoRedirect = false,
        });
    }

    [Fact]
    public async Task TestRegisterApproveSubmitView()
    {
        var homePageResponse = await _client.GetAsync("/");
        homePageResponse.EnsureSuccessStatusCode();
        var homePageContent = await homePageResponse.GetDocumentAsync();
        var registerLink = homePageContent.QuerySelectorAll("a")
                               .FirstOrDefault(e => e.TextContent.ToLower().Contains("register")) as IHtmlAnchorElement
                           ?? throw new Exception("Register link not found");
        var goToRegisterPageResponse = await _client.GetAsync(registerLink.Href);
        goToRegisterPageResponse.EnsureSuccessStatusCode();
        var registerResponse = await Register("testuser", "testpassword", goToRegisterPageResponse);
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
        var userManagementDiv = userManagementIndexDocument.QuerySelector("#manage-testuser")
                                ?? throw new Exception("User management div not found");
        var userApprovalForm = userManagementDiv.QuerySelector("#approve-testuser") as IHtmlFormElement
            ?? throw new Exception("User approval form not found");
        var approveUserResponse = await SubmitForm(userApprovalForm);
        Assert.Equal(HttpStatusCode.Found, approveUserResponse.StatusCode);
        var approveUserRedirectResponse = await _client.GetAsync(approveUserResponse.Headers.Location);
        approveUserRedirectResponse.EnsureSuccessStatusCode();

        userManagementIndexDocument = await approveUserRedirectResponse.GetDocumentAsync();
        userManagementDiv = userManagementIndexDocument.QuerySelector("#manage-testuser")
                            ?? throw new Exception("User management div not found");
        Assert.Null(userManagementDiv.QuerySelector("#approve-testuser"));
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
        IDictionary<string, string>? data = null, string? verificationToken = null)
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
            else if (formControl is IHtmlTextAreaElement textAreaElement && !string.IsNullOrEmpty(textAreaElement.Name) &&
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
        request.Content = new FormUrlEncodedContent(data);
        return await _client.SendAsync(request);
    }
}