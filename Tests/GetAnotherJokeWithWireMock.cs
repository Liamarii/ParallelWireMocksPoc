using ExampleApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests;

internal class GetAnotherJokeWithWireMock
{
    private WebApplicationFactory<Program> _factory = null!;
    private WireMockServer _wireMock;

    [OneTimeSetUp]
    public void OneTimeSetup()
    {
        _wireMock = WireMockServer.Start();
        _wireMock.Given(Request.Create()
                .WithPath("/random_joke")
                .UsingGet())
                .RespondWith(Response.Create()
                    .WithStatusCode(200)
                    .WithBody("{\"setup\":\"How do you organize a space party?\",\"punchline\":\"Planet!\"}"));


        _factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureAppConfiguration((context, config) =>
                {
                    var overrides = new Dictionary<string, string?>
                    {
                        ["JokesApiUrl"] = _wireMock.Urls.Single()
                    };
                    config.AddInMemoryCollection(overrides);
                });
            });
    }

    [Test]
    public async Task GetAnotherJokeFromMockedApi()
    {
        var httpClient = _factory.CreateClient();
        var response = await httpClient.GetAsync("/joke");

        response.EnsureSuccessStatusCode();
        var joke = await response.Content.ReadFromJsonAsync<Joke>();
        Assert.That(joke?.Setup, Is.EqualTo("How do you organize a space party?"));
        Assert.That(joke?.Punchline, Is.EqualTo("Planet!"));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _wireMock.Dispose();
        _factory.Dispose();
    }
}