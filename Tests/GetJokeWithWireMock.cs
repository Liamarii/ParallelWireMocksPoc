using ExampleApi.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using System.Net.Http.Json;
using WireMock.RequestBuilders;
using WireMock.ResponseBuilders;
using WireMock.Server;

namespace Tests;

internal class GetJokeWithWireMock
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
                    .WithBody("{\"setup\":\"Why are pirates called pirates?\",\"punchline\":\"Because they arrr!\"}"));


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
    public async Task GetJokeFromMockedApi()
    {
        var httpClient = _factory.CreateClient();
        var response = await httpClient.GetAsync("/joke");

        response.EnsureSuccessStatusCode();
        var joke = await response.Content.ReadFromJsonAsync<Joke>();
        Assert.That(joke?.Setup, Is.EqualTo("Why are pirates called pirates?"));
    }

    [OneTimeTearDown]
    public void OneTimeTearDown()
    {
        _wireMock.Dispose();
        _factory.Dispose();
    }
}