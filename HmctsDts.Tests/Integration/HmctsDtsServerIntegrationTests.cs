using System.Net;
using Microsoft.AspNetCore.Mvc.Testing;

namespace HmctsDts.Tests.Integration;

public class HmctsDtsServerIntegrationTests
{
    private WebApplicationFactory<Server.Program> _factory;
    private HttpClient _client;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _factory = new WebApplicationFactory<Server.Program>();
        _client = _factory.CreateClient();
    }
    
    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _client.Dispose();
        _factory.Dispose();
    }

    /// <summary>
    /// Verifies that server successfully starts and the status endpoint returns an OK (200) response
    /// </summary>
    [Test]
    [Category("Integration")]
    [Category("API")]
    public async Task Status_Returns_Okay()
    {
        // Arrange
        await using var application = new WebApplicationFactory<Server.Program>();
        using var client = application.CreateClient();
        
        // Act
        var response = await client.GetAsync("/status");
        
        // Assert
        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
    }
}