using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using App.Common;
using Microsoft.AspNetCore.Mvc.Testing;
using Public.DTO.v1.Identity;
using Tests.Helpers;

namespace Tests.Integration.API;

public class ApiHappyFlowTests : IClassFixture<CustomWebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private const string ApiBase = "/api/v1.0";

    private static readonly JsonSerializerOptions CamelCaseJsonSerializerOptions = new()
    {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase
    };

    public ApiHappyFlowTests(CustomWebApplicationFactory<Program> factory)
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
        const string testPassword = "testpassword";

        var registerResponse = await RegisterAsync(testUserName, testPassword);
        Assert.Equal(HttpStatusCode.Accepted, registerResponse.StatusCode);
        await Assert.ThrowsAnyAsync<Exception>(async () =>
            VerifyJwtContent(await registerResponse.Content.ReadAsStringAsync()));

        await LoginAsRootAndApproveAndMakeAdminUser(testUserName);
    }

    private async Task LoginAsRootAndApproveAndMakeAdminUser(string username)
    {
        var rootLoginResponse = await LoginAsync("root", "root123");
        var rootLoginJwtResponse = await VerifyJwtContent(rootLoginResponse, "root");
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", rootLoginJwtResponse.Jwt);

        var listUsersResponse =
            await _client.GetAsync(
                $"{ApiBase}/admin/ManageUsers/ListAll?includeOnlyNotApproved=true&nameQuery={Uri.EscapeDataString(username)}");
        listUsersResponse.EnsureSuccessStatusCode();
        var listUsersResult = await listUsersResponse.Content.ReadFromJsonAsync<List<UserWithRoles>>()
            ?? throw new Exception("Failed to deserialize users list");
        var user = listUsersResult.FirstOrDefault(u =>
            string.Equals(u.UserName, username, StringComparison.OrdinalIgnoreCase))
            ?? throw new Exception("Expected user not found in users list");
        Assert.Empty(user.Roles);
        Assert.False(user.IsApproved);

        var approveUserResponse = await _client.PutAsJsonAsync($"{ApiBase}/admin/ManageUsers/ApproveRegistration?id={user.Id}", new { });
        approveUserResponse.EnsureSuccessStatusCode();

        var listRolesResponse = await _client.GetAsync($"{ApiBase}/admin/ManageUsers/ListAllRoleNames");
        listRolesResponse.EnsureSuccessStatusCode();
        var fetchedRoleNames = await listRolesResponse.Content.ReadFromJsonAsync<List<string>>();
        Assert.NotNull(fetchedRoleNames);
        foreach (var roleName in RoleNames.AllAsList)
        {
            Assert.Contains(roleName, fetchedRoleNames);
        }
        Assert.Equal(RoleNames.AllAsList.Length, fetchedRoleNames.Count);

        var addToRoleResponse = await _client.PostAsJsonAsync(
            $"{ApiBase}/admin/ManageUsers/AddRole?userId={Uri.EscapeDataString(user.Id.ToString())}&roleName={Uri.EscapeDataString(RoleNames.Admin)}", new { });
        addToRoleResponse.EnsureSuccessStatusCode();

        await LogoutAsync(rootLoginJwtResponse);
    }

    private async Task LogoutAsync(JwtResponse jwtResponse)
    {
        var response = await _client.PostAsJsonAsync($"{ApiBase}/identity/Account/LogOut",
            new { jwtResponse.Jwt, jwtResponse.RefreshToken });
        response.EnsureSuccessStatusCode();
        _client.DefaultRequestHeaders.Authorization = null;
    }
    
    private async Task<HttpResponseMessage> LoginAsync(string userName, string password, int expiresInSeconds = 60)
    {
        return await _client.PostAsJsonAsync(
            $"{ApiBase}/Identity/Account/LogIn?expiresInSeconds={expiresInSeconds}",
            new { Username = userName, Password = password }
        );
    }

    private async Task<HttpResponseMessage> RegisterAsync(string userName, string password, int expiresInSeconds = 60)
    {
        return await _client.PostAsJsonAsync(
            $"{ApiBase}/Identity/Account/Register?expiresInSeconds={expiresInSeconds}",
            new { Username = userName, Password = password });
    }

    private async Task<HttpResponseMessage> RefreshToken(JwtResponse jwtResponse, int expiresInSeconds = 60)
    {
        return await _client.PostAsJsonAsync(
            $"{ApiBase}/Identity/Account/Register?expiresInSeconds={expiresInSeconds}",
            new { jwtResponse.Jwt, jwtResponse.RefreshToken, }
        );
    }

    private async Task<JwtResponse> VerifyJwtContent(HttpResponseMessage jwtResponseMessage, string? username = null,
        DateTime? validToIsSmallerThan = null)
    {
        jwtResponseMessage.EnsureSuccessStatusCode();
        return VerifyJwtContent(await jwtResponseMessage.Content.ReadAsStringAsync(), username, validToIsSmallerThan);
    }
    
    private JwtResponse VerifyJwtContent(string jwtResponseContent, string? username = null,
        DateTime? validToIsSmallerThan = null)
    {
        var jwtResponse = JsonSerializer.Deserialize<JwtResponse>(jwtResponseContent, CamelCaseJsonSerializerOptions);

        Assert.NotNull(jwtResponse);
        Assert.NotNull(jwtResponse.RefreshToken);
        Assert.NotNull(jwtResponse.Jwt);

        var jwtToken = new JwtSecurityTokenHandler().ReadJwtToken(jwtResponse.Jwt);
        if (username != null)
        {
            Assert.Equal(username, jwtToken.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value);
        }

        if (validToIsSmallerThan != null)
        {
            Assert.True(jwtToken.ValidTo < validToIsSmallerThan);
        }

        return jwtResponse;
    }
}