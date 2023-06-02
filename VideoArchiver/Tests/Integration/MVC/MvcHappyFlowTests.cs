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
    public async Task Test()
    {
        var response = await _client.GetAsync("/");
        response.EnsureSuccessStatusCode();
    }
}